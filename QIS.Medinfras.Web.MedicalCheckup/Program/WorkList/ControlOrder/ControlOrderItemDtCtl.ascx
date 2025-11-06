<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlOrderItemDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.ControlOrderItemDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_mcudetailctl">
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnVisitIDCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationNoCtl" runat="server" />
    <input type="hidden" id="hdnPatientNameCtl" runat="server" />
    <input type="hidden" id="hdnItemIDCtl" runat="server" />
    <input type="hidden" id="hdnItemCodeCtl" runat="server" />
    <input type="hidden" id="hdnItemNameCtl" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="20px" />
            <col width="250px" />
            <col width="40%" />
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
        <tr></tr>
        <tr>
            <td colspan="3">
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
                                        <asp:TemplateField HeaderStyle-Width="250px" HeaderText = "Unit Pelayanan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("ServiceUnitName") %></div>
                                                <div style="font-style:italic"><%#:Eval("DepartmentName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px" HeaderText = "Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><%#:Eval("ItemName1") %></div>
                                                <div style="font-style:italic"><%#:Eval("ParamedicName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemQty" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderText="Quantity" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="OrderStatus" HeaderText="Status" HeaderStyle-Width="140px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
