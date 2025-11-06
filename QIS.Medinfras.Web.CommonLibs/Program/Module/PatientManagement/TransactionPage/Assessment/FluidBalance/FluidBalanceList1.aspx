<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="FluidBalanceList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.FluidBalanceList1" %>

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
    <script id="dxis_IPInitialAssessmentListctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_IPInitialAssessmentListctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>

    <script type="text/javascript" id="dxss_IPInitialAssessmentList">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function (evt) {
                onRefreshControl();
            });

            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVisitID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnLogDate.ClientID %>').val($(this).find('.cfLogDate1').html());
                $('#<%=hdnOrderNo.ClientID %>').val($(this).find('.orderNo').html());
                 
                if ($('#<%=hdnLogDate.ClientID %>').val() != "") {
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                    cbpViewDt5.PerformCallback('refresh');
                }
            });

            $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnGCFluidType.ClientID %>').val($(this).find('.gcFluidType').html());
                $('#<%=hdnFluidName.ClientID %>').val($(this).find('.fluidName').html());
                $('#<%=hdnLogDateDt.ClientID %>').val($(this).find('.logDateDt').html());
                
                $(this).addClass('selected');
                $('#<%=hdnVisitID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt4.PerformCallback('refresh');
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

        //#region Summary : Action Button
        $('#lblAddIntake').die('click');
        $('#lblAddIntake').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
                var param = "0|" + "init" + "|||1|1||";
                openUserControlPopup(url, param, "Intake (Inisialisasi)", 700, 500);
            }
        });

        $('#lblAddIntake2').die('click');
        $('#lblAddIntake2').live('click', function (evt) {
           
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
              
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
                var param = "0|" + "init" + "|||2|1||";
                openUserControlPopup(url, param, "Intake Yang Tidak Diukur", 700, 500);
            }
        });

        $('#lblAddOutput').die('click');
        $('#lblAddOutput').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
                var param = "0|" + "1";
                openUserControlPopup(url, param, "Output", 700, 500);
            }
        });

        $('#lblAddOutput2').die('click');
        $('#lblAddOutput2').live('click', function (evt) {
          
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
                var param = "0|" + "2";
                openUserControlPopup(url, param, "Output", 700, 500);
            }
        });

        $('.lblIntakeSummary.lblLink').die('click');
        $('.lblIntakeSummary.lblLink').live('click', function () {
              
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^01';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblIntake2Summary.lblLink').die('click');
        $('.lblIntake2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^04';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutputSummary.lblLink').die('click');
        $('.lblOutputSummary.lblLink').live('click', function () {
            var group = 'X459^02';
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceSummaryInfoCtl.ascx");
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutput2Summary.lblLink').die('click');
        $('.lblOutput2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^03';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });
        //#endregion

        //#region IVTheraphyNote : Action Button   
        $('.imgAddIVNote.imgLink').die('click');
        $('.imgAddIVNote.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/IVTheraphyNoteEntry.ascx");
                var param = "0";
                openUserControlPopup(url, param, "Catatan Program Infus (IV)", 700, 500);
            }
        });

        $('.imgEditIVNote.imgLink').die('click');
        $('.imgEditIVNote.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/IVTheraphyNoteEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID;
            openUserControlPopup(url, param, "Catatan Program Infus (IV)", 700, 500);
        });

        $('.imgDeleteIVNote.imgLink').die('click');
        $('.imgDeleteIVNote.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi catatan program terapi infus untuk pasien ini ?";
            displayConfirmationMessageBox("Intake", message, function (result) {
                if (result) {
                    cbpDeleteIVNote.PerformCallback(recordID);
                }
            });
        });
        //#endregion

        //#region Intake : Action Button        
        $('.imgAddInitiateIntake.imgLink').die('click');
        $('.imgAddInitiateIntake.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var logDate = $('#<%=hdnLogDate.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
                var param = "0|" + "init" + "|||1|1|" + logDate;
                openUserControlPopup(url, param, "Intake (Inisialisasi)", 700, 500);
            }
        });

        $('.imgAddIntake.imgLink').die('click');
        $('.imgAddIntake.imgLink').live('click', function (evt) {

            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var logDate = $('#<%=hdnLogDate.ClientID %>').val();
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
                var param = "0|" + "" + "|" + $('#<%=hdnGCFluidType.ClientID %>').val() + "|" + $('#<%=hdnFluidName.ClientID %>').val() + "|1|1|" + logDate;
                openUserControlPopup(url, param, "Intake", 700, 500);
            }
        });

        $('.imgEditInitiateIntake.imgLink').die('click');
        $('.imgEditInitiateIntake.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID + "|" + "init|||1||";
            openUserControlPopup(url, param, "Intake", 700, 500);
        });

        $('.imgEditIntake.imgLink').die('click');
        $('.imgEditIntake.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID + "||" + $('#<%=hdnGCFluidType.ClientID %>').val() + "|" + $('#<%=hdnFluidName.ClientID %>').val() + "|1||";
            openUserControlPopup(url, param, "Intake", 700, 500);
        });

        $('.imgEditIntake2.imgLink').die('click');
        $('.imgEditIntake2.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID + "||" + $('#<%=hdnGCFluidType.ClientID %>').val() + "|" + $('#<%=hdnFluidName.ClientID %>').val() + "|2||";
            openUserControlPopup(url, param, "Intake", 700, 500);
        });

        $('.imgDeleteIntake.imgLink').die('click');
        $('.imgDeleteIntake.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Intake untuk pasien ini ?";
            displayConfirmationMessageBox("Intake", message, function (result) {
                if (result) {
                    cbpDeleteFluidBalance.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgDeleteInitiateIntake.imgLink').die('click');
        $('.imgDeleteInitiateIntake.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var fluidName = $(this).attr('fluidName');
            var logDate = $(this).attr('logDate');
            var fluidGroup = $(this).attr('fluidGroup');
            var visitID = $(this).attr('visitID');
            var message = "Hapus informasi Intake untuk pasien ini ?";
            Methods.getObject('GetFluidBalanceList', "VisitID = " + visitID + " AND LogDate = '" + logDate + "' AND GCFluidGroup = '" + fluidGroup + "' AND FluidName = '" + fluidName + "' AND IsInitializeIntake = 0 AND IsDeleted = 0", function (resultCheck) {
                if (resultCheck != null) {
                    displayConfirmationMessageBox("Intake", "Ada intake untuk <b>" + fluidName + "</b>, apakah anda akan menghapus data detail intake-nya?", function (resultConfirm) {
                        if (resultConfirm) {
                            cbpDeleteFluidBalance.PerformCallback("1" + "|" + recordID);
                        }
                    });
                }
                else {
                    displayConfirmationMessageBox("Intake", message, function (result) {
                        if (result) {
                            cbpDeleteFluidBalance.PerformCallback("1" + "|" + recordID);
                        }
                    });
                }
            });
        });

        $('.imgDeleteIntake2.imgLink').die('click');
        $('.imgDeleteIntake2.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Intake yang tidak diukur untuk pasien ini ?";
            displayConfirmationMessageBox("Intake yang tidak diukur", message, function (result) {
                if (result) {
                    cbpDeleteFluidBalance.PerformCallback("1" + "|" + recordID);
                }
            });
        });

        $('.imgAddIntake2.imgLink').die('click');
        $('.imgAddIntake2.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var hdnLogDate = $('#<%=hdnLogDate.ClientID %>').val();
                 
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
                var param = "0||||2|1|" + hdnLogDate;
                openUserControlPopup(url, param, "Intake Yang Tidak Diukur", 700, 500);
            }
        });

        $('.imgCopyIntake.imgLink').die('click');
        $('.imgCopyIntake.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceIntakeEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var hdnLogDate = $('#<%=hdnLogDate.ClientID %>').val();
            var param = recordID + "||" + $('#<%=hdnGCFluidType.ClientID %>').val() + "|" + $('#<%=hdnFluidName.ClientID %>').val() + "|1|1|" + hdnLogDate;
            openUserControlPopup(url, param, "Intake", 700, 500);
        });

        //#endregion

        //#region Output : Action Button
        $('.imgAddOutput.imgLink').die('click');
        $('.imgAddOutput.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
                var param = "0|" + "1";
                openUserControlPopup(url, param, "Output", 700, 500);
            }
        });
        $('.imgEditOutput.imgLink').die('click');
        $('.imgEditOutput.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID + "|" + "1";
            openUserControlPopup(url, param, "Output", 700, 500);
        });
        $('.imgDeleteOutput.imgLink').die('click');
        $('.imgDeleteOutput.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Output untuk pasien ini ?";
            displayConfirmationMessageBox("Output", message, function (result) {
                if (result) {
                    cbpDeleteFluidBalance.PerformCallback("2" + "|" + recordID);
                }
            });
        });
        //#endregion

        //#region Output 2 : Action Button
        $('.imgAddOutput2.imgLink').die('click');
        $('.imgAddOutput2.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
                var param = "0|" + "2";
                openUserControlPopup(url, param, "Output yang tidak diukur", 700, 500);
            }
        });
        $('.imgEditOutput2.imgLink').die('click');
        $('.imgEditOutput2.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/PatientManagement/TransactionPage/Assessment/FluidBalance/FluidBalanceOutputEntry.ascx");
            var recordID = $(this).attr('recordID');
            var logTime = $(this).attr('logTime');
            var param = recordID + "|" + "2";
            openUserControlPopup(url, param, "Output yang tidak diukur", 700, 500);
        });
        $('.imgDeleteOutput2.imgLink').die('click');
        $('.imgDeleteOutput2.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var message = "Hapus informasi Output yang tidak diukur untuk pasien ini ?";
            displayConfirmationMessageBox("Output", message, function (result) {
                if (result) {
                    cbpDeleteFluidBalance.PerformCallback("3" + "|" + recordID);
                }
            });
        });
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
            cbpView6.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            onRefreshControl();
        }

        function onAfterSaveAddRecordEntryPopup() {
            onRefreshControl();
        }

        function onAfterSaveEditRecordEntryPopup() {
            onRefreshControl();
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
                var pageCount2 = parseInt(param[1]);

                if (pageCount2 > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount2, function (page) {
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

        function onCbpViewDt5EndCallback(s) {
            $('#containerImgLoadingViewDt5').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount5 = parseInt(param[1]);

                if (pageCount5 > 0)
                    $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt5"), pageCount5, function (page) {
                    cbpViewDt5.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();
        }

        function onCbpView6EndCallback(s) {
            $('#containerImgLoadingView6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount6 = parseInt(param[1]);

                if (pageCount6 > 0)
                    $('#<%=grdView6.ClientID %> tr:eq(1)').click();

                setPaging($("#paging6"), pageCount6, function (page) {
                    cbpView6.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView6.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            return true;
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

        function onCbpDeleteFluidBalanceInfoEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Intake Output', param[1]);
            }
        }

        function onCbpDeleteIVNoteEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                cbpView6.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox('Intake', param[1]);
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
    <input type="hidden" runat="server" id="hdnID" value="0" />
    <input type="hidden" runat="server" id="hdnLogDate" value="0" />
    <input type="hidden" runat="server" id="hdnLogTime" value="0" />
    <input type="hidden" runat="server" id="hdnGCFluidType" value="0" />
    <input type="hidden" runat="server" id="hdnFluidName" value="0" />
    <input type="hidden" runat="server" id="hdnLogDateDt" value="" /> 
    <input type="hidden" id="hdnMRN" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleID" value="" />
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
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td style="vertical-align:top">
                            <div style="position: relative; width: 100%; padding-top:26px">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage5">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfLogDate1" HeaderStyle-CssClass="hiddenColumn cfLogDate1" ItemStyle-CssClass="hiddenColumn cfLogDate1" />
                                                          <asp:BoundField DataField="LogDate" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn logDateHD" />
                                                                  
                                                        <asp:BoundField HeaderText="TANGGAL"  DataField="cfLogDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" ItemStyle-CssClass="cfLogDate" />
                                                        <asp:TemplateField HeaderText="INTAKE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblIntakeSummary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalIntake", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="INTAKE TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblIntake2Summary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalIntake2", "{0:N}")%>
                                                                        </label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OUTPUT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblOutputSummary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalOutput1", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OUTPUT TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblOutput2Summary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalOutput2", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="BALANCE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div <%# Eval("cfIsDehydration").ToString() == "False" ? "Style='text-align: right; color: blue'":"Style='text-align: right; color: red'" %>>
                                                                    <label class="lblBalance" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("cfFluidBalance", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Belum ada informasi intake-output untuk pasien ini")%>
                                                        <br />
                                                        <span class="lblLink" id="lblAddIntake" style="margin-left:30px">
                                                            <%= GetLabel("+ Intake")%></span>
                                                        <span class="lblLink" id="lblAddIntake2" style="margin-left:30px">
                                                            <%= GetLabel("+ Intake Tidak Diukur")%></span>
                                                        <span class="lblLink" id="lblAddOutput" style="padding-left: 30px">
                                                            <%= GetLabel("+ Output")%></span>
                                                        <span class="lblLink" id="lblAddOutput2" style="padding-left: 30px">
                                                            <%= GetLabel("+ Output Tidak Diukur")%></span>
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
                    </tr>
                    <tr>
                        <td style="vertical-align:top">
                            <dxcp:ASPxCallbackPanel ID="cbpView6" runat="server" Width="100%" ClientInstanceName="cbpView6"
                                ShowLoadingPanel="false" OnCallback="cbpView6_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView6').show(); }"
                                    EndCallback="function(s,e){ onCbpView6EndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage6">
                                            <asp:GridView ID="grdView6" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <img class="imgAddIVNote imgLink" title='<%=GetLabel("+ Program Terapi IV")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                alt=""/>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <img class="imgEditIVNote imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                    </td>
                                                                    <td style="width: 1px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <img class="imgDeleteIVNote imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                    </td>
                                                                    <td style="width: 1px">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Tanggal"  DataField="cfIVTherapyNoteDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="LogDate" />
                                                    <asp:BoundField HeaderText="Jam"  DataField="IVTherapyNoteTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                    <asp:TemplateField HeaderText="Catatan Terapi Infus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <div style="height: 50px; overflow-y: auto;">
                                                                <%#Eval("IVTherapyNotes").ToString().Replace("\n","<br />")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div>
                                                        <div class="blink"><%=GetLabel("Belum ada informasi program infus untuk pasien ini") %></div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>    
                            <div class="imgLoadingGrdView" id="containerImgLoadingView6" >
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="paging6"></div>
                                </div>
                            </div> 
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">   
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="panIntake">
                                        <%=GetLabel("INTAKE")%></li>
                                    <li contentid="panIntake2">
                                        <%=GetLabel("INTAKE YANG TIDAK DIUKUR")%></li>
                                    <li contentid="panOutput1">
                                        <%=GetLabel("OUTPUT")%></li>
                                    <li contentid="panOutput2">
                                        <%=GetLabel("OUTPUT YANG TIDAK DIUKUR")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="panIntake">
                                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:45%" />
                                        <col style="width:55%" />
                                    </colgroup>
                                    <tr>
                                        <td style="vertical-align:top">
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
                                                                    <asp:BoundField DataField="GCFluidType" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn gcFluidType" />
                                                                    <asp:BoundField DataField="FluidName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fluidName" />
                                                                    <asp:BoundField DataField="LogDate" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn logDateDt" />
                                                                    
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddInitiateIntake imgLink" title='<%=GetLabel("+ Intake (Inisialiasi)")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt=""/>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditInitiateIntake imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteInitiateIntake imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" fluidName="<%#:Eval("FluidName") %>" logDate="<%#:Eval("cfLogDate") %>" fluidGroup="<%#:Eval("GCFluidGroup") %>" visitID="<%#:Eval("VisitID") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                    <asp:TemplateField  HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="font-weight:bold">Nama Cairan</div>
                                                                            <div>Jenis Cairan</div>
                                                                            <div style="font-style:italic">Tenaga Medis</div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div style="font-weight:bold"><%#:Eval("FluidName") %></div>
                                                                            <div><%#:Eval("FluidType") %></div>
                                                                            <div style="font-style:italic"><%#:Eval("ParamedicName") %></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Jumlah Inisiasi"  DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink"><%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
                                                                    </div>
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
                                        </td>
                                        <td style="vertical-align:top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddIntake imgLink" title='<%=GetLabel("+ Intake")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt=""/>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditIntake imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteIntake imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgCopyIntake imgLink" title='<%=GetLabel("Copy")%>' src='<%# ResolveUrl("~/Libs/Images/Button/copy.png")%>'
                                                                                            alt="" recordID = "<%#:Eval("ID") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                    <asp:BoundField HeaderText="Cairan Ada"  DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                                                    <asp:BoundField HeaderText="Jumlah"  DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                                    <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink"><%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
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
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="containerOrderDt" id="panIntake2" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddIntake2 imgLink" title='<%=GetLabel("+ Intake Tidak Diukur")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditIntake2 imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteIntake2 imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                        <asp:BoundField HeaderText="Jenis"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Frekuensi"  DataField="Frequency" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi intake yang tidak diukur pada tanggal ini") %></div>
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
                            <div class="containerOrderDt" id="panOutput1" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddOutput imgLink" title='<%=GetLabel("+ Output")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt=""/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditOutput imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>" />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteOutput imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" />
                                                        <asp:BoundField HeaderText="Jenis Cairan"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                                        <asp:BoundField HeaderText="Catatan"  DataField="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"/>
                                                        <asp:BoundField HeaderText="Jumlah"  DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi output pada tanggal ini") %></div>
                                                        </div>
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
                                        <div id="pagingDt2"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="panOutput2" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="60px" ItemStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <img class="imgAddOutput2 imgLink" title='<%=GetLabel("+ Output Tidak Diukur")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                    alt="" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <img class="imgEditOutput2 imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>"  />
                                                                        </td>
                                                                        <td style="width: 1px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <img class="imgDeleteOutput2 imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                alt="" recordID = "<%#:Eval("ID") %>" logTime =  "<%#:Eval("LogTime") %>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                        <asp:BoundField HeaderText="Jenis"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Frekuensi"  DataField="Frequency" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi output yang tidak diukur pada tanggal ini") %></div>
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
                        </td>
                    </tr>            
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteFluidBalance" runat="server" Width="100%" ClientInstanceName="cbpDeleteFluidBalance"
            ShowLoadingPanel="false" OnCallback="cbpDeleteFluidBalance_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteFluidBalanceInfoEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpDeleteIVNote" runat="server" Width="100%" ClientInstanceName="cbpDeleteIVNote"
            ShowLoadingPanel="false" OnCallback="cbpDeleteIVNote_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteIVNoteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
