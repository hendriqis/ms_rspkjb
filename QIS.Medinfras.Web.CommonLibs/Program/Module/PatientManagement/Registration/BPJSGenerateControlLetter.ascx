<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BPJSGenerateControlLetter.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BPJSGenerateControlLetter" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryPopupSave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Create")%></div>
        </li>
        <li id="btnMPEntryPopupUpdate">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><div>
                <%=GetLabel("Update")%></div>
        </li>
        <li id="btnMPEntryPopupDelete">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
                <%=GetLabel("Delete")%></div>
        </li>
        <li id="btnMPEntryPopupPrintSuratKontrol">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("Surat Kontrol")%></div>
        </li>
        <li id="btnMPEntryPopupAppointmentSuratRujukan">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div id="divBtnSuratRujukan" runat="server">
                <%=GetLabel("Appointment")%>
            </div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_ChangeReferralManualCtl">
    $(function () {
        setDatePicker('<%=txtControlDate.ClientID %>');
        $('#<%:txtControlDate.ClientID %>').datepicker('option', 'minDate', '0');

        if ($('#<%:txtPhysicianCode.ClientID %>').val() != '') {
            Methods.getObject('GetvParamedicMasterList', "ParamedicCode = '" + $('#<%:txtPhysicianCode.ClientID %>').val() + "'", function (result) {
                if (result != null) {
                    $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnKodeDPJP.ClientID %>').val(result.BPJSReferenceInfo.split(';')[1].split('|')[0]);
                }
                else {
                    $('#<%:txtPhysicianCode.ClientID %>').val("");
                    $('#<%:txtPhysicianName.ClientID %>').val("");
                }
            });
        }

        reInitToolbarButton();
    });

    function reInitToolbarButton() {
        if ($('#<%:txtNoRencanaKontrol.ClientID %>').val() == "") {
            $('#btnMPEntryPopupSave').show();
            $('#btnMPEntryPopupUpdate').hide();
            $('#btnMPEntryPopupDelete').hide();
            $('#btnMPEntryPopupAppointmentSuratRujukan').hide();
        }
        else {
            $('#btnMPEntryPopupSave').hide();
            $('#btnMPEntryPopupUpdate').show();
            $('#btnMPEntryPopupDelete').show();
            if ($('#<%:hdnIsCreateAppointmentAfterCreateNoSurkon.ClientID %>').val() == "0") {
                $('#btnMPEntryPopupAppointmentSuratRujukan').hide();
            }
            else {
                $('#btnMPEntryPopupAppointmentSuratRujukan').show();
            }
        }
    }

    //#region Dokter DPJP
    function getPhysicianBPJSFilterExpression() {
        var filterExpression = "BPJSReferenceInfo IS NOT NULL AND IsDeleted = 0";
        return filterExpression;
    }
    $('#btnMPEntryPopupPrintSuratKontrol').click(function () {
        var isBpjs = $('#<%=hdnIsBpjsRegistrationCtl.ClientID %>').val();
        if (isBpjs == "1") {
            cbpPopupProcess.PerformCallback('printKontrol');
        }
        else {
            showToast('Warning', 'Maaf Pasien Ini Belum Melakukan Generate SEP');
        }
    });

    $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('physician', getPhysicianBPJSFilterExpression(), function (value) {
            $('#<%:txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onTxtPhysicianCodeChanged(value) {
        filterExpression = getPhysicianBPJSFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                $('#<%:hdnKodeDPJP.ClientID %>').val(result.BPJSReferenceInfo.split(';')[1].split('|')[0]);
            }
            else {
                $('#<%:txtPhysicianCode.ClientID %>').val("");
                $('#<%:txtPhysicianName.ClientID %>').val("");
            }
        });
    }
    //#endregion

    //#region Poli BPJS
    function getPoliBPJSFilterExpression() {
        var filterExpression = "GCBPJSObjectType = 'X358^004'";
        return filterExpression;
    }

    $('#<%:lblPoli.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('bpjsreference', getPoliBPJSFilterExpression(), function (value) {
            $('#<%:txtPoliCode.ClientID %>').val(value);
            onTxtPoliCodeChanged(value);
        });
    });

    $('#<%:txtPoliCode.ClientID %>').live('change', function () {
        onTxtPoliCodeChanged($(this).val());
    });

    function onTxtPoliCodeChanged(value) {
        filterExpression = getPoliBPJSFilterExpression() + " AND BPJSCode = '" + value + "'";
        Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtPoliCode.ClientID %>').val(result.BPJSCode);
                $('#<%:txtPoliName.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%:txtPoliCode.ClientID %>').val("");
                $('#<%:txtPoliName.ClientID %>').val("");
            }
        });
    }
    //#endregion

    $('#btnMPEntryPopupSave').click(function () {
        cbpPopupProcess.PerformCallback('save');
    });
    $('#btnMPEntryPopupUpdate').click(function () {
        cbpPopupProcess.PerformCallback('update');
    });
    $('#btnMPEntryPopupDelete').click(function () {
        cbpPopupProcess.PerformCallback('delete');
    });

    $('#btnMPEntryPopupAppointmentSuratRujukan').click(function () {
        var surKon = $('#<%=txtNoRencanaKontrol.ClientID %>').val();
        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        if (surKon != "") {
            if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSBL") {
                if (appointmentID == "") {
                    cbpPopupProcess.PerformCallback('appointment');
                }
                else {
                    showToast('Warning', 'Maaf Pasien Ini Sudah Ada Perjanjian Pasien');
                }
            }
            else {
                showToast('Warning', 'Maaf Pasien Ini Fitur Sedang Dalam Perbaikan');
            }
        }
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                displayMessageBox('Create No. Rencana Kontrol : GAGAL', 'Error Message : ' + param[2]);
            }
            else {
                $('#<%:txtNoRencanaKontrol.ClientID %>').val(param[2]);
                displayMessageBox("Create No. Rencana Kontrol : SUKSES", "No. Surat Kontrol : " + param[2]);
            }
        }
        else if (param[0] == 'update') {
            if (param[1] == 'fail') {
                displayMessageBox('Update No. Rencana Kontrol : GAGAL', 'Error Message : ' + param[2]);
            }
            else {
                $('#<%:txtNoRencanaKontrol.ClientID %>').val(param[2]);
                displayMessageBox("Update No. Rencana Kontrol : SUKSES", "No. Surat Kontrol : " + param[2]);
            }
        }
        else if (param[0] == 'printKontrol') {
            if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSBL") {
                openReportViewer('PM-00200', $('#<%=hdnRegistrationID.ClientID %>').val());
            }
            else {
                openReportViewer('PM-00160', $('#<%=hdnVisitID.ClientID %>').val());
            }
        }
        if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                displayMessageBox('Delete No. Rencana Kontrol : GAGAL', 'Error Message : ' + param[2]);
            }
            else {
                displayMessageBox("Delete No. Rencana Kontrol : SUKSES", "");
                $('#<%:txtNoRencanaKontrol.ClientID %>').val("");
            }
        }

        reInitToolbarButton();
    }

</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnKodeDPJP" value="" />
    <input type="hidden" runat="server" id="hdnIsBpjsRegistrationCtl" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareInitial" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnIsCreateAppointmentAfterCreateNoSurkon" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnKodePoli" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnVisitTypeID" value="" />
    <input type="hidden" runat="server" id="hdnGCCostumerType" value="" />
    <input type="hidden" runat="server" id="hdnGCTariffSchemePersonal" value="" />
    <input type="hidden" runat="server" id="hdnGCTariffScheme" value="" />
    <input type="hidden" runat="server" id="hdnPayerID" value="" />
    <input type="hidden" runat="server" id="hdnContractID" value="" />
    <input type="hidden" runat="server" id="hdnCoverageTypeID" value="" />
    <input type="hidden" runat="server" id="hdnParticipantNo" value="" />
    <input type="hidden" runat="server" id="hdnIsCoverageLimitPerDay" value="" />
    <input type="hidden" runat="server" id="hdnIsControlClassCare" value="" />
    <input type="hidden" runat="server" id="hdnControlClassCare" value="" />
    <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
    <input type="hidden" runat="server" id="hdnIsUsedReferenceQueueNo" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No SEP")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtSEP" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Peserta")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoPeserta" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" runat="server" id="lblPhysician">
                    <%:GetLabel("Dokter DPJP")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 80px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" runat="server" id="lblPoli">
                    <%:GetLabel("Poli Tujuan")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 80px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPoliCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPoliName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Tanggal Rencana Kontrol")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtControlDate" Width="25%" runat="server" CssClass="datepicker"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Rencana Kontrol")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoRencanaKontrol" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
