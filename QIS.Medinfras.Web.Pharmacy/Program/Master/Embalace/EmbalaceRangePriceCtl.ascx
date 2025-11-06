<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmbalaceRangePriceCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.EmbalaceRangePriceCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    <%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    
<script type="text/javascript" id="dxis_prescriptioncompoundentryctl" src='<%= ResolveUrl("~/Libs/Scripts/inlineEditing-1.0.js")%>'></script>
<script type="text/javascript" id="dxss_prescriptioncompoundentryctl">
    $('#lblPopupAddData').die('click');
    $('#lblPopupAddData').live('click', function () {
        $('#<%=txtStartingQty.ClientID %>').removeAttr('ReadOnly');
        $('#<%=txtEndingQty.ClientID %>').removeAttr('ReadOnly');
        
        $('#<%=txtStartingQty.ClientID %>').val('');
        $('#<%=txtEndingQty.ClientID %>').val('');
        $('#<%=txtTariff.ClientID %>').val('');
        $('#<%=hdnIsAdd.ClientID %>').val(1);
        $('#containerPopupEntryData').show();
    });

    $('#btnPopupCancel').click(function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnPopupSave').click(function () {
        cbpPopupView.PerformCallback('save');
        $('#containerPopupEntryData').hide();
    });

    $('.imgPopupEdit.imgLink').die('click');
    $('.imgPopupEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=txtStartingQty.ClientID %>').val(entity.StartingQty);
        $('#<%=txtEndingQty.ClientID %>').val(entity.EndingQty);
        $('#<%=txtTariff.ClientID %>').val(entity.Tariff);
        $('#<%=hdnIsAdd.ClientID %>').val(0);

        $('#<%=txtStartingQty.ClientID %>').attr('ReadOnly', 'readonly');
        $('#<%=txtEndingQty.ClientID %>').attr('ReadOnly', 'readonly');
        $('#containerPopupEntryData').show();
    });

    $('.imgPopupDelete.imgLink').die('click');
    $('.imgPopupDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=txtStartingQty.ClientID %>').val(entity.StartingQty);
        $('#<%=txtEndingQty.ClientID %>').val(entity.EndingQty);
        $('#<%=txtTariff.ClientID %>').val(entity.Tariff);

        cbpPopupView.PerformCallback('delete');
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<div class="pageTitle"><%=GetLabel("Embalace Range Price")%></div>
<div style="overflow-y: auto">
    <input type="hidden" id="hdnEmbalaceID" runat="server" value="" />
    <input type="hidden" id="hdnIsAdd" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td>
                <table width="350px">
                    <colgroup>
                        <col style="width:120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td><label class="lblNormal"><%=GetLabel("Embalace Code") %></label></td>
                        <td><asp:TextBox runat="server" ID="txtEmbalaceCode" ReadOnly="true" Width="120px" /></td>
                    </tr>
                    <tr>
                        <td><label class="lblNormal"><%=GetLabel("Embalace Name") %></label></td>
                        <td><asp:TextBox runat="server" ID="txtEmbalaceName" ReadOnly="true" Width="100%" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                                <div class="pageTitle"><%=GetLabel("Entry Range")%></div>
                                <table class="tblEntryDetail">
                                    <colgroup>
                                        <col style="width:120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Starting Qty") %></label></td>
                                        <td><asp:TextBox runat="server" ID="txtStartingQty" CssClass="number" /></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Ending Qty") %></label></td>
                                        <td><asp:TextBox runat="server" ID="txtEndingQty" CssClass="number" /></td>
                                    </tr>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("Tariff") %></label></td>
                                        <td><asp:TextBox runat="server" ID="txtTariff" CssClass="txtCurrency"/></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td><input type="button" id="btnPopupSave" value='<%= GetLabel("Save")%>' /></td>
                                                    <td><input type="button" id="btnPopupCancel" value='<%= GetLabel("Cancel")%>' /></td>
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
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                         <asp:TemplateField ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <img class="imgPopupEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                                &nbsp;
                                                                <img class="imgPopupDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                                <input type="hidden" value="<%#:Eval("StartingQty") %>" bindingfield="StartingQty" />
                                                                <input type="hidden" value="<%#:Eval("EndingQty") %>" bindingfield="EndingQty" />
                                                                <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="StartingQty" HeaderText="Starting Qty" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                                                        <asp:BoundField DataField="EndingQty" HeaderText="Ending Qty" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                                                        <asp:BoundField DataField="cfTariffInString" HeaderText="Tariff" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No Data To Display")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <div style="width:100%;text-align:center">
                                                    <span class="lblLink" id="lblPopupAddData" style=" text-align:center"><%= GetLabel("Add Data")%></span>
                                                </div>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="Div1" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="paging"></div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
   