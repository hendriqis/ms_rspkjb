<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionSchemeItemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.PromotionSchemeItemEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_promotionTypeDepartmentEntryCtl">
    $(function () {
        $('#<%=txtTariff.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariffComp1.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariffComp2.ClientID %>').attr("disabled", "disabled");
        $('#<%=txtTariffComp3.ClientID %>').attr("disabled", "disabled");

        $('#<%=chkIsUsePromotionPrice.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=txtTariff.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariffComp1.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariffComp2.ClientID %>').removeAttr("disabled");
                $('#<%=txtTariffComp3.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%=txtTariff.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariffComp1.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariffComp2.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtTariffComp3.ClientID %>').attr("disabled", "disabled");

                $('#<%=txtTariff.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariffComp1.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariffComp2.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTariffComp3.ClientID %>').val('0').trigger('changeValue');
            }
        });
    });


    $('#<%=txtTariff.ClientID %>').change(function () {
        $(this).blur();
        var total = parseFloat($(this).attr('hiddenVal'));
        if (isNaN(total)) {
            $('#<%:txtTariff.ClientID %>').val('0').trigger('changeValue');
            total = 0;
        }
        else {
            if (total < 0) {
                $('#<%:txtTariff.ClientID %>').val('0').trigger('changeValue');
                total = 0;
            }
        }

        var component1 = parseFloat($('#<%=txtTariffComp1.ClientID %>').attr('hiddenVal'));
        var component2 = parseFloat($('#<%=txtTariffComp2.ClientID %>').attr('hiddenVal'));
        var component3 = parseFloat($('#<%=txtTariffComp3.ClientID %>').attr('hiddenVal'));

        if (isNaN(component1)) component1 = 0;
        if (isNaN(component2)) component2 = 0;
        if (isNaN(component3)) component3 = 0;

        if (component1 < 0) component1 = 0;
        if (component2 < 0) component2 = 0;
        if (component3 < 0) component3 = 0;

        var component1 = total - (component2 + component3);
        if (component1 < 0 || component1 == null) {
            $('#<%=txtTariffComp1.ClientID %>').val(total).trigger('changeValue');
            $('#<%=txtTariffComp2.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtTariffComp3.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            $('#<%=txtTariffComp1.ClientID %>').val(component1).trigger('changeValue');
            $('#<%=txtTariffComp2.ClientID %>').val(component2).trigger('changeValue');
            $('#<%=txtTariffComp3.ClientID %>').val(component3).trigger('changeValue');
        }
    });

    $('#<%=txtTariffComp1.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp1 = parseFloat($(this).attr('hiddenVal'));
        if (isNaN(tariffComp1)) {
            $('#<%:txtTariffComp1.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp1 < 0) {
                $('#<%:txtTariffComp1.ClientID %>').val('0').trigger('changeValue');
            }
        }
        calculateTariffTotal();
    });

    $('#<%=txtTariffComp2.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp2 = parseFloat($(this).attr('hiddenVal'));
        if (isNaN(tariffComp2)) {
            $('#<%:txtTariffComp2.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp2 < 0) {
                $('#<%:txtTariffComp2.ClientID %>').val('0').trigger('changeValue');
            }
        }
        calculateTariffTotal();
    });

    $('#<%=txtTariffComp3.ClientID %>').change(function () {
        $(this).blur();
        var tariffComp3 = parseFloat($(this).attr('hiddenVal'));
        if (isNaN(tariffComp3)) {
            $('#<%:txtTariffComp3.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (tariffComp3 < 0) {
                $('#<%:txtTariffComp3.ClientID %>').val('0').trigger('changeValue');
            }
        }
        calculateTariffTotal();
    });

    function calculateTariffTotal() {
        var total = 0;
        var comp1 = parseFloat($('#<%=txtTariffComp1.ClientID %>').attr('hiddenVal'));
        var comp2 = parseFloat($('#<%=txtTariffComp2.ClientID %>').attr('hiddenVal'));
        var comp3 = parseFloat($('#<%=txtTariffComp3.ClientID %>').attr('hiddenVal'));

        if (isNaN(comp1)) comp1 = 0;
        if (isNaN(comp2)) comp2 = 0;
        if (isNaN(comp3)) comp3 = 0;

        total = comp1 + comp2 + comp3;
        $('#<%=txtTariff.ClientID %>').val(total).trigger('changeValue');
    }

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');

        $('#<%=chkIsUsePromotionPrice.ClientID %>').prop('checked', false);
        $('#<%=txtTariff.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariffComp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariffComp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtTariffComp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
        cboItemType.SetEnabled(true);

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
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
        var ItemName = $row.find('.tdItemName').html();

        var revenueSharingID = $row.find('.hdnRevenueSharingID').val();
        var revenueSharingCode = $row.find('.hdnRevenueSharingCode').val();
        var revenueSharingName = $row.find('.tdRevenueSharingName').html();

        var tariff = $row.find('.tdTariff').html();
        var tariffComp1 = $row.find('.hdnTariffComp1').val();
        var tariffComp2 = $row.find('.hdnTariffComp2').val();
        var tariffComp3 = $row.find('.hdnTariffComp3').val();
        var discountAmount = $row.find('.tdDiscountAmount').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(ItemID);
        cboItemType.SetValue(gcItemType);
        cboItemType.SetEnabled(false);
        $('#<%=txtItemCode.ClientID %>').val(ItemCode);
        $('#<%=txtItemName.ClientID %>').val(ItemName);

        $('#<%=chkIsUsePromotionPrice.ClientID %>').prop('checked', tariff != '-');
        if (tariff != '-') {
            $('#<%=txtTariff.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariffComp1.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariffComp2.ClientID %>').removeAttr("disabled");
            $('#<%=txtTariffComp3.ClientID %>').removeAttr("disabled");
        }
        $('#<%=txtTariff.ClientID %>').val(tariff.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariffComp1.ClientID %>').val(tariffComp1.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariffComp2.ClientID %>').val(tariffComp2.replace('-', '0')).trigger('changeValue');
        $('#<%=txtTariffComp3.ClientID %>').val(tariffComp3.replace('-', '0')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage.ClientID %>').prop('checked', discountAmount.indexOf("%") > -1);
        $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

        $('#<%=hdnRevenueSharingID.ClientID %>').val(revenueSharingID);
        $('#<%=txtRevenueSharingCode.ClientID %>').val(revenueSharingCode);
        $('#<%=txtRevenueSharingName.ClientID %>').val(revenueSharingName);

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
        var filterExpression = "ItemID NOT IN (SELECT ItemID FROM PromotionSchemeItem WHERE PromotionSchemeID = " + $('#<%=hdnPromotionSchemeID.ClientID %>').val() + " AND IsDeleted = 0) AND IsDeleted = 0";
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

    //#region Revenue Sharing
    function onGetRevenueSharingMasterCodeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblRevenueSharing.lblLink').live('click', function () {
        openSearchDialog('revenuesharing', onGetRevenueSharingMasterCodeFilterExpression(), function (value) {
            $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
            onTxtRevenueSharingMasterCodeChanged(value);
        });
    });

    $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
        onTxtRevenueSharingMasterCodeChanged($(this).val());
    });

    function onTxtRevenueSharingMasterCodeChanged(value) {
        var filterExpression = onGetRevenueSharingMasterCodeFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
        Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
            }
            else {
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                $('#<%=txtRevenueSharingName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%:txtDiscountAmount.ClientID %>').live('change', function () {
        var value = $('#<%:txtDiscountAmount.ClientID %>').val();
        var isPercentage = $('#<%:chkIsDiscountInPercentage.ClientID %>').is(':checked');        

        if (isNaN(value)) {
            $('#<%:txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
        }
        else {
            if (value < 0) {
                $('#<%:txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
            }
        }

        if (isPercentage) {
            if (value > 100) {
                $('#<%:txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
            }
        }
    });

    $('#<%:chkIsDiscountInPercentage.ClientID %>').live('change', function () {
        var value = $('#<%:txtDiscountAmount.ClientID %>').val();
        var token = ",";
        var newToken = "";
        value = value.split(token).join(newToken);
        var isPercentage = $('#<%:chkIsDiscountInPercentage.ClientID %>').is(':checked');

        if (isPercentage) {
            if (value > 100) {
                $('#<%:txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
            }
        }
    });

    function onCboItemTypeChanged() {
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=hdnGCItemType.ClientID %>').val('');
    }
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
                    <input type="hidden" id="hdnTariffComp1Text" runat="server" value="" />
                    <input type="hidden" id="hdnTariffComp2Text" runat="server" value="" />
                    <input type="hidden" id="hdnTariffComp3Text" runat="server" value="" />
                    <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
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
                                <td colspan="4">
                                    <table cellpadding="1" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 180px" />
                                            <col style="width: 20px" />
                                            <col style="width: 140px" />
                                            <col style="width: 20px" />
                                            <col style="width: 140px" />
                                            <col style="width: 20px" />
                                            <col style="width: 140px" />
                                        </colgroup>
                                        <tr>
                                            <td colspan="6">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Penggunaan Harga Khusus")%></label>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:CheckBox ID="chkIsUsePromotionPrice" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTariff" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align: right">
                                                <label class="lblNormal">
                                                    <%=GetTariffComponent1Text()%></label>
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTariffComp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align: right">
                                                <label class="lblNormal">
                                                    <%=GetTariffComponent2Text()%></label>
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTariffComp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="text-align: right">
                                                <label class="lblNormal">
                                                    <%=GetTariffComponent3Text()%></label>
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTariffComp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                            <td>
                                                &nbsp
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <div class="lblComponent">
                                                    <%=GetLabel("%")%></div>
                                            </td>
                                            <td>
                                                <div class="lblComponent" style="text-align: right">
                                                    <%=GetLabel("Nilai")%></div>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Pemberian Diskon")%></label>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style='display:none'>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblRevenueSharing">
                                        <%=GetLabel("Formula Jasa Medis")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRevenueSharingCode" CssClass="required" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" />
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
                                                    <%=GetLabel("Kenaikan Harga")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Diskon")%>
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
                                                    <%=GetLabel("Harga Khusus")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Diskon")%>
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
                                                <input type="hidden" class="hdnRevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                <input type="hidden" class="hdnRevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                <input type="hidden" class="hdnTariffComp1" value="<%#: Eval("DisplayTariffComp1")%>" />
                                                <input type="hidden" class="hdnTariffComp2" value="<%#: Eval("DisplayTariffComp2")%>" />
                                                <input type="hidden" class="hdnTariffComp3" value="<%#: Eval("DisplayTariffComp3")%>" />
                                            </td>
                                            <td class="tdItemTypeName">
                                                <%#: Eval("ItemTypeName")%>
                                            </td>
                                            <td class="tdItemName">
                                                <%#: Eval("ItemName1")%>
                                            </td>
                                            <td class="tdTariff" align="right">
                                                <%#: Eval("DisplayTariff")%>
                                            </td>
                                            <td class="tdDiscountAmount" align="right">
                                                <%#: Eval("DisplayDiscountAmount")%>
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
