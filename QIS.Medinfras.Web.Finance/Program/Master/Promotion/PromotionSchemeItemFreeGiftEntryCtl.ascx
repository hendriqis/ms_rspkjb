<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionSchemeItemFreeGiftEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.PromotionSchemeItemFreeGiftEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_promotionTypeDepartmentEntryCtl">
    $(function () {
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtQty.ClientID %>').val('');
        $('#<%=txtItemUnit.ClientID %>').val('');
        $('#<%=hdnGCItemUnit.ClientID %>').val('');
        $('#<%=hdnConversionFactor.ClientID %>').val('');
        cboItemType.SetValue('');
        cboItemType.SetEnabled(true);

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        var unit = $('#<%=txtItemUnit.ClientID %>').val();
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            if (unit != '') {
                cbpEntryPopupView.PerformCallback('save');
            }
            else {
                displayMessageBox("SKEMA PROMO (FREE GIFT)", "Satuan Wajib Di isi.");
                return false;            
            }
        }
        else {
            return false;
        }
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        displayConfirmationMessageBox('DELETE', 'Are you sure want to delete?', function (result) {
            if (result) {
                $('#<%=hdnID.ClientID %>').val(ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        var gcItemType = $row.find('.hdnGCItemType').val();
        var ItemID = $row.find('.hdnItemID').val();
        var ItemCode = $row.find('.hdnItemCode').val();
        var ItemName = $row.find('.tdItemName').html().trim();
        var qty = $row.find('.hdnQty').val();
        var convertionFactor = $row.find('.hdnConversionFactor').val();
        var gcItemUnit = $row.find('.hdnGCItemUnit').val();
        var itemUnitName = $row.find('.hdnItemUnitName').val();
        
        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(ItemID);
        cboItemType.SetValue(gcItemType);
        cboItemType.SetEnabled(false);
        $('#<%=txtItemCode.ClientID %>').val(ItemCode);
        $('#<%=txtItemName.ClientID %>').val(ItemName);
        $('#<%=txtQty.ClientID %>').val(qty);
        $('#<%=txtItemUnit.ClientID %>').val(itemUnitName);
        $('#<%=hdnGCItemUnit.ClientID %>').val(gcItemUnit);
        $('#<%=hdnConversionFactor.ClientID %>').val(convertionFactor);

        $('#containerPopupEntryData').show();
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
        hideLoadingPanel();
    }

    //#region Item 
    function onGetItemCodeFilterExpression() {
        var filterExpression = "ItemID NOT IN (SELECT ItemID FROM PromotionSchemeItemFreeGift WHERE PromotionSchemeID = " + $('#<%=hdnPromotionSchemeID.ClientID %>').val() + " AND IsDeleted = 0) AND IsDeleted = 0";
        if (cboItemType.GetValue() != '') {
            filterExpression += " AND GCItemType = '" + cboItemType.GetValue() + "'";
        }
        return filterExpression;
    }

    $('#lblItem.lblLink').click(function () {
        if (cboItemType.GetValue() != null && cboItemType.GetValue() != '') {
            openSearchDialog('item', onGetItemCodeFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtPromotionSchemeItemCodeChanged(value);
            });
        }
        else {
            displayMessageBox("SEARCH", "Jenis Item belum dipilih.");
        }
    });

    $('#<%=txtItemCode.ClientID %>').change(function () {
        onTxtPromotionSchemeItemCodeChanged($(this).val());
    });

    function onTxtPromotionSchemeItemCodeChanged(value) {
        var filterExpression = onGetItemCodeFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=hdnGCItemType.ClientID %>').val(result.GCItemType);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=hdnGCItemType.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboItemTypeChanged() {
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=hdnGCItemType.ClientID %>').val('');
    }

    function onGetItemUnitFilterExpression() {
        var itemDtID = $('#<%=hdnItemID.ClientID %>').val();
        var filterExpression = "ItemID = " + itemDtID;
        return filterExpression;
    }

    $('#lblItemUnit.lblLink').live('click', function () {
        var itemDtID = $('#<%=hdnItemID.ClientID %>').val();
        if (itemDtID != '') {
            openSearchDialog('itemalternateunit', onGetItemUnitFilterExpression(), function (value) {
                $('#<%=txtItemUnit.ClientID %>').val(value);
                ontxtDetailObatItemUnitCodeChanged(value);
            });
        }
        else {
            showToast('Warning', 'Pilih Item Terlebih dahulu');
        }
    });

    function ontxtDetailObatItemUnitCodeChanged(value) {
        var filterExpression = onGetItemUnitFilterExpression() + " AND GCAlternateUnit = '" + value + "'";
        Methods.getObject('GetvItemAlternateUnitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtItemUnit.ClientID %>').val(result.AlternateUnit);
                $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCAlternateUnit);
                $('#<%=hdnConversionFactor.ClientID %>').val(parseFloat(result.ConversionFactor));
            }
            else {
                $('#<%=txtItemUnit.ClientID %>').val('');
                $('#<%=hdnGCItemUnit.ClientID %>').val('');
                $('#<%=hdnConversionFactor.ClientID %>').val('');
            }
        });
    }

    $('#<%=txtQty.ClientID %>').live('change', function () {
        var value = $(this).val();
        $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnPromotionSchemeID" value="" runat="server" />
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
                                <%=GetLabel("Skema Promo")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPromotionScheme" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <input type="hidden" runat="server" id="hdnItemID" />
                    <input type="hidden" runat="server" id="hdnGCItemType" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 180px" />
                                <col style="width: 100px" />
                                <col style="width: 3px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jenis Item")%></label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" runat="server"
                                        Width="100%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jumlah")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQty" CssClass="number required" runat="server" Width="100px" />
                                </td>
                            </tr>
                            <tr id="trDrugItemUnit" runat="server">
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItemUnit">
                                        <%=GetLabel("Satuan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnGCItemUnit" runat="server" value="" />
                                    <input type="hidden" id="hdnConversionFactor" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemUnit" ReadOnly="true" CssClass="required" Width="100px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div style="padding-bottom: 10px">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' class="btnEntryPopupSave w3-btn w3-hover-blue" />
                                        </td>
                                        <td>
                                            <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 70px">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Jenis Item")%>
                                                </th>
                                                <th style="width: 250px" align="left">
                                                    <%=GetLabel("Item")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Jumlah")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="6">
                                                    <%=GetLabel("Konfigurasi Promo tidak tersedia")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 70px" align="center">
                                                    &nbsp;
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Jenis Item")%>
                                                </th>
                                                <th style="width: 250px" align="left">
                                                    <%=GetLabel("Item")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Jumlah")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Satuan")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td style="width: 1px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnGCItemType" value="<%#: Eval("GCItemType")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnQty" value="<%#: Eval("Quantity")%>" />
                                                <input type="hidden" class="hdnConversionFactor" value="<%#: Eval("ConversionFactor")%>" />
                                                <input type="hidden" class="hdnGCItemUnit" value="<%#: Eval("GCItemUnit")%>" />
                                                <input type="hidden" class="hdnItemUnitName" value="<%#: Eval("ItemUnitName")%>" />
                                            </td>
                                            <td class="tdItemTypeName">
                                                <%#: Eval("ItemTypeName")%>
                                            </td>
                                            <td class="tdItemName">
                                                <%#: Eval("ItemName1")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("Quantity")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("ItemUnitName")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
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
</div>
