<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcedurePanelItemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcedurePanelItemEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtFormulaPercentage.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');

        var procID = $('#<%=hdnProcedureID.ClientID %>').val();
        var filterExpression = "IsDeleted = 0 AND IsControlItem = 1 AND ProcedurePanelID = " + procID;
        Methods.getObject('GetProcedurePanelDtList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=chkIsControlItem.ClientID %>').prop('checked', false);
            }
            else {
                $('#<%=chkIsControlItem.ClientID %>').prop('checked', true);
            }
        });
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            if ($('#<%:chkIsControlItem.ClientID %>').is(':checked')) {
                var procID = $('#<%=hdnProcedureID.ClientID %>').val();
                var filterExpression = "IsDeleted = 0 AND IsControlItem = 1 AND ProcedurePanelID = " + procID;
                Methods.getObject('GetProcedurePanelDtList', filterExpression, function (result) {
                    if (result != null) {
                        var confirm = 'Sudah ada item yang ditunjuk sebagai "Control Item", ubah ke item ini?';
                        showToastConfirmation(confirm, function (answer) {
                            if (answer) {
                                $('#<%=hdnIsUpdateControlItem.ClientID %>').val("1");
                                cbpEntryPopupView.PerformCallback('save');
                            } else {
                                $('#<%=chkIsControlItem.ClientID %>').prop('checked', false);
                            }
                        });
                    }
                    else {
                        cbpEntryPopupView.PerformCallback('save');
                    }
                });
            } else {
                cbpEntryPopupView.PerformCallback('save');
            }
        } else {
            return false;
        }
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName = $row.find('.hdnItemName').val();
        var formulaPercentage = $row.find('.hdnFormulaPercentage').val();
        var displayOrder = $row.find('.hdnDisplayOrder').val();
        var isControlItem = $row.find('.hdnIsControlItem').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=txtFormulaPercentage.ClientID %>').val(formulaPercentage);
        $('#<%=txtDisplayOrder.ClientID %>').val(displayOrder);
        if (isControlItem == "True") {
            $('#<%=chkIsControlItem.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsControlItem.ClientID %>').prop('checked', false);
        }
        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblEntryPopupAddData').click();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    function onRefreshControl(filterExpression) {
        cbpEntryPopupView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

    //#region Item
    function onGetSupplierItemCodeFilterExpression() {
        var procedurePanelID = $('#<%=hdnProcedureID.ClientID %>').val();
        var filterExpression = "GCItemStatus = 'X181^001' AND GCItemType IN ('X001^006') AND IsDeleted = 0 AND ItemID NOT IN (SELECT ItemID FROM ProcedurePanelDt WHERE ProcedurePanelID = " + procedurePanelID + " AND IsDeleted = 0)";
        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        openSearchDialog('item', onGetSupplierItemCodeFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = onGetSupplierItemCodeFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }            
        });
    }
    //#endregion    
</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnGCPurchaseUnit" value="" runat="server" />
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnQuickText" value="" runat="server" />
    <input type="hidden" id="hdnProcedureID" value="" runat="server" />
    <input type="hidden" id="hdnIsUpdateControlItem" value="0" runat="server" />
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
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Formula Percentage [%]")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFormulaPercentage" CssClass="txtCurrency required" runat="server"
                                        Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Display Order")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" Width="100px"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="padding-left: 5px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Is Control Item")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsControlItem" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnProcedurePanelID" value="<%#: Eval("ProcedurePanelID")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnItemName" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="hdnFormulaPercentage" value="<%#: Eval("FormulaPercentage")%>" />
                                                <input type="hidden" class="hdnDisplayOrder" value="<%#: Eval("DisplayOrder")%>" />
                                                <input type="hidden" class="hdnIsControlItem" value="<%#: Eval("IsControlItem")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemCode" HeaderStyle-Width="60px" HeaderText="Item Code" />
                                        <asp:BoundField DataField="ItemName1" HeaderStyle-Width="200px" HeaderText="Item Name" />
                                        <asp:BoundField DataField="FormulaPercentage" HeaderStyle-Width="80px" HeaderText="Formula Percentage [%]"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DisplayOrder" HeaderStyle-Width="80px" HeaderText="Display Order"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IsControlItem" HeaderStyle-Width="80px" HeaderText="Is Control Item"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:N}" />
                                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfCreatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Diubah Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("LastUpdatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfLastUpdatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
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
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
