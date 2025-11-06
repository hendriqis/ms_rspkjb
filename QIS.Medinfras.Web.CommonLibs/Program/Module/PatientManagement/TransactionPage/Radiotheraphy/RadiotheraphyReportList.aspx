<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="RadiotheraphyReportList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.RadiotheraphyReportList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientallergylist">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').live('click', function () {
                cbpView.PerformCallback('refresh');
            });
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnProgramID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnProgramStatus.ClientID %>').val($(this).find('.programStatus').html());
                $('#<%=hdnTotalFraction.ClientID %>').val($(this).find('.totalFraction').html());
                if ($('#<%=hdnProgramID.ClientID %>').val() != "") {
                    cbpViewDt1.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion
        });

        $('#<%=grdViewDt1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnProgramLogID.ClientID %>').val($(this).find('.keyField').html());
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshReportGrid() {
            cbpViewDt1.PerformCallback('refresh');
        }

        //#region View Program
        $('.imgViewProgram.imgLink').die('click');
        $('.imgViewProgram.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewRTProgramCtl.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
            openUserControlPopup(url, param, "Program Radiasi Eksterna", 800, 500);
        });
        //#endregion

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

        function onCbpViewDt1EndCallback(s) {
            $('#containerImgLoadingViewDt1').hide();

            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();
        }

        function onBeforeEditDelete(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, data hanya bisa diubah/dihapus oleh user yang membuat.');
                return false;
            }
        }

        $('#lblAddReport').die('click');
        $('#lblAddReport').live('click', function (evt) {
            addReport();
        });

        function addReport() {
            if (($('#<%=hdnProgramStatus.ClientID %>').val() != "X121^001") && $('#<%=hdnProgramStatus.ClientID %>').val() != "X121^999") {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ProgramReportEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                openUserControlPopup(url, param, "Laporan Selesai Radiasi", 700, 500, "");
            }
            else {
                displayErrorMessageBox("Laporan Selesai Radiasi", "Laporan Selesai Radiasi tidak bisa dilakukan karena Status Program masih OPEN atau sudah DIBATALKAN.");
            }
        }

        $('.imgEditReport.imgLink').die('click');
        $('.imgEditReport.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ProgramReportEntryCtl.ascx");
                var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                openUserControlPopup(url, param, "Laporan Selesai Radiasi", 700, 500, "");
            }
        });

        $('.imgDeleteReport.imgLink').die('click');
        $('.imgDeleteReport.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            if (onBeforeEditDelete(paramedicID)) {
                var message = "Hapus informasi laporan selesai radiasi untuk pasien ini ?";
                displayConfirmationMessageBox("Laporan Selesai Radiasi", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpDeleteReport.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgCopyRadiotheraphyLog.imgLink').die('click');
        $('.imgCopyRadiotheraphyLog').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var message = "Lakukan copy log penyinaran ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramLogEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + recordID;
                    openUserControlPopup(url, param, "Penyinaran Onkologi Radiasi", 700, 500, "");
                }
            });
        });


        function oncbpDeleteReportEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Penyinaran Onkologi Radiasi', param[1]);
            }
        }
        //#endregion

        function onBeforeEditDelete(paramedicID) {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT/DELETE', 'Maaf, pengkajian hanya bisa diubah/dihapus oleh user yang mengkaji.');
                return false;
            }
        }
    </script>

    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnProgramID" runat="server" />
    <input type="hidden" value="" id="hdnProgramStatus" runat="server" />
    <input type="hidden" value="" id="hdnTotalFraction" runat="server" />
    <input type="hidden" value="" id="hdnProgramLogID" runat="server" />
    <input type="hidden" value="0" id="hdnPageVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPageMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnSubMenuType" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <input type="hidden" value="" id="hdnSimulationRequestID" runat="server" />

    <table style="width: 100%">
        <colgroup>
            <col style="width: 26%" />
            <col style="width: 74%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ProgramID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="GCProgramStatus" HeaderStyle-CssClass="hiddenColumn programStatus" ItemStyle-CssClass="hiddenColumn programStatus" />
                                            <asp:BoundField DataField="cfTotalFraction" HeaderStyle-CssClass="hiddenColumn totalFraction" ItemStyle-CssClass="hiddenColumn totalFraction" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgViewProgram imgLink" title='<%=GetLabel("Lihat Program")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ProgramID") %>"
                                                        visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" />                                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfProgramDate" HeaderText="Tanggal" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:TemplateField HeaderText="Tipe Radioterapi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-weight:bold"><%#Eval("TherapyType")%></span>
                                                    </div>
                                                    <div style="<%# Eval("GCTherapyType").ToString() != "X582^001" ? "display:none": "" %>">
                                                        <span style="font-style:italic">Teknik : </span><span style="font-weight:bold"><%#Eval("cfRadiotherapyBeamTechnique")%></span>
                                                    </div>
                                                    <div style="<%# Eval("GCTherapyType").ToString() != "X582^001" ? "display:none": "" %>">
                                                        <span style="font-style:italic">Verifikasi : </span><span style="font-weight:bold"><%#Eval("VerificationType")%></span>
                                                    </div>
                                                    <div style="<%# Eval("GCTherapyType").ToString() != "X582^001" ? "display:none": "" %>">
                                                        <span style="font-style:italic">Dosis : </span><br />
                                                        <span style="font-weight:bold"><%#Eval("cfDosageInfo")%></span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Status" DataField="ProgramStatus" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="programStatus" />
                                            <asp:TemplateField HeaderText="Catatan Tambahan" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <div style="height: auto; max-height:150px; overflow-y: auto;">
                                                        <%#Eval("Remarks").ToString().Replace("\n","<br />")%><br />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada program radioterapi untuk pasien ini")%>
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
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr style="display:none">
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li contentid="programReport" class="selected">
                                        <%=GetLabel("Laporan")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <div class="containerOrderDt" id="programReport">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <div id="preVitalSign">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContentLog" runat="server">
                                                            <asp:Panel runat="server" ID="PanelLog" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-Width="100px">
                                                                            <HeaderTemplate>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                    <img class="imgEditReport imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" recordID="<%#:Eval("ProgramReportID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img class="imgDeleteReport imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordID="<%#:Eval("ProgramReportID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ProgramReportID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="cfReportDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="ReportTime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="ProgramDuration" HeaderText="Lama Program" HeaderStyle-Width="60px"
                                                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:TemplateField HeaderText="Ringkasan Program" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <div style="height: auto; max-height:150px; overflow-y: auto;">
                                                                                    <%#Eval("ProgramSummary").ToString().Replace("\n","<br />")%><br />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Kondisi Klinis setelah Radiasi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <div style="height: auto; max-height:150px; overflow-y: auto;">
                                                                                    <%#Eval("PostProgramMedicalSummary").ToString().Replace("\n","<br />")%><br />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Rencana Tindak Lanjut" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <div style="height: auto; max-height:150px; overflow-y: auto;">
                                                                                    <%#Eval("FollowupSummary").ToString().Replace("\n","<br />")%><br />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div>
                                                                                <%=GetLabel("Belum ada informasi laporan selesai radiasi untuk program ini") %></div>
                                                                            <br />
                                                                            <span class="lblLink" id="lblAddReport">
                                                                                <%= GetLabel("+ Laporan Selesai Radiasi")%></span>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt1">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingDt1">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteReport" runat="server" Width="100%" ClientInstanceName="cbpDeleteReport"
            ShowLoadingPanel="false" OnCallback="cbpDeleteReport_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteReportEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
