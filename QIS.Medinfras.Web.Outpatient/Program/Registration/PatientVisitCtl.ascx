<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientVisitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Outpatient.Program.PatientVisitCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitctl">
    $('#lblPatientVisitAddData').die('click');
    $('#lblPatientVisitAddData').live('click', function () {
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
        $('#<%=txtClinicCode.ClientID %>').val('');
        $('#<%=txtClinicName.ClientID %>').val('');
        $('#<%=hdnPhysicianID.ClientID %>').val('');
        $('#<%=txtPhysicianCode.ClientID %>').val('');
        $('#<%=txtPhysicianName.ClientID %>').val('');
        $('#<%=hdnVisitTypeID.ClientID %>').val('');
        $('#<%=txtVisitTypeCode.ClientID %>').val('');
        $('#<%=txtVisitTypeName.ClientID %>').val('');
        $('#<%=txtVisitDuration.ClientID %>').val('');
        $('#<%=hdnVisitDuration.ClientID %>').val('');
        $('#<%=txtVisitReason.ClientID %>').val('');
        cboRegistrationEditSpecialty.SetValue('');

        $('#containerPatientVisitEntryData').show();
    });

    $('#btnPatientVisitCancel').die('click');
    $('#btnPatientVisitCancel').live('click', function () {
        $('#containerPatientVisitEntryData').hide();
    });

    $('#btnPatientVisitSave').click(function (evt) {
        if (IsValid(evt, 'fsPatientVisit', 'mpPatientVisit'))
            cbpPatientVisitTransHd.PerformCallback('save');
        return false;
    });

    $('#btnPlusVisitDuration').live('click', function () {
        var hdnVisitDuration = parseFloat($('#<%=hdnVisitDuration.ClientID %>').val());
        var visitDuration = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());
        if ($('#<%=txtVisitDuration.ClientID %>').val() != '') {
            var value = visitDuration + hdnVisitDuration
            $('#<%=txtVisitDuration.ClientID %>').val(value);
        }
        else {
            displayMessageBox('Warning', 'Harap pilih jenis kunjungan dahulu!');
        }
    });

    $('#btnMinVisitDuration').live('click', function () {
        var hdnVisitDuration = parseFloat($('#<%=hdnVisitDuration.ClientID %>').val());
        var visitDuration = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());

        if (visitDuration != 0 && visitDuration > hdnVisitDuration) {
            var value = parseFloat($('#<%=txtVisitDuration.ClientID %>').val());
            value -= hdnVisitDuration;
            $('#<%=txtVisitDuration.ClientID %>').val(value);
        }
        else if ($('#<%=hdnVisitDuration.ClientID %>').val() != '' && $('#<%=txtVisitDuration.ClientID %>').val() != '') {
            if ($('#<%=hdnVisitDuration.ClientID %>').val() != $('#<%=txtVisitDuration.ClientID %>').val()) {
                displayMessageBox('Warning', 'Harap pilih jenis kunjungan dahulu!');
            }
        }
        else {
            displayMessageBox('Warning', 'Harap pilih jenis kunjungan dahulu!');
        }
    });

    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var polyclinicID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl1.ClientID %>').is(":checked");
        var filterExpression = 'IsDeleted = 0 AND (IsHasPhysicianRole = 1)';

        var date = new Date();
        var hourInString = date.getHours().toString();
        var minutesInString = date.getMinutes().toString();

        if (hourInString.length == 1) {
            hourInString = '0' + hourInString;
        }

        if (minutesInString.length == 1) {
            minutesInString = '0' + minutesInString;
        }

        var formattedTime = hourInString + ":" + minutesInString;
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var registrationDateFormatString = Methods.dateToString(date);
        var daynumber = "<%:GetDayNumber() %>";

        if (isCheckedFilter) {
            if (polyclinicID != '') {
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + polyclinicID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND DayNumber = '" + daynumber + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)) UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + polyclinicID + "' AND ScheduleDate = '" + registrationDateFormatString + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5))) AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
            }
        }
        else {
            if (polyclinicID != '') {
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ") AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)";
            }
        }
        return filterExpression;
    }

    $('#lblPatientVisitPhysician.lblLink').live('click', function () {
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl1.ClientID %>').is(":checked");
        var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        if (isCheckedFilter) {
            if (hsuID != '' && hsuID != '0') {
                openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPatientVisitPhysicianCodeChanged(value);
                });
            }
            else {
                showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");
            }
        }
        else {
            openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        }
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl1.ClientID %>').is(":checked");
        var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        if (isCheckedFilter) {
            if (hsuID != '' && hsuID != '0') {
                onTxtPatientVisitPhysicianCodeChanged($(this).val());
            }
            else {
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                showToast("Warning", "Silahkan Pilih Klinik Terlebih Dahulu");
            }
        }
        else {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        }
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                var filterExpressionLeave = "IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                    if (resultLeave == null) {
                        cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        cboRegistrationEditSpecialty.SetValue('');
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');

                        var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                        showToast("INFORMASI", info);
                    }
                });
            }
            else {
                cboRegistrationEditSpecialty.SetValue('');
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Clinic
    function onGetPatientVisitClinicFilterExpression() {
        var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + Constant.Facility.OUTPATIENT + "'";
        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
        if (paramedicID != '')
            filterExpression += ' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + ')';
        return filterExpression;
    }

    $('#lblPatientVisitClinic.lblLink').live('click', function () {
        openSearchDialog('serviceunitparamedicvisittypeperhealthcare', onGetPatientVisitClinicFilterExpression(), function (value) {
            $('#<%=txtClinicCode.ClientID %>').val(value);
            onTxtPatientVisitClinicCodeChanged(value);
        });
    });

    $('#<%=txtClinicCode.ClientID %>').live('change', function () {
        onTxtPatientVisitClinicCodeChanged($(this).val());
    });

    function onTxtPatientVisitClinicCodeChanged(value) {
        var filterExpression = onGetPatientVisitClinicFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%=txtClinicName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=txtClinicCode.ClientID %>').val('');
                $('#<%=txtClinicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Visit Type
    function onGetPatientVisitVisitTypeFilterExpression() {
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '')
            serviceUnitID = '0';
        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';
        var filterExpression = serviceUnitID + ';' + paramedicID + ';';
        return filterExpression;
    }

    $('#<%=lblVisitType.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('paramedicvisittype', onGetPatientVisitVisitTypeFilterExpression(), function (value) {
            $('#<%=txtVisitTypeCode.ClientID %>').val(value);
            onTxtPatientVisitVisitTypeCodeChanged(value);
        });
    });

    $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
        onTxtPatientVisitVisitTypeCodeChanged($(this).val());
    });

    function onTxtPatientVisitVisitTypeCodeChanged(value) {
        var filterExpression = onGetPatientVisitVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                $('#<%=hdnVisitDuration.ClientID %>').val(result.VisitDuration);
            }
            else {
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitTypeCode.ClientID %>').val('');
                $('#<%=txtVisitTypeName.ClientID %>').val('');
                $('#<%=txtVisitDuration.ClientID %>').val('');
                $('#<%=hdnVisitDuration.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('.imgPatientVisitDelete.imgLink').die('click');
    $('.imgPatientVisitDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).parent().parent();
            var id = $row.find('.hdnVisitID').val();
            var serviceUnitID = $row.find('.hdnServiceUnitID').val();
            $('#<%=hdnVisitID.ClientID %>').val(id);
            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(serviceUnitID);
            cbpPatientVisitTransHd.PerformCallback('delete');
        }
    });

    $('.imgPrint.imgLink').die('click');
    $('.imgPrint.imgLink').live('click', function () {
        $row = $(this).parent().parent();
        var id = $row.find('.hdnVisitID').val();
        $('#<%=hdnVisitID.ClientID %>').val(id);
        cbpPatientVisitTransHd.PerformCallback('print');
    });

    $('.imgPrintSEP.imgLink').die('click');
    $('.imgPrintSEP.imgLink').live('click', function () {
        $row = $(this).parent().parent();
        var id = $row.find('.hdnVisitID').val();
        $('#<%=hdnVisitID.ClientID %>').val(id);
        cbpPatientVisitTransHd.PerformCallback('printSEP');
    });

    //    $('.imgPatientVisitEdit.imgLink').die('click');
    //    $('.imgPatientVisitEdit.imgLink').live('click', function () {
    //        $row = $(this).parent().parent();

    //        var visitID = $row.find('.hdnVisitID').val();
    //        var serviceUnitID = $row.find('.hdnServiceUnitID').val();
    //        var paramedicID = $row.find('.hdnParamedicID').val();
    //        var serviceUnitCode = $row.find('.hdnServiceUnitCode').val();
    //        var paramedicCode = $row.find('.hdnParamedicCode').val();
    //        var serviceUnitName = $row.find('.divServiceUnitName').html();
    //        var paramedicName = $row.find('.divParamedicName').html();

    //        var visitTypeID = $row.find('.hdnVisitTypeID').val();
    //        var visitTypeCode = $row.find('.hdnVisitTypeCode').val();
    //        var visitTypeName = $row.find('.divVisitTypeName').html();
    //        var specialtyID = $row.find('.hdnSpecialtyID').val();
    //        var visitReason = $row.find('.hdnVisitReason').val();

    //        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(serviceUnitID);
    //        $('#<%=txtClinicCode.ClientID %>').val(serviceUnitCode);
    //        $('#<%=txtClinicName.ClientID %>').val(serviceUnitName);
    //        $('#<%=hdnPhysicianID.ClientID %>').val(paramedicID);
    //        $('#<%=txtPhysicianCode.ClientID %>').val(paramedicCode);
    //        $('#<%=txtPhysicianName.ClientID %>').val(paramedicName);
    //        $('#<%=hdnVisitTypeID.ClientID %>').val(visitTypeID);
    //        $('#<%=txtVisitTypeCode.ClientID %>').val(visitTypeCode);
    //        $('#<%=txtVisitTypeName.ClientID %>').val(visitTypeName);
    //        $('#<%=txtVisitReason.ClientID %>').val(visitReason);
    //        $('#<%=hdnVisitID.ClientID %>').val(visitID);
    //        cboRegistrationEditSpecialty.SetValue(specialtyID);
    //        
    //        $('#containerPatientVisitEntryData').show();
    //    });

    function onCbpPatientVisitTransHdEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPatientVisitEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#<%=hdnVisitID.ClientID %>').val('');
            }
        } else if (param[0] == 'printSEP') {
            if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "DEMO" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "rsdo-soba") {
                openReportViewer('PM-90043', $('#<%=hdnVisitID.ClientID %>').val());
            } else {
                showToast('Tidak bisa print SEP ');
            }
        }
        hideLoadingPanel();
    }

    $('#<%:chkParamedicHasSchedulePopUpCtl1.ClientID %>').live('change', function () {
        cboRegistrationEditSpecialty.SetValue('');
        $('#<%:hdnPhysicianID.ClientID %>').val('');
        $('#<%:txtPhysicianCode.ClientID %>').val('');
        $('#<%:txtPhysicianName.ClientID %>').val('');
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnMRNVisitCtl" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationIDVisitCtl" />
    <input type="hidden" value="" runat="server" id="hdnClassIDVisitCtl" />
    <input type="hidden" value="" runat="server" id="hdnBusinessPartnerIDVisitCtl" />
    <input type="hidden" value="" runat="server" id="hdnItemCardFee" />
    <input type="hidden" value="" runat="server" id="hdnScheduleValidationBeforeRegistration" />
    <input type="hidden" value="" runat="server" id="hdnIsAdmin1x1hari" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnMenggunakanValidasiChiefComplaint" runat="server" value="" />
    <input type="hidden" id="hdnIsParamedicInRegistrationUseScheduleCtl1" runat="server" />
    <input type="hidden" id="hdnHealthcareInitial" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Registrasi")%>
                                /
                                <%=GetLabel("No RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPatientVisitEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry Kunjungan")%></div>
                    <input type="hidden" id="hdnVisitID" runat="server" value="" />
                    <fieldset id="fsPatientVisit" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblPatientVisitClinic">
                                        <%=GetLabel("Klinik")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtClinicCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClinicName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trParamedicHasScheduleCtl1" runat="server">
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkParamedicHasSchedulePopUpCtl1" runat="server" /><%:GetLabel("Tampilkan Hanya Dokter Yang Punya Jadwal")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblPatientVisitPhysician">
                                        <%=GetLabel("Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Spesialisasi")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboRegistrationEditSpecialty" ClientInstanceName="cboRegistrationEditSpecialty"
                                        Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                        <%=GetLabel("Jenis Kunjungan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                    <input type="hidden" id="hdnVisitDuration" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" CssClass="required" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Visit Duration")%></label>
                                </td>
                                <td colspan="3">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtVisitDuration" Width="100%" runat="server" CssClass="number"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <input type="button" id="btnMinVisitDuration" style="width: 32px;" value='<%= GetLabel("-")%>' />
                                                &nbsp;
                                                <input type="button" id="btnPlusVisitDuration" style="width: 32px;" value='<%= GetLabel("+")%>' />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Alasan Kunjungan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVisitReason" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnPatientVisitSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnPatientVisitCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPatientVisitTransHd" runat="server" Width="100%" ClientInstanceName="cbpPatientVisitTransHd"
                    ShowLoadingPanel="false" OnCallback="cbpPatientVisitTransHd_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientVisitTransHdEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdPatientVisitTransHd" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <%--                                                <img class='imgPatientVisitEdit <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Edit")%>' 
                                                    src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;margin-right: 2px;" />--%>
                                                <img class='imgPatientVisitDelete <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Delete")%>' src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <img class="imgPrint <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                    title='<%=GetLabel("Print")%>' src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("No Registrasi")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="divVisitNo">
                                                    <%#: Eval("RegistrationNo") %></div>
                                                <input type="hidden" class="hdnVisitID" value="<%#: Eval("VisitID")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Informasi Kunjungan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" class="hdnServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                <input type="hidden" class="hdnParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="hdnServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                <input type="hidden" class="hdnParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" class="hdnSpecialtyID" value="<%#: Eval("SpecialtyID")%>" />
                                                <input type="hidden" class="hdnVisitTypeID" value="<%#: Eval("VisitTypeID")%>" />
                                                <input type="hidden" class="hdnVisitTypeCode" value="<%#: Eval("VisitTypeCode")%>" />
                                                <input type="hidden" class="hdnVisitReason" value="<%#: Eval("VisitReason")%>" />
                                                <div style="float: left; width: 100px;" class="divServiceUnitName">
                                                    <%#: Eval("ServiceUnitName")%></div>
                                                <div class="divVisitTypeName">
                                                    <%#: Eval("VisitTypeName")%></div>
                                                <div class="divParamedicName">
                                                    <%#: Eval("ParamedicName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <HeaderTemplate>
                                                <div style="text-align: right; padding-left: 3px">
                                                    <%=GetLabel("No Antrian")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: right;">
                                                    <%#: Eval("cfQueueNo") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Print SEP")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <img class="imgPrintSEP <%#: "imgLink"%>" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPatientVisitAddData">
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
