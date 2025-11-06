<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="PatientObsygnHistoryList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientObsygnHistoryList"
    EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Detail Tab
            $('#ulDetail li').click(function () {
                $('#ulDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion
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

        //region tab detail
        function onCbpViewDetailEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdViewDetail.ClientID %> tr:eq(1)').click();
            }
            else
                $('#<%=grdViewDetail.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="containerUlTabPage" style="margin-bottom: 3px;">
                    <ul class="ulTabPage" id="ulDetail">
                        <li class="selected" contentid="panPregancyHistory">
                            <%=GetLabel("Riwayat Kehamilan")%></li>
                        <li contentid="panObgynNotes">
                            <%=GetLabel("Catatan Kebidanan")%></li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr style="height: 550px">
            <td valign="top">
                <div class="containerOrderDt" id="panPregancyHistory" style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 510px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PregnancyNo" HeaderText="Hamil Ke-" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PregnancyDuration" HeaderText="Usia Kehamilan (Minggu)"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BirthMethod" HeaderText="Jenis Persalinan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CaesarMethod" HeaderText="Tipe Caesar" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BornCondition" HeaderText="Kondisi Persalinan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ParamedicType" HeaderText="Penolong Persalinan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Weight" HeaderText="BBL (gram)" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Length" HeaderText="PBL (cm)" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Dientry Oleh" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="LastUpdatedByName" HeaderText="Terakhir Update" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div>
                                                <div>
                                                    <%=GetLabel("Belum ada informasi riwayat persalinan di pasien ini") %></div>
                                            </div>
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
                <div class="containerOrderDt" id="panObgynNotes" style='display: none'>
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 510px">
                                    <asp:GridView ID="grdViewDetail" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdViewDetail_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfVisitDate" HeaderText="Tanggal" HeaderStyle-Width="40px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PregancyAge" HeaderText="Usia Kehamilan (Minggu)" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BloodPreasure" HeaderText="Tekanan Darah" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PatientWeight" HeaderText="Berat Badan (Kg)" HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ChiefComplaintText" HeaderText="Data Pemeriksaan Klinis"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col style="width: 60%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%=GetLabel("Data Pemeriksaan Penunjang") %></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="font-style: italic">
                                                                    <asp:Repeater ID="rptTestOrderDt" runat="server">
                                                                        <ItemTemplate>
                                                                            <div style="padding-left: 5px;">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <br style="clear: both" />
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col style="width: 60%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%=GetLabel("Diagnosa") %></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="font-style: italic">
                                                                    <asp:Repeater ID="rptDiagnose" runat="server">
                                                                        <ItemTemplate>
                                                                            <div style="padding-left: 5px;">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "DiagnoseType") %>
                                                                                :
                                                                                <%#: DataBinder.Eval(Container.DataItem, "DiagnosisText") %>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <br style="clear: both" />
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col style="width: 60%" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%=GetLabel("Terapi") %></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="font-style: italic">
                                                                    <asp:Repeater ID="rptPrescription" runat="server">
                                                                        <ItemTemplate>
                                                                            <div style="padding-left: 5px;">
                                                                                <%#: DataBinder.Eval(Container.DataItem, "ItemName1") %>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <br style="clear: both" />
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div>
                                                <div>
                                                    <%=GetLabel("Belum ada catatan kebidanan yang aktif di pasien ini") %></div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
