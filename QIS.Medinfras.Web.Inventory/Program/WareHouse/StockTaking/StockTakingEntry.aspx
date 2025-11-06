<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="StockTakingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.StockTakingEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();

            if ($('#<%=txtFormDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtFormDate.ClientID %>');
                $('#<%=txtFormDate.ClientID %>').datepicker('option', 'minDate', '0');
            }

            $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            });

            //#region Stock Taking No
            $('#lblStockTakingNo.lblLink').click(function () {
                openSearchDialog('stocktakinghd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtStockTakingNo.ClientID %>').val(value);
                    onTxtStockTakingNoChanged(value);
                });
            });

            $('#<%=txtStockTakingNo.ClientID %>').change(function () {
                onTxtStockTakingNoChanged($(this).val());
            });

            function onTxtStockTakingNoChanged(value) {
                onLoadObject(value);
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

            $('#<%=btnStartCalculate.ClientID %>').click(function () {
                cbpProcess.PerformCallback('calculate');
            });

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'calculate') {
                if (param[1] == 'fail')
                    showToast('Proses Hitung Gagal', 'Error Message : ' + param[2]);
                else {
                    $('#<%=btnStartCalculate.ClientID %>').attr('enabled', 'false');
                    cbpView.PerformCallback('refresh');
                }
            }
            else {
                var result = s.cpResult.split('|');
                if (result[1] == 'success') {
                    $tr = $btnSave.closest('tr');
                    $btnSave.attr('enabled', 'false');
                    cboCheckCountType.SetEnabled(false);
                    $lblExpiredDate.attr('class', 'lblExpiredDate lblDisabled');
                }
                else {
                    if (result[2] != '')
                        showToast('Save Failed', 'Error Message : ' + result[2]);
                    else
                        showToast('Save Failed', '');
                }
            }
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (transactionStatus != 'X121^001') {
                $('#<%=btnVoidByReason.ClientID %>').hide();
            }
            else if (transactionStatus == 'X121^001') {
                if (isVoid == 1) {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                }
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }

            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });
        }
        //#endregion

        //#region Product Line
        function getProductLineFilterExpression() {
            var itemType = cboItemType.GetValue();
            var filterExpression = "IsDeleted = 0";

            if (itemType != null) {
                filterExpression += " AND GCItemType = '" + itemType + "'";
            }

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

        function onAfterSaveAddRecord(param) {
            $('#<%=btnStartCalculate.ClientID %>').removeAttr('enabled');
        }

        $('.txtAdjustment').live('change', function () {
            var qtyBSO = parseFloat($(this).parent().find('.hdnQuantityBSO').val());
            var adjustment = parseFloat($(this).val());
            $tr = $(this).closest('tr');
            $tr.find('.txtQuantityEND').val(qtyBSO + adjustment).trigger('changeValue');
            $tr.find('.btnSave').removeAttr('enabled');
            var idx = $tr.find('.hdnItemIndex').val();
            cboCheckCountType = eval('cboCheckCountType' + idx);
            cboCheckCountType.SetEnabled(true);
            if (cboCheckCountType.GetValue() == null)
                cboCheckCountType.SetValue($('#<%=hdnDefaultCycleCountType.ClientID %>').val());
        });

        $('.txtQuantityEND').live('change', function () {
            $tr = $(this).closest('tr');
            var qtyBSO = parseFloat($tr.find('.hdnQuantityBSO').val());
            var qtyEnd = parseFloat($(this).val());
            $tr.find('.txtAdjustment').val(qtyEnd - qtyBSO).trigger('changeValue');
            $tr.find('.btnSave').removeAttr('enabled');
            var idx = $tr.find('.hdnItemIndex').val();
            cboCheckCountType = eval('cboCheckCountType' + idx);
            cboCheckCountType.SetEnabled(true);
            if (cboCheckCountType.GetValue() == null)
                cboCheckCountType.SetValue($('#<%=hdnDefaultCycleCountType.ClientID %>').val());
        });


        var cboCheckCountType = null;
        $btnSave = null;
        $('.btnSave').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                $tr = $(this).closest('tr');
                var itemID = $tr.find('.keyField').html();
                var adjustment = $tr.find('.txtAdjustment').val();
                $txtQuantityEND = $tr.find('.txtQuantityEND');
                var quantityEND = $txtQuantityEND.attr('hiddenVal');
                var remarks = $tr.find('.txtRemarksDt').val();

                if (quantityEND > -1) {
                    $txtQuantityEND.removeClass('error');
                    var idx = $tr.find('.hdnItemIndex').val();
                    cboCheckCountType = eval('cboCheckCountType' + idx);
                    var checkCountType = '';
                    if (cboCheckCountType.GetValue() != null)
                        checkCountType = cboCheckCountType.GetValue();
                    var param = 'save|' + itemID + '|' + adjustment + '|' + quantityEND + '|' + checkCountType + '|' + remarks;
                    $btnSave = $(this);
                    cbpProcess.PerformCallback(param);
                }
                else
                    $txtQuantityEND.addClass('error');
            }
        });

        function onRefreshGridView() {
            var filterExpression = txtSearchView.GenerateFilterExpression();
            //if (typeof onRefreshControl == 'function')
            //onRefreshControl(filterExpression);
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        function onCboBinLocationEndCallBackChanged() {
            $('#<%=hdnBinLocationID.ClientID %>').val(cboBinLocation.GetValue());
            cbpView.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var stockTakingID = $('#<%=hdnStockTakingID.ClientID %>').val();
            var binLocationID = $('#<%=hdnBinLocationID.ClientID %>').val();
            var locationID = $('#<%=hdnLocationID.ClientID %>').val();
            var gcItemType = cboItemType.GetValue();
            var gcABCClass = cboABCClass.GetValue();
            var productLineID = $('#<%=hdnProductLineID.ClientID %>').val();
            if (stockTakingID == '' || stockTakingID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else if (code == 'IM-00111') {
                filterExpression.text = stockTakingID;
                return true;
            }
            else if (code == 'IM-00214') {
                if (cboABCClass.GetValue() != null && productLineID != 0) {
                    filterExpression.text = "StockTakingID = " + stockTakingID + " AND " + "LocationID = " + locationID + " AND " + "GCABCClass = " + "'" + gcABCClass + "'" + " AND " + "GCItemType = " + "'" + gcItemType + "'" + " AND " + "ProductLineID = " + productLineID;
                }
                else if (cboABCClass.GetValue() != null && productLineID == 0) {
                    filterExpression.text = "StockTakingID = " + stockTakingID + " AND " + "LocationID = " + locationID + " AND " + "GCABCClass = " + "'" + gcABCClass + "'" + " AND " + "GCItemType = " + "'" + gcItemType + "'";
                }
                else {
                    filterExpression.text = "StockTakingID = " + stockTakingID + " AND " + "LocationID = " + locationID + " AND " + "GCItemType = " + "'" + gcItemType + "'";
                }
                return true;
            }
            else if (code == 'IM-00074') {
                if (binLocationID == 0) {
                    filterExpression.text = "StockTakingID = " + stockTakingID;
                }
                else {
                    filterExpression.text = "StockTakingID = " + stockTakingID + " AND " + "BinLocationID = " + binLocationID;
                }
                return true;
            }
            else {
                filterExpression.text = "StockTakingID = " + stockTakingID;
                return true;
            }
        }

        $lblExpiredDate = null;
        $('.lblExpiredDate').die('click');
        $('.lblExpiredDate').live('click', function () {
            $lblExpiredDate = $(this);
            $tr = $(this).closest('tr');
            var itemID = $tr.find('.keyField').html();
            var hdnStockTakingID = $('#<%=hdnStockTakingID.ClientID %>').val();

            $txtQuantityEND = $tr.find('.txtQuantityEND');
            var quantityEND = $txtQuantityEND.attr('hiddenVal');

            var param = hdnStockTakingID + '|' + itemID + '|' + quantityEND;
            var url = ResolveUrl("~/Program/WareHouse/StockTaking/StockTakingExpiredDateCtl.ascx");
            openUserControlPopup(url, param, 'Expired Date Per Item', 750, 450);
        });

        function onCboItemTypeValueChanged() {
            $('#<%=hdnProductLineID.ClientID %>').val('');
            $('#<%=txtProductLineCode.ClientID %>').val('');
            $('#<%=txtProductLineName.ClientID %>').val('');
            $('#<%=hdnProductLineItemType.ClientID %>').val('');
        }
        function onCboABCClassValueChanged() {
            $('#<%=hdnProductLineID.ClientID %>').val('');
            $('#<%=txtProductLineCode.ClientID %>').val('');
            $('#<%=txtProductLineName.ClientID %>').val('');
            $('#<%=hdnProductLineItemType.ClientID %>').val('');
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnDefaultCycleCountType" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoFillQtyEnd" runat="server" />
    <input type="hidden" value="0" id="hdnInputAll" runat="server" />
    <input type="hidden" value="0" id="hdnInputQtyFisik" runat="server" />
    <input type="hidden" value="0" id="hdnInputQtySelisih" runat="server" />
    <input type="hidden" value="" id="hdnBinLocationID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoidByReason" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <input type="hidden" id="hdnStockTakingID" value="0" runat="server" />
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblStockTakingNo">
                                <%=GetLabel("No Bukti Opname")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtStockTakingNo" Width="150px" runat="server" TabIndex="1" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input type="button" runat="server" id="btnStartCalculate" value="Mulai Hitung Stok Fisik" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Opname") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                <%=GetLabel("Lokasi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnLocationID" value="" runat="server" />
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelompok A/B/C")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboABCClass" ClientInstanceName="cboABCClass"
                                Width="50%">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboABCClassValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Item Type")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" Width="50%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
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
            <td valign="top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Quick Search")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="300px" Watermark="Search">
                                <clientsideevents searchclick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <intellisensehints>
                                    <qis:QISIntellisenseHint Text="Kode Item" FieldName="ItemCode" />
                                    <qis:QISIntellisenseHint Text="Nama Item" FieldName="ItemName1" />
                                </intellisensehints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Accuracy")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccuracy" Width="40%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr id="trBinLocation" runat="server" style="display: none">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Rak")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboBinLocation" ClientInstanceName="cboBinLocation"
                                Width="50%" OnCallback="cboBinLocation_Callback">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboBinLocationEndCallBackChanged(); }" />
                            </dxe:ASPxComboBox>
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
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView grdSelected" cellspacing="0" rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                    &nbsp;
                                                </th>
                                                <th rowspan="2" style="width: 80px">
                                                    <%=GetLabel("Kode")%>
                                                </th>
                                                <th rowspan="2" style="width: 150px">
                                                    <%=GetLabel("Nama Item")%>
                                                </th>
                                                <th rowspan="2" style="width: 150px">
                                                    <%=GetLabel("Nama Rak")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Quantity")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px">
                                                    <%=GetLabel("Expired Date")%>
                                                </th>
                                                <th rowspan="2" style="width: 140px">
                                                    <%=GetLabel("Check Count Type")%>
                                                </th>
                                                <th rowspan="2" style="width: 80px">
                                                    <%=GetLabel("Simpan")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Qty BSO")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Selisih")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Qty Akhir")%>
                                                </th>
                                                <th style="width: 80px">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 80px">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Konversi")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="12">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdStokTaking grdSelected" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                    &nbsp;
                                                </th>
                                                <th rowspan="2" style="width: 80px">
                                                    <%=GetLabel("Kode")%>
                                                </th>
                                                <th rowspan="2" style="width: 150px">
                                                    <%=GetLabel("Nama Item")%>
                                                </th>
                                                <th rowspan="2" style="width: 150px">
                                                    <%=GetLabel("Nama Rak")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Quantity")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px">
                                                    <%=GetLabel("Expired Date")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px">
                                                    <%=GetLabel("Remarks")%>
                                                </th>
                                                <th rowspan="2" style="width: 140px">
                                                    <%=GetLabel("Check Count Type")%>
                                                </th>
                                                <th rowspan="2" style="width: 80px">
                                                    <%=GetLabel("Simpan")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px">
                                                    <%=GetLabel("Qty BSO")%>
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Selisih")%>
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Qty Akhir")%>
                                                </th>
                                                <th style="width: 80px">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 80px">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("Konversi")%>
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
                                            <td>
                                                <%#: Eval("ItemCode")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ItemName1")%>
                                            </td>
                                            <td>
                                                <%#: Eval("BinLocationName")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("QuantityBSO")%>
                                            </td>
                                            <td>
                                                <input type="hidden" class="hdnItemIndex" value='<%#: Container.DataItemIndex %>' />
                                                <input type="hidden" class="hdnQuantityBSO" value='<%#:Eval("QuantityBSO") %>' />
                                                <input type="text" runat="server" value='<%#:Eval("QuantityAdjustment") %>' id="txtAdjustment"
                                                    class="txtAdjustment number" style="width: 100%" />
                                            </td>
                                            <td>
                                                <input type="text" class="txtQuantityEND txtCurrency" id="txtQuantityEND" runat="server"
                                                    style="width: 100%" />
                                            </td>
                                            <td>
                                                <%#: Eval("ItemUnit")%>
                                            </td>
                                            <td>
                                                <div id="divPurchaseUnit" runat="server">
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div id="divConversionFactor" runat="server">
                                                </div>
                                            </td>
                                            <td align="center">
                                                <input type="hidden" runat="server" id="hdnControlExpired" class="hdnControlExpired"
                                                    value='<%#:Eval("IsControlExpired") %>' />
                                                <label id="lblExpiredDate" runat="server" class="lblExpiredDate lblLink">
                                                    <%=GetLabel("Expired Date") %></label>
                                            </td>
                                            <td>
                                                <input type="text" class="txtRemarksDt" id="txtRemarksDt" runat="server" style="width: 100%"
                                                    value='<%#:Eval("remarks") %>' />
                                            </td>
                                            <td align="center">
                                                <dxe:ASPxComboBox ID="cboCheckCountType" runat="server" Width="90%" />
                                            </td>
                                            <td align="center">
                                                <input type="button" id="btnSave" class="btnSave" value="Simpan" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
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
                                            <tr id="trProposedBy" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Dipropose Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divProposedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trProposedDate" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Dipropose Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divProposedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trApprovedBy" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Diapprove Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divApprovedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trApprovedDate" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Diapprove Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divApprovedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trVoidBy" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Divoid Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divVoidBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trVoidDate" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Divoid Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divVoidDate">
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
                                            <tr id="trVoidReason" style="display: none" runat="server">
                                                <td align="left">
                                                    <%=GetLabel("Alasan Batal")%>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divVoidReason">
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
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
            ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
