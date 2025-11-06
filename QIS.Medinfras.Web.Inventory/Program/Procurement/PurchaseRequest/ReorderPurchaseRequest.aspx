<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ReorderPurchaseRequest.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ReorderPurchaseRequest" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnReorderPurchaseRequestProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnReorderPurchaseAll" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process All")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPurchaseRequestDate.ClientID %>');
            $('#<%=txtPurchaseRequestDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=btnRefresh.ClientID %>').click(function () {
                var isUsedPurchaseOrderType = $('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val();
                if (isUsedPurchaseOrderType == 1) {
                    var checkPOType = cboPurchaseOrderType.GetValue();
                    if (checkPOType != null) {
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
                    } else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Jenis Permintaan terlebih dahulu.");
                    }
                } else {
                    cbpView.PerformCallback('refresh');
                }
            });

            $('#<%=btnReorderPurchaseRequestProcess.ClientID %>').click(function () {
                var isUsedPurchaseOrderType = $('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val();
                if (isUsedPurchaseOrderType == 1) {
                    var checkPOType = cboPurchaseOrderType.GetValue();
                    if (checkPOType != null) {
                        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                            getCheckedMember();
                            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                                showToast('Warning', 'Please Select Item First');
                            }
                            else {
                                onCustomButtonClick('process');
                                $('#<%=hdnListSupplierID.ClientID %>').val('');
                            }
                        }
                        else {
                            displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Jenis Permintaan terlebih dahulu.");
                        }
                    }
                } else {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        getCheckedMember();
                        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                            showToast('Warning', 'Please Select Item First');
                        }
                        else {
                            onCustomButtonClick('process');
                            $('#<%=hdnListSupplierID.ClientID %>').val('');
                        }
                    }
                }
            });

            $('#<%=btnReorderPurchaseAll.ClientID %>').click(function () {
                var isUsedPurchaseOrderType = $('#<%=hdnIsUsedPurchaseOrderType.ClientID %>').val();
                if (isUsedPurchaseOrderType == 1) {
                    var checkPOType = cboPurchaseOrderType.GetValue();
                    if (checkPOType != null) {
                        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                            getCheckedMember();
                            var message = "Apakah anda yakin ingin memproses semua barang?";
                            showToastConfirmation(message, function (result) {
                                if (result) onCustomButtonClick('processall');
                            });
                        }
                    } else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Jenis Permintaan terlebih dahulu.");
                    }
                } else {
                    if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                        getCheckedMember();
                        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                            showToast('Warning', 'Please Select Item First');
                        }
                        else {
                            onCustomButtonClick('processall');
                        }
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

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                    $('#<%=hdnListSupplierID.ClientID %>').val('');
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
                    //                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion
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
            if ($(this).is(':checked')) {
                $tr.find('.txtPurchaseRequest').removeAttr('readonly');
                $tr.find('.txtPurchaseRequest').focus();
            }
            else {
                $tr.find('.txtPurchaseRequest').attr('readonly', 'readonly');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', 'Permintaan Pembelian Berhasil Dibuat Dengan No Permintaan <b>' + retval + '</b>', function () {
                $('#<%=hdnPurchaseRequest.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                $('#<%=hdnRecommendedQty.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            var lstPurchaseRequest = $('#<%=hdnPurchaseRequest.ClientID %>').val().split('|');
            var lstRecommendedQty = $('#<%=hdnRecommendedQty.ClientID %>').val().split('|');
            var lstSupplierID = $('#<%=hdnListSupplierID.ClientID %>').val().split('|');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var purchaseRequest = $(this).closest('tr').find('.txtPurchaseRequest').val();
                    var recommendQty = $(this).closest('tr').find('.txtRecommendedQty').val();
                    var supplierID = $(this).closest('tr').find('.hdnSupplierID').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstPurchaseRequest.push(purchaseRequest);
                        lstRecommendedQty.push(recommendQty);
                        lstSupplierID.push(supplierID);
                    }
                    else {
                        lstPurchaseRequest[idx] = purchaseRequest;
                        lstRecommendedQty[idx] = recommendQty;
                        lstSupplierID[idx] = supplierID;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var purchaseRequest = $(this).closest('tr').find('.txtPurchaseRequest').val();
                    var recommendQty = $(this).closest('tr').find('.txtRecommendedQty').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstPurchaseRequest.splice(idx, 1);
                        lstRecommendedQty.splice(idx, 1);
                        lstSupplierID.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnPurchaseRequest.ClientID %>').val(lstPurchaseRequest.join('|'));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
            $('#<%=hdnRecommendedQty.ClientID %>').val(lstRecommendedQty.join('|'));
            $('#<%=hdnListSupplierID.ClientID %>').val(lstSupplierID.join('|'));
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

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
                    $td.find('.lblSupplier').html(result.BusinessPartnerName);
                }
                else {
                    $td.find('.hdnSupplierID').val('0');
                    $td.find('.lblSupplier').html('');
                }
            });
        }
        //#endregion

        //#region SupplierHeader
        function getSupplierFilterExpression() {
            var filterExpression = "<%:filterExpressionSupplier %>";
            return filterExpression;
        }

        $('#<%=lblSupplierHeader.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                onTxtSupplierHeaderChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            onTxtSupplierHeaderChanged($(this).val());
        });

        function onTxtSupplierHeaderChanged(value) {
            var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
                $('#<%=hdnListSupplierID.ClientID %>').val('');
            });
        }
        //#endregion

        $('#<%:chkTanpaSupplier.ClientID %>').live('change', function () {
            $chkTanpaSupplier = $('#<%:chkTanpaSupplier.ClientID %>');
            if ($(this).is(':checked')) {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                $('#<%=txtBusinessPartnerName.ClientID %>').val('');

                $('#<%:trSupplier.ClientID %>').attr('style', 'display:none');                
            }
            else {
                $('#<%:trSupplier.ClientID %>').removeAttr('style');
            }
        });

        $('#<%=rblDisplayOption.ClientID %>').live('change', function () {
            var displayOption = $('#<%=rblDisplayOption.ClientID %>').find(":checked").val();
            $('#<%=hdnDisplayOption.ClientID %>').val(displayOption);
        });

        $('.lblPurchaseRequestQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/ItemPurchaseRequestQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Purchase Request - Detail', 800, 500);
        });

        $('.lblQtyOnHandAll.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/ItemOnHandAllQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Hand All', 800, 500);
        });

        $('.lblPurchaseOrderQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var itemID = $tr.find('.hiddenColumn').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/PurchaseOrderQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Purchase Order - Detail', 800, 500);
        });

        $('.txtPurchaseRequest').live('change', function () {
            $tr = $(this).closest('tr').closest('tr');
            var qtyMax = $tr.find('.hdnQtyMax').html();
            var hdnSetVarAllowMax = $('#<%=hdnQtyPRAllowBiggerThanQtyMax.ClientID %>').val();
            if (hdnSetVarAllowMax == "0") {
                $tr.find('.txtPurchaseRequest').attr('max', qtyMax);
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai maksimal sejumlah ' + qtyMax);
                $tr.find('.txtPurchaseRequest').val(qtyMax);
            } else {
                $tr.find('.txtPurchaseRequest').removeAttr('max');
            }

            var qtyRecommed = $tr.find('.hdnRecommendQtyPR').val();
            var hdnSetVarAllowRecommend = $('#<%=hdnQtyPRAllowBiggerThanReccomend.ClientID %>').val();
            if (hdnSetVarAllowRecommend == "0") {
                $tr.find('.txtPurchaseRequest').attr('max', qtyRecommed);
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai rekomendasi sistem sejumlah ' + qtyRecommed);
                $tr.find('.txtPurchaseRequest').val(qtyRecommed);
            } else {
                $tr.find('.txtPurchaseRequest').removeAttr('max');
            }
        });
    </script>
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseRequest" runat="server" value="" />
    <input type="hidden" id="hdnRecommendedQty" runat="server" value="" />
    <input type="hidden" value="" id="hdnListSupplierID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="1" id="hdnDisplayOption" runat="server" />
    <input type="hidden" value="0" id="hdnSortByQuantityEND" runat="server" />
    <input type="hidden" value="0" id="hdnAutoApprovePR" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="1" id="hdnIsOutstandingPOPRVisible" runat="server" />
    <input type="hidden" value="1" id="hdnQtyPRAllowBiggerThanQtyMax" runat="server" />
    <input type="hidden" value="1" id="hdnQtyPRAllowBiggerThanReccomend" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedPurchaseOrderType" runat="server" />
    <input type="hidden" value="0" id="hdnIM0131" runat="server" />
    <div style="overflow-x: hidden;">
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
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
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
                        <tr id="trPurchaseOrderType" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Permintaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPurchaseOrderType" ClientInstanceName="cboPurchaseOrderType"
                                    Width="45%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
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
                                            <asp:TextBox ID="txtPurchaseRequestDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseRequestTime" Width="60px" CssClass="time" runat="server"
                                                Style="text-align: center" />
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
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr id="trSupplier" runat="server">
                            <td>
                                <label class="lblLink" runat="server" id="lblSupplierHeader">
                                    <%=GetLabel("Supplier")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
                                <input type="hidden" id="hdnBusinessPartnerName" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkTanpaSupplier" Text="Tanpa Supplier" Checked="false"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trItemType" runat="server" style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Item")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Semua" Value="0" Selected="True" />
                                    <asp:ListItem Text="Obat" Value="2" />
                                    <asp:ListItem Text="Alkes" Value="3" />
                                    <asp:ListItem Text="Barang Umum" Value="8" />
                                </asp:RadioButtonList>
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
                                <%=GetLabel("Display Option")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblDisplayOption" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Semua" Value="0" />
                                    <asp:ListItem Text="Rekomendasi Sistem" Value="1" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 20%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel" style="font-style: oblique; color: Red">
                                                        <%=GetLabel("By Qty")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblByQty" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="MIN" Value="min" />
                                                            <asp:ListItem Text="MAX" Value="max" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel" style="font-style: oblique; color: Red">
                                                        <%=GetLabel("By Location")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblByLocation" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="On Hand" Value="0" />
                                                            <asp:ListItem Text="On Hand ALL" Value="1" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%#:Eval("ItemName1")%>
                                                    <br>
                                                    <font size="1"><i>
                                                        <%#:Eval("ItemCode")%></i></font>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomMinimum2" HeaderText="Minimum" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomMaximum2" HeaderText="Maximum" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="hdnQtyMax"
                                                ItemStyle-CssClass="hdnQtyMax" />
                                            <asp:BoundField DataField="CustomAverageQtyInString" HeaderText="Average Usage" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomEndingBalance2" HeaderText="On HAND Location" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="On HAND ALL" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <label class="lblQtyOnHandAll lblLink">
                                                        <%#:Eval("CustomQtyOnHandAll", "{0:N2}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Purchase Request" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseRequestQty lblLink">
                                                        <%#:Eval("CustomPurchaseRequestQtyOnOrder2", "{0:N2}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Purchase Order" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseOrderQty lblLink">
                                                        <%#:Eval("CustomQtyOnOrderPurchaseOrder2", "{0:N2}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Reorder Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRecommendedQty" Width="60px" runat="server" CssClass="number txtRecommendedQty"
                                                        Min="0" ReadOnly="true" Text='<%#: Eval("CustomRecommendedQty")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="135px" ItemStyle-HorizontalAlign="Left" HeaderText="DIMINTA (PROSES PR)">
                                                <ItemTemplate>
                                                    <input type="hidden" value="0" class="hdnRecommendQtyPR" id="hdnRecommendQtyPR" runat="server" />
                                                    <asp:TextBox ID="txtPurchaseRequest" Width="45%" runat="server" CssClass="number txtPurchaseRequest"
                                                        ReadOnly="true" />
                                                    &nbsp;
                                                    <%#: Eval("PurchaseUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SUPPLIER" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <input type="hidden" value="0" class="hdnSupplierID" id="hdnSupplierID" runat="server" />
                                                    <label runat="server" id="lblSupplier" class="lblSupplier lblLink">
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomConversionFactor" HeaderText="Faktor Konversi" HeaderStyle-Width="145px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data Reorder Permintaan Pembelian untuk Lokasi ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <%--                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
