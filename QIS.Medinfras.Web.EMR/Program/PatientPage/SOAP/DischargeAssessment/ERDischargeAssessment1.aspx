<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true"
    CodeBehind="ERDischargeAssessment1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ERDischargeAssessment1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            setDatePicker('<%=txtDischargeDate.ClientID %>');
            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            setDatePicker('<%=txtAppointmentDate.ClientID %>');

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge')) {
                    var message = "Lanjutkan proses discharge pasien ?";
                    showToastConfirmation(message, function (result) {
                        if (result) onCustomButtonClick('process'); ;
                    });
                }
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            onDischargeDateTimeChange();
            onCboPatientOutcomeChanged();
            onCboDischargeRoutineChanged();

            registerCollapseExpandHandler();
        });

        function dateDiff(date1, date2) {
            var diff = date2 - date1;
            return isNaN(diff) ? NaN : {
                diff: diff,
                ms: Math.floor(diff % 1000),
                s: Math.floor(diff / 1000 % 60),
                m: Math.floor(diff / 60000 % 60),
                h: Math.floor(diff / 3600000 % 24),
                d: Math.floor(diff / 86400000)
            };
        }

        function onDischargeDateTimeChange() {
            var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDate.ClientID %>').val());
            var dischargeTime = $('#<%=txtDischargeTime.ClientID %>').val();
            var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
            var diff = dateDiff(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
            $('#<%=hdnLOSInDay.ClientID %>').val(diff.d);
            $('#<%=hdnLOSInHour.ClientID %>').val(diff.h);
            $('#<%=hdnLOSInMinute.ClientID %>').val(diff.m);
            $('#<%=txtLengthOfVisit.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');
        }

        function onCboPatientOutcomeChanged() {
            if (cboPatientOutcome.GetValue() != null && (cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.GetValue() == Constant.PatientOutcome.DEAD_AFTER_48)) {
                $('#tblDischarge tr.trDeathInfo').show();
            }
            else {
                $('#tblDischarge tr.trDeathInfo').hide();
            }
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#tblDischarge tr.trDischargeOtherReason').show();
            }
            else {
                $('#tblDischarge tr.trDischargeOtherReason').hide();
            }
        }

        function onCboDischargeRoutineChanged() {
            if (cboDischargeRoutine.GetValue() != null) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $("#tblDischarge tr.trAppointment").show();
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').show();
                    $("#tblDischarge tr.trAppointment").hide();
                    cboDischargeReason.SetValue('');
                    $('#tblDischarge tr.trDischargeReason').hide();
                    $('#tblDischarge tr.trReferToOtherProvider1').hide();
                    $('#tblDischarge tr.trReferToOtherProvider2').hide();
                }
                else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                    $("#tblDischarge tr.trAppointment").hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    cboDischargeReason.SetValue('');
                    $('#tblDischarge tr.trDischargeReason').show();
                    $('#tblDischarge tr.trReferToOtherProvider1').show();
                    $('#tblDischarge tr.trReferToOtherProvider2').show();
                }
                else {
                    $("#tblDischarge tr.trAppointment").hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trDischargeReason').hide();
                    $('#tblDischarge tr.trDischargeOtherReason').hide();
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $('#tblDischarge tr.trReferToOtherProvider1').hide();
                    $('#tblDischarge tr.trReferToOtherProvider2').hide();
                }
            }
            else {
                $("#tblDischarge tr.trAppointment").hide();
                $('#tblDischarge tr.trInpatientPhysician').hide();
                $('#tblDischarge tr.trDischargeReason').hide();
                $('#tblDischarge tr.trDischargeOtherReason').hide();
                $('#tblDischarge tr.trInpatientPhysician').hide();
                $('#tblDischarge tr.trReferToOtherProvider1').hide();
                $('#tblDischarge tr.trReferToOtherProvider2').hide();
            }
        }

        //#region Physician
        function getPhysicianFilterExpression() {
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';
            if (polyclinicID != '' && polyclinicID != null)
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblParamedic2.lblLink').live('click', function () {
            openSearchDialog('paramedic', " GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'", function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        });

        function onTxtParamedicCodeChanged(value) {
            var filterExpression = " ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID2.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID2.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                }
            });
        }

        function onTxtPatientVisitPhysicianCodeChanged(value) {
            var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Discharge to Other Hospital
        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
            }
        }

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
            var referral = cboReferral.GetValue();
            openSearchDialog('referrer1', getReferralDescriptionFilterExpression(), function (value) {
                $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                onTxtReferralDescriptionCodeChanged(value);
            });
        });

        $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
            onTxtReferralDescriptionCodeChanged($(this).val());
        });

        function onTxtReferralDescriptionCodeChanged(value) {
            var filterExpression = "";
            var referral = cboReferral.GetValue();
            filterExpression = getReferralDescriptionFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                }
            });
        }
        //#endregion
        //#endregion

        //#region Vital Sign Paging
        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "1|", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", "1|" + $('#<%=hdnVitalSignRecordID.ClientID %>').val(), "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            showToastConfirmation(message, function (result) {
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
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }
        //#endregion

        function onAfterCustomClickSuccess(type) {
            var reportCode = "MR000002";
            var filterExpression = { text: "" };

            filterExpression.text = $('#<%=hdnVisitID.ClientID %>').val();
            openReportViewer(reportCode, filterExpression.text);
            //            PrintEpisodeSummary(reportCode, filterExpression, function () {
            //                setTimeout(exitPatientPage(),20000);
            //            });
        }

        function PrintEpisodeSummary(reportCode, param, callback) {
            openReportViewer(reportCode, param);
            callback();
        } 
    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnRegistrationDateTime" runat="server" />
    <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnPrescriptionOrderID" runat="server" value="" />
    <fieldset id="fsPatientDischarge">
        <table class="tblEntryContent" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <colgroup>
                <col style="width: 45%" />
                <col style="width: 55%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top;">
                    <table id="tblDischarge" style="width: 100%">
                        <colgroup>
                            <col style="width: 153px" />
                            <col style="width: 150px" />
                            <col style="width: 80px" />
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDischargeTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keadaan Keluar")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Keluar")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDischargeRoutine" ClientInstanceName="cboDischargeRoutine"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDischargeRoutineChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trDeathInfo" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Death Date - Time")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateOfDeath" CssClass="datepicker" Width="120px" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTimeOfDeath" CssClass="time" Width="80px" />
                            </td>
                        </tr>
                        <tr class="trInpatientPhysician" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" id="lblParamedic2">
                                    <%=GetLabel("DPJP")%></label>
                            </td>
                            <td colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 100px">
                                            <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                            <asp:TextBox runat="server" ID="txtParamedicCode" Width="100%" />
                                        </td>
                                        <td style="width: 255px">
                                            <asp:TextBox runat="server" ID="txtParamedicName" ReadOnly="true" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="trAppointment" style="display: none">
                            <td colspan="4">
                                <div id="divAppointment">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Clinic")%></label>
                                            </td>
                                            <td colspan="2">
                                                <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblPhysician">
                                                    <%=GetLabel("Physician")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Appointment Date")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Remarks")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="220px" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr class="trReferToOtherProvider1" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Dirujuk Ke")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trReferToOtherProvider2" style="display: none">
                            <td class="tdLabel">
                                <label class="lblLink" runat="server" id="lblReferralDescription">
                                    <%:GetLabel("Rumah Sakit / Faskes")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="trDischargeReason" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alasan Ke Rumah Sakit/Faskes lain")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDischargeReason" Width="100%" runat="server" ClientInstanceName="cboDischargeReason">
                                    <ClientSideEvents Init="function(s,e){ onCboDischargeReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeReasonValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr class="trDischargeOtherReason" style="display: none">
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDischargeOtherReason" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                    <h4 class="h4collapsed">
                        <%=GetLabel("Pemeriksaan Tanda Vital")%></h4>
                    <div class="containerTblEntryContent">
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
                                                                                    <div style="padding-left: 20px; float: left; width: 300px;">
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
                        </table>
                    </div>
                    <h4 class="h4collapsed" style="display:none">
                        <%=GetLabel("Discharge Prescription")%></h4>
                    <div class="containerTblEntryContent" style="display:none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                            <colgroup>
                                <col width="350px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <table cellpadding="1">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Prescription No") %>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtPrescriptionNo" Width="99%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Dispensary Unit") %></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox ID="cboDispensaryUnit" ClientInstanceName="cboDispensaryUnit" runat="server"
                                                    Width="100%">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Location")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboLocation" ClientInstanceName="cboLocation"
                                                    Width="100%">
                                                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align: right" valign="top">
                                    <table cellpadding="0" style="text-align: left;" width="100%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Order Remarks") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox1" Width="100%" runat="server" TextMode="MultiLine" Height="45px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Refill Instruction") %>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction"
                                                    Width="130px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="0">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 180px" />
                                            <col style="width: 50px" />
                                            <col style="width: 50px" />
                                            <col style="width: 60px" />
                                            <col style="width: 50px" />
                                            <col style="width: 80px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                            </td>
                                            <td class="tdLabel">
                                                <%=GetLabel("Item Name")%>
                                            </td>
                                            <td class="tdLabel">
                                                <%=GetLabel("Kadar")%>
                                            </td>
                                            <td class="tdLabel" colspan="2">
                                                <%=GetLabel("Frekuensi")%>
                                            </td>
                                            <td class="tdLabel">
                                                <%=GetLabel("Dosis")%>
                                            </td>
                                            <td class="tdLabel">
                                                <%=GetLabel("Jumlah")%>
                                            </td>
                                            <td class="tdLabel">
                                                <%=GetLabel("Catatan")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <%=GetLabel("Quick Entry")%>
                                            </td>
                                            <td>
                                                <qis:QISQuickEntry runat="server" ClientInstanceName="txtQuickEntry" ID="txtQuickEntry"
                                                    Width="100%">
                                                    <QuickEntryHints>
                                                        <qis:QISQuickEntryHint Text="Drug Name" ValueField="ItemID" TextField="ItemName1"
                                                            Description="Item Name" FilterExpression="IsDeleted = 0" MethodName="GetvDrugInfoList">
                                                            <Columns>
                                                                <qis:QISQuickEntryHintColumn Caption="Item Code" FieldName="ItemCode" Width="80px" />
                                                                <qis:QISQuickEntryHintColumn Caption="Item Name" FieldName="ItemName1" Width="600px" />
                                                                <qis:QISQuickEntryHintColumn Caption="Employee Formularium" FieldName="IsEmployeeFormularium"
                                                                    Width="70px" />
                                                                <qis:QISQuickEntryHintColumn Caption="Qty On Hand (All)" FieldName="QtyOnHandAll"
                                                                    Width="80px" />
                                                            </Columns>
                                                        </qis:QISQuickEntryHint>
                                                        <qis:QISQuickEntryHint Text="Frequency" ValueField="StandardCodeID" TextField="StandardCodeName"
                                                            Description="i.e. QD / BID / TID / QID / QH# / #dd / prn" MethodName="GetStandardCodeList"
                                                            FilterExpression="ParentID = 'X233'">
                                                            <Columns>
                                                                <qis:QISQuickEntryHintColumn Caption="Frequency" FieldName="StandardCodeName" Width="300px" />
                                                            </Columns>
                                                        </qis:QISQuickEntryHint>
                                                        <qis:QISQuickEntryHint Text="Dosing" Description="Dosing Quantity" />
                                                        <qis:QISQuickEntryHint Text="Dispense Quantity" />
                                                        <qis:QISQuickEntryHint Text="Special Instruction" Description="Special Instruction" />
                                                    </QuickEntryHints>
                                                </qis:QISQuickEntry>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsUsingDoseUnit" runat="server" Width="100%" Checked="false" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFrequency" Width="100%" runat="server" CssClass="qty" />
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboFrequency" ClientInstanceName="cboFrequency"
                                                    Width="100%">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumberOfDosage" Width="100%" runat="server" CssClass="qty" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDispenseQty" Width="100%" runat="server" CssClass="qty" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecialInstruction" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="position: relative;">
                                        <dxcp:ASPxCallbackPanel ID="cbpPrescriptionView" runat="server" Width="100%" ClientInstanceName="cbpPrescriptionView"
                                            ShowLoadingPanel="false" OnCallback="cbpPrescriptionView_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                EndCallback="function(s,e){ onCbpPrescriptionViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server">
                                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 300px">
                                                        <asp:GridView ID="grdPrescriptionView" runat="server" CssClass="grdSelected grdPatientPage"
                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                            <Columns>
                                                                <asp:BoundField DataField="PrescriptionOrderDetailID" HeaderStyle-CssClass="keyField"
                                                                    ItemStyle-CssClass="keyField" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="imgPrescriptionEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgPrescriptionDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                    <ItemTemplate>
                                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                                        <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                                        <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                                                        <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                                                        <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                                                        <input type="hidden" value="<%#:Eval("GCDosingUnit") %>" bindingfield="GCDosingUnit" />
                                                                        <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                                                        <input type="hidden" value="<%#:Eval("GCRoute") %>" bindingfield="GCRoute" />
                                                                        <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDate" />
                                                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                                                        <input type="hidden" value="<%#:Eval("DispenseQtyInString") %>" bindingfield="DispenseQtyInString" />
                                                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                                        <input type="hidden" value="<%#:Eval("IsAllergyAlert") %>" bindingfield="IsAllergyAlert" />
                                                                        <input type="hidden" value="<%#:Eval("IsDuplicateTheraphyAlert") %>" bindingfield="IsDuplicateTheraphyAlert" />
                                                                        <input type="hidden" value="<%#:Eval("IsAdverseReactionAlert") %>" bindingfield="IsAdverseReactionAlert" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemName">
                                                                    <HeaderTemplate>
                                                                        <div>
                                                                            <%=GetLabel("Drug Name")%>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <%#: Eval("cfMedicationName")%></div>
                                                                        <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                            <%#: Eval("cfCompoundDetail")%></div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Frequency" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:BoundField DataField="DosingFrequency" HeaderText="Timeline" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="cfNumberOfDosage" HeaderText="Dose" HeaderStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:BoundField DataField="DosingUnit" HeaderText="Unit" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <div>
                                                                            <%=GetLabel("Signa")%></div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <%#: Eval("cfConsumeMethod")%></div>
                                                                        <div>
                                                                            <%#: Eval("MedicationAdministration")%></div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="StartDateInDatePickerFormat" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="DispenseQtyInString" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAllergyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                            title='<%=GetLabel("Allergy Alert") %>' alt="" />
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsAdverseReactionAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                            title='<%=GetLabel("Adverse Reaction") %>' alt="" />
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Status/warning.png") %>' <%# Eval("IsDuplicateTheraphyAlert").ToString() == "True" ? "" : "Style ='display:none'" %>
                                                                            title='<%=GetLabel("Duplicate Theraphy") %>' alt="" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <%=GetLabel("No Data To Display")%>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
                                            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                        <div class="containerPaging">
                                            <div class="wrapperPaging">
                                                <div id="prescriptionPaging">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <h4 class="h4collapsed" style="display: none">
                        <%=GetLabel("Resume Medis")%></h4>
                    <div class="containerTblEntryContent" style="display: none">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Anamnesa")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAnamnesis" runat="server" Width="100%" TextMode="Multiline" Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pemeriksaan Fisik")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhysicalExamination" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pemeriksaan Penunjang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnosticSummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diagnosa")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnosisSummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pengobatan/Tindakan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicationSummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Edukasi Pasien")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicalSummary" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="6" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
