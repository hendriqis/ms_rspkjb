<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="StockAdjustmentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.StockAdjustmentEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblAddData').show();
            else
                $('#lblAddData').hide();

            setDatePicker('<%=txtAdjustmentDate.ClientID %>');
            $('#<%=txtAdjustmentDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region Location
            function getLocationFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionLocation() %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationallroleuser', getLocationFilterExpression(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').live('change', function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationAllUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result2) {
                            $('#<%=hdnLocationItemGroupID.ClientID %>').val(result2.ItemGroupID);
                            if (result2.GCHealthcareUnit != null && result2.GCHealthcareUnit != "") {
                                cboHealthcareUnit.SetValue(result2.GCHealthcareUnit);
                            }
                        });
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnLocationItemGroupID.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Adjustment No
            function onGetItemAdjustmentFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblAdjustmentNo.lblLink').live('click', function () {
                var filterExpression = onGetItemAdjustmentFilterExpression();
                openSearchDialog('itemtransactionhd', filterExpression, function (value) {
                    $('#<%=txtAdjustmentNo.ClientID %>').val(value);
                    onTxtAdjustmentNoChanged(value);
                });
            });

            $('#<%=txtAdjustmentNo.ClientID %>').live('change', function () {
                onTxtAdjustmentNoChanged($(this).val());
            });

            function onTxtAdjustmentNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionItemProduct() %>";
                if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '')
                    filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
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
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
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
            function getItemFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionItemProduct() %>";
                var adjustmentID = $('#<%=hdnAdjustmentID.ClientID %>').val();
                var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '') {
                    filterExpression += " AND ItemGroupID = '" + $('#<%=hdnItemGroupID.ClientID %>').val() + "'";
                }
                if (adjustmentID != '') {
                    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM ItemTransactionDt WHERE TransactionID = " + adjustmentID + " AND GCItemDetailStatus != 'X121^999')";
                }
                filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + locationID + " AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemStatus != 'X181^999'";
                return filterExpression;
            }

            $('#lblItem.lblLink').live('click', function () {
                openSearchDialog('item', getItemFilterExpression(), function (value) {
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
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                        $('#<%=hdnGCBaseUnit.ClientID %>').val(result.GCItemUnit);

                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupCode.ClientID %>').val(result.ItemGroupCode);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);

                        var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND ItemID = " + result.ItemID + " AND IsDeleted = 0";
                        Methods.getObject('GetItemPlanningList', filterExpression, function (result) {
                            if (result != null)
                                $('#<%=hdnCostAmount.ClientID %>').val(result.AveragePrice);
                            else
                                $('#<%=hdnCostAmount.ClientID %>').val('0');
                        });

                        var filterExpression = "LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
                        Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                            var qtyEnd = parseFloat(result.QuantityEND);
                            $('#<%=txtStockLocation.ClientID %>').val(qtyEnd + ' ' + result.ItemUnit);
                            $('#<%=txtStockEnd.ClientID %>').val(qtyEnd);
                            $('#<%=txtQuantity.ClientID %>').val('0');
                        });

                        cboItemUnit.PerformCallback();
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtStockLocation.ClientID %>').val('0');
                        $('#<%=hdnCostAmount.ClientID %>').val('0');
                        $('#<%=txtStockEnd.ClientID %>').val('0');
                    }
                });
            }
            //#endregion

            $('#<%=txtStockEnd.ClientID %>').live('change', function () {
                CalculateStock();
            });

            $('#<%=txtQuantity.ClientID %>').live('change', function () {
                CalculateStock();
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                    var allowSave = true;
                    var qtyInput = parseFloat($('#<%=txtQuantity.ClientID %>').val());
                    var adjType = cboGCAdjustmentType.GetValue();

                    if (adjType == "X173^001" && qtyInput < 0) {
                        allowSave = false;
                    }

                    if (adjType == "X173^002" && qtyInput > 0) {
                        allowSave = false;
                    }

                    if (allowSave == true) {
                        cbpProcess.PerformCallback('save');
                    } else {
                        alert("Penyesuaian dengan jenis PENGELUARAN, nilai penyesuaian harus kurang dari nol (nilai minus).");
                    }
                }
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        //#region Add & Edit & Delete
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

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                editedLineAmount = 0;
                $('#<%=txtQuantity.ClientID %>').val('1');
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                $('#<%=hdnItemGroupID.ClientID %>').val('');
                $('#<%=txtItemGroupCode.ClientID %>').val('');
                $('#<%=txtItemGroupName.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtStockEnd.ClientID %>').val('');
                $('#<%=txtQuantity.ClientID %>').val('');
                $('#<%=txtNotesDt.ClientID %>').val('');
                $('#<%=txtAdjustmentReason.ClientID %>').val('');
                $('#<%=txtStockLocation.ClientID %>').val('');
                cboGCAdjustmentType.SetEnabled(false);
                cboItemUnit.SetValue('');
                cboGCAdjustmentReason.SetValue('');
                $('#<%=txtConversion.ClientID %>').val('');

                $('#<%=chkIsFromStockEnd.ClientID %>').prop('checked', false);
                $('#<%=txtStockEnd.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtQuantity.ClientID%>').removeAttr('readonly');

                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=hdnItemGroupID.ClientID %>').val(entity.ItemGroupID);
            $('#<%=txtItemGroupCode.ClientID %>').val(entity.ItemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(entity.ItemGroupName1);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=txtNotesDt.ClientID %>').val(entity.Remarks);
            $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);
            $('#<%=txtAdjustmentReason.ClientID %>').val(entity.AdjustmentReason);
            $('#<%=hdnCostAmount.ClientID %>').val(entity.CostAmount);
            cboGCAdjustmentReason.SetValue(entity.GCAdjustmentReason);

            var filterExpression = "LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                var qtyEnd = parseFloat(result.QuantityEND);
                $('#<%=txtStockLocation.ClientID %>').val(qtyEnd + ' ' + result.ItemUnit);
                $('#<%=txtStockEnd.ClientID %>').val(qtyEnd);
            });

            cboItemUnit.PerformCallback();

            $('#<%=chkIsFromStockEnd.ClientID %>').prop('checked', false);
            $('#<%=txtStockEnd.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtQuantity.ClientID%>').removeAttr('readonly');

            $('#containerEntry').show();
        });

        //#endregion

        //#region Cbo Item Unit
        function onCboItemUnitEndCallBack() {
            if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '')
                cboItemUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
            else
                cboItemUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            onCboItemUnitChanged();
        }

        function onCboItemUnitChanged() {
            var baseValue = $('#<%=hdnGCBaseUnit.ClientID %>').val();
            var toUnitItem = cboItemUnit.GetValue();
            var baseText = getItemUnitName(baseValue);

            if (baseValue == toUnitItem) {
                $('#<%=hdnItemConversionFactor.ClientID %>').val('1');
                var conversion = "1 " + baseText + " = 1 " + baseText;
                $('#<%=txtConversion.ClientID %>').val(conversion);
            }
            else {
                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + toUnitItem + "'";
                Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                    var toConversion = getItemUnitName(toUnitItem);
                    $('#<%=hdnItemConversionFactor.ClientID %>').val(result);
                    var conversion = "1 " + toConversion + " = " + result + " " + baseText;
                    $('#<%=txtConversion.ClientID %>').val(conversion);
                });
            }

            $('#<%=chkIsFromStockEnd.ClientID %>').prop('checked', false);
            $('#<%=txtStockEnd.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtQuantity.ClientID%>').removeAttr('readonly');

            CalculateStock();
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

        $('#<%:chkIsFromStockEnd.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtStockEnd.ClientID%>').removeAttr('readonly');
                $('#<%=txtQuantity.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtStockEnd.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtQuantity.ClientID%>').removeAttr('readonly');
            }
        });

        function CalculateStock() {
            var value = cboGCAdjustmentType.GetValue();
            var newQty = parseFloat($('#<%=txtQuantity.ClientID %>').val());
            var newStockEnd = parseFloat($('#<%=txtStockEnd.ClientID %>').val());
            var toUnitItem = cboItemUnit.GetValue();
            var toConversionFactor = parseFloat($('#<%=hdnItemConversionFactor.ClientID %>').val());
            var minQty = 0; maxQty = 0;

            var filterExpression = "LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                var stockEnd = parseFloat(result.QuantityEND);
                var gcItemUnit = result.GCItemUnit;
                var itemUnit = result.ItemUnit;

                if (gcItemUnit == toUnitItem) {
                    maxQty = stockEnd;
                    stockEnd = stockEnd;
                } else {
                    maxQty = stockEnd / toConversionFactor;
                    stockEnd = stockEnd / toConversionFactor;
                }

                if (value == "X173^001") {
                    if (!$('#<%=chkIsFromStockEnd.ClientID %>').is(':checked')) {
                        newStockEnd = stockEnd + newQty;
                    }

                    if (stockEnd <= newStockEnd) {
                        if (!$('#<%=chkIsFromStockEnd.ClientID %>').is(':checked')) {
                            newStockEnd = stockEnd + newQty;
                            $('#<%=txtStockEnd.ClientID %>').val(newStockEnd);
                        } else {
                            newQty = newStockEnd - stockEnd;
                            $('#<%=txtQuantity.ClientID %>').val(newQty);
                        }
                    } else {
                        alert("Stok Sekarang harus lebih besar dari Stok Awal (minimal sejumlah " + stockEnd + " " + itemUnit + ").");

                        $('#<%=txtQuantity.ClientID %>').val('0');
                        $('#<%=txtStockEnd.ClientID %>').val('0');
                    }
                } else {
                    if ($('#<%=chkIsFromStockEnd.ClientID %>').is(':checked')) {
                        newQty = stockEnd - newStockEnd;
                        newQty = newQty * -1;
                    }

                    if ((newQty * -1) <= maxQty) {
                        if ($('#<%=chkIsFromStockEnd.ClientID %>').is(':checked')) {
                            newQty = stockEnd - newStockEnd;
                            newQty = newQty * -1;
                            $('#<%=txtQuantity.ClientID %>').val(newQty);
                        } else {
                            newStockEnd = stockEnd + newQty;
                            $('#<%=txtStockEnd.ClientID %>').val(newStockEnd);
                        }
                    } else {
                        alert("Jumlah penyesuaian tidak boleh lebih dari Stok Awal (maksimal sejumlah " + maxQty + " " + itemUnit + ").");

                        $('#<%=txtQuantity.ClientID %>').val('0');
                        $('#<%=txtStockEnd.ClientID %>').val('0');
                    }
                }
            });
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

        function onAfterSaveRecordDtSuccess(AdjustmentID) {
            if ($('#<%=hdnAdjustmentID.ClientID %>').val() == '0') {
                $('#<%=hdnAdjustmentID.ClientID %>').val(AdjustmentID);
                var filterExpression = 'TransactionID = ' + AdjustmentID;
                Methods.getObject('GetItemTransactionHdList', filterExpression, function (result) {
                    $('#<%=txtAdjustmentNo.ClientID %>').val(result.TransactionNo);
                });
                onAfterCustomSaveSuccess();
            }
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var adjustmentID = s.cpAdjustmentID;
                    onAfterSaveRecordDtSuccess(adjustmentID);
                    $('#lblAddData').click();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            $('#containerEntry').hide();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var adjustmentID = $('#<%=hdnAdjustmentID.ClientID %>').val();
            filterExpression.text = 'TransactionID = ' + adjustmentID;
            if (adjustmentID == 0 || adjustmentID == "") {
                errMessage.text = "Adjustment Tidak DiTemukan";
                return false;
            }
            return true;
        }

    </script>
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnAdjustmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
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
                                <label class="lblLink" id="lblAdjustmentNo">
                                    <%=GetLabel("No. Penyesuaian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdjustmentNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Penyesuaian") %>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAdjustmentDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdjustmentTime" Width="100px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <input type="hidden" id="hdnLocationItemGroupID" value="" runat="server" />
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
                                <label class="lblMandatory">
                                    <%=GetLabel("Unit/Bagian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcareUnit" ClientInstanceName="cboHealthcareUnit" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Penyesuaian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCAdjustmentType" ClientInstanceName="cboGCAdjustmentType"
                                    Width="100%" runat="server">
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
                                <label class="lblNormal">
                                    <%=GetLabel("No Referensi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="5" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Edit atau Tambah Item")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%">
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
                                                            <col style="width: 300px" />
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
                                                    <input type="hidden" value="" id="hdnCostAmount" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 300px" />
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
                                                        <%=GetLabel("Stok Awal")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 60px" />
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 120px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStockLocation" ReadOnly="true" CssClass="number" Width="100%"
                                                                    runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td class="tdLabel">
                                                                <label>
                                                                    <%=GetLabel("Stok Sekarang")%></label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsFromStockEnd" Checked="false" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStockEnd" CssClass="number" Width="100%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                        Width="200px" OnCallback="cboItemUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <input type="hidden" value="" id="hdnItemConversionFactor" runat="server" />
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConversion" Width="200px" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Alasan Penyesuaian")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 50%" />
                                                            <col style="width: 3px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxComboBox runat="server" ID="cboGCAdjustmentReason" ClientInstanceName="cboGCAdjustmentReason"
                                                                    Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAdjustmentReason" Width="100%" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="padding-top: 5px; vertical-align: top">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Catatan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotesDt" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                                    position: relative;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                    <input type="hidden" value="<%#:Eval("ItemGroupID") %>" bindingfield="ItemGroupID" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupCode") %>" bindingfield="ItemGroupCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemGroupName1") %>" bindingfield="ItemGroupName1" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCAdjustmentReason") %>" bindingfield="GCAdjustmentReason" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CostAmount") %>" bindingfield="CostAmount" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Barang" HeaderStyle-Width="300px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty Disesuaikan" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                            <asp:BoundField DataField="Conversion" HeaderText="Konversi" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="AdjustmentReason" HeaderText="Alasan" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada transaksi penyesuaian")%>
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
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Tambah Data")%></span>
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
