<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx4.master"
    AutoEventWireup="true" CodeBehind="ItemDistribution.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemDistribution" %>

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
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
                $('#lblQuickPick').show();
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
            }

            setDatePicker('<%=txtItemTransactionDate.ClientID %>');
            $('#<%=txtItemTransactionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region Distribution No
            $('#lblDistributionNo.lblLink').click(function () {
                openSearchDialog('itemdistributionhd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtDistributionNo.ClientID %>').val(value);
                    onTxtDistributionNoChanged(value);
                });
            });

            $('#<%=txtDistributionNo.ClientID %>').change(function () {
                onTxtDistributionNoChanged($(this).val());
            });

            function onTxtDistributionNoChanged(value) {
                onLoadObject(value);
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
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result2) {
                            $('#<%=hdnLocationItemGroupID.ClientID %>').val(result2.ItemGroupID);
                            $('#<%=hdnGCLocationGroup.ClientID %>').val(result2.GCLocationGroup);
                            $('#<%=hdnDistributionLocationID.ClientID %>').val(result2.DistributionLocationID);
                        });
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnLocationItemGroupID.ClientID %>').val('');
                        $('#<%=hdnDistributionLocationID.ClientID %>').val();
                    }
                });
            }
            //#endregion

            //#region Location To
            function getLocationFilterExpressionTo() {
                var itemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                var distributionLocationID = $('#<%=hdnDistributionLocationID.ClientID %>').val();

                var filterExpression = "<%:filterExpressionLocationTo %>";
                if (itemGroupID != '') {
                    filterExpression += "LocationID IN (SELECT LocationID FROM Location WHERE ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%'))";
                }

                if (distributionLocationID != '') {
                    filterExpression += " LocationID = " + distributionLocationID;
                }

                return filterExpression;

            }

            $('#<%=lblLocationTo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                    $('#<%=txtLocationCodeTo.ClientID %>').val(value);
                    onTxtLocationCodeChangedTo(value);
                });
            });

            $('#<%=txtLocationCodeTo.ClientID %>').live('change', function () {
                onTxtLocationCodeChangedTo($(this).val());
            });

            function onTxtLocationCodeChangedTo(value) {
                var filterExpression = getLocationFilterExpressionTo();
                filterExpression += "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
                        $('#<%=txtLocationNameTo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('.lblExpiredDate').live('click', function () {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {

                    $tr = $(this).closest('tr');
                    var param = $tr.find('.keyField').html();
                    var url = ResolveUrl("~/Program/Warehouse/ItemDistribution/ItemDistributionExpiredDateCtl.ascx");
                    openUserControlPopup(url, param, 'Expired Date', 550, 450);
                }
            });

            $('#lblAddData').click(function (evt) {
                var allowed = $('#<%=hdnIsDistributionAllowedWithoutRequest.ClientID %>').val();
                if (allowed == "1") {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        $('#<%=hdnIsEdit.ClientID %>').val('0');
                        $('#<%=lblLocation.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%=txtLocationCode.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtQuantity.ClientID %>').val('1');
                        $('#<%=hdnEntryID.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtStockFromLocation.ClientID %>').val('');
                        cboItemUnit.SetValue('');
                        $('#<%=txtConversion.ClientID %>').val('');

                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');

                        $('#containerEntry').show();
                    }
                } else {
                    showToast('Failed', 'Proses distribusi hanya diperbolehkan dari permintaan barang.');
                }
            });

            $('#lblQuickPick').click(function () {
                var allowed = $('#<%=hdnIsDistributionAllowedWithoutRequest.ClientID %>').val();
                if (allowed == "1") {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/Warehouse/ItemDistribution/ItemDistributionQuickPicksCtl.ascx');
                        var transactionID = $('#<%=hdnDistributionID.ClientID %>').val();
                        var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                        var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                        var id = transactionID + '|' + locationID + '|' + GCLocationGroup;
                        openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                    }
                } else {
                    showToast('Failed', 'Proses distribusi hanya diperbolehkan dari permintaan barang.');
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

        function onGetItemTypeFromFilterExpression() {
            var filterExpression = "";
            if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == '') {
                filterExpression = "GCItemType IN ('X001^002','X001^003','X001^008','X001^009') AND IsDeleted = 0";
            }
            else if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == 'X227^1') {
                filterExpression = "GCItemType IN ('X001^002','X001^003') AND IsDeleted = 0";
            }
            else {
                filterExpression = "GCItemType IN ('X001^008') AND IsDeleted = 0";
            }
            return filterExpression;
        }

        function onGetItemGroupFilterExpression() {
            var filterExpression = onGetItemTypeFromFilterExpression();
            if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != '0')
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
            return filterExpression;
        }

        //#region Item Group
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

        function getItemFilterExpression() {
            var filterExpression = onGetItemTypeFromFilterExpression();
            var distributionID = $('#<%=hdnDistributionID.ClientID %>').val();
            var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            if ($('#<%=txtItemGroupCode.ClientID %>').val() != '')
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
            if (distributionID != '')
                filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM ItemDistributionDt WHERE DistributionID = " + distributionID + " AND IsDeleted = 0)";
            filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + locationID + ' AND IsDeleted = 0) AND IsDeleted = 0';

            filterExpression += " AND GCItemStatus != 'X181^999'";
            return filterExpression;
        }

        //#region Item
        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('itemdruginfo', getItemFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemCodeChanged($(this).val());
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
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
                    Methods.getItemQtyOnOrder(result.ItemID, $('#<%=hdnLocationIDTo.ClientID %>').val(), 4, function (result3) {
                        if (result3 != null)
                            $('#<%=txtQtyOnProcess.ClientID %>').val(result3.QtyOnOrder + " " + result.ItemUnit);
                        else
                            $('#<%=txtQtyOnProcess.ClientID %>').val("0 " + result.ItemUnit);
                        GetItemQtyFromLocation();
                    });
                    cboItemUnit.PerformCallback();
                }
                else {
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=txtQtyOnProcess.ClientID %>').val('');
                    $('#<%=txtQuantity.ClientID %>').val('');
                    $('#<%=txtStockFromLocation.ClientID %>').val('');
                }
            });
        }
        //#endregion

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
            Methods.getItemQtyOnOrder(entity.ItemID, $('#<%=hdnLocationIDTo.ClientID %>').val(), 4, function (result3) {
                if (result3 != null)
                    $('#<%=txtQtyOnProcess.ClientID %>').val((result3.QtyOnOrder - entity.CustomTotal) + " " + entity.BaseUnit);
                GetItemQtyFromLocation();
            });
            cboItemUnit.PerformCallback();
            $('#containerEntry').show();
        });
        //#endregion

        function GetItemQtyFromLocation() {
            var filterExpression = "LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                $('#<%=txtQuantity.ClientID %>').attr('max', result.QuantityEND);
                $('#<%=txtStockFromLocation.ClientID %>').val(result.QuantityEND + ' ' + result.ItemUnit);
            });
        }

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

        function onAfterSaveRecordDtSuccess(DistributionID) {
            if ($('#<%=hdnDistributionID.ClientID %>').val() == '0') {
                $('#<%=hdnDistributionID.ClientID %>').val(DistributionID);
                var filterExpression = 'DistributionID = ' + DistributionID;
                Methods.getObject('GetItemDistributionHdList', filterExpression, function (result) {
                    $('#<%=txtDistributionNo.ClientID %>').val(result.DistributionNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
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
            var distributionID = $('#<%=hdnDistributionID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (code == 'IM-00048' || code == 'IM-00137' || code == 'IM-00205' || code == 'IM-00213') {
                    if (distributionID == '' || distributionID == '0') {
                        errMessage.text = 'Please Set Transaction First!';
                        return false;
                    }
                    else {
                        filterExpression.text = "DistributionID = " + distributionID;
                        return true;
                    }
                }
                else {
                    if (distributionID == '' || distributionID == '0') {
                        errMessage.text = 'Please Set Transaction First!';
                        return false;
                    } else {
                        filterExpression.text = "DistributionID = " + distributionID;
                        return true;
                    }
                }
            }
            else {
                errMessage.text = "Tidak dapat cetak karena status pemesanan belum diperbolehkan untuk cetak sekarang.";
                return false;
            }
        }  
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnDistributionID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnIsAutoReceived" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="" id="hdnIsDistributionQty" runat="server" />
    <input type="hidden" value="" id="hdnIsDistributionAllowedWithoutRequest" runat="server" />
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
                                <label class="lblLink" id="lblDistributionNo">
                                    <%=GetLabel("No. Distribusi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistributionNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <input type="hidden" id="hdnLocationItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnDistributionLocationID" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroup" value="" runat="server" />
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
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                                            <asp:TextBox ID="txtItemTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemTransactionTime" Width="100px" CssClass="time" runat="server"
                                                Style="text-align: center" />
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
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 145px" />
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
                                <label class="lblNormal" id="lblRegistrationNo" runat="server">
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
                            <%=GetLabel("Edit atau Tambah Distribusi Barang")%></div>
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
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
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
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
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
                                                    <label>
                                                        <%=GetLabel("Stok (Dari Lokasi)")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockFromLocation" ReadOnly="true" CssClass="number" Width="120px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" CssClass="number" Width="120px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Satuan Item")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                        Width="300px" OnCallback="cboItemUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnItemUnitValue" runat="server" />
                                                    <asp:TextBox ID="txtConversion" Width="180px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Qty diproses")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQtyOnProcess" Width="180px" runat="server" Style="text-align: right;"
                                                        ReadOnly="true" />
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
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CustomTotal") %>" bindingfield="CustomTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="350px" />
                                            <asp:BoundField DataField="CustomItemUnit" ItemStyle-HorizontalAlign="Right" HeaderText="Didistribusi"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="BaseUnit" ItemStyle-HorizontalAlign="Center" HeaderText="Satuan Dasar"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="CustomConversion" ItemStyle-HorizontalAlign="Center" HeaderText="Conversion" />
                                            <asp:BoundField DataField="CustomItemDistribution" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Total Didistribusi" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="LineAmountText" HeaderText="Line Amount" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="No.Batch|Exp.Date|Qty" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <label <%# IsEditable() == "1" ? "class='lblExpiredDate lblLink'":"class='lblExpiredDate lblLink lblDisabled'" %>>
                                                        <%=GetLabel("Expired Date")%></label>
                                                    <br />
                                                    <%#:Eval("BatchNoExpiredDate")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblQuickPick">
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
