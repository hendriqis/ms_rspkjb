<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequestEntryPackageCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestEntryPackageCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PurchaseRequestEntryPackageCtl">

    //#region Supplier
    $('#lblSupplierCtl.lblLink').click(function () {
        openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
            $('#<%=txtSupplierCodeCtl.ClientID %>').val(value);
            ontxtSupplierCodeCtlChanged(value);
        });
    });

    $('#<%=txtSupplierCodeCtl.ClientID %>').change(function () {
        ontxtSupplierCodeCtlChanged($(this).val());
    });

    function ontxtSupplierCodeCtlChanged(value) {
        var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSupplierIDCtl.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtSupplierNameCtl.ClientID %>').val(result.BusinessPartnerName);

                Methods.getDateDiffPOPORPerSupplier($('#<%=hdnSupplierIDCtl.ClientID %>').val(), function (result) {
                    if (result != null) {
                        $('#<%=txtETACtl1.ClientID %>').val(result.Hasil + ' Hari');
                    }
                    else {
                        $('#<%=txtETACtl1.ClientID %>').val('');
                    }
                });   
            }
            else {
                $('#<%=hdnSupplierIDCtl.ClientID %>').val('');
                $('#<%=txtSupplierCodeCtl.ClientID %>').val('');
                $('#<%=txtSupplierNameCtl.ClientID %>').val('');
                $('#<%=txtETACtl1.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Item Package
    function getItemProductFilterExpression() {
        var filterExpression = "<%:OnGetFilterExpressionItemProduct() %>";

        return filterExpression;
    }

    $('#lblItemPackage.lblLink').click(function () {
        openSearchDialog('item', getItemProductFilterExpression(), function (value) {
            $('#<%=txtPackageCode.ClientID %>').val(value);
            ontxtPackageCodeChanged(value);
        });
    });

    $('#<%=txtPackageCode.ClientID %>').change(function () {
        ontxtPackageCodeChanged($(this).val());
    });

    function ontxtPackageCodeChanged(value) {
        var filterExpression = getItemProductFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPackageID.ClientID %>').val(result.ItemID);
                $('#<%=txtPackageCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtPackageName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnPackageID.ClientID %>').val("");
                $('#<%=txtPackageCode.ClientID %>').val("");
                $('#<%=txtPackageName.ClientID %>').val("");
            }

            cbpViewPopup.PerformCallback("refresh");
        });
    }
    //#endregion

    $('#<%=txtQuantity.ClientID %>').change(function () {
        $('#<%=grdView.ClientID %> tr:gt(0)').each(function () {
            var itemQty = parseFloat($(this).find('.hdnItemQuantity').val());
            var BOMQty = parseFloat($(this).find('.hdnBOMQuantity').val());
            var allQty = parseFloat($('#<%=txtQuantity.ClientID %>').val());

            var qty = BOMQty / itemQty * allQty;
            $(this).find('.txtQuantityDt').val(qty).trigger('changeValue');
        });
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        $('#<%=grdView.ClientID %> .txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });
    }

    function getCheckedMember() {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var lstSelectedMemberQtyBOM = $('#<%=hdnSelectedMemberQtyBOM.ClientID %>').val().split(',');
        var result = '';
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var qtyBOM = $tr.find('.txtQuantityDt').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    lstSelectedMemberQtyBOM.push(qtyBOM);
                }
                else {
                    lstSelectedMemberQtyBOM[idx] = qtyBOM;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                    lstSelectedMemberQtyBOM.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQtyBOM.ClientID %>').val(lstSelectedMemberQtyBOM.join(','));
    }

    $('#chkCheckAll').live('click', function () {
        var isChecked = $(this).is(':checked');
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked).trigger('change');
        });
    });

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtQuantityDt').removeAttr('readonly');
        }
        else {
            $tr.find('.txtQuantityDt').attr('readonly', 'readonly');
        }
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Item BOM First';
            return false;
        }
        return true;
    }

</script>
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnSelectedMemberQtyBOM" runat="server" value="" />
<input type="hidden" id="hdnRequestIDCtl" runat="server" value="" />
<input type="hidden" id="hdnParam" runat="server" value="" />
<input type="hidden" id="hdnLocationIDCtl" runat="server" value="" />
<input type="hidden" id="hdnLocationItemGroupIDCtl" runat="server" value="" />
<input type="hidden" id="hdnGCLocationGroupCtl" runat="server" value="" />
<input type="hidden" id="hdnProductLineIDCtl" runat="server" value="" />
<input type="hidden" id="hdnProductLineItemTypeCtl" runat="server" value="" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 150px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink" id="lblSupplierCtl">
                            <%=GetLabel("Supplier")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnSupplierIDCtl" value="" runat="server" />
                        <asp:TextBox ID="txtSupplierCodeCtl" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSupplierNameCtl" Width="250px" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label1">
                            <%=GetLabel("Rata Rata Waktu Pengiriman")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtETACtl1" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink lblMandatory" id="lblItemPackage">
                            <%=GetLabel("Paket Produksi")%></label>
                    </td>
                    <td>
                        <input type="hidden" id="hdnPackageID" value="" runat="server" />
                        <asp:TextBox ID="txtPackageCode" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPackageName" Width="250px" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Jumlah") %></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" CssClass="number min" min="0" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.90em; max-height: 250px; overflow-y: auto">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="BillOfMaterialID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="50px">
                                        <HeaderTemplate>
                                            <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BillOfMaterialCode" HeaderText="BOM Code" HeaderStyle-Width="150px" />
                                    <asp:BoundField DataField="BillOfMaterialName1" HeaderText="BOM Name" />
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Qty Min" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <div id="divQtyMin" runat="server">
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Qty Max" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <div id="divQtyMax" runat="server">
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Qty On Hand" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <div id="divQtyOnHand" runat="server">
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Qty On Hand RS" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <div id="divQtyOnHandRS" runat="server">
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Qty Diminta" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="hidden" value="<%#:Eval("ItemQuantity") %>" class="hdnItemQuantity" />
                                            <input type="hidden" value="<%#:Eval("BOMQuantity") %>" class="hdnBOMQuantity" />
                                            <asp:TextBox ID="txtQuantityDt" runat="server" CssClass="number txtQuantityDt" Width="90px"
                                                Text="0" ReadOnly="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("No data to display")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
    </tr>
</table>
