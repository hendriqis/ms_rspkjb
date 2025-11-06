<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateEDAndBatchNoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.UpdateEDAndBatchNoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ExpiredDatectl">
    $(function () {
        setDatePicker('<%=txtExpiredDateCtl.ClientID %>');
    });

    $('#lblAddDataCtl').die('click');
    $('#lblAddDataCtl').live('click', function () {
        $('#<%=txtBatchNumberCtl.ClientID %>').removeAttr('ReadOnly');
        $('#<%=hdnBatchNumberCtl.ClientID %>').val('');
        $('#<%=txtBatchNumberCtl.ClientID %>').val('');
        $('#<%=txtQuantityCtl.ClientID %>').val('');
        $('#<%=txtExpiredDateCtl.ClientID %>').val('');
        $('#containerEntryDataCtl').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIDCtl.ClientID %>').val(entity.ID);
            $('#<%=hdnBatchNumberCtl.ClientID %>').val(entity.BatchNumber);
            cbpView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnIDCtl.ClientID %>').val(entity.ID);
        $('#<%=txtBatchNumberCtl.ClientID %>').val(entity.BatchNumber);
        $('#<%=txtBatchNumberCtl.ClientID %>').attr('ReadOnly', 'true');
        $('#<%=hdnBatchNumberCtl.ClientID %>').val(entity.BatchNumber);
        $('#<%=txtExpiredDateCtl.ClientID %>').val(entity.cfExpiredDateInDatePickerFormat);
        $('#<%=txtQuantityCtl.ClientID %>').val(entity.Quantity);
        $('#containerEntryDataCtl').show();
    });

    $('#btnCancelCtl').die('click');
    $('#btnCancelCtl').live('click', function () {
        $('#<%=hdnBatchNumberCtl.ClientID %>').val('');
        $('#<%=txtBatchNumberCtl.ClientID %>').val('');
        $('#<%=txtQuantityCtl.ClientID %>').val('');
        $('#<%=txtExpiredDateCtl.ClientID %>').val('');
        $('#containerEntryDataCtl').hide();
    });

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                cbpView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpView.PerformCallback('refresh');
            }
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
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    $('#btnEntrySaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsEntry', 'mpEntryCtl'))
            cbpView.PerformCallback('save');
        return false;
    });

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncboReferrerSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpView.PerformCallback('refresh');
    }

    function getItemFilterExpression() {
        var id = $('#<%=hdnPurchaseReceiveIDCtl.ClientID %>').val();
        var filterExpression = "PurchaseReceiveID = " + id + " AND GCTransactionStatus != 'X121^999' AND GCItemDetailStatus != 'X121^999'";
        return filterExpression;
    }

    $('#lblItemCtl.lblLink').live('click', function () {
        var id = $('#<%=hdnPurchaseReceiveIDCtl.ClientID %>').val();
        var filterExpression = getItemFilterExpression();
        openSearchDialog('purchasereceivedtitem', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChangedText($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = getItemFilterExpression() + " AND ItemID = '" + value + "'";
        Methods.getObject('GetvPurchaseReceiveDtList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnIDCtl.ClientID %>').val(result.ID);
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=hdnPurchaseOrderID.ClientID %>').val(result.PurchaseOrderID);
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=divContainerAddData.ClientID %>').removeAttr('style');
                $('#<%=divContainerAddData.ClientID %>').attr('style', 'text-align: center');
            }
            else {
                $('#<%=hdnIDCtl.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnPurchaseOrderID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=divContainerAddData.ClientID %>').attr('style', 'display:none');
            }
        });
        cbpView.PerformCallback('refresh');
    }

    function onTxtItemCodeChangedText(value) {
        var filterExpression = getItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvPurchaseReceiveDtList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnIDCtl.ClientID %>').val(result.ID);
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=hdnPurchaseOrderID.ClientID %>').val(result.PurchaseOrderID);
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=divContainerAddData.ClientID %>').removeAttr('style');
                $('#<%=divContainerAddData.ClientID %>').attr('style', 'text-align: center');
            }
            else {
                $('#<%=hdnIDCtl.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=hdnPurchaseOrderID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=divContainerAddData.ClientID %>').attr('style', 'display:none');
            }
        });
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" id="hdnPurchaseReceiveIDCtl" value="" runat="server" />
<input type="hidden" id="hdnBatchNumberCtl" value="" runat="server"/>
<div style="height: 440px; overflow-y: auto">
    <div class="pageTitle">
        <%=GetLabel("Ubah Expired Date Dan Batch No")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnIDCtl" runat="server" value="" />
                    <fieldset id="fsEntry" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 15%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Batch Number") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBatchNumberCtl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Expired Date") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExpiredDateCtl" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal">
                                                    <%=GetLabel("Quantity") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtQuantityCtl" Width="70px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntrySaveCtl" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancelCtl" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <table>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("No. BPB")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseReceiveNo" Width="180px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblItemCtl">
                                <%=GetLabel("Item")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnItemID" runat="server" />
                            <input type="hidden" value="" id="hdnPurchaseOrderID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col style="width: 350px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
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
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-right: 2px;" />
                                                    <img class="imgLink imgDelete" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseReceiveID") %>" bindingfield="PurchaseReceiveID" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseReceiveNo") %>" bindingfield="PurchaseReceiveNo" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderID") %>" bindingfield="PurchaseOrderID" />
                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderNo") %>" bindingfield="PurchaseOrderNo" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                    <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("BatchNumber") %>" bindingfield="BatchNumber" />
                                                    <input type="hidden" value="<%#:Eval("cfExpiredDateInDatePickerFormat") %>" bindingfield="cfExpiredDateInDatePickerFormat" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Item")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("cfItemName")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="20%">
                                                <HeaderTemplate>
                                                    <div style="text-align: center; padding-left: 3px">
                                                        <%=GetLabel("Nomor Order")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: center; padding: 3px">
                                                        <div>
                                                            <%#: Eval("PurchaseOrderNo")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Jumlah")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: right; padding: 3px">
                                                        <div>
                                                            <%#: Eval("cfQty")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Bacth No")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("BatchNumber")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="20%">
                                                <HeaderTemplate>
                                                    <div style="text-align: center; padding-left: 3px">
                                                        <%=GetLabel("Expired Date")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: center; padding: 3px">
                                                        <div>
                                                            <%#: Eval("cfExpiredDateInSring")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; display: none; text-align: center" id="divContainerAddData"
                    runat="server">
                    <span class="lblLink" id="lblAddDataCtl">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
