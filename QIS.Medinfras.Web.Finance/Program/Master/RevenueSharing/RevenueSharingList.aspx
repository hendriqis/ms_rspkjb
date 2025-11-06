<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdRevenueSharing', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdRevenueSharing tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdRevenueSharing tr:eq(2)').click();
        }
        //#endregion

        $('.lblPatientOwnerStatus').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html().trim();
            var url = ResolveUrl("~/Program/Master/RevenueSharing/RevenueSharingPatientOwnerStatusCtl.ascx");
            openUserControlPopup(url, id, 'Formula Jasa Medis - Status Pasien', 900, 450);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <table class="grdRevenueSharing grdSelected" cellspacing="0" rules="all">
                            <tr>
                                <th class="keyField" rowspan="2">
                                    &nbsp;
                                </th>
                                <th style="width: 100px;" rowspan="2" align="left">
                                    <%=GetLabel("Kode Jasa Medis")%>
                                </th>
                                <th rowspan="2" align="left">
                                    <%=GetLabel("Nama Jasa Medis")%>
                                </th>
                                <th style="width: 100px" rowspan="2" align="right">
                                    <%=GetLabel("Pembagian")%>
                                </th>
                                <th style="width: 100px" rowspan="2" align="right">
                                    <%=GetLabel("Biaya Kartu Kredit") %>
                                </th>
                                <th style="width: 100px" rowspan="2" align="center">
                                    <%=GetLabel("Status Pasien")%>
                                </th>
                                <th colspan="20">
                                    <%=GetLabel("Detail")%>
                                </th>
                            </tr>
                            <asp:Repeater ID="rptRevenueSharingDtHeader" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <th style="width: 100px" align="right">
                                        <%#:Eval("StandardCodeName")%>
                                    </th>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                <EmptyDataTemplate>
                                    <tr class="trEmpty">
                                        <td colspan="10">
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="keyField">
                                            <%#: Eval("RevenueSharingID")%>
                                        </td>
                                        <td>
                                            <%#: Eval("RevenueSharingCode")%>
                                        </td>
                                        <td>
                                            <%#: Eval("RevenueSharingName")%>
                                        </td>
                                        <td align="right">
                                            <%#: Eval("DisplaySharingAmountWithoutRounding")%>
                                        </td>
                                        <td align="right">
                                            <%#: Eval("DisplayCreditCard")%>
                                        </td>
                                        <td align="center">
                                            <label class="lblLink lblPatientOwnerStatus">
                                                <%=GetLabel("Status Pasien")%></label>
                                        </td>
                                        <asp:Repeater ID="rptRevenueSharingDt" runat="server" OnItemDataBound="rptRevenueSharingDt_ItemDataBound">
                                            <ItemTemplate>
                                                <td style="width: 100px" id="tdRevenueSharingDtValue" runat="server" align="right">
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </table>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <td style="width: 100px" id="tdRevenueSharingDtValue" runat="server">
        </td>
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
</asp:Content>
