<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesEntryQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChargesEntryQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_attendingnotesctl">

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
    });

    function onBeforeSaveRecord(errMessage) {
        //        if (IsValid(null, 'fsDrugsQuickPicks', 'mpChargeQuickPicks')) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        //        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberUnit = [];
        var lstSelectedMemberIsDiscount = [];
        var lstSelectedMemberDiscount = [];
        var lstSelectedMemberIsVariable = [];
        var lstSelectedMemberUnitPrice = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var unit = $(this).find('.gcItemUnit').val();
            var discount = $(this).find('.txtDiscount').val();
            var tariff = parseFloat($(this).find('.txtUnitPrice').val().replace(/[^0-9-.]/g, '')); // 12345.99;
            var qty = $(this).find('.txtQty').val();

            var isVariable = '0';
            if ($(this).find('.chkIsVariable').is(':checked')) {
                isVariable = 1;
            }
            var isDiscount = '0';
            if ($(this).find('.chkIsDiscount').is(':checked')) {
                isDiscount = 1;
            }

            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberUnit.push(unit);
            lstSelectedMemberIsDiscount.push(isDiscount);
            lstSelectedMemberDiscount.push(discount);
            lstSelectedMemberIsVariable.push(isVariable);
            lstSelectedMemberUnitPrice.push(tariff);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberUnit.ClientID %>').val(lstSelectedMemberUnit.join(','));
        $('#<%=hdnSelectedMemberIsDiscount.ClientID %>').val(lstSelectedMemberIsDiscount.join(','));
        $('#<%=hdnSelectedMemberDiscount.ClientID %>').val(lstSelectedMemberDiscount.join(','));
        $('#<%=hdnSelectedMemberIsVariable.ClientID %>').val(lstSelectedMemberIsVariable.join(','));
        $('#<%=hdnSelectedMemberUnitPrice.ClientID %>').val(lstSelectedMemberUnitPrice.join(','));
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

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{ItemUnit}/g, $selectedTr.find('.itemUnit').html());
            $newTr = $newTr.replace(/\$\{ItemTariff}/g, $selectedTr.find('span[id$="txtItemTariff"]').text());
            $newTr = $newTr.replace(/\$\{ItemDiscount}/g, $selectedTr.find('span[id$="txtItemDiscount"]').text());
            $newTr = $($newTr);

            var isAllowVariable = $selectedTr.find('.IsAllowVariable').html();
            var isAllowDiscount = $selectedTr.find('.IsAllowDiscount').html();

            var discount = parseFloat($selectedTr.find('span[id$="txtItemDiscount"]').text().replace(/[^0-9-.]/g, ''));

            if (isAllowVariable == "True")
                $newTr.find(".chkIsVariable").removeAttr("disabled");
            else
                $newTr.find(".chkIsVariable").attr("disabled", "disabled");

            $newTr.find(".txtUnitPrice").attr("disabled", "disabled");

            if (isAllowDiscount == "True")
                $newTr.find(".chkIsDiscount").removeAttr("disabled");
            else
                $newTr.find(".chkIsDiscount").attr("disabled", "disabled");

            if (discount > 0) {
                $newTr.find(".chkIsDiscount").prop('checked', true);
            }

            $newTr.find(".txtDiscount").attr("disabled", "disabled");

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

    $('#tblSelectedItem .chkIsVariable').die('change');
    $('#tblSelectedItem .chkIsVariable').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked'))
            $tr.find(".txtUnitPrice").removeAttr("disabled");
        else
            $tr.find(".txtUnitPrice").attr("disabled", "disabled");

    });

    $('#tblSelectedItem .chkIsDiscount').die('change');
    $('#tblSelectedItem .chkIsDiscount').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked'))
            $tr.find(".txtDiscount").removeAttr("disabled");
        else
            $tr.find(".txtDiscount").attr("disabled", "disabled");

    });
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="gcItemUnit" value='${ItemUnit}' />
            </td>
            <td>${ItemName1}</td>
            <td style="text-align:right"><input type="text" validationgroup="mpChargeQuickPicks" class="txtQty number min" min="1" value="1" style="width:40px" /></td>
            <td><input type="checkbox" class="chkIsVariable" style="width:20px" /></td>
            <td><input type="checkbox" class="chkIsDiscount" style="width:20px" /></td>
            <td style="text-align:right"><input type="text" validationgroup="mpChargeQuickPicks" class="txtDiscount number min" min="0"  style="width:50px" value="${ItemDiscount}"/></td>
            <td style="text-align:right"><input type="text" validationgroup="mpChargeQuickPicks" class="txtUnitPrice number min" min="0" style="width:80px" value="${ItemTariff}" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="0" />
    <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnParamedicIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnNoteDateCtl" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberUnit" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsDiscount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDiscount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsVariable" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberUnitPrice" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnLongConsultationMinutes" runat="server" value="0" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Service Unit")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboPopupServiceUnit" ClientInstanceName="cboPopupServiceUnit"
                    Width="100%" runat="server" OnCallback="cboPopupServiceUnit_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 45%" />
            <col style="width: 55%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Available service unit item :")%></h4>
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
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" ItemStyle-CssClass="tdItemName1" />
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Tariff">
                                            <ItemTemplate>
                                                <asp:Label ID="txtItemTariff" runat="server" Enabled="False" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Disc(%)">
                                            <ItemTemplate>
                                                <asp:Label ID="txtItemDiscount" runat="server" Enabled="False" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GCItemUnit" HeaderStyle-CssClass="itemUnit hiddenColumn"
                                            ItemStyle-CssClass="itemUnit hiddenColumn" />
                                        <asp:BoundField DataField="IsAllowVariable" HeaderStyle-CssClass="IsAllowVariable hiddenColumn"
                                            ItemStyle-CssClass="IsAllowVariable hiddenColumn" />
                                        <asp:BoundField DataField="IsAllowDiscount" HeaderStyle-CssClass="IsAllowDiscount hiddenColumn"
                                            ItemStyle-CssClass="IsAllowDiscount hiddenColumn" />
                                        <asp:BoundField DataField="IsUnbilledItem" HeaderStyle-CssClass="IsUnbilledItem hiddenColumn"
                                            ItemStyle-CssClass="IsUnbilledItem hiddenColumn" />
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
                    <%=GetLabel("Selected item(s) :")%></h4>
                <fieldset id="fsDrugsQuickPicks">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr id="trHeader2">
                            <th style="width: 20px">
                                &nbsp;
                            </th>
                            <th align="left">
                                <%=GetLabel("Item Name")%>
                            </th>
                            <th align="right" style="width: 40px">
                                <%=GetLabel("Qty")%>
                            </th>
                            <th align="center" style="width: 20px">
                                <%=GetLabel("Var")%>
                            </th>
                            <th align="center" style="width: 20px">
                                <%=GetLabel("Disc")%>
                            </th>
                            <th align="right" style="width: 50px">
                                <%=GetLabel("Disc %")%>
                            </th>
                            <th align="right" style="width: 80px">
                                <%=GetLabel("Tariff")%>
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
