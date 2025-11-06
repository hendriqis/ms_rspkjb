<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationItemQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.LocationItemQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
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
            cbpPopup.PerformCallback('refresh');
        }
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();

        //#region Item Group
        $('#lblItemGroupDrugLogistic.lblLink').click(function () {
            openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').val(value);
                onTxtItemGroupDrugLogisticCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').change(function () {
            onTxtItemGroupDrugLogisticCodeChanged($(this).val());
        });

        function onTxtItemGroupDrugLogisticCodeChanged(value) {
            var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
            Methods.getObject('GetvItemGroupMasterList', filterExpression, function (result) {
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
                cbpPopup.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Bin Location
        function onGetBinLocationFilterExpressionCtl() {
            var filterExpression = "LocationID = '" + $('#<%=hdnLocationIDCtl.ClientID %>').val() + "'";
            return filterExpression;
        }

        $('#lblBinLocationCtl.lblLink').click(function () {
            openSearchDialog('binlocation', onGetBinLocationFilterExpressionCtl(), function (value) {
                $('#<%=txtBinLocationCodeCtl.ClientID %>').val(value);
                ontxtBinLocationCodeCtlChanged(value);
            });
        });

        $('#<%=txtBinLocationCodeCtl.ClientID %>').change(function () {
            ontxtBinLocationCodeCtlChanged($(this).val());
        });

        function ontxtBinLocationCodeCtlChanged(value) {
            var filterExpression = onGetBinLocationFilterExpressionCtl() + " AND BinLocationCode = '" + value + "'";
            Methods.getObject('GetBinLocationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBinLocationIDCtl.ClientID %>').val(result.BinLocationID);
                    $('#<%=txtBinLocationNameCtl.ClientID %>').val(result.BinLocationName);
                }
                else {
                    $('#<%=hdnBinLocationIDCtl.ClientID %>').val('');
                    $('#<%=txtBinLocationCodeCtl.ClientID %>').val('');
                    $('#<%=txtBinLocationNameCtl.ClientID %>').val('');
                }
            });
        }
        //#endregion
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
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
        var lstSelectedMemberMin = [];
        var lstSelectedMemberMax = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var min = $(this).find('.QuantityMin').val();
            var max = $(this).find('.QuantityMax').val();
            lstSelectedMember.push(key);
            lstSelectedMemberMin.push(min);
            lstSelectedMemberMax.push(max);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnQuantityMin.ClientID %>').val(lstSelectedMemberMin.join(','));
        $('#<%=hdnQuantityMax.ClientID %>').val(lstSelectedMemberMax.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });

    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var minValue = $('#<%=hdnDefaultValueMin.ClientID %>').val();
            var maxValue = $('#<%=hdnDefaultValueMax.ClientID %>').val();            

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{DefaultValueMin}/g, minValue);
            $newTr = $newTr.replace(/\$\{DefaultValueMax}/g, maxValue);

            $newTr = $($newTr);
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
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
            </td>
            <td>${ItemName1}</td>
            <td><input type="text" validationgroup="mpDrugsQuickPicks" class="QuantityMin number min" min="0" value='${DefaultValueMin}' style="width:30px" /></td>
            <td><input type="text" validationgroup="mpDrugsQuickPicks" class="QuantityMax number min" min="0" value='${DefaultValueMax}' style="width:30px" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnQuantityMin" runat="server" />
    <input type="hidden" id="hdnQuantityMax" runat="server" />
    <input type="hidden" id="hdnLocationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnGCLocationGroupCtl" runat="server" value="" />
    <input type="hidden" id="hdnDefaultValueMin" runat="server" value="" />
    <input type="hidden" id="hdnDefaultValueMax" runat="server" value="" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 700px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblItemGroupDrugLogistic">
                    <%=GetLabel("Kelompok Barang")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnItemGroupDrugLogisticID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
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
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Daftar Barang yang belum terdaftar di lokasi ini")%></h4>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
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
                                        <asp:BoundField DataField="ItemName1" HeaderText="Barang" ItemStyle-CssClass="tdItemName1" />
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
                <h4>
                    <%=GetLabel("Dipilih")%></h4>
                <table>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Transaksi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTransactionTypeCtl" ClientInstanceName="cboTransactionTypeCtl"
                                Width="130px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblBinLocationCtl">
                                <%=GetLabel("Rak")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnBinLocationIDCtl" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBinLocationCodeCtl" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBinLocationNameCtl" ReadOnly="true" Width="99%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <fieldset id="fsDrugsQuickPicks">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr id="trHeader2">
                            <th style="width: 40px">
                                &nbsp;
                            </th>
                            <th align="center">
                                <%=GetLabel("Barang")%>
                            </th>
                            <th align="center" style="width: 30px">
                                <%=GetLabel("Min")%>
                            </th>
                            <th align="center" style="width: 30px">
                                <%=GetLabel("Max")%>
                            </th>
                        </tr>
                        <tr id="trFooter">
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
