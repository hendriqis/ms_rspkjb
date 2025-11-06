<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="PrinterLocationPerIPList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.PrinterLocationPerIPList" %>
    
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:content id="Content1" contentplaceholderid="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onChangeCboIPAddress() {
            var ipAddress = cboIPAddress.GetText();
            $('#<%=hdnIPAddress.ClientID %>').val(ipAddress);

            cbpView.PerformCallback('refresh');
        }

        //
        function onChangeCboDeviceType() {
            var DeviceType = cboDeviceType.GetValue();
            $('#<%=hdnGCPrinterType.ClientID %>').val(DeviceType);
          
            cbpView.PerformCallback('refresh');
        }
        //

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
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnIPAddress" runat="server" />
    <input type="hidden" value="" id="hdnGCPrinterType" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table width="50%">
            <colgroup>
                <col width="25%" />
            </colgroup>
            <tr>
                <td class="tdLabel"><label><%=GetLabel("Device Type")%></label></td>
                <td>
                    <dx:ASPxComboBox ID="cboDeviceType" runat="server" Width="250px" ClientInstanceName="cboDeviceType">
                        <%--ClientSideEvents SelectedIndexChanged="onChangeCboDeviceType" />--%>
                        <ClientSideEvents ValueChanged="function(s,e) { onChangeCboDeviceType(); }" />
                    </dx:ASPxComboBox>
                </td>
            </tr>
            
            <tr>
                <td class="tdLabel"><label><%=GetLabel("IP Address")%></label></td>
                <td>
                    <dx:ASPxComboBox ID="cboIPAddress" runat="server" Width="250px" ClientInstanceName="cboIPAddress">
                        <ClientSideEvents SelectedIndexChanged="onChangeCboIPAddress" />
                    </dx:ASPxComboBox>
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="IPAddress" HeaderText="IP Address" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="PrinterType" HeaderText="Device Type" HeaderStyle-Width="200px" />
                                <asp:BoundField DataField="PrinterName" HeaderText="Printer Name" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data tidak tersedia")%>
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
</asp:content>
