<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReceiveExpiredDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseReceiveExpiredDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
    });
</script>

<input type="hidden" runat="server" id="hdnID" value=""/>
<input type="hidden" runat="server" id="hdnBatchNumber" value=""/>
<fieldset id="fsTrxCtlPopup">  
<table class="tblContentArea">
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdNormal"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="BatchNumber" HeaderStyle-CssClass="batchNumber" ItemStyle-CssClass="batchNumber" HeaderText = "Batch Number"  />
                                    <asp:BoundField DataField="ExpiredDateInString" HeaderText = "Expired Date"  />
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
                    <div id="paging">
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
</fieldset>
