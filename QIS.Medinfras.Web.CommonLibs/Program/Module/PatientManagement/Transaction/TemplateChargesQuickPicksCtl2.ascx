<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateChargesQuickPicksCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplateChargesQuickPicksCtl2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_templatechargesctl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    function onRefreshGrid() {
        cbpViewCtl.PerformCallback('refresh');
    }

    $('#txtFilterItem').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpViewCtl.PerformCallback('refresh');
        }
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
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

    //#region Template
    $('#lblTemplate.lblLink').live('click', function () {
        var filter = "IsDeleted = 0 AND HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "'";
        openSearchDialog('chargestemplatehd', filter, function (value) {
            $('#<%=txtTemplateCode.ClientID %>').val(value);
            onTxtTemplateCodeChanged(value);
        });
    });

    $('#<%=txtTemplateCode.ClientID %>').live('change', function () {
        onTxtTemplateCodeChanged($(this).val());
    });

    function onTxtTemplateCodeChanged(value) {
        var filterExpression = "ChargesTemplateCode = '" + value + "' AND IsDeleted = 0 AND HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "'";
        Methods.getObject('GetvChargesTemplateHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnChargesTemplateID.ClientID %>').val(result.ChargesTemplateID);
                $('#<%=txtTemplateName.ClientID %>').val(result.ChargesTemplateName);
            }
            else {
                $('#<%=hdnChargesTemplateID.ClientID %>').val('');
                $('#<%=txtTemplateCode.ClientID %>').val('');
                $('#<%=txtTemplateName.ClientID %>').val('');
            }
        });
        cbpViewCtl.PerformCallback('refresh');
    }
    //#endregion



    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpViewCtl.PerformCallback('changepage|' + page);
        });
    });
    //#endregion

    function oncbpViewCtlEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpViewCtl.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{DetailItemUnit}/g, $selectedTr.find('.tdItemUnit').html());
            $newTr = $newTr.replace(/\$\{Qty}/g, $selectedTr.find('.tdQuantity').html());
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
            <td><input type="text" validationgroup="mpDrugsQuickPicks" class="txtQty number min" min="0" value="${Qty}" style="width:60px" /></td>
            <td>${DetailItemUnit}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnChargesTemplateID" runat="server" value="0" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="0" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnIsAccompany" runat="server" value="" />
    <input type="hidden" id="hdnIsDrugChargesJustDistributionQP" runat="server" value="0" />
    <input type="hidden" id="hdnTransactionIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnListItemBefore" runat="server" value="0" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblTemplate">
                    <%=GetLabel("Template")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal lblLink" id="Label1">
                    <%=GetLabel("Tipe Item")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" Width="200px"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
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
                    <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
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
                                            <asp:BoundField DataField="Quantity" HeaderText="Jumlah" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="tdQuantity" />
                                            <asp:BoundField DataField="DetailItemUnit" HeaderText="Satuan" ItemStyle-CssClass="tdItemUnit" />
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
                                <th align="center" style="width: 60px">
                                    <%=GetLabel("Satuan")%>
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
