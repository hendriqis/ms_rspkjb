<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionWorkListMenuCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionWorkListMenuCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnMealPlanDtID" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanDtIDTemplate" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanIDTemplate" value="" runat="server" />
    <input type="hidden" id="hdnScheduleDate" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <tr>
                        <td style="vertical-align: top">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Panel Menu")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanCode" ReadOnly="true" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanName" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jadwal Makan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMealTime" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MealPlanID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderText="Kode Menu" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <label><%#:Eval("MealCode") %></label>
                                                <input type="hidden" value="<%#:Eval("MealID") %>" bindingfield="MealID" />
                                                <input type="hidden" value="<%#:Eval("MealCode") %>" bindingfield="MealCode" />
                                                <input type="hidden" value="<%#:Eval("MealName") %>" bindingfield="MealName" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MealName" HeaderText="Nama Menu" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="JumlahPorsi" HeaderText="Jumlah Porsi" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                </td>
        </tr>
    </table>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpPrintJobOrder" runat="server" Width="100%" ClientInstanceName="cbpPrintJobOrder"
        ShowLoadingPanel="false" OnCallback="cbpPrintJobOrder_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){       
                var printResult = s.cpZebraPrinting;
                if (printResult != '')
                    showToast('Warning', printResult);
                hideLoadingPanel();}" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="width: 100%; text-align: center">
        <br />
        <input type="button" value='<%= GetLabel("Print Job Order")%>' style="min-width:110px; min-height:25px" onclick="cbpPrintJobOrder.PerformCallback('print');" />
    </div>
</div>
