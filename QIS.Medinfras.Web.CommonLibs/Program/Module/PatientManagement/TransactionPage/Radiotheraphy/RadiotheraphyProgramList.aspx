<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="RadiotheraphyProgramList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.RadiotheraphyProgramList" %>

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
    <uc1:toolbarctl id="ctlToolbar" runat="server" />
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
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                $('#<%=hdnTotalFraction.ClientID %>').val($(this).find('.totalFraction').html());
                if ($('#<%=hdnProgramID.ClientID %>').val() != "") {
                    cbpViewDt1.PerformCallback('refresh');
                    setTimeout(function () { cbpViewDt2.PerformCallback('refresh'); }, 1000);
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

        function onRefreshLogGrid() {
            cbpViewDt1.PerformCallback('refresh');
        }

        function onRefreshQAGrid() {
            cbpViewDt2.PerformCallback('refresh');
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


        $('.imgAddRadiotheraphyLog.imgLink').die('click');
        $('.imgAddRadiotheraphyLog.imgLink').live('click', function (evt) {
            addRadiotheraphyLog();
        });

        $('#lblAddProgramLog').die('click');
        $('#lblAddProgramLog').live('click', function (evt) {
            addRadiotheraphyLog();
        });

        function addRadiotheraphyLog() {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                if (($('#<%=hdnProgramStatus.ClientID %>').val() != "X121^001") && $('#<%=hdnProgramStatus.ClientID %>').val() != "X121^999") {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramLogEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                    openUserControlPopup(url, param, "Penyinaran Onkologi Radiasi", 700, 500, "");
                }
                else {
                    displayErrorMessageBox("Catatan Log Penyinaran", "Catatan Log Penyinaran tidak bisa dilakukan karena Status Program masih OPEN atau sudah DIBATALKAN.");
                }
            }
            else {
                displayErrorMessageBox("Catatan Log Penyinaran", "Catatan Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");            
            }
        }

        $('.imgEditRadiotheraphyLog.imgLink').die('click');
        $('.imgEditRadiotheraphyLog.imgLink').live('click', function (evt) {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var allow = true;
                if (allow) {
                    var recordID = $(this).attr('recordID');
                    var paramedicID = $(this).attr('paramedicID');
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramLogEntryCtl.ascx");
                    var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
                    openUserControlPopup(url, param, "Penyinaran Onkologi Radiasi", 700, 500, "");
                }
            }
            else {
                displayErrorMessageBox("Catatan Log Penyinaran", "Catatan Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgDeleteRadiotheraphyLog.imgLink').die('click');
        $('.imgDeleteRadiotheraphyLog.imgLink').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi log penyinaran untuk pasien ini ?";
                    displayConfirmationMessageBox("Penyinaran Radiasi Onkologi", message, function (result) {
                        if (result) {
                            var param = recordID;
                            cbpDeleteLog.PerformCallback(param);
                        }
                    });
                }
            }
            else {
                displayErrorMessageBox("Catatan Log Penyinaran", "Catatan Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgCopyRadiotheraphyLog.imgLink').die('click');
        $('.imgCopyRadiotheraphyLog').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var message = "Lakukan copy log penyinaran ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramLogEntryCtl.ascx");
                        var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val() + "|" + recordID;
                        openUserControlPopup(url, param, "Penyinaran Onkologi Radiasi", 700, 500, "");
                    }
                });
            }
            else {
                displayErrorMessageBox("Catatan Log Penyinaran", "Catatan Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgViewLog.imgLink').die('click');
        $('.imgViewLog.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewRadiotherapyLogCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnTotalFraction.ClientID %>').val();
            openUserControlPopup(url, param, "Log Penyinaran", 800, 500, "");
        });

        function onCbpDeleteLogEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Penyinaran Onkologi Radiasi', param[1]);
            }
        }


        $('.imgAddQA.imgLink').die('click');
        $('.imgAddQA.imgLink').live('click', function (evt) {
            addQA();
        });

        $('#lblAddQA').die('click');
        $('#lblAddQA').live('click', function (evt) {
            addQA();
        });

        function addQA() {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                if (($('#<%=hdnProgramStatus.ClientID %>').val() != "X121^001") && $('#<%=hdnProgramStatus.ClientID %>').val() != "X121^999") {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramQAEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val();
                    openUserControlPopup(url, param, "QA Penyinaran", 700, 500, "");
                }
                else {
                    displayErrorMessageBox("Catatan QA Penyinaran", "Catatan QA Penyinaran tidak bisa dilakukan karena Status Program masih OPEN atau sudah DIBATALKAN.");
                }
            }
            else {
                displayErrorMessageBox("Catatan QA Penyinaran", "Catatan QA Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        }

        $('.imgEditQA.imgLink').die('click');
        $('.imgEditQA.imgLink').live('click', function (evt) {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var url = ResolveUrl(ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramQAEntryCtl.ascx"));
                    var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val();
                    openUserControlPopup(url, param, "Catatan QA Penyinaran", 700, 500, "");
                }
            }
            else {
                displayErrorMessageBox("Catatan QA Penyinaran", "Catatan QA Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgDeleteQA.imgLink').die('click');
        $('.imgDeleteQA.imgLink').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi Catatan QA untuk pasien ini ?";
                    displayConfirmationMessageBox("Catatan QA Penyinaran", message, function (result) {
                        if (result) {
                            var param = recordID;
                            cbpDeleteQA.PerformCallback(param);
                        }
                    });
                }
            }
            else {
                displayErrorMessageBox("Catatan QA Penyinaran", "Catatan QA Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgCopyQA.imgLink').die('click');
        $('.imgCopyQA').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var message = "Lakukan copy catatan QA ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RadiotherapyProgramQAEntryCtl.ascx");
                        var param = "0" + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + recordID;
                        openUserControlPopup(url, param, "Catatan QA", 700, 500, "");
                    }
                });
            }
            else {
                displayErrorMessageBox("Catatan QA Penyinaran", "Catatan QA Penyinaran hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgViewQA.imgLink').die('click');
        $('.imgViewQA.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewQACtl.ascx");
            var param = recordID + "|" + $('#<%=hdnProgramID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val();
            openUserControlPopup(url, param, "Catatan QA", 800, 500, "");
        });

        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount2 = parseInt(param[1]);

                if (pageCount2 > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount1, function (page) {
                    cbpViewDt2.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();
        }

        function onCbpDeleteQAEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt2.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Quality Assurance', param[1]);
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

        $('.imgVerification1.imgLink').die('click');
        $('.imgVerification1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            if (onBeforeVerification()) {
                var message = "Lakukan proses verifikasi oleh Dokter ?";
                displayConfirmationMessageBox("Verifikasi Dokter", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpVerification.PerformCallback(param);
                    }
                });
            }
        });

        $('.imgCancelVerification1.imgLink').die('click');
        $('.imgCancelVerification1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            if (onBeforeVerification()) {
                var message = "Lakukan proses batal verifikasi Dokter ?";
                displayConfirmationMessageBox("Batal Verifikasi Dokter", message, function (result) {
                    if (result) {
                        var param = recordID;
                        cbpCancelVerification.PerformCallback(param);
                    }
                });
            }
        });

        function onBeforeVerification() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('Verifikasi Catatan Simulasi', 'Maaf, Verifikasi dan Batal Verifikasi hanya bisa dilakukan oleh Dokter yang mengajukan permintaan simulasi.');
                return false;
            }
        }

        function onCbpVerificationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Verifikasi Dokter', param[1]);
            }
        }

        function onCbpCancelVerificationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Batal Verifikasi Dokter', param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnProgramID" runat="server" />
    <input type="hidden" value="" id="hdnProgramStatus" runat="server" />
    <input type="hidden" value="" id="hdnProgramLogID" runat="server" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
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
    <input type="hidden" runat="server" id="hdnTotalFraction" value="" />
    <input type="hidden" value="0" id="hdnIsEMR" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 26%" />
            <col style="width: 74%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:aspxcallbackpanel id="cbpView" runat="server" width="100%" clientinstancename="cbpView"
                        showloadingpanel="false" oncallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ProgramID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn" />
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
                    </dxcp:aspxcallbackpanel>
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
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li contentid="radiationLog" class="selected">
                                        <%=GetLabel("Log Penyinaran")%></li>
                                    <li contentid="qaLog">
                                        <%=GetLabel("QA Penyinaran")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="radiationLog">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <div id="logDiv">
                                                <dxcp:aspxcallbackpanel id="cbpViewDt1" runat="server" width="100%" clientinstancename="cbpViewDt1"
                                                    showloadingpanel="false" oncallback="cbpViewDt1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContentLog" runat="server">
                                                            <asp:Panel runat="server" ID="PanelLog" CssClass="pnlContainerGridProcessList1">
                                                                <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-Width="180px">
                                                                            <HeaderTemplate>
                                                                                <img class="imgAddRadiotheraphyLog imgLink" title='<%=GetLabel("+ Penyinaran")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                    alt="" recordID="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgEditRadiotheraphyLog imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" recordID="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgDeleteRadiotheraphyLog imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordID="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgCopyRadiotheraphyLog imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                        alt="" recordID="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img class="imgViewLog imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                        alt="" recordid="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                                                        alt="" recordid="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                                                    <img <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %> class="imgCancelVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Batal Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                                                        alt="" recordid="<%#:Eval("RadiotheraphyLogID") %>" programID="<%#:Eval("ProgramID") %>" /> 
                                                                                    <div <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                                        <%#Eval("cfVerifiedInformation")%>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="RadiotheraphyLogID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="FractionNo" HeaderText="Fraksi Ke-" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="Energy" HeaderText="Energi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="GCPesawatDescription" HeaderText="Pesawat" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />
<%--                                                                        <asp:BoundField DataField="GCBeamUnitDescription" HeaderText="Unit Rad" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />--%>
<%--                                                                        <asp:BoundField DataField="GCAccessDescription" HeaderText="Access" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />--%>
<%--                                                                        <asp:BoundField DataField="GCSetupDescription" HeaderText="Setup" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" />--%>
                                                                        <asp:BoundField DataField="VRT" HeaderText="VRT" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="LNG" HeaderText="LNG" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="LAT" HeaderText="LAT" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="ROT" HeaderText="ROT"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                        <asp:BoundField DataField="Duration" HeaderText="Durasi"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                        <asp:BoundField DataField="NumberOfFields" HeaderText="Jumlah Field"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                        <asp:BoundField DataField="MachineUnit" HeaderText="Total MU"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                                        <asp:BoundField DataField="IsoCenter" HeaderText="Isocenter"  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
<%--                                                                        <asp:BoundField DataField="Wkt" HeaderText="GS" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right" />--%>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <div>
                                                                            <div>
                                                                                <%=GetLabel("Belum ada informasi penyinaran untuk program ini") %></div>
                                                                            <br />
                                                                            <span class="lblLink" id="lblAddProgramLog">
                                                                                <%= GetLabel("+ Penyinaran")%></span>
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:aspxcallbackpanel>
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
                            <div class="containerOrderDt" id="qaLog" style="display:none">
                                <dxcp:aspxcallbackpanel id="cbpViewDt2" runat="server" width="100%" clientinstancename="cbpViewDt2"
                                    showloadingpanel="false" oncallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContentQA" runat="server">
                                            <asp:Panel runat="server" ID="PanelQA" CssClass="pnlContainerGridPatientPage5">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="200px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddQA imgLink" title='<%=GetLabel("+ QA")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID="<%#:Eval("ProgramQAID") %>" programID="<%#:Eval("ProgramID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div id="div2" runat="server" style='margin-top: 5px; text-align: center'>
                                                                    <img class="imgEditQA imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" recordID="<%#:Eval("ProgramQAID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                    <img class="imgDeleteQA imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" recordID="<%#:Eval("ProgramQAID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                    <img class="imgCopyQA imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                        alt="" recordID="<%#:Eval("ProgramQAID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                    <img class="imgViewQA imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                        alt="" recordid="<%#:Eval("ProgramQAID") %>" programID="<%#:Eval("ProgramID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ProgramQAID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfQADate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="QATime" HeaderText="Jam" HeaderStyle-Width="60px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dibuat Oleh" HeaderStyle-Width="250px"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="Energy" HeaderText="Energi" HeaderStyle-Width="235px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="GCPesawatDescription" HeaderText="Pesawat" HeaderStyle-Width="235px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="TotalFraction" HeaderText="Jumlah Fraksi"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                        <asp:BoundField DataField="TotalDosage" HeaderText="Total Dosis"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                        <asp:BoundField DataField="MachineUnit" HeaderText="Total MU"  HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign = "Right" ItemStyle-HorizontalAlign = "Right"/>
                                                        <asp:BoundField DataField="VerificationType" HeaderText="Verifikasi" HeaderStyle-Width="235px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada informasi QA untuk program ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddQA">
                                                                <%= GetLabel("+ QA")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:aspxcallbackpanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:aspxcallbackpanel id="cbpDeleteLog" runat="server" width="100%" clientinstancename="cbpDeleteLog"
            showloadingpanel="false" oncallback="cbpDeleteLog_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteLogEndCallback(s); }" />
        </dxcp:aspxcallbackpanel>
        <dxcp:aspxcallbackpanel id="cbpDeleteQA" runat="server" width="100%" clientinstancename="cbpDeleteQA"
            showloadingpanel="false" oncallback="cbpDeleteQA_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteQAEndCallback(s); }" />
        </dxcp:aspxcallbackpanel>
        <dxcp:ASPxCallbackPanel ID="cbpVerification" runat="server" Width="100%" ClientInstanceName="cbpVerification"
            ShowLoadingPanel="false" OnCallback="cbpVerification_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVerificationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpCancelVerification" runat="server" Width="100%" ClientInstanceName="cbpCancelVerification"
            ShowLoadingPanel="false" OnCallback="cbpCancelVerification_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpCancelVerificationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
