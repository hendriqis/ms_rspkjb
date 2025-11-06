<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionOrderDetailListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.TransactionOrderDetailListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

</script>
<input type="hidden" value="" id="hdnSelectedMemberCtl" runat="server" />
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderTypeID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Order")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTestOrderHdNo" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Penunjang")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" class="hdnOrderID" value='<%#: Eval("OrderID")%>' />
                                                    <input type="hidden" class="hdnOrderType" value='<%#: Eval("OrderType")%>' />
                                                    <input type="hidden" class="hdnGCTestOrderStatus" value='<%#: Eval("GCItemStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Pelayanan") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("ItemName1")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Qty") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div align="Right">
                                                    <%#: Eval("Qty")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Unit") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div align="Center">
                                                    <%#: Eval("UnitName")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"/>
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
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="onClosePopUp();pcRightPanelContent.Hide();" />
    </div>
</div>
