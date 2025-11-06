<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PostHDAssessmentList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PostHDAssessmentList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_IPInitialAssessmentList">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.isEditable').html());
                $('#<%=hdnIsAllowRevision.ClientID %>').val($(this).find('.isAllowRevision').html());

            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('.btnView').live('click', function () {
                var id = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/MDNurseInitialAssessmentCtl1.ascx");
                openUserControlPopup(url, visitID+'|'+id, 'Asesmen Penunjang', 1300, 600);
            });

            $('.btnCopy').live('click', function () {
                var id = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var message = "Lakukan copy kajian awal pasien ?";
                displayConfirmationMessageBox('COPY KAJIAN AWAL :', message, function (result) {
                    if (result) {
                        var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/CopyVitalSignCtl.ascx");
                        var param = visitID + "|" + id;
                        openUserControlPopup(url, param, 'Copy Kajian Awal', 400, 300);
                    }
                });
            });

            $('.imgViewAssessment.imgLink').die('click');
            $('.imgViewAssessment.imgLink').live('click', function (evt) {
                var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/Hemodialysis/Assessment/ViewPreHDAssessmentCtl.ascx");
                var recordID = $(this).attr('recordID');
                var param = recordID;
                openUserControlPopup(url, param, "Peresepan Hemodialisa", 700, 500);
            });

            $('.imgCopyAssessment.imgLink').die('click');
            $('.imgCopyAssessment.imgLink').live('click', function (evt) {
                var url = ResolveUrl("~/libs/Controls/EMR/Nursing/CopyPreHDAssessmentCtl.ascx");
                var recordID = $(this).attr('recordID');
                var param = recordID;
                openUserControlPopup(url, param, "Copy Peresepan Hemodialisa", 400, 300);
            });
        });
        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnParamedicID.ClientID %>').val() != '0') {
                if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val())) {
                    if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                        displayErrorMessageBox('Warning', 'Maaf, Pengkajian sudah ditandai selesai atau sudah diverifikasi sehingga tidak bisa diubah.');
                        return false;
                    }
                    else
                        return true;
                }
                else {
                    displayErrorMessageBox('EDIT', 'Maaf, Kajian hanya bisa dilakukan perubahan oleh user yang mengkaji.');
                    return false;
                }
            }
            else {
                return true;
            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Kajian hanya bisa dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (hdnID == '' || hdnID == '0') {
                if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00160' || code == 'PM-00524') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'MR000013' || code == 'MR000017') {
                    filterExpression.text = visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                    return false;
                }
            }
            else if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019') {
                filterExpression.text = visitID;
                return true;
            }
            else if (code == 'PM-00425' || code == 'MR000016' || code == 'PM-00522' || code == 'PM-00523' || code == 'PM-00159') {
                filterExpression.text = registrationID;
                return true;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

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
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" runat="server" id="hdnMenuType" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <div style="position: relative; width: 100%">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                            <img class="imgViewAssessment imgLink" title='<%=GetLabel("Lihat Asesmen")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                alt="" recordID = "<%#:Eval("ID") %>" visitID = "<%#:Eval("VisitID") %>" />
<%--                                            <img class="imgCopyAssessment imgLink" title='<%=GetLabel("Copy Asesmen")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                alt="" recordID = "<%#:Eval("ID") %>" visitID = "<%#:Eval("VisitID") %>" />--%>                                        
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi " HeaderStyle-Width="160px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="RegistrationNo" />
                                <asp:BoundField DataField="PostHDParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:BoundField DataField="HDNo" HeaderText="HD Ke- " HeaderStyle-Width="60px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdType" />
                                <asp:BoundField DataField="HDType" HeaderText="Jenis Peresepan HD" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdNo" />
                                <asp:BoundField DataField="HDMachineType" HeaderText="Jenis Dialiser" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdMachineType" />
                                <asp:BoundField DataField="MachineNo" HeaderText="No. Mesin" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="machineNo" />
                                <asp:BoundField DataField="HDMethod" HeaderText="Teknik HD" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdMethod" />
                                <asp:BoundField DataField="cfStartDate" HeaderText="Tanggal Mulai " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfStartDate" />
                                <asp:BoundField DataField="StartTime" HeaderText="Jam Mulai" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="startTime" />
                                <asp:BoundField DataField="cfEndDate" HeaderText="Tanggal Selesai " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfEndDate" />
                                <asp:BoundField DataField="EndTime" HeaderText="Jam Selesai " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="endTime" />
                                <asp:BoundField DataField="PostHDParamedicName" HeaderText="Pengkajian Post HD oleh " HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                <asp:BoundField DataField="FinalUFG" HeaderText="UFG Akhir HD" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="finalUFG" />
                                <asp:BoundField DataField="TotalOutput" HeaderText="Output (Monitoring)" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalOutput" />
                                <asp:BoundField DataField="PrimingBalance" HeaderText="Priming" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="priming" />
                                <asp:BoundField DataField="WashOut" HeaderText="Wash Out" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="priming" />
                                <asp:BoundField DataField="TotalIntake" HeaderText="Intake (Monitoring)" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalIntake" />
                                <asp:BoundField DataField="TotalUF" HeaderText="Total UF" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalUF" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada informasi peresepan Hemodialisa untuk pasien ini") %>
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
    <dxcp:ASPxCallbackPanel ID="cbpCompleted" runat="server" Width="100%" ClientInstanceName="cbpCompleted"
        ShowLoadingPanel="false" OnCallback="cbpCompleted_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCompletedEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
