<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="UserRolesList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.UserRolesList" %>

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

        $('.lnkMember a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixWithParameterControl('UserRoleSelectUser', id, 'User');
            //var url = ResolveUrl("~/Program/UserRoles/UserInRoleEntryCtl.ascx");
            //openUserControlPopup(url, id, 'User', 1000, 500);
        });

        $('.lnkServiceUnit a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/ControlPanel/UserRoles/UserRolesServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'Service Unit', 900, 500);
        });

        $('.lnkLocation a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/ControlPanel/UserRoles/UserRolesLocationEntryCtl.ascx");
            openUserControlPopup(url, id, 'Location', 900, 550);
        });

        $('.lnkMenuAccess a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/ControlPanel/UserRoles/UserRolesMenuAccessEntryCtl.ascx");
            openUserControlPopup(url, id, 'Menu Access', 1000, 500);
        });

        $('.lnkCOA a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('UserRoleCOA', id, 'User Role COA');
        });

        $('.lnkCOATreasury a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('UserRoleCOATreasury', id, 'User Role COA Treasury');
        });

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
                                <asp:BoundField DataField="RoleID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RoleName" HeaderText="Nama Profil" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LoweredRoleName" HeaderText="Nama Profil (Huruf Kecil)" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="Description" HeaderText="Deskripsi" HeaderStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText="Anggota" Text="Anggota" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkMember" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Unit Pelayanan" Text="Unit Pelayanan" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkServiceUnit" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Lokasi" Text="Lokasi" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkLocation" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Akses Menu" Text="Akses Menu" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkMenuAccess" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="COA" Text="COA" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkCOA" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="COA Treasury" Text="COA Treasury" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkCOATreasury" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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