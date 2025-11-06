<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_testorderquickpicksctl">
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
            cbpViewPopup.PerformCallback('refresh');
        }
    });

    $('#tblSelectedItem .txtQty').live('change', function () {
        $row = $(this).closest('tr');
        totalPayer = 0;
        totalPatient = 0;
        grandTotal = 0;
        $('#tblSelectedItem tr').each(function () {
            if ($(this).find('.keyField').val() != undefined) {
                calculateTariffEstimation($(this));
            }
        });
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
    }
    //#region Diagnose
    $('#lblDiagnose.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseID.ClientID %>').val(value);
            onTxtDiagnoseIDChanged(value);
        });
    });

    $('#<%=txtDiagnoseID.ClientID %>').live('change', function () {
        onTxtDiagnoseIDChanged($(this).val());
    });

    function onTxtDiagnoseIDChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%=txtDiagnoseID.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberItemQty = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var qty = $(this).find('.txtItemQty').val();
            lstSelectedMember.push(key);
            lstSelectedMemberItemQty.push(qty);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberItemQty.ClientID %>').val(lstSelectedMemberItemQty.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopUp"), pageCount, function (page) {
            getCheckedMember();
            cbpViewPopup.PerformCallback('changepage|' + page);
        });

        //#region Item Group
        $('#lblItemGroup.lblLink').click(function () {
            var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
            openSearchDialog('itemgroup', filterExpression, function (value) {
                $('#<%=txtItemGroupCode.ClientID %>').val(value);
                onTxtItemGroupCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupCode.ClientID %>').change(function () {
            onTxtItemGroupCodeChanged($(this).val());
        });

        function onTxtItemGroupCodeChanged(value) {
            var filterExpression = "ItemGroupCode = '" + value + "'";
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
                getCheckedMember();
                cbpViewPopup.PerformCallback('refresh');
            });
        }
        //#endregion
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopUp"), pageCount, function (page) {
                getCheckedMember();
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
        else { }
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
            $newTr.insertAfter($('#trHeader2'));

            $newTr.find(".txtUnitPrice").attr("disabled", "disabled");
            if (isDisplayPrice == "0") {
                $newTr.find(".txtUnitPrice").attr("style", "display:none");
            } else {
                $newTr.find(".txtUnitPrice").removeAttr("style");
            }
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
            var registrationID = $('#<%=hdnRegistrationIDCtl.ClientID %>').val();
            var visitID = $('#<%=hdnVisitIDCtl.ClientID %>').val();
            var classID = $('#<%=hdnChargesClassIDCtl.ClientID %>').val();
            var visitID = $('#<%=hdnVisitIDCtl.ClientID %>').val();
            var isDisplayPrice = $('#<%=hdnIsDisplayPrice.ClientID %>').val();
            var qty = $(this).find('td input.txtItemQty.number.min').val();
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
                            discountTotal = (discountComp1 + discountComp2 + discountComp3);
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
                                    var priceTemp = resultPrice.Price * resultItemDetail[i].Quantity;

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
                                    discountTotal = (discountComp1 + discountComp2 + discountComp3) * resultItemDetail[i].Quantity;
                                    priceTotal += (priceTemp - discountTotal);
                                }
                            });
                        }
                        priceTotal = priceTotal * qty;
                    });
                    $(this).find('.txtUnitPrice input.txtUnitPrice.number.min').val(priceTotal.formatMoney(2, '.', ','));
                }
            }
        });
        hideLoadingPanel();
    });
    //#region Physician
    function onGetPhysicianExecutorFilterExpression() {
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID= '" + $('#<%=hdnHealthcareServiceUnitIDCTL2.ClientID %>').val() + "') AND IsDeleted = 0 AND IsAvailable = 1";
        return filterExpression;
    }
    $('#lblExecutorParamedic.lblLink').die('click');
    $('#lblExecutorParamedic.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianExecutorFilterExpression(), function (value) {
            $('#<%=txtExecutorParamedicCode.ClientID %>').val(value);
            onTxtServicePhysicianCodeChanged(value);
        });
    });

    $('#<%=txtExecutorParamedicCode.ClientID %>').live('change', function () {
        onTxtServicePhysicianCodeChanged($(this).val());
    });

    function onTxtServicePhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianExecutorFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnExecutorParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtExecutorParamedicName.ClientID %>').val(result.ParamedicName);

            }
            else {
                $('#<%=hdnExecutorParamedicID.ClientID %>').val('');
                $('#<%=txtExecutorParamedicCode.ClientID %>').val('');
                $('#<%=txtExecutorParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
            </td>
            <td>${ItemName1}</td>
            <td><input type="text" class="txtItemQty number min" min="1" value="1" style="width:80px" /></td>
            <td style="text-align:right" class="txtUnitPrice"><input type="text" class="txtUnitPrice number min" min="0" style="width:80px" value="${ItemTariff}" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberItemQty" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCTL2" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnRadiotheraphyServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnGCSubItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsLabUnit" runat="server" value="0" />
    <input type="hidden" id="hdnOrderHanyaItemPemeriksaanQPCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsDisplayPrice" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnChargesClassIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnTransactionDateQuickPicksCtl" runat="server" value="" />
    <input type="hidden" id="hdnMCUItemTambahanProposeCtl" runat="server" value="0" />
    
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr id="trPhysicanExecuter" runat="server" style="display:none;">
             <td class="tdLabel">
                <label class="lblLink  lblMandatory" id="lblExecutorParamedic">
                    <%=GetLabel("Dokter Pelaksana")%></label>
            </td>
            <td>
             <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 450px" />
                    </colgroup>
                    <tr>
                        <td>
                         
                <input type="hidden" id="hdnExecutorParamedicID" runat="server" value="0"/>
                <asp:TextBox ID="txtExecutorParamedicCode" Width="100%" runat="server" />
                        </td>
                        <td>
                               <asp:TextBox ID="txtExecutorParamedicName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>

            </td>
            
            
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblDiagnose">
                    <%=GetLabel("Diagnosa")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 450px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDiagnoseID" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblItemGroup">
                    <%=GetLabel("Kelompok Pelayanan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 450px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
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
    <table style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfItemName" HeaderText="Pelayanan yang tersedia :" ItemStyle-CssClass="tdItemName1" />
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
                        <div id="pagingPopUp">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                    <tr id="trHeader2">
                        <th style="width: 40px">
                            &nbsp;
                        </th>
                        <th align="left">
                            <%=GetLabel("Pelayanan yang telah dipilih :")%>
                        </th>
                        <th align="right" style="width: 50px;">
                            <%=GetLabel("Qty")%>
                        </th>
                        <th align="right" style="width: 100px;" id="thHarga" runat="server">
                            <%=GetLabel("Harga")%>
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
