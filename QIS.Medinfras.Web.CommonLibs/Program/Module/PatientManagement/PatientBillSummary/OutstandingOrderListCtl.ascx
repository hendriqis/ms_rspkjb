<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutstandingOrderListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutstandingOrderListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="150px" HeaderText="Tanggal/Jam Order">
                                            <ItemTemplate>
                                                <label>
                                                    <%#: Eval("cfOrderDateTimeInString")%>
                                                </label>
                                                <br />
                                                <label style="font-style: italic; font-size: x-small">
                                                    <%#: Eval("cfScheduledDateInfo")%>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" HeaderText="Unit Pelayanan">
                                            <ItemTemplate>
                                                <label style="font-weight: bold">
                                                    <%#: Eval("ServiceUnitName")%>
                                                </label>
                                                <br />
                                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                                <label style="font-style: italic; font-size: xx-small">
                                                    <%=GetLabel("Unit Visit Asal : ")%>
                                                </label>
                                                <label style="font-style: italic; font-size: xx-small; font-weight: bold">
                                                    <%#: Eval("VisitServiceUnitName")%>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="150px" HeaderText="No. Order">
                                            <ItemTemplate>
                                                <label style="font-weight: bold">
                                                    <%#: Eval("OrderNo")%>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Paramedis" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Jenis Order" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="80px" HeaderText="Status Transaksi">
                                            <ItemTemplate>
                                                <label style="font-weight: bold">
                                                    <%#: Eval("TransactionStatus")%>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DetailTransaction" HeaderText="Transaksi Open/Total" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="120px" HeaderText="Informasi Dibuat">
                                            <ItemTemplate>
                                                <label style="font-size: x-small; font-style: italic">
                                                    <%#: Eval("CreatedByName")%>
                                                </label>
                                                <br />
                                                <label style="font-size: xx-small">
                                                    <%#: Eval("cfCreatedDateInFullString")%>
                                                </label>
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
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
