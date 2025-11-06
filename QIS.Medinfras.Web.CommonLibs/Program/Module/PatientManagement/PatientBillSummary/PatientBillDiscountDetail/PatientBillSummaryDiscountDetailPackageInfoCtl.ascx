<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryDiscountDetailPackageInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryDiscountDetailPackageInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientBillSummaryDiscountDetailPackageInfoCtl">
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDtCtl').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnChargesDtID" runat="server" value="" />
    <input type="hidden" id="hdnChargesHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionDate" value="" runat="server" />
    <input type="hidden" id="hdnTransactionTime" value="" runat="server" />
    <input type="hidden" id="hdnItemID" runat="server" value="" />
    <input type="hidden" id="hdnChargesClassID" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
    <div>
        <table class="tblEntryContent" style="width: 90%">
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 400px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Paket Pelayanan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li class="selected" contentid="containerServiceCtl">
                <%=GetLabel("PELAYANAN") %></li>
            <li contentid="containerDrugMSCtl">
                <%=GetLabel("OBAT & ALKES") %></li>
            <li contentid="containerLogisticsCtl">
                <%=GetLabel("BARANG UMUM") %></li>
        </ul>
    </div>
    <div id="containerServiceCtl" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                        <input type="hidden" class="ItemCode" value="<%#: Eval("ItemCode")%>" />
                                                        <input type="hidden" class="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" class="ParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                        <input type="hidden" class="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                        <input type="hidden" class="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                        <input type="hidden" class="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                        <input type="hidden" class="RevenueSharingName" value="<%#: Eval("RevenueSharingCode")%>" />
                                                        <input type="hidden" class="ChargedQuantity" value="<%#: Eval("ChargedQuantity")%>" />
                                                        <input type="hidden" class="Tariff" value="<%#: Eval("Tariff")%>" />
                                                        <input type="hidden" class="TariffComp1" value="<%#: Eval("TariffComp1")%>" />
                                                        <input type="hidden" class="TariffComp2" value="<%#: Eval("TariffComp2")%>" />
                                                        <input type="hidden" class="TariffComp3" value="<%#: Eval("TariffComp3")%>" />
                                                        <input type="hidden" class="Discount" value="<%#: Eval("DiscountAmount")%>" />
                                                        <input type="hidden" class="DiscountComp1" value="<%#: Eval("DiscountComp1")%>" />
                                                        <input type="hidden" class="DiscountComp2" value="<%#: Eval("DiscountComp2")%>" />
                                                        <input type="hidden" class="DiscountComp3" value="<%#: Eval("DiscountComp3")%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Item")%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("ItemName1")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                            <%#: Eval("ItemCode")%></div>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("ChargedQuantity")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tariff" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <b>
                                                                <%#: Eval("Tariff", "{0:N2}")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i><%#: Eval("TariffComp1", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i><%#: Eval("TariffComp2", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i><%#: Eval("TariffComp3", "{0:N2}")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <b>
                                                                <%#: Eval("DiscountAmount", "{0:N2}")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i><%#: Eval("DiscountComp1", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i><%#: Eval("DiscountComp2", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i><%#: Eval("DiscountComp3", "{0:N2}")%></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <%#: Eval("cfLineAmount", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerDrugMSCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewObat" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupViewObat"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewObat_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupObat').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewObatEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewObat" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewObat" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdViewObat_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" class="IDObat" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDObat" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeObat" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Obat" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDObat" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeObat" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameObat" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="RevenueSharingIDObat" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="RevenueSharingNameObat" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="ChargedQuantityObat" value="<%#: Eval("ChargedQuantity")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Item")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: left;">
                                                        <b>
                                                            <%#: Eval("ItemName1")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                        <%#: Eval("ItemCode")%></div>
                                                    <div style="padding: 3px; text-align: left;">
                                                        <%#: Eval("ParamedicName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tariff" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <b>
                                                            <%#: Eval("Tariff", "{0:N2}")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c1 : ")%></i><%#: Eval("TariffComp1", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c2 : ")%></i><%#: Eval("TariffComp2", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c3 : ")%></i><%#: Eval("TariffComp3", "{0:N2}")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <b>
                                                            <%#: Eval("DiscountAmount", "{0:N2}")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c1 : ")%></i><%#: Eval("DiscountComp1", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c2 : ")%></i><%#: Eval("DiscountComp2", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c3 : ")%></i><%#: Eval("DiscountComp3", "{0:N2}")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <%#: Eval("cfLineAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerLogisticsCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewBarang" runat="server" Width="100%"
                        ClientInstanceName="cbpEntryPopupViewBarang" ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewBarang_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupBarang').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewBarangEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewBarang" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewBarang" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" OnRowDataBound="grdViewBarang_RowDataBound" ShowHeaderWhenEmpty="true"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <input type="hidden" class="IDBarang" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDBarang" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeBarang" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Barang" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDBarang" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeBarang" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameBarang" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="RevenueSharingIDBarang" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="RevenueSharingNameBarang" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="ChargedQuantityBarang" value="<%#: Eval("ChargedQuantity")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Item")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: left;">
                                                        <b>
                                                            <%#: Eval("ItemName1")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                        <%#: Eval("ItemCode")%></div>
                                                    <div style="padding: 3px; text-align: left;">
                                                        <%#: Eval("ParamedicName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tariff" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <b>
                                                            <%#: Eval("Tariff", "{0:N2}")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c1 : ")%></i><%#: Eval("TariffComp1", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c2 : ")%></i><%#: Eval("TariffComp2", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c3 : ")%></i><%#: Eval("TariffComp3", "{0:N2}")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <b>
                                                            <%#: Eval("DiscountAmount", "{0:N2}")%></b></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c1 : ")%></i><%#: Eval("DiscountComp1", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c2 : ")%></i><%#: Eval("DiscountComp2", "{0:N2}")%></div>
                                                    <div style="padding: 3px; text-align: left; font-size: x-small">
                                                        <i>
                                                            <%=GetLabel("c3 : ")%></i><%#: Eval("DiscountComp3", "{0:N2}")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <%#: Eval("cfLineAmount", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</div>
