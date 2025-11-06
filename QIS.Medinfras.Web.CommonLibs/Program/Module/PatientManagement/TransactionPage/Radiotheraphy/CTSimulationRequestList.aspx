<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="CTSimulationRequestList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.CTSimulationRequestList" %>

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
                $('#<%=hdnSimulationRequestID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnSimulationRequestStatus.ClientID %>').val($(this).find('.requestStatus').html());
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                if ($('#<%=hdnSimulationRequestID.ClientID %>').val() != "") {
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

        function onRefreshLogGrid() {
            cbpViewDt1.PerformCallback('refresh');
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

        //#region View Program
        $('.imgRequest.imgLink').die('click');
        $('.imgRequest.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewCTSimulationRequestCtl.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID + "|" + + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
            openUserControlPopup(url, param, "Program Radiasi Eksterna", 800, 500);
        });
        //#endregion

        $('.imgAddSimulationLog.imgLink').die('click');
        $('.imgAddSimulationLog.imgLink').live('click', function (evt) {
            addSimulationLog();
        });

        $('#lblAddSimulationLog').die('click');
        $('#lblAddSimulationLog').live('click', function (evt) {
            addSimulationLog();
        });

        function addSimulationLog() {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                if (($('#<%=hdnSimulationRequestStatus.ClientID %>').val() != "X121^001") && $('#<%=hdnSimulationRequestStatus.ClientID %>').val() != "X121^999") {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestLogEntryCtl.ascx");
                    var param = "0" + "|" + $('#<%=hdnSimulationRequestID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
                    openUserControlPopup(url, param, "Catatan Simulasi", 800, 500, "");
                }
                else {
                    displayErrorMessageBox("Catatan Simulasi", "Catatan Simulasi tidak bisa dilakukan karena Status Permintaan masih OPEN atau sudah DIBATALKAN.");
                }
            }
            else {
                displayErrorMessageBox("Catatan Simulasi", "Catatan Simulasi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        }

        $('.imgEditSimulationLog.imgLink').die('click');
        $('.imgEditSimulationLog.imgLink').live('click', function (evt) {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var allow = true;
                if (allow) {
                    var recordID = $(this).attr('recordID');
                    var paramedicID = $(this).attr('paramedicID');
                    if (onBeforeEditDelete(paramedicID)) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestLogEntryCtl.ascx");
                        var param = recordID + "|" + $('#<%=hdnSimulationRequestID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
                        openUserControlPopup(url, param, "Pencatatan Simulasi", 800, 500, "");
                    }
                }
            }
            else {
                displayErrorMessageBox("Catatan Simulasi", "Catatan Simulasi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgDeleteSimulationLog.imgLink').die('click');
        $('.imgDeleteSimulationLog.imgLink').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                if (onBeforeEditDelete(paramedicID)) {
                    var message = "Hapus informasi catatan simulasi untuk pasien ini ?";
                    displayConfirmationMessageBox("Catatan Simulasi", message, function (result) {
                        if (result) {
                            var param = recordID;
                            cbpDeleteLog.PerformCallback(param);
                        }
                    });
                }
            }
            else {
                displayErrorMessageBox("Catatan Simulasi", "Catatan Simulasi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgCopySimulationLog.imgLink').die('click');
        $('.imgCopySimulationLog').live('click', function () {
            if ($('#<%=hdnIsEMR.ClientID %>').val() != "1") {
                var recordID = $(this).attr('recordID');
                var paramedicID = $(this).attr('paramedicID');
                var message = "Lakukan copy catatan simulasi ?";
                displayConfirmationMessageBox('COPY :', message, function (result) {
                    if (result) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/CTSimulationRequestLogEntryCtl.ascx");
                        var param = "0" + "|" + $('#<%=hdnSimulationRequestID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val() + "|" + recordID;
                        openUserControlPopup(url, param, "Pencatatan Simulasi", 800, 500, "");
                    }
                });
            }
            else {
                displayErrorMessageBox("Catatan Simulasi", "Catatan Simulasi hanya bisa dilakukan melalui akses user di Module Radioterapi");
            }
        });

        $('.imgViewSimulationLog.imgLink').die('click');
        $('.imgViewSimulationLog.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/ViewCTSimulationRequestLogCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnSimulationRequestID.ClientID %>').val() + "|" + $('#<%=hdnPageVisitID.ClientID %>').val() + "|" + $('#<%=hdnPageMRN.ClientID %>').val() + "|" + $('#<%=hdnPageMedicalNo.ClientID %>').val() + "|" + $('#<%=hdnPagePatientName.ClientID %>').val() + "|" + $('#<%=hdnCurrentParamedicID.ClientID %>').val();
            openUserControlPopup(url, param, "Pencatatan Simulasi", 800, 500, "");
        });

        $('.imgVerification1.imgLink').die('click');
        $('.imgVerification1.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            if (onBeforeVerification()) {
                var message = "Lakukan proses verifikasi terhadap Catatan Simulasi ?";
                displayConfirmationMessageBox("Verifikasi Catatan Simulasi", message, function (result) {
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
                var message = "Lakukan proses batal verifikasi terhadap Catatan Simulasi ?";
                displayConfirmationMessageBox("Batal Verifikasi Catatan Simulasi", message, function (result) {
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
                displayErrorMessageBox('Verifikasi Catatan Simulasi', param[1]);
            }
        }

        function onCbpCancelVerificationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Batal Verifikasi Catatan Simulasi', param[1]);
            }
        }

        function onCbpDeleteLogEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt1.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Catatan Simulasi', param[1]);
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
    <input type="hidden" value="" id="hdnSimulationRequestID" runat="server" />
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
    <input type="hidden" value="" id="hdnSimulationRequestStatus" runat="server" />
    <input type="hidden" value="0" id="hdnIsEMR" runat="server" />

    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
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
                                            <asp:BoundField DataField="SimulationRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn" />
                                            <asp:BoundField DataField="GCRequestStatus" HeaderStyle-CssClass="hiddenColumn requestStatus" ItemStyle-CssClass="hiddenColumn requestStatus" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgRequest imgLink" title='<%=GetLabel("Lihat Program")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("SimulationRequestID") %>"
                                                        visitid="<%#:Eval("VisitID") %>"  />                                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Tanggal" DataField="cfRequestDateText" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfRequestDateText" HeaderStyle-Width="100px" />
                                            <asp:TemplateField HeaderText="Permintaan Simulasi" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="font-weight:bold"><%#Eval("SimulationRequestType")%></span>
                                                    </div>
                                                    <div>
                                                        <span style="font-style:italic">Scan Area : </span><span style="font-weight:bold"><%#Eval("cfScanArea")%></span>
                                                    </div>
                                                    <div>
                                                        <span style="font-weight:bold"><%#Eval("cfContrastType")%></span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>                         
                                            <asp:BoundField HeaderText="Status" DataField="RequestStatus" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="requestStatus" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada permintaan simulasi untuk pasien ini")%>
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
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li contentid="simulationLog" class="selected">
                                        <%=GetLabel("Catatan Simulasi")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="simulationLog">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt1').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2_1" runat="server">
                                        <asp:Panel runat="server" ID="Panel2_1" CssClass="pnlContainerGridPatientPage">
                                            <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="160px">
                                                        <HeaderTemplate>
                                                            <img class="imgAddSimulationLog imgLink" title='<%=GetLabel("+ Catatan Simulasi")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div id="div4" runat="server" style='margin-top: 5px; text-align: center'>
                                                                <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgEditSimulationLog imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgDeleteSimulationLog imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgCopySimulationLog imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                <img class="imgViewSimulationLog imgLink" title='<%=GetLabel("View")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" paramedicid="<%#:Eval("ParamedicID") %>" />
                                                                <img <%# Eval("IsVerified").ToString() == "True" ? "Style='display:none'":"" %> class="imgVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" />
                                                                <img <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %> class="imgCancelVerification1 imgLink hvr-pulse-grow" title='<%=GetLabel("Batal Verifikasi Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                                    alt="" recordid="<%#:Eval("SimulationRequestLogID") %>" simulationRequestID="<%#:Eval("SimulationRequestID") %>" /> 
                                                                <div <%# Eval("IsVerified").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                    <%#Eval("cfVerifiedInformation")%>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SimulationRequestLogID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ParamedicName" HeaderText="Pelaksana Simulasi" HeaderStyle-Width="150px"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Orientation" HeaderText="Orientasi Posisi" HeaderStyle-Width="100px"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="PatientPosition" HeaderText="Posisi Pasien" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="HandPosition" HeaderText="Posisi Tangan" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ImmobilizationTool" HeaderText="Alat Bantu Immobilisasi" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="cfHasPatientPositionImage" HeaderText="Foto Posisi Pasien" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div>
                                                        <div>
                                                            <%=GetLabel("Belum ada informasi catatan simulasi.") %></div>
                                                        <br />
                                                        <span class="lblLink" id="lblAddSimulationLog">
                                                            <%= GetLabel("+ Catatan Simulasi")%></span>
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
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteLog" runat="server" Width="100%" ClientInstanceName="cbpDeleteLog"
            ShowLoadingPanel="false" OnCallback="cbpDeleteLog_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteLogEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
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
