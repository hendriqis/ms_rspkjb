<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PatientDischarge1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientDischarge1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            setDatePicker('<%=txtDischargeDate.ClientID %>');
            setDatePicker('<%=txtDateOfDeath.ClientID %>');
            setDatePicker('<%=txtAppointmentDate.ClientID %>');

            $('#<%=txtDischargeDate.ClientID %>').datepicker('option', 'maxDate', '0');
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

            $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnDiagnosisID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            $('.btnApplyDiagnosis').click(function () {
                submitDiagnosis();
                $('#<%=ledDiagnose.ClientID %>').focus();
            });

            $('.btnCancelDiagnosis').click(function () {
                ResetDiagnosisEntryControls();
            });

            function submitDiagnosis() {
                if ((cboDiagnosisType.GetValue() != '' && $('#<%=txtDiagnosisText.ClientID %>').val() != '')) {
                    if ($('#<%=hdnDiagnosisProcessMode.ClientID %>').val() == "1")
                        cbpDiagnosis.PerformCallback('add');
                    else
                        cbpDiagnosis.PerformCallback('edit');
                }
                else {
                    showToast("ERROR", "You should fill Diagnosis Type and Name field !");
                }
            }

            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            $('#<%=txtDiagnosisText.ClientID %>').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    submitDiagnosis();
                }
            }); 

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

        function onCboDischargeRoutineChanged() {
            if (cboDischargeRoutine.GetValue() != null && (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT || cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_INPATIENT)) {
                if (cboDischargeRoutine.GetValue() == Constant.DischargeRoutine.REFER_TO_OUTPATIENT) {
                    $('#tblDischarge tr.trInpatientPhysician').hide();
                    $("#tblDischarge tr.trAppointment").show();
                }
                else {
                    $("#tblDischarge tr.trAppointment").hide();
                    $('#tblDischarge tr.trInpatientPhysician').show();
                }
            }
            else {
                $("#tblDischarge tr.trAppointment").hide();
                $('#tblDischarge tr.trInpatientPhysician').hide();
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

        function onAfterCustomClickSuccess(type) {
            exitPatientPage();
        }
    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnRegistrationDateTime" runat="server" />
    <input type="hidden" value="" id="hdnDiagnosisID" runat="server" />
    <input type="hidden" value="1" id="hdnDiagnosisProcessMode" runat="server" />
    <input type="hidden" id="hdnIsMainDiagnosisExists" runat="server" value='0' />
    <input type="hidden" id="hdnDiagnosisSummary" runat="server" />

    <fieldset id="fsPatientDischarge">  

    <table class="tblEntryContent" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col width="45%" />
            <col />
        </colgroup>
        <tr>
            <td style="vertical-align:top;">
                <table id="tblDischarge" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
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
                            <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" Style="text-align:center" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Keadaan Keluar")%></label>
                        </td>
                        <td colspan="4">
                            <dxe:ASPxComboBox ID="cboPatientOutcome" ClientInstanceName="cboPatientOutcome" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientOutcomeChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Cara Keluar / Tindak Lanjut")%></label>
                        </td>
                        <td colspan="4">
                            <dxe:ASPxComboBox ID="cboDischargeRoutine" ClientInstanceName="cboDischargeRoutine"  Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDischargeRoutineChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr class="trDeathInfo" style="display:none">
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
                    <tr class="trInpatientPhysician" style="display:none">
                        <td class="tdLabel">
                            <label class="lblNormal lblLink" id="lblParamedic2">
                                <%=GetLabel("DPJP")%></label>
                        </td>
                        <td colspan="4">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width:100px">
                                        <input type="hidden" id="hdnParamedicID2" value="" runat="server" />
                                        <asp:TextBox runat="server" ID="txtParamedicCode" Width="100%" />
                                    </td>
                                    <td  style="width:255px">
                                        <asp:TextBox runat="server" ID="txtParamedicName" ReadOnly="true" Width="100%" />
                                    </td>
                                </tr>                            
                            </table>
                        </td>
                    </tr>  
                    <tr class="trAppointment" style="display:none">
                        <td colspan="5">
                            <div id="divAppointment">
                                <table style="width: 100%" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Klinik Rujukan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" id="lblPhysician">
                                                <%=GetLabel("Dokter ")%></label>
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
                                                <%=GetLabel("Tanggal Perjanjian")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAppointmentDate" CssClass="datepicker" Width="120px" />
                                        </td>
                                    </tr>
                                    <tr style="display:none">
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
                </table>
            </td>
            <td style="vertical-align:top" rowspan="2">
                <h4 class="h4collapsed">
                    <%=GetLabel("Diagnosa")%></h4>
                <div class="containerTblEntryContent containerEntryPanel1">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="100px" />
                                        <col width="150px" />
                                        <col width="100px" />
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("Tipe Diagnosa")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboDiagnosisType" ClientInstanceName="cboDiagnosisType"
                                                Width="100%">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("ICD X")%></label>
                                        </td>
                                        <td colspan="3">
                                            <input type="hidden" value="" id="hdnEntryDiagnoseID" runat="server" />
                                            <input type="hidden" value="" id="hdnEntryDiagnoseText" runat="server" />
                                            <qis:QISSearchTextBox ID="ledDiagnose" ClientInstanceName="ledDiagnose" runat="server"
                                                Width="99%" ValueText="DiagnoseID" FilterExpression="" DisplayText="DiagnoseName"
                                                MethodName="GetDiagnosisList">
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
                                            <label class="lblNormal">
                                                <%=GetLabel("Status Diagnosa")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboDiagnosisStatus" ClientInstanceName="cboDiagnosisStatus"
                                                Width="100%">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Diagnosa Text")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDiagnosisText" runat="server" Width="370px" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <table border="0" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <img class="btnApplyDiagnosis imgLink" title='<%=GetLabel("Apply")%>' src='<%= ResolveUrl("~/Libs/Images/status/done.png")%>'
                                                            alt="" />
                                                    </td>
                                                    <td>
                                                        <img class="btnCancelDiagnosis imgLink" title='<%=GetLabel("Cancel")%>' src='<%= ResolveUrl("~/Libs/Images/status/cancel.png")%>'
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
                            <td>
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                                        ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditDiagnosis imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeleteDiagnosis imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                    alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Diagnose Information")%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                        <%#: Eval("DifferentialDateInString")%>,
                                                                        <%#: Eval("DifferentialTime")%>,
                                                                        <%#: Eval("GCDifferentialStatus")%>,                                                                                
                                                                        <%#: Eval("ParamedicName")%></div>
                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                        <span Style="color: Blue; font-size: 1.1em">
                                                                            <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                                    </div>
                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                        <%#: Eval("ICDBlockName")%></div>
                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                        <b>
                                                                            <%#: Eval("DiagnoseType")%></b> -
                                                                        <%#: Eval("DifferentialStatus")%></div>
                                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                                        <%#: Eval("Remarks")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="diagnosisPaging">
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
    </table>
    </fieldset>

</asp:Content>
