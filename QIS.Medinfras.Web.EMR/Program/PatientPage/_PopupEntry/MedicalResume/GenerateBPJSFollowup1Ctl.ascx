<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateBPJSFollowup1Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.GenerateBPJSFollowup1Ctl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<script type="text/javascript" id="dxss_diagnosisentryctl">
    $(function () {
        $('#<%=rblReferBackType.ClientID %> input').die('change');
        $('#<%=rblReferBackType.ClientID %> input').live('change', function () {
            ToggleFollowUpVisitControl();
        });

        setDatePicker('<%=txtMedicalResumeDate.ClientID %>');
        setDatePicker('<%=txtFollowupVisitDate.ClientID %>');
        $('#<%=txtMedicalResumeDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtFollowupVisitDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtAssessmentText.ClientID %>').focus();

        ToggleFollowUpVisitControl();
    });

    function ToggleFollowUpVisitControl() {
        var value = $('#<%=rblReferBackType.ClientID %> input[type=radio]:checked').val();
        if (value == '1') {
            $('#trFollowupVisit').attr('style', 'display:none');
            $('#trReferralNotes').removeAttr('style');
        }
        else {
            $('#trFollowupVisit').removeAttr('style');
            $('#trReferralNotes').attr('style', 'display:none');
        }
    }

    function onAfterSaveEditRecordEntryPopup(value) {
        if (typeof onAfterGenerateBPSFollowupVisit == 'function')
            onAfterGenerateBPSFollowupVisit(value);
    }

    $('#ulTabFollowup li').click(function () {
        $('#ulTabFollowup li.selected').removeAttr('class');
        $('.ctFollowupDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID = $contentID;
    });


    function onCboClinicValueChanged(s) {
        var parameter2 = "HealthcareServiceUnitID = " + cboClinic.GetValue() + "";
        Methods.getObject('GetvHealthcareServiceUnitList', parameter2, function (result2) {
            if (result2 != null) {
                $('#<%=hdnBPJSPoli.ClientID %>').val(result2.BPJSPoli);
            }
            else $('#<%=hdnBPJSPoli.ClientID %>').val('');
        });

        $('#<%=txtPhysicianCode.ClientID %>').val('');
        onTxtPatientVisitPhysicianCodeChanged('');
        cbpPopulateVisitType.PerformCallback(cboClinic.GetValue());
    }

    function onCboDiagnosisValueChanged(s) {
        var filterExpressionDiagnose = "DiagnoseID = '" + cboDiagnosis.GetValue() + "'";
        Methods.getObject('GetDiagnoseList', filterExpressionDiagnose, function (resultCheck) {
            if (resultCheck != null) {
                $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val(resultCheck.INACBGLabel);
            }
            else {
                $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                showToast("BPJS Bridging", "Diagnosa " + cboDiagnosis.GetText() + " (" + cboDiagnosis.GetValue() + ") belum dimapping dengan Referensi BPJS (Diagnosa) !");
            }
        });
    }

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

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);

                if (result.BPJSReferenceInfo == "" || result.BPJSReferenceInfo == null) {
                    $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val('');
                    showToast("BPJS Bridging", "Dokter " + result.ParamedicName + " belum dimapping dengan Referensi BPJS (VClaim dan HFIS) !");
                }
                else {
                    $('#<%:hdnPhysicianBPJSReferenceInfo.ClientID %>').val(result.BPJSReferenceInfo);
                }
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpPopulateVisitTypeEndCallback(s) {
    }
</script>
<div style="height: auto; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnKodeDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnNamaDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestID" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td>
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Surat")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalResumeDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="tdLabel" style="width:150px">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Surat Kontrol")%></label>
                                    </td>     
                                    <td>
                                        <asp:TextBox ID="txtNoSuratKontrol1" runat="server" Width="100px" ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoSuratKontrol2" runat="server" Width="100px" ReadOnly="true" />
                                    </td>                                                                
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dokter ")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician"
                                Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblMandatory">
                                <%=GetLabel("Diagnosa")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAssessmentText" Width="95%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Terapi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Rujukan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblReferBackType" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Text=" Dapat Dikembalikan / Rujuk Balik" Value="1" Selected="True" />
                                <asp:ListItem Text=" Belum dapat dikembalikan / belum dapat dirujuk balik; dengan alasan"
                                    Value="2" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td colspan="2" style="padding-left: 20px">
                            <asp:CheckBox ID="chkRefferalReasonMedication" runat="server" Text=" Terapi Farmakologis tidak tersedia di Faskes Tingkat 1"
                                Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td colspan="2" style="padding-left: 20px">
                            <asp:CheckBox ID="chkRefferalReasonFollowup" runat="server" Text=" Perlu Follow up hasil pemeriksaan dan terapi"
                                Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td colspan="2" style="padding-left: 20px">
                            <asp:CheckBox ID="chkRefferalReasonOther" runat="server" Text=" Lain-lain" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td colspan="2" style="padding-left: 40px">
                            <asp:TextBox ID="txtRefferalReasonOtherText" Width="95%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                    <tr id="trReferralNotes">
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Rujukan Balik")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtReferralNotes" Width="95%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trFollowupVisit">
            <td>
                <div class="containerUlTabPage">
                    <ul class="ulTabPage" id="ulTabFollowup">
                        <li class="selected" contentid="ctSchedule">
                            <%=GetLabel("Jadwal Kunjungan Berikutnya")%></li>
                        <li contentid="ctPlanning">
                            <%=GetLabel("Rencana Tindak Lanjut Pada Kunjungan Berikutnya")%></li>
                    </ul>
                </div>
                <div id="ctSchedule" class="ctFollowupDt">
                    <table style="width: 100%" class="tblEntryContent">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Kontrol")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFollowupVisitDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Poli Pelayanan")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnBPJSPoli" value="" runat="server" />
                                <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%">
                                    <ClientSideEvents ValueChanged="function(s){ onCboClinicValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Kunjungan") %></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboFollowupVisitType" ClientInstanceName="cboFollowupVisitType"
                                    Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" id="lblPhysician">
                                    <%=GetLabel("Dokter Tujuan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <input type="hidden" id="hdnPhysicianBPJSReferenceInfo" value="" runat="server" />
                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Diagnosa Rujukan")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnBPJSDiagnoseCode" value="" runat="server" />                            
                                <dxe:ASPxComboBox runat="server" ID="cboDiagnosis" ClientInstanceName="cboDiagnosis" Width="100%">
                                    <ClientSideEvents ValueChanged="function(s) { onCboDiagnosisValueChanged(s);}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="ctPlanning" class="ctFollowupDt" style="display: none">
                    <table style="width: 100%" class="tblEntryContent">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="padding-left: 20px">
                                <asp:CheckBox ID="chkPlanForMedication" runat="server" Text=" Terapi Farmakologis lanjutan yang tidak tersedia di Faskes Tingkat 1"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 20px">
                                <asp:CheckBox ID="chkPlanForTheraphy" runat="server" Text=" Follow up hasil pemeriksaan dan terapi"
                                    Checked="false" />
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 20px">
                                <asp:CheckBox ID="chkPlanForOthers" runat="server" Text=" Lain-lain" Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 40px">
                                <asp:TextBox ID="txtPlanForOtherRemarks" Width="95%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>

    <dxcp:ASPxCallbackPanel ID="cbpPopulateVisitType" runat="server" Width="100%" ClientInstanceName="cbpPopulateVisitType"
        ShowLoadingPanel="false" OnCallback="cbpPopulateVisitType_Callback">
        <ClientSideEvents EndCallback="function(s,e) { onCbpPopulateVisitTypeEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
