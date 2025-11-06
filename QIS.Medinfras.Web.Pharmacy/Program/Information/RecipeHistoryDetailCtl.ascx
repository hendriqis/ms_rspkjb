<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecipeHistoryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.RecipeHistoryDetailCtl" %>
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
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    var currPage = parseInt('<%=CurrPage %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        }, null, currPage);
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();
            else
                $('#<%=hdnID.ClientID %>').val('');

            setPaging($("#paging"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnPrescriptionOrderID" runat="server" />
<table class="tblContentArea" style="height: 550px">
    <tr>
        <td>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 60px" />
                    <col style="width: 300px" />
                    <col style="width: 20px" />
                    <col style="width: 80px" />
                    <col style="width: 350px" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <label>
                            <%=GetLabel("No. RM") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtMedicalNo" Width="150px" Enabled="false" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <label>
                            <%=GetLabel("No. Registrasi") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRegistrationNo" Width="150px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=GetLabel("Pasien") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPatientName" Width="100%" Enabled="false" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <label>
                            <%=GetLabel("Tanggal Resep") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPrescriptionDate" Width="150px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <%=GetLabel("Dokter") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPhysicianName" Width="100%" Enabled="false" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <label>
                            <%=GetLabel("Dibuat Oleh") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCreatedBy" Width="90%" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <label>
                            <%=GetLabel("Catatan") %></label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRemarksCtl" Width="90%" Enabled="false" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div style="position: relative;">
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
                                                <%=GetLabel(" ") %>
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
                                                <div style="font-weight: bold">
                                                    <%#:Eval("cfItemName") %></div>
                                                <div <%# Eval("IsCompound").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <pre><%#:Eval("cfCompoundDetail") %></pre>
                                                </div>
                                                <div style="font-style: italic">
                                                    <%#:Eval("cfSignaRule") %></div>
                                                <div style="font-style: italic">
                                                    <%#:Eval("MedicationAdministration") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="110px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah Resep") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("DispenseQty") %>&nbsp;<%#:Eval("DosingUnit") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="110px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Jumlah Diambil") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("TakenQty") %>&nbsp;<%#:Eval("DosingUnit") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="110px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Dibuat Oleh") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("CreatedByUserName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <img class="imgOutstanding" title='<%=GetLabel("cancel")%>' src='<%# ResolveUrl("~/Libs/Images/Status/cancel.png")%>'
                                                        style='<%# Eval("cfPrescriptionOrderStatus").ToString() == "True"? "width:25px": "width:24px;heght:24px;display:none" %>'
                                                        alt="" />
                                                </div>
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
