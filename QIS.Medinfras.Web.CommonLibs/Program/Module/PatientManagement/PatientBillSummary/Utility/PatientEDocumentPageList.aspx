<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientEDocumentPageList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientEDocumentPageList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnScanDocument" runat="server" crudmode="C" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Scan Document")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnScanDocument.ClientID %>').click(function () {
                cbpScanDocument.PerformCallback('verify');
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
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
            $('#containerImgLoadingView').hide();

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

        $('.lnkOpenDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });

        function onCbpOpenDocumentEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[1] != 'success') {
                showToast('Open Document Failed', 'Error Message : ' + param[2]);
            }
        }

        function onCbpScanDocumentEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[1] != 'success') {
                showToast('Scan Document Failed', 'Error Message : ' + param[2]);
            }
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnPort" runat="server" value="6000" />
    <input type="hidden" id="hdnPatientDocumentUrl" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fileName"  />
                                <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkOpenDocument" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No patient document record available in this episode") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <dxcp:ASPxCallbackPanel ID="cbpOpenDocument" runat="server" Width="100%" ClientInstanceName="cbpOpenDocument"
            ShowLoadingPanel="false" OnCallback="cbpOpenDocument_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpOpenDocumentEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpScanDocument" runat="server" Width="100%" ClientInstanceName="cbpScanDocument"
            ShowLoadingPanel="false" OnCallback="cbpScanDocument_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpScanDocumentEndCallback(s); }" />
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
