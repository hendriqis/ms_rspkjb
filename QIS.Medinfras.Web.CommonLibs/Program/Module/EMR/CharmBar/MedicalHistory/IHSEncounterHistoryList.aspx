<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="IHSEncounterHistoryList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.IHSEncounterHistoryList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_SurgeryHistoryList">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
            if ($('#<%=hdnID.ClientID %>').val() != "") {
                cbpViewDt.PerformCallback('refresh');
//                cbpViewDt2.PerformCallback('refresh');
//                cbpViewDt3.PerformCallback('refresh');
//                cbpViewDt4.PerformCallback('refresh');
//                cbpViewDt5.PerformCallback('refresh');
//                cbpViewDt6.PerformCallback('refresh');
//                cbpViewDt7.PerformCallback('refresh');
//                cbpViewDt8.PerformCallback('refresh');
            }
        });
        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=rblItemType.ClientID %> input').live('change', function () {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
//                cbpViewDt2.PerformCallback('refresh');
//                cbpViewDt3.PerformCallback('refresh');
//                cbpViewDt4.PerformCallback('refresh');
//                cbpViewDt5.PerformCallback('refresh');
//                cbpViewDt6.PerformCallback('refresh');
//                cbpViewDt7.PerformCallback('refresh');
//                cbpViewDt8.PerformCallback('refresh');
            });

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                switch ($('#<%=hdnSelectedTab.ClientID %>').val()) {
                    case "preSurgeryAssessment":
                        cbpViewDt.PerformCallback('refresh');
                        break;
                    case "preAnesthesyAssessment":
                        cbpViewDt7.PerformCallback('refresh');
                        break;
                    case "anesthesyStatus":
                        cbpViewDt8.PerformCallback('refresh');
                        break;
                    case "preSurgicalSafetyChecklist":
                        cbpViewDt2.PerformCallback('refresh');
                        break;
                    case "periOperative":
                        cbpViewDt6.PerformCallback('refresh');
                        break;
                    case "surgeryReport":
                        cbpViewDt4.PerformCallback('refresh');
                        break;
                    case "patientMedicalDevice":
                        cbpViewDt3.PerformCallback('refresh');
                        break;
                    case "surgeryDocument":
                        cbpViewDt5.PerformCallback('refresh');
                        break;
                    default:
                        break;
                }
            });
            //#endregion
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
//            cbpViewDt.PerformCallback('refresh');
//            cbpViewDt2.PerformCallback('refresh');
//            cbpViewDt3.PerformCallback('refresh');
//            cbpViewDt4.PerformCallback('refresh');
//            cbpViewDt5.PerformCallback('refresh');
//            cbpViewDt6.PerformCallback('refresh');
//            cbpViewDt7.PerformCallback('refresh');
//            cbpViewDt8.PerformCallback('refresh');
        }

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

        //#region Asesmen Pra Anestesi
        $('.imgViewPreAnesthesyAssessment.imgLink').die('click');
        $('.imgViewPreAnesthesyAssessment.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPreAnesthesyAssesmentCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            var param = visitID + '|' + testorderID + '|' + recordID;
            openUserControlPopup(url, param, "Asesmen Pra Anestesi", 1024, 500);
        });
        //#endregion

        //#region Status Anestesi
        $('.imgViewAnesthesyStatus.imgLink').die('click');
        $('.imgViewAnesthesyStatus.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewAnesthesyStatusCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            var testorderNo = $(this).attr('testorderNo');
            var param = visitID + '|' + testorderID + '|' + recordID + '|' + testorderNo;
            openUserControlPopup(url, param, "Status Anestesi", 1024, 500);
        });
        //#endregion

        //#region Surgery Safety Check List
        $('.imgViewSignIn.imgLink').die('click');
        $('.imgViewSignIn.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgerySignInCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signInDate = $(this).attr('signInDate');
            var signInTime = $(this).attr('signInTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + signInDate + "|" + signInTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN IN", 700, 500);
        });

        $('.imgViewTimeOut.imgLink').die('click');
        $('.imgViewTimeOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgeryTimeOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var timeOutDate = $(this).attr('timeOutDate');
            var timeOutTime = $(this).attr('timeOutTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + timeOutDate + "|" + timeOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - TIME OUT", 700, 500);
        });

        $('.imgViewSignOut.imgLink').die('click');
        $('.imgViewSignOut.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewSurgerySignOutCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var signOutDate = $(this).attr('signOutDate');
            var signOutTime = $(this).attr('signOutTime');
            var paramedicName = $(this).attr('paramedicName');
            var physicianName = $(this).attr('physicianName');
            var anesthetistName = $(this).attr('anesthetistName');
            var nurseName = $(this).attr('nurseName');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + signOutDate + "|" + signOutTime + "|" + paramedicName + "|" + physicianName + "|" + anesthetistName + "|" + nurseName + "|" + layout + "|" + values;
            openUserControlPopup(url, param, "Surgical Safety Check List - SIGN OUT", 700, 500);
        });
        //#endregion

        //#region Asuhan Keperawatan Perioperatif
        $('.imgViewPreOperative.imgLink').die('click');
        $('.imgViewPreOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingPreOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('preOperativeDate');
            var time = $(this).attr('preOperativeTime');
            var paramedicID1 = $(this).attr('preoperativeSurgeryNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var paramedicID2 = $(this).attr('preoperativeWardNurseID');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - PRE OPERATIVE", 700, 500);
        });

        $('.imgViewIntraOperative.imgLink').die('click');
        $('.imgViewIntraOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingIntraOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('intraOperativeDate');
            var time = $(this).attr('intraOperativeTime');
            var paramedicID1 = $(this).attr('intraOperativeCircularNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var paramedicID2 = $(this).attr('intraOperativeRecoveryRoomNurseID');
            var paramedicID3 = $(this).attr('intraOperativeInstrumentNurseID');
            var nurseName3 = $(this).attr('nurseName3');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + nurseName3 + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - INTRA OPERATIVE", 700, 500);
        });

        $('.imgViewPostOperative.imgLink').die('click');
        $('.imgViewPostOperative.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/OperatingRoom/Assessment/ViewNursingPostOperativeCtl.ascx");
            var recordID = $(this).attr('recordID');
            var testOrderID = $(this).attr('testOrderID');
            var date = $(this).attr('postOperativeDate');
            var time = $(this).attr('postOperativeTime');
            var paramedicID1 = $(this).attr('postOperativeRecoveryRoomNurseID');
            var nurseName1 = $(this).attr('nurseName1');
            var nurseName2 = $(this).attr('nurseName2');
            var paramedicID2 = $(this).attr('postOperativeWardNurseID');


            var remarks = $(this).attr('remarks');
            var layout = $(this).attr('layout');
            var values = $(this).attr('values');
            var param = recordID + "|" + testOrderID + "|" + date + "|" + time + "|" + nurseName1 + "|" + nurseName2 + "|" + "|" + "|" + layout + "|" + values + "|" + remarks;
            openUserControlPopup(url, param, "Asuhan Keperawatan Perioperatif - POST OPERATIVE", 700, 500);
        });
        //#endregion

        //#region Laporan Operasi
        $('.imgViewSurgeryReport.imgLink').die('click');
        $('.imgViewSurgeryReport.imgLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewSurgeryReportCtl1.ascx");
            var recordID = $(this).attr('recordID');
            var visitID = $(this).attr('visitID');
            var testorderID = $(this).attr('testorderID');
            var testorderNo = $(this).attr('testorderNo');
            var param = visitID + '|' + testorderID + '|' + recordID + '|' + testorderNo;
            openUserControlPopup(url, param, "Laporan Operasi", 1024, 500);
        });
        //#endregion

        //#region e-Document
        $('.lnkViewDocument a').die('click');
        $('.lnkViewDocument a').live('click', function () {
            var fileName = $(this).closest('tr').find('.fileName').html();
            var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
            window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
        });
        //#endregion


        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshDocumentGrid() {
            //cbpViewDt5.PerformCallback('refresh');
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

        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount1, function (page) {
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
                $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();
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

        function onCbpViewDt7EndCallback(s) {
            $('#containerImgLoadingViewDt7').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt7.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt7"), pageCount1, function (page) {
                    cbpViewDt7.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt7.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt8EndCallback(s) {
            $('#containerImgLoadingViewDt8').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt8.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt8"), pageCount1, function (page) {
                    cbpViewDt8.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt8.ClientID %> tr:eq(1)').click();
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

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("contentDetail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
        
        #contentDetailNavPane > a       { margin:0; font-size:11px}
        #contentDetailNavPane > a.selected { color:#fff!important;background-color:#f44336!important }                  
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitIDCBCtl" runat="server" />
    <input type="hidden" id="hdnMRNCBCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationIDCBCtl" runat="server" />
    <input type="hidden" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" id="hdnOperatingRoomIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnID" value="0" />
    <input type="hidden" runat="server" id="hdnSurgeryReportID" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnOrderNo" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevised" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevision" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative; width: 100%">
                    <table border="0" cellpadding="0" cellspacing="1">
                        <colgroup>
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Filter")%></label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                    <asp:ListItem Text=" Perawatan saat ini " Value="1" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1">
                                </table>
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="EncounterID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="EncounterVisitNo" HeaderStyle-CssClass="hiddenColumn orderNo"
                                                            ItemStyle-CssClass="hiddenColumn orderNo" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Tanggal ") %>
                                                                    -
                                                                    <%=GetLabel("Jam Kunjungan") %>, <span style="color: blue">
                                                                        <%=GetLabel("No. Kunjungan") %></span></div>
                                                                <div style="font-weight: bold">
                                                                    <%=GetLabel("Dokter") %></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <%#: Eval("EncounterStartDate")%> - <%#: Eval("EncounterStartTime") %>, <span>
                                                                                    <%#: Eval("EncounterVisitNo")%></span></div>
                                                                            <div style="font-weight: bold;color: blue">
                                                                                <%#: Eval("EncounterRegisteredPhysician") %></div>
                                                                            <div style="font-style: italic; color: Red" class="blink"></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada informasi kunjungan SATUSEHAT untuk pasien ini")%>
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
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="diagnosisList">
                                        <%=GetLabel("Diagnosa")%></li>
<%--                                    <li class="selected" contentid="preAnesthesyAssessment">
                                        <%=GetLabel("Diagnosa")%></li>
                                    <li contentid="anesthesyStatus">
                                        <%=GetLabel("Status Anestesi")%></li>
                                    <li contentid="preSurgicalSafetyChecklist">
                                        <%=GetLabel("Surgical Safety Checklist")%></li>
                                    <li contentid="periOperative">
                                        <%=GetLabel("Perioperatif")%></li>
                                    <li contentid="surgeryReport">
                                        <%=GetLabel("Laporan Operasi")%></li>
                                    <li contentid="patientMedicalDevice">
                                        <%=GetLabel("Pemasangan Implant")%></li>
                                    <li contentid="surgeryDocument">
                                        <%=GetLabel("e-Document")%></li>--%>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="diagnosisList">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreSurgeryAssessment imgLink" title='<%=GetLabel("Lihat")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DiagnosisCode" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Rank" DataField="Rank" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                                        <asp:BoundField HeaderText="Kode Diagnosa" DataField="DiagnosisCode" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                                        <asp:BoundField HeaderText="Nama Diagnosa" DataField="DiagnosisName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink">
                                                                <%=GetLabel("Belum ada Informasi Diagnosa untuk kunjungan pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preAnesthesyAssessment" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt7" runat="server" Width="100%" ClientInstanceName="cbpViewDt7"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt7_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt7').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt7EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent8" runat="server">
                                            <asp:Panel runat="server" ID="Panel7" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt7" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreAnesthesyAssessment imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PreAnesthesyAssessmentID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>"/>
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
                                                        <asp:BoundField HeaderText="ASA"  DataField="cfASAStatus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Teknik Anestesi"  DataField="cfAnesthesiaType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada Asesmen Pra Anestesi untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt7">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt7"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="anesthesyStatus" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt8" runat="server" Width="100%" ClientInstanceName="cbpViewDt8"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt8_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt8').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt8EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent9" runat="server">
                                            <asp:Panel runat="server" ID="Panel8" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt8" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewAnesthesyStatus imgLink" title='<%=GetLabel("Lihat Asesmen Pra Anestesi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("AnesthesyStatusID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" testorderno="<%#:Eval("TestOrderNo") %>"/>
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
                                                                <input type="hidden" value="<%#:Eval("AssessmentTime") %>" bindingfield="AssessmentTime" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfAssessmentDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="AssessmentTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada Status Anestesi untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt8">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt8">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="preSurgicalSafetyChecklist" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="SIGN IN">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewSignIn imgLink" title='<%=GetLabel("Lihat Sign In")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                layout="<%#:Eval("SignInLayout") %>" values="<%#:Eval("SignInValues") %>" paramedicname="<%#:Eval("SignInParamedicName") %>"
                                                                                physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                nursename="<%#:Eval("SignOutNurseName") %>" signindate="<%#:Eval("cfSignInDate") %>"
                                                                                signintime="<%#:Eval("SignInTime") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfSignInDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("SignInParamedicName") %></div>
                                                                            <div <%# Eval("cfIsSignInInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Sign In Surgical Safety Check List untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="TIME OUT">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewTimeOut imgLink" title='<%=GetLabel("Lihat Time Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                layout="<%#:Eval("TimeOutLayout") %>" values="<%#:Eval("TimeOutValues") %>" paramedicname="<%#:Eval("TimeOutParamedicName") %>"
                                                                                physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                nursename="<%#:Eval("SignOutNurseName") %>" timeoutdate="<%#:Eval("cfTimeOutDate") %>"
                                                                                timeouttime="<%#:Eval("TimeOutTime") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfTimeOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("TimeOutParamedicName") %></div>
                                                                            <div <%# Eval("cfIsTimeOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Time Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="SIGN OUT">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewSignOut imgLink" title='<%=GetLabel("Lihat Sign Out")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                layout="<%#:Eval("SignOutLayout") %>" values="<%#:Eval("SignOutValues") %>" paramedicname="<%#:Eval("SignInParamedicName") %>"
                                                                                physicianname="<%#:Eval("SignOutPhysicianName") %>" anesthetistname="<%#:Eval("SignOutAnesthetistName") %>"
                                                                                nursename="<%#:Eval("SignOutNurseName") %>" signoutdate="<%#:Eval("cfSignOutDate") %>"
                                                                                signouttime="<%#:Eval("SignOutTime") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfSignOutDateTime") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("SignOutNurseName") %></div>
                                                                            <div <%# Eval("cfIsSignOutInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Sign Out Surgical Safety Check List untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada formulir Surgical Safety Check List untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
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
                            <div class="containerOrderDt" id="periOperative" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt6" runat="server" Width="100%" ClientInstanceName="cbpViewDt6"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt6_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt6').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt6EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent7" runat="server">
                                            <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt6" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="PRE OPERATIVE">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPreOperative imgLink" title='<%=GetLabel("Lihat Pre Operative")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" layout="<%#:Eval("PreOperativeLayout") %>"
                                                                                values="<%#:Eval("PreOperativeValues") %>" preoperativewardnurseid="<%#:Eval("PreoperativeWardNurseID") %>"
                                                                                preoperativesurgerynurseid="<%#:Eval("PreOperativeSurgeryNurseID") %>" nursename1="<%#:Eval("PreOperativeSurgeryNurseName") %>"
                                                                                nursename2="<%#:Eval("PreOperativeWardNurseName") %>" preoperativedate="<%#:Eval("cfPreOperativeDate") %>"
                                                                                preoperativetime="<%#:Eval("PreOperativeTime") %>" remarks="<%#:Eval("PreOperativeRemarks") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfPreOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("PreOperativeSurgeryNurseName") %></div>
                                                                            <div <%# Eval("cfIsPreOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Pre Operasi untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="INTRA OPERASI">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewIntraOperative imgLink" title='<%=GetLabel("Lihat Intra Operasi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" layout="<%#:Eval("IntraOperativeLayout") %>"
                                                                                values="<%#:Eval("IntraOperativeValues") %>" intraoperativecircularnurseid="<%#:Eval("IntraOperativeCircularNurseID") %>"
                                                                                intraoperativerecoveryroomnurseid="<%#:Eval("IntraOperativeRecoveryRoomNurseID") %>" intraoperativeInstrumentNurseid="<%#:Eval("IntraOperativeInstrumentNurseID") %>"
                                                                                nursename1="<%#:Eval("IntraOperativeCircularNurseName") %>" nursename2="<%#:Eval("IntraOperativeRecoveryRoomNurseName") %>" nursename3="<%#:Eval("IntraOperativeInstrumentNurseName") %>"
                                                                                intraoperativedate="<%#:Eval("cfIntraOperativeDate") %>" intraoperativetime="<%#:Eval("IntraOperativeTime") %>"
                                                                                remarks="<%#:Eval("IntraOperativeRemarks") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfIntraOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("IntraOperativeRecoveryRoomNurseName") %></div>
                                                                            <div <%# Eval("cfIsIntraOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Intra Operasi untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="220px"
                                                            HeaderText="PASKA OPERASI">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <colgroup>
                                                                        <col style="width: 60px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewPostOperative imgLink" title='<%=GetLabel("Lihat Paska Operasi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("ID") %>"
                                                                                testorderid="<%#:Eval("TestOrderID") %>" layout="<%#:Eval("PostOperativeLayout") %>"
                                                                                values="<%#:Eval("PostOperativeValues") %>" postoperativerecoveryroomnurseid="<%#:Eval("PostOperativeRecoveryRoomNurseID") %>"
                                                                                postoperativerecoveryroomnurseid="<%#:Eval("PostOperativeRecoveryRoomNurseID") %>"
                                                                                nursename1="<%#:Eval("PostOperativeWardNurseName") %>" nursename2="<%#:Eval("PostOperativeRecoveryRoomNurseName") %>"
                                                                                postoperativedate="<%#:Eval("cfPostOperativeDate") %>" postoperativetime="<%#:Eval("PostOperativeTime") %>"
                                                                                remarks="<%#:Eval("PostOperativeRemarks") %>" />
                                                                        </td>
                                                                        <td>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("cfPostOperativeDateTime") %></div>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "True" ? "":"Style='display:none'" %>>
                                                                                <%#:Eval("PostOperativeRecoveryRoomNurseName") %></div>
                                                                            <div <%# Eval("cfIsPostOperativeInfoAvailable").ToString() == "False" ? "":"Style='display:none'" %>>
                                                                                <div>
                                                                                    <%=GetLabel("Belum ada informasi Paska Operasi untuk pasien ini") %></div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada Asuhan Keperawatan Perioperatif untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt6">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt6">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="surgeryReport" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgViewSurgeryReport imgLink" title='<%=GetLabel("Lihat Hasil Operasi")%>'
                                                                                src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>' alt="" recordid="<%#:Eval("PatientSurgeryID") %>"
                                                                                visitid="<%#:Eval("VisitID") %>" testorderid="<%#:Eval("TestOrderID") %>" testorderno="<%#:Eval("TestOrderNo") %>" />
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
                                                        <asp:BoundField HeaderText="Tanggal" DataField="cfReportDate" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" />
                                                        <asp:BoundField HeaderText="Jam" DataField="ReportTime" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                        <asp:BoundField HeaderText="Dokter" DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" />
                                                        <asp:BoundField HeaderText="Pre Diagnosis" DataField="PreOperativeDiagnosisText"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Post Diagnosis" DataField="PostOperativeDiagnosisText"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada laporan operasi untuk pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt4">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="containerOrderDt" id="patientMedicalDevice" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfImplantDate" HeaderText="Tanggal Pemasangan" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="Nama Implant" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" HeaderStyle-Width="200px"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada informasi pemasangan implant untuk tindakan operasi di pasien ini") %></div>
                                                        </div>
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
                            <div class="containerOrderDt" id="surgeryDocument" style="display: none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn"
                                                            ItemStyle-CssClass="hiddenColumn fileName" />
                                                        <asp:HyperLinkField HeaderText=" " Text="Open" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div>
                                                                <%=GetLabel("Belum ada informasi e=document untuk tindakan operasi di pasien ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt5">
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
</asp:Content>
