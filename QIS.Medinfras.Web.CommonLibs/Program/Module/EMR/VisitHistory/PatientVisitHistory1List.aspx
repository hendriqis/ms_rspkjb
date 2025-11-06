<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientVisitHistory1List.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientVisitHistory1List" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientHistoryEpisodeSummary" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Episode Summary")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnPatientHistoryEpisodeSummary.ClientID %>').live('click', function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryCtl.ascx");
                    openUserControlPopup(url, id, 'Medical Record - Detail', 1100, 700);
                }
            });
        });

        function onRefreshControl(filterExpression) {
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
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var id = $('#<%=hdnID.ClientID %>').val();
            if (code == 'MR000007') {
                filterExpression.text = "VisitID = " + id;
            }
            else {
                filterExpression.text = id;
            }
            return true;
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="No Pendaftaran" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-Width="450px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Kunjungan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="color:blue"><%=GetLabel("Masuk : ")%><%#: Eval("VisitDateInString")%>, <%#: Eval("VisitTime")%></div>
                                        <div style="color:red"><%=GetLabel("Pulang : ")%><%#: Eval("cfDischargedInfo")%></div>
                                        <div><%#: Eval("ServiceUnitName")%> - <%#: Eval("ParamedicName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DisplayPatientDiagnosis" HeaderText="Diagnosis" HeaderStyle-HorizontalAlign="Left" />   
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Patient Medical History To Display")%>
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
