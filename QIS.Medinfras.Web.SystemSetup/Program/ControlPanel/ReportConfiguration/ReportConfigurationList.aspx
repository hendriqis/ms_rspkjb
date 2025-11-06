<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ReportConfigurationList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ReportConfigurationList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
            cboModuleName.SetValue(null);
        });

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onChangeModuleName() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
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

        $('.lnkFilterParameter a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/ControlPanel/ReportConfiguration/ReportParameterEntryCtl.ascx");
            openUserControlPopup(url, id, 'Report Parameter', 600, 500);
        });
        $('.lnkGenerateReport a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            cbpGenerateSingleReport.PerformCallback('single|'+id);
        });
        function onGenerateAllRpt() {
            cbpGenerateSingleReport.PerformCallback('all');
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table width="50%">
            <colgroup>
                <col width="30%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Module")%></label>
                </td>
                <td>                    
                    <dx:ASPxComboBox ID="cboModuleName" runat="server" Width="300px" ClientInstanceName="cboModuleName">
                        <ClientSideEvents SelectedIndexChanged="onChangeModuleName" />
                    </dx:ASPxComboBox>                    
                </td>
                <td>
                    <dx:ASPxHyperLink ID="lnkGenerateAllRpt" runat="server" Text="Generate All Report" ClientInstanceName="lnkGenerateAllRpt">
                        <ClientSideEvents Click="onGenerateAllRpt"/>
                    </dx:ASPxHyperLink>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ReportID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="ReportCode" HeaderText="Report Code" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="ModuleName" HeaderText="Module Name" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="ReportTitle1" HeaderText="Report Title" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="ReportTitle2" HeaderText="Alternate Title" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="ReportType" HeaderText="Report Type" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:HyperLinkField HeaderText="Filter Parameter" Text="Filter Parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkFilterParameter" HeaderStyle-Width="120px" />
                                <asp:HyperLinkField HeaderText="Generate Report" Text="Generate Report" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkGenerateReport" HeaderStyle-Width="120px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <dxcp:ASPxCallbackPanel ID="cbpGenerateSingleReport" runat="server" ClientInstanceName="cbpGenerateSingleReport"
            ShowLoadingPanel="false" OnCallback="generateSingleReport_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ hideLoadingPanel(); }" />
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