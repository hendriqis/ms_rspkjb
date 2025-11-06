<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogisticsQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LogisticsQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_LogisticsQuickPicksCtl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterItem').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpView.PerformCallback('refresh');
        }
    });
    
    $('#<%=rblFilterItemType.ClientID %>').live('change', function () {
        cbpView.PerformCallback('refresh');
    });

    //#region Item Group
    $('#lblItemGroupDrugLogistic.lblLink').click(function () {
        var filterExpression = "GCItemType IN ('" + Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN + "') AND IsDeleted = 0";

        var value = $('#<%=rblFilterItemType.ClientID %>').find('input:checked').val();
        if (value == 'nutrition') {
            filterExpression += " AND GCItemType = '" + Constant.ItemType.BAHAN_MAKANAN + "'";
        } else {
            filterExpression += " AND GCItemType = '" + Constant.ItemType.BARANG_UMUM + "'";
        }

        openSearchDialog('itemgroup', filterExpression, function (value) {
            $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').val(value);
            onTxtItemGroupDrugLogisticCodeChanged(value);
        });
    });

    $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').change(function () {
        onTxtItemGroupDrugLogisticCodeChanged($(this).val());
    });

    function onTxtItemGroupDrugLogisticCodeChanged(value) {
        var filterExpression = "ItemGroupCode = '" + value + "'";
        Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemGroupDrugLogisticID.ClientID %>').val(result.ItemGroupID);
                $('#<%=txtItemGroupDrugLogisticName.ClientID %>').val(result.ItemGroupName1);
            }
            else {
                $('#<%=hdnItemGroupDrugLogisticID.ClientID %>').val('');
                $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').val('');
                $('#<%=txtItemGroupDrugLogisticName.ClientID %>').val('');
            }
            getCheckedMember();
            cbpView.PerformCallback('refresh');
        });
    }
    //#endregion

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                var isQtyChecked = "0";
                var qtyBeginList = $('#<%=hdnSelectedMemberQty.ClientID %>').val().split(",");

                var i = 0;
                while (i < qtyBeginList.length) {
                    var qtyBegin = qtyBeginList[i];
                    if (qtyBegin.includes(".")) {
                        var qtyCheckDesimalList = qtyBegin.split(".");
                        var qtyCheckDesimal = qtyCheckDesimalList[1];
                        if (qtyCheckDesimal.length > 2) {
                            isQtyChecked = "1";
                            break;
                        }
                    }

                    i++;
                }

                if (isQtyChecked == "1") {
                    errMessage.text = 'Maksimal digit desimal belakang koma adalah 2 digit.';
                    return false;
                } else {
                    return true;
                }
            }
            else {
                errMessage.text = 'Harap pilih item terlebih dahulu.';
                return false;
            }
        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var qty = $(this).find('.txtQty').val();
            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCboLocationValueChanged() {
        getCheckedMember();
        cbpView.PerformCallback('refresh');
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            var isDisplayPrice = $('#<%=hdnIsDisplayPrice.ClientID %>').val();
            $selectedTr = $(this).closest('tr');
            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{ItemTariff}/g, '0.00');
            $newTr = $($newTr);

            $newTr.find(".txtUnitPrice").attr("disabled", "disabled");
            if (isDisplayPrice == "0") {
                $newTr.find(".txtUnitPrice").attr("style", "display:none");
            } else {
                $newTr.find(".txtUnitPrice").removeAttr("style");
            }

            $newTr.insertBefore($('#trFooter'));
        }
        else {
            var id = $(this).closest('tr').find('.keyField').html();
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItem .chkIsSelected2').die('change');
    $('#tblSelectedItem .chkIsSelected2').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var id = $selectedTr.find('.keyField').val();
            var isFound = false;
            $('#<%=grdView.ClientID %> tr').each(function () {
                if (id == $(this).find('.keyField').html()) {
                    $(this).find('.chkIsSelected').find('input').prop('checked', false);
                    isFound = true;
                }
            });
            if (!isFound) {
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });

    $('#btnCalculate').live('click', function () {
        showLoadingPanel();
        $('#tblSelectedItem tr.trSelectedItem').each(function () {
            var itemID = $(this).find('.keyField').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitIDCtl.ClientID %>').val();
            var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var classID = $('#<%=hdnChargesClassID.ClientID %>').val();
            var qty = $(this).find('td input.txtQty.number.min').val();
            var isDisplayPrice = $('#<%=hdnIsDisplayPrice.ClientID %>').val();
            if (deptID == 'INPATIENT') {
                classID = cboDrugChargeClassIDCtl.GetValue();
            }
            var date = Methods.getDatePickerDate($('#<%=hdnTransactionDateQuickPicksCtl.ClientID %>').val());
            var trxDate = Methods.dateToYMD(date);
            var price = 0;
            if (isDisplayPrice == "1") {
                var isPackageItemAccumulated = 0;
                Methods.getObject("GetItemServiceList", "ItemID = '" + itemID + "'", function (resultItem) {
                    if (resultItem != null) {
                        if (resultItem.IsPackageItem) {
                            if (resultItem.IsUsingAccumulatedPrice) {
                                isPackageItemAccumulated = 1;
                            }
                        }
                    }
                });

                if (isPackageItemAccumulated != 1) {
                    Methods.getItemTariff(registrationID, visitID, classID, itemID, trxDate, function (result) {
                        if (result != null) {
                            price = result.Price * qty;

                            var discountAmount = parseFloat(result.DiscountAmount);
                            var discountAmountComp1 = parseFloat(result.DiscountAmountComp1);
                            var discountAmountComp2 = parseFloat(result.DiscountAmountComp2);
                            var discountAmountComp3 = parseFloat(result.DiscountAmountComp3);

                            var isDiscountUsedComp = result.IsDiscountUsedComp ? '1' : '0';

                            var isDicountInPercentage = (result.IsDiscountInPercentage ? '1' : '0');
                            var isDicountInPercentageComp1 = (result.IsDiscountInPercentageComp1 ? '1' : '0');
                            var isDicountInPercentageComp2 = (result.IsDiscountInPercentageComp2 ? '1' : '0');
                            var isDicountInPercentageComp3 = (result.IsDiscountInPercentageComp3 ? '1' : '0');

                            var tariff = parseFloat(result.Price);
                            var tariffComp1 = parseFloat(result.PriceComp1);
                            var tariffComp2 = parseFloat(result.PriceComp2);
                            var tariffComp3 = parseFloat(result.PriceComp3);

                            var discountTotal = 0;
                            var discountComp1 = 0;
                            var discountComp2 = 0;
                            var discountComp3 = 0;

                            if (isDiscountUsedComp == "1") {
                                if (tariffComp1 > 0) {
                                    if (isDicountInPercentageComp1 == "1") {
                                        discountComp1 = tariffComp1 * discountAmountComp1 / 100;
                                    } else {
                                        discountComp1 = discountAmountComp1;
                                    }
                                }

                                if (tariffComp2 > 0) {
                                    if (isDicountInPercentageComp2 == "1") {
                                        discountComp2 = tariffComp2 * discountAmountComp2 / 100;
                                    } else {
                                        discountComp2 = discountAmountComp2;
                                    }
                                }

                                if (tariffComp3 > 0) {
                                    if (isDicountInPercentageComp3 == "1") {
                                        discountComp3 = tariffComp3 * discountAmountComp3 / 100;
                                    } else {
                                        discountComp3 = discountAmountComp3;
                                    }
                                }
                            } else {
                                if (tariffComp1 > 0) {
                                    discountComp1 = tariffComp1 * discountAmount / 100;
                                }
                                if (tariffComp2 > 0) {
                                    discountComp2 = tariffComp2 * discountAmount / 100;
                                }
                                if (tariffComp3 > 0) {
                                    discountComp3 = tariffComp3 * discountAmount / 100;
                                }
                            }
                            discountTotal = (discountComp1 + discountComp2 + discountComp3) * qty;
                            price = price - discountTotal;
                        }
                    });
                    $(this).find('.txtUnitPrice input.txtUnitPrice.number.min').val(price.formatMoney(2, '.', ','));
                }
                else {
                    var discountAmount = 0;
                    var discountAmountComp1 = 0;
                    var discountAmountComp2 = 0;
                    var discountAmountComp3 = 0;

                    var isDiscountUsedComp = '0';

                    var isDicountInPercentage = '0';
                    var isDicountInPercentageComp1 = '0';
                    var isDicountInPercentageComp2 = '0';
                    var isDicountInPercentageComp3 = '0';

                    var tariff = 0;
                    var tariffComp1 = 0;
                    var tariffComp2 = 0;
                    var tariffComp3 = 0;

                    var discountTotal = 0;
                    var discountComp1 = 0;
                    var discountComp2 = 0;
                    var discountComp3 = 0;

                    var priceTotal = 0;

                    var filterItemDetail = "ItemID = " + itemID + " AND IsDeleted = 0";
                    Methods.getListObject('GetvItemServiceDtList', filterItemDetail, function (resultItemDetail) {
                        for (i = 0; i < resultItemDetail.length; i++) {
                            Methods.getItemTariff(registrationID, visitID, classID, resultItemDetail[i].DetailItemID, trxDate, function (resultPrice) {
                                if (resultPrice != null) {
                                    var qtyTotal = qty * resultItemDetail[i].Quantity;
                                    var priceTemp = resultPrice.Price * qtyTotal;

                                    var discountAmount = parseFloat(resultPrice.DiscountAmount);
                                    var discountAmountComp1 = parseFloat(resultPrice.DiscountAmountComp1);
                                    var discountAmountComp2 = parseFloat(resultPrice.DiscountAmountComp2);
                                    var discountAmountComp3 = parseFloat(resultPrice.DiscountAmountComp3);

                                    var isDiscountUsedComp = resultPrice.IsDiscountUsedComp ? '1' : '0';

                                    var isDicountInPercentage = (resultPrice.IsDiscountInPercentage ? '1' : '0');
                                    var isDicountInPercentageComp1 = (resultPrice.IsDiscountInPercentageComp1 ? '1' : '0');
                                    var isDicountInPercentageComp2 = (resultPrice.IsDiscountInPercentageComp2 ? '1' : '0');
                                    var isDicountInPercentageComp3 = (resultPrice.IsDiscountInPercentageComp3 ? '1' : '0');

                                    var tariff = parseFloat(resultPrice.Price);
                                    var tariffComp1 = parseFloat(resultPrice.PriceComp1);
                                    var tariffComp2 = parseFloat(resultPrice.PriceComp2);
                                    var tariffComp3 = parseFloat(resultPrice.PriceComp3);

                                    var discountTotal = 0;
                                    var discountComp1 = 0;
                                    var discountComp2 = 0;
                                    var discountComp3 = 0;

                                    if (isDiscountUsedComp == "1") {
                                        if (tariffComp1 > 0) {
                                            if (isDicountInPercentageComp1 == "1") {
                                                discountComp1 = tariffComp1 * discountAmountComp1 / 100;
                                            } else {
                                                discountComp1 = discountAmountComp1;
                                            }
                                        }

                                        if (tariffComp2 > 0) {
                                            if (isDicountInPercentageComp2 == "1") {
                                                discountComp2 = tariffComp2 * discountAmountComp2 / 100;
                                            } else {
                                                discountComp2 = discountAmountComp2;
                                            }
                                        }

                                        if (tariffComp3 > 0) {
                                            if (isDicountInPercentageComp3 == "1") {
                                                discountComp3 = tariffComp3 * discountAmountComp3 / 100;
                                            } else {
                                                discountComp3 = discountAmountComp3;
                                            }
                                        }
                                    } else {
                                        if (tariffComp1 > 0) {
                                            discountComp1 = tariffComp1 * discountAmount / 100;
                                        }
                                        if (tariffComp2 > 0) {
                                            discountComp2 = tariffComp2 * discountAmount / 100;
                                        }
                                        if (tariffComp3 > 0) {
                                            discountComp3 = tariffComp3 * discountAmount / 100;
                                        }
                                    }
                                    discountTotal = (discountComp1 + discountComp2 + discountComp3) * qtyTotal;
                                    priceTotal += (priceTemp - discountTotal);
                                }
                            });
                        }
                    });
                    $(this).find('.txtUnitPrice input.txtUnitPrice.number.min').val(priceTotal.formatMoney(2, '.', ','));
                }
            }
        });
        hideLoadingPanel();
    });
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
            </td>
            <td>${ItemName1}</td>
            <td><input type="text" validationgroup="mpDrugsQuickPicks" class="txtQty number min" min="0" value="1" style="width:60px" /></td>
              <td style="text-align:right" class="txtUnitPrice"><input type="text" class="txtUnitPrice number min" min="0" style="width:80px" value="${ItemTariff}" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowOverissued" runat="server" value="0" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnIsAccompany" runat="server" value="" />
    <input type="hidden" id="hdnIsDrugChargesJustDistributionQP" runat="server" value="0" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
      <input type="hidden" id="hdnChargesClassID" runat="server" value="" />
    <input type="hidden" id="hdnIsDisplayPrice" runat="server" value="0" />
    <input type="hidden" id="hdnIsUsingValidateDigitDecimal" runat="server" value="0" />
     <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
     <input type="hidden" id="hdnIsBPJSRegistration" runat="server" value="" />
     <input type="hidden" id="hdnTransactionDateQuickPicksCtl" runat="server" value="" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Lokasi")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboLocation" ClientInstanceName="cboLocation" Width="100%"
                    runat="server" OnCallback="cboLocation_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }"
                        ValueChanged="function(s,e){ onCboLocationValueChanged(e) }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Filter")%></label>
            </td>
            <td colspan="3">
                <asp:RadioButtonList runat="server" ID="rblFilterItemType" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Barang Umum" Value="logistic" Selected="True" />
                    <asp:ListItem Text="Bahan Makanan" Value="nutrition" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblItemGroupDrugLogistic">
                    <%=GetLabel("Kelompok Barang")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnItemGroupDrugLogisticID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtItemGroupDrugLogisticCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupDrugLogisticName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Kelas Tagihan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboDrugChargeClassIDCtl" ClientInstanceName="cboDrugChargeClassIDCtl"
                    Width="100%" runat="server">
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr id="trCalculate" runat="server">
                <td>
                </td>
                <td>
                    <input type="button" id="btnCalculate" value="C a l c u l a t e" class="btnCalculate w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                </td>
            </tr>
    </table>
    <div style="height: 400px; overflow-y: scroll;">
        <table style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Item yang tersedia :" ItemStyle-CssClass="tdItemName1" />
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                                HeaderText="Stok">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 20px">
                                                                <%#:Eval("QuantityEND") %>
                                                            </td>
                                                            <td style="width: 5px">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 50px">
                                                                <%#: Eval("ItemUnit") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsDrugsQuickPicks">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                            <tr id="trHeader2">
                                <th style="width: 40px">
                                    &nbsp;
                                </th>
                                <th align="center">
                                    <%=GetLabel("Item yang telah dipilih :")%>
                                </th>
                                <th align="center" style="width: 60px">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                                 <th align="right" style="width: 80px;" id="thHarga" runat="server">
                                    <%=GetLabel("Harga")%>
                                </th>
                            </tr>
                            <tr id="trFooter">
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
