<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryDtTestOrderCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryDtTestOrderCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_testorderctl">
    $(function () {
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').die('click');
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt.PerformCallback('refresh');
            }
        });
    });

    function onInit() {
        $('#<%=grdView.ClientID %> tr:eq(1)').click();        
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        $('#containerImgLoadingView').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    //#region Paging Dt
    function onCbpViewDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingDt"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>
<input type="hidden" value="" id="hdnID" runat="server" />
<table style="width:100%">
    <colgroup>
        <col style="width:40%"/>
        <col style="width:60%"/>
    </colgroup>
    <tr>
        <td>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>Order Date - Time | Ordered By</div>
                                                <div>Test Order No. | Status</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div><%#: Eval("TestOrderDateTimeInString")%> | <%#: Eval("ParamedicName") %></div>
                                                <div><%#: Eval("TestOrderNo")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Data To Display
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
        </td>
        <td>
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                    <ClientSideEvents Init="function(s,e) { onInit(); }"
                        BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>Test Item Name</div>
                                                <div>Diagnose Name | To Be Performed | Status</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div><%#: Eval("ItemName1") %></div>
                                                <div><%#: Eval("DiagnoseName")%> | <%#: Eval("cfToBePerformed")%> | <%#: Eval("TestOrderStatus")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Data To Display
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDt"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>