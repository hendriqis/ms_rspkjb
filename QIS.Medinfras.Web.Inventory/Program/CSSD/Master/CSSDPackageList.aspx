<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="CSSDPackageList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDPackageList" %>

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

        $('.lnkDetail a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/CSSD/Master/CSSDPackageDetailEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item', 1200, 550);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="PackageID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="PackageCode" HeaderText="Package Code" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Created Information") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#:Eval("CreatedByName")%>
                                        <br />
                                        (<%#:Eval("cfCreatedDateInStringDDTF")%>)
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Last Updated Information") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#:Eval("LastUpdatedByName")%>
                                        <br />
                                        (<%#:Eval("cfLastUpdatedDateInStringDDTF")%>)
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField HeaderText="Detail" Text="Detail" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkDetail" HeaderStyle-Width="120px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data to Display.")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>