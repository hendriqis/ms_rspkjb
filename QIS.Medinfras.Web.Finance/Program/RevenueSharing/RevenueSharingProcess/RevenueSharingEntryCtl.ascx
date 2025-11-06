<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div style="height: 400px; overflow-y: auto">
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Transaction No")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="50%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Patient Name")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative;" id="dView">
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <table class="grdRevenueSharing grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th class="keyField" rowspan="2">
                                                &nbsp;
                                            </th>
                                            <th rowspan="2" align="center">
                                                <%=GetLabel("Nama Item")%>
                                            </th>
                                            <th rowspan="2" align="center">
                                                <%=GetLabel("Nama Detail Item")%>
                                            </th>
                                            <th rowspan="2" align="center">
                                                <%=GetLabel("Peran") %>
                                            </th>
                                            <th rowspan="2" align="right">
                                                <%=GetLabel("Transaksi Asli")%>
                                            </th>
                                            <th rowspan="2" align="right">
                                                <%=GetLabel("Jumlah Transaksi")%>
                                            </th>
                                            <th colspan="<%=GetFormulaTypeCount() %>" align="center">
                                                <%=GetLabel("Komponen")%>
                                            </th>
                                            <th rowspan="2" align="center">
                                                <%=GetLabel("Dokter") %>
                                            </th>
                                        </tr>
                                        <tr>
                                            <asp:Repeater ID="rptRevenueSharingDtHeader" runat="server">
                                                <ItemTemplate>
                                                    <th align="center">
                                                        <%#:Eval("StandardCodeName")%>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#:Eval("ID") %>
                                                    </td>
                                                    <td>
                                                        <%#:Eval("ItemName1") %>
                                                    </td>
                                                    <td>
                                                        <%#:Eval("DetailItemName1") %>
                                                    </td>
                                                    <td align="center">
                                                        <%#:Eval("ParamedicRole") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("LineAmount","{0:N2}") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("TransactionAmount","{0:N2}") %>
                                                    </td>
                                                    <asp:Repeater ID="rptRevenueSharing" runat="server">
                                                        <ItemTemplate>
                                                            <td style="width: 100px" class="tdComponent" align="right">
                                                                <%#: String.Format("{0, 0:N2}", Container.DataItem)%>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <td align="right">
                                                        <%#:Eval("SharingAmount","{0:N2}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
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
            </td>
        </tr>
    </table>
</div>
