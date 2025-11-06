<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MedicalResumeList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalResumeList" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
     <li id="btnPrint" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print")%></div>
    </li>
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

            $('#<%=btnPrint.ClientID %>').die('click');
            $('#<%=btnPrint.ClientID %>').live('click', function () {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                var medicalResumeID = $row.find('.hdnID').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var reportCode = $('#<%=hdnReportCode.ClientID %>').val();
                var id = $('#<%=hdnID.ClientID %>').val();
                var filterExpression = visitID + '|' + medicalResumeID;
                $('#<%=hdnID.ClientID %>').val(medicalResumeID);

                if (id == '' || id == '0') {
                    displayErrorMessageBox('PRINT', 'Maaf, Tidak ada data Resume Medis yang dapat di print.');
                    return false;
                }
                else {
                    openReportViewer(reportCode, filterExpression);
                    return true;
                }
            });

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

            //#region Signature
            $('.btnSignature').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteimTe').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.lblParamedicName').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var signature1 = $tr.find('.signature1').html();
                var signature2 = $tr.find('.signature2').html();
                var signature3 = $tr.find('.signature3').html();
                var signature4 = $tr.find('.signature4').html();
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });
            //#endregion
        });
        function onBeforeBasePatientPageListEdit() {
            var id = $('#<%=hdnID.ClientID %>').val();

            if (id == '' || id == '0') {
                displayErrorMessageBox('PRINT', 'Maaf, Tidak ada data Resume Medis yang dapat diedit saat ini.');
                return false;
            }
            else {
                return true;
            }
//            if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) || $('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnRevisedParamedicID.ClientID %>').val()) {
//                if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
//                    displayErrorMessageBox('Warning', 'Maaf, Pengkajian sudah ditandai selesai atau sudah diverifikasi sehingga tidak bisa diubah.');
//                    return false;
//                }
//                else
//                    return true;
//            }
//                else {
//                displayErrorMessageBox('EDIT', 'Maaf, Kajian awal hanya bisa dilakukan perubahan oleh user yang mengkaji atau yang melakukan koreksi.');
//                return false;
//            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Kajian awal hanya bisa dihapus oleh user yang mengkaji.');
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
            else if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'PM-90008' || code == 'PM-90009' ||
                     code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014') {
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
    <input type="hidden" id="hdnReportCode" runat="server" />
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isEditable" />
                                <asp:BoundField DataField="cfMedicalResumeDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="MedicalResumeTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:BoundField DataField="ParamedicName" HeaderText="Dibuat oleh " HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                <asp:TemplateField HeaderText="Resume Medis" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="height: 130px; overflow-y: auto;">
                                            <%#Eval("SubjectiveResumeText").ToString().Replace("\n","<br />")%><br />
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
                                <%=GetLabel("Tidak ada pengkajian awal untuk pasien ini") %>
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
