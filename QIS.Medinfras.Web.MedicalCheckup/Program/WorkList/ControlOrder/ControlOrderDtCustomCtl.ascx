<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlOrderDtCustomCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.ControlOrderDtCustomCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_mcudetailcustomctl">

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnSelectedOrderID.ClientID %>').val(entity.OrderID);
        $('#<%=hdnSelectedOrderDtID.ClientID %>').val(entity.OrderDtID);
        $('#<%=hdnSelectedTransactionID.ClientID %>').val(entity.TransactionID);
        $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(entity.TransactionDtID);
        $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val(entity.HealthcareServiceUnitID);
        $('#<%=hdnSelectedDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName1.ClientID %>').val(entity.ItemName1);
        $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);

        $('#containerPopupEntryData').show();
    });

    //#region Item Master
    function onGetItemMasterFilterExpression() {
        var filterExpression = "HealthcareServiceUnitID = '" + $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val() + "' AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblItemMaster.lblLink').live('click', function () {
        openSearchDialog('serviceunititem', onGetItemMasterFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtServiceItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtServiceItemCodeChanged($(this).val());
    });

    function onTxtServiceItemCodeChanged(value) {
        var filterExpression = onGetItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvServiceUnitItemList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName1.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName1.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Physician
    function onGetPhysicianFilterExpression() {
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblParamedic.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtServicePhysicianCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
        onTxtServicePhysicianCodeChanged($(this).val());
    });

    function onTxtServicePhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpViewPopup.PerformCallback('save');
        return false;
    });

    function onCbpViewPopupEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        hideLoadingPanel();
    }
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnVisitIDCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationNoCtl" runat="server" />
    <input type="hidden" id="hdnPatientNameCtl" runat="server" />
    <input type="hidden" id="hdnItemIDCtl" runat="server" />
    <input type="hidden" id="hdnItemCodeCtl" runat="server" />
    <input type="hidden" id="hdnItemNameCtl" runat="server" />
    <input type="hidden" id="hdnSelectedOrderID" runat="server" />
    <input type="hidden" id="hdnSelectedOrderDtID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" id="hdnSelectedHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnSelectedDepartmentID" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="20px" />
            <col width="350px" />
            <col width="35%" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoReg" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Nama Paket")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnVisitTypeID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Edit Detail Paket MCU")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItemMaster">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" Width="90%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName1" ReadOnly="true" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblParamedic">
                                        <%=GetLabel("Paramedic")%></label>
                                </td>
                                <td>
                                    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtParamedicCode" Width="90%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
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
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; max-height: 420px; overflow-y: auto">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("OrderID") %>" bindingfield="OrderID" />
                                                    <input type="hidden" value="<%#:Eval("OrderDtID") %>" bindingfield="OrderDtID" />
                                                    <input type="hidden" value="<%#:Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                    <input type="hidden" value="<%#:Eval("TransactionDtID") %>" bindingfield="TransactionDtID" />
                                                    <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                    <input type="hidden" value="<%#:Eval("DepartmentID") %>" bindingfield="DepartmentID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <img class="imgEdit <%# Eval("IsAllowChanged").ToString() == "False" ? "imgDisabled" : Eval("IsHasRevenue").ToString() == "1"  ? "imgDisabled" : "imgLink"%>"
                                                        title='<%=GetLabel("Edit")%>' src='<%# Eval("IsAllowChanged").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : Eval("IsHasRevenue").ToString() == "1" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="margin-right: 2px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="No. Order" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("OrderNo") %></div>
                                                    <div style="color: Navy; font-size: x-small; font-style: italic">
                                                        <%#: Eval("cfTransactionDisplay") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Unit Pelayanan" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("ServiceUnitName") %></div>
                                                    <div style="font-style: italic">
                                                        <%#:Eval("DepartmentName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Item" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("ItemName1") %>
                                                        (<%#:Eval("ItemCode") %>)</div>
                                                    <div style="font-style: italic">
                                                        <%#:Eval("ParamedicName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemQty" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="Quantity" HeaderStyle-Width="80px" />
                                            <asp:BoundField DataField="OrderStatus" HeaderText="Status" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
