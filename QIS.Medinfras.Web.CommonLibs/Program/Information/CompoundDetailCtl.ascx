<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompoundDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CompoundDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<style type="text/css">
    pre
        {
            font-family: Segoe UI;
            white-space: pre-wrap; /* Since CSS 2.1 */
            white-space: -moz-pre-wrap; /* Mozilla, since 1999 */
            white-space: -pre-wrap; /* Opera 4-6 */
            white-space: -o-pre-wrap; /* Opera 7 */
            word-wrap: break-word; /* Internet Explorer 5.5+ *
        } 
</style>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
                cbpPopupView.PerformCallback('refresh');
            }
        else
            $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();
    }
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnPrescriptionOrderDetailID" runat="server" />
<table class="tblContentArea">
    <tr>
        <td valign="top">
            <div style="vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback(s)}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                            ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="25px">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("cfIsRFlag") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Nama Obat") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-weight: bold;font-size:13pt">
                                                    <%#:Eval("cfItemName") %></div>
                                                <div <%# Eval("IsCompound").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <pre><%#:Eval("cfCompoundDetail") %></pre></div> 
                                                <div style="font-style:italic">
                                                    <%#:Eval("cfSignaRule") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah Resep") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("DispenseQty") %>&nbsp;<%#:Eval("DosingUnit") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px"> 
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah Diambil") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("TakenQty") %>&nbsp;<%#:Eval("DosingUnit") %>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </td>
    </tr>
</table>
