<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPhysicianInstructionCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoPhysicianInstructionCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

    <script type="text/javascript" id="dxss_infoPhysicianInstructionctl">
        //#region Paging
        var pageCount1 = parseInt('<%=PageInstructionCount %>');
        setPaging($("#pagingPopup"), pageCount1, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });

        function onCbpPopupViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#pagingPopup"), pageCount, function (page) {
                    cbpPopupView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion
    </script>

    <input type="hidden" value="" id="hdnVisitID" runat="server" />

    <div style="position: relative;">    
        <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
            ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
            <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlInstructionView" CssClass="pnlContainerGrid" Style="height:500px">
                        <asp:GridView ID="grdInstructionView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="PatientInstructionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfInstructionDate" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="InstructionTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InstructionGroup" HeaderText="Tipe Instruksi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PhysicianName" HeaderText="Dokter" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="Instruksi" HeaderStyle-CssClass="description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="description" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada informasi instruksi dokter")%>
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
                <div id="pagingPopup"></div>
            </div>
        </div> 
    </div>