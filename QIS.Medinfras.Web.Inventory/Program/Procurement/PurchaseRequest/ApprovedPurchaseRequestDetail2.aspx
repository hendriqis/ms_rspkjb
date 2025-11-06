<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ApprovedPurchaseRequestDetail2.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ApprovedPurchaseRequestDetail2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessPurchaseRequest" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnPurchaseRequestHdDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Decline")%></div>
    </li>
    <li id="btnPurchaseRequestHdClose" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Close")%></div>
    </li>
    <li id="btnPurchaseRequestHdSave" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var flag = true;

        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }

        function onLoad() {
            setDatePicker('<%=txtItemOrderDate.ClientID %>');

            $('#<%=txtItemOrderDate.ClientID %>').live('change', function () {
                var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                $('.grdPurchaseRequest .chkIsSelected input').each(function () {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html().trim();
                    var supplierID = $tr.find('.hdnSupplierID').val();
                    var itemName = $tr.find('.tdItemName').html().trim();
                    var hdnItemName1 = $tr.find('.hdnItemName1').val();
                    var supplierName = $tr.find('.lblSupplier').html();
                    var GCPurchaseUnitORI = $tr.find('.hdnGCPurchaseUnitORI').val();
                    var GCPurchaseUnit = $tr.find('.hdnGCPurchaseUnit').val();
                    var conversionFactorORI = $tr.find('.hdnConversionFactorORI').val();
                    var conversionFactor = $tr.find('.hdnConversionFactor').val();
                    var purchaseUnit = $tr.find('.lblPurchaseUnit').html();
                    var tempPO = parseFloat($tr.find('.txtPurchaseQty').val());
                    var tempMaxPO = parseFloat($tr.find('.txtPurchaseQty').attr("max"));
                    var purchaseQty = parseFloat($tr.find('.txtPurchaseQty').val());
                    var supplierID = $tr.find('.hdnSupplierID').val();

                    if (hdnIM0131 == "0") {
                        Methods.getItemMasterPurchase(key, supplierID, function (result2) {
                            if (result2 != null) {
                                var oConversionFactor = parseFloat($tr.find('.hdnConversionFactor').val());
                                if (oConversionFactor == 1) {
                                    $tr.find('.txtPrice').val(result2.Price);
                                } else {
                                    var price = parseFloat(result2.Price * oConversionFactor);
                                    $tr.find('.txtPrice').val(price).trigger('change');

                                }
                                $tr.find('.txtDiscount1').val(result2.Discount);
                                $tr.find('.txtDiscount2').val(result2.Discount2);
                            }
                            else {
                                $tr.find('.txtPrice').val('0');
                                $tr.find('.txtDiscount1').val('0');
                                $tr.find('.txtDiscount2').val('0');
                            }
                        });
                    }
                    else {
                        Methods.getItemMasterPurchaseWithDate(key, supplierID, orderDateFormatString, function (result2) {
                            if (result2 != null) {
                                var oConversionFactor = parseFloat($tr.find('.hdnConversionFactor').val());
                                if (oConversionFactor == 1) {
                                    $tr.find('.txtPrice').val(result2.Price);
                                }
                                else {
                                    var price = parseFloat(result2.Price * oConversionFactor);
                                    $tr.find('.txtPrice').val(price).trigger('change');
                                }
                                $tr.find('.txtDiscount1').val(result2.Discount);
                                $tr.find('.txtDiscount2').val(result2.Discount2);
                            }
                            else {
                                $tr.find('.txtPrice').val('0');
                                $tr.find('.txtDiscount1').val('0');
                                $tr.find('.txtDiscount2').val('0');
                            }
                        });
                    }
                });
            });

            $('#<%=btnProcessPurchaseRequest.ClientID %>').click(function () {
                var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                if (locationID == '0' || locationID == '')
                    $('#<%=txtLocationCode.ClientID %>').addClass('error');
                else {
                    $('#<%=txtLocationCode.ClientID %>').removeClass('error');
                    var errMessage = { text: '' };

                    $('#<%=hdnListGCPurchaseUnitORI.ClientID %>').val('');
                    $('#<%=hdnListGCPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnListPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnListSupplierID.ClientID %>').val('');
                    $('#<%=hdnListSupplierName.ClientID %>').val('');
                    $('#<%=hdnSelectedMember.ClientID %>').val('');
                    $('#<%=hdnPurchaseOrderQty.ClientID %>').val('');
                    $('#<%=hdnPrice.ClientID %>').val('');
                    $('#<%=hdnDiscount1.ClientID %>').val('');
                    $('#<%=hdnDiscount2.ClientID %>').val('');
                    $('#<%=hdnListConversionFactorORI.ClientID %>').val('');
                    $('#<%=hdnListConversionFactor.ClientID %>').val('');
                    $('#<%=hdnListTermID.ClientID %>').val('');
                    $('#<%=hdnListIsUrgent.ClientID %>').val('');
                    $('#<%=hdnIsSortByItemAndPurchaseUnit.ClientID %>').val('');

                    getCheckedMember(errMessage);
                    if (errMessage.text == '') {
                        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                            showToast('Warning', 'Please Select Item First');
                        else
                            onCustomButtonClick('approve');
                    }
                    else
                        showToast('Warning', errMessage.text);
                }
            });

            $('#<%=btnPurchaseRequestHdDecline.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Item First');
                else
                    onCustomButtonClick('decline');
            });

            $('#<%=btnPurchaseRequestHdClose.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Item First');
                else
                    onCustomButtonClick('close');
            });

            $('#<%=btnPurchaseRequestHdSave.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Item First');
                else
                    onCustomButtonClick('save');
            });

            //#region Purchase Request
            $('#lblPurchaseRequestNo.lblLink').click(function () {
                if ($('#<%=txtLocationCode.ClientID %>').val() == '') {
                    showToast('Warning', 'Lokasi Permintaan Pembelian harus diisi!');
                }
                else {
                    if ($('#<%=hdnIsUsedProductLine.ClientID %>').val() == "1") {
                        if ($('#<%=hdnProductLineID.ClientID %>').val() == '0' || $('#<%=hdnProductLineID.ClientID %>').val() == '') {
                            showToast('Warning', 'Product Line harus diisi!');
                        } else {
                            var LocationIDFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                            var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                            if ($('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val() == "1") {
                                var PurchaseOrderType = cboPurchaseOrderType.GetValue();
                            } else {
                                var PurchaseOrderType = "";
                            }

                            var id = LocationIDFrom + "|" + ProductLineID + "|" + PurchaseOrderType;
                            var url = ResolveUrl("~/Program/Procurement/PurchaseRequest/PurchaseRequestSearchCtl2.ascx");
                            openUserControlPopup(url, id, 'Permintaan Pembelian', 1200, 500);
                        }
                    } else {
                        var LocationIDFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                        var ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                        if ($('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val() == "1") {
                            var PurchaseOrderType = $('#<%=hdnPurchaseOrderType.ClientID %>').val();
                            //                            var PurchaseOrderType = cboPurchaseOrderType.GetValue();
                        } else {
                            var PurchaseOrderType = "";
                        }

                        if ($('#<%=hdnProductLineID.ClientID %>').val() == '') {
                            ProductLineID = 0;
                        }
                        else {
                            ProductLineID = $('#<%=hdnProductLineID.ClientID %>').val();
                        }
                        var id = LocationIDFrom + "|" + ProductLineID + "|" + PurchaseOrderType; ;
                        var url = ResolveUrl("~/Program/Procurement/PurchaseRequest/PurchaseRequestSearchCtl2.ascx");
                        openUserControlPopup(url, id, 'Permintaan Pembelian', 1200, 500);
                    }
                }
            });
            //#endregion

            //#region Location From

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
                var orderNo = $('#<%=txtOrderNo.ClientID %>').val();
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";

                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        if (orderNo != '') {
                            setPurchaseRequest('', '');
                        }
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        setPurchaseRequest('', '');
                    }
                });
            }
            //#endregion

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
                var orderNo = $('#<%=txtOrderNo.ClientID %>').val();
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);

                        if (orderNo != '') {
                            setPurchaseRequest('', '');
                        }
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                        setPurchaseRequest('', '');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsNotProceedToPOOnly.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=chkIsSortByQtyOnHand.ClientID %>').change(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    var message = "Ada item yang sudah dipilih untuk dilakukan proses.\n Refresh tampilan tidak bisa dilakukan karena akan mereset item-item yang sudah terpilih.";
                    showToast('Warning', message);

                    if ($('#<%=hdnOrderByLastValue.ClientID %>').val() == '1')
                        $('#<%=chkIsSortByQtyOnHand.ClientID %>').prop('checked', true);
                    else
                        $('#<%=chkIsSortByQtyOnHand.ClientID %>').prop('checked', false);

                    event.stopImmediatePropagation();
                    return false;
                }
                else {
                    if ($('#<%=chkIsSortByQtyOnHand.ClientID %>').is(':checked'))
                        $('#<%=hdnOrderByLastValue.ClientID %>').val('1');
                    else
                        $('#<%=hdnOrderByLastValue.ClientID %>').val('0');

                    cbpView.PerformCallback('refresh');
                }
            });
        }

        function setPurchaseRequest(purchaseRequestID, purchaseRequestNo) {
            $('#<%=hdnPurchaseRequestID.ClientID %>').val(purchaseRequestID);
            $('#<%=txtOrderNo.ClientID %>').val(purchaseRequestNo);
            cbpView.PerformCallback('refresh');
        }

        function getCheckedMember(errMessage) {
            var lstGCPurchaseUnitORI = $('#<%=hdnListGCPurchaseUnitORI.ClientID %>').val().split('|');
            var lstGCPurchaseUnit = $('#<%=hdnListGCPurchaseUnit.ClientID %>').val().split('|');
            var lstPurchaseUnit = $('#<%=hdnListPurchaseUnit.ClientID %>').val().split('|');
            var lstSupplierID = $('#<%=hdnListSupplierID.ClientID %>').val().split('|');
            var lstSupplierName = $('#<%=hdnListSupplierName.ClientID %>').val().split('|');
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstPurchaseOrderQty = $('#<%=hdnPurchaseOrderQty.ClientID %>').val().split('|');
            var lstPrice = $('#<%=hdnPrice.ClientID %>').val().split('|');
            var lstDiscount1 = $('#<%=hdnDiscount1.ClientID %>').val().split('|');
            var lstDiscount2 = $('#<%=hdnDiscount2.ClientID %>').val().split('|');
            var lstConversionFactorORI = $('#<%=hdnListConversionFactorORI.ClientID %>').val().split('|');
            var lstConversionFactor = $('#<%=hdnListConversionFactor.ClientID %>').val().split('|');
            var lstTermID = $('#<%=hdnListTermID.ClientID %>').val().split('|');
            var lstIsUrgent = $('#<%=hdnListIsUrgent.ClientID %>').val().split('|');
            var hdnIsSortByItemAndPurchaseUnit = $('#<%=hdnIsSortByItemAndPurchaseUnit.ClientID %>').val();

            var result = '';
            var itemEmptySupplier = "";
            var itemEmptyQty = "";
            var itemOverRequest = "";
            $('.grdPurchaseRequest .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html().trim();
                    var supplierID = $tr.find('.hdnSupplierID').val();
                    var itemName = $tr.find('.tdItemName').html().trim();
                    var hdnItemName1 = $tr.find('.hdnItemName1').val();
                    var supplierName = $tr.find('.lblSupplier').html();
                    var GCPurchaseUnitORI = $tr.find('.hdnGCPurchaseUnitORI').val();
                    var GCPurchaseUnit = $tr.find('.hdnGCPurchaseUnit').val();
                    var conversionFactorORI = $tr.find('.hdnConversionFactorORI').val();
                    var conversionFactor = $tr.find('.hdnConversionFactor').val();
                    var purchaseUnit = $tr.find('.lblPurchaseUnit').html();
                    var tempPO = parseFloat($tr.find('.txtPurchaseQty').val());
                    var tempMaxPO = parseFloat($tr.find('.txtPurchaseQty').attr("max"));
                    var purchaseQty = parseFloat($tr.find('.txtPurchaseQty').val());

                    if (GCPurchaseUnitORI != GCPurchaseUnit) {
                        tempMaxPO = ((tempMaxPO * conversionFactorORI) / conversionFactor);
                    }

                    if (purchaseQty > tempMaxPO) {
                        if ($('#<%=hdnIsPOQtyCannotOverPRQty.ClientID %>').val() == "1") {
                            if (GCPurchaseUnitORI == GCPurchaseUnit) {
                                purchaseQty = parseFloat($tr.find('.txtPurchaseQty').attr("max"));
                            }
                            else {
                                purchaseQty = ((parseFloat($tr.find('.txtQty').attr("max")) * conversionFactorORI) / conversionFactor);
                            }
                        } else {
                            purchaseQty = parseFloat($tr.find('.txtPurchaseQty').val());
                        }
                    } else {
                        purchaseQty = parseFloat($tr.find('.txtPurchaseQty').val());
                    }

                    var requestQty = $tr.find('.txtPurchaseQty').attr("max");
                    var requestQtyCustom = 0;
                    var price = $tr.find('.txtPrice').val();
                    var discount1 = $tr.find('.txtDiscount1').val();
                    var discount2 = $tr.find('.txtDiscount2').val();
                    var termID = $tr.find('.hdnTermID').val();
                    var isUrgent = $tr.find('.hdnIsUrgent').val();
                    var idx = lstSelectedMember.indexOf(key);

                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSupplierID.push(supplierID);
                        lstSupplierName.push(supplierName);
                        lstGCPurchaseUnit.push(GCPurchaseUnit);
                        lstPurchaseUnit.push(purchaseUnit);
                        lstPurchaseOrderQty.push(purchaseQty);
                        lstPrice.push(price);
                        lstDiscount1.push(discount1);
                        lstDiscount2.push(discount2);
                        lstConversionFactor.push(conversionFactor);
                        lstTermID.push(termID);
                        lstIsUrgent.push(isUrgent);
                    }
                    else {
                        if (hdnIsSortByItemAndPurchaseUnit != '1') {
                            lstDiscount1[idx] = discount1;
                            lstDiscount2[idx] = discount2;
                            lstPrice[idx] = price;
                            lstPurchaseOrderQty[idx] = purchaseQty;
                            lstSupplierName[idx] = supplierName;
                            lstSupplierID[idx] = supplierID;
                            lstGCPurchaseUnit[idx] = GCPurchaseUnit;
                            lstPurchaseUnit[idx] = purchaseUnit;
                            lstConversionFactor[idx] = conversionFactor;
                            lstTermID[idx] = termID;
                            lstIsUrgent[idx] = isUrgent;
                        }
                        else {
                            lstSelectedMember.push(key);
                            lstSupplierID.push(supplierID);
                            lstSupplierName.push(supplierName);
                            lstGCPurchaseUnit.push(GCPurchaseUnit);
                            lstPurchaseUnit.push(purchaseUnit);
                            lstPurchaseOrderQty.push(purchaseQty);
                            lstPrice.push(price);
                            lstDiscount1.push(discount1);
                            lstDiscount2.push(discount2);
                            lstConversionFactor.push(conversionFactor);
                            lstTermID.push(termID);
                            lstIsUrgent.push(isUrgent);
                        }
                    }

                    if (supplierID == '0') {
                        if (itemEmptySupplier != "")
                            itemEmptySupplier += ", ";
                        itemEmptySupplier += hdnItemName1.toString();
                    }

                    if (purchaseQty == '0') {
                        if (itemEmptyQty != "")
                            itemEmptyQty += ", ";
                        itemEmptyQty += hdnItemName1.toString();
                    }

                    if (GCPurchaseUnitORI != GCPurchaseUnit) {
                        if (conversionFactorORI > conversionFactor) {
                            requestQtyCustom = requestQty * conversionFactorORI;
                        }
                        else {
                            requestQtyCustom = requestQty / conversionFactor;
                        }
                    } else {
                        requestQtyCustom = requestQty;
                    }

                    if (requestQtyCustom < purchaseQty) {
                        if (itemOverRequest != "")
                            itemOverRequest += ", ";
                        itemOverRequest += itemName.toString().trim();
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstPurchaseOrderQty.splice(idx, 1);
                        lstPrice.splice(idx, 1);
                        lstSupplierID.splice(idx, 1);
                        lstSupplierName.splice(idx, 1);
                        lstDiscount1.splice(idx, 1);
                        lstDiscount2.splice(idx, 1);
                        lstGCPurchaseUnit.splice(idx, 1);
                        lstPurchaseUnit.splice(idx, 1);
                        lstConversionFactor.splice(idx, 1);
                        lstTermID.splice(idx, 1);
                        lstIsUrgent.splice(idx, 1);
                    }
                }
            });

            if (errMessage != null) {
                if (itemEmptySupplier != '') {
                    errMessage.text = "Silakan Pilih Supplier untuk Item <b>" + itemEmptySupplier + "</b> Terlebih Dahulu";
                }

                if (itemEmptyQty != '') {
                    if (errMessage.text != '') {
                        errMessage.text += '<br>';
                    }
                    errMessage.text += "Silakan Isi Qty untuk Item <b>" + itemEmptySupplier + "</b> Terlebih Dahulu";
                }

                if ($('#<%=hdnIsPOQtyCannotOverPRQty.ClientID %>').val() == "1") {
                    if (itemOverRequest != '') {
                        if (errMessage.text != '') {
                            errMessage.text += '<br>';
                        }
                        errMessage.text += "Qty untuk Item <b>" + itemOverRequest + "</b> Melebihi Permintaan";
                    }
                }
            }

            $('#<%=hdnListGCPurchaseUnit.ClientID %>').val(lstGCPurchaseUnit.join('|'));
            $('#<%=hdnListPurchaseUnit.ClientID %>').val(lstPurchaseUnit.join('|'));
            $('#<%=hdnListSupplierID.ClientID %>').val(lstSupplierID.join('|'));
            $('#<%=hdnListConversionFactor.ClientID %>').val(lstConversionFactor.join('|'));
            $('#<%=hdnListSupplierName.ClientID %>').val(lstSupplierName.join('|'));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(',').trim());
            $('#<%=hdnPurchaseOrderQty.ClientID %>').val(lstPurchaseOrderQty.join('|'));
            $('#<%=hdnPrice.ClientID %>').val(lstPrice.join('|'));
            $('#<%=hdnDiscount1.ClientID %>').val(lstDiscount1.join('|'));
            $('#<%=hdnDiscount2.ClientID %>').val(lstDiscount2.join('|'));
            $('#<%=hdnListTermID.ClientID %>').val(lstTermID.join('|'));
            $('#<%=hdnListIsUrgent.ClientID %>').val(lstIsUrgent.join('|'));
        }

        function onAfterCustomClickSuccess(type, retval) {
            var param = retval.split('|');
            var orderPerSupplier = param[0].split(';');
            $('#<%=hdnPurchaseRequestID.ClientID %>').val(param[2]);
            $('#<%=txtOrderNo.ClientID %>').val(param[3]);
            var tempText = "";
            for (var a = 0; a < orderPerSupplier.length - 1; a++) {
                var paramDetail = orderPerSupplier[a].split('^');
                if (tempText != '')
                    tempText += "<br />";
                tempText += "Pemesanan Barang untuk Supplier <b>" + paramDetail[1] + "</b> dengan Nomor Pemesanan <b>" + paramDetail[0] + "</b>";
            }

            if (type != 'decline') {
                showToast('Save Success', tempText, function () {
                    $('#<%=hdnSelectedMember.ClientID %>').val('');
                    $('#<%=hdnListSupplierID.ClientID %>').val('');
                    $('#<%=hdnListConversionFactor.ClientID %>').val('');
                    $('#<%=hdnListSupplierName.ClientID %>').val('');
                    $('#<%=hdnListGCPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnListPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnPurchaseOrderQty.ClientID %>').val('');
                    $('#<%=hdnDiscount1.ClientID %>').val('');
                    $('#<%=hdnDiscount2.ClientID %>').val('');
                    $('#<%=hdnPrice.ClientID %>').val('');
                    $('#<%=hdnListTermID.ClientID %>').val('');
                    $('#<%=hdnListIsUrgent.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                });
            }
            else {
                showToast('Decline Success', tempText, function () {
                    $('#<%=hdnSelectedMember.ClientID %>').val('');
                    $('#<%=hdnListSupplierID.ClientID %>').val('');
                    $('#<%=hdnListConversionFactor.ClientID %>').val('');
                    $('#<%=hdnListSupplierName.ClientID %>').val('');
                    $('#<%=hdnListGCPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnListPurchaseUnit.ClientID %>').val('');
                    $('#<%=hdnPurchaseOrderQty.ClientID %>').val('');
                    $('#<%=hdnDiscount1.ClientID %>').val('');
                    $('#<%=hdnDiscount2.ClientID %>').val('');
                    $('#<%=hdnPrice.ClientID %>').val('');
                    $('#<%=hdnListTermID.ClientID %>').val('');
                    $('#<%=hdnListIsUrgent.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                });
            }

            var filterExpression = getLocationFilterExpression() + "LocationID = " + param[4] + "";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                }
                else {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                    $('#<%=txtLocationCode.ClientID %>').val('');
                    $('#<%=txtLocationName.ClientID %>').val('');
                }
            });
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            $('#<%=hdnListSupplierID.ClientID %>').val('');
            $('#<%=hdnListConversionFactor.ClientID %>').val('');
            $('#<%=hdnListSupplierName.ClientID %>').val('');
            $('#<%=hdnListGCPurchaseUnit.ClientID %>').val('');
            $('#<%=hdnListPurchaseUnit.ClientID %>').val('');
            $('#<%=hdnPurchaseOrderQty.ClientID %>').val('');
            $('#<%=hdnDiscount1.ClientID %>').val('');
            $('#<%=hdnDiscount2.ClientID %>').val('');
            $('#<%=hdnPrice.ClientID %>').val('');
            $('#<%=hdnListTermID.ClientID %>').val('');
            $('#<%=hdnListIsUrgent.ClientID %>').val('');
        }

        $('.chkIsSelected').live('change', function () {
            $tr = $(this).closest('tr');
            $lblSupplier = $tr.find('.lblSupplier');
            $lblPurchaseUnit = $tr.find('.lblPurchaseUnit');
            if ($(this).find('input').is(':checked')) {
                $tr.find('.txtPrice').removeAttr('readonly');
                $tr.find('.txtDiscount1').removeAttr('readonly');
                $tr.find('.txtDiscount2').removeAttr('readonly');
                $tr.find('.txtPurchaseQty').removeAttr('readonly');
                $lblSupplier.removeClass('lblDisabled');
                $lblSupplier.addClass('lblLink');
                $lblPurchaseUnit.removeClass('lblDisabled');
                $lblPurchaseUnit.addClass('lblLink');
            }
            else {
                $tr.find('.txtPrice').attr('readonly', 'readonly');
                $tr.find('.txtDiscount1').attr('readonly', 'readonly');
                $tr.find('.txtDiscount2').attr('readonly', 'readonly');
                $tr.find('.txtPurchaseQty').attr('readonly', 'readonly');
                $lblSupplier.removeClass('lblLink');
                $lblSupplier.addClass('lblDisabled');
                $lblPurchaseUnit.removeClass('lblLink');
                $lblPurchaseUnit.addClass('lblDisabled');
            }
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live("click", function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });

            $('.containerPaging').hide();
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdPurchaseRequest tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdPurchaseRequest tr:eq(1)').click();

            $('.containerPaging').hide();
        }
        //#endregion

        $('.lblItemName.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $('#<%=hdnLocationIDFrom.ClientID %>').val() + '|' + $tr.find('.keyField').html();
            var url = ResolveUrl("~/Program/Procurement/PurchaseRequest/ItemDetailInfoCtl.ascx");
            openUserControlPopup(url, id, 'Informasi Persediaan', 800, 600);
        });

        $('.lblQuantity.lblLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var id = $('#<%=hdnPurchaseRequestID.ClientID %>').val() + '|' + $tr.find('.keyField').html();
            var url = ResolveUrl("~/Program/Procurement/PurchaseRequest/PurchaseRequestQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'Detil Permintaan Pembelian', 1200, 500);
        });

        $('.lblQtyOnOrder.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.keyField').html() + '|' + $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/PurchaseOrderQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'Outstanding Order', 800, 500);
        });

        //#region Supplier
        $td = null;
        $('.lblSupplier.lblLink').live('click', function () {
            $td = $(this).parent();
            $tr = $td.closest('tr');
            var itemID = $tr.find('.keyField').html();

            var filter = 'ItemID = ' + itemID + ' AND IsDeleted = 0';
            var filterExpression = 'IsBlackList = 0 AND IsDeleted = 0';
            var isUsingSupplierCatalog = 'false';

            Methods.getObject('GetItemPlanningList', filter, function (result) {
                if (result != null) {
                    isUsingSupplierCatalog = result.IsUsingSupplierCatalog.toString();
                }
            });

            if (isUsingSupplierCatalog == 'true') {
                filterExpression += ' AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM SupplierItem WHERE ' + filter + ')';
            }

            openSearchDialog('supplier', filterExpression, function (value) {
                onTxtSupplierChanged(value);
            });
        });

        function onTxtSupplierChanged(value) {
            var filterExpression = "BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvSupplierList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnSupplierID').val(result.BusinessPartnerID);
                    $td.find('.hdnTermID').val(result.TermID);
                    if (result.BusinessPartnerName != "")
                        $td.find('.lblSupplier').html(result.BusinessPartnerName);
                    else
                        $td.find('.lblSupplier').html(result.BusinessPartnerName);

                    var itemID = $td.parent().find('.hdnItemID').val();

                    var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                    var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                    var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                    var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                    if (hdnIM0131 == "0") {
                        Methods.getItemMasterPurchase(itemID, result.BusinessPartnerID, function (result2) {
                            if (result2 != null) {
                                var oConversionFactor = parseFloat($tr.find('.hdnConversionFactor').val());
                                if (oConversionFactor == 1) {

                                    $td.parent().find('.txtPrice').val(result2.Price);
                                } else {
                                    var price = parseFloat(result2.Price * oConversionFactor);
                                    $td.parent().find('.txtPrice').val(result2.UnitPrice).trigger('change');

                                }
                                $td.parent().find('.txtDiscount1').val(result2.Discount);
                                $td.parent().find('.txtDiscount2').val(result2.Discount2);
                            }
                            else {
                                $td.parent().find('.txtPrice').val('0');
                                $td.parent().find('.txtDiscount1').val('0');
                                $td.parent().find('.txtDiscount2').val('0');
                            }
                        });
                    }
                    else {
                        Methods.getItemMasterPurchaseWithDate(itemID, result.BusinessPartnerID, orderDateFormatString, function (result2) {
                            if (result2 != null) {
                                var oConversionFactor = parseFloat($tr.find('.hdnConversionFactor').val());
                                if (oConversionFactor == 1) {

                                    $td.parent().find('.txtPrice').val(result2.Price);
                                } else {
                                    var price = parseFloat(result2.Price * oConversionFactor);
                                    $td.parent().find('.txtPrice').val(result2.UnitPrice).trigger('change');

                                }
                                $td.parent().find('.txtDiscount1').val(result2.Discount);
                                $td.parent().find('.txtDiscount2').val(result2.Discount2);
                            }
                            else {
                                $td.parent().find('.txtPrice').val('0');
                                $td.parent().find('.txtDiscount1').val('0');
                                $td.parent().find('.txtDiscount2').val('0');
                            }
                        });
                    }
                }
                else {
                    $td.find('.hdnSupplierID').val('0');
                    $td.find('.hdnTermID').val('0');
                    //$td.find('.lblSupplier').html('');
                    //$td.parent().find('.tdSupplierItemName').html('');
                }
            });
        }
        //#endregion

        $('.txtPrice').live('change', function () {
            $(this).trigger('changeValue');
        });

        var itemID = 0;
        $('.lblPurchaseUnit.lblLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            $td = $(this).parent();
            itemID = $tr.find('.keyField').html();
            var supplierID = $tr.find('.hdnSupplierID').val();

            var filter = getPurchaseUnitFilterExpression();
            var filterExpressionIP = "ItemID = " + itemID + " AND IsDeleted = 0 AND IsUsingSupplierCatalog = 1";
            var isUsingSupplierCatalog = 0;
            Methods.getObject('GetItemPlanningList', filterExpressionIP, function (result) {
                if (result != null) {
                    isUsingSupplierCatalog = 1;
                }
            });

            var filterItem = "ItemID = " + itemID;
            var itemUnit = "";
            Methods.getObject('GetItemMasterList', filterItem, function (resultItem) {
                if (resultItem != null) {
                    itemUnit = resultItem.GCItemUnit;
                }
            });

            if (isUsingSupplierCatalog == 1) {
                var lstUnit = "";

                var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                if (hdnIM0131 == "0") {
                    Methods.getItemMasterPurchaseList(itemID, supplierID, function (result1) {
                        if (result1.length > 0) {
                            for (i = 0; i < result1.length; i++) {
                                if (lstUnit == "") {
                                    lstUnit = "'" + result1[i].PurchaseUnit + "'";
                                }
                                else {
                                    lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                                }
                            }
                        }
                    });
                }
                else {
                    Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDateFormatString, function (result1) {
                        if (result1.length > 0) {
                            for (i = 0; i < result1.length; i++) {
                                if (lstUnit == "") {
                                    lstUnit = "'" + result1[i].PurchaseUnit + "'";
                                }
                                else {
                                    lstUnit += ",'" + result1[i].PurchaseUnit + "'";
                                }
                            }
                        }
                    });
                }

                if (lstUnit == "") {
                    lstUnit = "'" + itemUnit + "'";
                }
                else {
                    lstUnit += ",'" + itemUnit + "'";
                }

                filter += " AND GCAlternateUnit IN (" + lstUnit + ")";
            }
            openSearchDialog('itemalternateitemunit', filter, function (value) {
                onTxtPurchaseUnitChanged(value);
            });
        });

        //#region Purchase Unit
        function getPurchaseUnitFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ItemID = " + itemID;
            return filterExpression;
        }

        function onTxtPurchaseUnitChanged(value) {
            var conversionOld = $tr.find('.hdnConversionFactor').val();
            var hdnGCPurchaseUnitOld = $tr.find('.hdnGCPurchaseUnit').val();

            var filterExpression = getPurchaseUnitFilterExpression() + " AND GCAlternateUnit = '" + value + "'";
            Methods.getObject('GetvItemAlternateItemUnitList', filterExpression, function (result) {
                $lblPurchaseUnitPrice = $td.parent().find('.lblPurchaseUnitPrice');
                if (result != null) {
                    if (hdnGCPurchaseUnitOld != result.GCAlternateUnit) {
                        var qtyBegin = parseFloat($tr.find('.txtPurchaseQty').val().replace(/,/g, ''));
                        var conversion = parseFloat(result.ConversionFactor);
                        var qtyEnd = 0;

                        var price = parseFloat($tr.find('.txtPrice').val().replace(/,/g, ''));
                        var totalPrice = 0;

                        var itemID = $tr.find('.hdnItemID').val();
                        var supplierID = $tr.find('.hdnSupplierID').val();
                        var isFound = 0;

                        var hdnIM0131 = $('#<%=hdnIM0131.ClientID %>').val();
                        var orderDate = $('#<%:txtItemOrderDate.ClientID %>').val();
                        var orderDateInDatePicker = Methods.getDatePickerDate(orderDate);
                        var orderDateFormatString = Methods.dateToString(orderDateInDatePicker);

                        if (hdnIM0131 == "0") {
                            Methods.getItemMasterPurchaseList(itemID, supplierID, function (result2) {
                                for (i = 0; i < result2.length; i++) {
                                    if (result2[i].ItemUnit == value) {
                                        totalPrice = result2[i].Price;
                                        conversion = 1;
                                        isFound = 1;
                                    } else if (result2[i].PurchaseUnit == value) {
                                        totalPrice = result2[i].UnitPrice;
                                        conversion = result2[i].ConversionFactor;
                                        isFound = 1;
                                    }
                                }
                            });
                        }
                        else {
                            Methods.getItemMasterPurchaseWithDateList(itemID, supplierID, orderDateFormatString, function (result2) {
                                for (i = 0; i < result2.length; i++) {
                                    if (result2[i].ItemUnit == value) {
                                        totalPrice = result2[i].Price;
                                        conversion = 1;
                                        isFound = 1;
                                    } else if (result2[i].PurchaseUnit == value) {
                                        totalPrice = result2[i].UnitPrice;
                                        conversion = result2[i].ConversionFactor;
                                        isFound = 1;
                                    }
                                }
                            });
                        }

                        $tr.find('.hdnGCPurchaseUnit').val(result.GCAlternateUnit);
                        $tr.find('.lblPurchaseUnit').html(result.AlternateUnit);
                        $tr.find('.hdnConversionFactor').val(result.ConversionFactor);
                        $lblPurchaseUnitPrice.html(result.AlternateUnit);

                        if (conversion == 1) {
                            conversion = conversionOld;
                            qtyEnd = parseFloat(qtyBegin) * parseFloat(conversion);
                        } else if (conversionOld != 1) {
                            qtyEnd = parseFloat(qtyBegin) * parseFloat(conversionOld / conversion);
                        } else {
                            conversion = result.ConversionFactor;
                            qtyEnd = parseFloat(qtyBegin) / parseFloat(conversion);
                        }

                        $tr.find('.txtPurchaseQty').val(qtyEnd.toFixed(2)).trigger('changeValue');

                        if (isFound == 1) {
                            $tr.find('.txtPrice').val(totalPrice).trigger('changeValue');
                        }
                        else {
                            $tr.find('.txtPrice').val(price * conversion).trigger('changeValue');
                        }
                    }
                }
                else {
                    $tr.find('.hdnGCPurchaseUnit').val('');
                    $tr.find('.lblPurchaseUnit').html('');
                    $tr.find('.hdnConversionFactor').val('');
                    $lblPurchaseUnitPrice.html('');
                }
            });
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            if (code == 'IM-00062') {
                getCheckedMember();
                $tr = $(this).closest('tr').parent().closest('tr');
                var purchaseRequestID = $('#<%=hdnPurchaseRequestID.ClientID %>').val() + ',' + $tr.find('.keyField').html();
                var itemID = $('#<%=hdnSelectedMember.ClientID %>').val().substring(1);
                if (itemID != '') {
                    filterExpression.text = "purchaseRequestID IN (" + purchaseRequestID + ") AND ItemID IN (" + itemID + ")";
                    return true;
                } else {
                    errMessage.text = "Silahkan Pilih Item Terlebih Dahulu";
                }
            } else {
                errMessage.text = "Silahkan Pilih Item Terlebih Dahulu";
                return false;
            }
        }

        $('.txtPurchaseQty').die('change');
        $('.txtPurchaseQty').live('change', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var qty = parseFloat($(this).val());

            var GCPurchaseUnitORI = $tr.find('.hdnGCPurchaseUnitORI').val();
            var conversionFactorORI = $tr.find('.hdnConversionFactorORI').val();
            var GCPurchaseUnit = $tr.find('.hdnGCPurchaseUnit').val();
            var conversionFactor = $tr.find('.hdnConversionFactor').val();
            var maxQty = parseFloat($tr.find('.hdnRemainingQtyBaseUnit').val());

            if (GCPurchaseUnitORI != GCPurchaseUnit) {
                maxQty = ((maxQty * conversionFactorORI) / conversionFactor);
            }

            if ($('#<%=hdnIsPOQtyCannotOverPRQty.ClientID %>').val() == "1") {
                if (qty > maxQty) {
                    $tr.find('.txtPurchaseQty').val(maxQty);
                }
                else if (qty < 0) {
                    $tr.find('.txtPurchaseQty').val(maxQty);
                }
            } else {
                $tr.find('.txtPurchaseQty').val(qty);
            }
        });

    </script>
    <input type="hidden" value="" id="hdnDefaultPurchaseOrderType" runat="server" />
    <input type="hidden" value="" id="hdnDefaultFrancoRegion" runat="server" />
    <input type="hidden" value="" id="hdnDefaultCurrencyCode" runat="server" />
    <input type="hidden" value="" id="hdnListConversionFactorORI" runat="server" />
    <input type="hidden" value="" id="hdnListConversionFactor" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseOrderQty" runat="server" />
    <input type="hidden" value="" id="hdnDiscount1" runat="server" />
    <input type="hidden" value="" id="hdnDiscount2" runat="server" />
    <input type="hidden" value="" id="hdnPrice" runat="server" />
    <input type="hidden" value="" id="hdnListSupplierID" runat="server" />
    <input type="hidden" value="" id="hdnListSupplierName" runat="server" />
    <input type="hidden" value="" id="hdnListGCPurchaseUnitORI" runat="server" />
    <input type="hidden" value="" id="hdnListGCPurchaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnListPurchaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseRequestID" runat="server" />
    <input type="hidden" value="" id="hdnListTermID" runat="server" />
    <input type="hidden" value="" id="hdnListIsUrgent" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnSortByQuantityEND" runat="server" />
    <input type="hidden" value="0" id="hdnOrderByLastValue" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowDeclinePROnOrdered" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedPurchaseOrderType" runat="server" />
    <input type="hidden" value="0" id="hdnIsPOQtyCannotOverPRQty" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseRequestListOrderBy" runat="server" />
    <input type="hidden" value="" id="hdnIsSortByItemAndPurchaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnIM0131" runat="server" />
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
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
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
                                        <col style="width: 120px" />
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
                        <tr id="trPurchaseOrderType" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Pemesanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPurchaseOrderType" value="" runat="server" />
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="100%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPurchaseRequestNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNotProceedToPOOnly" runat="server" Checked="false" Visible="false"
                                    Text="Belum diproses PO" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsSortByQtyOnHand" runat="server" Checked="false" Text="Urut berdasarkan Quantity On Hand" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pesan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdPurchaseRequest grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th rowspan="2" style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th rowspan="2" style="width: 119px" align="left">
                                                        <%=GetLabel("Nama Barang")%>
                                                    </th>
                                                    <th colspan="4" align="center">
                                                        <%=GetLabel("PERMINTAAN BELI")%>
                                                    </th>
                                                    <th colspan="7" align="center">
                                                        <%=GetLabel("INFORMASI PERSEDIAAN")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("SUPPLIER")%>
                                                    </th>
                                                    <th colspan="4" align="center">
                                                        <%=GetLabel("PROSES PEMESANAN")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 40px" align="left">
                                                        <%=GetLabel("Diminta")%>
                                                    </th>
                                                    <th style="width: 40px" align="left">
                                                        <%=GetLabel("Konversi")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Hand RS")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Hand")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Order")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Unit Request")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("Min")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("Max")%>
                                                    </th>
                                                    <th style="width: 40px" align="left">
                                                        <%=GetLabel("Satuan Kecil")%>
                                                    </th>
                                                    <th style="width: 100px" align="left">
                                                        <%=GetLabel("Nama")%>
                                                    </th>
                                                    <th style="width: 60px;" align="right">
                                                        <%=GetLabel("Pesan")%>
                                                    </th>
                                                    <th style="width: 80px" align="right">
                                                        <%=GetLabel("Harga Satuan Kecil")%>
                                                    </th>
                                                    <th style="width: 55px" align="right">
                                                        <%=GetLabel("Disc.1(%)")%>
                                                    </th>
                                                    <th style="width: 55px" align="right">
                                                        <%=GetLabel("Disc.2(%)")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="21">
                                                        <%=GetLabel("Tidak ada data permintaan pembelian")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPurchaseRequest grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                    </th>
                                                    <th rowspan="2" style="width: 20px" align="center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th rowspan="2" style="width: 130px" align="left">
                                                        <%=GetLabel("Nama Barang")%>
                                                    </th>
                                                    <th colspan="4" align="center">
                                                        <%=GetLabel("PERMINTAAN BELI")%>
                                                    </th>
                                                    <th colspan="7" align="center">
                                                        <%=GetLabel("INFORMASI PERSEDIAAN")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("SUPPLIER")%>
                                                    </th>
                                                    <th colspan="4" align="center">
                                                        <%=GetLabel("PROSES PEMESANAN")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 40px" align="center">
                                                        <%=GetLabel("Diminta")%>
                                                    </th>
                                                    <th style="width: 40px" align="center">
                                                        <%=GetLabel("Konversi")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Hand RS")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Hand")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Order")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("On Unit Request")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("Min")%>
                                                    </th>
                                                    <th style="width: 40px" align="right">
                                                        <%=GetLabel("Max")%>
                                                    </th>
                                                    <th style="width: 40px" align="left">
                                                        <%=GetLabel("Satuan Kecil")%>
                                                    </th>
                                                    <th style="width: 100px" align="left">
                                                        <%=GetLabel("Name")%>
                                                    </th>
                                                    <th style="width: 100px;" align="left">
                                                        <%=GetLabel("Pesan")%>
                                                    </th>
                                                    <th style="width: 80px" align="right">
                                                        <%=GetLabel("Harga Satuan")%>
                                                    </th>
                                                    <th style="width: 55px" align="right">
                                                        <%=GetLabel("Disc.1(%)")%>
                                                    </th>
                                                    <th style="width: 55px" align="right">
                                                        <%=GetLabel("Disc.2(%)")%>
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
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <div <%# Eval("IsUrgent").ToString() != "True" ? "Style='display:none'":"" %>>
                                                        <img src='<%# ResolveUrl("~/Libs/Images/Button/warning.png") %>' width="17" height="17"
                                                            alt="" visible="true" title='<%=GetLabel("Urgent") %>' />
                                                    </div>
                                                </td>
                                                <td class="tdItemName">
                                                    <label class="lblItemName lblLink">
                                                        <%#: Eval("ItemName1")%></label>
                                                    <div>
                                                        <%#: Eval("Remarks")%></div>
                                                </td>
                                                <td align="center">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 40px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td align="right">
                                                                <label class="lblQuantity lblLink">
                                                                    <%#: Eval("Quantity", "{0:N2}")%></label>
                                                            </td>
                                                            <td>
                                                                &nbsp<%#: Eval("PurchaseUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("CustomConversion")%>
                                                </td>
                                                <td align="right" class="lblReadOnlyText">
                                                    <%#: Eval("QtyOnHandAll")%>
                                                </td>
                                                <td align="right" class="lblReadOnlyText">
                                                    <%#: Eval("QuantityEND")%>
                                                </td>
                                                <td align="right" class="lblReadOnlyText">
                                                    <input type="hidden" class="hdnItemID" value='<%#:Eval("ItemID") %>' />
                                                    <input type="hidden" class="hdnItemName1" value='<%#:Eval("ItemName1") %>' />
                                                    <input type="hidden" class="hdnTermID" value='<%#:Eval("TermID") %>' />
                                                    <input type="hidden" class="hdnIsUrgent" value='<%#:Eval("IsUrgent") %>' />
                                                    <input type="hidden" class="hdnRemainingQtyBaseUnit" value='<%#:Eval("cfRemainingQtyBaseUnit") %>' />
                                                    <label class="lblQtyOnOrder lblLink">
                                                        <%#: Eval("QtyOnOrder", "{0:N2}")%></label>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("ItemRequestQtyOnOrder", "{0:N2}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("QuantityMIN", "{0:N2}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("QuantityMAX", "{0:N2}")%>
                                                </td>
                                                <td class="tdBaseUnit">
                                                    <%#: Eval("BaseUnit")%>
                                                </td>
                                                <td align="left">
                                                    <input type="hidden" value="0" class="hdnSupplierID" id="hdnSupplierID" runat="server" />
                                                    <label runat="server" id="lblSupplier" class="lblSupplier">
                                                    </label>
                                                </td>
                                                <td align="right">
                                                    <input type="hidden" value="0" class="hdnGCPurchaseUnitORI" id="hdnGCPurchaseUnitORI"
                                                        runat="server" />
                                                    <input type="hidden" value="0" class="hdnGCPurchaseUnit" id="hdnGCPurchaseUnit" runat="server" />
                                                    <input type="hidden" value="0" class="hdnConversionFactorORI" id="hdnConversionFactorORI"
                                                        runat="server" />
                                                    <input type="hidden" value="0" class="hdnConversionFactor" id="hdnConversionFactor"
                                                        runat="server" />
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 50px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPurchaseQty" Width="50px" runat="server" value="0" CssClass="number txtPurchaseQty"
                                                                    ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;<label runat="server" id="lblPurchaseUnit" class="lblPurchaseUnit"></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtPrice" Width="75px" runat="server" value="0" CssClass="txtCurrency txtPrice"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtDiscount1" Width="75%" runat="server" value="0" CssClass="number txtDiscount1"
                                                        ReadOnly="true" />
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtDiscount2" Width="75%" runat="server" value="0.00" CssClass="number txtDiscount2"
                                                        ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging" style="display: none">
        <div class="wrapperPaging">
            <div id="paging">
            </div>
        </div>
    </div>
</asp:Content>
