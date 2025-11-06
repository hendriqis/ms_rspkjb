<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingVerificationEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingVerificationEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div>
    <input type="hidden" id="hdnRSTransactionID" value="" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nomor Bukti")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtRevenueSharingNo" ReadOnly="true" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Proses")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtProcessDate" ReadOnly="true" Width="200px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative;" id="dView">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <table class="grdRevenueSharing grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th class="keyField" rowspan="2">&nbsp;</th>
                                            <th ><%=GetLabel("Patient Name") %></th>
                                            <th ><%=GetLabel("Item Name") %></th>
                                            <th style="text-align:right; width:150px" ><%=GetLabel("Transaction Amount") %></th>
                                            <th style="text-align:right; width:150px" ><%=GetLabel("Discount Amount") %></th>
                                            <th style="text-align:right; width:150px" ><%=GetLabel("Revenue Amount") %></th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField"><%#:Eval("TransactionDtID") %></td>
                                                    <td><%#:Eval("PatientName") %></td>
                                                    <td><%#:Eval("ItemName") %></td>
                                                    <td align="right"><%#:Eval("TransactionAmount","{0:N2}") %></td>
                                                    <td align="right"><%#:Eval("DiscountAmount","{0:N2}") %></td>
                                                    <td align="right"><%#:Eval("RevenueSharingAmount", "{0:N2}")%></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <tr>
                                            <td colspan="6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"><h4><%=GetLabel("PENAMBAHAN")%></h4></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:GridView ID="grdView2" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="AdjusmentType" HeaderText="Tipe" />
                                                        <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N}" HeaderText="Jumlah" />
                                                        <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Dibuat Pada" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No Data To Display")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"><h4><%=GetLabel("PENGURANGAN")%></h4></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:GridView ID="grdView3" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="AdjusmentType" HeaderText="Tipe" />
                                                        <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N}" HeaderText="Jumlah" />
                                                        <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Dibuat Pada" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No Data To Display")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </td>
                                        </tr>
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
