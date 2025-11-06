<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemLaboratoryFractionEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Laboratory.Program.ItemLaboratoryFractionEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_itemlaboratoryfractionentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnFractionID.ClientID %>').val('');
        $('#<%=txtFractionCode.ClientID %>').val('');
        $('#<%=txtFractionName.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');
        $('#<%=txtItemLaboratoryCode.ClientID %>').val('');
        $('#<%=txtItemLaboratoryName.ClientID %>').val('');
        $('#<%=chkIsTestItem.ClientID %>').attr("checked", false);
        $('#trTestItem').css('display', 'none');
        $('#trFractionItem').show();
        $('#containerPopupEntryData').show();
    });

    $('#<%=chkIsTestItem.ClientID %>').live('change', function () {
        if ($(this).is(":checked")) {
            $('#<%=hdnFractionID.ClientID %>').val('');
            $('#<%=txtFractionCode.ClientID %>').val('');
            $('#<%=txtFractionName.ClientID %>').val('');
            $('#trTestItem').show();
            $('#trFractionItem').css('display', 'none');
        }
        else {
            $('#<%=hdnItemLaboratoryID.ClientID %>').val('');
            $('#<%=txtItemLaboratoryCode.ClientID %>').val('');
            $('#<%=txtItemLaboratoryName.ClientID %>').val('');
            $('#trTestItem').css('display', 'none');
            $('#trFractionItem').show();
        }
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {

        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemLaboratoryID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemLaboratoryCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemLaboratoryName.ClientID %>').val(entity.ItemName);
        $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
        $('#<%=hdnFractionID.ClientID %>').val(entity.FractionID);
        $('#<%=txtFractionCode.ClientID %>').val(entity.FractionCode);
        $('#<%=txtFractionName.ClientID %>').val(entity.FractionName);
        $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
        $('#<%=hdnIsTestItem.ClientID %>').val(entity.IsTestItem);

        if ($('#<%=hdnIsTestItem.ClientID %>').val() == 'True') {
            $('#<%=chkIsTestItem.ClientID %>').attr("checked", true);
            $('#trTestItem').show();
            $('#trFractionItem').css('display', 'none');
        }
        else {
            $('#<%=chkIsTestItem.ClientID %>').attr("checked", false);
            $('#trTestItem').css('display', 'none');
            $('#trFractionItem').show();        
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
                $('#containerPopupEntryData').hide();        
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[2]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    $('#lblItemLaboratory.lblLink').die('click');
    $('#lblItemLaboratory.lblLink').live('click', function () {
        var filterExpression = "GCItemType = '" + Constant.ItemGroupMaster.LABORATORY + "' AND IsDeleted = 0 AND ItemID NOT IN (SELECT ISNULL(DetailItemID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + ") AND ItemID NOT IN (SELECT ItemID FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND FractionID IN (SELECT FractionID FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemLaboratoryFraction.ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + "))";
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemLaboratoryCode.ClientID %>').val(value);
            onTxtItemLaboratoryCodeChanged(value);
        });
    });

    $('#<%=txtItemLaboratoryCode.ClientID %>').die('change');
    $('#<%=txtItemLaboratoryCode.ClientID %>').live('change', function () {
        onTxtItemLaboratoryCodeChanged($(this).val());
    });

    function onTxtItemLaboratoryCodeChanged(value) {
        var filterExpression = "GCItemType = '" + Constant.ItemGroupMaster.LABORATORY + "' AND IsDeleted = 0 AND ItemID NOT IN (SELECT ISNULL(DetailItemID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + ") AND ItemID NOT IN (SELECT ItemID FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND FractionID IN (SELECT FractionID FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemLaboratoryFraction.ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + "))";
        filterExpression += "AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemLaboratoryID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemLaboratoryName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemLaboratoryID.ClientID %>').val('');
                $('#<%=txtItemLaboratoryCode.ClientID %>').val('');
                $('#<%=txtItemLaboratoryName.ClientID %>').val('');
            }
        });
    }

    $('#lblFraction.lblLink').die('click');
    $('#lblFraction.lblLink').live('click', function () {
        var filterExpression = "IsDeleted = 0 AND FractionID NOT IN (SELECT ISNULL(FractionID,0) FROM ItemLaboratoryFraction WHERE ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + " AND IsDeleted = 0)";
        filterExpression += " AND FractionID NOT IN (SELECT ISNULL(FractionID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0";
        filterExpression += " AND ItemID IN (SELECT ISNULL(DetailItemID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + "))";
        openSearchDialog('fraction', filterExpression, function (value) {
            $('#<%=txtFractionCode.ClientID %>').val(value);
            onTxtFractionCodeChanged(value);
        });
    });

    $('#<%=txtFractionCode.ClientID %>').die('change');
    $('#<%=txtFractionCode.ClientID %>').live('change', function () {
        onTxtFractionCodeChanged($(this).val());
    });

    function onTxtFractionCodeChanged(value) {
        var filterExpression = "IsDeleted = 0 AND FractionID NOT IN (SELECT ISNULL(FractionID,0) FROM ItemLaboratoryFraction WHERE ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + " AND IsDeleted = 0)";
        filterExpression += " AND FractionID NOT IN (SELECT ISNULL(FractionID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0";
        filterExpression += " AND ItemID IN (SELECT ISNULL(DetailItemID,0) FROM ItemLaboratoryFraction WHERE IsDeleted = 0 AND ItemID = " + $('#<%=hdnItemLabID.ClientID %>').val() + "))";
        filterExpression += "AND FractionCode = '" + value + "'";
        Methods.getObject('GetFractionList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnFractionID.ClientID %>').val(result.FractionID);
                $('#<%=txtFractionName.ClientID %>').val(result.FractionName1);
            }
            else {
                $('#<%=hdnFractionID.ClientID %>').val('');
                $('#<%=txtFractionCode.ClientID %>').val('');
                $('#<%=txtFractionName.ClientID %>').val('');
            }
        });
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 
</script>
<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnIsTestItem" value="" runat="server"/>
    <input type="hidden" id="hdnItemLabID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:95%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:100px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pemeriksaan")%></label></td>
                        <td><asp:TextBox ID="txtItemLabCode" ReadOnly="true" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtItemLabName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col style="width:400px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <input type="checkbox" id="chkIsTestItem" runat="server" />&nbsp;&nbsp;<%=GetLabel("Is Test Item") %>
                                </td>
                            </tr>
                            <tr id="trTestItem" style="display:none">
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItemLaboratory"><%=GetLabel("Item Laboratory") %></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnItemLaboratoryID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px" />
                                            <col style="width:3px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemLaboratoryCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemLaboratoryName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trFractionItem">
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblFraction"><%=GetLabel("Artikel Pemeriksaan")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnFractionID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtFractionCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtFractionName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Cetak")%></label></td>
                                <td><asp:TextBox ID="txtDisplayOrder" CssClass="number required" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" value="<%#: Eval("ID")%>" bindingfield = "ID"/>
                                                <input type="hidden" value="<%#: Eval("ItemID")%>" bindingfield= "ItemID"/>
                                                <input type="hidden" value="<%#: Eval("ItemCode")%>" bindingfield= "ItemCode" />
                                                <input type="hidden" value="<%#: Eval("ItemName1")%>" bindingfield= "ItemName"/>                                                
                                                <input type="hidden" value="<%#: Eval("FractionID")%>" bindingfield= "FractionID"/>
                                                <input type="hidden" value="<%#: Eval("FractionCode")%>" bindingfield= "FractionCode" />
                                                <input type="hidden" value="<%#: Eval("FractionName1")%>" bindingfield= "FractionName"/>
                                                <input type="hidden" value="<%#: Eval("DisplayOrder")%>" bindingfield= "DisplayOrder"/>
                                                <input type="hidden" value="<%#: Eval("IsTestItem")%>" bindingfield= "IsTestItem"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FractionCode" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText tdFractionCode" HeaderText="Kode Pemeriksaan" HeaderStyle-Width="120px"/>
                                        <asp:BoundField DataField="Name" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText tdFractionName1" HeaderText="Artikel Pemeriksaan" />
                                        <asp:BoundField DataField="DisplayOrder" ItemStyle-CssClass="tdDisplayOrder" HeaderText="Urutan Cetak" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data tidak tersedia")%>
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
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>

