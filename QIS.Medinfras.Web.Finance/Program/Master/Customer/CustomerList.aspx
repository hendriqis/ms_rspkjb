<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="CustomerList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
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

        function onCboCustomerTypeChanged() {
            $('#<%=hdnCustomerType.ClientID %>').val(cboCustomerType.GetValue());
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/Customer/CustomerItemEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 900, 500);
        });

        $('.lnkMember a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/Customer/CustomerMemberEntryCtl.ascx");
            openUserControlPopup(url, id, 'Anggota', 900, 500);
        });

        $('.lnkVirtualAccount a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/Customer/CustomerVirtualAccountEntryCtl.ascx");
            openUserControlPopup(url, id, 'Virtual Account', 900, 500);
        });
        
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnCustomerType" runat="server" value="" />
    <div>
        <table>
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 300px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tipe Instansi")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" Width="100%"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboCustomerTypeChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="BusinessPartnerID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="BusinessPartnerCode" HeaderText="Kode Instansi" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="BusinessPartnerName" HeaderText="Nama Instansi" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ShortName" HeaderText="Nama Singkat" HeaderStyle-Width="250px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkItem" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Anggota" Text="Anggota" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkMember" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Virtual Account" Text="Virtual Account" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkVirtualAccount" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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
</asp:Content>
