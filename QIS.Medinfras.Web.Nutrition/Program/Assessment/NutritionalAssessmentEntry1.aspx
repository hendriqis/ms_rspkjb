<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="NutritionalAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionalAssessmentEntry1" %>

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
            <%=GetLabel("Kembali ke List")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Simpan")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));


            $('#<%=txtServiceDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime1.ClientID %>').val() >= 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 24 && $('#<%=txtServiceTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime2.ClientID %>').val() >= 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60 && $('#<%=txtServiceTime2.ClientID %>').val().length == 2) {
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

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

            //#region Riwayat Gizi
            $('#<%=txtNutritionHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Riwayat Penyakit Sekarang
            $('#<%=txtAntropometricNotes.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Riwayat Penyakit Dahulu
            $('#<%=txtMedicalHistory.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Biokimia
            $('#<%=txtBiochemistryNotes.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Catatan Planning
            $('#<%=txtInterventionCollaboration.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Catatan Instruksi
            $('#<%=txtEvaluation.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });
            //#endregion

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').click(function () {
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                if (contentID == 'divPage2') {
                    cbpVitalSignView.PerformCallback('refresh');
                }

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

            //#region Vital Sign View
            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
            //#endregion

            $('#leftPageNavPanel ul li').first().click();
        });

        //#region Vital Sign
        var pageCountVitalSign = parseInt('<%=gridVitalSignPageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCountVitalSign, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        });

        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            if (onCheckAssessmentID() == "1") {
                onBeforeOpenTrxPopup();
                var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
                var id = $('#<%=hdnID.ClientID %>').val();
                var param = "0||" + visitNoteID + "||1|1|" + id;
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Tanda Vital & Indikator Lainnya", 700, 500);
            }
            else {
                displayMessageBox("WARNING", "Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });


        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            onBeforeOpenTrxPopup();
            var id = $('#<%=hdnID.ClientID %>').val();
            var visitNoteID = $('#<%=hdnPatientVisitNoteID.ClientID %>').val();
            var param = "0|" + $('#<%=hdnVitalSignRecordID.ClientID %>').val() + "|" + visitNoteID + "||1|1|" + id;
            openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/VitalSign/VitalSignEntry1Ctl.ascx", param, "Vital Sign & Indicator", 700, 500);
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

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                $('#<%=hdnID.ClientID %>').val(retval);
            }
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {

            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                if ($('#<%=txtNutritionHistory.ClientID %>').val() == '') {
                    {
                        displayErrorMessageBox('SAVE', 'Riwayat Gizi Tidak Boleh Kosong');
                    }
                }
                else {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsSaved.ClientID %>').val('1');
                }
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
                displayConfirmationMessageBox("SAVE", message, function (result) {
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
            displayConfirmationMessageBox("SAVE", message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion

        $('#lblMedicalHistory').die('click');
        $('#lblMedicalHistory').live('click', function (evt) {
            var param = "";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/PastMedicalLookupCtl1.ascx", param, "Riwayat Kunjungan", 700, 500);
        });

        function onAfterVisitHistoryLookUp(param) {
            var text = param.replace("&nbsp","");
            $('#<%=txtMedicalHistory.ClientID %>').val(text);
        }

        function onAfterCustomClickSuccessSetRecordID(retval) {
            if (retval != '') {
                if (retval.includes(';')) {
                    var pvnID = retval.split(";")[0];
                    var nutritionassID = retval.split(";")[1];
                    if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                        $('#<%=hdnPatientVisitNoteID.ClientID %>').val(pvnID);
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                        $('#<%=hdnIsSaved.ClientID %>').val('1');
                    }
                    if ($('#<%=hdnID.ClientID %>').val() == '' || $('#<%=hdnID.ClientID %>').val() == '0') {
                        $('#<%=hdnID.ClientID %>').val(nutritionassID);
                    }
                }
                else {
                    if ($('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '' || $('#<%=hdnPatientVisitNoteID.ClientID %>').val() == '0') {
                        $('#<%=hdnPatientVisitNoteID.ClientID %>').val(retval);
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                        $('#<%=hdnIsSaved.ClientID %>').val('1');
                    }
                }
            }
        }

        function onCheckAssessmentID() {
            if ($('#<%=hdnID.ClientID %>').val() == '' || $('#<%=hdnID.ClientID %>').val() == '0') {
                return "0";
            }
            else {
                return "1";
            }
        }

        $('#<%:lblNutritionProblem.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsNutritionDiagnosis = 1 AND IsDeleted = 0", function (value) {
                onTxtProblemChanged(value);
            });
        });

        function onTxtProblemChanged(value) {
            var filterExpression = " DiagnoseID = '" + value + "'";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtProblem.ClientID %>').val(result.DiagnoseID + ' - ' + result.DiagnoseName);
                }
                else {
                    $('#<%=txtProblem.ClientID %>').val('');
                }
            });
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="0" />
        <input type="hidden" runat="server" id="hdnRegistrationDate" value="00" />
        <input type="hidden" runat="server" id="hdnRegistrationTime" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" runat="server" id="hdnPatientEducationID" value="0" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnAssessmentParamedicID" value="0" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="" id="hdnAdimeAText" runat="server" />
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
        
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="vertical-align:top">
                        <div id="leftPageNavPanel" class="w3-border">
                            <ul>
                                <li contentID="divPage1" title="Riwayat Gizi" class="w3-hover-red">Riwayat Gizi</li>
                                <li contentID="divPage2" title="Antropometri" class="w3-hover-red">Antropometri</li>                 
                                <li contentID="divPage3" title="Biokimia" class="w3-hover-red">Biokimia</li>
                                <li contentID="divPage4" title="Klinik/Fisik" class="w3-hover-red">Klinik/Fisik</li>
                                <li contentID="divPage5" title="Riwayat Personal" class="w3-hover-red">Riwayat Personal</li>
                                <li contentID="divPage6" title="Diagnosa Gizi" class="w3-hover-red">Diagnosa Gizi</li>
                                <li contentID="divPage7" title="Intervensi Gizi" class="w3-hover-red">Intervensi Gizi</li>
                                <li contentID="divPage8" title="Monitoring dan Evaluasi" class="w3-hover-red">Monitoring dan Evaluasi</li>
                            </ul>     
                        </div> 
                    </td>
                    <td style="vertical-align:top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tanggal dan Waktu")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                                                <asp:TextBox ID="txtServiceTime1" Width="40px" CssClass="number" runat="server" Style="text-align: center"
                                                                    MaxLength="2" max="24" min="0" />
                                                            </td>
                                                            <td>
                                                                <label class="lblNormal" />
                                                                <%=GetLabel(":")%>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtServiceTime2" Width="40px" CssClass="number" runat="server" Style="text-align: center"
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
                                            <%=GetLabel("Ahli Gizi")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Riwayat Gizi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNutritionHistory" runat="server" TextMode="MultiLine" Rows="8"
                                            Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <td style="width: 130px">
                                                <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto Anamnesis" Checked="false" />
                                            </td>
                                            <td style="width: 130px">
                                                <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo Anamnesis" Checked="false" />
                                            </td>
                                            <td class="tdLabel" style="width: 180px">
                                                <label class="lblNormal" id="lblFamilyRelation">
                                                    <%=GetLabel("Hubungan dengan Pasien")%></label>
                                            </td>
                                            <td style="width: 130px">
                                                <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                    Width="100%" />
                                            </td>
                                            <td style="padding-left:10px">
                                                <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                                    Checked="false" />
                                            </td>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                                                ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage5">
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
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteVitalSign imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="text-align: right">
                                                                                <span class="lblLink" id="lblAddVitalSign">
                                                                                    <%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                            </div>
                                                                        </HeaderTemplate>
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
                                                                    <%=GetLabel("No Data To Display") %>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 150px; vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Antropometri")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAntropometricNotes" runat="server" Width="99%" TextMode="Multiline" Rows="6" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align:top">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                            <colgroup>
                                                <col width="150px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                   <label class="lblNormal" id="lblBiochemistryNotes">
                                                     <%=GetLabel("Biokimia") %>
                                                   </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBiochemistryNotes" runat="server" Width="98%" TextMode="Multiline"
                                                        Rows="10" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Klinik/Fisik") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicalNotes" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Riwayat Personal") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMedicalHistory" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage6" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblLink" id="lblNutritionProblem" runat="server">
                                            <%=GetLabel("Masalah") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProblem" runat="server" Width="98%" TextMode="Multiline" Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label8">
                                            <%=GetLabel("Etiologi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEtiology" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label9">
                                            <%=GetLabel("Symptom") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSymptom" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage7" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label3">
                                            <%=GetLabel("Tujuan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInterventionPurpose" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label5">
                                            <%=GetLabel("Pemberian/Penentuan Diet") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInterventionDiet" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label4">
                                            <%=GetLabel("Edukasi/Konseling") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInterventionEducation" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label2">
                                            <%=GetLabel("Kolaborasi/Rujukan Gizi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInterventionCollaboration" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label6">
                                            <%=GetLabel("Monitoring") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMonitoring" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal" id="Label1">
                                            <%=GetLabel("Evaluasi") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEvaluation" runat="server" Width="98%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked="false"/> <%:GetLabel("Konfirmasi Dokter")%>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server" Width="300px" >
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
            ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
