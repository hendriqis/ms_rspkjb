<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionOrderHeaderListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.TransactionOrderHeaderListCtl" %>
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
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
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
                                <%=GetLabel("Nama Item")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
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
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <input type="hidden" class="RegistrationID" value='<%#: Eval("RegistrationID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("No. Order") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("OrderNo")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Order") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("cfOrderDateInString")%>
                                                    |
                                                    <%#: Eval("OrderTime")%>
                                                </div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Dijadwalkan") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("cfScheduledDateInString")%>
                                                    |
                                                    <%#: Eval("ScheduledTime")%>
                                                </div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="230px">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Dibuat Oleh") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("CreatedByName")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Jumlah") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("Qty")%>
                                                    <%#: Eval("UnitName")%>
                                                </div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Status") %></div>
                                                <div style="color: Blue">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("ItemStatusWatermark")%></div>
                                                <div style="color: Blue">
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
