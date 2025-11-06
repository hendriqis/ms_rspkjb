<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="FAItemFromPurchaseReceiveProcessList.aspx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemFromPurchaseReceiveProcessList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnDecline" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtReceivedDateFrom.ClientID %>');
            setDatePicker('<%=txtReceivedDateTo.ClientID %>');

            onLoad();
        });

        function onLoad() {
            $('.txtDepreciationStartDate').each(function () {
                setDatePickerElement($(this));
            });
        }

        //#region Supplier
        function getSupplierFilterExpression() {
            var filterExpression = "GCBusinessPartnerType = 'X017^003' AND IsBlackList = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                $('#<%=txtSupplierCode.ClientID %>').val(value);
                onTxtSupplierChanged(value);
            });
        });

        $('#<%=txtSupplierCode.ClientID %>').live('change', function () {
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
                }
            });
        }
        //#endregion

        //#region Item
        function getItemFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCItemStatus != 'X181^999'";
            filterExpression += " AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";

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
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                }
            });

        }
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
                }
                else {
                    $('#<%=hdnProductLineID.ClientID %>').val('');
                    $('#<%=txtProductLineCode.ClientID %>').val('');
                    $('#<%=txtProductLineName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected').each(function () {
                $chk = $(this).find('input');
                $chk.prop('checked', isChecked);
                $chk.change();
            });
        });

        function getCheckedMember() {
            var lstSelectedRowNumber = $('#<%=hdnSelectedRowNumber.ClientID %>').val().split(',');
            var lstSelectedID = $('#<%=hdnSelectedID.ClientID %>').val().split(',');
            var lstSelectedParentID = $('#<%=hdnSelectedParentID.ClientID %>').val().split(',');
            var lstSelectedSerialNumber = $('#<%=hdnSelectedSerialNumber.ClientID %>').val().split(',');
            var lstSelectedFAGroupID = $('#<%=hdnSelectedFAGroupID.ClientID %>').val().split(',');
            var lstSelectedFALocationID = $('#<%=hdnSelectedFAGroupID.ClientID %>').val().split(',');
            var lstSelectedGCBudgetCategory = $('#<%=hdnSelectedGCBudgetCategory.ClientID %>').val().split(',');
            var lstSelectedBudgetPlanNo = $('#<%=hdnSelectedBudgetPlanNo.ClientID %>').val().split(',');
            var lstSelectedMethodID = $('#<%=hdnSelectedMethodID.ClientID %>').val().split(',');
            var lstSelectedDepreciationLength = $('#<%=hdnSelectedDepreciationLength.ClientID %>').val().split(',');
            var lstSelectedDepreciationStartDate = $('#<%=hdnSelectedDepreciationStartDate.ClientID %>').val().split(',');
            var lstSelectedAssetFinalValue = $('#<%=hdnSelectedAssetFinalValue.ClientID %>').val().split(',');
            $('.lvwView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var hdncfID = $tr.find('.hdncfID').val();
                    var hdnParentID = $tr.find('.hdnParentID').val();
                    var txtSerialNumber = $tr.find('.txtSerialNumber').val();
                    var hdnFAGroupID = $tr.find('.hdnFAGroupID').val();
                    var hdnFALocationID = $tr.find('.hdnFALocationID').val();
                    var hdnGCBudgetCategory = $tr.find('.hdnGCBudgetCategory').val();
                    var txtBudgetPlanNo = $tr.find('.txtBudgetPlanNo').val();
                    var hdnMethodID = $tr.find('.hdnMethodID').val();
                    var txtDepreciationLength = $tr.find('.txtDepreciationLength').val();
                    var txtDepreciationStartDate = $tr.find('.txtDepreciationStartDate').val();
                    var txtAssetFinalValue = $tr.find('.txtAssetFinalValue').val();
                    var idx = lstSelectedRowNumber.indexOf(key);
                    if (idx < 0) {
                        lstSelectedRowNumber.push(key);
                        lstSelectedID.push(hdncfID);
                        lstSelectedParentID.push(hdnParentID);
                        lstSelectedSerialNumber.push(txtSerialNumber);
                        lstSelectedFAGroupID.push(hdnFAGroupID);
                        lstSelectedFALocationID.push(hdnFALocationID);
                        lstSelectedGCBudgetCategory.push(hdnGCBudgetCategory);
                        lstSelectedBudgetPlanNo.push(txtBudgetPlanNo);
                        lstSelectedMethodID.push(hdnMethodID);
                        lstSelectedDepreciationLength.push(txtDepreciationLength);
                        lstSelectedDepreciationStartDate.push(txtDepreciationStartDate);
                        lstSelectedAssetFinalValue.push(txtAssetFinalValue);
                    }
                    else {
                        lstSelectedID[idx] = hdncfID;
                        lstSelectedParentID[idx] = hdnParentID;
                        lstSelectedSerialNumber[idx] = txtSerialNumber;
                        lstSelectedFAGroupID[idx] = hdnFAGroupID;
                        lstSelectedFALocationID[idx] = hdnFALocationID;
                        lstSelectedGCBudgetCategory[idx] = hdnGCBudgetCategory;
                        lstSelectedBudgetPlanNo[idx] = txtBudgetPlanNo;
                        lstSelectedMethodID[idx] = hdnMethodID;
                        lstSelectedDepreciationLength[idx] = txtDepreciationLength;
                        lstSelectedDepreciationStartDate[idx] = txtDepreciationStartDate;
                        lstSelectedAssetFinalValue[idx] = txtAssetFinalValue;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstSelectedRowNumber.indexOf(key);
                    if (idx > -1) {
                        lstSelectedRowNumber.splice(idx, 1);
                        lstSelectedID.splice(idx, 1);
                        lstSelectedParentID.splice(idx, 1);
                        lstSelectedSerialNumber.splice(idx, 1);
                        lstSelectedFAGroupID.splice(idx, 1);
                        lstSelectedFALocationID.splice(idx, 1);
                        lstSelectedGCBudgetCategory.splice(idx, 1);
                        lstSelectedBudgetPlanNo.splice(idx, 1);
                        lstSelectedMethodID.splice(idx, 1);
                        lstSelectedDepreciationLength.splice(idx, 1);
                        lstSelectedDepreciationStartDate.splice(idx, 1);
                        lstSelectedAssetFinalValue.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedRowNumber.ClientID %>').val(lstSelectedRowNumber.join(','));
            $('#<%=hdnSelectedID.ClientID %>').val(lstSelectedID.join(','));
            $('#<%=hdnSelectedParentID.ClientID %>').val(lstSelectedParentID.join(','));
            $('#<%=hdnSelectedSerialNumber.ClientID %>').val(lstSelectedSerialNumber.join(','));
            $('#<%=hdnSelectedFAGroupID.ClientID %>').val(lstSelectedFAGroupID.join(','));
            $('#<%=hdnSelectedFALocationID.ClientID %>').val(lstSelectedFALocationID.join(','));
            $('#<%=hdnSelectedGCBudgetCategory.ClientID %>').val(lstSelectedGCBudgetCategory.join(','));
            $('#<%=hdnSelectedBudgetPlanNo.ClientID %>').val(lstSelectedBudgetPlanNo.join(','));
            $('#<%=hdnSelectedMethodID.ClientID %>').val(lstSelectedMethodID.join(','));
            $('#<%=hdnSelectedDepreciationLength.ClientID %>').val(lstSelectedDepreciationLength.join(','));
            $('#<%=hdnSelectedDepreciationStartDate.ClientID %>').val(lstSelectedDepreciationStartDate.join(','));
            $('#<%=hdnSelectedAssetFinalValue.ClientID %>').val(lstSelectedAssetFinalValue.join(','));
        }

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            getCheckedMember();

            var isAllowProcess = "0";
            var errMessage = "";
            var oFAGroupID = $('#<%=hdnSelectedFAGroupID.ClientID %>').val();
            var oFALocationID = $('#<%=hdnSelectedFALocationID.ClientID %>').val();
            var oMethodID = $('#<%=hdnSelectedMethodID.ClientID %>').val();

            if (oFAGroupID != null && oFAGroupID != "" && oFAGroupID != ",0") {
                if (oFALocationID != null && oFALocationID != "" && oFALocationID != ",0") {
                    if (oMethodID != null && oMethodID != "" && oMethodID != ",0") {
                        isAllowProcess = "1";
                    } else {
                        isAllowProcess = "0";
                        errMessage = "Harap pilih Metode Penyusutan Aset terlebih dahulu.";
                    }
                } else {
                    isAllowProcess = "0";
                    errMessage = "Harap pilih Lokasi Aset terlebih dahulu.";
                }
            } else {
                isAllowProcess = "0";
                errMessage = "Harap pilih Kelompok Aset terlebih dahulu.";
            }

            if (isAllowProcess == "1") {
                onCustomButtonClick('process');
            } else {
                showToast('FAILED', errMessage);
            }
        });

        $('#<%=btnDecline.ClientID %>').live('click', function () {
            getCheckedMember();

            var lstID = $('#<%=hdnSelectedID.ClientID %>').val();
            var url = ResolveUrl("~/Program/Master/FAItem/VoidFAItemFromListCtl.ascx");
            openUserControlPopup(url, lstID, 'Proses Batal Aset', 600, 200);
        });

        //#region Fill Detail

        //#region ParentFixedAsset
        $td = null;
        $('.lblParent.lblLink').live('click', function () {
            $td = $(this).parent();
            var filterExpression = "ParentID IS NULL AND IsDeleted = 0";
            openSearchDialog('faitem', filterExpression, function (value) {
                onFAParentChanged(value);
            });
        });

        function onFAParentChanged(value) {
            var $tr = $(this).closest('tr');
            var filterExpression = "ParentID IS NULL AND IsDeleted = 0 AND FixedAssetCode = '" + value + "'";
            Methods.getObject('GetFAItemList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnParentID').val(result.FixedAssetID);
                    $td.find('.lblParent').html(result.FixedAssetName);
                }
                else {
                    $td.find('.hdnParentID').val('0');
                    $td.find('.lblParent').html('Pilih Induk');
                }
            });
        }
        //#endregion

        //#region FAGroup
        $td = null;
        $('.lblFAGroup.lblLink').live('click', function () {
            $td = $(this).parent();
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('fagroup', filterExpression, function (value) {
                onFAGroupChanged(value);
            });
        });

        function onFAGroupChanged(value) {
            var $tr = $(this).closest('tr');
            var filterExpression = "IsDeleted = 0 AND FAGroupCode = '" + value + "'";
            Methods.getObject('GetFAGroupList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnFAGroupID').val(result.FAGroupID);
                    $td.find('.lblFAGroup').html(result.FAGroupName);
                }
                else {
                    $td.find('.hdnFAGroupID').val('0');
                    $td.find('.lblFAGroup').html('Pilih Kelompok');
                }
            });
        }
        //#endregion

        //#region FALocation
        $td = null;
        $('.lblFALocation.lblLink').live('click', function () {
            $td = $(this).parent();
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('falocation', filterExpression, function (value) {
                onFALocationChanged(value);
            });
        });

        function onFALocationChanged(value) {
            var $tr = $(this).closest('tr');
            var filterExpression = "IsDeleted = 0 AND FALocationCode = '" + value + "'";
            Methods.getObject('GetFALocationList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnFALocationID').val(result.FALocationID);
                    $td.find('.lblFALocation').html(result.FALocationName);
                }
                else {
                    $td.find('.hdnFALocationID').val('0');
                    $td.find('.lblFALocation').html('Pilih Lokasi Aset');
                }
            });
        }
        //#endregion

        //#region GCBudgetCategory
        $td = null;
        $('.lblBudgetCategory.lblLink').live('click', function () {
            $td = $(this).parent();
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ParentID = 'X393'";
            openSearchDialog('standardcode', filterExpression, function (value) {
                onBudgetCategoryChanged(value);
            });
        });

        function onBudgetCategoryChanged(value) {
            var $tr = $(this).closest('tr');
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ParentID = 'X393' AND StandardCodeID = '" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnGCBudgetCategory').val(result.StandardCodeID);
                    $td.find('.lblBudgetCategory').html(result.StandardCodeName);
                }
                else {
                    $td.find('.hdnGCBudgetCategory').val('0');
                    $td.find('.lblBudgetCategory').html('Pilih Kategori Anggaran');
                }
            });
        }
        //#endregion

        //#region Depreciation Method
        $td = null;
        $('.lblMethod.lblLink').live('click', function () {
            $td = $(this).parent();
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('fadepreciationmethod', filterExpression, function (value) {
                onMethodChanged(value);
            });
        });

        function onMethodChanged(value) {
            var $tr = $(this).closest('tr');
            var filterExpression = "IsDeleted = 0 AND MethodCode = '" + value + "'";
            Methods.getObject('GetFADepreciationMethodList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.hdnMethodID').val(result.MethodID);
                    $td.find('.lblMethod').html(result.MethodName);
                }
                else {
                    $td.find('.hdnMethodID').val('0');
                    $td.find('.lblMethod').html('Pilih Metode');
                }
            });
        }
        //#endregion

        //#endregion

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                onLoad();
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedID.ClientID %>').val('');
            $('#<%=hdnSelectedParentID.ClientID %>').val('');
            $('#<%=hdnSelectedSerialNumber.ClientID %>').val('');
            $('#<%=hdnSelectedFAGroupID.ClientID %>').val('');
            $('#<%=hdnSelectedFALocationID.ClientID %>').val('');
            $('#<%=hdnSelectedGCBudgetCategory.ClientID %>').val('');
            $('#<%=hdnSelectedBudgetPlanNo.ClientID %>').val('');
            $('#<%=hdnSelectedMethodID.ClientID %>').val('');
            $('#<%=hdnSelectedDepreciationLength.ClientID %>').val('');
            $('#<%=hdnSelectedDepreciationStartDate.ClientID %>').val('');
            $('#<%=hdnSelectedAssetFinalValue.ClientID %>').val('');

            var newFACode = $('#<%=hdnNewFixedAssetCode.ClientID %>').val();

            if (type == 'process') {
                showToast('Process Success', 'Proses Simpan Aset & Inventaris sudah berhasil dilakukan, dengan Kode : <b>' + retval + '</b>.');
            }
            else if (type == 'decline') {
                showToast('Process Success', 'Proses Decline sudah berhasil dilakukan');
            }

            cbpView.PerformCallback('refresh');
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnNewFixedAssetCode" runat="server" />
    <input type="hidden" value="" id="hdnSupplierID" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnProductLineID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRowNumber" runat="server" />
    <input type="hidden" value="" id="hdnSelectedID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedParentID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedSerialNumber" runat="server" />
    <input type="hidden" value="" id="hdnSelectedFAGroupID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedFALocationID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedGCBudgetCategory" runat="server" />
    <input type="hidden" value="" id="hdnSelectedBudgetPlanNo" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMethodID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDepreciationLength" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDepreciationStartDate" runat="server" />
    <input type="hidden" value="" id="hdnSelectedAssetFinalValue" runat="server" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToFAItem" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToFAItem" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 10px" />
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Tgl Penerimaan") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtReceivedDateFrom" CssClass="datepicker" />
                            </td>
                            <td style="text-align: center">
                                <label class="lblNormal">
                                    <%=GetLabel("s/d") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="120px" ID="txtReceivedDateTo" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblSupplier" runat="server">
                                    <%=GetLabel("Supplier/Penyedia")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblItem">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server">
                            <td>
                                <label class="lblLink" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th rowspan="2" style="width: 40px" align="center">
                                            <input id="chkSelectAll" type="checkbox" />
                                        </th>
                                        <th rowspan="2" align="left">
                                            <%=GetLabel("Aset & Inventaris")%>
                                        </th>
                                        <th rowspan="2" align="center">
                                            <%=GetLabel("Informasi Penerimaan Barang")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Induk")%>
                                        </th>
                                        <th rowspan="2" style="width: 100px" align="center">
                                            <%=GetLabel("No.Seri")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Kelompok & Lokasi Aset")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Kategori Anggaran")%>
                                        </th>
                                        <th rowspan="2" style="width: 100px" align="center">
                                            <%=GetLabel("No.Anggaran")%>
                                        </th>
                                        <th colspan="4" style="width: 350px" align="center">
                                            <%=GetLabel("Penyusutan")%>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 80px" align="center">
                                            <%=GetLabel("Metode")%>
                                        </th>
                                        <th style="width: 50px" align="center">
                                            <%=GetLabel("Umur")%>
                                        </th>
                                        <th style="width: 120px" align="center">
                                            <%=GetLabel("Tgl.Mulai")%>
                                        </th>
                                        <th style="width: 100px" align="center">
                                            <%=GetLabel("Nilai Buku Residu")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="20">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="lvwView grdView notAllowSelect" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th rowspan="2" style="width: 40px" align="center">
                                            <input id="chkSelectAll" type="checkbox" />
                                        </th>
                                        <th rowspan="2" align="left">
                                            <%=GetLabel("Aset & Inventaris")%>
                                        </th>
                                        <th rowspan="2" align="center">
                                            <%=GetLabel("Informasi Penerimaan Barang")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Induk")%>
                                        </th>
                                        <th rowspan="2" style="width: 100px" align="center">
                                            <%=GetLabel("No.Seri")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Kelompok & Lokasi Aset")%>
                                        </th>
                                        <th rowspan="2" style="width: 80px" align="center">
                                            <%=GetLabel("Kategori Anggaran")%>
                                        </th>
                                        <th rowspan="2" style="width: 100px" align="center">
                                            <%=GetLabel("No.Anggaran")%>
                                        </th>
                                        <th colspan="4" style="width: 350px" align="center">
                                            <%=GetLabel("Penyusutan")%>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 80px" align="center">
                                            <%=GetLabel("Metode")%>
                                        </th>
                                        <th style="width: 50px" align="center">
                                            <%=GetLabel("Umur")%>
                                        </th>
                                        <th style="width: 120px" align="center">
                                            <%=GetLabel("Tgl.Mulai")%>
                                        </th>
                                        <th style="width: 100px" align="center">
                                            <%=GetLabel("Nilai Buku Residu")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("RowNumber")%>' />
                                        <input type="hidden" class="hdncfID" id="hdncfID" runat="server" value='<%#: Eval("cfID")%>' />
                                    </td>
                                    <td>
                                        <div style="font-size: 14px;">
                                            [<%#:Eval("ItemCode") %>]
                                            <%#:Eval("ItemName1") %></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("Catatan : ")%></i><b><%#:Eval("ProductLineName") %></b></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("ProductLine : ")%></i><b><%#:Eval("ProductLineName") %></b></div>
                                    </td>
                                    <td>
                                        <div style="font-size: 14px;">
                                            <%#:Eval("PurchaseReceiveNo") %></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("Tgl.Terima : ")%></i><b><%#:Eval("ReceivedDateInString") %></b></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("Lokasi : ")%></i><b><%#:Eval("LocationName") %></b></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("Supplier : ")%></i><b><%#:Eval("BusinessPartnerName") %></b></div>
                                        <div style="font-size: 10px;">
                                            <i>
                                                <%=GetLabel("No.Faktur : ")%></i><b><%#:Eval("ReferenceNo") %></b></div>
                                    </td>
                                    <td align="center">
                                        <input type="hidden" class="hdnParentID" id="hdnParentID" runat="server" value='0' />
                                        <label runat="server" id="lblParent" class="lblLink lblParent">
                                            <%=GetLabel("Pilih Induk")%></label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtSerialNumber" CssClass="txtSerialNumber" Text='' placeholder="No.Seri"
                                            Width="90%" runat="server" />
                                    </td>
                                    <td align="center">
                                        <input type="hidden" class="hdnFAGroupID" id="hdnFAGroupID" runat="server" value='0' />
                                        <label runat="server" id="lblFAGroup" class="lblLink lblFAGroup">
                                            <%=GetLabel("Pilih Kelompok")%></label>
                                        <%=GetLabel("-------------------------")%>
                                        <input type="hidden" class="hdnFALocationID" id="hdnFALocationID" runat="server"
                                            value='0' />
                                        <label runat="server" id="lblFALocation" class="lblLink lblFALocation">
                                            <%=GetLabel("Pilih Lokasi Aset")%></label>
                                    </td>
                                    <td align="center">
                                        <input type="hidden" class="hdnGCBudgetCategory" id="hdnGCBudgetCategory" runat="server"
                                            value='<%#:Eval("GCBudgetCategory") %>' />
                                        <label runat="server" id="lblBudgetCategory" class="lblLink lblBudgetCategory" title='<%#:Eval("BudgetCategory") %>'>
                                            <%#: Eval("BudgetCategory") != "" ? Eval("BudgetCategory") : "Pilih Kategori Anggaran" %></label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtBudgetPlanNo" CssClass="txtBudgetPlanNo" Text='<%#:Eval("OtherReferenceNo") %>'
                                            placeholder="No.Anggaran" Width="90%" runat="server" />
                                    </td>
                                    <td align="center">
                                        <input type="hidden" class="hdnMethodID" id="hdnMethodID" runat="server" value='0' />
                                        <label runat="server" id="lblMethod" class="lblLink lblMethod">
                                            <%=GetLabel("Pilih Metode")%></label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtDepreciationLength" CssClass="txtDepreciationLength number" Text='0'
                                            placeholder="Umur" Width="90%" runat="server" />
                                    </td>
                                    <td align="center">
                                        <asp:TextBox runat="server" ID="txtDepreciationStartDate" CssClass="txtDepreciationStartDate datepicker"
                                            Width="80px" />
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtAssetFinalValue" CssClass="txtAssetFinalValue number" Text='0'
                                            placeholder="Nilai Buku Residu" Width="90%" runat="server" />
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
    </div>
</asp:Content>
