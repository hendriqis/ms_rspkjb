<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CSSDRequestEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDRequestEntry" %>

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

            setDatePicker('<%=txtRequestDate.ClientID %>');
            $('#<%=txtRequestDate.ClientID %>').datepicker('option', 'maxDate', '0');

            //#region Request No
            $('#lblRequestNo.lblLink').click(function () {
                var filter = "<%=GetFilterExpression() %>";
                openSearchDialog('mdservicerequesthd', filter, function (value) {
                    $('#<%=txtRequestNo.ClientID %>').val(value);
                    ontxtRequestNoChanged(value);
                });
            });

            $('#<%=txtRequestNo.ClientID %>').change(function () {
                ontxtRequestNoChanged($(this).val());
            });

            function ontxtRequestNoChanged(value) {
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
                var LocationToID = $('#<%=hdnLocationIDTo.ClientID %>').val();
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnFromLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                            $('#<%=hdnLocationHealthcareUnitFrom.ClientID %>').val(result.GCHealthcareUnit);
                        });
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnFromLocationItemGroupID.ClientID %>').val('');
                        $('#<%=hdnGCLocationGroupFrom.ClientID %>').val('');
                        $('#<%=hdnLocationHealthcareUnitFrom.ClientID %>').val('');
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
                openSearchDialog('location', getLocationFilterExpressionTo(), function (value) {
                    $('#<%=txtLocationCodeTo.ClientID %>').val(value);
                    onTxtLocationToCodeChanged(value);
                });
            });

            $('#<%=txtLocationCodeTo.ClientID %>').live('change', function () {
                onTxtLocationToCodeChanged($(this).val());
            });

            function onTxtLocationToCodeChanged(value) {
                var filterExpression = "LocationCode = '" + value + "'";
                Methods.getObject('GetvLocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                        $('#<%=hdnToLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=hdnGCLocationGroupTo.ClientID %>').val(result.GCLocationGroup);
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

            //#region CSSD Package
            function getPackageCSSDFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblCSSDPackage.ClientID %>.lblLink').click(function () {
                openSearchDialog('cssdpackagehd', getPackageCSSDFilterExpression(), function (value) {
                    $('#<%=txtPackageCode.ClientID %>').val(value);
                    onTxtPackageCodeChanged(value);
                });
            });

            $('#<%=txtPackageCode.ClientID %>').change(function () {
                onTxtPackageCodeChanged($(this).val());
            });

            function onTxtPackageCodeChanged(value) {
                var filterExpression = "PackageCode = '" + value + "'";
                Methods.getObject('GetCSSDItemPackageHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPackageID.ClientID %>').val(result.PackageID);
                        $('#<%=txtPackageName.ClientID %>').val(result.PackageName);
                    }
                    else {
                        $('#<%=hdnPackageID.ClientID %>').val("");
                        $('#<%=txtPackageCode.ClientID %>').val("");
                        $('#<%=txtPackageName.ClientID %>').val("");
                    }
                });
            }
            //#endregion

            $('#lblPackageEntry').click(function (evt) {
                var packageID = $('#<%=hdnPackageID.ClientID %>').val();
                var packageCode = $('#<%=txtPackageCode.ClientID %>').val();
                var packageName = $('#<%=txtPackageName.ClientID %>').val();
                var packageTitle = packageCode + " - " + packageName;

                if (packageID != "" && packageID != "0") {
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        url = ResolveUrl('~/Program/CSSD/CSSDRequest/CSSDRequestEntryPackageQuickPickCtl.ascx');
                        var requestID = $('#<%=hdnRequestID.ClientID %>').val();
                        var locationIDFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        var locationUnitFrom = $('#<%=hdnLocationHealthcareUnitFrom.ClientID %>').val();
                        var locationIDTo = $('#<%=hdnLocationIDTo.ClientID %>').val();
                        var id = requestID + '|' + locationIDFrom + '|' + locationUnitFrom + '|' + locationIDTo + '|' + packageID;
                        openUserControlPopup(url, id, packageTitle, 1000, 550);
                    }
                }
                else {
                    showToast('INFORMATION', 'Please select package first !');
                }
            });

            $('#lblQuickPick').click(function () {
                var packageID = $('#<%=hdnPackageID.ClientID %>').val();

                if (packageID == "" || packageID == "0") {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        showLoadingPanel();
                        var url = ResolveUrl('~/Program/CSSD/CSSDRequest/CSSDRequestEntryQuickPickCtl.ascx');
                        var requestID = $('#<%=hdnRequestID.ClientID %>').val();
                        var locationIDFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        var locationIDTo = $('#<%=hdnLocationIDTo.ClientID %>').val();
                        var id = requestID + '|' + locationIDFrom + '|' + locationIDTo;
                        openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                    }
                }
                else {
                    showToast('FAILED', 'Jika sudah memilih paket, maka tidak bisa memilih manual item !');
                }
            });

            $('#lblAddData').click(function (evt) {
                var packageID = $('#<%=hdnPackageID.ClientID %>').val();

                if (packageID == "" || packageID == "0") {
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
                        cboItemUnit.SetValue('');
                        $('#<%=txtConversion.ClientID %>').val('');
                        $('#<%=txtBinLocationName.ClientID %>').val('');

                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');

                        $('#containerEntry').show();
                    }
                }
                else {
                    showToast('FAILED', 'Jika sudah memilih paket, maka tidak bisa memilih manual item !');
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
            var filterExpression = "IsDeleted = 0 AND GCItemType IN ('X001^002','X001^003')";
            return filterExpression;
        }

        function GetItemQtyFromLocation() {
            var filterExpression = "LocationID = " + $('#<%=hdnLocationIDFrom.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                if (result == null) {
                    $('#<%=txtBinLocationName.ClientID %>').val('');
                }
                else {
                    $('#<%=txtBinLocationName.ClientID %>').val(result.BinLocationName);
                }
            });
        }

        //#region Edit & Delete
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
            cboItemUnit.PerformCallback();
            GetItemQtyFromLocation();
            $('#containerEntry').show();
        });
        //#endregion

        //#region Cbo Item Unit
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

        //#region Item Group

        function onGetItemGroupFilterExpression() {
            var filterExpression = onGetItemTypeFromFilterExpression();

            if (filterExpression != "") {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM ItemMaster WHERE ItemID IN (SELECT ItemID FROM DrugInfo WHERE IsCSSD = 1))";
            } else {
                filterExpression += "ItemGroupID IN (SELECT ItemGroupID FROM ItemMaster WHERE ItemID IN (SELECT ItemID FROM DrugInfo WHERE IsCSSD = 1))";
            }

            if ($('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '0') {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() + "/%')";
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
            var filterExpression = onGetItemTypeFromFilterExpression();

            var orderID = $('#<%=hdnRequestID.ClientID %>').val();
            var fromLocationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var toLocationID = $('#<%=hdnLocationIDTo.ClientID %>').val();

            if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnItemGroupID.ClientID %>').val() + "/%')";
            }
            else {
                if ($('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() != '0')
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + $('#<%=hdnFromLocationItemGroupID.ClientID %>').val() + "/%')";
            }

            if (orderID != '' && orderID != '0') {
                filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM MDServiceRequestDt WHERE RequestID = " + orderID + " AND IsDeleted = 0)";
            }

            filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID IN (" + fromLocationID + "))";
            filterExpression += " AND ItemID IN (SELECT ItemID FROM DrugInfo WHERE IsCSSD = 1)";
            filterExpression += " AND GCItemStatus != 'X181^999'";

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
            var filterExpression = "ItemCode = '" + value + "'";
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
                    GetItemQtyFromLocation();
                    cboItemUnit.PerformCallback();
                }
                else {
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtBinLocationName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(RequestID) {
            if ($('#<%=hdnRequestID.ClientID %>').val() == '0') {
                $('#<%=hdnRequestID.ClientID %>').val(RequestID);
                var filterExpression = 'RequestID = ' + RequestID;
                Methods.getObject('GetMDServiceRequestHdList', filterExpression, function (result) {
                    $('#<%=txtRequestNo.ClientID %>').val(result.RequestNo);
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
                    var RequestID = s.cpRequestID;
                    onAfterSaveRecordDtSuccess(RequestID);
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
            var requestID = $('#<%=hdnRequestID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (requestID == '' || requestID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "RequestID = " + requestID;
                    return true;
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }

        function setRightPanelButtonEnabled() {
            if ($('#<%:hdnRequestID.ClientID %>').val() == '' || $('#<%:hdnRequestID.ClientID %>').val() == '0') {
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
    </script>
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowUsingAlternateUnit" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowGetFromDestinationLocation" runat="server" />
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
                                <label class="lblLink" id="lblRequestNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRequestNo" Width="130px" ReadOnly="true" runat="server" />
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
                                <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                                <input type="hidden" id="hdnLocationHealthcareUnitFrom" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 1px" />
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
                                <label class="lblNormal lblLink" runat="server" id="lblCSSDPackage">
                                    <%=GetLabel("Paket CSSD")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPackageID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 1px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPackageCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPackageName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Permintaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboServiceType" ClientInstanceName="cboServiceType"
                                    Width="125px">
                                </dxe:ASPxComboBox>
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
                                            <asp:TextBox ID="txtRequestDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRequestTime" Width="100px" CssClass="time" runat="server" Style="text-align: center" />
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
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item Permintaan CSSD")%></div>
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
                                                    <asp:TextBox ID="txtBinLocationName" Width="120px" runat="server" ReadOnly="true" />
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
                                                    <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                        Width="125px" OnCallback="cboItemUnit_Callback">
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
                                                    <asp:TextBox ID="txtConversion" Width="360px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
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
                                                                <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
                                                                    src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                    src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
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
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Item Code" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfIsConsumption" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Is Consumption" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="cfBaseQtyUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Base Qty" HeaderStyle-Width="170px" />
                                            <asp:BoundField DataField="cfRequestQtyUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Request Qty" HeaderStyle-Width="170px" />
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
                        <span class="lblLink" id="lblAddData" <%= IsEditable() == "0" ? "style='display:none'" : "style='margin-right: 300px'" %>>
                            <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblQuickPick" <%= IsEditable() == "0" ? "style='display:none'" : "style='margin-right: 300px'" %>>
                                <%= GetLabel("Quick Picks")%></span> <span class="lblLink" id="lblPackageEntry" <%= IsEditable() == "0" ? "style='display:none'" : "" %>>
                                    <%= GetLabel("Package Entry")%></span>
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
