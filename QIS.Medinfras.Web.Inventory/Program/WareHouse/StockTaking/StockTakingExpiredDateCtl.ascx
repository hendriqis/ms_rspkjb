<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockTakingExpiredDateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.StockTakingExpiredDateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        setDatePicker('<%=txtExpiredDate.ClientID %>');
        $('#btnPopupCancel').click(function () {
            $('#trEditEntry').hide();
        });

        $('#btnPopupSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxCtlPopup', 'mpTrxCtlPopup')) {
                cbpPopupProcess.PerformCallback('save');
            }
        });
    });

    $('#lblPopupAddData').die('click');
    $('#lblPopupAddData').live('click', function () {
        $('#<%=txtBatchNumber.ClientID %>').removeAttr('ReadOnly');
        $('#<%=hdnBatchNumber.ClientID %>').val('');
        $('#<%=txtBatchNumber.ClientID %>').val('');
        $('#<%=txtQuantity.ClientID %>').val('');
        $('#<%=txtExpiredDate.ClientID %>').val('');
        $('#<%=txtQuantity.ClientID %>').val($('#<%=hdnQuantityEnd.ClientID %>').val());
        $('#trEditEntry').show();
    });
    
    $('.imgPopupEdit').die('click');
    $('.imgPopupEdit').live('click', function () {
        $tr = $(this).closest('tr');

        var id = $tr.find('.keyField').html();
        var batchNumber = $tr.find('.batchNumber').html();
        var filterExpression = "StockTakingID = " + $('#<%=hdnStockTakingID.ClientID %>').val() + " AND ItemID = " + id + " AND BatchNumber = " + batchNumber;
        Methods.getObject('GetStockTakingDtExpiredList', filterExpression, function (result) {
            if (result != null) {
                $('#trEditEntry').show();
                $('#<%=hdnStockTakingID.ClientID %>').val(result.StockTakingID);
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtBatchNumber.ClientID %>').val(result.BatchNumber);
                $('#<%=txtBatchNumber.ClientID %>').attr('ReadOnly', 'true');
                $('#<%=hdnBatchNumber.ClientID %>').val(result.BatchNumber);
                $('#<%=txtExpiredDate.ClientID %>').val(result.ExpiredDateInDatePickerFormat);
                $('#<%=txtQuantity.ClientID %>').val(result.Quantity);
            }
        });
    });

    $('.imgPopupDelete').die('click');
    $('.imgPopupDelete').live('click', function () {
        $tr = $(this).closest('tr');
        var id = $tr.find('.keyField').html();
        $('#<%=hdnItemID.ClientID %>').val($tr.find('.keyField').html());
        $('#<%=txtBatchNumber.ClientID %>').val($tr.find('.batchNumber').html());
        cbpPopupProcess.PerformCallback('delete');
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        $('#<%=txtBatchNumber.ClientID %>').removeAttr('ReadOnly');
        $('#<%=hdnBatchNumber.ClientID %>').val('');
        $('#<%=txtBatchNumber.ClientID %>').val('');
        $('#<%=txtQuantity.ClientID %>').val('');
        $('#<%=txtExpiredDate.ClientID %>').val('');
        cbpPopupView.PerformCallback();
    }
</script>

<input type="hidden" runat="server" id="hdnStockTakingID" value="" />
<input type="hidden" runat="server" id="hdnItemID" value=""/>
<input type="hidden" runat="server" id="hdnBatchNumber" value=""/>
<input type="hidden" runat="server" id="hdnQuantityEnd" value="" />
<div class="pageTitle">
    <%=GetLabel("Expired Date Per Item")%></div>
<fieldset id="fsTrxCtlPopup">
<table class="tblContentArea">
    <tr id="trEditEntry" style="display:none;">
        <td>
            <div class="pageTitle"><%=GetLabel("Entry")%></div>
            <div>
                <table class="tblEntryDetail" style="width: 100%">
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td><label class="lblMandatory"><%=GetLabel("Batch Number") %></label></td>
                        <td><asp:TextBox runat="server" ID="txtBatchNumber"  /></td>
                    </tr>
                    <tr>
                        <td><label class="lblNormal"><%=GetLabel("Expired Date") %></label></td>
                        <td><asp:TextBox ID="txtExpiredDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><label class="lblNormal"><%=GetLabel("Quantity") %></label></td>
                        <td><asp:TextBox runat="server" ID="txtQuantity" Width="70px"/></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <input type="button" id="btnPopupSave" value='<%= GetLabel("Save")%>' />
                                    </td>
                                    <td>
                                        <input type="button" id="btnPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdNormal"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-Width="70px" >
                                        <ItemTemplate>
                                            <img class="imgPopupEdit" title='<%=GetLabel("Edit")%>' alt="" src='<%= ResolveUrl("~/Libs/Images/Button/edit.png")%>' />&nbsp;
                                            <img class="imgPopupDelete" title='<%=GetLabel("Delete")%>' alt="" src='<%= ResolveUrl("~/Libs/Images/Button/delete.png")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BatchNumber" HeaderStyle-CssClass="batchNumber" ItemStyle-CssClass="batchNumber" HeaderText = "Batch Number"  />
                                    <asp:BoundField DataField="ExpiredDateInString" HeaderText = "Expired Date"  />
                                    <asp:BoundField DataField="Quantity" HeaderText = "Qty" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  />
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("No Data To Display")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                        <div style="width:100%;text-align:center">
                            <span class="lblLink" id="lblPopupAddData" style=" text-align:center"><%= GetLabel("Add Data")%></span>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
            <div class="containerPaging">
                <div class="wrapperPaging">
                    <div id="paging">
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</table>
</fieldset>
