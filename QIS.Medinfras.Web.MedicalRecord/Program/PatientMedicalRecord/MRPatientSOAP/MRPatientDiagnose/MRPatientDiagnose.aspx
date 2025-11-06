<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="MRPatientDiagnose.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MRPatientDiagnose" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFinalDate.ClientID %>');
            $('#<%=txtFinalDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Entity To Control
        function entityToControl(entity) {
            var hdnIsRMDiagnoseTextEditable = $('#<%=hdnIsRMDiagnoseTextEditable.ClientID %>').val();
            if (hdnIsRMDiagnoseTextEditable == "0") {
                $('#<%=txtDiagnosisText.ClientID %>').attr('ReadOnly', 'ReadOnly');
            } else {
                $('#<%=txtDiagnosisText.ClientID %>').removeAttr('ReadOnly');
            }

            $('#<%=txtFinalDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');

            var filterVisit = "VisitID = '" + $('#<%=hdnVisitID.ClientID %>').val() + "'";
            Methods.getObject('GetConsultVisitList', filterVisit, function (result) {
                if (result != null) {
                    if (result.GCCaseType == "" || result.GCCaseType == null) {
                        var filterExpressionSpecialty = "SpecialtyID = '" + result.SpecialtyID + "'";
                        Methods.getObject('GetSpecialtyList', filterExpressionSpecialty, function (resultSpecialty) {
                            if (resultSpecialty != null) {
                                $('#<%=hdnDefaultVisitCaseType.ClientID %>').val(resultSpecialty.GCCaseType);
                            }
                            else {
                                $('#<%=hdnDefaultVisitCaseType.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%=hdnDefaultVisitCaseType.ClientID %>').val(result.GCCaseType);
                    }
                }
            });

            cboVisitCaseType.SetValue($('#<%=hdnDefaultVisitCaseType.ClientID %>').val());

            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

                cboDiagnoseType.SetValue(entity.GCDiagnoseType);
                cboDiagnoseType.SetText(entity.DiagnoseType);

                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);

                $('#<%=txtSpecialtyName.ClientID %>').val(entity.SpecialtyName);

                $('#<%=txtDifferentialDate.ClientID %>').val(entity.DifferentialDate);
                $('#<%=txtDifferentialTime.ClientID %>').val(entity.DifferentialTime);
                $('#<%=txtDifferentialDiagnoseCode.ClientID %>').val(entity.DiagnoseID);
                $('#<%=txtDifferentialDiagnoseName.ClientID %>').val(entity.DiagnoseName);
                $('#<%=txtDifferentialDiagnosisText.ClientID %>').val(entity.DiagnosisText);

                $('#<%=txtClaimDiagnosisDate.ClientID %>').val(entity.ClaimDiagnosisDate);
                $('#<%=txtClaimDiagnosisTime.ClientID %>').val(entity.ClaimDiagnosisTime);
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val(entity.ClaimDiagnosisID);
                $('#<%=txtClaimDiagnosisName.ClientID %>').val(entity.ClaimDiagnosisName);
                $('#<%=txtClaimDiagnosisText.ClientID %>').val(entity.ClaimDiagnosisText);

                if (entity.FinalDate != "") {
                    $('#<%=txtFinalDate.ClientID %>').val(entity.FinalDate);
                }
                else {
                    $('#<%=txtFinalDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                }
                if (entity.FinalTime != "") {
                    $('#<%=txtFinalTime.ClientID %>').val(entity.FinalTime);
                }
                else {
                    $('#<%=txtFinalTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                }

                if (entity.FinalDiagnosisID != "") {
                    $('#<%=txtDiagnoseCode.ClientID %>').val(entity.FinalDiagnosisID);
                    $('#<%=txtDiagnoseName.ClientID %>').val(entity.FinalDiagnosisName);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val(entity.DiagnoseID);
                    $('#<%=txtDiagnoseName.ClientID %>').val(entity.DiagnoseName);
                }

                $('#<%=txtDiagnosisText.ClientID %>').val(entity.FinalDiagnosisText);

                $('#<%=txtMorphologyCode.ClientID %>').val(entity.MorphologyID);
                $('#<%=txtMorphologyName.ClientID %>').val(entity.MorphologyName);

                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', (entity.IsFollowUpCase == 'True'));
                $('#<%=chkIsChronic.ClientID %>').prop('checked', (entity.IsChronicDisease == 'True'));

                cboStatus.SetValue(entity.GCFinalStatus);
                cboStatus.SetText(entity.FinalStatus);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');

                cboDiagnoseType.SetValue('<%=GetDefaultDiagnosisType() %>');

                $('#<%=hdnPhysicianID.ClientID %>').val($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtPhysicianCode.ClientID %>').val($('#<%=hdnDefaultParamedicCode.ClientID %>').val());
                $('#<%=txtPhysicianName.ClientID %>').val($('#<%=hdnDefaultParamedicName.ClientID %>').val());

                $('#<%=txtSpecialtyName.ClientID %>').val($('#<%=hdnDefaultSpecialtyName.ClientID %>').val());

                $('#<%=txtDifferentialDate.ClientID %>').val('');
                $('#<%=txtDifferentialTime.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnoseName.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnosisText.ClientID %>').val('');

                $('#<%=txtClaimDiagnosisDate.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisTime.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisName.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisText.ClientID %>').val('');

                $('#<%=txtFinalDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                $('#<%=txtFinalTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                $('#<%=txtDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
                $('#<%=txtDiagnosisText.ClientID %>').val('');

                $('#<%=txtMorphologyCode.ClientID %>').val('');
                $('#<%=txtMorphologyName.ClientID %>').val('');
                
                var hdnIsFollowUpDefaultChecked = $('#<%=hdnIsFollowUpDefaultChecked.ClientID %>').val();
                if (hdnIsFollowUpDefaultChecked == "1") {
                    $('#<%=chkIsFollowUp.ClientID %>').prop('checked', true);
                } else {
                    $('#<%=chkIsFollowUp.ClientID %>').prop('checked', false);
                }

                $('#<%=chkIsChronic.ClientID %>').prop('checked', false);

                cboStatus.SetValue('<%=GetDefaultDifferentialDiagnosisStatus() %>');
            }
        }
        //#endregion

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            if ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val() == '1')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    $('#<%=txtSpecialtyName.ClientID %>').val(result.SpecialtyName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                    $('#<%=txtSpecialtyName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Diagnose
        $('#lblDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0 AND IsNutritionDiagnosis = 0", function (value) {
                $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                onTxtDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
            onTxtDiagnoseCodeChanged($(this).val());
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Morphology
        function onGetMorphologyFilterExpression() {
            var filterExpression = " IsDeleted = 0";
            return filterExpression;
        }

        $('#lblMorphology.lblLink').live('click', function () {
            openSearchDialog('morphology', onGetMorphologyFilterExpression(), function (value) {
                $('#<%=txtMorphologyCode.ClientID %>').val(value);
                onTxtMorphologyCodeChanged(value);
            });
        });

        $('#<%=txtMorphologyCode.ClientID %>').live('change', function () {
            onTxtMorphologyCodeChanged($(this).val());
        });

        function onTxtMorphologyCodeChanged(value) {
            var filterExpression = onGetMorphologyFilterExpression() + " AND MorphologyID = '" + value + "'";
            Methods.getObject('GetMorphologyList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtMorphologyName.ClientID %>').val(result.MorphologyName);
                }
                else {
                    $('#<%=txtMorphologyCode.ClientID %>').val('');
                    $('#<%=txtMorphologyName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitCaseType" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultSpecialtyName" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasRecord" runat="server" />
    <input type="hidden" value="0" id="hdnIsMainDiagnosisExists" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDateToday" runat="server" />
    <input type="hidden" value="" id="hdnDefaultTimeToday" runat="server" />
    <input type="hidden" value="" id="hdnIsRMDiagnoseTextEditable" runat="server" />
    <input type="hidden" value="" id="hdnIsFollowUpDefaultChecked" runat="server" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 200px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Jenis Kasus")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboVisitCaseType" ClientInstanceName="cboVisitCaseType"
                    Width="450px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tipe Diagnosa")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboDiagnoseType" ClientInstanceName="cboDiagnoseType"
                    Width="450px" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblPhysician">
                    <%=GetLabel("Dokter")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Spesialisasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtSpecialtyName" Width="100%" runat="server" ReadOnly="true" Style="text-align: left" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam Diagnosa (Dokter)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDifferentialDate" Width="120px" runat="server" ReadOnly="true"
                    Style="text-align: center" />
                <asp:TextBox ID="txtDifferentialTime" Width="60px" CssClass="time" runat="server"
                    Style="text-align: center" ReadOnly="true" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label id="lblDifferentialDiagnoseCode">
                    <%=GetLabel("Kode Diagnosa (Dokter)")%></label>
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
                            <asp:TextBox ID="txtDifferentialDiagnoseCode" Width="100px" ReadOnly="true" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDifferentialDiagnoseName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Diagnosa Text (Dokter)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDifferentialDiagnosisText" ReadOnly="true" Width="100%" runat="server" />
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
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam Diagnosa (Klaim)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtClaimDiagnosisDate" Width="120px" runat="server" ReadOnly="true"
                    Style="text-align: center" />
                <asp:TextBox ID="txtClaimDiagnosisTime" Width="60px" CssClass="time" runat="server"
                    Style="text-align: center" ReadOnly="true" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label id="Label1">
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
                            <asp:TextBox ID="txtClaimDiagnosisCode" Width="100px" ReadOnly="true" runat="server" />
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
                <asp:TextBox ID="txtClaimDiagnosisText" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam Diagnosa (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFinalDate" Width="120px" CssClass="datepicker" runat="server" />
                <asp:TextBox ID="txtFinalTime" Width="60px" CssClass="time" runat="server" Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblDiagnose">
                    <%=GetLabel("Diagnosa (RM)")%></label>
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
                            <asp:TextBox ID="txtDiagnoseCode" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Diagnosa Text (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnosisText" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblMorphology">
                    <%=GetLabel("Morfologi/Manifestasi")%></label>
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
                            <asp:TextBox ID="txtMorphologyCode" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMorphologyName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Status")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboStatus" ClientInstanceName="cboStatus" Width="450px" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsFollowUp" /><%=GetLabel(" Kasus Lama")%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsChronic" /><%=GetLabel(" Akut/Kronis")%>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnosa oleh Dokter")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
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
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnosa oleh Rekam Medis")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='<%# Eval("cfFinalDate") == "" ? "display:none;": "" %>'>
                                            <div>
                                                <%#: Eval("cfFinalDate")%>,
                                                <%#: Eval("FinalTime")%></div>
                                            <div>
                                                <span style="color: Blue; font-size: 1.1em">
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
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnosa Klaim")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
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
                                                    <%#: Eval("DiagnoseType")%></b></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("GCCaseType") %>" bindingfield="GCCaseType" />
                                        <input type="hidden" value="<%#:Eval("CaseType") %>" bindingfield="CaseType" />
                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseType") %>" bindingfield="DiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("SpecialtyName") %>" bindingfield="SpecialtyName" />
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
                                        <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                        <input type="hidden" value="<%#:Eval("MorphologyName") %>" bindingfield="MorphologyName" />
                                        <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                        <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Belum ada pengkodean diagnosa untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
