<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationTakenInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationTakenInfoDetailCtl" %>
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
<input type="hidden" id="hdnPrescriptionOrderDetailID" runat="server" />
<input type="hidden" id="hdnPastMedicationID" runat="server" />
<input type="hidden" id="hdnItemName" runat="server" />
<input type="hidden" id="hdnParamedicName" runat="server" />
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width: 70%">
                <colgroup>
                    <col style="width: 120px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Item Name")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Prescribed By")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 340px;
                                overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="MedicationDateInString" HeaderStyle-Width="80px" HeaderText="Tanggal"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" />
                                        <asp:BoundField DataField="cfMedicationTime" HeaderStyle-Width="50px" HeaderText="Jam"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Dosing") %>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("NumberOfDosage", "{0:N}") %>&nbsp;<%#:Eval("DosingUnit") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Using UDD" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div <%# Eval("IsUsingUDD").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img id="imgIsUsingUDD" src='<%# ResolveUrl("~/Libs/Images/Status/completed.png")%>' runat="server" width="24" height="24" alt="" visible="true" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is IMM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div <%# Eval("IsIMM").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img id="imgIsIMM" src='<%# ResolveUrl("~/Libs/Images/Status/completed.png")%>' runat="server" width="24" height="24" alt="" visible="true" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is As Required" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div <%# Eval("IsAsRequired").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img id="imgIsAsRequired" src='<%# ResolveUrl("~/Libs/Images/Status/completed.png")%>' runat="server" width="24" height="24" alt="" visible="true" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diberikan Oleh" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("ParamedicName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Verifikasi Farmasi" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("cfPharmacistVerificationInfo") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Verifikasi DPJP" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("cfPhysicianVerificationInfo") %></div>
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
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
