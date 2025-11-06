<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientReferralEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.PatientReferralEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    <%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientReferralEntryCtl">
    setDatePicker('<%=txtReferralDate.ClientID %>');
    $('#<%=txtReferralDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    $('#<%=txtDiagnosisText.ClientID %>').focus();

    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }
    function onCboPhysician2() {
        var IsDefaultServiceUnitParamedic = $('#<%=hdnIsDefaultServiceUnitParamedic.ClientID %>').val();   
        if (IsDefaultServiceUnitParamedic == "1") {
            cbpPhysician.PerformCallback('refresh');
        }
    }

    $('#lblAddFromDxHistory2.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND IsDeleted = 0";
        openSearchDialog('patientDiagnosis', filterExpression, function (value) {
            $('#<%=hdnPatientDiagnosisID.ClientID %>').val(value);
            onSearchPatientDiagnosisValue(value);
        });
    });

    function onSearchPatientDiagnosisValue(value) {
        var objText = $('#<%=txtDiagnosisText.ClientID %>').val();
        var filterExpression = "ID = " + value;
        Methods.getObject('GetvPatientDiagnosis3List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnosisText.ClientID %>').val(objText + result.cfDiagnosisText);
            }
            else {
                $('#<%=txtDiagnosisText.ClientID %>').val('');
            }
        });
    }

    $('#lblAddFromROSLookup.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND IsDeleted = 0";
        openSearchDialog('reviewOfSystem', filterExpression, function (value) {
            $('#<%=hdnPatientReviewOfSystemID.ClientID %>').val(value);
            onSearchPatientReviewOfSystemValue(value);
        });
    });

    function onSearchPatientReviewOfSystemValue(value) {
        var objText = $('#<%=txtMedicalResumeText.ClientID %>').val();
        var filterExpression = "ID = " + value;
        var selectedAtrikel = "";
        var TanggalPemeriksaan = "";

        Methods.getListObject('GetvReviewOfSystemDt2List', filterExpression, function (result) {
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    TanggalPemeriksaan = result[i].ObservationDateInString + ", " + result[i].ObservationTime + ", " + result[i].ParamedicName;
                    selectedAtrikel += TanggalPemeriksaan + "\n" + result[i].DisplayRemarks + "\n";
                }
                $('#<%=txtMedicalResumeText.ClientID %>').val(objText + selectedAtrikel);
            }
            else {
                $('#<%=txtMedicalResumeText.ClientID %>').val('');
            }
        });
    }

    $('#lblLaboratory.lblLink').live('click', function () {
        var filterExpression = "VisitID IN " + $('#<%=hdnLinkedVisitID.ClientID %>').val() + " AND GCItemType = 'X001^004'";
        openSearchDialog('patientChargesDiagnosticResult', filterExpression, function (value) {
            $('#<%=hdnTransactionID.ClientID %>').val(value);
            onSearchLaboratoryValue(value);
        });
    });

    function onSearchLaboratoryValue(value) {
        var objText = $('#<%=txtMedicalResumeText.ClientID %>').val();
        var filterExpression = "TransactionID = " + value + " AND GCItemType = 'X001^004'";
        var selectedAtrikel = "";
        var serviceUnitName = "";

        Methods.getListObject('GetvPatientChargesDiagnosticResultList', filterExpression, function (result) {
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    serviceUnitName = "Laboratorium :";
                    selectedAtrikel += serviceUnitName + "\n" + result[i].DisplayItemName + "\n" + "\n";
                }
                $('#<%=txtMedicalResumeText.ClientID %>').val(objText + selectedAtrikel);
            }
            else {
                $('#<%=txtMedicalResumeText.ClientID %>').val('');
            }
        });
    }

    $('#lblImaging.lblLink').live('click', function () {
        var filterExpression = "VisitID IN " + $('#<%=hdnLinkedVisitID.ClientID %>').val() + " AND GCItemType = 'X001^005'";
        openSearchDialog('patientChargesDiagnosticResult', filterExpression, function (value) {
            $('#<%=hdnTransactionID.ClientID %>').val(value);
            onSearchImagingValue(value);
        });
    });

    function onSearchImagingValue(value) {
        var objText = $('#<%=txtMedicalResumeText.ClientID %>').val();
        var filterExpression = "TransactionID = " + value + " AND GCItemType = 'X001^005'";
        var selectedAtrikel = "";
        var serviceUnitName = "";
        Methods.getListObject('GetvPatientChargesDiagnosticResultList', filterExpression, function (result) {
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    serviceUnitName = "Radiologi :";
                    selectedAtrikel += serviceUnitName + "\n" + result[i].DisplayItemName + "\n" + "\n";
                }
                $('#<%=txtMedicalResumeText.ClientID %>').val(objText + selectedAtrikel);
            }
            else {
                $('#<%=txtMedicalResumeText.ClientID %>').val('');
            }
        });
    }

    $('#lblOtherDiagnostic.lblLink').live('click', function () {
        var filterExpression = "VisitID IN " + $('#<%=hdnLinkedVisitID.ClientID %>').val() + " AND GCItemType = 'X001^006'";
        openSearchDialog('patientChargesDiagnosticResult', filterExpression, function (value) {
            $('#<%=hdnTransactionID.ClientID %>').val(value);
            onSearchOtherDiagnosticValue(value);
        });
    });

    function onSearchOtherDiagnosticValue(value) {
        var objText = $('#<%=txtMedicalResumeText.ClientID %>').val();
        var filterExpression = "TransactionID = " + value + " AND GCItemType = 'X001^006'";
        var selectedAtrikel = "";
        var serviceUnitName = "";

        Methods.getListObject('GetvPatientChargesDiagnosticResultList', filterExpression, function (result) {
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    serviceUnitName = result[i].ServiceUnitName + " :";
                    selectedAtrikel += serviceUnitName + "\n" + result[i].DisplayItemName + "\n" + "\n";
                }
                $('#<%=txtMedicalResumeText.ClientID %>').val(objText + selectedAtrikel);
            }
            else {
                $('#<%=txtMedicalResumeText.ClientID %>').val('');
            }
        });
    }

    $('#lblEpisodeMedication.lblLink').live('click', function () {
        var filterExpression = "VisitID IN " + $('#<%=hdnLinkedVisitID.ClientID %>').val();
        openSearchDialog('medicationEpisode', filterExpression, function (value) {
            $('#<%=hdnTransactionID.ClientID %>').val(value);
            onSearchEpisodeMedicationValue(value);
        });
    });

    function onSearchEpisodeMedicationValue(value) {
        var objText = $('#<%=txtPlanningResumeText.ClientID %>').val();
        var filterExpression = "PrescriptionOrderID = " + value;
        var selectedAtrikel = "";
        var serviceUnitName = "";

        Methods.getListObject('GetvMedicationEpisodeList', filterExpression, function (result) {
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    TanggalPemeriksaan = result[i].PrescriptionOrderNo + ", " + result[i].PrescriptionDateInString + ", " + result[i].PrescriptionTime;
                    selectedAtrikel += TanggalPemeriksaan + "\n"  + result[i].DisplayDrugName + "\n" + "\n";
                }
                $('#<%=txtPlanningResumeText.ClientID %>').val(objText + selectedAtrikel);
            }
            else {
                $('#<%=txtPlanningResumeText.ClientID %>').val('');
            }
        });
    }

    function onCbpPhysicianEndCallback(s) {
        hideLoadingPanel();

    }
</script>
<div style="height: 500px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="0" id="hdnIsDefaultAppointment" runat="server" />
    <input type="hidden" value="0" id="hdnIsDefaultServiceUnitParamedic" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowCreateAppointment" runat="server" />
    <input type="hidden" value="0" id="hdnPatientDiagnosisID" runat="server" />
    <input type="hidden" value="0" id="hdnPatientReviewOfSystemID" runat="server" />
    <input type="hidden" value="0" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedVisitID" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
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
                <asp:TextBox ID="txtReferralDate" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtReferralTime" Width="80px" CssClass="time" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Dokter ")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician"
                    Width="300px" />
            </td>
            
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Dokter Rujukan")%></label>
            </td>
            <td colspan="2">
     

                <dxe:ASPxComboBox runat="server" ID="cboPhysician2" ClientInstanceName="cboPhysician2"
                    Width="300px">
                    <ClientSideEvents ValueChanged="function(s,e){
                                    onCboPhysician2();
                                }" />
                  </dxe:ASPxComboBox>

            </td>
        </tr>
        <tr id="trClinic" runat="server" style="display:none">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Klinik Rujukan")%></label>
            </td>
            <td colspan="2">
              <dxcp:ASPxCallbackPanel ID="cbpPhysician" runat="server" Width="100%" ClientInstanceName="cbpPhysician"
                                        ShowLoadingPanel="false" OnCallback="cbpPhysician_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPhysicianEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 22px; ">
                                                    <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="300px" />
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>

                
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tipe Rujukan")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboReferralType" ClientInstanceName="cboReferralType"
                    Width="300px" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblMandatory">
                    <%=GetLabel("Diagnosis Pasien:")%></label>
                    <span class="lblLink" id="lblAddFromDxHistory2"><%= GetLabel(" + Lookup Diagnosa")%></span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtDiagnosisText" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="2" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblMandatory">
                    <%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="lblLink" id="lblAddFromROSLookup"><%= GetLabel(" + Lookup : Pemeriksaan Fisik")%></span>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <span id="Span5"><%= GetLabel("+ Lookup : Pemeriksaan : ")%></span>
                <span class="lblLink" id="lblLaboratory"><%= GetLabel("   Laboratorium")%></span>
                <span id="Span2"><%= GetLabel(" | ")%></span>
                <span class="lblLink" id="lblImaging"><%= GetLabel("   Radiologi")%></span>
                <span id="Span6"><%= GetLabel(" | ")%></span>
                <span class="lblLink" id="lblOtherDiagnostic"><%= GetLabel("   Lain - lain")%></span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtMedicalResumeText" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="6" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <label class="lblMandatory">
                    <%=GetLabel("Terapi yang telah diberikan :")%></label>
                    <span class="lblLink" id="lblEpisodeMedication"><%= GetLabel(" + Lookup Terapi Pengobatan")%></span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtPlanningResumeText" Width="95%" runat="server" TextMode="MultiLine"
                    Rows="6" />
            </td>
        </tr>
    </table>
</div>
