<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridInpatientRegistrationAllCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridInpatientRegistrationAllCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_gridreigsteredpatientallctl">
    
</script>
<div>
    <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
        ShowLoadingPanel="false">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                    <table id="grdView" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                        <tr>
                            <th style="width: 300px" rowspan="2" align="left">
                                <%=GetLabel("RUANG PERAWATAN")%>
                            </th>
                            <th colspan="<%=ClassCount %>" align="center">
                                <%=GetLabel("KELAS") %>
                            </th>
                            <th style="width: 120px" rowspan="2" align="center">
                                <%=GetLabel("TOTAL")%>
                            </th>
                        </tr>
                        <tr>
                            <asp:Repeater ID="rptClassCareHeader" runat="server">
                                <ItemTemplate>
                                    <th style="width: 120px">
                                        <%#: Eval("ClassName")%>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                            <LayoutTemplate>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#: Eval("ServiceUnitName")%>
                                    </td>
                                    <asp:Repeater ID="rptClassCare" runat="server">
                                        <ItemTemplate>
                                            <td align="right">
                                                <%#:Eval("Jumlah", "{0, 0:N2}")%>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <td align="right">
                                        <%#: Eval("TotalPasienServiceUnit")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <tr class="trEmpty">
                                    <td colspan="10">
                                        <%=GetLabel("Tidak ada pasien yang sedang dalam perawatan") %>
                                    </td>
                                </tr>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </table>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
