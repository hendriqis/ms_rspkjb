<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemPriceHistoryDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemPriceHistoryDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ItemPriceHistoryDetail">
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=lvwDetail.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewItemPriceHistoryDetail.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function onCbpViewItemPriceHistoryDetailEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=lvwDetail.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }


</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnFilterExpressionCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 50px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b><%=GetLabel("Item")%></b></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewItemPriceHistoryDetail" runat="server" Width="100%" ClientInstanceName="cbpViewItemPriceHistoryDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewItemPriceHistoryDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpViewItemPriceHistoryDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:ListView runat="server" ID="lvwDetail">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                </th>
                                                <th style="width: 100px; text-align: center" rowspan="2">
                                                    <%=GetLabel("Tanggal Histori")%>
                                                </th>
                                                <th colspan="4" style="text-align: center">
                                                    <%=GetLabel("Harga LAMA")%>
                                                </th>
                                                <th colspan="4" style="text-align: center">
                                                    <%=GetLabel("Harga BARU")%>
                                                </th>
                                                <th style="width: 200px; text-align: center" rowspan="2">
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Rata-Rata")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("HET")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Rata-Rata")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("HET")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                </th>
                                                <th style="width: 100px; text-align: center" rowspan="2">
                                                    <%=GetLabel("Tanggal Histori")%>
                                                </th>
                                                <th colspan="4" style="text-align: center">
                                                    <%=GetLabel("Harga LAMA")%>
                                                </th>
                                                <th colspan="4" style="text-align: center">
                                                    <%=GetLabel("Harga BARU")%>
                                                </th>
                                               <th style="width: 100px; text-align: center" rowspan="2">
                                                    <%=GetLabel("Updated by System")%>
                                                </th>
                                                <th style="width: 100px; text-align: center" rowspan="2">
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Rata-Rata")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("HET")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Rata-Rata")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Kecil")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("Satuan Besar")%>
                                                </th>
                                                <th style="width: 120px; text-align: center">
                                                    <%=GetLabel("HET")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <%#: Eval("cfHistoryDateInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("OldAveragePrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("OldUnitPrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("OldPurchasePrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("OldHETAmount", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NewAveragePrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NewUnitPrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NewPurchasePrice", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NewHETAmount", "{0:N2}")%>
                                            </td>
                                            <td align="center">
                                                <input type="checkbox" disabled="disabled" <%# Convert.ToBoolean(Eval("IsPriceLastUpdatedBySystem")) ? "checked" : "" %> /> 
                                            </td>
                                            <td align="center">
                                                <i><%#: Eval("CreatedByName")%></i>
                                                <br />
                                                <label style="font-size:smaller"><%#: Eval("cfCreatedDateInStringFullFormat")%></label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>