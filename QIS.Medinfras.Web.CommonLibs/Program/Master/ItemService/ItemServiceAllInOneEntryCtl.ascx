<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemServiceAllInOneEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceAllInOneEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ItemServiceAllInOneEntryCtl">
    $(function () {
        $('.txtClassTariff').die('change');
        $('.txtClassTariff').live('change', function () {
            $(this).blur();
            $li = $(this).closest('li');

            var value = $(this).val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);

            var totalClass = parseFloat(value);

            $txtTariffComponent1 = $li.find('.txtTariffComponent1');
            $txtTariffComponent2 = $li.find('.txtTariffComponent2');
            $txtTariffComponent3 = $li.find('.txtTariffComponent3');

            var valueComp2 = $txtTariffComponent2.val();
            valueComp2 = valueComp2.split(token).join(newToken);

            var valueComp3 = $txtTariffComponent3.val();
            valueComp3 = valueComp3.split(token).join(newToken);

            var component2 = parseFloat(valueComp2);
            var component3 = parseFloat(valueComp3);
            var component1 = totalClass - (component2 + component3);

            if (component1 < 0) {
                $txtTariffComponent1.val(totalClass).trigger('changeValue');
                $txtTariffComponent2.val('0').trigger('changeValue');
                $txtTariffComponent3.val('0').trigger('changeValue');
            }
            else {
                $txtTariffComponent1.val(component1).trigger('changeValue');
                //                    $txtTariffComponent2.val(component2).trigger('changeValue');
                //                    $txtTariffComponent3.val(component3).trigger('changeValue');
            }
        });

        $('.txtComponent').die('change');
        $('.txtComponent').live('change', function () {
            $(this).blur();
            $li = $(this).closest('li');
            calculateTariffTotalPerClass($li);
        });

        function calculateTariffTotalPerClass($li) {
            $total = 0;
            $li.find('.txtComponent').each(function () {
                $total += parseFloat($(this).val().split(",").join(""));
            });
            $li.find('.txtClassTariff').val($total).trigger('changeValue');
        }

        $('.txtClassDiscount').die('change');
        $('.txtClassDiscount').live('change', function () {
            $(this).blur();
            $li = $(this).closest('li');

            var value = $(this).val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);

            var totalClass = parseFloat(value);

            $txtTariffComponent1 = $li.find('.txtDiscountComponent1');
            $txtTariffComponent2 = $li.find('.txtDiscountComponent2');
            $txtTariffComponent3 = $li.find('.txtDiscountComponent3');

            var valueComp2 = $txtTariffComponent2.val();
            valueComp2 = valueComp2.split(token).join(newToken);

            var valueComp3 = $txtTariffComponent3.val();
            valueComp3 = valueComp3.split(token).join(newToken);

            var component2 = parseFloat(valueComp2);
            var component3 = parseFloat(valueComp3);
            var component1 = totalClass - (component2 + component3);

            if (component1 < 0) {
                $txtTariffComponent1.val(totalClass).trigger('changeValue');
                $txtTariffComponent2.val('0').trigger('changeValue');
                $txtTariffComponent3.val('0').trigger('changeValue');
            }
            else {
                $txtTariffComponent1.val(component1).trigger('changeValue');
                //                    $txtTariffComponent2.val(component2).trigger('changeValue');
                //                    $txtTariffComponent3.val(component3).trigger('changeValue');
            }
        });

        $('.txtDiscountComponent').die('change');
        $('.txtDiscountComponent').live('change', function () {
            $(this).blur();
            $li = $(this).closest('li');
            calculateDiscountTotalPerClass($li);
        });

        function calculateDiscountTotalPerClass($li) {
            $total = 0;
            $li.find('.txtDiscountComponent').each(function () {
                $total += parseFloat($(this).val().split(",").join(""));
            });
            $li.find('.txtClassDiscount').val($total).trigger('changeValue');
        }
    });

    function oncboDepartmentValueChanged() {
        var department = cboDepartment.GetValue();
        $('#<%=hdnDepartmentID.ClientID %>').val(department);
    }

    //#region HealthcareServiceUnit
    function onGetHealthcareServiceUnitFilterExpression() {
        var filterExpression = "IsUsingRegistration = 1 AND IsDeleted = 0 AND DepartmentID IN ('" + $('#<%=hdnDepartmentID.ClientID %>').val() + "')";
        return filterExpression;
    }

    $('#<%:lblHealthcareServiceUnit.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitperhealthcare', onGetHealthcareServiceUnitFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            ontxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        ontxtServiceUnitCodeChanged($(this).val());
    });

    function ontxtServiceUnitCodeChanged(value) {
        var filterExpression = onGetHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (result != null) {
                if (result.DepartmentID == Constant.Facility.INPATIENT) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                } else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
            }
            else {
                $('#<%=hdnDepartmentID.ClientID %>').val(cboDepartment.GetValue());
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=hdnServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }

            $('#<%=hdnDetailGCItemType.ClientID %>').val('');
            $('#<%=hdnDetailItemID.ClientID %>').val('');
            $('#<%=txtDetailItemName.ClientID %>').val('');
            $('#<%=txtDetailItemCode.ClientID %>').val('');

        });
    }
    //#endregion  

    //#region Item
    function onGetItemServiceDtFilterExpression() {
        var itemID = $('#<%=hdnPackageItemIDCtl.ClientID %>').val();
        var itemType = "'" + Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS + "','" + Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN + "'";

        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
        var suID = $('#<%=hdnServiceUnitID.ClientID %>').val();
        var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();

        var filterExpression = "IsDeleted = 0 AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "' AND GCItemType NOT IN (" + itemType + ")";
        filterExpression += " AND ItemID IN (SELECT ItemID FROM ItemService WHERE IsPackageItem = 0 AND IsPackageAllInOne = 0)";

        if (deptID == Constant.Facility.INPATIENT) {
            filterExpression += " AND ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WITH(NOLOCK) WHERE IsDeleted = 0 AND ItemID = " + itemID + ")";
            filterExpression += " AND ItemID IN (SELECT ItemID FROM vServiceUnitItem WITH(NOLOCK) WHERE IsDeleted = 0 AND DepartmentID = '" + deptID + "')";
        } else {
            if (hsuID != null && hsuID != 0) {
                filterExpression += " AND ItemID NOT IN (SELECT DetailItemID FROM ItemServiceDt WITH(NOLOCK) WHERE IsDeleted = 0 AND ServiceUnitID = " + suID + " AND ItemID = " + itemID + ")";
                filterExpression += " AND ItemID IN (SELECT ItemID FROM vServiceUnitItem WITH(NOLOCK) WHERE IsDeleted = 0 AND HealthcareServiceUnitID = " + hsuID + ")";
            }
        }
        return filterExpression;
    }

    $('#lblItemCtl.lblLink').live('click', function () {
        var oHealthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();
        if (deptID != Constant.Facility.INPATIENT) {
            if (oHealthcareServiceUnitID != null && oHealthcareServiceUnitID != "" && oHealthcareServiceUnitID != "0") {
                openSearchDialog('item', onGetItemServiceDtFilterExpression(), function (value) {
                    $('#<%=txtDetailItemCode.ClientID %>').val(value);
                    onTxtDetailItemCodeChanged(value);
                });
            } else {
                displayMessageBox("INFORMATION", "Harap pilih unit pelayanan terlebih dahulu.");
            }
        } else {
            openSearchDialog('item', onGetItemServiceDtFilterExpression(), function (value) {
                $('#<%=txtDetailItemCode.ClientID %>').val(value);
                onTxtDetailItemCodeChanged(value);
            });
        }
    });

    $('#<%=txtDetailItemCode.ClientID %>').live('change', function () {
        onTxtDetailItemCodeChanged($(this).val());
    });

    function onTxtDetailItemCodeChanged(value) {
        var filterExpression = onGetItemServiceDtFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDetailGCItemType.ClientID %>').val(result.GCItemType);
                $('#<%=hdnDetailItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtDetailItemName.ClientID %>').val(result.ItemName1);

                if (result.GCItemType == Constant.ItemType.PELAYANAN) {
                    $('#<%=tdIsControlAmount.ClientID %>').attr('style', '');
                } else {
                    $('#<%=tdIsControlAmount.ClientID %>').attr('style', 'display:none');
                }
            }
            else {
                $('#<%=hdnDetailGCItemType.ClientID %>').val('');
                $('#<%=hdnDetailItemID.ClientID %>').val('');
                $('#<%=txtDetailItemName.ClientID %>').val('');
                $('#<%=txtDetailItemCode.ClientID %>').val('');
            }
        });
    }
    //#endregion  

    //#region TariffBook
    $('#lblTariffBookNo.lblLink').live('click', function () {
        openSearchDialog('tariffBook', '', function (value) {
            $('#<%=txtDocumentNo.ClientID %>').val(value);
            onTxtDocumentNoChanged(value);
        });
    });

    function onTxtDocumentNoChanged(value) {
        var filterExpression = "DocumentNo = '" + value + "'";
        Methods.getObject('GetvTariffBookHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBookID.ClientID %>').val(result.BookID);
                $('#<%=hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                $('#<%=txtTariffScheme.ClientID %>').val(result.TariffScheme);
                $('#<%=hdnStartDate.ClientID %>').val(result.StartingDateInDatePicker);
                $('#<%=txtStartDate.ClientID %>').val(result.StartingDateInString);

                var itemID = $('#<%=hdnDetailItemID.ClientID %>').val();
                var bookID = $('#<%=hdnBookID.ClientID %>').val();

                if (itemID != "" && bookID != "") {
                    Methods.getSuggestedPrice(bookID, itemID, function (result) {
                        var baseTariff = parseFloat(result.BaseTariff);
                        $('#ulClass li').each(function () {
                            var margin = $(this).find('.hdnMarginPercentage').val();
                            var value = baseTariff * (100 + parseFloat(margin)) / 100;
                            $(this).find('.txtClassTariff').val(value).change();
                        });
                        cbpClassTariff.PerformCallback('refresh');
                    });
                }
            }
            else {
                $('#<%=hdnBookID.ClientID %>').val('');
                $('#<%=hdnGCTariffScheme.ClientID %>').val('');
                $('#<%=txtTariffScheme.ClientID %>').val('');
                $('#<%=hdnStartDate.ClientID %>').val('');
                $('#<%=txtStartDate.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=txtDetailItemQuantity.ClientID %>').live('change', function () {
        var value = $(this).val();
        value = checkMinus(value);
        if (value == 0) {
            value = 1;
        }
        $(this).val(value).trigger('changeValue');
    });

    function onClassTariffEndCallback(s) {
        hideLoadingPanel();
    }

    function onClassDsicountEndCallback(s) {
        hideLoadingPanel();
    }

    function onBeforeSaveRecord(param) {
        var detailItemID = $('#<%=hdnDetailItemID.ClientID %>').val();
        var bookID = $('#<%=hdnBookID.ClientID %>').val();

        if (detailItemID != '' && detailItemID != '0') {
            if (bookID != '' && bookID != '') {
                return true;
            } else {
                showToast('Information', 'Harap pilih buku tariff terlebih dahulu.');
                return false;
            }
        }
        else {
            showToast('Information', 'Harap pilih item terlebih dahulu.');
            return false;
        }
    }
</script>
<style type="text/css">
    #ulClass
    {
        margin: 0;
        padding: 0;
    }
    #ulClass li
    {
        list-style-type: none;
        display: inline-block;
        width: 150px;
        margin-bottom: 10px;
    }
</style>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden;">
    <input type="hidden" id="hdnProcessCtl" value="" runat="server" />
    <input type="hidden" id="hdnItemServiceDtIDCtl" value="0" runat="server" />
    <input type="hidden" id="hdnPackageItemIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnPackageGCItemTypeCtl" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnDetailItemID" runat="server" value="" />
    <input type="hidden" id="hdnDetailGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnBookID" runat="server" value="" />
    <input type="hidden" id="hdnStartDate" runat="server" value="" />
    <input type="hidden" id="hdnGCTariffScheme" runat="server" value="" />
    <input type="hidden" id="hdnListClassID" runat="server" value="" />
    <input type="hidden" id="hdnListClassCode" runat="server" value="" />
    <input type="hidden" id="hdnListClassName" runat="server" value="" />
    <input type="hidden" id="hdnListClassMargin" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryDetail" style="width: 100%">
                    <colgroup>
                        <col style="width: 200px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblNormal" id="lblDepartment" runat="server">
                                <%=GetLabel("Department")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                Width="350px">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboDepartmentValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblLink" id="lblHealthcareServiceUnit" runat="server">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="500px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblMandatory lblLink" id="lblItemCtl">
                                <%=GetLabel("Item")%></label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDetailItemCode" CssClass="required" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDetailItemName" ReadOnly="true" Width="500px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblNormal">
                                <%=GetLabel("Quantity")%></label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDetailItemQuantity" CssClass="number" runat="server" Width="150px" />
                                    </td>
                                    <td id="tdIsControlAmount" runat="server" style="display: none">
                                        <asp:CheckBox ID="chkIsControlAmount" runat="server" /><%=GetLabel(" Kontrol Batasan Nilai Obat Alkes")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblMandatory lblLink" id="lblTariffBookNo">
                                <%=GetLabel("No. Buku Tariff")%></label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDocumentNo" CssClass="required" Width="150px" runat="server"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Skema Tariff")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTariffScheme" Width="150px" runat="server" ReadOnly="true" />
                                    </td>
                                    <td class="tdLabel" style="padding: 5px;">
                                        <label class="lblNormal" id="lblStartDate">
                                            <%=GetLabel("Mulai Berlaku")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h4>
                                <%=GetLabel("Tariff")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Tagihan")%></label>
                            <div style="margin-top: 25px;" id="divComponentLabel" runat="server">
                                <div style="height: 23px">
                                    <%=GetTariffComponent1Text()%></div>
                                <div style="height: 23px">
                                    <%=GetTariffComponent2Text()%></div>
                                <div style="height: 23px">
                                    <%=GetTariffComponent3Text()%></div>
                            </div>
                        </td>
                        <td valign="top">
                            <dxcp:ASPxCallbackPanel ID="cbpClassTariff" runat="server" Width="100%" ClientInstanceName="cbpClassTariff"
                                ShowLoadingPanel="false" OnCallback="cbpClassTariff_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onClassTariffEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainer">
                                            <asp:Repeater ID="rptClassTariff" runat="server">
                                                <HeaderTemplate>
                                                    <ul id="ulClass">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="lblComponent">
                                                            <%#: Eval("ClassName") %></div>
                                                        <span>
                                                            <asp:TextBox ID="txtClassTariff" runat="server" Width="90%" CssClass="txtCurrency txtClassTariff"
                                                                Text='<%#: Eval("Tariff", "{0:N2}")%>' /></span>
                                                        <div style="margin-top: 5px;" id="divComponent" runat="server">
                                                            <asp:TextBox ID="txtTariffComponent1" runat="server" Width="90%" CssClass="txtCurrency txtComponent txtTariffComponent1"
                                                                Text='<%#: Eval("TariffComponent1", "{0:N2}")%>' />
                                                            <asp:TextBox ID="txtTariffComponent2" runat="server" Width="90%" CssClass="txtCurrency txtComponent txtTariffComponent2"
                                                                Text='<%#: Eval("TariffComponent2", "{0:N2}")%>' />
                                                            <asp:TextBox ID="txtTariffComponent3" runat="server" Width="90%" CssClass="txtCurrency txtComponent txtTariffComponent3"
                                                                Text='<%#: Eval("TariffComponent3", "{0:N2}")%>' />
                                                        </div>
                                                        <input type="hidden" id="hdnMarginPercentage" class="hdnMarginPercentage" value='<%#: Eval("MarginPercentage1")%>'
                                                            runat="server" />
                                                        <input type="hidden" id="hdnClassID" class="hdnClassID" value='<%#: Eval("ClassID")%>'
                                                            runat="server" />
                                                        <input type="hidden" class="hdnClassName" value='<%#: Eval("ClassName")%>' />
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </ul>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h4>
                                <%=GetLabel("Diskon")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Tagihan")%></label>
                            <div style="margin-top: 25px;" id="div1" runat="server">
                                <div style="height: 23px">
                                    <%=GetTariffComponent1Text()%></div>
                                <div style="height: 23px">
                                    <%=GetTariffComponent2Text()%></div>
                                <div style="height: 23px">
                                    <%=GetTariffComponent3Text()%></div>
                            </div>
                        </td>
                        <td valign="top">
                            <dxcp:ASPxCallbackPanel ID="cbpClassDiscount" runat="server" Width="100%" ClientInstanceName="cbpClassDiscount"
                                ShowLoadingPanel="false" OnCallback="cbpClassDiscount_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onClassDsicountEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainer">
                                            <asp:Repeater ID="rptClassDiscount" runat="server">
                                                <HeaderTemplate>
                                                    <ul id="ulClass">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="lblComponent">
                                                            <%#: Eval("ClassName") %></div>
                                                        <span>
                                                            <asp:TextBox ID="txtClassDiscount" runat="server" Width="90%" CssClass="txtCurrency txtClassDiscount"
                                                                Text='<%#: Eval("DiscountAmount", "{0:N2}")%>' /></span>
                                                        <div style="margin-top: 5px;" id="divComponent" runat="server">
                                                            <asp:TextBox ID="txtDiscountComponent1" runat="server" Width="90%" CssClass="txtCurrency txtDiscountComponent txtDiscountComponent1"
                                                                Text='<%#: Eval("DiscountComp1", "{0:N2}")%>' />
                                                            <asp:TextBox ID="txtDiscountComponent2" runat="server" Width="90%" CssClass="txtCurrency txtDiscountComponent txtDiscountComponent2"
                                                                Text='<%#: Eval("DiscountComp2", "{0:N2}")%>' />
                                                            <asp:TextBox ID="txtDiscountComponent3" runat="server" Width="90%" CssClass="txtCurrency txtDiscountComponent txtDiscountComponent3"
                                                                Text='<%#: Eval("DiscountComp3", "{0:N2}")%>' />
                                                        </div>
                                                        <input type="hidden" id="hdnClassIDDiscount" class="hdnClassIDDiscount" value='<%#: Eval("ClassID")%>'
                                                            runat="server" />
                                                        <input type="hidden" class="hdnClassNameDiscount" value='<%#: Eval("ClassName")%>' />
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </ul>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
