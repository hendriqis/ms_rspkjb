<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatePharmacyChargesEntryItemCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePharmacyChargesEntryItemCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    $('#btnCancelCtl').click(function () {
        $('#containerEntryDataCtl').hide();
    });

    $('#btnSaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsTemplateCharges', 'mpTemplateCharges'))
            cbpTemplateChargesCtl.PerformCallback('save');
        return false;
    });

    $('#lblAddDataCtl').die('click');
    $('#lblAddDataCtl').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtQty.ClientID %>').val('');
        $('#<%=txtItemUnit.ClientID %>').val('');
        $('#containerEntryDataCtl').show();
    });

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        $('#<%=txtQty.ClientID %>').val(entity.Quantity);
        $('#<%=txtItemUnit.ClientID %>').val(entity.DetailItemUnit);
        $('#containerEntryDataCtl').show();
    });

    $('.imgDeleteCtl.imgLink').die('click');
    $('.imgDeleteCtl.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpTemplateChargesCtl.PerformCallback('delete');
        }
    });

    //#region Item
    function onGetItemMasterFilterExp() {
        var filterExpression = "<%:onGetItemMasterFilter() %>";
        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        var filterExpression = onGetItemMasterFilterExp();
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        if (value != '') {
            var filterItem = onGetItemMasterFilterExp() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetvItemMasterList', filterItem, function (resultp) {
                if (resultp != null) {
                    $('#<%=hdnItemID.ClientID %>').val(resultp.ItemID);
                    $('#<%=txtItemCode.ClientID %>').val(resultp.ItemCode);
                    $('#<%=txtItemName.ClientID %>').val(resultp.ItemName1);
                    $('#<%=txtItemUnit.ClientID %>').val(resultp.ItemUnit);
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtItemUnit.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion

    function cbpTemplateChargesCtlEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
        }
        hideLoadingPanel();
    }

    function oncboTestResultSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpTemplateChargesCtl.PerformCallback('refresh');
    }

    $('#btnCloseCtl').click(function (evt) {
        pcRightPanelContent.Hide();
        onAfterChangeResult();
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnChargesTemplateID" />
    <input type="hidden" value="" runat="server" id="hdnParamItemType" />
    <input type="hidden" value="" runat="server" id="hdnHealthcareServiceUnitID" />
    <input type="hidden" value="" runat="server" id="hdnLocationID" />
    <input type="hidden" value="" runat="server" id="hdnLogisticLocationID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 800px">
                    <colgroup>
                        <col style="width: 5px" />
                        <col style="width: 350px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("Template Name : ")%></label>
                            </b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargesTemplateName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsTemplateCharges" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 10%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
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
                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jumlah") %>
                                    </label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtQty" Width="80px" runat="server" CssClass="number" input type="number"
                                                    Style="text-align: center" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemUnit" ReadOnly="true" Width="50%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
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
                <dxcp:ASPxCallbackPanel ID="cbpTemplateChargesCtl" runat="server" Width="100%" ClientInstanceName="cbpTemplateChargesCtl"
                    ShowLoadingPanel="false" OnCallback="cbpTemplateChargesCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpTemplateChargesCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEditCtl" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: center; margin-right: 2px;" />
                                                    <img class="imgLink imgDeleteCtl" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="float: center; margin-right: 2px;" />
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ChargesTemplateID") %>" bindingfield="ChargesTemplateID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("DetailItemUnit") %>" bindingfield="DetailItemUnit" />
                                                    <input type="hidden" value="<%#:Eval("Quantity") %>" bindingfield="Quantity" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="7%"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="50%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Item")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("ItemName1")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                                <HeaderTemplate>
                                                    <div style="text-align: center; padding-left: 3px">
                                                        <%=GetLabel("Jumlah")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: right; padding: 3px">
                                                        <div>
                                                            <%#: Eval("cfQty")%>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblAddDataCtl">
                     <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="width: 100%; text-align: right">
<input type="button" value='<%= GetLabel("Close")%>' id="btnCloseCtl" style="width: 100px" />
</div>
