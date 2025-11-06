<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoiceReceivingInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoiceReceivingInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ARInvoiceReceivingInformationCtl">
    $(function () {
    });

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncbpViewCtlEndCallback(s) {
        $('#containerImgLoadingView').hide();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
</script>
<input type="hidden" value="" id="hdnParam" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <table style="width: 50%">
                    <colgroup>
                        <col style="width: 25%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Invoice")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtARInvoiceNo" ReadOnly="true" Width="30%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ARReceivingID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="ARReceivingNo" HeaderText="No Pembayaran" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfARReceivingDateInString" HeaderText="Tanggal Pembayaran"
                                            HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="cfReceivingAmountInString" HeaderText="Jumlah Bayar" HeaderStyle-Width="180px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Info Pembuatan Pembayaran")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("CreatedByName")%></div>
                                                <div style="font-size: x-small">
                                                    <%#: Eval("cfCreatedDateTimeInString")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%=GetLabel("Info Alokasi Pembayaran")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("AlocationByName")%></div>
                                                <div style="font-size: x-small">
                                                    <%#: Eval("cfAlocationDateInDateTimeString")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
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
