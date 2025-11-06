<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstimatedChargesQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EstimatedChargesQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_servicequickpicksctl">
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

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsServiceQuickPicks', 'mpServiceQuickPicks')) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            $('#<%=hdnSelectedMemberQty.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPrice.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPriceComp1.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPriceComp2.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPriceComp3.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPatientAmount.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPayerAmount.ClientID %>').val('');
            $('#<%=hdnSelectedMemberLineAmount.ClientID %>').val('');

            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
                return true;
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberPrice = [];
        var lstSelectedMemberPriceComp1 = [];
        var lstSelectedMemberPriceComp2 = [];
        var lstSelectedMemberPriceComp3 = [];
        var lstSelectedMemberPatientAmount = [];
        var lstSelectedMemberPayerAmount = [];
        var lstSelectedMemberLineAmount = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var qty = $(this).find('.txtQty').val();

            var token = ",";
            var newToken = "";

            var price = parseFloat($(this).find('.hdnPrice').val().split(token).join(newToken));
            var priceComp1 = parseFloat($(this).find('.hdnPriceComp1').val().split(token).join(newToken));
            var priceComp2 = parseFloat($(this).find('.hdnPriceComp2').val().split(token).join(newToken));
            var priceComp3 = parseFloat($(this).find('.hdnPriceComp3').val().split(token).join(newToken));
            var patientAmount = parseFloat($(this).find('.txtPatient').val().split(token).join(newToken));
            var payerAmount = parseFloat($(this).find('.txtPayer').val().split(token).join(newToken));
            var lineAmount = parseFloat($(this).find('.txtTotal').val().split(token).join(newToken));

            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberPrice.push(price);
            lstSelectedMemberPriceComp1.push(priceComp1);
            lstSelectedMemberPriceComp2.push(priceComp2);
            lstSelectedMemberPriceComp3.push(priceComp3);
            lstSelectedMemberPatientAmount.push(patientAmount);
            lstSelectedMemberPayerAmount.push(payerAmount);
            lstSelectedMemberLineAmount.push(lineAmount);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberPrice.ClientID %>').val(lstSelectedMemberPrice.join(','));
        $('#<%=hdnSelectedMemberPriceComp1.ClientID %>').val(lstSelectedMemberPriceComp1.join(','));
        $('#<%=hdnSelectedMemberPriceComp2.ClientID %>').val(lstSelectedMemberPriceComp2.join(','));
        $('#<%=hdnSelectedMemberPriceComp3.ClientID %>').val(lstSelectedMemberPriceComp3.join(','));
        $('#<%=hdnSelectedMemberPatientAmount.ClientID %>').val(lstSelectedMemberPatientAmount.join(','));
        $('#<%=hdnSelectedMemberPayerAmount.ClientID %>').val(lstSelectedMemberPayerAmount.join(','));
        $('#<%=hdnSelectedMemberLineAmount.ClientID %>').val(lstSelectedMemberLineAmount.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopUp"), pageCount, function (page) {
            getCheckedMember();
            cbpView.PerformCallback('changepage|' + page);
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
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopUp"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    function onCboItemTypeValueChanged() {
        $('#<%=hdnGCItemType.ClientID %>').val(cboItemType.GetValue());
        $('#<%=hdnItemGroupID.ClientID %>').val('');
        $('#<%=txtItemGroupCode.ClientID %>').val('');
        $('#<%=txtItemGroupName.ClientID %>').val('');
        getCheckedMember();
        cbpView.PerformCallback('refresh');
    }

    function getTrxDate() {
        var date = Methods.getDatePickerDate($('#<%=hdnTransactionDateCtl.ClientID %>').val());
        var dateInYMD = Methods.dateToYMD(date);
        return dateInYMD;
    }

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

    $('#btnCalculate').live('click', function () {
        calculate();
    });

    //#region Calculate Item Tariff Estimation
    function calculate() {
        getCheckedMember();
        var selectedItem = $('#<%=hdnSelectedMember.ClientID %>').val();
        if (selectedItem == '')
            showToast('Warning', 'Please Select Item First');
        else {
            showLoadingPanel();

            var businessPartnerID = $('#<%=hdnBusinessPartnerIDCtl.ClientID %>').val();
            var bpp = cboCustomerType.GetValue();
            var bppList = "";
            var filterExpression = "GCCustomerType = '" + bpp + "' AND IsDeleted = 0";
            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                if (result != null) {
                    bppList += result.BusinessPartnerID + ",";
                }
            });
            bppList = bppList.substring(0, bppList.length - 1);
            if (businessPartnerID == '') {
                businessPartnerID = bppList;
            }

            var coverageTypeID = $('#<%=hdnCoverageTypeIDCtl.ClientID %>').val();
            if (coverageTypeID == '')
                coverageTypeID = '0';

            Methods.getTariffEstimation(cboClass.GetValue(), businessPartnerID, coverageTypeID, selectedItem, getTrxDate(), 'INPATIENT', 2, function (result) {
                if (result != null) {
                    totalPatient = 0;
                    totalPayer = 0;
                    grandTotal = 0;
                    for (var i = 0; i < result.length; i++) {
                        $row = null;
                        $('#tblSelectedItem .trSelectedItem').each(function () {

                            if ($(this).find('.keyField').val() == result[i].ItemID) {
                                $row = $(this);
                                $row.find('.hdnCoverageAmount').val(result[i].CoverageAmount);
                                $row.find('.hdnDiscountAmount').val(result[i].DiscountAmount);
                                $row.find('.hdnIsCoverageInPercentage').val(result[i].IsCoverageInPercentage);
                                $row.find('.hdnIsDiscountInPercentage').val(result[i].IsDiscountInPercentage);
                                console.log(result[i].IsPackageItem);
                                if (result[i].IsPackageItem == true) {
                                    $row.find('.hdnPrice').val(result[i].ItemPackagePrice);
                                    $row.find('.hdnPriceComp1').val(result[i].PriceComp1);
                                    $row.find('.hdnPriceComp2').val(result[i].PriceComp2);
                                    $row.find('.hdnPriceComp3').val(result[i].PriceComp3);
                                } else {
                                    $row.find('.hdnPrice').val(result[i].Price);
                                    $row.find('.hdnPriceComp1').val(result[i].PriceComp1);
                                    $row.find('.hdnPriceComp2').val(result[i].PriceComp2);
                                    $row.find('.hdnPriceComp3').val(result[i].PriceComp3);
                                }

                                calculateTariffEstimation($row);
                            }
                        });
                    }
                }
                hideLoadingPanel();
            });
        }    
    }

    function calculateTariffEstimation($row) {
        var totalPrice = $row.find('.hdnPrice').val() * $row.find('.txtQty').val();
        var payer, patient, discount = 0;
        if ($row.find('.hdnIsDiscountInPercentage').val())
            discount = ($row.find('.hdnDiscountAmount').val() / 100) * totalPrice;
        else
            discount = $row.find('.hdnDiscountAmount').val();

        totalPrice -= discount;

        var coverageInPercent = $row.find('.hdnIsCoverageInPercentage').val();
        if (coverageInPercent == "true") {
            payer = ($row.find('.hdnCoverageAmount').val() / 100) * totalPrice;
        }
        else {
            payer = $row.find('.hdnCoverageAmount').val();
        }

        patient = totalPrice - payer;
        totalPayer += payer;
        totalPatient += patient;
        grandTotal += totalPrice;

        $row.find('.txtPayer').val(payer).trigger('changeValue');
        $row.find('.txtPatient').val(patient).trigger('changeValue');
        $row.find('.txtTotal').val(totalPrice).trigger('changeValue');
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $($newTr);
            $newTr.insertBefore($('#trFooter'));
            calculate();
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
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="hdnCoverageAmount" value="0" />
                <input type="hidden" class="hdnIsCoverageInPercentage" value="0" />
                <input type="hidden" class="hdnDiscountAmount" value="0" />
                <input type="hidden" class="hdnIsDiscountInPercentage" value="0" />
                <input type="hidden" class="hdnPrice" value="0" />
                <input type="hidden" class="hdnPriceComp1" value="0" />
                <input type="hidden" class="hdnPriceComp2" value="0" />
                <input type="hidden" class="hdnPriceComp3" value="0" />
            </td>
            <td>${ItemName1}</td>
            <td><input type="text" validationgroup="mpServiceQuickPicks" class="txtQty number min" min="0.1" value="1" style="width:60px" /></td>
            <td><input type="text" class="txtPayer txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
            <td><input type="text" class="txtPatient txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
            <td><input type="text" class="txtTotal txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPrice" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPriceComp1" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPriceComp2" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPriceComp3" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPatientAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPayerAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberLineAmount" runat="server" value="" />
    <input type="hidden" id="hdnTransactionIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnTransactionDateCtl" runat="server" value="" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnCoverageTypeIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnTransactionNoCtl" runat="server" value="" />
    <input type="hidden" id="hdnCustomerTypeCtl" runat="server" value="" />
    <fieldset id="fsEntryPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 400px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tipe Item")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" Width="100%"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeValueChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblItemGroup">
                        <%=GetLabel("Kelompok Item")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                            <col style="width: 600px" />
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
                        <%=GetLabel("Kelas Tagihan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceChargeClassIDCtl" ClientInstanceName="cboServiceChargeClassIDCtl"
                        Width="100%" runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr style='display:none'>
                <td>
                </td>
                <td>
                    <input type="button" id="btnCalculate" value="Calculate" class="btnCalculate w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                </td>
            </tr>
        </table>
    </fieldset>
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
                                            <asp:BoundField DataField="cfItemName" HeaderText="Pelayanan yang tersedia :" ItemStyle-CssClass="tdItemName1"
                                                HeaderStyle-HorizontalAlign="Left" />
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
                    <fieldset id="fsServiceQuickPicks">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                            <tr id="trHeader2">
                                <th rowspan="2" style="width: 40px">
                                    &nbsp;
                                </th>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("Item")%>
                                </th>
                                <th rowspan="2" align="right" style="width: 60px">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                                <th colspan="3" align="center">
                                    <%=GetLabel("Harga")%>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 120px" align="center">
                                    <%=GetLabel("Instansi")%>
                                </th>
                                <th style="width: 120px" align="center">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 120px" align="center">
                                    <%=GetLabel("Total")%>
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
