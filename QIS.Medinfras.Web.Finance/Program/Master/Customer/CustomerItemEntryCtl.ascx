<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.CustomerItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtCustomerItemCode.ClientID %>').val('');
        $('#<%=txtCustomerItemName.ClientID %>').val('');
        $('#<%=hdnBillingGroupID.ClientID %>').val('');
        $('#<%=txtBillingGroupCode.ClientID %>').val('');
        $('#<%=txtBillingGroupName.ClientID %>').val('');
        
        $('#containerPopupEntryData').show();
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
        var billingGroupID = $row.find('.hdnBillingGroupID').val();
        var billingGroupCode = $row.find('.hdnBillingGroupCode').val();
        var billingGroupName = $row.find('.hdnBillingGroupName').val();

        var itemName = $row.find('.tdItemName').html();
        var customerItemCode = $row.find('.tdCustomerItemCode').html();
        var customerItemName = $row.find('.tdCustomerItemName').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=txtCustomerItemCode.ClientID %>').val(customerItemCode);
        $('#<%=txtCustomerItemName.ClientID %>').val(customerItemName);
        $('#<%=hdnBillingGroupID.ClientID %>').val(billingGroupID);
        $('#<%=txtBillingGroupCode.ClientID %>').val(billingGroupCode);
        $('#<%=txtBillingGroupName.ClientID %>').val(billingGroupName);

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
        $('#containerPopupEntryData').hide();
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

    //#region Item
    $('#lblItem.lblLink').live('click', function () {
        var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        var filterExpression = "ItemID NOT IN (SELECT ItemID FROM CustomerItem WHERE BusinessPartnerID = " + businessPartnerID + " AND IsDeleted = 0) AND IsDeleted = 0";
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtCustomerItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtCustomerItemCodeChanged($(this).val());
    });

    function onTxtCustomerItemCodeChanged(value) {
        var filterExpression = "ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtCustomerItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtCustomerItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtCustomerItemCode.ClientID %>').val('');
                $('#<%=txtCustomerItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion    

    //#region Billing Group
    $('#lblBillingGroup.lblLink').live('click', function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('billinggroup', filterExpression, function (value) {
            $('#<%=txtBillingGroupCode.ClientID %>').val(value);
            onTxtBillingGroupCodeChanged(value);
        });
    });

    $('#<%=txtBillingGroupCode.ClientID %>').live('change', function () {
        onTxtBillingGroupCodeChanged($(this).val());
    });

    function onTxtBillingGroupCodeChanged(value) {
        var filterExpression = "BillingGroupCode = '" + value + "'";
        Methods.getObject('GetBillingGroupList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBillingGroupID.ClientID %>').val(result.BillingGroupID);
                $('#<%=txtBillingGroupName.ClientID %>').val(result.BillingGroupName1);
            }
            else {
                $('#<%=hdnBillingGroupID.ClientID %>').val('');
                $('#<%=txtBillingGroupCode.ClientID %>').val('');
                $('#<%=txtBillingGroupName.ClientID %>').val('');
            }
        });
    }
    //#endregion    

</script>

<div style="height:440px; overflow-y:auto;overflow-x:hidden">
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Item")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Istansi")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItem"><%=GetLabel("Item")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnItemID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kode Item Instansi")%></label></td>
                                <td><asp:TextBox ID="txtCustomerItemCode" runat="server" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Item Instansi")%></label></td>
                                <td><asp:TextBox ID="txtCustomerItemName" runat="server" Width="100%" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal lblLink" id="lblBillingGroup"><%=GetLabel("Billing Group")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnBillingGroupID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtBillingGroupCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtBillingGroupName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnBillingGroupID" value="<%#: Eval("BillingGroupID")%>" />
                                                <input type="hidden" class="hdnBillingGroupCode" value="<%#: Eval("BillingGroupCode")%>" />
                                                <input type="hidden" class="hdnBillingGroupName" value="<%#: Eval("BillingGroupName1")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" ItemStyle-CssClass="tdItemName" HeaderText="Item Name" />
                                        <asp:BoundField DataField="CustomerItemCode" ItemStyle-CssClass="tdCustomerItemCode" HeaderText="Customer Item Code" />
                                        <asp:BoundField DataField="CustomerItemName" ItemStyle-CssClass="tdCustomerItemName" HeaderText="Customer Item Name" />
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
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

