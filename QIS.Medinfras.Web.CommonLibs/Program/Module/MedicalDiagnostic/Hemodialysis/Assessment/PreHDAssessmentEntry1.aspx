<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PreHDAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PreHDAssessmentEntry1" %>

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
    <li id="btnDiscardChanges" runat="server" crudmode="R">
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
            setDatePicker('<%=txtAsessmentDate.ClientID %>');
            $('#<%=txtAsessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtFirstHDDate.ClientID %>');
            $('#<%=txtFirstHDDate.ClientID %>').datepicker('option', 'maxDate', '0');

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
                    displayConfirmationMessageBox('BATAL PERUBAHAN', message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload(true);
                        }
                    });
                }
            });

            $('#<%=txtHDNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtFirstHDDate.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHFRNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHDFMDNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHemoperfusionNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtMachineNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtReuseNo.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtVolumePriming.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDialysateRemarks.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHDDuration.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHDFrequency.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtQB.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtQD.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtUFGoal.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtProgProfilingUF.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtProgProfilingNa.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHeparizationDosageInitiate.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHeparizationDosageCirculation.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHeparizationDosageContinues.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtHeparizationDosageIntermitten.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtWithoutHeparizationRemarks.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtDialysisBleach.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtLMWHRemarks.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            //#region Catatan Lain-lain
            $('#<%=txtAdditionalRemarks.ClientID %>').keydown(function () {
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

            $('#<%=optIsHeparization.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    ToggleHeparinizationControl("1");
                }
                else {
                    ToggleHeparinizationControl("0");
                }
            });

            $('#<%=optIsWithoutHeparization.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    ToggleHeparinizationControl("2");
                }
                else {
                    ToggleHeparinizationControl("0");
                }
            });

            $('#<%=optIsLMWH.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    ToggleHeparinizationControl("3");
                }
                else {
                    ToggleHeparinizationControl("0");
                }
            });

            function ToggleHeparinizationControl(mode) {
                switch (mode) {
                    case "1":
                        $('#<%=txtHeparizationDosageInitiate.ClientID %>').removeAttr("disabled");
                        $('#<%=txtHeparizationDosageCirculation.ClientID %>').removeAttr("disabled");
                        $('#<%=txtHeparizationDosageContinues.ClientID %>').removeAttr("disabled");
                        $('#<%=txtHeparizationDosageIntermitten.ClientID %>').removeAttr("disabled");

                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr('readonly', true);
                        $('#<%=chkIsDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        cboGCDialysisBleach.SetEnabled(false);

                        $('#<%=txtLMWHRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtLMWHRemarks.ClientID %>').attr('readonly', true);
                        break;
                    case "2":
                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').removeAttr("disabled");
                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').removeAttr('readonly', false);
                        $('#<%=chkIsDialysisBleach.ClientID %>').removeAttr("disabled");
                        $('#<%=txtDialysisBleach.ClientID %>').removeAttr("disabled");
                        cboGCDialysisBleach.SetEnabled(true);

                        $('#<%=txtHeparizationDosageInitiate.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageCirculation.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageContinues.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageIntermitten.ClientID %>').attr("disabled", "disabled");

                        $('#<%=txtLMWHRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtLMWHRemarks.ClientID %>').attr('readonly', true);
                        break;
                    case "3":
                        $('#<%=txtLMWHRemarks.ClientID %>').removeAttr("disabled");
                        $('#<%=txtLMWHRemarks.ClientID %>').removeAttr('readonly', false);

                        $('#<%=txtHeparizationDosageInitiate.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageCirculation.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageContinues.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageIntermitten.ClientID %>').attr("disabled", "disabled");

                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr('readonly', true);
                        $('#<%=chkIsDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        cboGCDialysisBleach.SetEnabled(false);

                        break;
                    default:
                        $('#<%=txtHeparizationDosageInitiate.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageCirculation.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageContinues.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtHeparizationDosageIntermitten.ClientID %>').attr("disabled", "disabled");

                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtWithoutHeparizationRemarks.ClientID %>').attr('readonly', true);
                        $('#<%=chkIsDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtDialysisBleach.ClientID %>').attr("disabled", "disabled");
                        cboGCDialysisBleach.SetEnabled(false);

                        $('#<%=txtLMWHRemarks.ClientID %>').attr("disabled", "disabled");
                        $('#<%=txtLMWHRemarks.ClientID %>').attr('readonly', true);
                        break;
                }
            }

            $('#leftPageNavPanel ul li').first().click();
            ToggleHeparinizationControl("0");
        });

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
                getPsychosocialFormValues();
                getEducationFormValues();
                getDischargePlanningFormValues();
                getAdditionalFormValues();
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
            var message = "Perubahan yang dilakukan terhadap kajian awal belum disimpan, disimpan ?";
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
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="0" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnSubjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" value="" id="hdnAssessmentText" runat="server" />
        <input type="hidden" value="" id="hdnPlanningText" runat="server" />
        <input type="hidden" value="" id="hdnInstructionText" runat="server" />

        <input type="hidden" runat="server" id="hdnPsychosocialLayout" value="" />
        <input type="hidden" runat="server" id="hdnPsychosocialValue" value="" />
        <input type="hidden" runat="server" id="hdnEducationLayout" value="" />
        <input type="hidden" runat="server" id="hdnEducationValue" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalLayout" value="" />
        <input type="hidden" runat="server" id="hdnAdditionalValue" value="" />

        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
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
                            <li contentID="divPage2" title="Heparinisasi" class="w3-hover-red">Heparinisasi</li>
                        </ul>     
                    </div> 
                </td>
                <td style="vertical-align:top">
                    <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 170px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtAsessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 40px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtAsessmentTime1" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAsessmentTime2" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server" style="display:none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penunjang Medis")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicalDiagnostic" ClientInstanceName="cboMedicalDiagnostic"
                                        runat="server" Width="350px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Perawat")%></label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("HD Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHDNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" />
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("HD Pertama Kali")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFirstHDDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("HFR Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHFRNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" />
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("HDFMD Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHDFMDNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Hemoperfusion Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHemoperfusionNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" />
                                </td>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nomor Mesin")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtMachineNo" Width="80px" runat="server" Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jenis Peresepan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboGCHDType" ClientInstanceName="cboGCHDType" Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Metode HD")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboGCHDMethod" ClientInstanceName="cboGCHDMethod" Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tipe Dialiser")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboGCHDMachineType" ClientInstanceName="cboGCHDMachineType" Width="100%">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 150px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Reuse Ke")%></label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtReuseNo" Width="80px" runat="server" Style="text-align: left" />
                                                        </td>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal">
                                                                <%=GetLabel("Volume Priming/Total Cel Volume")%></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtVolumePriming" Width="120px" runat="server" Style="text-align: left" />
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
                                        <%=GetLabel("Dialisat")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboGCDialysate" ClientInstanceName="cboGCDialysate" Width="140px">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <label class="lblNormal">
                                    <%=GetLabel("Catatan Dialisat")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDialysateRemarks" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Durasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHDDuration" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> jam
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Frekuensi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHDFrequency" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> x/minggu
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("QB")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQB" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> cc/menit
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("QD")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQD" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> cc/menit
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("UF Goal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUFGoal" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> cc
                                </td>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("UFR")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProgProfilingUF" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> ml/menit
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Prog. Profiling Na")%></label>
                                </td>
                                <td>
                                   <asp:TextBox ID="txtProgProfilingNa" Width="80px" CssClass="number" runat="server" Style="text-align: right" />
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 150px">

                                        </td>
                                        <td style="width: 150px">
                                        </td>
                                        <td class="tdLabel" style="width: 200px">
                                            <label class="lblNormal" id="lblFamilyRelation">
                                                <%=GetLabel("Hubungan dengan Pasien")%></label>
                                        </td>
                                        <td style="width: 130px">
                                            <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                Width="100%" />
                                        </td>
                                        <td style="padding-left:10px">
                                        </td>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan Lain-lain")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAdditionalRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="4">
                                    <asp:RadioButton GroupName="heparization" ID="optIsHeparization" runat="server" Text=" Heparinisasi" Checked="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="padding-left: 19px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Dosis Awal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeparizationDosageInitiate" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> iu
                                </td>
                                <td class="tdLabel" style="padding-left: 19px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Dosis Sirkulasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeparizationDosageCirculation" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> iu
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 19px">
                                    <label class="lblNormal" style="font-weight:bold;">
                                        <%=GetLabel("Dosis Maintenance :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="padding-left: 19px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Continous")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeparizationDosageContinues" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> iu/jam
                                </td>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Intermittent")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeparizationDosageIntermitten" Width="80px" CssClass="number" runat="server" Style="text-align: right" /> iu/jam
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:RadioButton GroupName="heparization" ID="optIsWithoutHeparization" runat="server" Text=" Tanpa Heparinisasi" Checked="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-left: 19px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penyebab Tanpa Heparinisasi")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtWithoutHeparizationRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                        Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td><asp:CheckBox ID="chkIsDialysisBleach" runat="server" Text=" Pembilasan NaCl 0.9%" Checked="false" /></td>
                                            <td><asp:TextBox ID="txtDialysisBleach" Width="80px" CssClass="number" runat="server" Style="text-align: right" /></td>
                                            <td> 
                                                <dxe:ASPxComboBox runat="server" ID="cboGCDialysisBleach" ClientInstanceName="cboGCDialysisBleach" Width="100px">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr                        
                            <tr>
                                <td valign="top">
                                    <asp:RadioButton GroupName="heparization" ID="optIsLMWH" runat="server" Text=" LMWH" Checked="false" />
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtLMWHRemarks" runat="server" TextMode="MultiLine" Rows="3"
                                        Width="100%" />
                                </td>
                            </tr
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
    </div>
</asp:Content>
