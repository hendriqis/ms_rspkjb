<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="NursingDiagnoseList.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingDiagnoseList" %>

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

            $('.lnkIntervention a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                openMatrixControl('NursingDiagnoseIntervention', id, 'Intervensi Diagnosa Keperawatan');
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

        $('.lnkNursingDiagnoseItem').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var type = $(this).closest('tr').find('.nursingDiagnosisType').html();
            var param = id + '|' + type;
            var url = ResolveUrl("~/Program/Master/NursingDiagnose/NursingDiagnoseItemEntryCtl.ascx");
            openUserControlPopup(url, param, 'Detail Diagnosa Keperawatan', 1100, 600);
        });
    </script>
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnID" runat="server" />
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
                                <asp:BoundField DataField="NurseDiagnoseID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="GCNursingDiagnosisType" HeaderStyle-CssClass="nursingDiagnosisType hiddenColumn" ItemStyle-CssClass="nursingDiagnosisType hiddenColumn" />
                                <asp:BoundField DataField="NurseDiagnoseCode" HeaderText="Kode Diagnosis" HeaderStyle-Width="40px" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText"/>
                                <asp:BoundField DataField="NurseDiagnoseName" HeaderText="Nama Diagnosis" HeaderStyle-Width="560px" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText"/>
                                <asp:HyperLinkField HeaderText="Intervensi" Text="Intervensi" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkIntervention" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField Text="Detail" ItemStyle-CssClass="lnkNursingDiagnoseItem" HeaderText="Komponen" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Data To Display
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
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>