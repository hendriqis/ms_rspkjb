<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PostHDAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PostHDAssessmentEntry1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back to List")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Discard Changes")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            //#region Vital Sign View
            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
            //#endregion

            setDatePicker('<%=txtStartDate.ClientID %>');
            $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtEndDate.ClientID %>');
            $('#<%=txtEndDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    PromptUserToSave();
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Lanjutkan proses pembatalan perubahan yang sudah dilakukan ?";
                    displayConfirmationMessageBox("BATAL PERUBAHAN", message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload();
                        }
                    });
                }
            });

            //#region Catatan Lain-lain
            $('#<%=txtPostHDSymptoms.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            $('#<%=txtFinalUFG.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            $('#<%=txtPrimingBalance.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            $('#<%=txtWashOut.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                showContent(contentID);
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
            }
            //#endregion

            $('#btnRefreshOutput.imgLink').click(function () {
                cbpCalculateBalanceSummary.PerformCallback();
            });

            $('#btnRefreshIntake.imgLink').click(function () {
                cbpCalculateBalanceSummary.PerformCallback();
            });

            $('#leftPageNavPanel ul li').first().click();
        });

        $('#<%=txtFinalUFG.ClientID %>').live('change', function () {
            $(this).blur();
            calculateTotalOutput();
        });

        $('#<%=txtPrimingBalance.ClientID %>').live('change', function () {
            $(this).blur();
            calculateTotalIntake();
        });

        $('#<%=txtWashOut.ClientID %>').live('change', function () {
            $(this).blur();
            calculateTotalIntake();
        });

        function calculateTotalIntake() {
            var priming = parseFloat($('#<%=txtPrimingBalance.ClientID %>').val());
            var washout = parseFloat($('#<%=txtWashOut.ClientID %>').val());
            var intakeMonitoring = parseFloat($('#<%=txtTotalIntake.ClientID %>').val());
            var total = priming + washout + intakeMonitoring;
            $('#<%=txtCalculatedTotalIntake.ClientID %>').val(total);
            calculateTotalUF();
        }

        function calculateTotalOutput() {
            var finalUFG = parseFloat($('#<%=txtFinalUFG.ClientID %>').val());
            var outputMonitoring = parseFloat($('#<%=txtTotalOutput.ClientID %>').val());
            var total = finalUFG + outputMonitoring;
            $('#<%=txtCalculatedTotalOutput.ClientID %>').val(total);
            calculateTotalUF();
        }

        function calculateTotalUF() {
            var totalOutput = parseFloat($('#<%=txtCalculatedTotalOutput.ClientID %>').val());
            var totalIntake = parseFloat($('#<%=txtCalculatedTotalIntake.ClientID %>').val());
            var totalUF = totalOutput - totalIntake;
            $('#<%=txtTotalUF.ClientID %>').val(totalUF);
        }

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                $('#<%=hdnID.ClientID %>').val(retval);
            }
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        });


        //#region Change Page - Save
        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Perubahan yang dilakukan belum disimpan, Apakah perubahan tersebut disimpan ?";
                displayConfirmationMessageBox("SAVE",message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                gotoNextPage();
            }
        }
        function onBeforeBackToListPage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                PromptUserToSave();
            }
            else {
                backToPatientList();
            }
        }

        function PromptUserToSave() {
            var message = "Perubahan yang dilakukan belum disimpan, disimpan ?";
            displayConfirmationMessageBox("SAVE",message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }

        function onAfterCustomClickSuccessSetRecordID(param) {
            $('#<%=hdnID.ClientID %>').val(param);
        }

        function onCbpCalculateBalanceSummaryEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] != 'success') {
                displayErrorMessageBox('Calculate Fluid Balance', param[1]);
            }
            else {
                var valueInfo = param[1].split(';');
                $('#<%=txtCalculatedTotalIntake.ClientID %>').val(valueInfo[0]);
                $('#<%=txtCalculatedTotalOutput.ClientID %>').val(valueInfo[1]);
                var intake = parseFloat(valueInfo[0]);
                var output = parseFloat(valueInfo[1]);

                var finalUFG = parseFloat($('#<%=txtFinalUFG.ClientID %>').val());
                var outputMonitoring = parseFloat($('#<%=txtTotalOutput.ClientID %>').val());
                var totalOutput = finalUFG + output;
                $('#<%=txtCalculatedTotalOutput.ClientID %>').val(totalOutput);

                var priming = parseFloat($('#<%=txtPrimingBalance.ClientID %>').val());
                var washout = parseFloat($('#<%=txtWashOut.ClientID %>').val());
                var totalIntake = priming + washout + intake;
                $('#<%=txtCalculatedTotalIntake.ClientID %>').val(totalIntake);

                var totalUF = totalOutput - totalIntake;

                $('#<%=txtTotalOutput.ClientID %>').val(output);
                $('#<%=txtTotalIntake.ClientID %>').val(intake);
                $('#<%=txtTotalUF.ClientID %>').val(totalUF);
            }
        }
        //#region Vital Sign
        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = "0";
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = "0";
                var date = $('#<%=txtEndDate.ClientID %>').val();
                var time = $('#<%=txtEndTime1.ClientID %>').val() + ":" + $('#<%=txtEndTime2.ClientID %>').val();
                var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
                var remarks = "";
                var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
                var vitalSignID = "0";
                var param = Constant.VitalSignAssessmentType.POST_HEMODIALYSIS + "|" + assessmentID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID + "|" + healthcareServiceUnitID + "|" + vitalSignID;
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
                openUserControlPopup(url, param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("Asesmen Penunjang", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });


        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            var assessmentID = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = "0";
            onBeforeOpenTrxPopup();
            var testOrderID = "0";
            var date = $('#<%=txtEndDate.ClientID %>').val();
            var time = $('#<%=txtEndTime1.ClientID %>').val() + ":" + $('#<%=txtEndTime2.ClientID %>').val();
            var paramedicID = $(this).attr('preoperativeSurgeryNurseID');
            var remarks = $(this).attr('remarks');
            var healthcareServiceUnitID = $(this).attr('healthcareServiceUnitID');
            var vitalSignID = $('#<%=hdnVitalSignRecordID.ClientID %>').val();
            var param = Constant.VitalSignAssessmentType.POST_HEMODIALYSIS + "|" + assessmentID + "|" + testOrderID + "|" + date + "|" + time + "|" + paramedicID + "|" + healthcareServiceUnitID + "|" + vitalSignID;
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/VitalSign/ServiceUnitVitalSignEntry.ascx");
            openUserControlPopup(url, param, "Tanda Vital & Indikator Lainnya", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            displayConfirmationMessageBox("DELETE : Vital Sign", message, function (result) {
                if (result) {
                    cbpDeleteVitalSign.PerformCallback();
                }
            });
        });

        function onCbpVitalSignViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#vitalSignPaging"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }

        function onCbpVitalSignDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpVitalSignView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox("DELETE : Vital Sign", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }
        //#endregion
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnRegistrationDate" value="00" />
        <input type="hidden" runat="server" id="hdnRegistrationTime" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnEndTime1" value="00" />
        <input type="hidden" runat="server" id="hdnEndTime2" value="00" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="0" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 20%" />
                <col style="width: 80%" />
            </colgroup>
            <tr>
                <td style="vertical-align:top">
                    <div id="leftPageNavPanel" class="w3-border">
                        <ul>
                            <li contentID="divPage1" title="Data Umum" class="w3-hover-red">Data Umum</li>
                            <li contentID="divPage2" title="Tanda Vital dan Indikator Lainnya" class="w3-hover-red">Tanda Vital dan Indikator Lainnya</li>
                            <li contentID="divPage3" title="Keluhan Post HD" class="w3-hover-red">Keluhan Post HD</li>
                        </ul>     
                    </div> 
                </td>
                <td style="vertical-align:top">
                    <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 30px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu Mulai")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 30px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 30px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtStartTime1" Width="30px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStartTime2" Width="30px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu Selesai")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 30px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 30px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtEndTime1" Width="30px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEndTime2" Width="30px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Perawat")%></label>
                                </td>
                                <td colspan="4">
                                    <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("UFG Akhir HD")%></label>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtFinalUFG" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0" /> cc
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Output (Monitoring)")%></label>
                                </td>
                                <td>
                                    <img class="imgLink" id="btnRefreshOutput" title='<%=GetLabel("Refresh Output")%>' src='<%= ResolveUrl("~/Libs/Images/button/refresh.png")%>' alt="" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalOutput" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td />
                                <td />
                                <td class="tdLabel" style="text-align:right; font-weight:bold">
                                    <label class="lblNormal">
                                        <%=GetLabel("TOTAL OUTPUT")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCalculatedTotalOutput" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Sisa Priming")%></label>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtPrimingBalance" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0"/> cc
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Wash Out")%></label>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtWashOut" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0"/> cc
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Intake (Monitoring)")%></label>
                                </td>
                                <td>
                                    <img class="imgLink" id="btnRefreshIntake" title='<%=GetLabel("Refresh Intake")%>' src='<%= ResolveUrl("~/Libs/Images/button/refresh.png")%>' alt="" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalIntake" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                                 <td></td>
                                 <td></td>
                            </tr>
                            <tr>
                                <td />
                                <td></td>
                                <td class="tdLabel" style="text-align:right; font-weight:bold">
                                    <label class="lblNormal">
                                        <%=GetLabel("TOTAL INTAKE")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCalculatedTotalIntake" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="font-weight:bold">
                                    <label class="lblNormal">
                                        <%=GetLabel("TOTAL UF (OUTPUT - INTAKE)")%></label>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtTotalUF" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdVitalSignView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgEditVitalSign imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" />
                                                                </td>
                                                                <td style="width: 1px">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <img class="imgDeleteVitalSign imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" recordID = "<%#:Eval("ID") %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                            <br />
                                                            <span style="font-style:italic">
                                                                <%#: Eval("Remarks") %>
                                                            </span>
                                                            <br />
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 350px;">
                                                                        <strong>
                                                                            <div style="width: 110px; float: left;" class="labelColumn">
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
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                        <%=GetLabel("Belum ada pengkajian tanda vital untuk asesmen ini") %>
                                                    <br />
                                                    <span class="lblLink" id="lblAddVitalSign">
                                                        <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="vitalSignPaging">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col width="150px" />
                                            <col width="100px" />
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Keluhan Post HD")%></label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtPostHDSymptoms" runat="server" TextMode="MultiLine" Rows="15"
                                                    Width="99%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpCalculateBalanceSummary" runat="server" Width="100%" ClientInstanceName="cbpCalculateBalanceSummary"
            ShowLoadingPanel="false" OnCallback="cbpCalculateBalanceSummary_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCalculateBalanceSummaryEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
