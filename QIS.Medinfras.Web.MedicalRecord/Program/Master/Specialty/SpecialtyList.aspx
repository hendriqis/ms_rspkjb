<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="SpecialtyList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.SpecialtyList" %>

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

        $('.lnkVitalSign a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('SpecialtyVitalSign', id, 'Specialty Vital Sign');
        });

        $('.lnkProcedure a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('SpecialtyProcedure', id, 'Specialty Procedure');
        });

        $('.lnkROS a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('SpecialtyROS', id, 'Specialty - Review of System');
        });

        $('.lnkBodyDiagram a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('SpecialtyBodyDiagram', id, 'Specialty Body Diagram');
        });
    </script>
 <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
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
                                <asp:BoundField DataField="SpecialtyID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="SpecialtyID" HeaderText="Kode Spesialisasi" HeaderStyle-Width="100px" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                <asp:BoundField DataField="SpecialtyName" HeaderText="Nama Spesialisasi" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                <asp:HyperLinkField HeaderText="Tanda Vital" Text="Tanda Vital" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkVitalSign" HeaderStyle-Width="150px" />
                                <asp:HyperLinkField HeaderText="Tindakan" Text="Tindakan" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkProcedure" HeaderStyle-Width="150px" />
                                <asp:HyperLinkField HeaderText="Pemeriksaan Fisik" Text="Pemeriksaan Fisik" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkROS" HeaderStyle-Width="150px" />
                                <asp:HyperLinkField HeaderText="Body Diagram" Text="Body Diagram" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkBodyDiagram" HeaderStyle-Width="150px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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