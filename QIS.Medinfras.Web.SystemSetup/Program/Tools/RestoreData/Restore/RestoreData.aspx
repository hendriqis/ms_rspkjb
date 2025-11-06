<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="RestoreData.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Tools.RestoreData" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnMPListRestore.ClientID %>').click(function () {
                if ($('#<%=hdnID.ClientID %>').val() != '') {
                    cbpRestore.PerformCallback();
                }
            });

            $('#<%=btnMPListBack.ClientID %>').click(function () {
                document.location = ResolveUrl("~/Libs/Tools/RestoreData/Restore/RestoreDataList.aspx");
            });
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
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
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpRestoreEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'success')
                cbpView.PerformCallback('refresh');
            else
                alert(param[1]);
        }
        
    </script>
    <div class="toolbarArea">
        <table style="float:right;margin-top:20px;" id="tblFilter" runat="server">
            <tr>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="300px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <ul>
            <li id="btnMPListRestore" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Restore")%></div></li>
            <li id="btnMPListBack" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Back")%></div></li>
        </ul>
    </div>
    
    <input type="hidden" value="" id="hdnHeaderID" runat="server" />
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

    <dxcp:ASPxCallbackPanel ID="cbpRestore" runat="server" Width="100%" ClientInstanceName="cbpRestore"
        ShowLoadingPanel="false" OnCallback="cbpRestore_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpRestoreEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
