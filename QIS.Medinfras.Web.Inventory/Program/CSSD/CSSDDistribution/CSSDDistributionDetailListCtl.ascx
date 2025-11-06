<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CSSDDistributionDetailListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDDistributionDetailListCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewPopup.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>
<input type="hidden" id="hdnDistributionID" value="" runat="server" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="lblDistribution">
                            <%=GetLabel("No. Distribusi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemDistributionNo" Width="150px" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" runat="server" id="Label1">
                            <%=GetLabel("Dari Lokasi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromLocationName" Width="300px" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" runat="server" id="lblLocation">
                            <%=GetLabel("Ke Lokasi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToLocationName" Width="300px" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="vertical-align:top">
                        <%=GetLabel("Keterangan") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; height:290px; overflow-y:scroll;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="ItemName1" HeaderText="Item Name" />
                                    <asp:BoundField DataField="CustomItemUnit" HeaderText="Quantity" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="CustomConversion" HeaderText="Konversi" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CustomItemDistribution" HeaderText="Total" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("No Data To Display")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
            <div class="containerPaging">
                <div class="wrapperPaging">
                    <div id="pagingPopup">
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
