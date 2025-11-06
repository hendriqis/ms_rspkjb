<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrxPatient1.master"
    AutoEventWireup="true" CodeBehind="EKlaimEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnEKlaimEntryBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnCreateNewClaim" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png") %>' alt="" />
        <div>
            <%=GetLabel("Klaim Baru") %></div>
    </li>
    <li id="btnSetClaimData" runat="server" crudmode="R" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Isi/Update Klaim") %></div>
    </li>
    <li id="btnGrouper1" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Grouper Stage1")%></div>
    </li>
    <li id="btnGrouper2" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Grouper Stage2")%></div>
    </li>
    <li id="btnFinalKlaim" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Final Klaim")%></div>
    </li>
    <li id="btnReeditKlaim" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Edit Klaim")%></div>
    </li>
    <li id="btnDeleteClaim" runat="server" crudmode="R" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png") %>' alt="" />
        <div>
            <%=GetLabel("Hapus Klaim") %></div>
    </li>
    <li id="btnUploadDocument" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Upload Document")%></div>
    </li>
    <li id="btnPrintKlaim" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cetak Klaim")%></div>
    </li>
    <li id="btnSendKlaimOnline" crudmode="R" runat="server" style="display: none;">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Kirim Klaim Online")%></div>
    </li>
    <li id="btnRefreshKlaim" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh Status Klaim")%></div>
    </li>
    <li id="btnMPEntryPopupPrintSEP">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Surat Eligibilitas Peserta")%></div>
    </li>
    <li id="btnViewEpisodeSummary" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("View Rekam Medis")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function isValidateNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        $(function () {
            setDatePicker('<%=txtClaimDiagnosisDate.ClientID %>');
            $('#<%=txtClaimDiagnosisDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtClaimProcedureDate.ClientID %>');
            $('#<%=txtClaimProcedureDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtVentilatorStartDate.ClientID %>');
            $('#<%=txtVentilatorStartDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtVentilatorEndDate.ClientID %>');
            $('#<%=txtVentilatorEndDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtDeliverySHKDate.ClientID %>');
            $('#<%=txtDeliverySHKDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        $('#<%=btnEKlaimEntryBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/BPJS/EKlaim/EKlaimList.aspx');
        });

        //#region Create New Claim
        $('#<%=btnCreateNewClaim.ClientID %>').live('click', function (evt) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            var IsMaapingTarifFailed = $('#<%=hdnIsMaapingTarifFailed.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                if (IsMaapingTarifFailed == "0") {
                    var messageBeforeCreateNewClaim = '<b>' + 'Proses membuat klaim baru.' + '</b>' + ' Lanjutkan ?';
                    showToastConfirmation(messageBeforeCreateNewClaim, function (resultConfirmCreateNewClaim) {
                        if (resultConfirmCreateNewClaim) {
                            cbpProcess.PerformCallback('newClaim');
                        }
                    });
                } else {
                    showToast('Failed', 'Masih terdapat item tagihan yang belum termaping dengan tarif incbgs');
                }
            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });
        //#endregion

        //#region Set Claim Data
        $('#<%=btnSetClaimData.ClientID %>').live('click', function (evt) {

            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                if (cboEKlaimCaraMasuk.GetValue() == null || cboEKlaimCaraMasuk.GetValue() == "") {
                    alert("Silahkan pilih cara masuk dahulu");
                    return false;
                } else if (cboEKlaimCaraPulang.GetValue() == null || cboEKlaimCaraPulang.GetValue() == "") {
                    alert("Silahkan pilih cara pulang dahulu");
                    return false;
                } else {
                    var messageBeforeSetClaimData = '<b>' + 'Proses mengisi/mengubah data detail klaim.' + '</b>' + ' Lanjutkan Proses ?';
                    showToastConfirmation(messageBeforeSetClaimData, function (resultConfirmSetClaimData) {
                        if (resultConfirmSetClaimData) {
                            cbpProcess.PerformCallback('updateClaim');
                        }
                    });
                }

            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });

        //#endregion

        //#region Delete Claim
        $('#<%=btnDeleteClaim.ClientID %>').live('click', function (evt) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                var messageBeforeDeleteClaim = '<b>' + 'Proses hapus klaim. Klaim yang sudah dihapus tidak bisa dikembalikan lagi.' + '</b>' + ' Lanjutkan Proses ?';
                showToastConfirmation(messageBeforeDeleteClaim, function (resultConfirmDeleteClaim) {
                    if (resultConfirmDeleteClaim) {
                        cbpProcess.PerformCallback('deleteClaim');
                    }
                });
            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });
        //#endregion

        $('#<%=btnUploadDocument.ClientID %>').live('click', function () {
            if ($('#<%=hdnRegistrationID.ClientID %>').val() != "") {
                onCustomButtonClick('upload');
            }
            else showToast('Warning', 'Pilih nomor registrasi terlebih dahulu.');
        });

        function oncboCaraBayarEKlaimValueChanged(s) {
            var oValue = cboCaraBayarEKlaim.GetValue();
            if (oValue == "X555^001") {
                $('#<%=trCOB.ClientID %>').removeAttr('style');
            } else {
                $('#<%:trCOB.ClientID %>').attr('style', 'display:none');
            }
        }

        //#region Diagnosa
        $('#<%=grdDiagnosaView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdDiagnosaView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnEntryID.ClientID %>').val($(this).find('.keyField').html());
        });

        function selectGridRowDiagnosa() {
            $('#<%=grdDiagnosaView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnEntryID.ClientID %>').val($(this).find('.keyField').html());
        }

        function onButtonCancelClick() {
            $('#<%=grdDiagnosaView.ClientID %> tr.selected').removeClass('selected');
        }

        function getSelectedRow() {
            return $('#<%=grdDiagnosaView.ClientID %> tr.selected');
        }

        function getSelectedProcedureRow() {
            return $('#<%=grdProcedureView.ClientID %> tr.selected');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        $('#lblDiagnosaAdd').live('click', function () {
            ResetForm();
            $("#formEntry").show();
            $('#<%=hdnEntryID.ClientID %>').val('');
        });

        $('#lblDiagnosaEdit').live('click', function () {
            $("#formEntry").hide();
            var idx = $(this).closest('tr').index();
            $('#<%=grdDiagnosaView.ClientID %> tr:eq(' + idx + ')').click();

            var evID = $(this).attr("data-val");
            var ID = $('#<%=hdnEntryID.ClientID %>').val(evID);
            if (ID != "") {
                $("#formEntry").show();
                var filterExpression = "ID=" + evID;
                Methods.getObject('GetvPatientDiagnosisList', filterExpression, function (result) {
                    if (result != null) {
                        entityToControl(result);
                    }
                    else {
                        ResetForm();
                    }
                });



            } else {
                showToast('Failed', 'Silahkan pilih terlebih dahulu diagnosa yang akan di edit.');
            }

        });

        $('#lblDiagnosaDelete').live('click', function () {
            var idx = $(this).closest('tr').index();
            $('#<%=grdDiagnosaView.ClientID %> tr:eq(' + idx + ')').click();

            var evID = $(this).attr("data-val");
            var ID = $('#<%=hdnEntryID.ClientID %>').val(evID);
            if (ID != "") {
                cbpDiagnosaView.PerformCallback('deletedClaimDiagnose');
            } else {
                showToast('Failed', 'Silahkan pilih terlebih dahulu diagnosa yang akan di edit.');
            }

        });

        function entityToControl(entity) {
            $('#<%=txtClaimDiagnosisDate.ClientID %>').focus();
            $('#<%=grdDiagnosaView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdDiagnosaView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val(entity.ClaimDiagnosisID);
                $('#<%=txtClaimDiagnosisName.ClientID %>').val(entity.ClaimDiagnosisName);
                $('#<%=txtClaimDiagnosisText.ClientID %>').val(entity.ClaimDiagnosisText);

                cboDiagnoseTypeClaim.SetValue(entity.GCDiagnoseType);
                cboDiagnoseTypeClaim.SetText(entity.DiagnoseType);

                $('#<%=txtv5DiagnosaID.ClientID %>').val(entity.ClaimDiagnosisID);
                $('#<%=txtv5DiagnosaName.ClientID %>').val(entity.ClaimDiagnosisName);
                $('#<%=txtv6DiagnosaID.ClientID %>').val(entity.ClaimINADiagnoseID);
                $('#<%=txtv6DiagnosaName.ClientID %>').val(entity.ClaimDiagnosisName);
            }
            else {
                ResetForm();
            }
        }

        function ResetForm() {
            $('#<%=hdnEntryID.ClientID %>').val('');
            cboDiagnoseTypeClaim.SetValue('<%=GetDefaultDiagnosisType() %>');

            $('#<%=txtClaimDiagnosisCode.ClientID %>').val("");
            $('#<%=txtClaimDiagnosisName.ClientID %>').val("");
            $('#<%=txtClaimDiagnosisText.ClientID %>').val("");

            $('#<%=txtv5DiagnosaID.ClientID %>').val("");
            $('#<%=txtv5DiagnosaName.ClientID %>').val("");
            $('#<%=txtv6DiagnosaID.ClientID %>').val("");
            $('#<%=txtv6DiagnosaName.ClientID %>').val("");

        }

        function getRowData($row) {
            if ($row.length > 0) {
                $('#divPatientPageEntryTitle').html('Edit');
                var selectedObj = {};
                $row.find('input[type=hidden]').each(function () {
                    selectedObj[$(this).attr('bindingfield')] = $(this).val();
                });
                return selectedObj;
            }
        }

        $('#btnCancelDiagnosa').live('click', function () {
            ResetForm();
            $("#formEntry").hide();
            $('#<%=hdnEntryID.ClientID %>').val('');
        });

        $('#btnSendDataDiagnosa').live('click', function () {

            cbpDiagnosaView.PerformCallback('saveClaimDiagnose');
        });

        function onGetClaimDiagnoseFilterExpression() {
            var filterExpression = "INACBGLabel != '' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblClaimDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnoseEklaim', onGetClaimDiagnoseFilterExpression(), function (value) {
                ontxtClaimDiagnosisCodeChanged(value);
            });
        });

        $('#<%=txtClaimDiagnosisCode.ClientID %>').live('change', function () {
            ontxtClaimDiagnosisCodeChanged($(this).val());
        });

        function ontxtClaimDiagnosisCodeChanged(value) {
            var filterExpression = onGetClaimDiagnoseFilterExpression() + " AND DiagnoseID = '" + value + "'";
            Methods.getObject('GetvDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtClaimDiagnosisCode.ClientID %>').val(result.DiagnoseID);
                    $('#<%=txtClaimDiagnosisName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=txtClaimDiagnosisText.ClientID %>').val(result.DiagnoseName);

                    $('#<%=txtv5DiagnosaID.ClientID %>').val(result.INACBGLabel);
                    $('#<%=txtv5DiagnosaName.ClientID %>').val(result.INACBGText);
                    $('#<%=txtv6DiagnosaID.ClientID %>').val(result.INACBGINALabel);
                    $('#<%=txtv6DiagnosaName.ClientID %>').val(result.INACBGINAText);

                }
                else {
                    $('#<%=txtClaimDiagnosisCode.ClientID %>').val('');
                    $('#<%=txtClaimDiagnosisName.ClientID %>').val('');
                    $('#<%=txtClaimDiagnosisText.ClientID %>').val('');

                    $('#<%=txtv5DiagnosaID.ClientID %>').val('');
                    $('#<%=txtv5DiagnosaName.ClientID %>').val('');
                    $('#<%=txtv6DiagnosaID.ClientID %>').val('');
                    $('#<%=txtv6DiagnosaName.ClientID %>').val('');
                }
            });
        }

        function onGetClaimDiagnoseINAFilterExpression() {
            var filterExpression = "GCBPJSObjectType = 'X358^018'";
            return filterExpression;
        }

        function getDiagnosaEKlaimFilterExpression() {
            var filterExpression = "BPJSCode IS NOT NULL";
            return filterExpression;
        }

        $('#lblEKlaimDiagnose.lblLink').live('click', function () {
            openSearchDialog('eklaimdiagnose', getDiagnosaEKlaimFilterExpression(), function (value) {
                $('#<%=txtClaimDiagnoseCode.ClientID %>').val(value);
                ontxtClaimDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtClaimDiagnoseCode.ClientID %>').live('change', function () {
            ontxtClaimDiagnoseCodeChanged($(this).val());
        });

        function ontxtClaimDiagnoseCodeChanged(value) {
            var filterExpression = getDiagnosaEKlaimFilterExpression() + " AND BPJSCode = '" + value + "'";
            Methods.getObject('GetvBPJSReferenceDiagnoseEKlaimList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtClaimDiagnoseCode.ClientID %>').val(result.BPJSCode);
                    $('#<%=txtClaimDiagnoseName.ClientID %>').val(result.BPJSName);
                }
                else {
                    $('#<%=txtClaimDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtClaimDiagnoseName.ClientID %>').val('');
                }
            });
        }

        $('#<%=txtClaimDiagnosisCode.ClientID %>').live('change', function () {
            ontxtClaimDiagnosisCodeChanged($(this).val());
        });

        function onAfterCustomClickSuccess(type, paramUrl) {
            var url = ResolveUrl(paramUrl);
            showLoadingPanel();
            window.location.href = url;
        }

        $('#btnSaveClaimDiagnose').live('click', function (evt) {
            var txtClaimDiagnoseCode = $('#<%=txtClaimDiagnoseCode.ClientID %>').val();
            if (txtClaimDiagnoseCode != "") {
                cbpProcess.PerformCallback('saveClaimDiagnose');
            }
        });

        function onCbpDiagnosaViewEndCallback(s) {
            $('#containerImgLoadingView').hide();
            $('#ctnEntry').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'saveClaimDiagnose') {
                if (param[1] == "1") {
                    ShowSnackbarSuccess('Save Claim Diagnose Success');
                    $("#formEntry").hide();
                    cbpDiagnosaView.PerformCallback('refresh');
                } else {
                    showToast('Failed : ' + param[2]);
                }
            } else if (param[0] == "deletedClaimDiagnose") {
                if (param[1] == "1") {
                    ShowSnackbarSuccess('Delete Claim Diagnose Success');
                    $("#formEntry").hide();
                    cbpDiagnosaView.PerformCallback('refresh');
                } else {
                    showToast('Failed : ' + param[2]);
                }
            }

        }
        //#endregion

        //#region Procedure
        $(function () {
            setDatePicker('<%=txtClaimProcedureDate.ClientID %>');
            $('#<%=txtClaimProcedureDate.ClientID %>').datepicker('option', 'maxDate', '0');
            cbpProcess.PerformCallback('getClaimData');
        });

        $('#<%=grdProcedureView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdProcedureView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnEntryProsedureID.ClientID %>').val($(this).find('.keyField').html());
        });

        function selectGridRowProcedure() {
            $('#<%=grdProcedureView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnEntryProsedureID.ClientID %>').val($(this).find('.keyField').html());
        }

        function getProsedurEKlaimFilterExpression() {
            var filterExpression = "BPJSCode IS NOT NULL";
            return filterExpression;
        }

        $('#lblProsedurAdd.lblLink').live('click', function () {
            $('#FormProcedure').show();
            FormResetProcedure();
            $('#<%=hdnEntryProsedureID.ClientID %>').val('');
        });

        $('#lblProsedurEdit.lblLink').live('click', function () {
            var idx = $(this).closest('tr').index();
            $('#<%=grdProcedureView.ClientID %> tr:eq(' + idx + ')').click();

            var evID = $(this).attr("data-val");
            var ID = $('#<%=hdnEntryProsedureID.ClientID %>').val(evID);
            if (ID != "") {
                $("#FormProcedure").show();
                var filterExpression = "ID=" + evID;
                Methods.getObject('GetvPatientProcedureList', filterExpression, function (result) {
                    if (result != null) {
                        entityToControlProcedure(result);
                    }
                    else {
                        FormResetProcedure();
                        showToast('Failed', 'Silahkan pilih terlebih dahulu prosedure yang akan di edit.');
                    }
                });
            }

        });

        $('#lblProsedurDelete.lblLink').live('click', function () {
            var idx = $(this).closest('tr').index();
            $('#<%=grdProcedureView.ClientID %> tr:eq(' + idx + ')').click();

            var evID = $(this).attr("data-val");
            var ID = $('#<%=hdnEntryProsedureID.ClientID %>').val(evID);

            if (ID != "") {
                cbpProcedureView.PerformCallback('deletedClaimProcedure');
            } else {
                showToast('Failed', 'Silahkan pilih terlebih dahulu prosedure yang akan di edit.');
            }
        });

        function onGetClaimProceduresFilterExpression() {
            var filterExpression = "INACBGLabel != ''";
            return filterExpression;
        }

        $('#lblClaimProcedure.lblLink').live('click', function () {
            openSearchDialog('eklaimprocedures', onGetClaimProceduresFilterExpression(), function (value) {

                ontxtClaimProcedureCodeChanged(value);
            });
        });

        $('#<%=txtClaimProcedureCode.ClientID %>').live('change', function () {
            ontxtClaimProcedureCodeChanged($(this).val());
        });

        function ontxtClaimProcedureCodeChanged(value) {
            var filterExpression = onGetClaimProceduresFilterExpression() + " AND ProcedureID = '" + value + "'";
            Methods.getObject('GetvProceduresList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtClaimProcedureCode.ClientID %>').val(result.ProcedureID);
                    $('#<%=txtClaimProcedureName.ClientID %>').val(result.ProcedureName);
                    $('#<%=txtClaimProcedureText.ClientID %>').val(result.ProcedureName);
                    $('#<%=txtProcedurev5Code.ClientID %>').val(result.INACBGLabel);
                    $('#<%=txtProcedurev5Name.ClientID %>').val(result.INACBGText);
                    $('#<%=txtProcedurev6Code.ClientID %>').val(result.INACBGINALabel);
                    $('#<%=txtProcedurev6Name.ClientID %>').val(result.INACBGINALabelName);
                }
                else {
                    FormResetProcedure();
                }
            });
        }

        function onGetClaimProceduresINAFilterExpression() {
            var filterExpression = "GCBPJSObjectType = 'X358^019'";
            return filterExpression;
        }

        function entityToControlProcedure(entity) {
            $('#<%=txtClaimProcedureCode.ClientID %>').val(entity.ClaimProcedureID);
            $('#<%=txtClaimProcedureName.ClientID %>').val(entity.ClaimProcedureName);
            $('#<%=txtClaimProcedureText.ClientID %>').val(entity.ClaimProcedureText);
            $('#<%=txtProcedurev5Code.ClientID %>').val(entity.ClaimINAProcedureID);
            $('#<%=txtProcedurev5Name.ClientID %>').val(entity.ClaimProcedureName);
            $('#<%=txtProcedurev6Code.ClientID %>').val(entity.ClaimINACBGINAProcedureID);
            $('#<%=txtProcedurev6Name.ClientID %>').val(entity.ClaimProcedureName);
        }

        function FormResetProcedure() {
            $('#<%=txtClaimProcedureCode.ClientID %>').val("");
            $('#<%=txtClaimProcedureName.ClientID %>').val("");
            $('#<%=txtClaimProcedureText.ClientID %>').val("");
            $('#<%=txtProcedurev5Code.ClientID %>').val("");
            $('#<%=txtProcedurev5Name.ClientID %>').val("");
            $('#<%=txtProcedurev6Code.ClientID %>').val("");
            $('#<%=txtProcedurev6Name.ClientID %>').val("");
        }

        function onCbpProcedureViewEndCallback(s) {
            $('#containerImgLoadingView').hide();
            $('#FormProcedure').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
            }
            else if (param[0] == "saveClaimProcedure") {
                if (param[1] == "1") {
                    ShowSnackbarSuccess('Save Claim Prosedure Success');
                    $('#FormProcedure').hide();
                    FormResetProcedure();
                    cbpProcedureView.PerformCallback('refresh');
                }
                else {
                    showToast('Failed Claim Prosedure : ' + param[2]);
                }
            }
            else if (param[0] == "deletedClaimProcedure") {
                if (param[1] == "1") {
                    ShowSnackbarSuccess('Delete Claim Prosedure Success');
                    $('#FormProcedure').hide();
                    FormResetProcedure();
                    cbpProcedureView.PerformCallback('refresh');
                } else {
                    showToast('Failed Delete Claim Prosedure : ' + param[2]);
                }
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('#btnProcedureSave').live('click', function () {
            cbpProcess.PerformCallback('saveClaimProcedure');
        });

        $('#btnProcedureCancel').live('click', function () {
            $('#FormProcedure').hide();
            FormResetProcedure();
        });

        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == "newClaim") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                        cbpProcess.PerformCallback('getClaimData');
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            } else if (param[0] == 'saveClaimDiagnose') {
                if (param[1] == 'fail') {
                    showToast('Save Claim Diagnose Failed', 'Error Message : ' + param[2]);
                }
                else {
                    ShowSnackbarSuccess('Save Claim Diagnose Success');
                    cbpDiagnosaView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'saveClaimProcedure') {
                if (param[1] == 'fail') {
                    showToast('Save Claim Procedure Failed', 'Error Message : ' + param[2]);
                }
                else {
                    ShowSnackbarSuccess('Save Claim Procedure Success');
                    cbpProcedureView.PerformCallback('refresh');
                }
            } else if (param[0] == "updateClaim") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    ShowSnackbarSuccess(respInfo.metadata.message);
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }

                cbpProcess.PerformCallback('getClaimData');
            }
            else if (param[0] == "grouperStage1") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                        cbpProcess.PerformCallback('getClaimData');
                    } else {
                        var isStage2 = $('#<%=hdnIsGrouperStage2.ClientID %>').val();
                        if (isStage2 == "1") {
                            alert("Silahkan dilakukan grouper stage 2");
                            $('#<%=btnGrouper1.ClientID %>').hide();
                            $('#<%=btnGrouper2.ClientID %>').show();
                        } else {
                            ShowSnackbarSuccess(respInfo.metadata.message);
                            cbpProcess.PerformCallback('getClaimData');
                        }
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                    cbpProcess.PerformCallback('getClaimData');
                }
            }
            else if (param[0] == "grouperStage2") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                    }
                    $('#<%=btnGrouper1.ClientID %>').hide();
                    $('#<%=btnGrouper2.ClientID %>').hide();
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }

                cbpProcess.PerformCallback('getClaimData');
            }
            else if (param[0] == "reeditClaim") {
                if (param[1] == "success") {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                    }

                    cbpProcess.PerformCallback('getClaimData');
                }
            }
            else if (param[0] == "finalClaim") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }

                cbpProcess.PerformCallback('getClaimData');
            }
            else if (param[0] == "sendClaimIndividual") {
                if (param[1] == "success") {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                        cbpProcess.PerformCallback('getClaimData');
                    }
                }
            } else if (param[0] == "deleteClaim") {
                if (param[1] == "success") {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        ShowSnackbarSuccess(respInfo.metadata.message);
                    }
                }

                cbpProcess.PerformCallback('getClaimData');
            }
            else if (param[0] == "getPrintClaim") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        var resp = respInfo.data;
                        window.open("data:application/pdf;base64," + resp);
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == "getStatusClaim") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        ShowSnackbarError(respInfo.metadata.message);
                    } else {
                        var resp = respInfo;
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == "getClaimData") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        if (respInfo.metadata.error_no != "E2004") {
                            ShowSnackbarError(respInfo.metadata.message);
                        }
                    }
                    settingButtonMenu(respInfo);
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }

                cbpProcess.PerformCallback('Refresh');
            }
            else if (param[0] == "sitb_validate") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code == "200") {
                        if (respInfo.response.status == "VALID") {
                            ShowSnackbarSuccess(respInfo.response.detail);
                        } else {
                            ShowSnackbarError(respInfo.response.detail);
                        }
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == "print_sep") {
                if (param[1] == 'success') {
                    if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "DEMO" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSES") {
                        openReportViewer('PM-00196', $('#<%=hdnRegistrationID.ClientID %>').val());
                    }
                    else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSA") {
                        openReportViewer('PM-90064', $('#<%=hdnRegistrationID.ClientID %>').val());
                    }
                    else {
                        openReportViewer('PM-00128', $('#<%=hdnRegistrationID.ClientID %>').val());
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == "Refresh") {
                if (param[1] == 'success') {
                    if (param[2] != "") {
                        var respInfo = JSON.parse(param[2]);
                        if (respInfo.metadata.code != "200") {
                            if (respInfo.metadata.error_no != "E2004") {
                                ShowSnackbarError(respInfo.metadata.message);
                            }
                        }
                        settingButtonMenu(respInfo);
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
        }

        function settingButtonMenu(entity) {
            var param = "";

            if (entity.metadata.code != "200") {
                param = entity.metadata.error_no;
            }
            else {
                param = entity.response.data.klaim_status_cd;
            }

            if (param == "E2004") //sep belum terbentuk
            {
                $('#<%=btnCreateNewClaim.ClientID %>').show();
                $('#<%=btnSetClaimData.ClientID %>').hide();
                $('#<%=btnDeleteClaim.ClientID %>').hide();
                $('#<%=btnUploadDocument.ClientID %>').hide();
                $('#<%=btnGrouper1.ClientID %>').hide();
                $('#<%=btnGrouper2.ClientID %>').hide();
                $('#<%=btnFinalKlaim.ClientID %>').hide();
                $('#<%=btnReeditKlaim.ClientID %>').hide();
                $('#<%=btnRefreshKlaim.ClientID %>').show();
                $('#<%=btnPrintKlaim.ClientID %>').hide();
                $('#<%=btnSendKlaimOnline.ClientID %>').hide();
            }
            else if (param == "normal") {
                $('#<%=btnCreateNewClaim.ClientID %>').hide();
                $('#<%=btnSetClaimData.ClientID %>').show();
                $('#<%=btnDeleteClaim.ClientID %>').show();
                $('#<%=btnUploadDocument.ClientID %>').hide();
                $('#<%=btnGrouper1.ClientID %>').show();
                $('#<%=btnRefreshKlaim.ClientID %>').show();
                $('#<%=btnGrouper2.ClientID %>').hide();

                $('#<%=btnFinalKlaim.ClientID %>').show();
                $('#<%=btnReeditKlaim.ClientID %>').hide();
                $('#<%=btnPrintKlaim.ClientID %>').hide();
                $('#<%=btnSendKlaimOnline.ClientID %>').hide();
            }
            else if (param == "final") {
                $('#<%=btnCreateNewClaim.ClientID %>').hide();
                $('#<%=btnSetClaimData.ClientID %>').hide();
                $('#<%=btnDeleteClaim.ClientID %>').hide();
                $('#<%=btnUploadDocument.ClientID %>').hide();
                $('#<%=btnGrouper1.ClientID %>').hide();
                $('#<%=btnGrouper2.ClientID %>').hide();

                $('#<%=btnRefreshKlaim.ClientID %>').show();
                $('#<%=btnFinalKlaim.ClientID %>').hide();
                $('#<%=btnReeditKlaim.ClientID %>').show();
                $('#<%=btnPrintKlaim.ClientID %>').show();
                $('#<%=btnSendKlaimOnline.ClientID %>').show();
            }

            if (entity.metadata.code == "200") {
                if (entity.response.data.grouper.response != null) {
                    $('#lblklaim_status_cd').html(entity.response.data.klaim_status_cd);

                    $('#lblkemenkes_dc_status_cd').html(entity.response.data.kemenkes_dc_status_cd);
                    $('#lblkemenkes_dc_sent_dttm').html(entity.response.data.kemenkes_dc_sent_dttm);
                    $('#lblbpjs_dc_status_cd').html(entity.response.data.bpjs_dc_status_cd);
                    $('#bpjs_dc_sent_dttm').html(entity.response.data.bpjs_dc_sent_dttm);

                    $('#lblbpjs_klaim_status_cd').html(entity.response.data.bpjs_klaim_status_cd);
                    $('#lblbpjs_klaim_status_nm').html(entity.response.data.bpjs_klaim_status_nm);
                }
            }
            else {
                $('#<%=grouperCode.ClientID %>').html('');
                $('#<%=grouperDescription.ClientID %>').html('');


                var jenisRawat = $('#<%=txtServiceUnitType.ClientID %>').val();
                var kelas = '';


                $('#<%=JenisRawat.ClientID %>').html('');
                $('#<%=grouperValue1.ClientID %>').val('');
                $('#<%=grouperValue2.ClientID %>').val('');
                $('#<%=grouperValue3.ClientID %>').val('');

                //V6
                $('#v6Grouper').show();
                $('#v6_MDC').html('');
                $('#v6_MDC_SCORE').html('');
                $('#v6_DRG').html('');
                $('#v6_DRG_SCORE').html('');

                $('#lblklaim_status_cd').html('');
                $('#lblkemenkes_dc_status_cd').html('');
                $('#lblkemenkes_dc_sent_dttm').html('');
                $('#lblbpjs_dc_status_cd').html('');
                $('#bpjs_dc_sent_dttm').html('');

                $('#lblbpjs_klaim_status_cd').html('');
                $('#lblbpjs_klaim_status_nm').html('');

            }

        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('#<%=btnGrouper1.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('grouperStage1');
            }
        });

        $('#<%=btnGrouper2.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('grouperStage2');
            }
        });

        $('#<%=btnFinalKlaim.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('finalClaim');
            }
        });

        $('#<%=btnPrintKlaim.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('getPrintClaim');

            }
        });

        $('#<%=btnRefreshKlaim.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('getClaimData');
            }
            cbpProcess.PerformCallback('Refresh');
        });

        $('#<%=btnReeditKlaim.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('reeditClaim');

            }
        });

        $('#<%=btnSendKlaimOnline.ClientID %>').live('click', function () {
            var nomor_sep = $('#<%=txtNoSEP.ClientID %>').val();
            if (nomor_sep == "") {
                showToast('Failed', 'Nomor SEP Kosong');
            } else {
                cbpProcess.PerformCallback('sendClaimIndividual');

            }
        });

        $('#<%=btnViewEpisodeSummary.ClientID %>').live('click', function () {
            var oVisitID = $('#<%=hdnVisitID.ClientID %>').val();
            if (oVisitID != '') {
                var url = ResolveUrl("~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryCtl.ascx");
                openUserControlPopup(url, oVisitID, 'Ringkasan Perawatan', 1300, 600);
            }
        });

        $('#btnTBValidate').live('click', function () {
            cbpProcess.PerformCallback('sitb_validate');
        });

        $('#btnTBInvalidate').live('click', function () {
            cbpProcess.PerformCallback('sitb_invalidate');
        });

        $('#btnMPEntryPopupPrintSEP').click(function () {
            cbpProcess.PerformCallback('print_sep');
        });

        $('#<%=chkIsRawatIntensif.ClientID %>').live('change', function () {
            if (this.checked) {
                $('#<%=trValueRawatIntensif.ClientID %>').show();
                $('#<%=trValueVentilatorRow1.ClientID %>').show();
                $('#<%=trValueVentilatorRow2.ClientID %>').show();
                $('#<%=trValueVentilatorRow3.ClientID %>').show();
            }
            else {
                $('#<%=trValueRawatIntensif.ClientID %>').hide();
                $('#<%=trValueVentilatorRow1.ClientID %>').hide();
                $('#<%=trValueVentilatorRow2.ClientID %>').hide();
                $('#<%=trValueVentilatorRow3.ClientID %>').hide();
            }
        });

        $('#<%=txtVentilatorStartTime1.ClientID %>').live('change', function () {
            var oData = $('#<%=txtVentilatorStartTime1.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtVentilatorStartTime1.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime1.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtVentilatorStartTime1.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime1.ClientID %>').focus();
            } else if (oData > 23) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtVentilatorStartTime1.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime1.ClientID %>').focus();
            }
        });

        $('#<%=txtVentilatorStartTime2.ClientID %>').live('change', function () {
            var oData = $('#<%=txtVentilatorStartTime2.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtVentilatorStartTime2.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime2.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtVentilatorStartTime2.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime2.ClientID %>').focus();
            } else if (oData > 59) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtVentilatorStartTime2.ClientID %>').val("");
                $('#<%=txtVentilatorStartTime2.ClientID %>').focus();
            }
        });

        $('#<%=txtVentilatorEndTime1.ClientID %>').live('change', function () {
            var oData = $('#<%=txtVentilatorEndTime1.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtVentilatorEndTime1.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime1.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtVentilatorEndTime1.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime1.ClientID %>').focus();
            } else if (oData > 23) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtVentilatorEndTime1.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime1.ClientID %>').focus();
            }
        });

        $('#<%=txtVentilatorEndTime2.ClientID %>').live('change', function () {
            var oData = $('#<%=txtVentilatorEndTime2.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtVentilatorEndTime2.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime2.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtVentilatorEndTime2.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime2.ClientID %>').focus();
            } else if (oData > 59) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtVentilatorEndTime2.ClientID %>').val("");
                $('#<%=txtVentilatorEndTime2.ClientID %>').focus();
            }
        });

        $('#<%=chkIsExecutiveClass.ClientID %>').live('change', function () {
            if (this.checked) {
                $('#<%=trExecutiveClassAmount.ClientID %>').show();

                $('#<%=txtExecutiveClassAmount.ClientID %>').val("0");
            }
            else {
                $('#<%=trExecutiveClassAmount.ClientID %>').hide();

                $('#<%=txtExecutiveClassAmount.ClientID %>').val("0");
            }
        });

        $('#<%=chkIsUpgradeClass.ClientID %>').live('change', function () {
            if (this.checked) {
                $('#<%=trUpgradeClass.ClientID %>').show();
                $('#<%=trUpgradeClassLOS.ClientID %>').show();
                $('#<%=trUpgradeClassPayor.ClientID %>').show();
                $('#<%=trAddPaymentPct.ClientID %>').show();

                $('#<%=txtUpgradeClassLOS.ClientID %>').val("0");
            }
            else {
                $('#<%=trUpgradeClass.ClientID %>').hide();
                $('#<%=trUpgradeClassLOS.ClientID %>').hide();
                $('#<%=trUpgradeClassPayor.ClientID %>').hide();
                $('#<%=trAddPaymentPct.ClientID %>').hide();

                $('#<%=txtUpgradeClassLOS.ClientID %>').val("0");
            }
        });

        $('#<%=chkPasienTB.ClientID %>').live('change', function () {
            if (this.checked) {
                $('#<%=tdtb1.ClientID %>').removeAttr("style");
                $('#<%=tdtb2.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=tdtb1.ClientID %>').attr("style", "display:none");
                $('#<%=tdtb2.ClientID %>').attr("style", "display:none");
            }
        });

        function GrouperErrorCodeValidasi(kodeError) {
            var kode = kodeError;
            var isError = false;
            if (kode.charAt(0) == "X") {
                isError = true;
            }
            return isError;
        }

        function onAfterPopupControlClosing() {
            cbpView.PerformCallback('refresh');
        }

        $('.lnkLinkedFromRegistration').live('click', function () {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoRegistrationLinkedFromCtl.ascx");
            openUserControlPopup(url, registrationID, 'Informasi Registrasi Asal', 1200, 400);
        });

        $('.lnkHistoryPatientTransfer').live('click', function () {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoPatientTransferCtl.ascx");
            openUserControlPopup(url, registrationID, 'Patient Transfer History', 1200, 500);
        });

        $('.lnkHistoryClosedReopenBilling').live('click', function () {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoHistoryRegistrationCtl.ascx");
            openUserControlPopup(url, registrationID, 'Registration History', 1200, 500);
        });

        $('.lnkInfoVentilatorLog').live('click', function () {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoPatientVentilatorLogCtl.ascx");
            openUserControlPopup(url, registrationID, 'Informasi Log Pemakaian Ventilator', 1200, 500);
        });

        $('.lnkIsMultipleVisit').live('click', function () {
            var registrationNo = $('#<%:hdnRegistrationNo.ClientID %>').val();
            var noSEP = $('#<%:txtNoSEP.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InformationMultipleVisitCtl.ascx");
            var data = registrationNo + "|" + noSEP;
            openUserControlPopup(url, data, 'Multi Visit', 900, 500);
        });

        $('#<%=txtDeliverySHKTime1.ClientID %>').live('change', function () {
            var oData = $('#<%=txtDeliverySHKTime1.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtDeliverySHKTime1.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime1.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtDeliverySHKTime1.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime1.ClientID %>').focus();
            } else if (oData > 23) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 23");
                $('#<%=txtDeliverySHKTime1.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime1.ClientID %>').focus();
            }
        });

        $('#<%=txtDeliverySHKTime2.ClientID %>').live('change', function () {
            var oData = $('#<%=txtDeliverySHKTime2.ClientID %>').val();

            if (oData.length != 2) {
                ShowSnackbarError("Harap isi format jam dgn 2 digit angka.");
                $('#<%=txtDeliverySHKTime2.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime2.ClientID %>').focus();
            } else if (oData < 0) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtDeliverySHKTime2.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime2.ClientID %>').focus();
            } else if (oData > 59) {
                ShowSnackbarError("Harap isi format jam antara 00 s/d 59");
                $('#<%=txtDeliverySHKTime2.ClientID %>').val("");
                $('#<%=txtDeliverySHKTime2.ClientID %>').focus();
            }
        });

        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var registrationNo = $('#<%:txtRegistrationNo.ClientID %>').val();
            var menuID = $('#<%:hdnMenuID.ClientID %>').val();
            var nomor_sep = $('#<%:txtNoSEP.ClientID %>').val();
            var coder_nik = $('#<%=hdnUsernameLogin.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var mrn = $('#<%:txtMedicalNo.ClientID %>').val();

            if (code == 'uploadDocument' || code == 'infoTransactionParameter' || code == 'infoTransactionDetailParameter' || code == 'infoTransactionDetailPharmacyParameter'
                    || code == 'infoTransactionParameterFromBilling' || code == 'infoTransactionDetailParameterFromBilling' || code == 'infoDiagnosticResult'
                    || code == 'transactionNotes' || code == 'printSuratKontrol' || code == 'dischargeDateBpjs') {
                if (registrationID != "" && registrationID != null) {
                    return registrationID;
                } else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    return false;
                }
            }
            else if (code == 'infoSurgeryReport' || code == 'infoRingkasanPerawatan') {
                if (visitID != "") {
                    return visitID;
                }
                else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    return false;
                }
            }
            else if (code == 'infoEdocument') {
                return "|" + mrn + "|" + visitID;
            }
            else if (code == 'getClaimStatus' || code == 'reeditClaim' || code == 'sendClaimPerSEP' || code == 'infoNoSEPDuplicate') {
                return nomor_sep;
            }
            else if (code == 'finalClaim') {
                return nomor_sep + "|" + coder_nik;
            }
            else if (code == 'downloadReportTagihan') {
                return registrationID + "|" + registrationNo + "|" + menuID;
            }
            else if (code == 'infoPersalinanReport') {
                return registrationID + "|" + registrationNo + "|" + visitID;
            } else if (code == 'getPeserta') {
                return registrationID;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            var mrn = $('#<%:hdnMRN.ClientID %>').val();

            if (code == 'MR000007' || code == 'MR000005' || code == 'MR000026' || code == 'MR000025' || code == 'MR000021') {
                filterExpression.text = "VisitID = " + visitID;
            }
            else if (code == 'MR000017' || code == 'MR000023' || code == 'MR000039' || code == 'PM-90022' || code == 'PM-90023' || code == 'PM-90035'
                    || code == 'PM-90011' || code == 'PM-90046') {
                filterExpression.text = visitID;
            }
            else if (code == 'PM-00103' || code == 'PM-00429' || code == 'MR000016' || code == 'PM-00523'
                    || code == 'PM-00524' || code == 'PM-00546' || code == 'PM-00565' || code == 'PM-00117'
                    || code == 'PM-00650' || code == 'PM-00248' || code == 'PM-00564') {
                filterExpression.text = registrationID;
            }
            else if (code == 'PM-00488' || code == 'PM-00489' || code == 'PM-002163' || code == 'PM-002136' || code == 'PM-002137' || code == 'PM-002138'
                    || code == 'PM-00282' || code == 'PM-00493' || code == 'PM-00283' || code == 'PM-00494' || code == 'PM-00284' || code == 'PM-00495' || code == 'PM-00285' || code == 'PM-00496') {
                filterExpression.text = "RegistrationID = " + registrationID;
            }
            else if (code == 'PM-90084') {
                var patientBirthRecordID = "";
                Methods.getObject("GetvPatientBirthRecordFullList", "MRN = " + mrn, function (result) {
                    if (result != null) {
                        patientBirthRecordID = result.BirthRecordID;
                    }
                });

                if (patientBirthRecordID == null || patientBirthRecordID == "" || patientBirthRecordID == "0") {
                    errMessage.text = 'Pasien Belum mengisi Data Bayi Lahir';
                    return false;
                }
                else {
                    filterExpression.text = 'MRN = ' + mrn;
                    return true;
                }
            }
            else {
                filterExpression.text = visitID;
            }
            return true;
        }

    </script>
    <style>
        .row
        {
            display: flex;
        }
        .column
        {
            flex: 50%;
        }
        tbody
        {
            font-size: 14px;
        }
    </style>
    <input type="hidden" id="hdnHealthcareInitial" runat="server" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnMenuID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationNo" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnDischargeStatus" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
    <input type="hidden" id="hdnUsernameLogin" value="" runat="server" />
    <input type="hidden" id="hdnpayor_id" value="" runat="server" />
    <input type="hidden" id="hdnpayor_code" value="" runat="server" />
    <input type="hidden" id="hdncob_cd" value="" runat="server" />
    <input type="hidden" id="hdnDiagnosaEklaim" runat="server" value="" />
    <input type="hidden" id="hdnDiagnosaEklaimINA" runat="server" value="" />
    <input type="hidden" id="hdnSystole" runat="server" value="" />
    <input type="hidden" id="hdnDiastole" runat="server" value="" />
    <input type="hidden" id="hdnIsSendEKlaimMedicalNo" runat="server" value="" />
    <input type="hidden" id="hdnIsSistoleAndDiastoleFromLinkedRegistration" runat="server"
        value="" />
    <input type="hidden" id="hdnIsNonInpatientDischargeDateFromSEPDate" runat="server"
        value="" />
    <input type="hidden" id="hdnIsNonInpatientRegistrationDateFromSEPDate" runat="server"
        value="" />
    <input type="hidden" id="hdnIsDokterUsingDokterKonsulenVClaim" runat="server" value="" />
    <input type="hidden" id="hdnMenampilkanDiagnosaMasuk" runat="server" value="" />
    <input id="hdnprosedur_non_bedah" type="hidden" runat="server" value="0" />
    <input id="hdnprosedur_bedah" type="hidden" runat="server" value="0" />
    <input id="hdnkonsultasi" type="hidden" runat="server" value="0" />
    <input id="hdntenaga_ahli" type="hidden" runat="server" value="0" />
    <input id="hdnkeperawatan" type="hidden" runat="server" value="0" />
    <input id="hdnpenunjang" type="hidden" runat="server" value="0" />
    <input id="hdnradiologi" type="hidden" runat="server" value="0" />
    <input id="hdnlaboratorium" type="hidden" runat="server" value="0" />
    <input id="hdnpelayanan_darah" type="hidden" runat="server" value="0" />
    <input id="hdnrehabilitasi" type="hidden" runat="server" value="0" />
    <input id="hdnkamar" type="hidden" runat="server" value="0" />
    <input id="hdnrawat_intensif" type="hidden" runat="server" value="0" />
    <input id="hdnobat" type="hidden" runat="server" value="0" />
    <input id="hdnobat_kemoterapi" type="hidden" runat="server" value="0" />
    <input id="hdnalkes" type="hidden" runat="server" value="0" />
    <input id="hdnbmhp" type="hidden" runat="server" value="0" />
    <input id="hdnsewa_alat" type="hidden" runat="server" value="0" />
    <input id="hdnlainlain" type="hidden" runat="server" value="0" />
    <input id="hdnobat_kronis" type="hidden" runat="server" value="0" />
    <input id="hdnMRN" type="hidden" runat="server" value="0" />
    <div style="height: 600px; overflow-y: auto; overflow-x: hidden">
        <h4>
            <%=GetLabel("Data Kunjungan Pasien")%></h4>
        <div class="containerTblEntryContent">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 55%" />
                    <col style="width: 45%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <table>
                            <colgroup>
                                <col style="width: 250px" />
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jaminan/Cara Bayar")%></label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxComboBox ID="cboCaraBayarEKlaim" ClientInstanceName="cboCaraBayarEKlaim"
                                        Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s){ oncboCaraBayarEKlaimValueChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trCOB" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("COB")%></label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxComboBox ID="cboCOBEKlaim" ClientInstanceName="cboCOBEKlaim" Width="100%"
                                        runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No SEP | Tgl SEP")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnTanggalSEP" value="" runat="server" />
                                    <input type="hidden" id="hdnJamSEP" value="" runat="server" />
                                    <asp:TextBox ID="txtNoSEP" Width="100%" runat="server" Style="text-align: center"
                                        ReadOnly="true" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTanggalSEP" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jenis Rawat")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtServiceUnitType" Width="100%" runat="server" ReadOnly="true"
                                        Style="text-align: center" />
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
                                    <asp:TextBox ID="txtServiceUnitName" Width="85%" runat="server" ReadOnly="true" Style="text-align: left;
                                        vertical-align: middle" />
                                    <img class="lnkHistoryPatientTransfer imgLink" title="<%=GetLabel("Patient Transfer History") %>"
                                        src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                        width="30px" />
                                </td>
                            </tr>
                            <tr id="trExecutiveClassFlag" runat="server" style="display: none">
                                <td>
                                    <label>
                                        <%=GetLabel("Kelas Eksekutif")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="chkIsExecutiveClass" runat="server" />
                                </td>
                            </tr>
                            <tr id="trExecutiveClassAmount" runat="server" style="display: none">
                                <td>
                                    <label title="Masukkan nilai tagihan utk poli eksekutif yg dibayar pasien">
                                        <%=GetLabel("Tariff Poli Eksekutif")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExecutiveClassAmount" Width="100%" runat="server" CssClass="number" />
                                </td>
                            </tr>
                            <tr id="trUpgradeClassFlag" runat="server" style="display: none">
                                <td>
                                    <label>
                                        <%=GetLabel("Naik Kelas")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="chkIsUpgradeClass" runat="server" />
                                </td>
                            </tr>
                            <tr id="trUpgradeClass" runat="server" style="display: none">
                                <td>
                                    <label title="Pilih Naik Kelas Pasien">
                                        <%=GetLabel("Kelas Pelayanan Naik")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList runat="server" ID="rblUpgradeClass" RepeatDirection="Horizontal">
                                        <asp:ListItem Text=" Kelas 2 " Value="kelas_2" Selected="True" />
                                        <asp:ListItem Text=" Kelas 1 " Value="kelas_1" />
                                        <asp:ListItem Text=" Kelas VIP " Value="vip" />
                                        <asp:ListItem Text=" Kelas VVIP " Value="vvip" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trUpgradeClassLOS" runat="server" style="display: none">
                                <td>
                                    <label title="Isikan LOS (hari) Naik Kelas">
                                        <%=GetLabel("Lama Naik Kelas")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUpgradeClassLOS" Width="75px" runat="server" CssClass="number"
                                        onkeypress="return isValidateNumber(event)" />
                                </td>
                            </tr>
                            <tr id="trUpgradeClassPayor" runat="server" style="display: none">
                                <td>
                                    <label title="Pilih Pembayar Naik Kelas Pasien">
                                        <%=GetLabel("Pembayar Naik Kelas")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList runat="server" ID="rblUpgradeClassPayor" RepeatDirection="Horizontal">
                                        <asp:ListItem Text=" Peserta " Value="peserta" Selected="True" />
                                        <asp:ListItem Text=" Pemberi Kerja " Value="pemberi_kerja" />
                                        <asp:ListItem Text=" Asuransi Tambahan " Value="asuransi_tambahan" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trAddPaymentPct" runat="server" style="display: none">
                                <td>
                                    <label title="Koefisien tambahan biaya khusus jika pasien naik ke kelas VIP">
                                        <%=GetLabel("Biaya Tambahan (%)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddPaymentPct" Width="75px" runat="server" CssClass="number"
                                        onkeypress="return isValidateNumber(event)" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Dokter DPJP")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtParamedicName" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Dokter V-Claim")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNamaDPJPKonsulan" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No Peserta | No RM | No RM E-Klaim")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoPeserta" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEKlaimMedicalNo" Width="100%" runat="server" ReadOnly="true"
                                        Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama Pasien")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtPatientName" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl Lahir | Jenis Kelamin")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDOB" value="" runat="server" />
                                    <input type="hidden" id="hdnGender" value="" runat="server" />
                                    <asp:TextBox ID="txtDOB" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGender" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Berat Lahir (gram)")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBirthWeight" value="" runat="server" />
                                    <asp:TextBox ID="txtBirthWeight" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pasien TB")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPasienTB" runat="server" />
                                </td>
                                <td class="tb" id="tdtb1" runat="server">
                                    <asp:TextBox ID="txtNoPasienTB" Width="200px" runat="server" Style="text-align: center;"
                                        placeholder="Registrasi SITB" />
                                </td>
                                <td class="tb" id="tdtb2" runat="server">
                                    <input type="button" value="Validasi" class="btn btn-primary" id="btnTBValidate" />
                                    <input type="button" value="Batal Validasi" class="btn btn-primary" id="btnTBInvalidate" />
                                </td>
                            </tr>
                            <tr id="trIsRawatIntensif" runat="server">
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Ada Rawat Intensif")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsRawatIntensif" runat="server" />
                                </td>
                            </tr>
                            <tr id="trValueRawatIntensif" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("ICU LOS (Hari)")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnICULOS" value="" runat="server" />
                                    <asp:TextBox ID="txtICULOS" Width="100%" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr id="trValueVentilatorRow1" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Ventilator (Jam)")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtLamaJamVentilator" Width="180px" runat="server" Style="text-align: center"
                                        onkeypress="return isValidateNumber(event)" />
                                    <img class="lnkInfoVentilatorLog imgLink" title="<%=GetLabel("Informasi Pemasangan Alat Ventilator") %>"
                                        src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                        width="30px" />
                                </td>
                            </tr>
                            <tr id="trValueVentilatorRow2" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl-Jam Mulai Ventilator")%></label>
                                </td>
                                <td colspan="3">
                                    <table>
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 50px" />
                                            <col style="width: 10px" />
                                            <col style="width: 50px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtVentilatorStartDate" runat="server" CssClass="datepicker" Style="width: 120px"
                                                    placeholder="dd-mm-yyyy" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtVentilatorStartTime1" runat="server" Style="width: 90%; text-align: center"
                                                    placeholder="00-23" onkeypress="return isValidateNumber(event)" />
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel(":")%></label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtVentilatorStartTime2" runat="server" Style="width: 90%; text-align: center"
                                                    placeholder="00-59" onkeypress="return isValidateNumber(event)" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trValueVentilatorRow3" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl-Jam Akhir Ventilator")%></label>
                                </td>
                                <td colspan="3">
                                    <table>
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 50px" />
                                            <col style="width: 10px" />
                                            <col style="width: 50px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtVentilatorEndDate" runat="server" CssClass="datepicker" Style="width: 120px"
                                                    placeholder="dd-mm-yyyy" />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtVentilatorEndTime1" runat="server" Style="width: 90%; text-align: center"
                                                    placeholder="00-23" onkeypress="return isValidateNumber(event)" />
                                            </td>
                                            <td align="center">
                                                <label class="lblNormal">
                                                    <%=GetLabel(":")%></label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtVentilatorEndTime2" runat="server" Style="width: 90%; text-align: center"
                                                    placeholder="00-59" onkeypress="return isValidateNumber(event)" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <table>
                            <colgroup>
                                <col style="width: 240px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No Registrasi | Tgl Registrasi")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnRegistrationDate" value="" runat="server" />
                                    <input type="hidden" id="hdnRegistrationTime" value="" runat="server" />
                                    <asp:TextBox ID="txtRegistrationNo" Width="160px" runat="server" ReadOnly="true"
                                        Style="text-align: center" />
                                    <asp:TextBox ID="txtRegistrationDate" Width="150px" runat="server" ReadOnly="true"
                                        Style="text-align: center" />
                                    <img class="lnkLinkedFromRegistration imgLink" title="<%=GetLabel("Registration From") %>"
                                        src='<%= ResolveUrl("~/Libs/Images/Toolbar/outpatient_visit_history.png")%>'
                                        alt="" width="30px" />
                                </td>
                                <td>
                                    <img id="imgIsMultipleVisit" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="35" src='' alt='' title="Multiple Visit" />
                                </td>
                                <td>
                                    <img id="imgIsTransferredToInpatient" runat="server" width="30" src='' alt='' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationStatus" Width="150px" runat="server" ReadOnly="true"
                                        Style="text-align: center; vertical-align: middle" />
                                    <img class="lnkHistoryClosedReopenBilling imgLink" title="<%=GetLabel("Registration History") %>"
                                        src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>' alt=""
                                        width="30px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl Pulang")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDischargeDate" value="" runat="server" />
                                    <input type="hidden" id="hdnDischargeTime" value="" runat="server" />
                                    <asp:TextBox ID="txtDischargeDate" Width="160px" runat="server" ReadOnly="true" Style="text-align: center;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl Pulang (sementara)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDateSementara" Width="160px" runat="server" ReadOnly="true"
                                        Style="text-align: center;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("LOS (hari-jam-menit)")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnLOSinDay" value="" runat="server" />
                                    <asp:TextBox ID="txtLOSinDay" Width="40px" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                    <asp:TextBox ID="txtLOSinHour" Width="40px" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                    <asp:TextBox ID="txtLOSinMinute" Width="40px" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas SEP | Kelas Tanggungan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnKelasSEP" value="" runat="server" />
                                    <input type="hidden" id="hdnKelasTanggungan" value="" runat="server" />
                                    <asp:TextBox ID="txtKelasSEP" Width="75px" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                    <asp:TextBox ID="txtKelasTanggungan" Width="75px" Style="text-align: center" runat="server"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No Rujukan | Tgl Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoRujukan" Width="200px" runat="server" ReadOnly="true" Style="text-align: center" />
                                    <asp:TextBox ID="txtTanggalRujukan" Width="150px" runat="server" ReadOnly="true"
                                        Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Cara Masuk (E-Klaim)")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboEKlaimCaraMasuk" ClientInstanceName="cboEKlaimCaraMasuk"
                                        Width="100%" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Cara Pulang (E-Klaim)")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboEKlaimCaraPulang" ClientInstanceName="cboEKlaimCaraPulang"
                                        Width="100%" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tarif INACBG")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboKdTarifINACBG" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <h4>
            <%=GetLabel("Detail Transaksi Klaim")%></h4>
        <div>
            <table class="tblContentArea" width="100%">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td>
                    </td>
                    <td>
                        <table class="grdSelected">
                            <tbody>
                                <tr>
                                    <td style="text-align: right">
                                        <b>Total Nilai BPJS</b>
                                    </td>
                                    <td style="text-align: right">
                                        <b>Total Nilai Pasien</b>
                                    </td>
                                    <td style="text-align: right">
                                        <b>Total Nilai</b>
                                    </td>
                                </tr>
                            </tbody>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTotalBPJSAmount" Width="100%" runat="server" ReadOnly="true"
                                        Style="text-align: right" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalPatientAmount" Width="100%" runat="server" ReadOnly="true"
                                        Style="text-align: right" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalAmount" Width="100%" runat="server" ReadOnly="true" Style="text-align: right" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table class="tblContentArea" style="display: none;">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total BPJS")%></label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Pasien")%></label>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <table>
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Transaksi")%></label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trIsTarifPoli" runat="server" style="display: none;">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tarif Poli Eks")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTarifPoliEks" Width="160px" runat="server" CssClass="number" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <input id="hdnTarifKlaimJson" type="hidden" runat="server" value="" />
                        <input id="hdnIsMaapingTarifFailed" type="hidden" runat="server" value="0" />
                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Height="200px">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="EKlaimParameterID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="EKlaimParameterCode" HeaderText="Kode Parameter" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="EKlaimParameterName" HeaderText="Nama Parameter" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="cfPayerAmountInString" HeaderText="Nilai BPJS" HeaderStyle-Width="200px"
                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="cfPatientAmountInString" HeaderText="Nilai Pasien" HeaderStyle-Width="200px"
                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="cfLineAmountInString" HeaderText="Nilai Total" HeaderStyle-Width="200px"
                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("No data to display.")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
        <h4>
            <%=GetLabel("Diagnosa [ICD-10]")%></h4>
        <div class="containerTblEntryContent">
            <table class="tblContentArea" style="display: none;">
                <colgroup>
                    <col style="width: 180px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Diagnosa (Dokter)")%></label>
                    </td>
                    <td>
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 120px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseName" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseInfo" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Diagnosa (Rekam Medis)")%></label>
                    </td>
                    <td>
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 120px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <input type="hidden" value="" runat="server" id="hdnDiagnosaID" />
                                    <asp:TextBox ID="txtFinalDiagnoseCode" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFinalDiagnoseName" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFinalDiagnoseInfo" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblLink lblMandatory" id="lblEKlaimDiagnose">
                            <%=GetLabel("Diagnosa (E-Klaim)")%></label>
                    </td>
                    <td>
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 120px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col style="width: 3px" />
                                <col style="width: 500px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtClaimDiagnoseCode" Width="100%" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClaimDiagnoseName" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClaimDiagnoseInfo" Width="500px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="button" id="btnSaveClaimDiagnose" value='<%= GetLabel("Simpan Diagnosa Klaim")%>' />
                    </td>
                </tr>
            </table>
            <div id="formEntry" style="display: none;">
                <h4>
                    Form Diagnosa</h4>
                <input type="hidden" value="" id="hdnEntryID" runat="server" />
                <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
                <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
                <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
                <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
                <input type="hidden" value="0" id="hdnIsHasRecord" runat="server" />
                <input type="hidden" value="0" id="hdnIsMainDiagnosisExists" runat="server" />
                <table style="width: 100%" class="tblContentArea">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Diagnosa (Klaim)")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboDiagnoseTypeClaim" ClientInstanceName="cboDiagnoseTypeClaim"
                                Width="450px" />
                        </td>
                    </tr>
                    <tr style='display: none'>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Tanggal")%>
                                -
                                <%=GetLabel("Jam Diagnosa (Klaim)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimDiagnosisDate" Width="120px" runat="server" Style="text-align: center" />
                            <asp:TextBox ID="txtClaimDiagnosisTime" Width="60px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblClaimDiagnose">
                                <%=GetLabel("Kode Diagnosa (Klaim)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClaimDiagnosisCode" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClaimDiagnosisName" Width="350px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Diagnosa Text (Klaim)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimDiagnosisText" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Diagnosa V5 (Klaim)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtv5DiagnosaID" Width="100px" runat="server" ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtv5DiagnosaName" Width="350px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Diagnosa V6 (Klaim)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtv6DiagnosaID" Width="100px" runat="server" ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtv6DiagnosaName" Width="350px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="button" value="SIMPAN" class="w3-button w3-btn" id="btnSendDataDiagnosa" />
                            &nbsp;
                            <input type="button" value="BATAL" class="w3-button w3-btn" id="btnCancelDiagnosa" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tblContentArea">
                <input type="hidden" value="" id="hdnID" runat="server" />
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <div style="position: relative;">
                    <div>
                        <label class="lblLink" id="lblDiagnosaAdd">
                            Tambah Diagnosa Klaim
                        </label>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpDiagnosaView"
                        ShowLoadingPanel="false" OnCallback="cbpDiagnosaView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpDiagnosaViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdDiagnosaView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa oleh Dokter")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%#: Eval("cfIsIgnorePhysicianDiagnose").ToString() == "1" ?  "display:none;" :""  %>'>
                                                        <div>
                                                            <%#: Eval("DifferentialDateInString")%>,
                                                            <%#: Eval("DifferentialTime")%></div>
                                                        <div>
                                                            <div>
                                                                <span style="color: Blue; font-size: 1.1em">
                                                                    <%#: Eval("cfDiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                            </div>
                                                            <div style="font-style: italic">
                                                                <%#: Eval("cfMorphologyInfo")%></div>
                                                            <div>
                                                                <%#: Eval("ICDBlockName")%></div>
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("DiagnoseType")%></b> -
                                                                <%#: Eval("DifferentialStatus")%></div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa oleh Rekam Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%#: Eval("cfIsIgnorePhysicianDiagnose").ToString() == "1" ?  "display:none;" :""  %>'>
                                                        <div style='<%# Eval("cfFinalDate") == "" ? "display:none;": "" %>'>
                                                            <div>
                                                                <%#: Eval("cfFinalDate")%>,
                                                                <%#: Eval("FinalTime")%></div>
                                                            <div>
                                                                <span style="color: Blue; font-size: 1.1em">
                                                                    <%#: Eval("cfIsIgnorePhysicianDiagnose")  %>
                                                                    <%#: Eval("cfFinalDiagnosisText")%></span> (<b><%#: Eval("FinalDiagnosisID")%></b>)
                                                            </div>
                                                            <div>
                                                                <%#: Eval("cfMorphologyInfo")%></div>
                                                            <div>
                                                                <%#: Eval("FinalICDBlockName")%></div>
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("DiagnoseType")%></b> -
                                                                <%#: Eval("FinalStatus")%></div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa E-Klaim")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div style='<%#: Eval("cfClaimDiagnosisText") == "" ?  "display:none;":""   %>'>
                                                                    <div style='<%# Eval("cfClaimDiagnosisDate") == "" ? "display:none;": "" %>'>
                                                                        <div>
                                                                            <%#: Eval("cfClaimDiagnosisDate")%>,
                                                                            <%#: Eval("ClaimDiagnosisTime")%></div>
                                                                        <div>
                                                                            <span style="color: Blue; font-size: 1.1em">
                                                                                <%#: Eval("cfClaimDiagnosisText")%></span> (<b><%#: Eval("ClaimDiagnosisID")%></b>)
                                                                        </div>
                                                                        <div>
                                                                            <%#: Eval("cfMorphologyInfo")%></div>
                                                                        <div>
                                                                            <%#: Eval("ClaimICDBlockName")%></div>
                                                                        <div>
                                                                            <b>
                                                                                <%#: Eval("DiagnoseTypeClaim")%></b></div>
                                                                        <div>
                                                                            <label>
                                                                                <%=GetLabel("----------------------------------------------------------------------------------------------------------")%>
                                                                            </label>
                                                                        </div>
                                                                        <div>
                                                                            <label style="font-style: italic">
                                                                                E-Klaim Diagnosa V5
                                                                            </label>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("cfClaimINACBGLabelName")%></b></div>
                                                                        </div>
                                                                        <div>
                                                                            <label style="font-style: italic">
                                                                                E-Klaim Diagnosa V6
                                                                            </label>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("cfINACBGINAName")%></b></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseType") %>" bindingfield="DiagnoseType" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                    <input type="hidden" value="<%#:Eval("DifferentialStatus") %>" bindingfield="DifferentialStatus" />
                                                    <input type="hidden" value="<%#:Eval("cfFinalDateTimePickerFormat") %>" bindingfield="FinalDate" />
                                                    <input type="hidden" value="<%#:Eval("FinalTime") %>" bindingfield="FinalTime" />
                                                    <input type="hidden" value="<%#:Eval("FinalDiagnosisID") %>" bindingfield="FinalDiagnosisID" />
                                                    <input type="hidden" value="<%#:Eval("FinalDiagnosisName") %>" bindingfield="FinalDiagnosisName" />
                                                    <input type="hidden" value="<%#:Eval("FinalDiagnosisText") %>" bindingfield="FinalDiagnosisText" />
                                                    <input type="hidden" value="<%#:Eval("GCFinalStatus") %>" bindingfield="GCFinalStatus" />
                                                    <input type="hidden" value="<%#:Eval("FinalStatus") %>" bindingfield="FinalStatus" />
                                                    <input type="hidden" value="<%#:Eval("cfClaimDiagnosisDateTimePickerFormat") %>"
                                                        bindingfield="ClaimDiagnosisDate" />
                                                    <input type="hidden" value="<%#:Eval("ClaimDiagnosisTime") %>" bindingfield="ClaimDiagnosisTime" />
                                                    <input type="hidden" value="<%#:Eval("ClaimDiagnosisID") %>" bindingfield="ClaimDiagnosisID" />
                                                    <input type="hidden" value="<%#:Eval("ClaimDiagnosisName") %>" bindingfield="ClaimDiagnosisName" />
                                                    <input type="hidden" value="<%#:Eval("ClaimDiagnosisText") %>" bindingfield="ClaimDiagnosisText" />
                                                    <input type="hidden" value="<%#:Eval("ClaimINADiagnoseID") %>" bindingfield="ClaimINADiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("ClaimINADiagnoseText") %>" bindingfield="ClaimINADiagnoseText" />
                                                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                    <input type="hidden" value="<%#:Eval("MorphologyName") %>" bindingfield="MorphologyName" />
                                                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="<%#: Eval("ID") %>">
                                                    </div>
                                                    <div>
                                                        <label class="lblLink" id="lblDiagnosaEdit" data-val="<%#: Eval("ID")%>">
                                                            Ubah Diagnosa Klaim</label>
                                                        <label class="lblLink" id="lblDiagnosaDelete" data-val="<%#: Eval("ID")%>" style='<%#: Eval("ClaimDiagnosisID") != "" ?  "": "display:none;"  %>'>
                                                            | Hapus Diagnosa Klaim</label>
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </div>
        </div>
        <h4>
            <%=GetLabel("Prosedur / Tindakan [ICD-9]")%></h4>
        <div class="containerTblEntryContent">
            <div class="tblContentArea" id="FormProcedure" style="display: none;">
                <h4>
                    Form Prosedur/Tindakan</h4>
                <input type="hidden" value="" id="hdnEntryProsedureID" runat="server" />
                <input type="hidden" value="" id="hdnProsedureHealthcareServiceUnitID" runat="server" />
                <input type="hidden" value="" id="hdnProsedureIsHealthcareServiceUnitHasParamedic"
                    runat="server" />
                <input type="hidden" value="" id="hdnProsedureDefaultParamedicID" runat="server" />
                <input type="hidden" value="" id="hdnProsedureDefaultParamedicCode" runat="server" />
                <input type="hidden" value="" id="hdnProsedureDefaultParamedicName" runat="server" />
                <table style="width: 100%" class="tblEntryDetail">
                    <colgroup>
                        <col style="width: 230px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal")%>
                                -
                                <%=GetLabel("Jam (E-Klaim)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimProcedureDate" Width="120px" CssClass="datepicker" runat="server"
                                Style="text-align: center" />
                            <asp:TextBox ID="txtClaimProcedureTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblClaimProcedure">
                                <%=GetLabel("Prosedur/Tindakan (E-Klaim)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtClaimProcedureCode" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClaimProcedureName" Width="300px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Prosedur/Tindakan Text (E-Klaim)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtClaimProcedureText" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Prosedur/Tindakan (E-Klaim V5)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtProcedurev5Code" ReadOnly="true" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProcedurev5Name" Width="300px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Prosedur/Tindakan (E-Klaim V6)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtProcedurev6Code" ReadOnly="true" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProcedurev6Name" Width="300px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="button" class="w3-button w3-btn" value="SIMPAN" id="btnProcedureSave" />
                            <input type="button" class="w3-button w3-btn" value="CANCEL" id="btnProcedureCancel" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tblContentArea">
                <div>
                    <label class="lblLink lblProsedurAdd" id="lblProsedurAdd">
                        Tambah Prosedur / Tindakan</label></div>
                <input type="hidden" value="" id="hdnProcedureID" runat="server" />
                <input type="hidden" id="hdnFilterExpressionProcedure" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" Width="100%" ClientInstanceName="cbpProcedureView"
                        ShowLoadingPanel="false" OnCallback="cbpProcedureView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show();  }"
                            EndCallback="function(s,e){ onCbpProcedureViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <input type="hidden" value="" id="hdnProcedureEklaim" runat="server" />
                                <input type="hidden" value="" id="hdnProcedureEklaimINA" runat="server" />
                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdProcedureView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureName") %>" bindingfield="ProcedureName" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                                    <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                                    <input type="hidden" value="<%#:Eval("FinalProcedureID") %>" bindingfield="FinalProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("FinalProcedureName") %>" bindingfield="FinalProcedureName" />
                                                    <input type="hidden" value="<%#:Eval("FinalProcedureText") %>" bindingfield="FinalProcedureText" />
                                                    <input type="hidden" value="<%#:Eval("cfFinalProcedureDateInDatePickerFormat") %>"
                                                        bindingfield="FinalProcedureDate" />
                                                    <input type="hidden" value="<%#:Eval("FinalProcedureTime") %>" bindingfield="FinalProcedureTime" />
                                                    <input type="hidden" value="<%#:Eval("ClaimProcedureID") %>" bindingfield="ClaimProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("ClaimProcedureName") %>" bindingfield="ClaimProcedureName" />
                                                    <input type="hidden" value="<%#:Eval("ClaimProcedureText") %>" bindingfield="ClaimProcedureText" />
                                                    <input type="hidden" value="<%#:Eval("ClaimINAProcedureID") %>" bindingfield="ClaimINAProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("ClaimINACBGINAProcedureID") %>" bindingfield="ClaimINACBGINAProcedureID" />
                                                    <input type="hidden" value="<%#:Eval("cfClaimProcedureDateInDatePickerFormat") %>"
                                                        bindingfield="ClaimProcedureDate" />
                                                    <input type="hidden" value="<%#:Eval("ClaimProcedureTime") %>" bindingfield="ClaimProcedureTime" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("IsCreatedBySystem") %>" bindingfield="IsCreatedBySystem" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Prosedur/Tindakan oleh Dokter") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%# Eval("ProcedureDateInString") == "" ? "display:none;": "" %>'>
                                                        <div style="float: left; width: 120px;">
                                                            <%#: Eval("ProcedureDateInString")%>,
                                                            <%#: Eval("ProcedureTime")%></div>
                                                        <div style="float: left; width: 300px;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div style="clear: both; font-weight: bold">
                                                            <%#: Eval("cfProcedureText")%>
                                                            (<%#: Eval("ProcedureID")%>)</div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Prosedur/Tindakan oleh Rekam Medis") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%# Eval("cfFinalProcedureDateInString") == "" ? "display:none;": "" %>'>
                                                        <div style="float: left; width: 120px;">
                                                            <%#: Eval("cfFinalProcedureDateInString")%>,
                                                            <%#: Eval("FinalProcedureTime")%></div>
                                                        <div style="float: left; width: 300px;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div style="clear: both; font-weight: bold">
                                                            <%#: Eval("FinalProcedureName")%>
                                                            (<%#: Eval("FinalProcedureID")%>)</div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Prosedur/Tindakan E-Klaim") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%# Eval("cfClaimProcedureDateInString") == "" ? "display:none;": "" %>'>
                                                        <div style="float: left; width: 120px;">
                                                            <%#: Eval("cfClaimProcedureDateInString")%>,
                                                            <%#: Eval("ClaimProcedureTime")%></div>
                                                        <div style="float: left; width: 300px;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div style="clear: both; font-weight: bold">
                                                            <%#: Eval("ClaimProcedureName")%>
                                                            (<%#: Eval("ClaimProcedureID")%>)</div>
                                                    </div>
                                                    <div>
                                                        <label>
                                                            <%=GetLabel("----------------------------------------------------------------------------------------------------------")%>
                                                        </label>
                                                    </div>
                                                    <div>
                                                        <label style="font-style: italic">
                                                            E-Klaim Prosedur/Tindakan V5
                                                        </label>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("cfClaimINAProcedureName")%></b></div>
                                                        <label style="font-style: italic">
                                                            E-Klaim Prosedur/Tindakan V6
                                                        </label>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("cfClaimINACBGINAProcedureName")%></b></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="<%#: Eval("ID") %>">
                                                        <div>
                                                            <label class="lblLink" id="lblProsedurEdit" data-val="<%#: Eval("ID") %>">
                                                                Ubah Prosedur Klaim</label>
                                                            <label class="lblLink" id="lblProsedurDelete" data-val="<%#: Eval("ID")%>" style='<%#: Eval("ProcedureID") != "" ?  "display:none;": ""  %>'>
                                                                | Hapus Prosedur Klaim</label>
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
                    <div class="imgLoadingGrdView" id="Div1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </div>
        </div>
        <h4>
            <%=GetLabel("Data Klinis")%></h4>
        <div>
            <table class="tblContentArea" width="100%">
                <colgroup>
                    <col style="width: 180px" />
                    <col style="width: 10px" />
                    <col style="width: 150px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col style="width: 120px" />
                    <col />
                </colgroup>
                <tr id="trSistoleDiastole" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Tekanan Darah (mmHg)")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Sistole")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSistole" Width="100px" runat="server" CssClass="number" Text="0" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Diastole")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDiastole" Width="100px" runat="server" CssClass="number" Text="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="20">
                        <div>
                            <hr style="margin: 0 0 0 0;" />
                        </div>
                    </td>
                </tr>
                <tr id="trAPGARScore" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("APGAR Score")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                </tr>
                <tr id="trAPGARScore1menit" runat="server">
                    <td align="right" style="font-weight: bold">
                        <%=GetLabel("1 menit")%>
                    </td>
                    <td style="font-weight: bold">
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Appearance")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit1Appearance" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Pulse")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit1Pulse" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Grimace")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit1Grimace" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Activity")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit1Activity" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Respiration")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit1Respiration" Width="100px" runat="server" CssClass="number" />
                    </td>
                </tr>
                <tr id="trAPGARScore5menit" runat="server">
                    <td align="right" style="font-weight: bold">
                        <%=GetLabel("5 menit")%>
                    </td>
                    <td style="font-weight: bold">
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Appearance")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit5Appearance" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Pulse")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit5Pulse" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Grimace")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit5Grimace" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Activity")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit5Activity" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Respiration")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimAPGARMenit5Respiration" Width="100px" runat="server" CssClass="number" />
                    </td>
                </tr>
                <tr>
                    <td colspan="20">
                        <div>
                            <hr style="margin: 0 0 0 0;" />
                        </div>
                    </td>
                </tr>
                <tr id="trDializer" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Penggunaan Dializer")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td colspan="3" align="center">
                        <asp:RadioButtonList runat="server" ID="rblDializer" RepeatDirection="Horizontal"
                            Style="width: 300px">
                            <asp:ListItem Text=" Multiple Use (reuse) " Value="0" />
                            <asp:ListItem Text=" Single Use " Value="1" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trTransfusiDarah" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Transfusi Darah")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td align="right">
                        <%=GetLabel("Jmlh Kantong Darah")%>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtEKlaimKantongDarah" Width="100px" runat="server" CssClass="number" />
                        <%=GetLabel("kantong")%>
                    </td>
                </tr>
                <tr>
                    <td colspan="20">
                        <div>
                            <hr style="margin: 0 0 0 0;" />
                        </div>
                    </td>
                </tr>
                <tr id="trStatusBayiLahir" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Status Bayi Lahir")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td colspan="3" align="center">
                        <asp:RadioButtonList runat="server" ID="rblStatusBayiLahir" RepeatDirection="Horizontal"
                            Style="width: 300px">
                            <asp:ListItem Text=" Tanpa Kelainan " Value="1" />
                            <asp:ListItem Text=" Dengan Kelainan " Value="2" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trPesalinan" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Persalinan")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Usia Kehamilan")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimPersalinanUsiaKehamilan" Width="100px" runat="server" CssClass="number"
                            placeholder="( minggu )" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Gravida")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimPersalinanGravida" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Partus")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimPersalinanPartus" Width="100px" runat="server" CssClass="number" />
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Abortus")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimPersalinanAbortus" Width="100px" runat="server" CssClass="number" />
                    </td>
                </tr>
                <tr id="trPersalinan2" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("On-set Kontraksi")%>
                    </td>
                    <td colspan="5">
                        <asp:RadioButtonList runat="server" ID="rblOnSetKontraksi" RepeatDirection="Horizontal"
                            Style="width: 500px">
                            <asp:ListItem Text=" Spontan " Value="spontan" />
                            <asp:ListItem Text=" Induksi " Value="induksi" />
                            <asp:ListItem Text=" Non-Spontan Non-Induksi " Value="non_spontan_non_induksi" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDelivery" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Delivery")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Urutan")%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEKlaimDeliverySequence" Width="100px" runat="server" CssClass="number" />
                    </td>
                </tr>
                <tr id="trDeliveryMethod" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Metode")%>
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rblDeliveryMethod" RepeatDirection="Horizontal"
                            Style="width: 150px">
                            <asp:ListItem Text=" Vaginal " Value="vaginal" />
                            <asp:ListItem Text=" SC " Value="sc" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliveryLetakJaninKondisi" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Letak Janin")%>
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rblDeliveryLetakJanin" RepeatDirection="Horizontal"
                            Style="width: 300px">
                            <asp:ListItem Text=" Kepala " Value="kepala" />
                            <asp:ListItem Text=" Sungsang " Value="sungsang" />
                            <asp:ListItem Text=" Lintang " Value="lintang" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Kondisi")%>
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rblDeliveryKondisi" RepeatDirection="Horizontal"
                            Style="width: 300px">
                            <asp:ListItem Text=" Live Birth " Value="livebirth" />
                            <asp:ListItem Text=" Still Birth " Value="stillbirth" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliveryUse" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Use-Manual")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDeliveryUseManual" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Use-Forcep")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDeliveryUseForcep" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Use-Vacuum")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDeliveryUseVacuum" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliverySHKSpesimenAmbil" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("SHK")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Spesimen Ambil")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDeliverySHKSpesienAmbil" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="ya" />
                            <asp:ListItem Text=" Tidak " Value="tidak" Selected="True" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliveryAlasan" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Alasan Tidak Diambil")%>
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rblDeliverySHKAlasan" RepeatDirection="Horizontal"
                            Style="width: 250px">
                            <asp:ListItem Text=" Tidak Dapat " Value="tidak-dapat" Selected="True" />
                            <asp:ListItem Text=" Akses Sulit " Value="akses-sulit" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliveryLokasi" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Lokasi")%>
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList runat="server" ID="rblDeliverySHKLokasi" RepeatDirection="Horizontal"
                            Style="width: 250px">
                            <asp:ListItem Text=" Tumit " Value="tumit" />
                            <asp:ListItem Text=" Vena " Value="vena" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trDeliveryTglJamSHK" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Tgl-Jam SHK")%>
                    </td>
                    <td colspan="3">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 50px" />
                                <col style="width: 10px" />
                                <col style="width: 50px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDeliverySHKDate" runat="server" CssClass="datepicker" Style="width: 120px"
                                        placeholder="dd-mm-yyyy" />
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtDeliverySHKTime1" runat="server" Style="width: 90%; text-align: center"
                                        placeholder="00-23" onkeypress="return isValidateNumber(event)" />
                                </td>
                                <td align="center">
                                    <label class="lblNormal">
                                        <%=GetLabel(":")%></label>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtDeliverySHKTime2" runat="server" Style="width: 90%; text-align: center"
                                        placeholder="00-59" onkeypress="return isValidateNumber(event)" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="20">
                        <div>
                            <hr style="margin: 0 0 0 0;" />
                        </div>
                    </td>
                </tr>
                <tr id="trPemulasaraanJenazah" runat="server">
                    <td style="font-weight: bold">
                        <%=GetLabel("Pemulasaraan Jenazah")%>
                    </td>
                    <td style="font-weight: bold">
                        <%=GetLabel(":")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblPemulasaraanJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trPemulasaraanJenazah2" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Kantong Jenazah")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblKantongJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Peti Jenazah")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblPetiJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Plastik Erat")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblPlastikErat" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trPemulasaraanJenazah3" runat="server">
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Desinfektan Jenazah")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDesinfektanJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Mobil Jenazah")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblMobilJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="font-style: italic">
                        <%=GetLabel("Desinfektan Mobil")%>
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblDesinfektanMobilJenazah" RepeatDirection="Horizontal"
                            Style="width: 110px">
                            <asp:ListItem Text=" Ya " Value="1" />
                            <asp:ListItem Text=" Tidak " Value="0" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
                ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent4" runat="server">
                        <input type="hidden" runat="server" id="hdnIsGrouperStage2" value="0" />
                        <div id="divGrouper" runat="server">
                            <div style="position: relative; padding: 10px;" id="v5Grouper">
                                <h4>
                                    Hasil Grouper E-Klaim v5</h4>
                                <table cellspacing="0" class="tblEntryContent" style="font-size: 16px">
                                    <colgroup>
                                        <col style="width: 300px;" />
                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <label>
                                                    Jenis Rawat</label>
                                            </td>
                                            <td colspan="4" id="JenisRawat" runat="server">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Group</label>
                                            </td>
                                            <td>
                                                <label id="grouperDescription" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                <label id="grouperCode" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="grouperValue1" runat="server" class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Sub Acute</label>
                                            </td>
                                            <td>
                                                <label>
                                                    -</label>
                                            </td>
                                            <td>
                                                <label>
                                                    -</label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="grouperValue2" runat="server" class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Chronic</label>
                                            </td>
                                            <td>
                                                <label>
                                                    -</label>
                                            </td>
                                            <td>
                                                <label>
                                                    -</label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="grouperValue3" runat="server" class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Special Procedure</label>
                                            </td>
                                            <td>
                                                <label>
                                                    <dxe:ASPxComboBox ID="cboSpecialProcedure" ClientInstanceName="cboSpecialProcedure"
                                                        Width="200px" runat="server" />
                                                </label>
                                            </td>
                                            <td>
                                                <label id="lblSpecialProcedure" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="txtSpecialProcedureAmount" runat="server"
                                                    class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Special Prosthesis
                                                </label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboSpecialProsthesis" ClientInstanceName="cboSpecialProsthesis"
                                                    Width="200px" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblSpecialProsthesis" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="txtSpecialProsthesisAmount" runat="server"
                                                    class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Special Investigation
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboSpecialInvestigation" ClientInstanceName="cboSpecialInvestigation"
                                                    Width="200px" runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblSpecialInvestigation" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="txtSpecialInvestigationAmount" runat="server"
                                                    class="txtCurrency" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Special Drug
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboSpecialDrug" ClientInstanceName="cboSpecialDrug" Width="200px"
                                                    runat="server" />
                                            </td>
                                            <td>
                                                <label id="lblSpecialDrugCode" runat="server">
                                                </label>
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" disabled="disabled" id="txtSpecialDrugAmount" runat="server" class="txtCurrency"
                                                    value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                TOTAL
                                            </td>
                                            <td>
                                                Rp.
                                            </td>
                                            <td>
                                                <input type="text" id="txtTotalAmountCBG" runat="server" class="txtCurrency" value="0"
                                                    disabled="disabled" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style="position: relative; padding: 10px; display: none;" id="v6Grouper">
                                <h4>
                                    Hasil Grouper E-Klaim v6</h4>
                                <table width="100%" cellspacing="0" class="tblEntryContent" style="font-size: 16px">
                                    <colgroup>
                                        <col style="width: 300px;" />
                                        <col style="width: 400px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            MDC
                                        </td>
                                        <td>
                                            <label id="v6_MDC">
                                            </label>
                                        </td>
                                        <td>
                                            <label id="v6_MDC_SCORE">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            DRG
                                        </td>
                                        <td>
                                            <label id="v6_DRG">
                                            </label>
                                        </td>
                                        <td>
                                            <label id="v6_DRG_SCORE">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="position: relative; padding: 10px;">
                                <h4>
                                    Informasi Data E-Klaim</h4>
                                <table width="100%" cellspacing="0" class="tblEntryContent" style="font-size: 16px">
                                    <colgroup>
                                        <col style="width: 250px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            Status Klaim
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label id="lblklaim_status_cd">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Status DC Kemenkes
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label id="lblkemenkes_dc_status_cd">
                                            </label>
                                            <label id="lblkemenkes_dc_sent_dttm">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Status BPJS Data Center
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label id="lblbpjs_dc_status_cd">
                                            </label>
                                            <label id="bpjs_dc_sent_dttm">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            BPJS Klaim Status CD
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label id="lblbpjs_klaim_status_cd">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            BPJS Klaim Status NM
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label id="lblbpjs_klaim_status_nm">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
