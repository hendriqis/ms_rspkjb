<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemServiceDtEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceDtEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnDetailItemID.ClientID %>').val('');
        $('#<%=txtDetailItemCode.ClientID %>').val('');
        $('#<%=txtDetailItemName.ClientID %>').val('');
        $('#<%=txtDetailItemQuantity.ClientID %>').val('1');
        $('#<%=chkIsAllowChangedQty.ClientID %>').prop('checked', false);
        $('#<%=chkIsAutoPosted.ClientID %>').prop('checked', false);

        $('#containerPopupEntryData').show();
    });

    $('#lblEntryPopupAddDataObat').live('click', function () {
        $('#<%=hdnOAID.ClientID %>').val('');
        $('#<%=hdnDetailObatID.ClientID %>').val('');
        $('#<%=txtDetailObatCode.ClientID %>').val('');
        $('#<%=txtDetailObatName.ClientID %>').val('');
        $('#<%=txtDetailObatQuantity.ClientID %>').val('1');
        $('#<%=chkIsAllowChangedQtyObat.ClientID %>').prop('checked', false);
        $('#<%=chkObatIsAutoPosted.ClientID %>').prop('checked', true);

        $('#containerPopupEntryDataObat').show();
    });

    $('#lblEntryPopupAddDataBarang').live('click', function () {
        $('#<%=hdnBID.ClientID %>').val('');
        $('#<%=hdnDetailBarangID.ClientID %>').val('');
        $('#<%=txtDetailBarangCode.ClientID %>').val('');
        $('#<%=txtDetailBarangName.ClientID %>').val('');
        $('#<%=txtDetailBarangQuantity.ClientID %>').val('1');
        $('#<%=chkIsAllowChangedQtyBarang.ClientID %>').prop('checked', false);
        $('#<%=chkBarangIsAutoPosted.ClientID %>').prop('checked', false);

        $('#containerPopupEntryDataBarang').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupCancelObat').live('click', function () {
        $('#containerPopupEntryDataObat').hide();
    });

    $('#btnEntryPopupCancelBarang').live('click', function () {
        $('#containerPopupEntryDataBarang').hide();
    });


    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupSaveObat').click(function (evt) {
        if (IsValid(evt, 'fsEntryObatPopup', 'mpEntryPopup'))
            cbpEntryPopupViewObat.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupSaveBarang').click(function (evt) {
        if (IsValid(evt, 'fsEntryBarangPopup', 'mpEntryPopup'))
            cbpEntryPopupViewBarang.PerformCallback('save');
        return false;
    });


    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgDeleteObat.imgLink').die('click');
    $('.imgDeleteObat.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnOAID.ClientID %>').val(entity.ID);

            cbpEntryPopupViewObat.PerformCallback('delete');
        }
    });

    $('.imgDeleteBarang.imgLink').die('click');
    $('.imgDeleteBarang.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnBID.ClientID %>').val(entity.ID);

            cbpEntryPopupViewBarang.PerformCallback('delete');
        }
    });


    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    function oncbpEntryPopupViewObatEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryDataObat').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    function oncbpEntryPopupViewBarangEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryDataBarang').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    //#region Item
    function onGetItemServiceDtFilterExpression() {
        var itemID = $('#<%=hdnItemID.ClientID %>').val();
        var hdnGCItemTypeMain = $('#<%=hdnGCItemTypeMain.ClientID %>').val();

        var filterExpression = "ItemID IN (SELECT ItemID FROM ItemService WHERE IsPackageItem = 0 AND IsPackageAllInOne = 0)";
        filterExpression += " AND ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WHERE ItemID = " + itemID + " AND IsDeleted = 0)";
        filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999' AND GCItemType IN ('" + hdnGCItemTypeMain + "')";

        return filterExpression;
    }

    function onGetObatDtFilterExpression() {
        var itemID = $('#<%=hdnObatID.ClientID %>').val();
        var hdnGCItemTypeObatMain = $('#<%=hdnGCItemTypeObatMain.ClientID %>').val();

        var filterExpression = "ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WHERE ItemID = " + itemID + " AND IsDeleted = 0)";
        filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999' AND GCItemType IN ('" + hdnGCItemTypeObatMain + "')";

        return filterExpression;
    }

    function onGetBarangDtFilterExpression() {
        var itemID = $('#<%=hdnBarangID.ClientID %>').val();
        var hdnGCItemTypeBarangMain = $('#<%=hdnGCItemTypeBarangMain.ClientID %>').val();

        var filterExpression = "ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WHERE ItemID = " + itemID + " AND IsDeleted = 0)";
        filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999' AND GCItemType IN ('" + hdnGCItemTypeBarangMain + "')";

        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', onGetItemServiceDtFilterExpression(), function (value) {
            $('#<%=txtDetailItemCode.ClientID %>').val(value);
            onTxtDetailItemCodeChanged(value);
        });
    });

    $('#lblObat.lblLink').live('click', function () {
        openSearchDialog('item', onGetObatDtFilterExpression(), function (value) {
            $('#<%=txtDetailObatCode.ClientID %>').val(value);
            ontxtDetailObatCodeChanged(value);
        });
    });

    $('#lblBarang.lblLink').live('click', function () {
        openSearchDialog('item', onGetBarangDtFilterExpression(), function (value) {
            $('#<%=txtDetailBarangCode.ClientID %>').val(value);
            ontxtDetailBarangCodeChanged(value);
        });
    });


    $('#<%=txtDetailItemCode.ClientID %>').live('change', function () {
        onTxtDetailItemCodeChanged($(this).val());
    });

    $('#<%=txtDetailObatCode.ClientID %>').live('change', function () {
        ontxtDetailObatCodeChanged($(this).val());
    });

    $('#<%=txtDetailBarangCode.ClientID %>').live('change', function () {
        ontxtDetailBarangCodeChanged($(this).val());
    });

    function onTxtDetailItemCodeChanged(value) {
        var filterExpression = onGetItemServiceDtFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGCItemType.ClientID %>').val(result.GCItemType);
                $('#<%=hdnDetailItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtDetailItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnGCItemType.ClientID %>').val('');
                $('#<%=hdnDetailItemID.ClientID %>').val('');
                $('#<%=txtDetailItemName.ClientID %>').val('');
                $('#<%=txtDetailItemCode.ClientID %>').val('');
            }
        });
    }

    function ontxtDetailObatCodeChanged(value) {
        var filterExpression = onGetObatDtFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdngcObatType.ClientID %>').val(result.GCItemType);
                $('#<%=hdnDetailObatID.ClientID %>').val(result.ItemID);
                $('#<%=txtDetailObatName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdngcObatType.ClientID %>').val('');
                $('#<%=hdnDetailObatID.ClientID %>').val('');
                $('#<%=txtDetailObatName.ClientID %>').val('');
                $('#<%=txtDetailObatCode.ClientID %>').val('');
            }
        });
    }

    function ontxtDetailBarangCodeChanged(value) {
        var filterExpression = onGetBarangDtFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdngcBarangType.ClientID %>').val(result.GCItemType);
                $('#<%=hdnDetailBarangID.ClientID %>').val(result.ItemID);
                $('#<%=txtDetailBarangName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdngcBarangType.ClientID %>').val('');
                $('#<%=hdnDetailBarangID.ClientID %>').val('');
                $('#<%=txtDetailBarangName.ClientID %>').val('');
                $('#<%=txtDetailBarangCode.ClientID %>').val('');
            }
        });
    }

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnGCItemType.ClientID %>').val(entity.GCItemType);
        $('#<%=hdnDetailItemID.ClientID %>').val(entity.DetailItemID);
        $('#<%=txtDetailItemCode.ClientID %>').val(entity.DetailItemCode);
        $('#<%=txtDetailItemName.ClientID %>').val(entity.DetailItemName1);
        $('#<%=txtDetailItemQuantity.ClientID %>').val(entity.Quantity);

        if (entity.IsAllowChanged == 'True') {
            $('#<%=chkIsAllowChangedQty.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkIsAllowChangedQty.ClientID %>').prop('checked', false);
        }

        if (entity.IsAutoPosted == 'True') {
            $('#<%=chkIsAutoPosted.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkIsAutoPosted.ClientID %>').prop('checked', false);
        }

        $('#containerPopupEntryData').show();
    });


    $('.imgEditObat.imgLink').die('click');
    $('.imgEditObat.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnOAID.ClientID %>').val(entity.ID);
        $('#<%=hdngcObatType.ClientID %>').val(entity.GCItemType);
        $('#<%=hdnDetailObatID.ClientID %>').val(entity.DetailItemID);
        $('#<%=txtDetailObatCode.ClientID %>').val(entity.DetailItemCode);
        $('#<%=txtDetailObatName.ClientID %>').val(entity.DetailItemName1);
        $('#<%=txtDetailObatQuantity.ClientID %>').val(entity.Quantity);

        if (entity.IsAllowChanged == 'True') {
            $('#<%=chkIsAllowChangedQtyObat.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkIsAllowChangedQtyObat.ClientID %>').prop('checked', false);
        }

        if (entity.IsAutoPosted == 'True') {
            $('#<%=chkObatIsAutoPosted.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkObatIsAutoPosted.ClientID %>').prop('checked', false);
        }

        $('#containerPopupEntryDataObat').show();
    });


    $('.imgEditBarang.imgLink').die('click');
    $('.imgEditBarang.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnBID.ClientID %>').val(entity.ID);
        $('#<%=hdngcBarangType.ClientID %>').val(entity.GCItemType);
        $('#<%=hdnDetailBarangID.ClientID %>').val(entity.DetailItemID);
        $('#<%=txtDetailBarangCode.ClientID %>').val(entity.DetailItemCode);
        $('#<%=txtDetailBarangName.ClientID %>').val(entity.DetailItemName1);
        $('#<%=txtDetailBarangQuantity.ClientID %>').val(entity.Quantity);

        if (entity.IsAllowChanged == 'True') {
            $('#<%=chkIsAllowChangedQtyBarang.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkIsAllowChangedQtyBarang.ClientID %>').prop('checked', false);
        }

        if (entity.IsAutoPosted == 'True') {
            $('#<%=chkBarangIsAutoPosted.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkBarangIsAutoPosted.ClientID %>').prop('checked', false);
        }

        $('#containerPopupEntryDataBarang').show();
    });

    var lastContentID = '';
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        if ($contentID == "containerDrugMS")
            $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.DRUGS);
        else if ($contentID == "containerLogistics")
            $('#<%=hdnGCItemType.ClientID %>').val(Constant.ItemGroupMaster.LOGISTIC);
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID = $contentID;
    });

    var lastContentID2 = '';
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID2 = $(this).attr('contentid');
        if ($contentID2 == "containerDrugMS")
            $('#<%=hdngcObatType.ClientID %>').val(Constant.ItemGroupMaster.DRUGS);
        else if ($contentID2 == "containerLogistics")
            $('#<%=hdngcObatType.ClientID %>').val(Constant.ItemGroupMaster.LOGISTIC);
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID2 = $contentID2;
    });

    var lastContentID3 = '';
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID3 = $(this).attr('contentid');
        if ($contentID3 == "containerDrugMS")
            $('#<%=hdngcBarangType.ClientID %>').val(Constant.ItemGroupMaster.DRUGS);
        else if ($contentID2 == "containerLogistics")
            $('#<%=hdngcBarangType.ClientID %>').val(Constant.ItemGroupMaster.LOGISTIC);
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID3 = $contentID3;
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    function oncbpEntryPopupViewObatEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryDataObat').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }

    function oncbpEntryPopupViewBarangEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryDataBarang').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingView').hide();
    }
    //#endregion   

</script>
<div style="padding: 15px;">
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li class="selected" contentid="containerService">
                <%=GetLabel("PELAYANAN") %></li>
            <li contentid="containerDrugMS">
                <%=GetLabel("OBAT & ALKES") %></li>
            <li contentid="containerLogistics">
                <%=GetLabel("BARANG UMUM") %></li>
        </ul>
    </div>
    <div id="containerDrugMS" style="display: none" class="containerTransDt">
        <input type="hidden" id="hdnObatID" value="" runat="server" />
        <input type="hidden" id="hdnGCItemTypeObatMain" value="" runat="server" />
        <div class="pageTitle">
            <%=GetLabel("Entry Obat & Alkes")%>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 70%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemServiceName3" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryDataObat" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnOAID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryObatPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory lblLink" id="lblObat">
                                            <%=GetLabel("Obat")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnDetailObatID" runat="server" value="" />
                                        <input type="hidden" id="hdngcObatType" runat="server" value="" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailObatCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailObatName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDetailObatQuantity" CssClass="number" runat="server" Width="100px" />
                                        <asp:CheckBox ID="chkIsAllowChangedQtyObat" runat="server" /><%=GetLabel("Qty Boleh Diubah (utk non-akumulasi)")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Posting Otomatis")%></label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkObatIsAutoPosted" runat="server" Checked="True" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSaveObat" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancelObat" value='<%= GetLabel("Batal")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewObat" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupViewObat"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewObat_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').hide(); }"
                            EndCallback="function(s,e){ oncbpEntryPopupViewObatEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewObat" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <div style="width: 100%; height: 400px; overflow: scroll">
                                        <asp:GridView ID="grdViewObat" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            OnRowDataBound="grdViewObat_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgEditObat imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        <img class="imgDeleteObat imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" bindingfield="ID" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" bindingfield="DetailItemID" value="<%#: Eval("DetailItemID")%>" />
                                                        <input type="hidden" bindingfield="DetailItemCode" value="<%#: Eval("DetailItemCode")%>" />
                                                        <input type="hidden" bindingfield="DetailItemName1" value="<%#: Eval("DetailItemName1")%>" />
                                                        <input type="hidden" bindingfield="Quantity" value="<%#: Eval("Quantity")%>" />
                                                        <input type="hidden" bindingfield="IsAutoPosted" value="<%#: Eval("IsAutoPosted") %>" />
                                                        <input type="hidden" bindingfield="IsAllowChanged" value="<%#: Eval("IsAllowChanged") %>" />
                                                        <input type="hidden" bindingfield="GCItemType" value="<%#: Eval("GCItemType") %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                                <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                                <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderText="Jumlah" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width: 100%; text-align: center" id="divContainerAddDataObat" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddDataObat">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerLogistics" style="display: none" class="containerTransDt">
        <input type="hidden" id="hdnBarangID" value="" runat="server" />
        <input type="hidden" id="hdnGCItemTypeBarangMain" value="" runat="server" />
        <div class="pageTitle">
            <%=GetLabel("Entry Barang Umum")%>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 70%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemServiceName2" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryDataBarang" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnBID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryBarangPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory lblLink" id="lblBarang">
                                            <%=GetLabel("Barang")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnDetailBarangID" runat="server" value="" />
                                        <input type="hidden" id="hdngcBarangType" runat="server" value="" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailBarangCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailBarangName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDetailBarangQuantity" CssClass="number" runat="server" Width="100px" />
                                        <asp:CheckBox ID="chkIsAllowChangedQtyBarang" runat="server" /><%=GetLabel("Qty Boleh Diubah (utk non-akumulasi)")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Posting Otomatis")%></label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkBarangIsAutoPosted" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSaveBarang" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancelBarang" value='<%= GetLabel("Batal")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewBarang" runat="server" Width="100%"
                        ClientInstanceName="cbpEntryPopupViewBarang" ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewBarang_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').hide(); }"
                            EndCallback="function(s,e){ oncbpEntryPopupViewBarangEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewBarang" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewBarang" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" OnRowDataBound="grdViewBarang_RowDataBound" ShowHeaderWhenEmpty="true"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEditBarang imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDeleteBarang imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" bindingfield="ID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" bindingfield="DetailItemID" value="<%#: Eval("DetailItemID")%>" />
                                                    <input type="hidden" bindingfield="DetailItemCode" value="<%#: Eval("DetailItemCode")%>" />
                                                    <input type="hidden" bindingfield="DetailItemName1" value="<%#: Eval("DetailItemName1")%>" />
                                                    <input type="hidden" bindingfield="Quantity" value="<%#: Eval("Quantity")%>" />
                                                    <input type="hidden" bindingfield="IsAutoPosted" value="<%#: Eval("IsAutoPosted") %>" />
                                                    <input type="hidden" bindingfield="IsAllowChanged" value="<%#: Eval("IsAllowChanged") %>" />
                                                    <input type="hidden" bindingfield="GCItemType" value="<%#: Eval("GCItemType") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                            <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                            <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="Jumlah" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width: 100%; text-align: center" id="divContainerAddDataBarang" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddDataBarang">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerService" class="containerTransDt" style="height: 440px; overflow-y: auto">
        <input type="hidden" id="hdnItemID" value="" runat="server" />
        <input type="hidden" id="hdnGCItemTypeMain" value="" runat="server" />
        <div class="pageTitle">
            <%=GetLabel("Entry Item Pelayanan")%>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 70%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory lblLink" id="lblItem">
                                            <%=GetLabel("Item")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnDetailItemID" runat="server" value="" />
                                        <input type="hidden" id="hdnGCItemType" runat="server" value="" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCode" CssClass="required" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDetailItemQuantity" CssClass="number" runat="server" Width="100px" />
                                        <asp:CheckBox ID="chkIsAllowChangedQty" runat="server" /><%=GetLabel("Qty Boleh Diubah (utk non-akumulasi)")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Posting Otomatis")%></label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsAutoPosted" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').hide(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" bindingfield="ID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" bindingfield="DetailItemID" value="<%#: Eval("DetailItemID")%>" />
                                                    <input type="hidden" bindingfield="DetailItemCode" value="<%#: Eval("DetailItemCode")%>" />
                                                    <input type="hidden" bindingfield="DetailItemName1" value="<%#: Eval("DetailItemName1")%>" />
                                                    <input type="hidden" bindingfield="Quantity" value="<%#: Eval("Quantity")%>" />
                                                    <input type="hidden" bindingfield="IsAutoPosted" value="<%#: Eval("IsAutoPosted") %>" />
                                                    <input type="hidden" bindingfield="IsAllowChanged" value="<%#: Eval("IsAllowChanged") %>" />
                                                    <input type="hidden" bindingfield="GCItemType" value="<%#: Eval("GCItemType") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                            <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                            <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="Jumlah" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddData">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
</div>
