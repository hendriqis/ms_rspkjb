<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemRequestProcessedDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemRequestProcessedDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_purchaserequestqtyonorderctl">
    
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnItemID" runat="server" />
    <input type="hidden" id="hdnOrderID" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td><asp:TextBox ID="txtItem" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Stok Tersedia")%></label></td>
                        <td><asp:TextBox ID="txtStock" ReadOnly="true" CssClass="number" Width="100px" runat="server" /></td>
                    </tr>   
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total Permintaan")%></label></td>
                        <td><asp:TextBox ID="txtItemRequestTotal" ReadOnly="true" CssClass="number" Width="100px" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sisa Stok")%></label></td>
                        <td><asp:TextBox ID="txtAvailableStock" ReadOnly="true" CssClass="number" Width="100px" runat="server" /></td>
                    </tr>  
                </table>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdBorder" cellspacing="0" rules="all">
                                            
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdItemRequest grdSelected grdBorder" cellspacing="0" rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">&nbsp;</th>
                                                <th rowspan="2"><%=GetLabel("No Permintaan")%></th>
                                                <th rowspan="2" align="center"><%=GetLabel("Jumlah Permintaan")%></th>
                                                <th colspan="2" align="center"><%=GetLabel("MINTA BELI")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width: 120px" align="center"><%=GetLabel("Order")%></th>
                                                <th style="width: 120px" align="center"><%=GetLabel("Terima")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="keyField"></td>
                                            <td><%#: Eval("ItemRequestNo")%></td>
                                            <td align="right">
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <colgroup>
                                                        <col />
                                                        <col style="width:35px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="lblReadOnlyText" align="right"><%#: Eval("ItemRequestQuantity")%></td>    
                                                        <td>&nbsp<%#: Eval("ItemUnit")%></td>
                                                    </tr>
                                                </table>                                                                                                        
                                            </td>
                                            <td align="right">
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <colgroup>
                                                        <col />
                                                        <col style="width:35px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="lblReadOnlyText" align="right"><%#: Eval("PurchaseRequestOrderQty")%></td>    
                                                        <td>&nbsp<%#: Eval("ItemUnit")%></td>
                                                    </tr>
                                                </table>                                                                                                        
                                            </td>
                                            <td align="right">
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <colgroup>
                                                        <col />
                                                        <col style="width:35px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="lblReadOnlyText" align="right"><%#: Eval("PurchaseRequestReceivedQty")%></td>    
                                                        <td>&nbsp<%#: Eval("ItemUnit")%></td>
                                                    </tr>
                                                </table>                                                                                                        
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>