<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="IPInitialAssessmentList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.IPInitialAssessmentList1" %>

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
    <script id="dxis_IPInitialAssessmentListctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>

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
                var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/IPNurseInitialAssessmentCtl1.ascx");
                openUserControlPopup(url, visitID, 'Kajian Awal Pasien Rawat Inap (Perawat)', 1300, 600);
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
                var noteTime = $tr.find('.noteTime').html();
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
            if ($('#<%=hdnID.ClientID %>').val() == 0 || $('#<%=hdnID.ClientID %>').val() == "") {
                displayErrorMessageBox('Warning', 'Maaf, Tidak ada Kajian awal yang dapat diedit.');
                return false;
            }
            else {
                if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) || $('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnRevisedParamedicID.ClientID %>').val()) {
                    if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                        displayErrorMessageBox('Warning', 'Maaf, Kajian awal hanya bisa dilakukan perubahan jika masih kurang dari 24 jam oleh user yang mengkaji atau yang melakukan koreksi serta belum diverifikasi');
                        return false;
                    }
                    else
                        return true;
                }
                else {
                    displayErrorMessageBox('EDIT', 'Maaf, Kajian awal hanya bisa dilakukan perubahan jika masih kurang dari 24 jam oleh user yang mengkaji atau yang melakukan koreksi serta belum diverifikasi');
                    return false;
                }
            }
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
            $('#<%=hdnID.ClientID %>').val('');
            $('#<%=hdnIsInitialAssessmentExists.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        $('#imgVisitNote.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/VisitNotesHistoryCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Perawat', 900, 500);
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
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
    <input type="hidden" runat="server" id="hdnMenuType" value="" />
    <input type="hidden" runat="server" id="hdnDeptType" value="" />
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
    <input type="hidden" runat="server" id="hdnIsInitialAssessmentExists" value="0" />
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
                                <asp:BoundField DataField="ChiefComplaintID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="RevisedByParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn revisedByParamedicID" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isEditable" />
                                <asp:BoundField DataField="cfIsAllowRevision" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn isAllowRevision" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:BoundField DataField="cfChiefComplaintDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="ChiefComplaintTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji oleh " HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                <asp:TemplateField HeaderText="Dikoreksi Oleh" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#:Eval("RevisedByParamedicName") %>
                                        <br />
                                        <span style="color: blue; font-style: italic; vertical-align: top; <%# Eval("RevisedByParamedicName") == "" ? "display:none": "" %>""> <%#:Eval("cfRevisionDate") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Keluhan Pasien" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="height: 130px; overflow-y: auto; <%# Eval("IsRevised").ToString() == "True" ? "font-style:italic; text-decoration: line-through": "" %>">
                                            <%#Eval("NurseChiefComplaintText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                      <div style="float: left; <%# Eval("IsRevised").ToString() == "False" ? "display:none": "" %>">
                                        <span style="color: blue; font-style: italic; vertical-align: top"><%#:Eval("RevisedByParamedicName") %>, <%#:Eval("cfRevisionDate") %></span>
                                        <div style="height: 130px; overflow-y: auto;">
                                            <%#Eval("RevisionRemarks").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                      </div>
                                      <div style="white-space: normal; overflow: auto; font-weight: bold; <%# Eval("IsVerifiedByPrimaryNurse").ToString() == "False" ? "display:none": "" %>">
                                        <span style='color: red;'>Diverifikasi oleh PP : </span>
                                        <span style='color: Blue;'>                                                    
                                            <%#:Eval("cfPrimaryNurseVerifiedDateTime") %>, <%#:Eval("PrimaryNurseName") %></span>
                                      </div>
                                      <div style="white-space: normal; overflow: auto; font-weight: bold; <%# Eval("IsVerifiedByRegisteredPhysician").ToString() == "False" ? "display:none": "" %>">
                                        <span style='color: red;'>Diverifikasi oleh DPJP : </span>
                                        <span style='color: Blue;'>                                                    
                                            <%#:Eval("cfRegisteredPhysicianVerifiedDateTime") %>, <%#:Eval("RegisteredPhysicianName") %></span>
                                      </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div id="divCompleted" runat="server" style='margin-top: 5px; text-align: center;'>
                                            <input type="button" id="btnComplete" runat="server" class="btnComplete w3-btn w3-hover-blue" value="Complete"
                                                style='<%# Eval("cfIsCompleted").ToString() == "True" ? "display:none;": "width: 100px; background-color: Red; color: White;" %>' />
                                        </div>
                                        <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Lihat"
                                                style='width: 100px; background-color: Green; color: White;'  />
                                            <div style="<%# Eval("cfIsAllowRevision").ToString() == "False" ? "display:none": "" %>" >
                                                <input type="button" id="btnRevision" runat="server" class="btnRevision w3-btn w3-hover-blue" value="Koreksi"
                                                    style='<%# Eval("cfIsCompleted").ToString() == "False" ? "display:none;": "margin-top:5px; width: 100px; background-color: Red; color: White;" %>'  />
                                            </div>
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
