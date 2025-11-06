<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecipeInformationDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.RecipeInformationDetailCtl" %>
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
<input type="hidden" id="hdnItemID" runat="server" />
<input type="hidden" id="hdnLocationID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />

<%--<div class="pageTitle"><%=GetLabel("Recipe Detail Information")%></div>--%>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width:160px"/>
                </colgroup>
                <tr>
                    <td class="tdLabel" Width="10%"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                    <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:475px; overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="TransactionDateInString" HeaderStyle-Width="75px" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Resep" HeaderStyle-Width="135px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Pasien") %>    
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("PatientName") %> <br /> <%#:Eval("MedicalNo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="175px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Asal Resep") %>    
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("ParamedicName") %> <br /> <%#:Eval("ServiceUnitName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HomeAddress" HeaderText="Alamat" HeaderStyle-HorizontalAlign="Center"  />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right   " HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" >
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah") %>    
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("UsedQuantity", "{0:N}") %>&nbsp;<%#:Eval("ItemUnit") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedByUserName" HeaderText="Dibuat Oleh" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
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