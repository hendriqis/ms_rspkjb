<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingPaymentInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingPaymentInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_RevenueSharingPaymentInfoCtl">
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
<input type="hidden" id="hdnRSPaymentID" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Detail Information")%></div>
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width: 70%">
                <colgroup>
                    <col style="width: 160px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("No. Rekap Jasa Medis")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRSPaymentNo" ReadOnly="true" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Tanggal Rekap")%></label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtRSPaymentDate" ReadOnly="true" Width="200px" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 400px;
                                overflow-y: scroll;">
                                <table class="grdView grdSelected" cellspacing="0" width="100%" rules="all">
                                    <tr>
                                        <th align="left" style="width: 150px">
                                            <%=GetLabel("No. Rekap Jasa Medis")%>
                                        </th>
                                        <th align="center" style="width: 150px">
                                            <%=GetLabel("Tgl. Rekap Jasa Medis")%>
                                        </th>
                                        <th align="left">
                                            <%=GetLabel("Dokter/Paramedis")%>
                                        </th>
                                        <th align="right">
                                            <%=GetLabel("Nilai Rekap Jasa Medis")%>
                                        </th>
                                        <th align="left" style="width: 150px">
                                            <%=GetLabel("Dibuat Oleh")%>
                                        </th>
                                        <th align="left" style="width: 150px">
                                            <%=GetLabel("Dibuat Pada")%>
                                        </th>
                                    </tr>
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <tr class="trEmpty">
                                                <td colspan="15">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#:Eval("ID") %>
                                                </td>
                                                <td>
                                                    <label class="lblLink lblTransRevenueSharingSummaryNo">
                                                        <%#: Eval("RSSummaryNo") %></label>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("cfRSSummaryDateInString") %>
                                                </td>
                                                <td>
                                                    <%#: Eval("ParamedicName") %>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfTotalRevenueSharingAmountInString") %>
                                                </td>
                                                <td>
                                                    <%#: Eval("CreatedByName") %>
                                                </td>
                                                <td>
                                                    <%#: Eval("cfCreatedDateInString") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </table>
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
