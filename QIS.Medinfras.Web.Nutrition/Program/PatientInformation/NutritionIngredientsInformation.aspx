<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="NutritionIngredientsInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionIngredientsInformation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

//            $('#<%=txtDate.ClientID %>').change(function () {
//                cbpView.PerformCallback('refresh');
//            });

            setDatePicker('<%=txtDate.ClientID %>');
        });

//        function onCboMealTimeChanged() {
//            cbpView.PerformCallback('refresh');
//        }

//        function oncboServiceUnitChanged() {
//            cbpView.PerformCallback('refresh');
//        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpProcessEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }
//        function onCboMealTimeChanged() {
//            cbpView.PerformCallback('refresh');
//        }

    </script>
    <div>
        <div style="padding: 2px;" id="containerOrder" class="containerOrder">
            <input type="hidden" value="" id="hdnGCMealDay" runat="server" />
            <input type="hidden" value="" id="hdnMealPlanID" runat="server" />
            <input type="hidden" value="" id="hdnGCItemDetailStatus" runat="server" />
            <input type="hidden" value="" id="hdnMealID" runat="server" />
            <input type="hidden" value="" id="hdnPageTitle" runat="server" />
            <input type="hidden" value="" id="hdnMealTime" runat="server" />
            <input type="hidden" value="" id="hdnServiceUnit" runat="server" />
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 100%;">
                            <colgroup>
                                <col style="width: 100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Tanggal Makan") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Jadwal Makan") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMealTime" Width="200px" runat="server" ClientInstanceName="cboMealTime">
                                        <%--<ClientSideEvents ValueChanged="function(s,e){onCboMealTimeChanged()}" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Bagian") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" runat="server" ClientInstanceName="cboServiceUnit">
                                        <%--<ClientSideEvents ValueChanged="function(s,e){oncboServiceUnitChanged()}" />--%>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="divShow1" class="divShow">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Bahan Baku Makan")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("Ingredients") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MealDay" HeaderText="Menu Hari ke - " HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="FoodQuantity" HeaderText="Jumlah Bahan Baku" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
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
        </div>
    </div>
</asp:Content>
