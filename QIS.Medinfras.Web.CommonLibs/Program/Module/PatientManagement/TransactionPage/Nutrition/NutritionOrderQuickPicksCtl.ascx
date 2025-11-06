<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionOrderQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NutritionOrderQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_nutritionorderquickpicksctl">
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
    //#endregion

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberGCMealTime = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var gcmealtime = $(this).find('.gcMealTime').val();
            lstSelectedMember.push(key);
            lstSelectedMemberGCMealTime.push(gcmealtime);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberMealTime.ClientID %>').val(lstSelectedMemberGCMealTime.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopUp"), pageCount, function (page) {
            getCheckedMember();
            cbpViewPopup.PerformCallback('changepage|' + page);
        });
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

    function onCboMealTimeChanged() {
        cbpViewPopup.PerformCallback('refresh');
    }

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{MealPlanName}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{MealPlanDtID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{MealTime}/g, $selectedTr.find('.mealTime').html());
            $newTr = $newTr.replace(/\$\{GCMealTime}/g, $selectedTr.find('.gcMealTime').html());
            $newTr = $($newTr);
            $newTr.insertAfter($('#trHeader2'));
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
                <input type="hidden" class="keyField" value='${MealPlanDtID}' />
                <input type="hidden" class="gcMealTime" value='${GCMealTime}' />
            </td>
            <td>${MealPlanName}</td>
            <td>${MealTime}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberMealTime" runat="server" value="" />
    <input type="hidden" id="hdnNutritionOrderHdID" runat="server" value="" />
    <input type="hidden" id="hdnMealDayCtlQP" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnGCItemType" runat="server" />
    <input type="hidden" id="hdnParam" runat="server" />
    <input type="hidden" id="hdnOrderDate" runat="server" />
    <input type="hidden" id="hdnOrderTime" runat="server" />
    <input type="hidden" id="hdnScheduleDate" runat="server" />
    <input type="hidden" id="hdnScheduleTime" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnParamedicID" runat="server" />
    <input type="hidden" id="hdnClassID" runat="server" />
    <input type="hidden" id="hdnNumberOfCalories" runat="server" />
    <input type="hidden" id="hdnNumberOfProtein" runat="server" />
    <input type="hidden" id="hdnGCMealDay" runat="server" />
    <input type="hidden" id="hdnDietType" runat="server" />
    <input type="hidden" id="hdnDiagnoseID" runat="server" />
    <input type="hidden" id="hdnRemarksHd" runat="server" />
    <input type="hidden" value="" id="hdnIsMealPlanByDayQPCtl" runat="server" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblMealTime">
                    <%=GetLabel("Jadwal Makan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboMealTime" ClientInstanceName="cboMealTime" runat="server"
                    Width="200px">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboMealTimeChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td>
                <asp:CheckBox ID="chkIsNotForPatient" runat="server" />
                <%=GetLabel("Bukan untuk Pasien")%>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblMealStatus">
                    <%=GetLabel("Status Makan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboMealStatus" ClientInstanceName="cboMealStatus" runat="server"
                    Width="200px">
                </dxe:ASPxComboBox>
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
                                        <asp:BoundField DataField="MealPlanDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MealPlanName" HeaderText="Panel Menu Makan yang tersedia :"
                                            ItemStyle-CssClass="tdItemName1" />
                                        <asp:BoundField DataField="MealTime" HeaderStyle-CssClass="mealTime hiddenColumn"
                                            ItemStyle-CssClass="mealTime hiddenColumn" />
                                        <asp:BoundField DataField="GCMealTime" HeaderStyle-CssClass="gcMealTime hiddenColumn"
                                            ItemStyle-CssClass="gcMealTime hiddenColumn" />
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
                            <%=GetLabel("Panel Menu Makan yang telah dipilih :")%>
                        </th>
                        <th align="left" style="width: 100px">
                            <%=GetLabel("Jadwal Makan")%>
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
