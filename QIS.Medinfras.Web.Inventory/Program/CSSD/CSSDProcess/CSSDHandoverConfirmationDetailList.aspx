<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CSSDHandoverConfirmationDetailList.aspx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDHandoverConfirmationDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnServiceRequestBack" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnServiceRequestBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/CSSD/CSSDProcess/CSSDHandoverConfirmationList.aspx');
            });

            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                if ($('#<%=hdnIsAllowCreateConsumption.ClientID %>').val() == '1') {
                    $('#lblAddData').show();
                    $('#lblQuickPick').show();
                } else {
                    $('#lblAddData').hide();
                    $('#lblQuickPick').hide();
                }
            }
            else {
                $('#lblAddData').hide();
                $('#lblQuickPick').hide();
            }

            setDatePicker('<%=txtConsumptionDate.ClientID %>');
            $('#<%=txtConsumptionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            //#region Consumption No
            function onGetItemConsumptionFilterExpression() {
                var filterExpression = "<%:GetFilterExpression() %>";
                return filterExpression;
            }

            $('#lblConsumptionNo.lblLink').click(function () {
                openSearchDialog('stockconsumptionhd', onGetItemConsumptionFilterExpression(), function (value) {
                    $('#<%=txtConsumptionNo.ClientID %>').val(value);
                    onTxtConsumptionNoChanged(value);
                });
            });

            $('#<%=txtConsumptionNo.ClientID %>').change(function () {
                onTxtConsumptionNoChanged($(this).val());
            });

            function onTxtConsumptionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            $('#lblQuickPick').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/CSSD/CSSDProcess/CSSDConsumptionEntryQuickPickCtl.ascx');
                    var transactionID = $('#<%=hdnConsumptionID.ClientID %>').val();
                    var locationID = $('#<%=hdnLocationConsumptionID.ClientID %>').val();
                    var locationItemGroupID = $('#<%=hdnLocationConsumptionItemGroupID.ClientID %>').val();
                    var GCLocationGroup = $('#<%=hdnLocationConsumptionGCLocationGroup.ClientID %>').val();
                    var serviceRequestID = $('#<%=hdnServiceRequestID.ClientID %>').val();
                    var id = transactionID + '|' + locationID + '|' + GCLocationGroup + '|' + serviceRequestID;
                    openUserControlPopup(url, id, 'Quick Picks', 1000, 550);
                }
            });

            $('#lblAddData').click(function (evt) {
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
                    $('#<%=txtQuantity.ClientID %>').val('');
                    $('#<%=txtNotesDt.ClientID %>').val('');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    cboItemUnit.SetValue('');
                    $('#<%=txtConversion.ClientID %>').val('');

                    $('#containerEntry').show();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrx', 'mpTrx'))
                    cbpProcess.PerformCallback('save');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewConsumption.PerformCallback('changepage|' + page);
            });

            //#region Paging CSSD
            var pageCount = parseInt('<%=PageCount %>');
            $(function () {
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            });

            function onCbpViewEndCallback(s) {
                hideLoadingPanel();

                var param = s.cpResult.split('|');
                if (param[0] == 'refresh') {
                    var pageCount = parseInt(param[1]);
                    if (pageCount > 0)
                        $('.grdCSSDRequest tr:eq(1)').click();

                    setPaging($("#paging"), pageCount, function (page) {
                        cbpView.PerformCallback('changepage|' + page);
                    });
                }
                else
                    $('.grdCSSDRequest tr:eq(1)').click();
            }
            //#endregion
        }


        function onGetItemTypeFromFilterExpression() {
            var filterExpression = "";
            if ($('#<%=hdnGCLocationGroupConsumption.ClientID %>').val() == '') {
                filterExpression = "GCItemType IN ('X001^002','X001^003','X001^008') AND IsDeleted = 0";
            }
            else if ($('#<%=hdnGCLocationGroupConsumption.ClientID %>').val() == 'X227^1') {
                filterExpression = "GCItemType IN ('X001^002','X001^003') AND IsDeleted = 0";
            }
            else {
                filterExpression = "GCItemType IN ('X001^008') AND IsDeleted = 0";
            }
            return filterExpression;
        }

        function onGetItemGroupFilterExpression() {
            var filterExpression = onGetItemTypeFromFilterExpression();
            if ($('#<%=hdnLocationConsumptionItemGroupID.ClientID %>').val() != '' && $('#<%=hdnLocationConsumptionItemGroupID.ClientID %>').val() != '0')
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/" + $('#<%=hdnLocationConsumptionItemGroupID.ClientID %>').val() + "/%')";
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

        function GetItemQtyFromLocation() {
            var filterExpression = "LocationID = " + $('#<%=hdnLocationConsumptionID.ClientID %>').val() + " AND ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0";
            Methods.getObject('GetvItemBalanceList', filterExpression, function (result) {
                $('#<%=txtQuantity.ClientID %>').attr('max', result.QuantityEND);
                $('#<%=txtStockLocation.ClientID %>').val(result.QuantityEND + ' ' + result.ItemUnit);
            });
        }

        //#region Item
        function getItemFilterExpression() {
            var filterExpression = onGetItemTypeFromFilterExpression();
            var adjustmentID = $('#<%=hdnConsumptionID.ClientID %>').val();
            var locationID = $('#<%=hdnLocationConsumptionID.ClientID %>').val();
            if ($('#<%=txtItemGroupCode.ClientID %>').val() != '')
                filterExpression += " AND ItemGroupID = '" + $('#<%=hdnItemGroupID.ClientID %>').val() + "'";
            if (adjustmentID != '')
                filterExpression += " AND ItemID NOT IN (SELECT ItemID FROM ItemTransactionDt WHERE TransactionID = " + adjustmentID + " AND IsDeleted = 0)";
            filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + locationID + " AND QuantityEND > 0) AND IsDeleted = 0";
            filterExpression += " AND GCItemStatus != 'X181^999'";
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

                    GetItemQtyFromLocation();
                    cboItemUnit.PerformCallback();
                }
                else {
                    $('#<%=hdnGCBaseUnit.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtStockLocation.ClientID %>').val('');
                    $('#<%=hdnCostAmount.ClientID %>').val('0');
                }
            });
        }
        //#endregion

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
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnGCBaseUnit.ClientID %>').val(entity.GCBaseUnit);
            $('#<%=hdnGCItemUnit.ClientID %>').val(entity.GCItemUnit);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Quantity);
            $('#<%=txtNotesDt.ClientID %>').val(entity.Remarks);
            $('#<%=hdnCostAmount.ClientID %>').val(entity.CostAmount);
            GetItemQtyFromLocation();
            cboItemUnit.PerformCallback();
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
        }

        function getItemUnitName(baseValue) {
            var value = cboItemUnit.GetValue();
            cboItemUnit.SetValue(baseValue);
            var text = cboItemUnit.GetText();
            cboItemUnit.SetValue(value);
            return text;
        }
        //#endregion

        //#region Paging Consumption
        function onCbpViewConsumptionEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewConsumption.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(ConsumptionID) {
            if ($('#<%=hdnConsumptionID.ClientID %>').val() == '0') {
                $('#<%=hdnConsumptionID.ClientID %>').val(ConsumptionID);
                var filterExpression = 'TransactionID = ' + ConsumptionID;
                Methods.getObject('GetItemTransactionHdList', filterExpression, function (result) {
                    $('#<%=txtConsumptionNo.ClientID %>').val(result.TransactionNo);
                    onLoadObject(result.TransactionNo);
                });
                onAfterCustomSaveSuccess();
            }
            else {
                cbpViewConsumption.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    onAfterSaveRecordDtSuccess(s.cpConsumptionID);
                    $('#lblAddData').click();
                    cbpViewConsumption.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpViewConsumption.PerformCallback('refresh');
            }
            $('#containerEntry').hide();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var TransactionID = $('#<%=hdnConsumptionID.ClientID %>').val();
            var serviceRequestID = $('#<%=hdnServiceRequestID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (TransactionID == '' || TransactionID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    if (code == 'IM-00066') {
                        filterExpression.text = "TransactionID = " + TransactionID;
                        return true;
                    } else if (code == 'IM-00047') {
                        filterExpression.text = "RequestID = " + serviceRequestID;
                        return true;
                    }
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowCreateConsumption" runat="server" />
    <input type="hidden" value="" id="hdnServiceRequestID" runat="server" />
    <input type="hidden" value="" id="hdnServiceRequestNo" runat="server" />
    <input type="hidden" value="" id="hdnServiceRequestStatus" runat="server" />
    <input type="hidden" value="" id="hdnParamID" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnConsumptionID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <div style="height: 500px; overflow-y: auto; overflow-x: hidden;">
        <h4>
            <%=GetLabel("Pemakaian CSSD")%></label></h4>
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
                                <label class="lblLink" id="lblConsumptionNo">
                                    <%=GetLabel("No. Pemakaian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConsumptionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pemakaian")%>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 70px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtConsumptionDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConsumptionTime" Width="100px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationConsumptionUnitID" value="" runat="server" />
                                <input type="hidden" id="hdnLocationConsumptionID" value="" runat="server" />
                                <input type="hidden" id="hdnLocationConsumptionItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnLocationConsumptionGCLocationGroup" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupConsumption" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationConsumptionCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationConsumptionName" Width="100%" runat="server" ReadOnly="true" />
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
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Pemakaian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCConsumptionType" ClientInstanceName="cboGCConsumptionType"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Unit/Bagian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcareUnit" ClientInstanceName="cboHealthcareUnit" Width="100%"
                                    runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                        <fieldset id="fsTrx" style="margin: 0">
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
                                                    <input type="hidden" value="" id="hdnCostAmount" runat="server" />
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
                                                        <%=GetLabel("Stok")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStockLocation" ReadOnly="true" CssClass="number" Width="120px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Jumlah")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" CssClass="number min" min="0" Width="120px" runat="server" />
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
                                                    <input type="hidden" value="" id="hdnItemConversionFactor" runat="server" />
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Konversi")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConversion" Width="180px" runat="server" ReadOnly="true" />
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
                    <dxcp:ASPxCallbackPanel ID="cbpViewConsumption" runat="server" Width="100%" ClientInstanceName="cbpViewConsumption"
                        ShowLoadingPanel="false" OnCallback="cbpViewConsumption_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewConsumptionEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center">
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
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                    <input type="hidden" value="<%#:Eval("ConversionFactor") %>" bindingfield="ConversionFactor" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("GCItemDetailStatus") %>" bindingfield="GCItemDetailStatus" />
                                                    <input type="hidden" value="<%#:Eval("CostAmount") %>" bindingfield="CostAmount" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Barang" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty Pemakaian" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Conversion" HeaderText="Konversi" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="LineAmountText" HeaderText="Line Amount" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada transaksi pemakaian")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="Div1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingConsumption">
                            </div>
                        </div>
                    </div>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="margin-right: 300px;">
                            <%= GetLabel("Tambah Data")%></span> <span class="lblLink" id="lblQuickPick">
                                <%= GetLabel("Quick Pick")%></span>
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
    <div style="overflow-y: auto; overflow-x: hidden;">
        <h4>
            <%=GetLabel("Permintaan CSSD")%></label></h4>
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
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRequestNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocation">
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
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" ReadOnly="true" />
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
                                    <%=GetLabel("Jenis Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceType" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblCSSDPackage">
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
                                            <asp:TextBox ID="txtPackageCode" Width="120px" runat="server" ReadOnly="true" />
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
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
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
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtRequestDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRequestTime" Width="100px" CssClass="time" runat="server" ReadOnly="true"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" ReadOnly="true" />
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
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Diminta Oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSentBy" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Diminta Pada")%>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtSentDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSentTime" Width="100px" CssClass="time" runat="server" ReadOnly="true"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Diterima Oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceivedBy" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Diterima Pada")%>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtReceivedDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReceivedTime" Width="100px" CssClass="time" runat="server" ReadOnly="true"
                                                Style="text-align: center" />
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
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewRequest" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
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
                                            <asp:BoundField DataField="cfIsConsumption" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderText="Is Consumption" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="cfBaseQtyUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Base Qty" HeaderStyle-Width="170px" />
                                            <asp:BoundField DataField="cfRequestQtyUnit" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" HeaderText="Request Qty" HeaderStyle-Width="170px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No data to display.")%>
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
