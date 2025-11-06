<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="LocationItemList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.LocationItemList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <div style="height: 50px">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Location
            function onGetLocationFilterExpression() {
                var filterExpression = "<%:OnGetLocationFilterExpression() %>";
                return filterExpression;
            }

            $('#lblLocation.lblLink').click(function () {
                openSearchDialog('locationroleuser', onGetLocationFilterExpression(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').change(function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = onGetLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                            $('#<%=hdnGCLocationGroup.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationID.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                        $('#<%=hdnGCLocationGroup.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion

            //#region Bin Location
            function onGetBinLocationFilterExpression() {
                var filterExpression = "LocationID = '" + $('#<%=hdnLocationID.ClientID %>').val() + "' AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblBinLocation.lblLink').click(function () {
                openSearchDialog('binlocation', onGetBinLocationFilterExpression(), function (value) {
                    $('#<%=txtBinLocationCode.ClientID %>').val(value);
                    onTxtBinLocationCodeChanged(value);
                });
            });

            $('#<%=txtBinLocationCode.ClientID %>').change(function () {
                onTxtBinLocationCodeChanged($(this).val());
            });

            function onTxtBinLocationCodeChanged(value) {
                var filterExpression = onGetBinLocationFilterExpression() + " AND BinLocationCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetBinLocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBinLocationID.ClientID %>').val(result.BinLocationID);
                        $('#<%=txtBinLocationName.ClientID %>').val(result.BinLocationName);
                    }
                    else {
                        $('#<%=hdnBinLocationID.ClientID %>').val('');
                        $('#<%=txtBinLocationCode.ClientID %>').val('');
                        $('#<%=txtBinLocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "<%:OnGetItemGroupFilterExpression() %>";
                return filterExpression;
            }

            $('#lblItemGroup.lblLink').click(function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
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
                });
            }
            //#endregion

            $('#btnCancel').click(function () {
                $('#containerPopupEntryData').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
                    cbpView.PerformCallback('save');
                return false;
            });

            $('#lblAddData').click(function () {
                $('#<%=hdnID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=hdnBinLocationID.ClientID %>').val('');
                $('#<%=txtBinLocationCode.ClientID %>').val('');
                $('#<%=txtBinLocationName.ClientID %>').val('');
                $('#<%=txtMinimum.ClientID %>').val('0');
                $('#<%=txtMaximum.ClientID %>').val('0');
                $('#<%=txtSatuanKecilMin.ClientID %>').val('');
                $('#<%=txtSatuanKecilMax.ClientID %>').val('');
                $('#<%=txtRemarks.ClientID %>').val('');
                cboTransactionType.SetValue(Constant.ItemRequestType.DISTRIBUTION);

                $('#containerPopupEntryData').show();
            });

            $('.lblExpiredDate.lblLink').live('click', function () {
                showLoadingPanel();
                var url = ResolveUrl('~/Program/Master/Location/LocationItemExpiredDateCtl.ascx');

                $row = $(this).closest('tr');
                var ID = $row.find('.hdnID').val();
                var itemID = $row.find('.hdnItemID').val();
                var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                var id = ID + '|' + locationID + '|' + itemID;
                openUserControlPopup(url, id, 'Expired Date', 700, 500);
            });

            $('#lblQuickPick').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/Master/Location/LocationItemQuickPicksCtl.ascx');
                    var locationID = $('#<%=hdnLocationID.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationItemGroupID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnGCLocationGroup.ClientID %>').val();
                    var id = locationID + '|' + locationItemGroupID + '|' + GCLocationGroup;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
                }
            });
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);
            var oLocationName = $row.find('.LocationName').val();
            var oItemName1 = $row.find('.hdnItemName1').val();
            var oCountAllMovement = parseInt($row.find('.CountAllMovement').val());

            if (oCountAllMovement > 0) {
                var messageBeforeDeleteBalance = "Item <b>" + oItemName1 + "</b> di lokasi <b>" + oLocationName + "</b> sudah pernah <b>digunakan dalam transaksi (pasien maupun inventory)</b>, lanjutkan hapus dari daftar ini?";
                displayConfirmationMessageBox("Konfirmasi", messageBeforeDeleteBalance, function (result) {
                    if (result) {
                        cbpView.PerformCallback('delete');
                    }
                });
            } else {
                var messageBeforeDeleteBalance = "Item " + oItemName1 + " di lokasi " + oLocationName + ", lanjutkan hapus dari daftar ini?";
                displayConfirmationMessageBox("Konfirmasi", messageBeforeDeleteBalance, function (result) {
                    if (result) {
                        cbpView.PerformCallback('delete');
                    }
                });
            }
        });

        function onAfterPopupControlClosing() {
            onRefreshGrid();
        }

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var ID = $row.find('.hdnID').val();
            var itemID = $row.find('.hdnItemID').val();
            var itemCode = $row.find('.hdnItemCode').val();
            var remarks = $row.find('.hdnRemarks').val();
            var binLocationID = $row.find('.hdnBinLocationID').val();
            var binLocationCode = $row.find('.hdnBinLocationCode').val();
            var binLocationName = $row.find('.hdnBinLocationName').val();
            var itemName = $row.find('.tdItemName1').html().trim();
            var itemUnit = $row.find('.tdItemUnit').html().trim();
            var minimum = $row.find('.tdMinimum').html().trim();
            var maximum = $row.find('.tdMaximum').html().trim();
            var beginningBalance = $row.find('.tdBeginningBalance').html().trim();
            var qtyIn = $row.find('.tdQtyIn').html().trim();
            var qtyOut = $row.find('.tdQtyOut').html().trim();
            var endingBalance = $row.find('.tdEndingBalance').html().trim();
            var gcItemRequestTypeItemProduct = $row.find('.GCItemRequestTypeItemProduct').val();
            var gcItemRequest = $row.find('.GCItemRequestType').val();
            $('#<%=hdnID.ClientID %>').val(ID);
            $('#<%=hdnItemID.ClientID %>').val(itemID);
            $('#<%=txtItemCode.ClientID %>').val(itemCode);
            $('#<%=txtItemName.ClientID %>').val(itemName);
            $('#<%=hdnBinLocationID.ClientID %>').val(binLocationID);
            $('#<%=txtBinLocationCode.ClientID %>').val(binLocationCode);
            $('#<%=txtBinLocationName.ClientID %>').val(binLocationName);
            $('#<%=txtMinimum.ClientID %>').val(minimum);
            $('#<%=txtMaximum.ClientID %>').val(maximum);
            $('#<%=txtSatuanKecilMin.ClientID %>').val(itemUnit);
            $('#<%=txtSatuanKecilMax.ClientID %>').val(itemUnit);
            $('#<%=txtRemarks.ClientID %>').val(remarks);
            if (gcItemRequest != "") {
                cboTransactionType.SetValue(gcItemRequest);
            } else {
                cboTransactionType.SetValue(gcItemRequestTypeItemProduct);
            }

            var isAllowEditMinMax = $('#<%=hdnIsAllowEditMinMax.ClientID %>').val();
            if (isAllowEditMinMax == "1") {
                $('#<%=txtMinimum.ClientID %>').removeAttr('readonly');
                $('#<%=txtMaximum.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtMinimum.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtMaximum.ClientID %>').attr('readonly', 'readonly');
            }

            $('#containerPopupEntryData').show();
        });

        //#region Item Group
        function onGetItemGroupFilterExpression() {
            var filterExpression = "<%:filterExpressionItemProduct %>";
            if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '')
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
            return filterExpression;
        }
        //#endregion

        //#region Item
        function onGetLocationItemProductFilterExpression() {
            //            var filterExpression = "<%:OnGetItemProductFilterExpression() %> AND ItemID NOT IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND IsDeleted = 0)";
            var filterExpression = "IsDeleted = 0 AND ItemID NOT IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND IsDeleted = 0)";
            if ($('#<%=hdnLocationItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationItemGroupID.ClientID %>').val() != null)
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationItemGroupID.ClientID %>').val() + "/%')";
            return filterExpression;
        }

        $('#lblItem.lblLink').die('click');
        $('#lblItem.lblLink').live('click', function () {
            var filterExpressionItem = onGetLocationItemProductFilterExpression();
            if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == Constant.ItemTypeLocation.DRUG_SUPPLIES) {
                filterExpressionItem += " AND GCItemType IN ('" + Constant.ItemGroupMaster.DRUGS + "','" + Constant.ItemGroupMaster.SUPPLIES + "')";
            } else if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == Constant.ItemTypeLocation.LOGISTICS) {
                filterExpressionItem += " AND GCItemType ='" + Constant.ItemGroupMaster.LOGISTIC + "'";
            } else if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == '' || $('#<%=hdnGCLocationGroup.ClientID %>').val() == null) {
                filterExpressionItem += " AND GCItemType IN ('" + Constant.ItemGroupMaster.DRUGS + "','" + Constant.ItemGroupMaster.SUPPLIES + "','" + Constant.ItemGroupMaster.LOGISTIC + "','" + Constant.ItemGroupMaster.FOOD + "')";
            }
            openSearchDialog('item', filterExpressionItem, function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').die('change');
        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemCodeChanged($(this).val());
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = onGetLocationItemProductFilterExpression() + " AND ItemCode = '" + value + "'";
            if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == Constant.ItemTypeLocation.DRUG_SUPPLIES) {
                filterExpression += " AND GCItemType IN ('" + Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS + "')";
            } else if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == Constant.ItemTypeLocation.LOGISTICS) {
                filterExpression += " AND GCItemType ='" + Constant.ItemType.BARANG_UMUM + "'";
            } else if ($('#<%=hdnGCLocationGroup.ClientID %>').val() == '' || $('#<%=hdnGCLocationGroup.ClientID %>').val() == null) {
                filterExpression += " AND GCItemType IN ('" + Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS + "','" + Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN + "')";
            }
            Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=txtSatuanKecilMin.ClientID %>').val(result.ItemUnit);
                    $('#<%=txtSatuanKecilMax.ClientID %>').val(result.ItemUnit);

                    var filterItemProduct = "ItemID = " + result.ItemID;
                    Methods.getObject('GetItemProductList', filterItemProduct, function (result) {
                        if (result != null) {
                            cboTransactionType.SetValue(result.GCItemRequestType);
                        }
                    });
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtSatuanKecilMin.ClientID %>').val('');
                    $('#<%=txtSatuanKecilMax.ClientID %>').val('');
                    $('#<%=txtRemarks.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPagingDetailItem(pageCount);
        });

        function setPagingDetailItem(pageCount) {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, 8);
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#lblAddData').click();
                    var pageCount = parseInt(param[2]);
                    setPagingDetailItem(pageCount);
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    var pageCount = parseInt(param[2]);
                    setPagingDetailItem(pageCount);
                }
            }
            else if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
            }
            hideLoadingPanel();
        }
        //#endregion

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnGCLocationGroup" runat="server" />
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <input type="hidden" value="" id="hdnIsAllowEditMinMax" runat="server" />
        <table width="100%">
            <colgroup>
                <col width="120px" />
                <col />
            </colgroup>
            <tr>
                <td>
                    <label class="lblNormal lblLink" id="lblLocation">
                        <%=GetLabel("Lokasi") %></label>
                </td>
                <td>
                    <input type="hidden" id="hdnLocationID" runat="server" />
                    <input type="hidden" id="hdnLocationItemGroupID" runat="server" />
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col width="120px" />
                            <col width="3px" />
                            <col width="350px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtLocationCode" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLocationName" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblItemGroup">
                        <%=GetLabel("Kelompok Barang")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col width="120px" />
                            <col width="3px" />
                            <col width="350px" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="470px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                            <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%" cellpadding="0" cellspacing="1">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <table class="tblEntryContent" style="width: 100%">
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory lblLink" id="lblItem">
                                                        <%=GetLabel("Item")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" runat="server" id="hdnItemID" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="99%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblBinLocation">
                                                        <%=GetLabel("Rak")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" runat="server" id="hdnBinLocationID" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBinLocationCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBinLocationName" ReadOnly="true" Width="99%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Reorder Minimum")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtMinimum" CssClass="number required" runat="server" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSatuanKecilMin" ReadOnly="true" Width="50%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Reorder Maximum")%></label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtMaximum" CssClass="number required" runat="server" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSatuanKecilMax" ReadOnly="true" Width="50%" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table class="tblEntryContent" style="width: 100%">
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Tipe Transaksi")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboTransactionType" ClientInstanceName="cboTransactionType"
                                                        Width="130px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 3px" valign="top">
                                                    <label class="lblNormal" id="lblRemarks">
                                                        <%=GetLabel("Remarks")%></label>
                                                </td>
                                                <td rowspan="3" valign="top">
                                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Width="350px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
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
                        </fieldset>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                    position: relative; font-size: 0.95em;">
                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                        <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                cellspacing="0" rules="all">
                                <tr>
                                    <th style="width: 70px" rowspan="2">
                                        &nbsp;
                                    </th>
                                    <th style="width: 90px" rowspan="2" align="Left">
                                        <%=GetLabel("KODE BARANG")%>
                                    </th>
                                    <th rowspan="2" align="Left">
                                        <%=GetLabel("NAMA BARANG")%>
                                    </th>
                                    <th rowspan="2" align="center">
                                        <%=GetLabel("SATUAN KECIL")%>
                                    </th>
                                    <th colspan="2" align="center">
                                        <%=GetLabel("REORDER POINT")%>
                                    </th>
                                    <th colspan="4" align="center">
                                        <%=GetLabel("BALANCE")%>
                                    </th>
                                    <th rowspan="2" style="width: 100px">
                                        <%=GetLabel("EXPIRED DATE")%>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Minimum")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Maximum")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Beginning")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("In")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Out")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Ending")%>
                                    </th>
                                </tr>
                                <tr class="trEmpty">
                                    <td colspan="11">
                                        <%=GetLabel("Belum ada barang yang didaftarkan di lokasi persediaan ini")%>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblView" runat="server" class="grdView notAllowSelect grdItemBalance"
                                cellspacing="0" rules="all">
                                <tr>
                                    <th style="width: 70px" rowspan="2">
                                        &nbsp;
                                    </th>
                                    <th style="width: 90px" rowspan="2" align="Left">
                                        <%=GetLabel("KODE BARANG")%>
                                    </th>
                                    <th rowspan="2" align="left">
                                        <%=GetLabel("NAMA BARANG")%>
                                    </th>
                                    <th rowspan="2" align="center">
                                        <%=GetLabel("SATUAN KECIL")%>
                                    </th>
                                    <th colspan="2" align="center">
                                        <%=GetLabel("REORDER POINT")%>
                                    </th>
                                    <th colspan="4" align="center">
                                        <%=GetLabel("BALANCE")%>
                                    </th>
                                    <th rowspan="2" style="width: 100px">
                                        <%=GetLabel("EXPIRED DATE")%>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Minimum")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Maximum")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Beginning")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("In")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Out")%>
                                    </th>
                                    <th style="width: 90px" align="center">
                                        <%=GetLabel("Ending")%>
                                    </th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="center">
                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                        alt="" style="float: left; margin-left: 7px" />
                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                        alt="" style="margin-left: 2px" />
                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                    <input type="hidden" class="LocationName" value="<%#: Eval("LocationName")%>" />
                                    <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                    <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                    <input type="hidden" class="hdnItemName1" value="<%#: Eval("ItemName1")%>" />
                                    <input type="hidden" class="hdnEndingBalance" value="<%#: Eval("QuantityEND")%>" />
                                    <input type="hidden" class="hdnRemarks" value="<%#: Eval("Remarks")%>" />
                                    <input type="hidden" class="hdnBinLocationID" value="<%#: Eval("BinLocationID")%>" />
                                    <input type="hidden" class="hdnBinLocationCode" value="<%#: Eval("BinLocationCode")%>" />
                                    <input type="hidden" class="hdnBinLocationName" value="<%#: Eval("BinLocationName")%>" />
                                    <input type="hidden" class="GCItemRequestTypeItemProduct" value="<%#: Eval("GCItemRequestTypeItemProduct")%>" />
                                    <input type="hidden" class="GCItemRequestType" value="<%#: Eval("GCItemRequestType")%>" />
                                    <input type="hidden" class="CountAllMovement" value="<%#: Eval("CountAllMovement")%>" />
                                </td>
                                <td class="tdItemCode">
                                    <%#: Eval("ItemCode")%>
                                </td>
                                <td class="tdItemName1">
                                    <%#: Eval("ItemName1")%>
                                </td>
                                <td class="tdItemUnit" align="center">
                                    <%#: Eval("ItemUnit")%>
                                </td>
                                <td class="tdMinimum" align="right" style="font-weight: bold">
                                    <%#: Eval("CustomMinimum2")%>
                                </td>
                                <td class="tdMaximum" align="right" style="font-weight: bold">
                                    <%#: Eval("CustomMaximum2")%>
                                </td>
                                <td class="tdBeginningBalance" align="right">
                                    <%#: Eval("CustomBEGIN2")%>
                                </td>
                                <td class="tdQtyIn" align="right">
                                    <%#: Eval("CustomIN2")%>
                                </td>
                                <td class="tdQtyOut" align="right">
                                    <%#: Eval("CustomOUT2")%>
                                </td>
                                <td class="tdEndingBalance" align="right">
                                    <%#: Eval("CustomEndingBalance2")%>
                                </td>
                                <td align="center">
                                    <label id="lblExpiredDate" runat="server" class="lblExpiredDate lblLink">
                                        <%=GetLabel("Expired Date") %></label>
                                </td>
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
    <div style="width: 100%; text-align: center">
        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
            <%= GetLabel("Add Data")%></span> <span class="lblLink" id="lblQuickPick">
                <%= GetLabel("Quick Picks")%></span>
    </div>
</asp:Content>
