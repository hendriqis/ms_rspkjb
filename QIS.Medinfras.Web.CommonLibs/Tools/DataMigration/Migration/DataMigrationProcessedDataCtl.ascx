<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataMigrationProcessedDataCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Tools.DataMigrationProcessedDataCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_datamigrationprocesseddata">
    //#region Paging
    var pageCountPopup = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCountPopup, function (page) {
            cbpViewPopup.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCountPopup = parseInt(param[1]);
            if (pageCountPopup > 0)
                $('#<%=grdViewPopup.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCountPopup, function (page) {
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdViewPopup.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onCboMigrationStatusValueChanged(s) {
        cbpViewPopup.PerformCallback('refresh');
    }
</script>

<div class="toolbarArea" style="height:50px">
    <table style="float:right;margin-top:20px;" id="tblFilter" runat="server">
        <tr>
            <td>
                <dxe:ASPxComboBox ID="cboMigrationStatus" runat="server" Width="200px">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboMigrationStatusValueChanged(s); }" />
                    <Items>
                        <dxe:ListEditItem Text="All" Value="-1" />
                        <dxe:ListEditItem Text="Transferred" Value="1" />
                        <dxe:ListEditItem Text="Trashed" Value="2" />
                        <dxe:ListEditItem Text="Failed" Value="3" />
                    </Items>
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <br />
</div>

<div style="position: relative;">
    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
            EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                    <asp:GridView ID="grdViewPopup" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">                            
                        <EmptyDataTemplate>
                            <%=GetLabel("No Data To Display")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingPopup"></div>
        </div>
    </div> 
</div>