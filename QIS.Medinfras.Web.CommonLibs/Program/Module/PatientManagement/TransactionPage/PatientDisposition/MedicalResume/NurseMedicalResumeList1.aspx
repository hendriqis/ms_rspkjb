<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="NurseMedicalResumeList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NurseMedicalResumeList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
<%--    <li id="btnPreview" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
            <%=GetLabel("Preview")%></div>
    </li>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
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
                $('#<%=hdnRevisedParamedicID.ClientID %>').val($(this).find('.revisedByParamedicID').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.isEditable').html());
                $('#<%=hdnIsAllowRevision.ClientID %>').val($(this).find('.isAllowRevision').html());
                $('#<%=hdnPatientNoteType.ClientID %>').val($(this).find('.controlColumn').html());

            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('.btnRevision').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var isAllowRevision = $(this).closest('tr').find('.isAllowRevision').html();
                if (isAllowRevision == "True") {
                    var message = "Lakukan koreksi kajian awal pasien ?";
                    displayConfirmationMessageBox('KOREKSI KAJIAN AWAL :', message, function (result) {
                        if (result) {
                            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/AssessmentRevisionNoteCtl.ascx");
                            var param = visitID + "|" + id;
                            openUserControlPopup(url, param, 'Koreksi Kajian Awal', 400, 300);
                        }
                    });
                }
                else {
                    displayErrorMessageBox('Warning', 'Maaf, Pengkajian hanya bisa dikoreksi dalam waktu 24 jam.');
                }
            });

            $('.btnView').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/IPNurseInitialAssessmentCtl.ascx");
                openUserControlPopup(url, visitID, 'Nurse Initial Assessment', 1300, 600);
            });

            //#region Completed
            $('.btnComplete').click(function (evt) {
                $('#<%=hdnID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) || $('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnRevisedParamedicID.ClientID %>').val()) {
                    if ($('#<%=hdnID.ClientID %>').val() != '') {
                        var message = "Proses pengkajian pasien menjadi selesai (completed) ?";
                        displayConfirmationMessageBox('STATUS PENGKAJIAN :', message, function (result) {
                            if (result) {
                                cbpCompleted.PerformCallback($('#<%=hdnID.ClientID %>').val());
                            }
                        });
                    }
                }
                else {
                    displayErrorMessageBox('Warning', 'Maaf, Pengkajian hanya bisa ditandai selesai oleh PPA yang mengkaji atau mengkoreksi.');
                }
            });
            //#endregion
        });
        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                displayErrorMessageBox('EDIT', 'Maaf, Tidak ada data Resume Medis yang dapat dilihat saat ini.');
                return false;
            }
            else {
                return true;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
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
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (hdnID == '' || hdnID == '0') {
                errMessage.text = 'Pasien tidak memiliki Resume Keperawatan';
                return false;
            }
            else {
                filterExpression.text = visitID;
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
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnMenuType" value="" />
    <input type="hidden" runat="server" id="hdnDeptType" value="" />
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isEditable" />
                                <asp:BoundField DataField="cfMedicalResumeDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="MedicalResumeTime" HeaderText="Jam " HeaderStyle-Width="60px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:BoundField DataField="ParamedicName" HeaderText="Dibuat oleh " HeaderStyle-Width="220px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                <asp:TemplateField HeaderText="Masalah Keperawatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                    <ItemTemplate>
                                        <div style="height: 130px; overflow-y: auto;">
                                            <%#Eval("SubjectiveResumeText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tindakan Keperawatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                    <ItemTemplate>
                                        <div style="height: 130px; overflow-y: auto;">
                                            <%#Eval("PlanningResumeText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Evaluasi Keperawatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                    <ItemTemplate>
                                        <div style="height: 130px; overflow-y: auto;">
                                            <%#Eval("EvaluationResumeText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="150px" Visible="false">
                                    <ItemTemplate>
                                        <%--                                        <div id="divCompleted" runat="server" style='margin-top: 5px; text-align: center;'>
                                            <input type="button" id="btnComplete" runat="server" class="btnComplete w3-btn w3-hover-blue" value="Complete"
                                                style='<%# Eval("cfIsCompleted").ToString() == "True" ? "display:none;": "width: 100px; background-color: Red; color: White;" %>' />
                                        </div>
                                        <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Lihat"
                                                style='<%# Eval("cfIsCompleted").ToString() == "False" ? "display:none;": "width: 100px; background-color: Green; color: White;" %>'  />
                                            <div style="<%# Eval("cfIsAllowRevision").ToString() == "False" ? "display:none": "" %>" >
                                                <input type="button" id="btnRevision" runat="server" class="btnRevision w3-btn w3-hover-blue" value="Koreksi"
                                                    style='<%# Eval("cfIsCompleted").ToString() == "False" ? "display:none;": "margin-top:5px; width: 100px; background-color: Red; color: White;" %>'  />
                                            </div>--%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada Resume Medis untuk pasien ini") %>
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
