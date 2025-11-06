<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="IntraHDAssessmentList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.IntraHDAssessmentList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
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
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_IPInitialAssessmentList">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAssessmentID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnAssessmentID.ClientID %>').val() != "") {
                    cbpViewDt3.PerformCallback('refresh');
                    cbpViewDt4.PerformCallback('refresh');
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

            $('#<%=grdViewDt3.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt3.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
            });

            $('#<%=grdViewDt4.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt4.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
            });
        });

        //#region Pemantauan

        $('#lblAddIntra').die('click');
        $('#lblAddIntra').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "04" + "|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, "Pemantauan Intra HD", 700, 500, "X487^001");
            }
        });

        $('.imgAddIntra.imgLink').die('click');
        $('.imgAddIntra.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "04" + "|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, "Pemantauan Intra HD", 700, 500, "X487^001");
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function (evt) {
            if (onBeforeEdit()) {
                var recordID = $(this).attr('recordID');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/MonitoringVitalSignEntry1.ascx");
                var param = "04" + "|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|";
                openUserControlPopup(url, param, "Pemantauan Intra HD", 700, 500, "X487^001");
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            if (onBeforeEdit()) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi pemantauan hemodialisa untuk pasien ini ?";
                displayConfirmationMessageBox("Pemantauan Intra HD", message, function (result) {
                    if (result) {
                        cbpDeleteIntra.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgCopy.imgLink').die('click');
        $('.imgCopy').live('click', function () {
            var id = $(this).attr('recordID');
            var message = "Lakukan copy monitoring Intra HD ?";
            displayConfirmationMessageBox('COPY :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl.ascx");
                    var param = id;
                    openUserControlPopup(url, param, 'Copy', 400, 300);
                }
            });
        });

        $('.imgAddIntake.imgLink').die('click');
        $('.imgAddIntake.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
                var param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|0|1|0";
                openUserControlPopup(url, param, "Hemodialisa - Pemantauan Intake", 700, 500, "X487^001");
            }
        });

        $('.imgCopyIntakeOutput.imgLink').die('click');
        $('.imgCopyIntakeOutput.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
            var recordID = $(this).attr('recordID');
            var fluidGroup = $(this).attr('fluidGroup');
            var logTime = $(this).attr('logTime');
            var param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|1|1";
            var title = "Hemodialisa - Pemantauan Intake";
            if (fluidGroup == "X459^02") {
                url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|3|1";
                title = "Hemodialisa - Pemantauan Output";
            }
            openUserControlPopup(url, param, title, 700, 500, "X487^001");
        });

        $('.imgEditIntakeOutput.imgLink').die('click');
        $('.imgEditIntakeOutput.imgLink').live('click', function (evt) {
            $('#<%=hdnCurrentUserID.ClientID %>').val($(this).attr('createdBy'));
            if (onBeforeEdit()) {
                var recordID = $(this).attr('recordID');
                var fluidGroup = $(this).attr('fluidGroup');
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceIntakeEntry.ascx");
                var param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID + "|1|0";
                title = "Hemodialisa - Pemantauan Intake";
                if (fluidGroup == "X459^02") {
                    url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                    param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|" + recordID  + "|3|0";
                    title = "Hemodialisa - Pemantauan Output";
                }
                openUserControlPopup(url, param, title, 700, 500, "X487^001");
            }
        });

        $('.imgDeleteIntakeOutput.imgLink').die('click');
        $('.imgDeleteIntakeOutput.imgLink').live('click', function () {
            $('#<%=hdnCurrentUserID.ClientID %>').val($(this).attr('createdBy'));
            if (onBeforeEdit()) {
                var recordID = $(this).attr('recordID');
                var message = "Hapus informasi pemantauan hemodialisa Intake Output untuk pasien ini ?";
                displayConfirmationMessageBox("Pemantauan Intra HD - Intake/Output", message, function (result) {
                    if (result) {
                        cbpDeleteIntraFluidBalance.PerformCallback(recordID);
                    }
                });
            }
        });

        $('.imgAddOutput.imgLink').die('click');
        $('.imgAddOutput.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/FluidBalance/HSUFluidBalanceOutputEntry.ascx");
                var param = "04|" + $('#<%=hdnAssessmentID.ClientID %>').val() + "|0|3|0";
                openUserControlPopup(url, param, "Pemantauan Output", 700, 500, "X487^001");
            }
        });
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup() {
            cbpView.PerformCallback('refresh');
        }

        $('#imgVisitNote.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/VisitNotesHistoryCtl.ascx");
            openUserControlPopup(url, id, 'History Catatan Perawat', 900, 500);
        });

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

        //#region Paging Dt
        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount1, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt4EndCallback(s) {
            $('#containerImgLoadingViewDt4').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount4 = parseInt(param[1]);

                if (pageCount4 > 0)
                    $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt4"), pageCount4, function (page) {
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpCompletedEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('COMPLETED : FAILED', param[1]);
            }
        }

        function oncbpDeleteIntraEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Intra HD - Tanda Vital dan Indikator Lainnya', param[1]);
            }
        }

        function oncbpDeleteIntraFluidBalanceEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt4.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Intra HD - Intake/Output', param[1]);
            }
        }

        function onBeforeEdit() {
            if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('MEDINFRAS', 'Maaf, tidak diijinkan mengedit/menghapus record user lain.');
                return false;

            }
        }

        function onRefreshVitalSignGrid() {
            cbpViewDt3.PerformCallback('refresh');
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnMRN" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="0" />
    <input type="hidden" runat="server" id="hdnSurgeryReportID" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnOrderNo" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:60%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative; width: 100%">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn visitID" ItemStyle-CssClass="hiddenColumn visitID" />
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                            <asp:BoundField DataField="HDNo" HeaderText="HD Ke- " HeaderStyle-Width="50px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdType" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Jenis Dialiser") %></div>
                                                    <div><%=GetLabel("No. Mesin") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("HDMachineType")%></div>
                                                                <div><%#: Eval("MachineNo")%></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="HDMethod" HeaderText="Teknik HD" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdMethod" />
                                            <asp:BoundField DataField="QB" HeaderText="QB" HeaderStyle-Width="40px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="qb" />
                                            <asp:BoundField DataField="QD" HeaderText="QD" HeaderStyle-Width="40px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="qd" />
                                            <asp:BoundField DataField="UFGoal" HeaderText="UF" HeaderStyle-Width="40px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="uf" />
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("HDNo") %>" bindingfield="hdNo" />
                                                    <input type="hidden" value="<%#:Eval("HDDuration") %>" bindingfield="hdDuration" />
                                                    <input type="hidden" value="<%#:Eval("QB") %>" bindingfield="qb" />
                                                    <input type="hidden" value="<%#:Eval("QD") %>" bindingfield="QD" />
                                                    <input type="hidden" value="<%#:Eval("UFGoal") %>" bindingfield="ufGoal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi peresepan hemodialisa untuk pasien di kunjungan saat ini")%>
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
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">      
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="preVitalSign">
                                        <%=GetLabel("Tanda Vital dan Indikator Lainnya")%></li>
                                    <li contentid="preFluidBalance">
                                        <%=GetLabel("Intake-Output")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>   
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="preVitalSign">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt3_RowDataBound">
                                                    <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="80px">
                                                        <HeaderTemplate>
                                                            <img class="imgAddIntra imgLink" title='<%=GetLabel("+ Pemantauan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PreHDAssessmentID") %>" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                <img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PreHDAssessmentID") %>" />
                                                                <img class="imgDelete imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PreHDAssessmentID") %>" />
                                                                <img class="imgCopy imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" assessmentID = "<%#:Eval("PreHDAssessmentID") %>" />                                        
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    Indikator Pemantauan Intra Hemodialiasa</div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("ObservationDateInString")%>,
                                                                        <%#: Eval("ObservationTime") %>,
                                                                        <%#: Eval("ParamedicName") %>
                                                                    </b>
                                                                </div>
                                                                <div>
                                                                    <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                        <ItemTemplate>
                                                                            <div style="padding-left: 20px; float: left; width: 300px;">
                                                                                <strong>
                                                                                    <div style="width: 110px; float: left; color: blue" class="labelColumn">
                                                                                        <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                                    <div style="width: 20px; float: left;">
                                                                                        :</div>
                                                                                </strong>
                                                                                <div style="float: left;">
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <br style="clear: both" />
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                                <div>
                                                                    <span style="font-weight:bold; text-decoration: underline; color:Black"><%=GetLabel("Catatan Tambahan :")%></span>
                                                                    <br />
                                                                    <span style="font-style:italic">
                                                                        <%#: Eval("Remarks")%>
                                                                    </span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi pemantauan intra hemodialisa untuk sesi ini.") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddIntra">
                                                                <%= GetLabel("+ Pemantauan")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="preFluidBalance" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false"  OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="80px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddIntake imgLink" title='<%=GetLabel("+ Intake")%>' src='<%# ResolveUrl("~/Libs/Images/Button/fluidbalance_intake.png")%>'
                                                                    alt=""/>
                                                                <img class="imgAddOutput imgLink" title='<%=GetLabel("+ Output")%>' src='<%# ResolveUrl("~/Libs/Images/Button/fluidbalance_output.png")%>'
                                                                    alt=""/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditIntakeOutput imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID="<%#:Eval("ID") %>" createdBy="<%#:Eval("CreatedBy") %>" logTime="<%#:Eval("LogTime") %>" fluidGroup="<%#:Eval("GCFluidGroup") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteIntakeOutput imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID="<%#:Eval("ID") %>" createdBy="<%#:Eval("CreatedBy") %>" fluidGroup="<%#:Eval("GCFluidGroup") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgCopyIntakeOutput imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                alt="" recordID="<%#:Eval("ID") %>" createdBy="<%#:Eval("CreatedBy") %>" fluidGroup="<%#:Eval("GCFluidGroup") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfLogDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                                        <asp:BoundField HeaderText="Intake"  DataField="cfIntakeAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                        <asp:BoundField HeaderText="Output"  DataField="cfOutputAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi intake/output pada tanggal ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt4"></div>
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
        <dxcp:ASPxCallbackPanel ID="cbpCompleted" runat="server" Width="100%" ClientInstanceName="cbpCompleted"
            ShowLoadingPanel="false" OnCallback="cbpCompleted_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCompletedEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteIntra" runat="server" Width="100%" ClientInstanceName="cbpDeleteIntra"
            ShowLoadingPanel="false" OnCallback="cbpDeleteIntra_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteIntraEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteIntraFluidBalance" runat="server" Width="100%" ClientInstanceName="cbpDeleteIntraFluidBalance"
            ShowLoadingPanel="false" OnCallback="cbpDeleteIntraFluidBalance_Callback">
            <ClientSideEvents EndCallback="function(s,e){ oncbpDeleteIntraFluidBalanceEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
