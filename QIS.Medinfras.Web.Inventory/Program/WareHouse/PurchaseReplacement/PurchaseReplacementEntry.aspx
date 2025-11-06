<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="PurchaseReplacementEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReplacementEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblAddData').show();
            else
                $('#lblAddData').hide();

            if ($('#<%=txtReplacementDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtReplacementDate.ClientID %>');
                $('#<%=txtReplacementDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            }

            if ($('#<%=txtReferenceDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtReferenceDate.ClientID %>');
                $('#<%=txtReferenceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }

            //#region Purchase Replacement No
            $('#lblPurchaseReplacementNo.lblLink').click(function () {
                openSearchDialog('purchasereplacementhd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtPurchaseReplacementNo.ClientID %>').val(value);
                    onTxtPurchaseReplacementNoChanged(value);
                });
            });

            $('#<%=txtPurchaseReplacementNo.ClientID %>').change(function () {
                onTxtPurchaseReplacementNoChanged($(this).val());
            });

            function onTxtPurchaseReplacementNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:GetSupplierFilterExpression() %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                    $('#<%=txtSupplierCode.ClientID %>').val(value);
                    onTxtSupplierChanged(value);
                });
            });

            $('#<%=txtSupplierCode.ClientID %>').change(function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtPurchaseReturnNo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Purchase Return
            function getPurchaseReturnFilterExpression() {
                var filterExpression = "<%:GetPurchaseReturnFilterExpression() %>";
                var supplierID = $('#<%=hdnSupplierID.ClientID %>').val();
                if (supplierID != '')
                    filterExpression += " AND BusinessPartnerID = " + $('#<%=hdnSupplierID.ClientID %>').val();
                return filterExpression;
            }

            $('#<%=lblPurchaseReturn.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('purchasereturnhd', getPurchaseReturnFilterExpression(), function (value) {
                    $('#<%=txtPurchaseReturnNo.ClientID %>').val(value);
                    onTxtPurchaseReturnChanged(value);
                });
            });

            $('#<%=txtPurchaseReturnNo.ClientID %>').change(function () {
                onTxtPurchaseReturnChanged($(this).val());
            });

            function onTxtPurchaseReturnChanged(value) {
                var filterExpression = getPurchaseReturnFilterExpression() + " AND PurchaseReturnNo = '" + value + "'";
                Methods.getObject('GetvPurchaseReturnHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val(result.PurchaseReturnID);
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%=txtSupplierName.ClientID %>').val(result.SupplierName);
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationCode.ClientID %>').val(result.LocationCode);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnPurchaseReturnID.ClientID %>').val('');
                        $('#<%=txtPurchaseReturnNo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Location
            function getLocationFilterExpression() {
                var filterExpression = "<%:GetLocationFilterExpression() %>";
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
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#lblOldItemGroup').attr('class', 'lblLink');
                    $('#lblOldItem').attr('class', 'lblLink');
                    $('#<%=txtOldItemGroupCode.ClientID %>').removeAttr('readonly');
                    $('#<%=txtOldItemCode.ClientID %>').removeAttr('readonly');

                    $('#<%=hdnIsEdit.ClientID %>').val('0');
                    $('#<%=txtQuantity.ClientID %>').val('1');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=hdnItemGroupID.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');
                    $('#<%=hdnOldItemID.ClientID %>').val('');
                    $('#<%=txtOldItemCode.ClientID %>').val('');
                    $('#<%=txtOldItemName.ClientID %>').val('');
                    $('#<%=hdnOldItemGroupID.ClientID %>').val('');
                    $('#<%=txtOldItemGroupCode.ClientID %>').val('');
                    $('#<%=txtOldItemGroupName.ClientID %>').val('');
                    $('#<%=txtOldQuantity.ClientID %>').val('');
                    $('#<%=txtOldItemUnit.ClientID %>').val('');
                    $('#<%=txtOldConversion.ClientID %>').val('');
                    $('#<%=txtPrice.ClientID %>').val('');
                    $('#<%=txtPricePerUnit.ClientID %>').val('');
                    $('#containerEntry').show();
                }
            });

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionItemProduct() %>";
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
                //var replacementID = $('#<%=hdnPurchaseReplacementID.ClientID %>').val();
                //var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                if ($('#<%=txtItemGroupCode.ClientID %>').val() != '')
                    filterExpression += " AND ItemGroupID = '" + $('#<%=hdnItemGroupID.ClientID %>').val() + "'";
                //if (replacementID != '')
                //    filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM ItemTransactionDt WHERE TransactionID = " + replacementID + " AND IsDeleted = 0)";
                //filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + locationID + ') AND IsDeleted = 0';
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
                        cboItemUnit.PerformCallback();
                    }
                    else {
                        $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

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

            //#region Item Group

            $('#lblOldItemGroup.lblLink').live('click', function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtOldItemGroupCode.ClientID %>').val(value);
                    onTxtOldItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtOldItemGroupCode.ClientID %>').live('change', function () {
                onTxtOldItemGroupCodeChanged($(this).val());
            });

            function onTxtOldItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                $('#<%=txtOldItemCode.ClientID %>').val('');
                $('#<%=txtOldItemName.ClientID %>').val('');
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnOldItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtOldItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnOldItemGroupID.ClientID %>').val('');
                        $('#<%=txtOldItemGroupCode.ClientID %>').val('');
                        $('#<%=txtOldItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item
            function getOldItemFilterExpression() {
                var filterExpression = "PurchaseReturnID = " + $('#<%=hdnPurchaseReturnID.ClientID %>').val() + " AND GCItemDetailStatus != '<%=GetTransactionStatusVoid() %>'";
                if ($('#<%=hdnPurchaseReplacementID.ClientID %>').val() != '')
                    filterExpression += " AND ID NOT IN (SELECT PurchaseReturnDtID FROM PurchaseReplacementDt WHERE PurchaseReplacementID =" + $('#<%=hdnPurchaseReplacementID.ClientID %>').val() + " AND GCItemDetailStatus != '<%=GetTransactionStatusVoid() %>'" + ")";
                if ($('#<%=txtOldItemGroupCode.ClientID %>').val() != '')
                    filterExpression += " AND ItemGroupID = '" + $('#<%=hdnOldItemGroupID.ClientID %>').val() + "'";
                return filterExpression;
            }

            $('#lblOldItem.lblLink').live('click', function () {
                openSearchDialog('purchasereturndt', getOldItemFilterExpression(), function (value) {
                    $('#<%=txtOldItemCode.ClientID %>').val(value);
                    onTxtOldItemCodeChanged(value);
                });
            });

            $('#<%=txtOldItemCode.ClientID %>').live('change', function () {
                onTxtOldItemCodeChanged($(this).val());
            });

            function onTxtOldItemCodeChanged(value) {
                var filterExpression = getOldItemFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvPurchaseReturnDtList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnOldItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtOldItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnPurchaseReturnDtID.ClientID %>').val(result.ID);
                        
                        $('#<%=hdnOldItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtOldItemGroupCode.ClientID %>').val(result.ItemGroupCode);
                        $('#<%=txtOldItemGroupName.ClientID %>').val(result.ItemGroupName1);

                        $('#<%=txtOldQuantity.ClientID %>').val(result.Quantity);
                        $('#<%=txtOldItemUnit.ClientID %>').val(result.ItemUnit);
                        $('#<%=txtOldConversion.ClientID %>').val("1 " + result.ItemUnit + " = " + result.ConversionFactor + " " + result.BaseUnit);

                        $('#<%=txtPrice.ClientID %>').val(result.UnitPrice).trigger('changeValue');
                        $('#<%=txtPricePerUnit.ClientID %>').val('per ' + result.ItemUnit);
                    }
                    else {
                        $('#<%=hdnOldItemID.ClientID %>').val('');
                        $('#<%=txtOldItemCode.ClientID %>').val('');
                        $('#<%=txtOldItemName.ClientID %>').val('');

                        $('#<%=txtOldQuantity.ClientID %>').val('');
                        $('#<%=txtOldItemUnit.ClientID %>').val('');
                        $('#<%=txtOldConversion.ClientID %>').val('');

                        $('#<%=txtPrice.ClientID %>').val(0).trigger('changeValue');
                        $('#<%=txtPricePerUnit.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

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
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

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

        function onAfterSaveRecordDtSuccess(PurchaseReplacementID) {
            if ($('#<%=hdnPurchaseReplacementID.ClientID %>').val() == '') {
                $('#<%=hdnPurchaseReplacementID.ClientID %>').val(PurchaseReplacementID);
                var filterExpression = 'PurchaseReplacementID = ' + PurchaseReplacementID;
                Methods.getObject('GetPurchaseReplacementHdList', filterExpression, function (result) {
                    $('#<%=txtPurchaseReplacementNo.ClientID %>').val(result.PurchaseReplacementNo);
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
                    onAfterSaveRecordDtSuccess(s.cpReplacementID);
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
            $('#lblOldItemGroup').attr('class', 'lblDisabled');
            $('#lblOldItem').attr('class', 'lblDisabled');
            $('#<%=txtOldItemGroupCode.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOldItemCode.ClientID %>').attr('readonly', 'readonly');

            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnOldItemID.ClientID %>').val(entity.FromItemID);
            $('#<%=txtOldItemCode.ClientID %>').val(entity.FromItemCode);
            $('#<%=txtOldItemName.ClientID %>').val(entity.FromItemName1);
            $('#<%=txtOldQuantity.ClientID %>').val(entity.FromQuantity);
            $('#<%=txtOldItemUnit.ClientID %>').val(entity.FromItemUnit);
            $('#<%=txtOldConversion.ClientID %>').val("1 " + entity.FromItemUnit + " = " + entity.FromConversionFactor + " " + entity.FromBaseUnit);
            $('#<%=txtPrice.ClientID %>').val(entity.FromUnitPrice).trigger('changeValue');
            $('#<%=txtPricePerUnit.ClientID %>').val('per ' + entity.FromItemUnit);

            $('#<%=hdnItemID.ClientID %>').val(entity.ToItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ToItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ToItemName1);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            //          $('#<%=hdnItemID.ClientID %>').val(entity.FromItemID);
            //          $('#<%=txtItemCode.ClientID %>').val(entity.FromItemCode);
            //          $('#<%=txtItemName.ClientID %>').val(entity.FromItemName1);

            cboItemUnit.PerformCallback();
            $('#containerEntry').show();
        });
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var purchaseReplacementID = $('#<%=hdnPurchaseReplacementID.ClientID %>').val();
            if (purchaseReplacementID == '' || purchaseReplacementID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "PurchaseReplacementID = " + purchaseReplacementID;
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
        <%--<div class="pageTitle"><%=GetLabel("Penggantian Barang")%></div>--%>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <input type="hidden" id="hdnPurchaseReplacementID" value="0" runat="server" />
                    <input type="hidden" id="hdnPurchaseReturnDtID" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblPurchaseReplacementNo"><%=GetLabel("No Penggantian Barang")%></label></td>
                            <td><asp:TextBox ID="txtPurchaseReplacementNo" Width="150px" ReadOnly="true" runat="server" TabIndex="1" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal") %></label></td>
                            <td><asp:TextBox ID="txtReplacementDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory lblLink" runat="server" id="lblLocation"><%=GetLabel("Lokasi")%></label></td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtLocationCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory lblLink" id="lblSupplier" runat="server"><%=GetLabel("Supplier/Penyedia")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory lblLink" runat="server" id="lblPurchaseReturn"><%=GetLabel("No. Pengembalian")%></label></td>
                            <td>
                                <input type="hidden" runat="server" id="hdnPurchaseReturnID" value="" />
                                <asp:TextBox ID="txtPurchaseReturnNo" Width="150px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("No Referensi")%></label></td>
                            <td><asp:TextBox ID="txtReferenceNo" Width="150px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Tanggal Referensi") %></label></td>
                            <td><asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                            <td><asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle"><%=GetLabel("Edit atau Tambah Item")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <div class="lblComponent"><%=GetLabel("Barang Lama") %></div>
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel"><label class="lblLink" id="lblOldItemGroup"><%=GetLabel("Kelompok Item")%></label></td>
                                                <td>
                                                    <input type="hidden" value="" id="hdnOldItemGroupID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td><asp:TextBox ID="txtOldItemGroupCode" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtOldItemGroupName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblLink" id="lblOldItem"><%=GetLabel("Item")%></label></td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnOldItemID" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td><asp:TextBox ID="txtOldItemCode" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtOldItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label><%=GetLabel("Jumlah")%></label></td>
                                                <td><asp:TextBox ID="txtOldQuantity" CssClass="number" ReadOnly="true" Width="120px" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label><%=GetLabel("Satuan Item")%></label></td>
                                                <td><asp:TextBox ID="txtOldItemUnit"  Width="300px" ReadOnly="true" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label><%=GetLabel("Konversi")%></label></td>
                                                <td><asp:TextBox ID="txtOldConversion" Width="180px" runat="server" ReadOnly="true" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Harga")%></label></td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td><asp:TextBox ID="txtPrice" CssClass="txtCurrency" ReadOnly="true" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtPricePerUnit" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <div class="lblComponent"><%=GetLabel("Barang Baru") %></div>
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 150px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblItemGroup"><%=GetLabel("Kelompok Item")%></label>
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
                                                            <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblItem"><%=GetLabel("Item")%></label></td>
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
                                                            <td><asp:TextBox ID="txtItemCode" Width="100%" runat="server" /></td>
                                                            <td>&nbsp;</td>
                                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Jumlah")%></label></td>
                                                <td><asp:TextBox ID="txtQuantity" CssClass="number" Width="120px" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Satuan Item")%></label></td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboItemUnit" ClientInstanceName="cboItemUnit"
                                                        Width="300px" OnCallback="cboItemUnit_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){ onCboItemUnitEndCallBack(); }" ValueChanged="function(s,e){ onCboItemUnitChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <input type="hidden" value="" id="hdnItemConversionFactor" runat="server" />
                                                    <label class="lblMandatory"><%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td><asp:TextBox ID="txtConversion" Width="180px" runat="server" ReadOnly="true" /></td>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>' 
                                                                    src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>' 
                                                                    src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                            </td>
                                                        </tr>
                                                    </table> 
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("FromItemID") %>" bindingfield="FromItemID" />
                                                    <input type="hidden" value="<%#:Eval("FromItemCode") %>" bindingfield="FromItemCode" />
                                                    <input type="hidden" value="<%#:Eval("FromItemName1") %>" bindingfield="FromItemName1" />
                                                    <input type="hidden" value="<%#:Eval("FromQuantity") %>" bindingfield="FromQuantity" />
                                                    <input type="hidden" value="<%#:Eval("FromBaseUnit") %>" bindingfield="FromBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("FromItemUnit") %>" bindingfield="FromItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("FromConversionFactor") %>" bindingfield="FromConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("FromUnitPrice") %>" bindingfield="FromUnitPrice" />
                                                    <input type="hidden" value="<%#:Eval("ToItemID") %>" bindingfield="ToItemID" />
                                                    <input type="hidden" value="<%#:Eval("ToItemCode") %>" bindingfield="ToItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ToItemName1") %>" bindingfield="ToItemName1" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FromItemName1" HeaderText="Barang Lama"  />
                                            <asp:BoundField DataField="CustomQuantityItemUnit" HeaderText="Jumlah" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ToItemName1" HeaderText="Barang Baru"  />
                                            <asp:BoundField DataField="CustomFromQuantityItemUnit" HeaderText="Jumlah" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
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
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Add Data")%></span>
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