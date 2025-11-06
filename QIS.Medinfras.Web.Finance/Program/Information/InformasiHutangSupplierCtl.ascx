<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformasiHutangSupplierCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InformasiHutangSupplierCtl" %>
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
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });
    
    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
 
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnBusinessPartnerID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />

<div class="pageTitle"><%=GetLabel("Detail Information")%></div>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width:160px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Supplier")%></label></td>
                    <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:330px; overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="MovementID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="MovementDateInString" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"  />
                                        <asp:BoundField DataField="BalanceBEGIN" HeaderText="Saldo Awal" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" DataFormatString="{0:n}" />
                                        <asp:BoundField DataField="BalanceIN" HeaderText="Jml Masuk" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" DataFormatString="{0:n}" />
                                        <asp:BoundField DataField="BalanceOUT" HeaderText="Jml Klr" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" DataFormatString="{0:n}" />
                                        <asp:BoundField DataField="BalanceEND" HeaderText="Saldo Akhir" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" DataFormatString="{0:n}" />
                                        <asp:TemplateField HeaderText="Transaksi">
                                            <ItemTemplate>
                                                <%#:Eval("TransactionDescription")%><br />
                                                <%#:Eval("DetailDesc")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransactionNo" HeaderText="No Transaksi" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"  />
                                        <asp:BoundField DataField="LastUpdatedByName" HeaderText="User" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>