<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="SurgeryAnesthesyList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.SurgeryAnesthesyList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhRightToolbar" runat="server">
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
    <script id="dxis_IPInitialAssessmentListctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>

    <script type="text/javascript" id="dxss_IPInitialAssessmentList">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnID.ClientID %>').val() != "") {
                    cbpViewDt.PerformCallback('refresh');
                    setTimeout(function () { cbpViewDt3.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt4.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt5.PerformCallback('refresh'); }, 1000);
                    setTimeout(function () { cbpViewDt6.PerformCallback('refresh'); }, 1000);
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Asesmen Pra Bedah
            $('.imgViewPreSurgeryAssessment.imgLink').die('click');
            $('.imgViewPreSurgeryAssessment.imgLink').live('click', function () {
                var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewAssesmentPreOpCtl1.ascx");
                var recordID = $(this).attr('recordID');
                var visitID = $(this).attr('visitID');
                var testorderID = $(this).attr('testorderID');
                $('#<%=hdnAssessmentID.ClientID %>').val(recordID);
                var param = visitID + '|' + testorderID + '|' + recordID;
                openUserControlPopup(url, param, "Asesmen Pra Bedah", 1024, 500);
            });
            //#endregion

            //#region Asesmen Pra Anestesi dan Pra Sedasi
            $('#lblAddAnesthesy').die('click');
            $('#lblAddAnesthesy').live('click', function (evt) {
                if ($('#<%=hdnInputSurgeryAssessmentFirst.ClientID %>').val() == "1") {
                    if ($('#<%=hdnIsSurgeryPreAssessmentExists.ClientID %>').val() == "1")
                        cbpMPListProcess.PerformCallback('add');
                    else
                        displayErrorMessageBox('Asesmen Pra Anestesi', 'Maaf, Asesmen Pra Anestesi hanya bisa dilakukan jika sudah ada Asesmen Pra Bedah');
                }
                else {
                    cbpMPListProcess.PerformCallback('add');
                }
            });
            //#endregion

            //#region Asesmen Status Anestesi
            $('#lblAddAnesthesyStatus').die('click');
            $('#lblAddAnesthesyStatus').live('click', function (evt) {
                if ($('#<%=hdnInputSurgeryAssessmentFirst.ClientID %>').val() == "1") {
                    if ($('#<%=hdnIsSurgeryPreAssessmentExists.ClientID %>').val() == "1")
                        cbpMPListProcess.PerformCallback('add');
                    else
                        displayErrorMessageBox('Asesmen Status Anestesi', 'Maaf, Asesmen Status Anestesi hanya bisa dilakukan jika sudah ada Asesmen Pra Bedah');
                }
                else {
                    cbpMPListProcess.PerformCallback('add');
                }

            });
            //#endregion

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

        $('.imgEditAssessment.imgLink').die('click');
        $('.imgEditAssessment.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAssessment.imgLink').die('click');
        $('.imgDeleteAssessment.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            var message = "Hapus assesmen pra bedah untuk pasien ini ?";
            displayConfirmationMessageBox("Asesmen Pra Bedah", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEditAnesthesy.imgLink').die('click');
        $('.imgEditAnesthesy.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAnesthesy.imgLink').die('click');
        $('.imgDeleteAnesthesy.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            var message = "Hapus assesmen pra anestesi untuk pasien ini ?";
            displayConfirmationMessageBox("Asesmen Pra Anestesi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEditAnesthesyStatus.imgLink').die('click');
        $('.imgEditAnesthesyStatus.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteAnesthesyStatus.imgLink').die('click');
        $('.imgDeleteAnesthesyStatus.imgLink').live('click', function () {
            var assessmentID = $(this).attr('assessmentID');
            $('#<%=hdnAssessmentID.ClientID %>').val(assessmentID);
            var message = "Hapus status anestesi untuk pasien ini ?";
            displayConfirmationMessageBox("Status Anestesi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });

        //#region Laporan Operasi
        $('#lblAddReport').die('click');
        $('#lblAddReport').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) cbpMPListProcess.PerformCallback('add');
        });

        $('.imgAddReport2.imgLink').die('click');
        $('.imgAddReport2.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) cbpMPListProcess.PerformCallback('add');
        });

        $('.imgEditReport.imgLink').die('click');
        $('.imgEditReport.imgLink').live('click', function () {
            var paramedicID = $(this).attr('paramedicID');
            var recordID = $(this).attr('recordID');
            $('#<%=hdnParamedicID.ClientID %>').val(paramedicID);
            $('#<%=hdnSurgeryReportID.ClientID %>').val(recordID);
            var allow = true;
            if (typeof onBeforeBasePatientPageListEdit == 'function') {
                allow = onBeforeBasePatientPageListEdit();
            }
            if (allow) cbpMPListProcess.PerformCallback('edit');
        });

        $('.imgDeleteReport.imgLink').die('click');
        $('.imgDeleteReport.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            $('#<%=hdnSurgeryReportID.ClientID %>').val(recordID);
            var message = "Hapus laporan operasi untuk untuk pasien ini ?";
            displayConfirmationMessageBox("Laporan Operasi", message, function (result) {
                if (result) {
                    cbpMPListProcess.PerformCallback('delete');
                }
            });
        });
        //#endregion

        //#region e-Document
        $('#lblAddDocument').die('click');
        $('#lblAddDocument').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
                var param = "|" + $('#<%=hdnID.ClientID %>').val();
                openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
            }
        });

        $('.imgAddDocument.imgLink').die('click');
        $('.imgAddDocument.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = "|" + $('#<%=hdnID.ClientID %>').val();
            openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
        });

        $('.imgEditDocument.imgLink').die('click');
        $('.imgEditDocument.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/Program/PatientPage/_PopupEntry/eDocument/PatientDocumentEntryCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnID.ClientID %>').val();
            openUserControlPopup(url, param, "Pengkajian Kamar Operasi - e-Document", 700, 500);
        });

        $('.imgDeleteDocument.imgLink').die('click');
        $('.imgDeleteDocument.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var fileName = $(this).attr('fileName');
            var message = "Hapus file/dokumen " + fileName + " dari Pengkajian Kamar Operasi untuk pasien ini ?";
            displayConfirmationMessageBox("e-Document", message, function (result) {
                if (result) {
                    cbpDeleteDocument.PerformCallback(recordID);
                }
            });
        });

        $('.lnkViewDocument a').die('click');
        $('.lnkViewDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });
        //#endregion


        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                    displayErrorMessageBox('Warning', 'Maaf, Assessment sudah ditandai selesai atau sudah diverifikasi sehingga tidak bisa diubah.');
                    return false;
                }
                else
                    return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Assessment hanya bisa diubah oleh user yang mengkaji.');
                return false;
            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('DELETE', 'Maaf, Assessment hanya bisa dihapus oleh user yang mengkaji.');
                return false;
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshDocumentGrid() {
            //cbpViewDt5.PerformCallback('refresh');
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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount3 = parseInt(param[1]);

                if (pageCount3 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount3, function (page) {
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
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt4"), pageCount1, function (page) {
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt5EndCallback(s) {
            $('#containerImgLoadingViewDt5').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt5"), pageCount1, function (page) {
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt6EndCallback(s) {
            $('#containerImgLoadingViewDt6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt6"), pageCount1, function (page) {
                    cbpViewDt6.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt6.ClientID %> tr:eq(1)').click();
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

        function onCbpDeleteDocumentEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt5.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('e-Document', param[1]);
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
    <input type="hidden" runat="server" id="hdnInputSurgeryAssessmentFirst" value="" />
    <input type="hidden" runat="server" id="hdnIsSurgeryPreAssessmentExists" value="0" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="TestOrderNo" HeaderStyle-CssClass="hiddenColumn orderNo" ItemStyle-CssClass="hiddenColumn orderNo" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal ") %> - <%=GetLabel("Jam Order") %>,  <span style="color:blue"><%=GetLabel("Diorder Oleh") %></span></div>
                                                    <div style="font-weight: bold"><%=GetLabel("Unit Pelayanan") %>, <%=GetLabel("No. Order") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("TestOrderDateTimeInString")%>,  <span style="color:blue"> <%#: Eval("ParamedicName") %></span></div>
                                                                <div style="font-weight: bold"><%#: Eval("ServiceUnitName")%>, <%#: Eval("TestOrderNo")%></div>
                                                                <div style="font-style:italic;color: Red" class="blink"><%#: Eval("ScheduleStatus")%>, <%#: Eval("cfRoomScheduleDate")%> - <%#: Eval("RoomScheduleTime")%></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> ><div><input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color:Red;color:White; width:100px;" /></div></td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %> ><div><input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color:Green;color:White; width:100px" /></div></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("EstimatedDuration") %>" bindingfield="EstimatedDuration" />
                                                    <input type="hidden" value="<%#:Eval("IsUsingSpecificItem") %>" bindingfield="IsUsingSpecificItem" />
                                                    <input type="hidden" value="<%#:Eval("IsUsedRequestTime") %>" bindingfield="IsUsedRequestTime" />
                                                    <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                    <input type="hidden" value="<%#:Eval("cfScheduledDateInString") %>" bindingfield="cfScheduledDateInString" />
                                                    <input type="hidden" value="<%#:Eval("ScheduledTime") %>" bindingfield="ScheduledTime" />
                                                    <input type="hidden" value="<%#:Eval("RoomCode") %>" bindingfield="RoomCode" />
                                                    <input type="hidden" value="<%#:Eval("RoomName") %>" bindingfield="RoomName" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order Penunjang untuk pasien ini")%>
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
                                    <li class="selected" contentid="preSurgeryAssessment">
                                        <%=GetLabel("Asesmen Pra Bedah")%></li>
                                    <li contentid="preSurgeryAnesthesy">
                                        <%=GetLabel("Asesmen Pra Anestesi")%></li>
                                    <li contentid="anesthesyStatus">
                                        <%=GetLabel("Status Anestesi")%></li>
                                    <li contentid="surgeryReport">
                                        <%=GetLabel("Laporan Operasi")%></li>
                                    <li contentid="surgeryDocument">
                                        <%=GetLabel("e-Document")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="preSurgeryAssessment">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreSurgeryAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreSurgicalAssessmentID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreSurgicalAssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreSurgicalAssessmentID") %>" bindingfield="PreSurgicalAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis"  DataField="PreDiagnoseText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada asesmen pra bedah untuk pasien ini") %></div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt1"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="preSurgeryAnesthesy" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditAnesthesy imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" assessmentID = "<%#:Eval("PreAnesthesyAssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteAnesthesy imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" assessmentID = "<%#:Eval("PreAnesthesyAssessmentID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PreAnesthesyAssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PreAnesthesyAssessmentID") %>" bindingfield="PreAnesthesyAssessmentID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada asesmen pra anestesi dan sedasi untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddAnesthesy">
                                                                <%= GetLabel("+ Asesmen Pra Anestesi")%></span>
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
                            <div class="containerOrderDt" id="anesthesyStatus" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt6" runat="server" Width="100%" ClientInstanceName="cbpViewDt6"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt6_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt6').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt6EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt6" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditAnesthesyStatus imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" assessmentID = "<%#:Eval("AnesthesyStatusID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteAnesthesyStatus imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" assessmentID = "<%#:Eval("AnesthesyStatusID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AnesthesyStatusID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("AnesthesyStatusID") %>" bindingfield="AnesthesyStatusID" />
                                                                <input type="hidden" value="<%#:Eval("cfAssessmentDate") %>" bindingfield="cfAssessmentDate" />
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="cfAssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada status anestesi untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddAnesthesyStatus">
                                                                <%= GetLabel("+ Status Anestesi")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>    
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt6" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt6"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="surgeryReport" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddReport2 imgLink" title='<%=GetLabel("+ Laporan")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditReport imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" recordID = "<%#:Eval("PatientSurgeryID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteReport imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" paramedicID = "<%#:Eval("ParamedicID") %>" recordID = "<%#:Eval("PatientSurgeryID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PatientSurgeryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("PatientSurgeryID") %>" bindingfield="PatientSurgeryID" />
                                                                <input type="hidden" value="<%#:Eval("cfReportDate") %>" bindingfield="cfReportDate" />
                                                                <input type="hidden" value="<%#:Eval("ReportTime") %>" bindingfield="ReportTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal"  DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="ReportTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis"  DataField="PreOperativeDiagnosisText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Post Diagnosis"  DataField="PostOperativeDiagnosisText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada laporan operasi untuk pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddReport">
                                                                <%= GetLabel("+ Laporan Operasi")%></span>
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
                            <div class="containerOrderDt" id="surgeryDocument" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddDocument imgLink" title='<%=GetLabel("+ Document")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("FileName") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditDocument imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("TestOrderID") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteDocument imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" fileName = "<%#:Eval("FileName") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fileName" />
                                                        <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div><%=GetLabel("Belum ada informasi e=document untuk tindakan operasi di pasien ini") %></div>
                                                            <br />
                                                            <span class="lblLink" id="lblAddDocument">
                                                                <%= GetLabel("+ Document")%></span>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt5"></div>
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

        <dxcp:ASPxCallbackPanel ID="cbpDeleteDocument" runat="server" Width="100%" ClientInstanceName="cbpDeleteDocument"
            ShowLoadingPanel="false" OnCallback="cbpDeleteDocument_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteDocumentEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
