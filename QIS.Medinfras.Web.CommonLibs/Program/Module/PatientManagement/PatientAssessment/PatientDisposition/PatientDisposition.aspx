<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PatientDisposition.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDisposition" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientAssessment/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:content id="Content3" contentplaceholderid="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:content>
<asp:content id="Content2" contentplaceholderid="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:content>
<asp:content id="Content1" contentplaceholderid="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            if (($('#<%=chkIsDead.ClientID %>').attr('checked'))) {
                cboPatientOutcome.SetVisible(false);
                cboPatientOutcomeDead.SetVisible(true);
                $('#tblEntry tr.trDeathInfo').show();
            } else {
                cboPatientOutcome.SetVisible(true);
                cboPatientOutcomeDead.SetVisible(false);
                $('#tblEntry tr.trDeathInfo').hide();
            }

            setDatePicker('<%=txtDischargeDate.ClientID %>');
            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            setDatePicker('<%=txtAppointmentDate.ClientID %>');

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    onCustomButtonClick('save');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            onDischargeDateTimeChange();

            $('#<%=chkIsDead.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    cboPatientOutcome.SetVisible(false);
                    cboPatientOutcomeDead.SetVisible(true);
                    $('#tblEntry tr.trDeathInfo').show();
                }
                else {
                    cboPatientOutcome.SetVisible(true);
                    cboPatientOutcomeDead.SetVisible(false);
                    $('#tblEntry tr.trDeathInfo').hide();
                }
            });
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

            if (diff.d < 2)
                cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadBefore48() %>');
            else
                cboPatientOutcomeDead.SetValue('<%=GetPatientOutcomeDeadAfter48() %>');
        }

        function onCboDischargeReasonValueChanged() {
            if (cboDischargeReason.GetValue() == Constant.DischargeReasonToOtherHospital.OTHER) {
                $('#trDischargeOtherReason').removeAttr('style');
            }
            else {
                $('#trDischargeOtherReason').attr('style', 'display:none');
            }
        }

        function onCboDischargeRoutineValueChanged() {
            if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.OTHER_HOSPITAL) {
                cboDischargeReason.SetValue('');
                $('#trDischargeReason').removeAttr('style');
                $('#trReferrerGroup').removeAttr('style');
                $('#trReferrer').removeAttr('style');
                $("#divAppointment").hide();
                $('#trInpatientPhysician').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                $("#divAppointment").show();
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $('#trInpatientPhysician').attr('style', 'display:none');
            }
            else if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT) {
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#divAppointment").hide();
                $('#trInpatientPhysician').removeAttr('style');
            }
            else {
                $('#trDischargeReason').attr('style', 'display:none');
                $('#trReferrerGroup').attr('style', 'display:none');
                $('#trReferrer').attr('style', 'display:none');
                $('#trDischargeOtherReason').attr('style', 'display:none');
                $("#divAppointment").hide();
                $('#trInpatientPhysician').attr('style', 'display:none');
            }
        }

        //#region Referrer
        function onCboReferrerGroupValueChanged() {
            $('#<%=hdnReferrerID.ClientID %>').val('');
            $('#<%=txtReferrerCode.ClientID %>').val('');
            $('#<%=txtReferrerName.ClientID %>').val('');
            return "GCReferrerGroup = '" + cboReferrerGroup.GetValue() + "'";
        }

        $('#<%:lbReferrerCode.ClientID %>.lblLink').live('click', function () {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND IsDeleted = 0"
            openSearchDialog('referrer2', filterExpression, function (value) {
                $('#<%=txtReferrerCode.ClientID %>').val(value);
                onTxtReferrerCodeChanged(value);
            });
        });

        $('#<%=txtReferrerCode.ClientID %>').live('change', function () {
            onTxtReferrerCodeChanged($(this).val());
        });

        function onTxtReferrerCodeChanged(value) {
            var filterExpression = onCboReferrerGroupValueChanged() + " AND BusinessPartnerCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtReferrerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtReferrerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnReferrerID.ClientID %>').val('');
                    $('#<%=txtReferrerCode.ClientID %>').val('');
                    $('#<%=txtReferrerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Physician
        function getPhysicianFilterExpression() {
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';
            if (polyclinicID != '' && polyclinicID != null)
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        });

        $('#lblParamedic2.lblLink').live('click', function () {
            openSearchDialog('paramedic', " GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'", function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
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
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
        <input type="hidden" id="hdnLOSInDay" runat="server" />
        <input type="hidden" id="hdnLOSInHour" runat="server" />
        <input type="hidden" id="hdnLOSInMinute" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 450px" />
                <col />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <fieldset id="fsPatientDischarge">
                        <table class="tblEntryContent" id="tblEntry" style="width: 500px">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal")%>
                                        -
                                        <%=GetLabel("Jam")%></label>
                                </td>
                                <td>
                                    <asp:textbox id="txtDischargeDate" width="120px" cssclass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <asp:textbox id="txtDischargeTime" width="80px" cssclass="time" runat="server" style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lama Perawatan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtLengthOfVisit" readonly="true" width="98%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:checkbox id="chkIsDead" runat="server" text="Meninggal" />
                                </td>
                            </tr>
                            <tr class="trDeathInfo" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Death Date - Time")%></label>
                                </td>
                                <td>
                                    <asp:textbox runat="server" id="txtDateOfDeath" cssclass="datepicker" width="120px" />
                                </td>
                                <td>
                                    <asp:textbox runat="server" id="txtTimeOfDeath" cssclass="time" width="80px" maxlength="5" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Keadaan Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%"
                                        runat="server" />
                                    <dxe:ASPxComboBox ID="cboPatientOutcomeDead" ClientInstanceName="cboPatientOutcomeDead"
                                        Width="100%" runat="server" ClientVisible="false">
                                        <ClientSideEvents Init="function(s,e){ onDischargeDateTimeChange(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Cara Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboDischargeRoutine" Width="100%" runat="server" ClientInstanceName="cboDischargeRoutine">
                                        <ClientSideEvents Init="function(s,e){ onCboDischargeRoutineValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeRoutineValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trReferrerGroup" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Rujuk Ke")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboReferrerGroup" Width="100%" runat="server" ClientInstanceName="cboReferrerGroup">
                                        <ClientSideEvents Init="function(s,e){ onCboReferrerGroupValueChanged(s); }" ValueChanged="function(s,e){ onCboReferrerGroupValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trReferrer" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lbReferrerCode" runat="server">
                                        <%:GetLabel("Rumah Sakit / Faskes")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                    <table style="width: 180%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:textbox id="txtReferrerCode" width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:textbox id="txtReferrerName" width="100%" readonly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDischargeReason" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Alasan Ke Rumah Sakit lain")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboDischargeReason" Width="100%" runat="server" ClientInstanceName="cboDischargeReason">
                                        <ClientSideEvents Init="function(s,e){ onCboDischargeReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboDischargeReasonValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr id="trDischargeOtherReason" style="display: none">
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtDischargeOtherReason" width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trInpatientPhysician" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblParamedic2">
                                        <%=GetLabel("DPJP")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 100px">
                                                <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                                <asp:textbox runat="server" id="txtParamedicCode" width="100%" />
                                            </td>
                                            <td style="width: 255px">
                                                <asp:textbox runat="server" id="txtParamedicName" readonly="true" width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="vertical-align: top" rowspan="2">
                    <div id="divAppointment" style="display: none">
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 180px" />
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
                                    <asp:textbox id="txtPhysicianCode" cssclass="required" width="100%" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtPhysicianName" readonly="true" width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Appointment Date")%></label>
                                </td>
                                <td>
                                    <asp:textbox runat="server" id="txtAppointmentDate" cssclass="datepicker" width="120px" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Remarks")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:textbox id="txtRemarks" width="100%" runat="server" textmode="MultiLine" height="220px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:content>
