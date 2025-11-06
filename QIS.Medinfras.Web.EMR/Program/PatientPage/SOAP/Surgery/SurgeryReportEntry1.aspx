<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="SurgeryReportEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.SurgeryReportEntry1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackToList" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Kembali ke Daftar")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnDiscardChanges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Batal Perubahan")%></div>
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
                    Laporan Operasi</div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .templatePatientBodyDiagram
        {
            padding: 10px;
        }
        .templatePatientBodyDiagram .containerImage
        {
            float: left;
            display: table-cell;
            vertical-align: middle;
            border: 1px solid #AAA;
            width: 300px;
            height: 300px;
            margin-right: 20px;
            text-align: center;
            position: relative;
        }
        .templatePatientBodyDiagram .containerImage img
        {
            max-height: 300px;
            max-width: 300px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }
        .templatePatientBodyDiagram .spLabel
        {
            display: inline-block;
            width: 120px;
            font-weight: bolder;
        }
        .templatePatientBodyDiagram .spValue
        {
            margin-left: 10px;
        }
    </style>
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            $('#<%=btnBackToList.ClientID %>').click(function () {
                if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    if (validateTime($('#<%=txtReportTime.ClientID %>').val())) {
                        if (validateTime($('#<%=txtStartTime.ClientID %>').val())) {
                            if ($('#<%=rblIsUsingProfilaksis.ClientID %> ').val() == "1") {
                                if (validateTime($('#<%=txtProfilaxisTime.ClientID %>').val())) {
                                    PromptUserToSave();
                                }
                                else
                                {
                                    displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Waktu Pemberian yang diinput salah');    
                                }
                            }
                        }
                        else
                        {
                            displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Mulai Operasi yang diinput salah');    
                        }
                    }
                    else {
                        displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Laporan yang diinput salah');                      
                    }
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            $('#<%=txtEndTime.ClientID %>').change(function () {
                if (validateTime($('#<%=txtEndTime.ClientID %>').val())) {
                    CalculateDuration();
                } else {
                    displayErrorMessageBox('Perhitungan Lama Operasi', "Format waktu yang diisikan di jam selesai operasi tidak valid");
                }
            });

            $('#<%=txtDuration.ClientID %>').change(function () {
                if ($('#<%=txtDuration.ClientID %>').val() > 0) {
                    CalculateEndDate($('#<%=txtDuration.ClientID %>').val());
                } else {
                    displayErrorMessageBox('Perhitungan Lama Operasi', "Lama operasi harus lebih besar dari 0");
                }
            });
                        
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

            setDatePicker('<%=txtReportDate.ClientID %>');
            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtEndDate.ClientID %>');
            $('#<%=txtReportDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtEndDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    if (validateTime($('#<%=txtReportTime.ClientID %>').val())) {
                        if (validateTime($('#<%=txtStartTime.ClientID %>').val())) {
                             if ($('#<%=rblIsUsingProfilaksis.ClientID %> ').val() == "1") {
                                if (validateTime($('#<%=txtProfilaxisTime.ClientID %>').val())) {
                                    onCustomButtonClick('save');
                                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                                }
                                else
                                {
                                    displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Waktu Pemberian yang diinput salah');    
                                }
                            }
                            else {
                                onCustomButtonClick('save');
                                $('#<%=hdnIsChanged.ClientID %>').val('0');
                            }
                        }
                        else
                        {
                            displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Mulai Operasi yang diinput salah');    
                        }
                    }
                    else {
                        displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Laporan yang diinput salah');                      
                    }
                }
            });

            $('#<%=btnDiscardChanges.ClientID %>').click(function (evt) {
                if ($('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                    var message = "Are you sure to discard your changes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) {
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
                            location.reload(true);
                        }
                    });
                }
            });

            $('#<%=grdImplantView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdImplantView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAllergyID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdImplantView.ClientID %> tr:eq(1)').click();

            $('#<%=grdParamedicTeamView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdParamedicTeamView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnOrderDtParamedicTeamID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

            $('#<%=grdProcedureGroupView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdProcedureGroupView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnOrderDtProcedureGroupID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

            $('#btnBodyDiagramContainerPrev').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('prev');
            });
            $('#btnBodyDiagramContainerNext').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpBodyDiagramView.PerformCallback('next');
            });

            $('#imgEditBodyDiagram').live('click', function () {
                onBeforeOpenTrxPopup();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPEdit1Ctl.ascx", $('#<%=hdnBodyDiagramID.ClientID %>').val(), "Body Diagram", 1200, 600);
            });

            $('#imgDeleteBodyDiagram').live('click', function () {
                var message = "Are you sure to delete this body diagram ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        cbpDeleteBodyDiagram.PerformCallback();
                    }
                });
            });

            $('#<%=txtRemarks.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%:txtRemarks.ClientID %>').blur(function () {
                ontxtRemarksChanged($(this).val());
            });
          
            $('#<%=rblSurgeryNoType.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "0") {
                    $('#<%=txtSurgeryNo.ClientID %>').removeAttr("disabled");                                        
                }
                else
                {
                    $('#<%=txtSurgeryNo.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsUsingProfilaksis.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtProfilaxis.ClientID %>').removeAttr("disabled");      
                    $('#<%=txtProfilaxisTime.ClientID %>').removeAttr("disabled");                                        
                }
                else
                {
                    $('#<%=txtProfilaxis.ClientID %>').attr("disabled", "disabled");
                    $('#<%=txtProfilaxisTime.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsHasComplexity.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtComplexityRemarks.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtComplexityRemarks.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsHemorrhage.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtHemorrhage.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtHemorrhage.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsBloodDrain.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtOtherBloodDrainType.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtOtherBloodDrainType.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsUsingTampon.ClientID %> input').change(function () {
                if ($(this).val() == "1") {
                    $('#<%=txtTamponType.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtTamponType.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsBloodTransfussion.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtBloodTransfussion.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtBloodTransfussion.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsTestKultur.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtOtherTestKulturType.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtOtherTestKulturType.ClientID %>').attr("disabled", "disabled");
                }
            });

            $('#<%=rblIsTestCytology.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    $('#<%=txtOtherTestCytologyType.ClientID %>').removeAttr("disabled");                                    
                }
                else
                {
                    $('#<%=txtOtherTestCytologyType.ClientID %>').attr("disabled", "disabled");
                }
            });
            
            $('#<%=rblIsSpecimenTest.ClientID %> input').change(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                if ($(this).val() == "1") {
                    cbpSpecimen.PerformCallback('enable');
                }
                else
                {
                    cbpSpecimen.PerformCallback('disabled');
                }
            });

            registerCollapseExpandHandler();

            $('#leftPageNavPanel ul li').first().click();
        });

		function onCboSpecimenValueChanged(s) {
            $('#<%=hdnSpecimenID.ClientID %>').val(cboSpecimen.GetValue());
            var specimenID = cboSpecimen.GetValue();
            var filterExpression = "SpecimenID = '" + specimenID + "'";
            Methods.getObject('GetSpecimenList', filterExpression, function (result) {
                if (result != null) {
                    var specimenCode = result.SpecimenCode;
                    if (specimenCode != 'SP999') {
                        $('#<%=trOtherSpecimenType.ClientID %>').attr('style', 'display:none');
                        $('#<%=txtOtherSpecimenType.ClientID %>').val('');
                    }
                    else {
                        $('#<%=trOtherSpecimenType.ClientID %>').removeAttr('style');
                    }
                }
            });
        }

        //#region Remarks
        $('#lblSurgeryRemarks').die('click');
        $('#lblSurgeryRemarks').live('click', function (evt) {
            var visitNoteID = 0;
            var param = "X058^09|" + visitNoteID + "|1|1";
            openUserControlPopup("~/libs/Controls/EMR/Lookup/SOAPTemplateLookupCtl1.ascx", param, "Clinical Noting Template Text", 700, 500);
        });

        function ontxtRemarksChanged(value) {
            if (value.length <= 11 && value.slice(-1) == ";") {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
                var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^09'";
                Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                    if (obj != null)
                        if ($('#<%=txtRemarks.ClientID %>').val() != '') {
                            var message1 = "Ganti text di uraian pembedahan dengan text dari template ?";
                            var message2 = "<i>"+obj.TemplateText+"</i>";
                            displayConfirmationMessageBox('TEMPLATE TEXT', message1+"<br/><br/>"+message2, function (result) {
                                if (result) {
                                    $('#<%=txtRemarks.ClientID %>').val(obj.TemplateText);
                                }
                            });
                        }
                });
            }
        }

        function onAfterLookUpSOAPTemplate(value) {
            var valueInfo = value.split('|');
            switch (valueInfo[1]) {
                case 'X058^09':
                    $('#<%=txtRemarks.ClientID %>').val(valueInfo[0]+';');
                    ontxtRemarksChanged($('#<%=txtRemarks.ClientID %>').val());
                    break;
                default:
                    displayErrorMessageBox("ERROR : TEMPLATE TEXT","Unhandled Template Text Type");
                    break;
            }
        }    
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
        }


        //#region Implant
        function oncbpImplantViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdImplantView.ClientID %> tr:eq(1)').click();

                setPaging($("#implantPaging"), pageCount, function (page) {
                    cbpImplantView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdImplantView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Pemasangan Implant

        $('#lblAddImplantDevice').die('click');
        $('#lblAddImplantDevice').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnTestOrderID.ClientID %>').val();
                openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
            }
        });

        $('.imgAddDevice.imgLink').die('click');
        $('.imgAddDevice.imgLink').live('click', function (evt) {
            var allow = true;
            if (typeof onBeforeBasePatientPageListAdd == 'function') {
                allow = onBeforeBasePatientPageListAdd();
            }
            if (allow) {
                var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
                var param = "0" + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnTestOrderID.ClientID %>').val();
                openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
            }
        });

        $('.imgEditDevice.imgLink').die('click');
        $('.imgEditDevice.imgLink').live('click', function (evt) {
            var recordID = $(this).attr('recordID');
            var url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/PatientMedicalDevice/PatientMedicalDeviceEntryCtl.ascx");
            var param = recordID + "|" + $('#<%=hdnVisitID.ClientID %>').val() + "|" + $('#<%=hdnMRN.ClientID %>').val() + "|" + $('#<%=hdnTestOrderID.ClientID %>').val();
            openUserControlPopup(url, param, "Pemasangan Implant", 700, 500);
        });

        $('.imgDeleteDevice.imgLink').die('click');
        $('.imgDeleteDevice.imgLink').live('click', function () {
            var recordID = $(this).attr('recordID');
            var itemName = $(this).attr('itemName');
            var message = "Hapus informasi implant " + itemName + " dari Pengkajian Kamar Operasi untuk pasien ini ?";
            displayConfirmationMessageBox("Pemasangan Implant", message, function (result) {
                if (result) {
                    cbpDeleteDevice.PerformCallback(recordID);
                }
            });
        });
        //#endregion

        //#region Clinical Noting Template Text
        $('#btnAddTemplate').die('click');
        $('#btnAddTemplate').live('click', function (evt) {
            if ($('#<%=txtRemarks.ClientID %>').val() != '') {
                onBeforeOpenTrxPopup();
                var text = $('#<%=txtRemarks.ClientID %>').val();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/TemplateText/UpdatePhysicianTextCtl1.ascx", "X058^09|" + text, "Physician Template Text", 700, 500);
            }
        });
        //#endregion

        //#region Paramedic Team
        $('#lblAddTeamFromOrder').die('click');
        $('#lblAddTeamFromOrder').live('click', function (evt) {
             var reportID = $('#<%=hdnReportID.ClientID %>').val();
            if (reportID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var param = "0|" + testOrderID + "|" + reportID;
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/Surgery/SurgeryParamedicTeamLookupCtl1.ascx", param, "Salin Team Pelaksana : Tindakan Operasi", 700, 500);
            }
            else {
                displayMessageBox("Laporan Operasi","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.imgAddParamedicTeam.imgLink').die('click');
        $('.imgAddParamedicTeam.imgLink').live('click', function () {
            $('#trParamedicTeamEntry').removeAttr('style');
            ResetParamedicTeamEntryControls();
            $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val("1");
            $('#<%=ledParamedicTeam.ClientID %>').focus();
        });

        $('.imgEditParamedicTeam.imgLink').die('click');
        $('.imgEditParamedicTeam.imgLink').live('click', function () {
            $('#trParamedicTeamEntry').removeAttr('style');
            SetParamedicTeamEntityToControl(this);
            $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('0');
        });

        $('.imgDeleteParamedicTeam.imgLink').die('click');
        $('.imgDeleteParamedicTeam.imgLink').live('click', function () {
            var recordID = $(this).attr("recordID");
            var paramedicName = $(this).attr("paramedicName");
            $('#<%=hdnOrderDtParamedicTeamID.ClientID %>').val(recordID);

            var message = "Hapus anggota Team Pelaksana <b>'" + paramedicName + "'</b> untuk pasien ini ?";
            displayConfirmationMessageBox('JENIS OPERASI', message, function (result) {
                if (result) {
                    cbpParamedicTeam.PerformCallback('delete');
                }
            });
        });

        function onLedParamedicLostFocus(led) {
            var paramedicID = led.GetValueText();
            $('#<%=hdnEntryParamedicID.ClientID %>').val(paramedicID);
        }

        function SetParamedicTeamEntityToControl(param) {
            var selectedObj = {};
            selectedObj = GetCurrentSelectedParamedicTeam(param);

            ledParamedicTeam.SetValue(selectedObj.ParamedicID);
            cboParamedicType.SetValue(selectedObj.GCParamedicRole);

            $('#<%=hdnEntryParamedicID.ClientID %>').val(selectedObj.ParamedicID);
        }

        function GetCurrentSelectedParamedicTeam(s) {
            var $tr = $(s).closest('tr').parent().closest('tr');
            var idx = $('#<%=grdParamedicTeamView.ClientID %> tr').index($tr);
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(' + idx + ')').click();

            $row = $('#<%=grdParamedicTeamView.ClientID %> tr.selected');
            var selectedObj = {};

            $row.find('input[type=hidden]').each(function () {
                selectedObj[$(this).attr('bindingfield')] = $(this).val();
            });

            return selectedObj;
        }

        function oncbpParamedicTeamViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

                setPaging($("#paramedicPaging"), pageCount, function (page) {
                    cbpParamedicTeamView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
        }

        function onCbpParamedicTeamDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpParamedicTeamView.PerformCallback('refresh');
            }
            else {
                displayErrorMessageBox("Laporan Operasi - Team Pelaksana", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshParamedicTeamGrid() {
            cbpParamedicTeamView.PerformCallback('refresh');
        }

        $('.btnApplyParamedicTeam').die('click');
        $('.btnApplyParamedicTeam').live('click', function (evt) {
            submitParamedicTeam();
        });

        $('.btnCancelParamedicTeam').die('click');
        $('.btnCancelParamedicTeam').live('click', function (evt) {
            $('#trParamedicTeamEntry').attr('style', 'display:none');
            ResetParamedicTeamEntryControls();
        });

        function submitParamedicTeam() {
            if ($('#<%=hdnEntryParamedicID.ClientID %>').val() != '' && cboParamedicType.GetValue() != null) {
                if ($('#<%=hdnParamedicTeamProcessMode.ClientID %>').val() == "1")
                    cbpParamedicTeam.PerformCallback('add');
                else
                    cbpParamedicTeam.PerformCallback('edit');
            }
            else {
                displayErrorMessageBox("ERROR", "Dokter/Tenaga Medis harus dipilih sebelum diproses !");
            }
        }


        function ResetParamedicTeamEntryControls(s) {
            ledParamedicTeam.SetValue('');
            cboParamedicType.SetValue('');
            $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('1');
        }

        function onCbpParamedicTeamEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == "edit")
                {
                    $('#<%=hdnParamedicTeamProcessMode.ClientID %>').val('0');
                    $('#trParamedicTeamEntry').attr('style', 'display:none');
                }
                else{
                    ResetParamedicTeamEntryControls();
                }
                cbpParamedicTeamView.PerformCallback('refresh');
            }
            else if (param[0] == '0') {
                displayErrorMessageBox('TEAM PELAKSANA', param[2]);
            }
            else
                $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Body Diagram
        $('#lblAddBodyDiagram').die('click');
        $('#lblAddBodyDiagram').live('click', function (evt) {
            var assessmentID = $('#<%=hdnReportID.ClientID %>').val();
            if (assessmentID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                openUserControlPopup("~/Program/PatientPage/_PopupEntry/BodyDiagram/BodyDiagramSOAPAdd1Ctl.ascx", testOrderID, "Body Diagram", 1200, 600);
            }
            else {
                displayMessageBox("Asesmen Pra Bedah","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        function onCbpBodyDiagramViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram.ClientID %>').show();
                    $('#<%=tblEmpty.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram.ClientID %>').hide();
                    $('#<%=tblEmpty.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }

        function onRefreshBodyDiagram(filterExpression) {
            if (filterExpression == 'edit')
                cbpBodyDiagramView.PerformCallback('edit');
            else
                cbpBodyDiagramView.PerformCallback('refresh');
        }

        function onCbpBodyDiagramDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpBodyDiagramView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", param[1]);
            }
        }
        //#endregion

        //#region Diagnosis
        function onLedDiagnoseLostFocus(led) {
            $('#<%=hdnIsChanged.ClientID %>').val('1');
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryDiagnoseText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtDiagnosisText.ClientID %>').val($('#<%=hdnEntryDiagnoseText.ClientID %>').val());
        }

        function onLedPostDiagnoseLostFocus(led) {
            $('#<%=hdnIsChanged.ClientID %>').val('1');
            var diagnoseID = led.GetValueText();
            $('#<%=hdnEntryPostDiagnoseID.ClientID %>').val(diagnoseID);
            $('#<%=hdnEntryPostDiagnoseText.ClientID %>').val(led.GetDisplayText());
            $('#<%=txtPostDiagnosisText.ClientID %>').val($('#<%=hdnEntryPostDiagnoseText.ClientID %>').val());
        }
        //#endregion
        
        //#region Procedure Group Button
        $('#lblAddProcedureFromOrder').die('click');
        $('#lblAddProcedureFromOrder').live('click', function (evt) {
            var reportID = $('#<%=hdnReportID.ClientID %>').val();
            if (reportID != '0') {
                onBeforeOpenTrxPopup();
                var testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var param = "0|" + testOrderID + "|" + reportID;
                openUserControlPopup("~/libs/Controls/EMR/_PopupEntry/Surgery/SurgeryProcedureGroupLookupCtl1.ascx", param, "Salin Tindakan Operasi", 700, 500);
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            }
            else {
                displayMessageBox("Laporan Operasi","Mohon simpan data terlebih dahulu, sebelum melakukan proses ini.");
            }
        });

        $('.btnApplyProcedureGroup').die('click');
        $('.btnApplyProcedureGroup').live('click', function (evt) {
            submitProcedureGroup();
            $('#<%=ledProcedureGroup.ClientID %>').focus();
            $('#trProcedureGroupEntry').attr('style', 'display:none');
        });

        $('.btnCancelProcedureGroup').die('click');
        $('.btnCancelProcedureGroup').live('click', function (evt) {
            $('#trProcedureGroupEntry').attr('style', 'display:none');
            ResetProcedureGroupEntryControls();
        });

        function submitProcedureGroup() { 
            if ($('#<%=hdnReportID.ClientID %>').val() != '') {
                if ($('#<%=hdnEntryProcedureGroupID.ClientID %>').val() != '') {
                    if ($('#<%=hdnProcedureGroupProcessMode.ClientID %>').val() == "1")
                        cbpProcedureGroup.PerformCallback('add');
                    else
                        cbpProcedureGroup.PerformCallback('edit');
                }
                else {
                    displayErrorMessageBox("ERROR", "Jenis prosedur operasi harus dipilih sebelum diproses !");
                }
            } else {
                displayErrorMessageBox("ERROR", "Harap simpan laporan operasi (tombol di toolbar) terlebih dahulu !");
            }
        }

        function onRefreshProcedureGroupGrid() {
            cbpProcedureGroupView.PerformCallback('refresh');
        }

        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (code == 'healthyinformation' || code == 'medicalSickLeave' || code == 'medicalSickLeaveBilingual') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function onCbpDeleteTestOrderEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                if (param[1] == 'LB') {
                    cbpLaboratoryView.PerformCallback('refresh');
                }
                else if (param[1] == 'IS') {
                    cbpImagingView.PerformCallback('refresh');
                }
                else {
                    cbpDiagnosticView.PerformCallback('refresh');
                }
            }
            else {
                showToast("ERROR", param[1]);
            }
        }

        //#region Change Page - Save
        function validateTime(timeValue) {
            var result = true;
            if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
                result = false;
            }
            else {
                var sHours = timeValue.split(':')[0];
                var sMinutes = timeValue.split(':')[1];

                if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                    result = false;
                }
                else if (parseInt(sHours) == 0)
                    sHours = "00";
                else if (sHours < 10)
                    sHours = "0" + sHours;

                if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                    result = false;
                }
                else if (parseInt(sMinutes) == 0)
                    sMinutes = "00";
                else if (sMinutes < 10)
                    sMinutes = "0" + sMinutes;
            }
            return result;
        }

        function CalculateDuration() {
            var startDateText = $('#<%=txtStartDate.ClientID %>').val();
            var startTimeText = $('#<%=txtStartTime.ClientID %>').val();

            var endDateText = $('#<%=txtEndDate.ClientID  %>').val();
            var endTimeText = $('#<%=txtEndTime.ClientID %>').val();

            var startDate = Methods.getDatePickerDate(startDateText);
            var endDate = Methods.getDatePickerDate(endDateText);

            var dateDiff = Methods.calculateDateDifference(startDate, endDate);
            $h1 = parseInt(endTimeText.substring(0, 2), 10);
            $m1 = parseInt(endTimeText.substring(3, 5), 10);

            $h2 = parseInt(startTimeText.substring(0, 2), 10);
            $m2 = parseInt(startTimeText.substring(3, 5), 10);

            if ($m1 < $m2) {
                $m = $m1 + 60 - $m2;
                $h1 -= 1;
            }
            else $m = $m1 - $m2;

            if (dateDiff.days > 0)
                $h1 = dateDiff.days * 24 + $h1;
            $h = $h1 - $h2;

            var duration = ($h*60) + $m;

            $('#<%=txtDuration.ClientID %>').val(duration);
        }

        function CalculateEndDate(duration) {
            var startDateText = $('#<%=txtStartDate.ClientID %>').val();
            var startTimeText = $('#<%=txtStartTime.ClientID %>').val();

            var startDate = Methods.convertToDateTime(startDateText, startTimeText);
            var endDate = Methods.calculateEndDate(startDateText, startTimeText, duration);
    
            $('#<%=txtEndDate.ClientID %>').val(Methods.dateToDatePickerFormat(endDate));
            $('#<%=txtEndTime.ClientID %>').val(Methods.getTimeFromDate(endDate));
        }



        function onBeforeOpenTrxPopup() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                if (validateTime($('#<%=txtReportTime.ClientID %>').val())) {
                    if (validateTime($('#<%=txtStartTime.ClientID %>').val())) {
                          if ($('#<%=rblIsUsingProfilaksis.ClientID %> ').val() == "1") {
                          if (validateTime($('#<%=txtProfilaxisTime.ClientID %>').val())) {
                            onCustomButtonClick('save');
                            $('#<%=hdnIsChanged.ClientID %>').val('0');
						  }
						  else
						  {
							displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Waktu Pemberian yang diinput salah');    
						  }  
						}
                    }                             
                    else
                    {
                        displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Mulai Operasi yang diinput salah');    
                    }
                }
                else {
                    displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Laporan yang diinput salah');                      
                }
            }
        }

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                displayConfirmationMessageBox("Asesment",message, function (result) {
                    if (result) {
                        if (validateTime($('#<%=txtReportTime.ClientID %>').val())) {
                            if (validateTime($('#<%=txtStartTime.ClientID %>').val())) {
                                if ($('#<%=rblIsUsingProfilaksis.ClientID %> ').val() == "1") {
                                    if (validateTime($('#<%=txtProfilaxisTime.ClientID %>').val())) {
                                        onCustomButtonClick('save');
                                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                                    }
                                    else
                                    {
                                        displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Waktu Pemberian yang diinput salah');    
                                    }
                                }
                            }
                            else
                            {
                                displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Mulai Operasi yang diinput salah');    
                            }
                        }
                        else {
                            displayErrorMessageBox('Laporan Operasi', 'Format Waktu pengisian Jam Laporan yang diinput salah');                      
                        }
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
            var message = "Your record is not saved yet, Do you want to save ?";
            displayConfirmationMessageBox("Assessment",message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }
        //#endregion     
        
    //#region Procedure Group
    function onLedProcedureGroupLostFocus(s) {
        var procedureGroupID = s.GetValueText();
        $('#<%=hdnEntryProcedureGroupID.ClientID %>').val(procedureGroupID);
        $('#<%=hdnEntryProcedureGroupText.ClientID %>').val(s.GetDisplayText());
    }

    function GetCurrentSelectedProcedureGroup(s) {
        var $tr = $(s).closest('tr').parent().closest('tr');
        var idx = $('#<%=grdProcedureGroupView.ClientID %> tr').index($tr);
        $('#<%=grdProcedureGroupView.ClientID %> tr:eq(' + idx + ')').click();

        $row = $('#<%=grdProcedureGroupView.ClientID %> tr.selected');
        var selectedObj = {};

        $row.find('input[type=hidden]').each(function () {
            selectedObj[$(this).attr('bindingfield')] = $(this).val();
        });
        return selectedObj;
    }

    function SetProcedureGroupEntityToControl(param) {
        var selectedObj = {};
        selectedObj = GetCurrentSelectedProcedureGroup(param);
        ledProcedureGroup.SetValue(selectedObj.ProcedureGroupID);
        $('#<%=hdnEntryProcedureGroupID.ClientID %>').val(selectedObj.ProcedureGroupID);
    }

    function ResetProcedureGroupEntryControls(s) {
        ledProcedureGroup.SetValue('');
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('0');
    }

    $('.imgAddProcedureGroup.imgLink').die('click');
    $('.imgAddProcedureGroup.imgLink').live('click', function () {
        $('#trProcedureGroupEntry').removeAttr('style');
        ResetProcedureGroupEntryControls();
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("1");
        $('#<%=ledProcedureGroup.ClientID %>').focus();
    });

    $('.imgEditProcedureGroup.imgLink').die('click');
    $('.imgEditProcedureGroup.imgLink').live('click', function () {
        $('#trProcedureGroupEntry').removeAttr('style');
        $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val("0");
        SetProcedureGroupEntityToControl(this);
    });

    $('.imgDeleteProcedureGroup.imgLink').die('click');
    $('.imgDeleteProcedureGroup.imgLink').live('click', function () {
        var recordID = $(this).attr("recordID");
        var procedureGroupName = $(this).attr("procedureGroupName");
        $('#<%=hdnOrderDtProcedureGroupID.ClientID %>').val(recordID);

        var message = "Hapus Jenis Operasi <b>'" + procedureGroupName + "'</b> untuk pasien ini ?";
        displayConfirmationMessageBox('JENIS OPERASI', message, function (result) {
            if (result) {
                cbpProcedureGroup.PerformCallback('delete');
            }
        });
    });

    function onCbpProcedureGroupViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

            setPaging($("#procedureGroupPaging"), pageCount, function (page) {
                cbpProcedureGroupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }

    function onCbpProcedureGroupEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == '1') {
            if (param[1] == "edit")
                $('#<%=hdnProcedureGroupProcessMode.ClientID %>').val('0');

            ResetProcedureGroupEntryControls();
            cbpProcedureGroupView.PerformCallback('refresh');
        }
        else if (param[0] == '0') {
            displayErrorMessageBox('JENIS OPERASI', param[2]);
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onCbpDeleteDeviceEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'success') {
            cbpImplantView.PerformCallback('refresh');
        }
        else {
            displayErrorMessageBox('Pemasangan Implant', param[1]);
        }
    }

    function onGetLocalHiddenFieldValue(param) {
        var filterExpression = "PatientSurgeryID = " + param + " AND VisitID IN (" + $('#<%=hdnVisitID.ClientID %>').val() + ")";
        Methods.getObject('GetPatientSurgeryList', filterExpression, function (obj) {
            if (obj != null) {
                $('#<%=hdnReportID.ClientID %>').val(param);
            }
        });
    }

    function onAfterSaveAddRecordEntryPopup() {
        cbpImplantView.PerformCallback('refresh');
    }

    function onAfterSaveEditRecordEntryPopup() {
        cbpImplantView.PerformCallback('refresh');
    }
    </script>
    <div>
        <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
        <input type="hidden" id="hdnVisitID" value="" runat="server" />
        <input type="hidden" id="hdnMRN" value="" runat="server" />
        <input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
        <input type="hidden" runat="server" id="hdnReportID" value="0" />
        <input type="hidden" value="" id="hdnAllergyID" runat="server" />
        <input type="hidden" value="1" id="hdnAllergyProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
        <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
        <input type="hidden" value="" id="hdnReviewOfSystemID" runat="server" />
        <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
        <input type="hidden" runat="server" id="hdnPastMedicalID" value="" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" id="hdnPageCount" runat="server" value='0' />
        <input type="hidden" id="hdnPageIndex" runat="server" value='0' />
        <input type="hidden" id="hdnDiagnosisSummary" runat="server" />
        <input type="hidden" id="hdnPatientInformation" runat="server" />
        <input type="hidden" id="hdnIsPatientAllergyExists" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
        <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
        <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
        <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 20%" />
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <div id="leftPageNavPanel" class="w3-border">
                            <ul>
                                <li contentid="divPage1" title="Data Operasi (Informasi umum mengenai operasi)" class="w3-hover-red">
                                    Data Operasi</li>
                                <li contentid="divPage2" title="Diagnosa dan Jenis Tindakan Operasi" class="w3-hover-red">
                                    Diagnosa dan Jenis Tindakan Operasi</li>
                                <li contentid="divPage3" title="Team Pelaksana" class="w3-hover-red">Team Pelaksana</li>
                                <li contentid="divPage4" title="Pemasangan Implant" class="w3-hover-red">Pemasangan
                                    Implant</li>
                                <li contentid="divPage5" title="Uraian Pembedahan" class="w3-hover-red">Uraian Pembedahan</li>
                                <li contentid="divPage9" title="Pengkajian Riwayat Kesehatan" class="w3-hover-red"
                                    style="display: none">Pengkajian Riwayat Kesehatan</li>
                                <li contentid="divPage6" title="Hasil Pemeriksaan Penunjang yang telah teridentifikasi secara benar"
                                    class="w3-hover-red" style="display: none">Hasil Pemeriksaan Penunjang</li>
                                <li contentid="divPage7" title="Checklist Dokumen Terkait" class="w3-hover-red" style="display: none">
                                    Kelengkapan Berkas/Dokumen Terkait</li>
                                <li contentid="divPage8" title="Penandaan Lokasi Operasi" class="w3-hover-red" style="display: none">
                                    Penandaan Lokasi Operasi</li>
                            </ul>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left">
                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                <colgroup>
                                    <col style="width: 55%" />
                                    <col style="width: 45%" />
                                </colgroup>
                                <tr>
                                    <td style="vertical-align: top">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col width="150px" />
                                                            <col width="150px" />
                                                            <col width="100px" />
                                                            <col width="150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label id="lblOrderNo">
                                                                    <%:GetLabel("Nomor Order")%></label>
                                                            </td>
                                                            <td colspan="2">
                                                                <input type="hidden" id="hdnTestOrderID" value="" runat="server" />
                                                                <asp:TextBox ID="txtTestOrderNo" Width="225px" runat="server" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblMandatory">
                                                                    <%=GetLabel("Tanggal Laporan")%></label>
                                                            </td>
                                                            <td colspan="3">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col width="150px" />
                                                                        <col width="150px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtReportDate" Width="120px" CssClass="datepicker" runat="server" />
                                                                        </td>
                                                                        <td class="tdLabel" style="padding-left: 5px">
                                                                            <label class="lblMandatory">
                                                                                <%=GetLabel("Jam Laporan")%></label>
                                                                        </td>
                                                                        <td style="padding-left: 5px">
                                                                            <asp:TextBox ID="txtReportTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                                                MaxLength="5" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblMandatory">
                                                                    <%=GetLabel("Tanggal Operasi")%></label>
                                                            </td>
                                                            <td colspan="3">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col width="150px" />
                                                                        <col width="150px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStartDate" Width="120px" CssClass="datepicker" runat="server" />
                                                                        </td>
                                                                        <td class="tdLabel" style="padding-left: 5px">
                                                                            <label class="lblMandatory">
                                                                                <%=GetLabel("Jam Mulai Operasi")%></label>
                                                                        </td>
                                                                        <td style="padding-left: 5px">
                                                                            <asp:TextBox ID="txtStartTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                                                MaxLength="5" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Tanggal Selesai Operasi")%></label>
                                                            </td>
                                                            <td colspan="3">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col width="150px" />
                                                                        <col width="150px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEndDate" Width="120px" CssClass="datepicker" runat="server" />
                                                                        </td>
                                                                        <td class="tdLabel" style="padding-left: 5px">
                                                                            <label class="lblMandatory">
                                                                                <%=GetLabel("Jam Selesai Operasi")%></label>
                                                                        </td>
                                                                        <td style="padding-left: 5px">
                                                                            <asp:TextBox ID="txtEndTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                                                MaxLength="5" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblMandatory">
                                                                    <%=GetLabel("Lama Operasi") %>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server" />
                                                                menit
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblMandatory">
                                                                    <%=GetLabel("Operasi Ke-")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="1">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblSurgeryNoType" CssClass="rblSurgeryNoType" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" 1 (Pertama)" Value="1" />
                                                                                <asp:ListItem Text=" Re-Do" Value="0" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSurgeryNo" Width="50px" runat="server" Style="text-align: right"
                                                                                Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" style="width: 120px">
                                                                <label class="lblNormal" id="lblFamilyRelation">
                                                                    <%=GetLabel("Cara Pembiusan")%></label>
                                                            </td>
                                                            <td style="width: 130px">
                                                                <dxe:ASPxComboBox runat="server" ID="cboAnesthesiaType" ClientInstanceName="cboAnesthesiaType"
                                                                    Width="100%" />
                                                            </td>
                                                            <td class="tdLabel" style="width: 120px">
                                                                <label class="lblNormal" id="Label4">
                                                                    <%=GetLabel("Jenis Pembedahan")%></label>
                                                            </td>
                                                            <td style="width: 130px">
                                                                <dxe:ASPxComboBox runat="server" ID="cboWoundType" ClientInstanceName="cboWoundType"
                                                                    Width="100%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Antibiotika Profilaksis")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="1" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsUsingProfilaksis" CssClass="rblIsUsingProfilaksis"
                                                                                runat="server" RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtProfilaxis" runat="server" Width="200px" Enabled="false" />
                                                                        </td>
                                                                        <td style="padding-left: 2px">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Waktu Pemberian")%></label>
                                                                        </td>
                                                                        <td style="padding-left: 5px">
                                                                            <asp:TextBox ID="txtProfilaxisTime" Width="50px" runat="server" Style="text-align: center"
                                                                                Enabled="false" CssClass="time" MaxLength="5" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Komplikasi/Penyulit")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsHasComplexity" CssClass="rblIsHasComplexity" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtComplexityRemarks" runat="server" Width="99%" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Perdarahan")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsHemorrhage" CssClass="rblIsHemorrhage" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtHemorrhage" runat="server" Width="60px" Style="text-align: right"
                                                                                Enabled="false" />
                                                                            ml
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Drain")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsBloodDrain" CssClass="rblIsBloodDrain" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOtherBloodDrainType" runat="server" Width="99%" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Tampon")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsUsingTampon" CssClass="rblIsUsingTampon" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTamponType" runat="server" Width="99%" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Tourniquet")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsUsingTourniquet" CssClass="rblIsUsingTourniquet" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Transfusi")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsBloodTransfussion" CssClass="rblIsBloodTransfussion"
                                                                                runat="server" RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBloodTransfussion" runat="server" Width="60px" Style="text-align: right"
                                                                                Enabled="false" />
                                                                            ml
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Pemeriksaan Kultur")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsTestKultur" CssClass="rblIsTestKultur" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOtherTestKulturType" runat="server" Width="99%" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Pemeriksaan Sitologi")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsTestCytology" CssClass="rblIsTestCytology" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOtherTestCytologyType" runat="server" Width="99%" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Pemeriksaan Jaringan ke PA")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <colgroup>
                                                                        <col width="100px" />
                                                                        <col />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblIsSpecimenTest" CssClass="rblIsSpecimenTest" runat="server"
                                                                                RepeatDirection="Horizontal" CellPadding="2">
                                                                                <asp:ListItem Text=" Tidak" Value="0" />
                                                                                <asp:ListItem Text=" Ya" Value="1" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td class="tdLabel" style="width: 120px">
                                                                            <label class="lblNormal" id="Label2">
                                                                                <%=GetLabel("Jenis Spesimen")%></label>
                                                                        </td>
                                                                        <td style="width: 130px">
                                                                            <dxcp:ASPxCallbackPanel ID="cbpSpecimen" runat="server" Width="100%" ClientInstanceName="cbpSpecimen"
                                                                                ShowLoadingPanel="false" OnCallback="cbpSpecimen_Callback">
                                                                                <ClientSideEvents BeginCallback="function(s,e){}" EndCallback="function(s,e){}" />
                                                                                <PanelCollection>
                                                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                                                        <input type="hidden" id="hdnSpecimenID" runat="server" />
                                                                                        <dxe:ASPxComboBox ID="cboSpecimen" ClientInstanceName="cboSpecimen" Width="100%"
                                                                                            Enabled="false" runat="server">
                                                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboSpecimenValueChanged(s); }" />
                                                                                        </dxe:ASPxComboBox>
                                                                                    </dx:PanelContent>
                                                                                </PanelCollection>
                                                                            </dxcp:ASPxCallbackPanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trOtherSpecimenType" runat="server" style="display: none">
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOtherSpecimenType" Width="180px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align: top">
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Pre Diagnosis (ICD X)")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                        <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                        <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                            Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName"
                                            Value="DiagnoseID" MethodName="GetvDiagnoseList">
                                            <ClientSideEvents ValueChanged="function(s){ onLedDiagnoseLostFocus(s); }" />
                                            <Columns>
                                                <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                    Description="i.e. Cholera" Width="500px" />
                                            </Columns>
                                        </qis:QISSearchTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Pre Diagnosis Text")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiagnosisText" runat="server" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal">
                                            <%=GetLabel("Post Diagnosis (ICD X)")%></label>
                                    </td>
                                    <td colspan="4">
                                        <input type="hidden" value="" id="hdnEntryPostDiagnoseID" runat="server" />
                                        <input type="hidden" value="" id="hdnEntryPostDiagnoseText" runat="server" />
                                        <qis:QISSearchTextBox ID="ledPostDiagnose" ClientInstanceName="ledPostDiagnose" runat="server"
                                            Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName"
                                            Value="DiagnoseID" MethodName="GetvDiagnoseList">
                                            <ClientSideEvents ValueChanged="function(s){ onLedPostDiagnoseLostFocus(s); }" />
                                            <Columns>
                                                <qis:QISSearchTextBoxColumn Caption="Diagnose Name (Code)" FieldName="DiagnoseName"
                                                    Description="i.e. Cholera" Width="500px" />
                                            </Columns>
                                        </qis:QISSearchTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="lblNormal lblMandatory">
                                            <%=GetLabel("Post Diagnosis Text")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostDiagnosisText" runat="server" Width="98%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <label class="lblNormal">
                                            <%=GetLabel("Jenis/Tindakan Operasi :")%></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                                            <tr id="trProcedureGroupEntry" style="display: none">
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
                                                            <td style="padding-left: 5px">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Jenis Operasi")%></label>
                                                            </td>
                                                            <td colspan="4">
                                                                <input type="hidden" value="" id="hdnEntryProcedureGroupID" runat="server" />
                                                                <input type="hidden" value="" id="hdnEntryProcedureGroupText" runat="server" />
                                                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                                                    <colgroup>
                                                                        <col style="width: 95%" />
                                                                        <col style="width: 5%" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td>
                                                                            <qis:QISSearchTextBox ID="ledProcedureGroup" ClientInstanceName="ledProcedureGroup"
                                                                                runat="server" Width="99%" ValueText="ProcedureGroupID" FilterExpression="IsDeleted = 0"
                                                                                DisplayText="ProcedureGroupName" MethodName="GetProcedureGroupList">
                                                                                <ClientSideEvents ValueChanged="function(s){ onLedProcedureGroupLostFocus(s); }" />
                                                                                <Columns>
                                                                                    <qis:QISSearchTextBoxColumn Caption="Jenis Operasi (Kode)" FieldName="ProcedureGroupName"
                                                                                        Description="i.e. Appendictomy" Width="500px" />
                                                                                </Columns>
                                                                            </qis:QISSearchTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <table border="0" cellpadding="1" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="btnApplyProcedureGroup imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="btnCancelProcedureGroup imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="position: relative;">
                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                            <colgroup>
                                                                <col width="150px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <dxcp:ASPxCallbackPanel ID="cbpProcedureGroupView" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroupView"
                                                                        ShowLoadingPanel="false" OnCallback="cbpProcedureGroupView_Callback">
                                                                        <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupViewEndCallback(s); }" />
                                                                        <PanelCollection>
                                                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 160px">
                                                                                    <asp:GridView ID="grdProcedureGroupView" runat="server" CssClass="grdSelected grdPatientPage"
                                                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                                                                                <HeaderTemplate>
                                                                                                    <img class="imgAddProcedureGroup imgLink" title='<%=GetLabel("+ Jenis/Tindakan Operasi")%>'
                                                                                                        src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>' alt="" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <img class="imgEditProcedureGroup imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                                                    alt="" recordid="ID" proceduregroupcode="ProcedureGroupCode" proceduregroupname="ProcedureGroupName" />
                                                                                                            </td>
                                                                                                            <td style="width: 1px">
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <img class="imgDeleteProcedureGroup imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                                                    alt="" recordid="<%#:Eval("ID") %>" proceduregroupcode="<%#:Eval("ProcedureGroupCode") %>"
                                                                                                                    proceduregroupname="<%#:Eval("ProcedureGroupName") %>" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                                                <HeaderTemplate>
                                                                                                    <%=GetLabel("Jenis Operasi")%>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <div>
                                                                                                        <%#: Eval("ProcedureGroupName")%></div>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Kategori Operasi" DataField="SurgeryClassification" HeaderStyle-HorizontalAlign="Left" />
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
                                                                                            <br />
                                                                                            <span class="lblLink" id="lblAddProcedureFromOrder">
                                                                                                <%= GetLabel("+ Salin dari Jadwal")%></span>
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </dx:PanelContent>
                                                                        </PanelCollection>
                                                                    </dxcp:ASPxCallbackPanel>
                                                                    <div class="containerPaging" style="display: none">
                                                                        <div class="wrapperPaging">
                                                                            <div id="procedureGroupPaging">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Catatan Jenis Operasi")%></label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtProcedureGroupRemarks" runat="server" Width="100%" TextMode="Multiline"
                                                                        Rows="5" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="180px" />
                                    <col />
                                </colgroup>
                                <tr id="trParamedicTeamEntry" style="display: none">
                                    <td colspan="2">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="200px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                                </td>
                                                <td colspan="3">
                                                    <input type="hidden" value="" id="hdnEntryParamedicID" runat="server" />
                                                    <qis:QISSearchTextBox ID="ledParamedicTeam" ClientInstanceName="ledParamedicTeam"
                                                        runat="server" Width="99%" ValueText="ParamedicID" FilterExpression="" DisplayText="ParamedicName"
                                                        MethodName="GetvParamedicMasterList">
                                                        <ClientSideEvents ValueChanged="function(s){ onLedParamedicLostFocus(s); }" />
                                                        <Columns>
                                                            <qis:QISSearchTextBoxColumn Caption="Nama Dokter/Tenaga Medis" FieldName="ParamedicName"
                                                                Description="i.e. samuel" Width="500px" />
                                                        </Columns>
                                                    </qis:QISSearchTextBox>
                                                </td>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Peran Team")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboParamedicType" ClientInstanceName="cboParamedicType"
                                                        Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <img class="btnApplyParamedicTeam imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td>
                                                                <img class="btnCancelParamedicTeam imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxcp:ASPxCallbackPanel ID="cbpParamedicTeamView" runat="server" Width="100%" ClientInstanceName="cbpParamedicTeamView"
                                                ShowLoadingPanel="false" OnCallback="cbpParamedicTeamView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ oncbpParamedicTeamViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 300px">
                                                            <asp:GridView ID="grdParamedicTeamView" runat="server" CssClass="grdSelected grdPatientPage"
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
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                                        <HeaderTemplate>
                                                                            <img class="imgAddParamedicTeam imgLink" title='<%=GetLabel("+ Team")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                                alt="" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditParamedicTeam imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" paramediccode="<%#:Eval("ParamedicCode") %>"
                                                                                            paramedicname="<%#:Eval("ParamedicName") %>" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteParamedicTeam imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" recordid="<%#:Eval("ID") %>" paramediccode="<%#:Eval("ParamedicCode") %>"
                                                                                            paramedicname="<%#:Eval("ParamedicName") %>" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Tenaga Medis" HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" HeaderStyle-Width="150px"
                                                                        HeaderStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data team pelaksana untuk order ini")%>
                                                                    <br />
                                                                    <span class="lblLink" id="lblAddTeamFromOrder">
                                                                        <%= GetLabel("+ Salin dari Jadwal")%></span>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                            <div class="containerPaging" style="display: none">
                                                <div class="wrapperPaging">
                                                    <div id="paramedicPaging">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-left: 5px">
                                        <label class="lblNormal" id="Label3">
                                            <%=GetLabel("Konsultasi Intra Operatif") %></label>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtReferralSummary" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="5" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <colgroup>
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col width="100px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:CheckBox ID="chkIsUsingImplant" runat="server" Text=" Dilakukan Pemasangan Implant"
                                                        Checked="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxcp:ASPxCallbackPanel ID="cbpImplantView" runat="server" Width="100%" ClientInstanceName="cbpImplantView"
                                            ShowLoadingPanel="false" OnCallback="cbpImplantView_Callback">
                                            <ClientSideEvents EndCallback="function(s,e){ oncbpImplantViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server">
                                                    <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                        <asp:GridView ID="grdImplantView" runat="server" CssClass="grdSelected grdPatientPage"
                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                    <ItemTemplate>
                                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                                        <input type="hidden" value="<%#:Eval("ItemName") %>" bindingfield="ItemName" />
                                                                        <input type="hidden" value="<%#:Eval("SerialNumber") %>" bindingfield="SerialNumber" />
                                                                        <input type="hidden" value="<%#:Eval("cfImplantDate") %>" bindingfield="cfImplantDate" />
                                                                        <input type="hidden" value="<%#:Eval("cfReviewDate") %>" bindingfield="cfReviewDate" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="80px"
                                                                    ItemStyle-Width="20px">
                                                                    <HeaderTemplate>
                                                                        <img class="imgAddDevice imgLink" title='<%=GetLabel("+ Implant")%>' src='<%# ResolveUrl("~/Libs/Images/Button/plus.png")%>'
                                                                            alt="" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="imgEditDevice imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                        itemname="<%#:Eval("ItemName") %>" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgDeleteDevice imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" recordid="<%#:Eval("ID") %>" testorderid="<%#:Eval("TestOrderID") %>"
                                                                                        itemname="<%#:Eval("ItemName") %>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px"
                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="ItemName" HeaderText="Nama Item" HeaderStyle-Width="300px"
                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" HeaderStyle-Width="200px"
                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="cfImplantDate" HeaderText="Tanggal Pemasangan" HeaderStyle-Width="100px"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div>
                                                                    <div>
                                                                        <%=GetLabel("Belum ada informasi pemasangan implant untuk tindakan operasi di pasien ini") %></div>
                                                                    <br />
                                                                    <span class="lblLink" id="lblAddImplantDevice">
                                                                        <%= GetLabel("+ Implant")%></span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="containerPaging" style="display: none">
                                            <div class="wrapperPaging">
                                                <div id="implantPaging">
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage8" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 1px">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="float: left; margin-top: 0px;">
                                            <tr>
                                                <td>
                                                    <span class="lblLink" id="lblAddBodyDiagram">
                                                        <%= GetLabel("+ Tambah Body Diagram")%></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table id="tblBodyDiagramNavigation" runat="server" border="0" cellpadding="0" cellspacing="0"
                                            style="float: right; margin-top: 0px;">
                                            <tr>
                                                <td>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px"
                                                        alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" />
                                                </td>
                                                <td>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px"
                                                        alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: relative;" id="divBodyDiagram" runat="server">
                                            <dxcp:ASPxCallbackPanel ID="cbpBodyDiagramView" runat="server" Width="100%" ClientInstanceName="cbpBodyDiagramView"
                                                ShowLoadingPanel="false" OnCallback="cbpBodyDiagramView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent5" runat="server">
                                                        <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid">
                                                            <div class="templatePatientBodyDiagram">
                                                                <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />
                                                                <div class="containerImage boxShadow">
                                                                    <img src='' alt="" id="imgBodyDiagram" runat="server" />
                                                                </div>
                                                                <div>
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="float: right; margin-top: 0px;">
                                                                        <tr>
                                                                            <td>
                                                                                <img src='<%=ResolveUrl("~/Libs/Images/Button/edit.png") %>' title="Edit" width="25px"
                                                                                    alt="" class="imgLink" id="imgEditBodyDiagram" style="margin-left: 5px;" />
                                                                            </td>
                                                                            <td>
                                                                                <img src='<%=ResolveUrl("~/Libs/Images/Button/delete.png") %>' title="Delete" width="25px"
                                                                                    alt="" class="imgLink" id="imgDeleteBodyDiagram" style="margin-left: 5px;" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Diagram Name") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Remarks") %></span>:
                                                                <br />
                                                                <asp:Repeater ID="rptRemarks" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table>
                                                                            <colgroup width="20px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="15px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="60px" />
                                                                            <colgroup width="2px" />
                                                                            <colgroup width="*" />
                                                                            <colgroup width="16px" />
                                                                            <colgroup width="16px" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <img alt="" style="width: 16px; height: 16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>" />
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%>
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "SymbolName")%>
                                                                            </td>
                                                                            <td>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "Remarks")%>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                                <br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                                                                <span class="spLabel">
                                                                    <%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime"
                                                                        runat="server"></span><br />
                                                            </div>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                        <table id="tblEmpty" style="display: none; width: 100%" runat="server">
                                            <tr class="trEmpty">
                                                <td align="center" valign="middle">
                                                    <%=GetLabel("Tidak ada data penanda gambar untuk pasien ini") %>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="tdLabel" valign="top" style="width: 150px">
                                                    <label class="lblLink lblMandatory" id="lblSurgeryRemarks">
                                                        <%=GetLabel("Uraian Pembedahan")%></label>
                                                </td>
                                                <td>
                                                    <img class="imgLink" id="btnAddTemplate" title='<%=GetLabel("Add to My Template")%>'
                                                        src='<%= ResolveUrl("~/Libs/Images/button/physician_template.png")%>' alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="18" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteBodyDiagram" runat="server" Width="100%" ClientInstanceName="cbpDeleteBodyDiagram"
                ShowLoadingPanel="false" OnCallback="cbpDeleteBodyDiagram_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpBodyDiagramDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpDeleteDevice" runat="server" Width="100%" ClientInstanceName="cbpDeleteDevice"
                ShowLoadingPanel="false" OnCallback="cbpDeleteDevice_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpDeleteDeviceEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpParamedicTeam" runat="server" Width="100%" ClientInstanceName="cbpParamedicTeam"
                ShowLoadingPanel="false" OnCallback="cbpParamedicTeam_Callback">
                <ClientSideEvents EndCallback="function(s,e){onCbpParamedicTeamEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
            <dxcp:ASPxCallbackPanel ID="cbpProcedureGroup" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroup"
                ShowLoadingPanel="false" OnCallback="cbpProcedureGroup_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
