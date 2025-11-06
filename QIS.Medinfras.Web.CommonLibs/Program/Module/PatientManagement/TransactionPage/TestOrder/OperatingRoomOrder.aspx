<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" CodeBehind="OperatingRoomOrder.aspx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.OperatingRoomOrder" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomLeftButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
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
<asp:Content ID="Content5" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="testOrderList_script">
        var pageCount = parseInt('<%=PageCount %>');
        var pageCountDt1 = parseInt('<%=PageCountDt1 %>');
        var pageCountDt2 = parseInt('<%=PageCountDt2 %>');
        var pageCountDt3 = parseInt('<%=PageCountDt3 %>');
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    SetOrderEntityToControl(this);
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                } 
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Propose
            $('.btnPropose').die('click');
            $('.btnPropose').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                var isNextVisit = $(this).attr("isNextVisit");
                var mode = 'propose';
                var message = "Kirim order pemeriksaan ke penunjang ?";
                var entity = rowToObject($tr);
                var visitID = entity.VisitID;
                var schDate = entity.cfScheduledDateInString2;
                var testOrderID = $tr.find('.keyField').html();
                var hsuID = entity.HealthcareServiceUnitID;
                var hsuIDOK = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                var isAlreadyHaveOKOrder = 0;
                if (hsuID == hsuIDOK) {
                    var filterExpression = "VisitID = " + visitID + " AND ScheduledDate = '" + schDate + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + testOrderID + "' AND HealthcareServiceUnitID = '" + hsuIDOK + "'";
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (resultCheck) {
                        if (resultCheck != null) {
                            isAlreadyHaveOKOrder = 1;
                        }
                    });
                }

                if (isAlreadyHaveOKOrder) {
                    displayConfirmationMessageBox('SEND ORDER :', 'Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?', function (result) {
                        if (result) {
                            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                            onCustomButtonClick(mode);
                        }
                    });
                }
                else {
                    displayConfirmationMessageBox('SEND ORDER :', message, function (result) {
                        if (result) {
                            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                            onCustomButtonClick(mode);
                        }
                    });
                }
            });
            //#endregion

            //#region Reopen
            $('.btnReopen').die('click');
            $('.btnReopen').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('REOPEN ORDER :', 'Reopen order pemeriksaan penunjang ?', function (result) {
                    if (result) {
                        onCustomButtonClick('ReOpen');
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                    }
                });
            });
            //#endregion

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');

                if ($contentID == "surgeryProcedureGroup") {
                    setPaging($("#pagingDt1"), pageCountDt1, function (page) {
                        cbpViewDt.PerformCallback('changepage|' + page);
                    });
                }
                else if ($contentID == "surgeryParamedicTeam") {
                    setPaging($("#pagingDt2"), pageCountDt2, function (page) {
                        cbpViewDt2.PerformCallback('changepage|' + page);
                    });
                }
                else {
                    setPaging($("#pagingDt3"), pageCountDt2, function (page) {
                        cbpViewDt3.PerformCallback('changepage|' + page);
                    });
                }
            });
            //#endregion
        });


        function GetCurrentSelectedOrder(s) {
            var $tr = s;
            var idx = $('#<%=grdView.ClientID %> tr').index($tr);
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });
            return selectedObj;
        }
        
        function SetOrderEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedOrder(param);
            $('#<%=hdnID.ClientID %>').val(selectedObj.TestOrderID);
            $('#<%:txtEstimatedDuration.ClientID %>').val(selectedObj.EstimatedDuration);
            $('#<%:txtScheduleDate.ClientID %>').val(selectedObj.cfScheduledDateInString);
            $('#<%:txtScheduleTime.ClientID %>').val(selectedObj.ScheduledTime);
            $('#<%:txtRoomCode.ClientID %>').val(selectedObj.RoomCode);
            $('#<%:txtRoomName.ClientID %>').val(selectedObj.RoomName);
            $('#<%:txtRemarks.ClientID %>').val(selectedObj.Remarks);
            if (selectedObj.IsUsedRequestTime == "True")
                $('#<%=chkIsUsedRequestTime.ClientID %>').prop('checked', true);
            else
                $('#<%=chkIsUsedRequestTime.ClientID %>').prop('checked', false);
            if (selectedObj.IsUsingSpecificItem == "True")
                $('#<%=chkIsUsingSpecificItem.ClientID %>').prop('checked', true);
            else
                $('#<%=chkIsUsingSpecificItem.ClientID %>').prop('checked', false);
            if (selectedObj.IsEmergency == "True")
                $('#<%=rblIsEmergency.ClientID %>').val("1");
            else
                $('#<%=rblIsEmergency.ClientID %>').val("0");

            if (selectedObj.cfIsNextVisit == "True")
                $('#<%=chkIsNextVisit.ClientID %>').prop('checked', true);
            else
                $('#<%=chkIsNextVisit.ClientID %>').prop('checked', false);

            $('#<%:txtNextVisitType.ClientID %>').val(selectedObj.cfNextVisitType);

            if (selectedObj.IsCITO == "True")
                $('#<%=chkIsCITO.ClientID %>').prop('checked', true);
            else
                $('#<%=chkIsCITO.ClientID %>').prop('checked', false);
        }

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            onRefreshControl();
        });


        function onAfterSaveAddRecordEntryPopup(param) {
            afterPopupEntry(param);
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            afterPopupEntry(param);
        }

        function onAfterSaveRecordPatientPageEntry() {
            onRefreshControl();
        }

        function onAfterCustomClickSuccess() {
            onRefreshControl();
        }

        function afterPopupEntry(param) {
            var paramInfo = param.split('|');
            if (paramInfo[0] != "checklist") {
                $('#<%=hdnID.ClientID %>').val(param);
                onRefreshControl();
            }
            else {
                cbpViewDt3.PerformCallback('refresh');
            }

        }

        function onBeforeRefreshControl() {
            $('#<%=hdnID.ClientID %>').val("0");
            $('#<%:txtEstimatedDuration.ClientID %>').val('');
            $('#<%:txtScheduleDate.ClientID %>').val('');
            $('#<%:txtScheduleTime.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').val('');
            $('#<%:txtRoomName.ClientID %>').val('');
            $('#<%:txtRemarks.ClientID %>').val('');
            $('#<%:txtNextVisitType.ClientID %>').val('');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
            cbpViewDt2.PerformCallback('refresh');
            cbpViewDt3.PerformCallback('refresh');
        }

        //#region Paging
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingDt1"), pageCountDt1, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingDt2"), pageCountDt2, function (page) {
                cbpViewDt2.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingDt3"), pageCountDt3, function (page) {
                cbpViewDt3.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                } else {
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                }

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
        //#endregion

        //#region Paging Dt
        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount, function (page) {
                    cbpViewDt2.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();
        }
        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeBackToListPage() {
            if ($('#<%:hdnIsOutstandingOrder.ClientID %>').val() == "1") {
                var line1 = "Masih ada order pemeriksaan yang belum dikirim ke Penunjang, Silahkan dikirim order terlebih dahulu dengan meng-klik tombol <b>Send Order</b>";
                var line2 = "<br />Jika masih mengalami kendala, silahkan klik tombol <b>Refresh</b>";
                var messageBody = line1 + line2;
                displayMessageBox('SEND ORDER', messageBody);
            }
            else {
                backToPatientList();
            }
        }

        //#region Checklist Form
        $('#lblAddChecklist').die('click');
        $('#lblAddChecklist').live('click', function (evt) {
            addChecklist();
        });

        $('.imgAddChecklist.imgLink').die('click');
        $('.imgAddChecklist.imgLink').live('click', function (evt) {
            addChecklist();
        });

        function addChecklist() {
            var allow = true;
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PreProcedureChecklistFormEntry.ascx");
                var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "||||||" + $('#<%=hdnRegisterRegistrationID.ClientID %>').val() + "|";
                openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
            }
        }

        $('.imgEditChecklist.imgLink').die('click');
        $('.imgEditChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/PreProcedureChecklistFormEntry.ascx");
                var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val() + "|";
                openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Operasi', 'Maaf, tidak diijinkan mengedit pengkajian user lain.');
                return false;
            }
        });

        $('.imgDeleteChecklist.imgLink').die('click');
        $('.imgDeleteChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var paramedicID = $(this).attr('paramedicID');

            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == paramedicID) {
                var message = "Hapus informasi checklist persiapan kamar operasi untuk pasien ini ?";
                displayConfirmationMessageBox("Checklist Persiapan Kamar Operasi", message, function (result) {
                    if (result) {
                        cbpDeleteChecklist.PerformCallback(recordID);
                    }
                });
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Operasi', 'Maaf, tidak diijinkan menghapus pengkajian user lain.');
                return false;
            }
        });

        $('.imgViewChecklist.imgLink').die('click');
        $('.imgViewChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicName = $(this).attr('paramedicName');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/ViewPreProcedureChecklistCtl.ascx");
            var param = recordID + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicName + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val();
            openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
        });

        $('.imgCopyChecklist.imgLink').die('click');
        $('.imgCopyChecklist.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var checklistDate = $(this).attr('checklistDate');
            var checklistTime = $(this).attr('checklistTime');
            var paramedicID = $(this).attr('paramedicID');
            var formLayout = $(this).attr('formLayout');
            var formValue = $(this).attr('formValue');

            var message = "Lakukan copy form checklist persiapan pasien ?";
            displayConfirmationMessageBox('COPY FORM CHECKLIST :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/MedicalForm/CopyPreProcedureChecklistEntry.ascx");
                    var param = "0" + "|" + "X530^001" + "|" + "X531^001" + "|" + $('#<%=hdnID.ClientID %>').val() + "|" + $('#<%=hdnRegisterVisitID.ClientID %>').val() + "|" + checklistDate + "|" + checklistTime + "|" + paramedicID + "|" + formLayout + "|" + formValue + "|" + +$('#<%=hdnRegisterRegistrationID.ClientID %>').val();
                    openUserControlPopup(url, param, "Checklist Persiapan Operasi", 700, 500);
                }
            });
        });
        //#endregion

        function onCbpDeleteChecklistEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpViewDt3.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Checklist Persiapan Kamar Operasi', param[1]);
            }
        }

        function onAfterPopupControlClosing() {
            onRefreshControl();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="0" id="hdnID" runat="server" />
    <input type="hidden" id="hdnRegisterDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterVisitID" runat="server" value="" />
    <input type="hidden" id="hdnRegisterMRN" runat="server" value="" />
    <input type="hidden" id="hdnRegisterParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
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
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal ") %> - <%=GetLabel("Jam Order") %>,  <%=GetLabel("No. Order") %></div>
                                                    <div style="font-weight: bold"><span style="color:blue"><%=GetLabel("Diorder Oleh") %></span></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("TestOrderDateTimeInString")%>,  <span style="color:blue"> <%#: Eval("ParamedicName") %></span></div>
                                                                <div style="font-weight: bold"><%#: Eval("TestOrderNo")%></div>
                                                                <div <%# Eval("ScheduleStatus") == "" ? "Style='display:none'":"Style='font-style:italic;color: Red'"%> class="blink"><%#: Eval("ScheduleStatus")%>, <%#: Eval("cfRoomScheduleDate")%> - <%#: Eval("RoomScheduleTime")%></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %> ><div><input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color:Red;color:White; width:100px;" isNextVisit="<%#: Eval("cfIsNextVisit")%>" /></div></td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %> ><div><input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color:Green;color:White; width:100px" /></div></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                                                    <input type="hidden" value="<%#:Eval("EstimatedDuration") %>" bindingfield="EstimatedDuration" />
                                                    <input type="hidden" value="<%#:Eval("IsEmergency") %>" bindingfield="IsEmergency" />
                                                    <input type="hidden" value="<%#:Eval("IsUsingSpecificItem") %>" bindingfield="IsUsingSpecificItem" />                                                    
                                                    <input type="hidden" value="<%#:Eval("IsUsedRequestTime") %>" bindingfield="IsUsedRequestTime" />
                                                    <input type="hidden" value="<%#:Eval("IsCITO") %>" bindingfield="IsCITO" />
                                                    <input type="hidden" value="<%#:Eval("cfIsNextVisit") %>" bindingfield="cfIsNextVisit" />
                                                    <input type="hidden" value="<%#:Eval("cfNextVisitType") %>" bindingfield="cfNextVisitType" />
                                                    <input type="hidden" value="<%#:Eval("cfScheduledDateInString") %>" bindingfield="cfScheduledDateInString" />
                                                    <input type="hidden" value="<%#:Eval("ScheduledTime") %>" bindingfield="ScheduledTime" />
                                                    <input type="hidden" value="<%#:Eval("RoomCode") %>" bindingfield="RoomCode" />
                                                    <input type="hidden" value="<%#:Eval("RoomName") %>" bindingfield="RoomName" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                                    <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                    <input type="hidden" value="<%#:Eval("cfScheduledDateInString2") %>" bindingfield="cfScheduledDateInString2" />
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
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
                                    <li class="selected" contentid="surgeryGeneralContent">
                                        <%=GetLabel("Informasi Order")%></li>
                                    <li contentid="surgeryProcedureGroup">
                                        <%=GetLabel("Jenis Operasi")%></li>
                                    <li contentid="surgeryParamedicTeam">
                                        <%=GetLabel("Team Pelaksana")%></li>
                                    <li contentid="preProcedureChecklist">
                                        <%=GetLabel("Checklist Persiapan Operasi")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="surgeryGeneralContent" style="position: relative;">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 180px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Estimasi Lama Operasi") %></td>
                                        <td><asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" Enabled="false" /> menit</td>    
                                        <td style="padding-left:5px"><asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" Enabled="false" /></td>                                            
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Tanggal ")%>
                                                -
                                                <%=GetLabel("Jam Rencana")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtScheduleDate" Width="120px" runat="server" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" />
                                        </td>
                                        <td />
                                    </tr>
                                    <tr>
                                        <td class="tdLabel"><%=GetLabel("Jadwal Operasi") %></td>
                                        <td colspan="2">
                                            <table border="0" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblIsEmergency" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                                            <asp:ListItem Text=" Emergency" Value="1" />
                                                            <asp:ListItem Text=" Elektif" Value="0" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td style="padding-left:10px">
                                                        <asp:CheckBox ID="chkIsNextVisit" Width="180px" runat="server" Text=" Kunjungan Berikutnya" Enabled="false" />                                                        
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNextVisitType" Width="120px" runat="server" Enabled="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>    
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label runat="server" id="lblRoom">
                                                <%:GetLabel("Ruang Operasi")%></label>
                                        </td>
                                        <td colspan="2">
                                            <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" Enabled="False" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td />
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsUsingSpecificItem" Width="180px" runat="server" Text=" Penggunaan Alat Tertentu" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td />
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                            <label class="lblNormal">
                                                <%=GetLabel("Catatan Order") %></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline"
                                                Height="250px" Enabled="false" />
                                        </td>
                                    </tr>                                                         
                                </table>
                            </div>
                            <div class="containerOrderDt" id="surgeryProcedureGroup" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                <input type="hidden" value="<%#:Eval("ProcedureGroupID") %>" bindingfield="ProcedureGroupID" />
                                                                <input type="hidden" value="<%#:Eval("ProcedureGroupCode") %>" bindingfield="ProcedureGroupCode" />
                                                                <input type="hidden" value="<%#:Eval("ProcedureGroupName") %>" bindingfield="ProcedureGroupName" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Jenis Operasi")%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div><%#: Eval("ProcedureGroupName")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Kategori Operasi"  DataField="SurgeryClassification" HeaderStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
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
                            <div class="containerOrderDt" id="surgeryParamedicTeam" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                                <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Tenaga Medis" 
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" HeaderStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data team pelaksana untuk order ini")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2" >
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preProcedureChecklist" style="position: relative;display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                           <input type="hidden" value="" id="hdnFileString" runat="server" />
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddChecklist imgLink" title='<%=GetLabel("+ Checklist")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditChecklist imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>"/>
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteChecklist imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" paramedicID = "<%#:Eval("ParamedicID") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgViewChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgCopyChecklist imgLink" title='<%=GetLabel("Lihat Asesmen Pra Bedah")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" testOrderID = "<%#:Eval("TestOrderID") %>" checklistDate = "<%#:Eval("cfChecklistDatePickerFormat") %>" checklistTime = "<%#:Eval("ChecklistTime") %>" paramedicID = "<%#:Eval("ParamedicID") %>" paramedicName = "<%#:Eval("ParamedicName") %>"
                                                                                formLayout = "<%#:Eval("ChecklistFormLayout") %>" formValue = "<%#:Eval("ChecklistFormValue") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                        <asp:BoundField DataField="cfChecklistDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField DataField="ChecklistTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                                        <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                                        <asp:BoundField DataField="GCChecklistType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                        <asp:BoundField DataField="ChecklistFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                        <asp:BoundField DataField="cfChecklistDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada data checklist persiapan untuk nomor order ini") %>
                                                        <br />
                                                        <span class="lblLink" id="lblAddChecklist">
                                                            <%= GetLabel("+ Checklist")%></span>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt3">
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
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteChecklist" runat="server" Width="100%" ClientInstanceName="cbpDeleteChecklist"
            ShowLoadingPanel="false" OnCallback="cbpDeleteChecklist_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteChecklistEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
