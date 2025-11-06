<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="TemplateChargesList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplateChargesList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnTemplateID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnTemplateID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onCbpViewEndCallback() {
            hideLoadingPanel();
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('.lnkItemService a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html() + '|service';
            var url = ResolveUrl('~/Libs/Program/Master/TemplateCharges/TemplateChargesEntryItemCtl.ascx');
            var width = 1000;
            openUserControlPopup(url, id, 'Template Charges Panel Detail (Service)', width, 600);
        });

        $('.lnkItemDrug a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html() + '|drug';
            var url = ResolveUrl('~/Libs/Program/Master/TemplateCharges/TemplateChargesEntryItemCtl.ascx');
            var width = 1000;
            openUserControlPopup(url, id, 'Template Charges Panel Detail (Drug)', width, 600);
        });

        $('.lnkItemLogistic a').live('click', function () {            
            var id = $(this).closest('tr').find('.keyField').html() + '|logistic';
            var url = ResolveUrl('~/Libs/Program/Master/TemplateCharges/TemplateChargesEntryItemCtl.ascx');
            var width = 1000;
            openUserControlPopup(url, id, 'Template Charges Panel Detail (Logistic)', width, 600);
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
                cbpView.PerformCallback('changepage|' + page);
            }, 25);
        }
        //#endregion 

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            hideLoadingPanel();
        }
        
    </script>
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnTemplateID" runat="server" />
    <input type="hidden" value="" id="hdnHSUIDImaging" runat="server" />
    <input type="hidden" value="" id="hdnHSUIDLaboratory" runat="server" />
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
                                <asp:BoundField DataField="ChargesTemplateID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="ChargesTemplateCode" HeaderText="Kode Template" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ChargesTemplateName" HeaderText="Nama Template" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText="Pelayanan" Text="Item" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkItemService" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Obat Obatan" Text="Item" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkItemDrug" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Barang Umum" Text="Item" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkItemLogistic" HeaderStyle-Width="100px" />
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
                <div id="pagingPopup">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
