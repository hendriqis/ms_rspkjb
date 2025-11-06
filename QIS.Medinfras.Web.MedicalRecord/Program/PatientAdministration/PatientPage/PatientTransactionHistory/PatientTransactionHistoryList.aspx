<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientDataPageList.master"
    AutoEventWireup="true" CodeBehind="PatientTransactionHistoryList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientTransactionHistoryList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientHistoryTransactionDetail" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Transaction Detail")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnPatientHistoryTransactionDetail.ClientID %>').live('click', function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientTransactionHistory/PatientTransactionHistoryDtCtl.ascx");
                    openUserControlPopup(url, id, 'Transaction History - Detail', 1200, 600);
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

        function onBeforeLoadRightPanelContent(code) {
            var id = $('#<%=hdnID.ClientID %>').val();
            if (code == 'infoPayment') {
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
                                <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="No Pendaftaran" HeaderStyle-Width="150px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-Width="450px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Kunjungan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="color: blue">
                                            <%=GetLabel("Masuk : ")%><%#: Eval("VisitDateInString")%>,
                                            <%#: Eval("VisitTime")%></div>
                                        <div style="color: red">
                                            <%=GetLabel("Pulang : ")%><%#: Eval("cfDischargedInfo")%></div>
                                        <div>
                                            <%#: Eval("ServiceUnitName")%>
                                            -
                                            <%#: Eval("ParamedicName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Keuangan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cCoverage")%></div>
                                        <div>
                                            <%#: Eval("cPayerCoverage")%></div>
                                        <div>
                                            <%#: Eval("cPatientARAmount")%></div>
                                        <div>
                                            <%#: Eval("cPayment")%></div>
                                        <div>
                                            <%=GetLabel("Total Amount : ")%><%#: Eval("TotalLineAmount")%></div>
                                        <div>
                                            <%=GetLabel("Remaining Amount : ")%><%#: Eval("RemainingAmount")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Diagnosa")%>
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
                                <asp:BoundField DataField="VisitStatus" HeaderText="Status" HeaderStyle-Width="120px"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" />
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
