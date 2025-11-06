<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ItemProductionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ItemProductionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPrintProductionLabel" runat="server" crudmode="R" style="display: none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print Label Produksi")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtProductionDate.ClientID %>');
            $('#<%=txtProductionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnPrintProductionLabel.ClientID %>').click(function () {
                var id = $('#<%=hdnProductionID.ClientID %>').val();
                if (id != 0) {
                    cbpView.PerformCallback('print');
                }
                else {
                    showToast('Warning', 'Pilih nomor produksi terlebih dahulu.');
                }
            });

            //#region Production No
            $('#lblProductionNo.lblLink').click(function () {
                openSearchDialog('itemproductionhd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtProductionNo.ClientID %>').val(value);
                    onTxtDistributionNoChanged(value);
                });
            });

            $('#<%=txtProductionNo.ClientID %>').change(function () {
                onTxtDistributionNoChanged($(this).val());
            });

            function onTxtDistributionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Location From
            function getLocationFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionFromLocation() %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').live('change', function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback();
                });
            }
            //#endregion

            //#region Location To
            function getLocationFilterExpressionTo() {
                var filterExpression = "<%:OnGetFilterExpressionToLocation() %>";
                return filterExpression;
            }

            $('#<%=lblLocationTo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpressionTo(), function (value) {
                    $('#<%=txtLocationCodeTo.ClientID %>').val(value);
                    onTxtLocationToCodeChanged(value);
                });
            });

            $('#<%=txtLocationCodeTo.ClientID %>').live('change', function () {
                onTxtLocationToCodeChanged($(this).val());
            });

            function onTxtLocationToCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDTo.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationNameTo.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnLocationIDTo.ClientID %>').val('');
                        $('#<%=txtLocationCodeTo.ClientID %>').val('');
                        $('#<%=txtLocationNameTo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item
            function getItemProductFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionItemProduct() %>";

                filterExpression += " AND GCItemStatus != 'X181^999'";

                return filterExpression;
            }

            $('#<%=lblItem.ClientID %>.lblLink').live('click', function () {
                var locationFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                if (locationFrom != null && locationFrom != "" && locationFrom != "0") {
                    openSearchDialog('item', getItemProductFilterExpression(), function (value) {
                        $('#<%=txtItemCode.ClientID %>').val(value);
                        onTxtItemCodeChanged(value);
                    });
                } else {
                    displayMessageBox('MEDINFRAS', "Harap pilih Dari Lokasi terlebih dahulu.");
                }
            });

            $('#<%=txtItemCode.ClientID %>').live('change', function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemProductFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback();
                });
            }
            //#endregion

            $('#<%=txtQuantity.ClientID %>').change(function () {
                var totalCostAmount = 0;
                $('#<%=grdView.ClientID %> tr:gt(0)').each(function () {
                    var itemQty = parseFloat($(this).find('.hdnItemQuantity').val());
                    var BOMQty = parseFloat($(this).find('.hdnBOMQuantity').val());
                    var allQty = parseFloat($('#<%=txtQuantity.ClientID %>').val());

                    var qty = BOMQty / itemQty * allQty;
                    $(this).find('.txtQuantityDt').val(qty.toFixed(2)).trigger('changeValue');

                    var cost = parseFloat($(this).find('.hdnCostAmount').val()) * qty;
                    totalCostAmount += cost;
                    $(this).find('.txtCostAmount').val(cost).trigger('changeValue');
                });

                $('#<%=hdnTotalCostAmount.ClientID %>').val(totalCostAmount.toFixed(2));
                $('#<%=txtUnitPrice.ClientID %>').val(totalCostAmount.toFixed(2)).trigger('changeValue');
                $('#<%=txtFixedCostAmount.ClientID %>').val(totalCostAmount.toFixed(2)).trigger('changeValue');
            });

            $('#<%=grdView.ClientID %> .txtCurrency').each(function () {
                $(this).val(parseFloat($(this).val())).trigger('changeValue');
            });
        }

        function onBeforeSaveRecord(errMessage) {
            $('#<%=grdView.ClientID %> .txtQuantityDt').each(function () {
                $(this).removeAttr('readonly');
            });
            var isValid = (IsValid(null, 'fsMPEntry', 'mpEntry'))
            $('#<%=grdView.ClientID %> .txtQuantityDt').each(function () {
                $(this).attr('readonly', 'readonly');
            });
            return isValid;
        }

        function onCbpViewEndCallback(s) {
            $('#<%=grdView.ClientID %> .txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            hideLoadingPanel();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var productionID = $('#<%=hdnProductionID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (printStatus == 'true') {
                if (productionID == '' || productionID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else {
                    if (code == 'IM-00043') {
                        filterExpression.text = productionID;
                    }
                    else {
                        filterExpression.text = "ProductionID = " + productionID;
                    }
                    return true;
                }
            }
            else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnProductionID" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnTotalCostAmount" runat="server" />
    <input type="hidden" runat="server" id="hdnIsDotMatrix" value="0" />
    <div style="height: 495px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProductionNo">
                                    <%=GetLabel("No. Produksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblItem">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Jumlah") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" Width="60px" CssClass="number min" min="0" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Harga Satuan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnitPrice" Width="120px" CssClass="txtCurrency" runat="server" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsFixedCost" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Fixed Cost")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Cost Amount") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFixedCostAmount" Width="120px" CssClass="txtCurrency" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductionDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Batch Number") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchNumber" Width="250px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No Referensi") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="250px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="BillOfMaterialID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="BillOfMaterialCode" HeaderText="BOM Code" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="160px" />
                                            <asp:BoundField DataField="BillOfMaterialName1" HeaderText="BOM Name" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SequenceNo" HeaderText="Sequence No" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px" />
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Stok" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <div id="divRemainingStock" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ItemQuantity") %>" class="hdnItemQuantity" />
                                                    <input type="hidden" value="<%#:Eval("BOMQuantity") %>" class="hdnBOMQuantity" />
                                                    <asp:TextBox ID="txtQuantityDt" runat="server" CssClass="number txtQuantityDt" ReadOnly="true"
                                                        Width="145px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Cost Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("CostAmountNew") %>" class="hdnCostAmount"
                                                        id="hdnCostAmount" />
                                                    <asp:TextBox ID="txtCostAmount" runat="server" CssClass="txtCurrency txtCostAmount"
                                                        ReadOnly="true" Width="195px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 600px;">
                                        <div class="pageTitle" style="text-align: center">
                                            <%=GetLabel("Informasi")%></div>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="600px" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="30px" />
                                                </colgroup>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divLastUpdatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
