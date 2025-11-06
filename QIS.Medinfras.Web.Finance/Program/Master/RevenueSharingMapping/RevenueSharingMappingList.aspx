<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingMappingList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingMappingList" %>

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

        $('.lnkUnitPelayanan a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingHealthcareServiceUnit', id, 'Unit Pelayanan');
        });

        $('.lnkStatusDokter a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingEmploymentStatus', id, 'Status Dokter');
        });

        $('.lnkDokter a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingParamedicMaster', id, 'Dokter/Paramedis');
        });

        $('.lnkHariJam a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/RevenueSharingMapping/RevenueSharingOperationalTimeCtl.ascx");
            openUserControlPopup(url, id, 'RevenueSharingOperationalTime', 900, 500);
        });

        $('.lnkTipeInstansi a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingCustomerType', id, 'TipeInstansi');
        });

        $('.lnkKelompokItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingItemGroupMaster', id, 'Kelompok Item');
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('RevenueSharingItemMaster', id, 'Item');
        });

        $('.lnkPatientOwnerStatus').live('click', function () {
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
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="RevenueSharingID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="RevenueSharingCode" HeaderText="Kode Jasa Medis" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="RevenueSharingName" HeaderText="Nama Jasa Medis" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:HyperLinkField HeaderText="Unit Pelayanan" Text="Unit Pelayanan" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkUnitPelayanan" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Status Dokter" Text="Status Dokter" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkStatusDokter" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Dokter/Paramedis" Text="Dokter/Paramedis" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkDokter" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Hari & Jam" Text="Hari & Jam" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkHariJam" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Tipe Instansi" Text="Tipe Instansi" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkTipeInstansi" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Kelompok Item" Text="Kelompok Item" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkKelompokItem" HeaderStyle-Width="120px" />
                                    <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkItem" HeaderStyle-Width="120px" /> 
                                    <asp:HyperLinkField HeaderText="Status Pasien" Text="Status Pasien" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-CssClass="lnkPatientOwnerStatus" HeaderStyle-Width="120px" />                                    
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Data Tidak Tersedia")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
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
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
