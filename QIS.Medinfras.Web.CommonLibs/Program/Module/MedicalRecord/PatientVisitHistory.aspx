<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPPatientDataPageList.master"
    AutoEventWireup="true" CodeBehind="PatientVisitHistory.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientMRView" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientHistoryPrint" runat="server" crudmode="R" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print")%></div>
    </li>
<%--    <li id="btnPatientHistoryEpisodeSummary" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Episode Summary")%></div>
    </li>--%>
    <li id="btnViewEpisodeSummary" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Riwayat Kunjungan 2")%></div>
    </li>
    <li id="btnPatientHistoryTransaction" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("Transaksi")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>'
        type='text/javascript'></script>
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>'
        type='text/javascript'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                }
            });

            $('#<%=btnViewEpisodeSummary.ClientID %>').live('click', function () {

                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryCtl.ascx");
                    openUserControlPopup(url, id, 'Ringkasan Perawatan', 1300, 600);
                }
            });

<%-- //            $('#<%=btnPatientHistoryEpisodeSummary.ClientID %>').live('click', function () {
//                var id = $('#<%=hdnID.ClientID %>').val();
//                if (id != '') {
//                    var url = ResolveUrl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryCtl.ascx");
//                    openUserControlPopup(url, id, 'Medical Record - Detail', 1300, 600);
//                }
//            });
--%>

            $('#<%=btnPatientHistoryPrint.ClientID %>').live('click', function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var reportCode = '<%=OnGetEpisodeSummaryReportCode() %>';
                    openReportViewer(reportCode, id);
                }
            });

            $('#<%=btnPatientHistoryTransaction.ClientID %>').live('click', function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Libs/Controls/PatientPage/VisitTransactionHistoryDtCtl.ascx");
                    openUserControlPopup(url, id, 'Transaction - Detail', 1200, 600);
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
            if (code == 'MR000019' || code == 'MR000024') {
                if (id == '' || id == '0') {
                    errMessage.text = 'Tidak Ada Riwayat Kunjungan';
                    return false;
                } else {
                    filterExpression.text = id;
                    return true;
                }

            }
            else if (code == 'PM-00546' || code == 'PM-00523') {
                var filter = 'VisitID = ' + id;
                Methods.getObject('GetvRegistration5List', filter, function (result) {
                    if (result != null) {
                        filterExpression.text = result.RegistrationID;
                    }
                });
            }
            else {
                filterExpression.text = "VisitID = " + id;
            }
            return true;
        }

        function onBeforeLoadRightPanelContent(code) {
            var id = $('#<%=hdnID.ClientID %>').val();
            if (code == 'SuratKeranganDirawat') {
                var filterExpression = '';
                var filter = 'VisitID = ' + id;
                Methods.getObject('GetvRegistration5List', filter, function (result) {
                    if (result != null) {
                        filterExpression = result.RegistrationID;
                    }
                });

                return filterExpression;
            }
            else if (code == 'keteranganIstirahat1') {
                return id;
            }
            else {
                var filter = 'VisitID = ' + id;
                Methods.getObject('GetvRegistration5List', filter, function (result) {
                    if (result != null) {
                        return result.RegistrationID;
                    }
                });
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitDateInString" HeaderText="Date" HeaderStyle-Width="50px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="VisitTime" HeaderText="Time" HeaderStyle-Width="30px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No." HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Service Unit")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("ServiceUnitName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ParamedicName" HeaderText="Physician" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnosis")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><i><%=GetLabel("oleh Dokter : ")%></i></div>
                                        <div><%#: Eval("cfDisplayDiagnosisDate")%></div>
                                        <div><%#: Eval("cfDisplayPatientDiagnosisText")%></div><br />
                                        <div><i><%=GetLabel("oleh Rekam Medis : ")%></i></div>
                                        <div><%#: Eval("cfDisplayFinalDiagnosisDate")%></div>
                                        <div><%#: Eval("cfDisplayPatientDiagnosisFinalText")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VisitStatus" HeaderText="Status" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Patient Medical History To Display")%>
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
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
