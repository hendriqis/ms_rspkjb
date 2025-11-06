<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferralVisitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.ReferralVisitCtl" %>
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
        $('#<%=txtDiagnosisText.ClientID %>').val('');
        $('#<%=txtMedicalResumeText.ClientID %>').val('');
        $('#<%=txtPlanningResumeText.ClientID %>').val('');
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

    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var polyclinicID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
        if (polyclinicID != '')
            filterExpression = "GCParamedicMasterType = 'X019^001' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
        return filterExpression;
    }

    $('#lblPatientVisitPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPatientVisitPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPatientVisitPhysicianCodeChanged($(this).val());
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
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
            }
            else {
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitTypeCode.ClientID %>').val('');
                $('#<%=txtVisitTypeName.ClientID %>').val('');
            }
        });
    }
    //#endregion

        $('.imgPatientVisitEdit.imgLink').die('click');
        $('.imgPatientVisitEdit.imgLink').live('click', function () {
            $row = $(this).parent().parent();

            var visitID = $row.find('.hdnVisitID').val();
            var serviceUnitID = $row.find('.hdnServiceUnitID').val();
            var paramedicID = $row.find('.hdnParamedicID').val();
            var serviceUnitCode = $row.find('.hdnServiceUnitCode').val();
            var paramedicCode = $row.find('.hdnParamedicCode').val();
            var serviceUnitName = $row.find('.divServiceUnitName').html();
            var paramedicName = $row.find('.divParamedicName').html();

            var visitTypeID = $row.find('.hdnVisitTypeID').val();
            var visitTypeCode = $row.find('.hdnVisitTypeCode').val();
            var visitTypeName = $row.find('.divVisitTypeName').html();
            var specialtyID = $row.find('.hdnSpecialtyID').val();
            var visitReason = $row.find('.hdnVisitReason').val();
            var patientReferralID = $row.find('.hdnPatientReferralID').val();
            var diagnosisText = $row.find('.hdnDiagnosisText').val();
            var medicalResumeText = $row.find('.hdnMedicalResumeText').val();
            var planningResumeText = $row.find('.hdnPlanningResumeText').val();

            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(serviceUnitID);
            $('#<%=txtClinicCode.ClientID %>').val(serviceUnitCode);
            $('#<%=txtClinicName.ClientID %>').val(serviceUnitName);
            $('#<%=hdnPhysicianID.ClientID %>').val(paramedicID);
            $('#<%=txtPhysicianCode.ClientID %>').val(paramedicCode);
            $('#<%=txtPhysicianName.ClientID %>').val(paramedicName);
            $('#<%=hdnVisitTypeID.ClientID %>').val(visitTypeID);
            $('#<%=txtVisitTypeCode.ClientID %>').val(visitTypeCode);
            $('#<%=txtVisitTypeName.ClientID %>').val(visitTypeName);
            $('#<%=hdnVisitID.ClientID %>').val(visitID);
            $('#<%=hdnPatientReferralID.ClientID %>').val(patientReferralID);
            $('#<%=txtDiagnosisText.ClientID %>').val(diagnosisText);
            $('#<%=txtMedicalResumeText.ClientID %>').val(medicalResumeText);
            $('#<%=txtPlanningResumeText.ClientID %>').val(planningResumeText);
            cboRegistrationEditSpecialty.SetValue(specialtyID);
            
            $('#containerPatientVisitEntryData').show();
        });

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
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 600px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnClassID" />
    <input type="hidden" value="" runat="server" id="hdnBusinessPartnerID" />
    <input type="hidden" value="" runat="server" id="hdnItemCardFee" />
    <input type="hidden" value="" runat="server" id="hdnPatientReferralID" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td colspan="2">
                <table style="width:100%">
                    <colgroup width="70px" />
                    <colgroup />
                    <tr>
                        <td>
                            <img src='<%=ResolveUrl("~/Libs/Images/add_visit.png")%>' alt="" height="65px" width="65px" />
                        </td>
                        <td style="vertical-align:top;">
                            <h4 style="background-color:transparent;color:black;font-weight:bold"><%=GetLabel("Kunjungan ke Klinik Lain")%></h4>
                            <%=GetLabel("Pembuatan Rujukan Pasien ke Poli Lain dengan nomor registrasi yang sama")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
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
                                <%=GetLabel("No. Registrasi")%>
                                /
                                <%=GetLabel("No. RM")%></label>
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
                        <%=GetLabel("Entry Rujukan")%></div>
                    <input type="hidden" id="hdnVisitID" runat="server" value="" />
                    <fieldset id="fsPatientVisit" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblPatientVisitPhysician">
                                        <%=GetLabel("Dokter ")%></label>
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
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
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
                                <td colspan="2">
                                    <label class="lblMandatory"><%=GetLabel("Diagnosis Pasien:")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:TextBox ID="txtDiagnosisText" CssClass="required" Width="95%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblMandatory"><%=GetLabel("Pemeriksaan Fisik / Pemeriksaan Penunjang / Catatan Medis lain yang perlu mendapat perhatian :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:TextBox ID="txtMedicalResumeText" CssClass="required" Width="95%" runat="server" TextMode="MultiLine" Rows="3" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label class="lblMandatory"><%=GetLabel("Terapi yang telah diberikan :")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:TextBox ID="txtPlanningResumeText" CssClass="required" Width="95%" runat="server" TextMode="MultiLine" Rows="3" /></td>
                            </tr>
                            <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" class="btnPropose w3-btn w3-hover-blue" id="btnPatientVisitSave" value='<%= GetLabel("Save")%>' style="width:80px"/>
                                        </td>
                                        <td>
                                            <input type="button" class="btnPropose w3-btn w3-hover-blue" id="btnPatientVisitCancel" value='<%= GetLabel("Cancel")%>' style="width:80px" />
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
                                                <img class='imgPatientVisitEdit <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Edit")%>' 
                                                    src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;margin-right: 2px;" />
                                                <img class='imgPatientVisitDelete <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Delete")%>' src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Informasi Kunjungan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" class="hdnVisitID" value="<%#: Eval("VisitID")%>" />
                                                <input type="hidden" class="hdnServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                <input type="hidden" class="hdnParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="hdnServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                <input type="hidden" class="hdnParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" class="hdnSpecialtyID" value="<%#: Eval("SpecialtyID")%>" />
                                                <input type="hidden" class="hdnVisitTypeID" value="<%#: Eval("VisitTypeID")%>" />
                                                <input type="hidden" class="hdnVisitTypeCode" value="<%#: Eval("VisitTypeCode")%>" />
                                                <input type="hidden" class="hdnVisitReason" value="<%#: Eval("VisitReason")%>" />
                                                <input type="hidden" class="hdnPatientReferralID" value="<%#: Eval("PatientReferralID")%>" />
                                                <input type="hidden" class="hdnDiagnosisText" value="<%#: Eval("DiagnosisText")%>" />
                                                <input type="hidden" class="hdnMedicalResumeText" value="<%#: Eval("MedicalResumeText")%>" />
                                                <input type="hidden" class="hdnPlanningResumeText" value="<%#: Eval("PlanningResumeText")%>" />
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
                                                    <%#: Eval("QueueNo") %></div>
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
                        <%= GetLabel("Tambah Kunjungan Rujukan")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
