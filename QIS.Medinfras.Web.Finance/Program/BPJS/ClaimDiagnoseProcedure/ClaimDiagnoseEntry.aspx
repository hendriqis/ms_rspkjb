<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="ClaimDiagnoseEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ClaimDiagnoseEntry" %>

<%@ Register Src="~/Program/BPJS/ClaimDiagnoseProcedure/BPJSClaimToolbarCtl.ascx"
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
            setDatePicker('<%=txtClaimDiagnosisDate.ClientID %>');
            $('#<%=txtClaimDiagnosisDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnEntryID.ClientID %>').val($(this).find('.keyField').html());
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
            $('#ctnEntry').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=txtClaimDiagnosisDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

                cboDiagnoseTypeClaim.SetValue(entity.GCDiagnoseType);
                cboDiagnoseTypeClaim.SetText(entity.DiagnoseType);

                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);

                $('#<%=txtDiagnoseType.ClientID %>').val(entity.DiagnoseType);
                $('#<%=txtDifferentialDate.ClientID %>').val(entity.DifferentialDate);
                $('#<%=txtDifferentialTime.ClientID %>').val(entity.DifferentialTime);
                $('#<%=txtDifferentialDiagnoseCode.ClientID %>').val(entity.DiagnoseID);
                $('#<%=txtDifferentialDiagnoseName.ClientID %>').val(entity.DiagnoseName);
                $('#<%=txtDifferentialDiagnosisText.ClientID %>').val(entity.DiagnosisText);
                $('#<%=txtDifferentialStatus.ClientID %>').val(entity.DifferentialStatus);

                $('#<%=txtFinalDate.ClientID %>').val('');
                $('#<%=txtFinalTime.ClientID %>').val('');
                $('#<%=txtDiagnoseCode.ClientID %>').val(entity.FinalDiagnosisID);
                $('#<%=txtDiagnoseName.ClientID %>').val(entity.FinalDiagnosisName);
                $('#<%=txtDiagnosisText.ClientID %>').val(entity.FinalDiagnosisText);
                $('#<%=txtFinalStatus.ClientID %>').val(entity.FinalStatus);

                if (entity.ClaimDiagnosisDate != "") {
                    $('#<%=txtClaimDiagnosisDate.ClientID %>').val(entity.ClaimDiagnosisDate);
                }
                else {
                    $('#<%=txtClaimDiagnosisDate.ClientID %>').val(getDateNowDatePickerFormat());
                }
                if (entity.ClaimDiagnosisTime != "") {
                    $('#<%=txtClaimDiagnosisTime.ClientID %>').val(entity.ClaimDiagnosisTime);
                }
                else {
                    $('#<%=txtClaimDiagnosisTime.ClientID %>').val(getTimeNow());
                }
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val(entity.ClaimDiagnosisID);
                $('#<%=txtClaimDiagnosisName.ClientID %>').val(entity.ClaimDiagnosisName);
                $('#<%=txtClaimDiagnosisText.ClientID %>').val(entity.ClaimDiagnosisText);

                $('#<%=txtv5DiagnosaID.ClientID %>').val(entity.ClaimDiagnosisID);
                $('#<%=txtv5DiagnosaName.ClientID %>').val(entity.ClaimDiagnosisName);
                $('#<%=txtv6DiagnosaID.ClientID %>').val(entity.ClaimINADiagnoseID);
                $('#<%=txtv6DiagnosaName.ClientID %>').val(entity.ClaimDiagnosisName);

                cboClaimStatus.SetValue(entity.ClaimStatus);
                cboClaimStatus.SetText(entity.ClaimStatus);

                $('#<%=txtMorphologyCode.ClientID %>').val(entity.MorphologyID);
                $('#<%=txtMorphologyName.ClientID %>').val(entity.MorphologyName);

                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', (entity.IsFollowUpCase == 'True'));
                $('#<%=chkIsChronic.ClientID %>').prop('checked', (entity.IsChronicDisease == 'True'));
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');

                cboDiagnoseTypeClaim.SetValue('<%=GetDefaultDiagnosisType() %>');

                $('#<%=hdnPhysicianID.ClientID %>').val($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtPhysicianCode.ClientID %>').val($('#<%=hdnDefaultParamedicCode.ClientID %>').val());
                $('#<%=txtPhysicianName.ClientID %>').val($('#<%=hdnDefaultParamedicName.ClientID %>').val());

                $('#<%=txtDiagnoseType.ClientID %>').val('');
                $('#<%=txtDifferentialDate.ClientID %>').val('');
                $('#<%=txtDifferentialTime.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnoseName.ClientID %>').val('');
                $('#<%=txtDifferentialDiagnosisText.ClientID %>').val('');
                $('#<%=txtDifferentialStatus.ClientID %>').val('');

                $('#<%=txtFinalDate.ClientID %>').val('');
                $('#<%=txtFinalTime.ClientID %>').val('');
                $('#<%=txtDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
                $('#<%=txtDiagnosisText.ClientID %>').val('');
                $('#<%=txtFinalStatus.ClientID %>').val('');

                $('#<%=txtClaimDiagnosisDate.ClientID %>').val(getDateNowDatePickerFormat());
                $('#<%=txtClaimDiagnosisTime.ClientID %>').val(getTimeNow());
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisName.ClientID %>').val('');
                $('#<%=txtClaimDiagnosisText.ClientID %>').val('');

                $('#<%=txtv5DiagnosaID.ClientID %>').val('');
                $('#<%=txtv5DiagnosaName.ClientID %>').val('');
                $('#<%=txtv6DiagnosaID.ClientID %>').val('');
                $('#<%=txtv6DiagnosaName.ClientID %>').val('');

                cboClaimStatus.SetValue('<%=GetDefaultDifferentialDiagnosisStatus() %>');

                $('#<%=txtMorphologyCode.ClientID %>').val('');
                $('#<%=txtMorphologyName.ClientID %>').val('');

                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', false);
                $('#<%=chkIsChronic.ClientID %>').prop('checked', false);
            }
        }
        //#endregion

        //#region Claim Diagnose
        $('#lblClaimDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                $('#<%=txtClaimDiagnosisCode.ClientID %>').val(value);
                ontxtClaimDiagnosisCodeChanged(value);
            });
        });

        $('#<%=txtClaimDiagnosisCode.ClientID %>').live('change', function () {
            ontxtClaimDiagnosisCodeChanged($(this).val());
        });

        function ontxtClaimDiagnosisCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtClaimDiagnosisName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=txtClaimDiagnosisText.ClientID %>').val(result.DiagnoseName);

                    $('#<%=txtv5DiagnosaID.ClientID %>').val(result.INACBGLabel);
                    $('#<%=txtv5DiagnosaName.ClientID %>').val(result.INACBGText);
                    $('#<%=txtv6DiagnosaID.ClientID %>').val(result.INACBGINALabel);
                    $('#<%=txtv6DiagnosaName.ClientID %>').val(result.INACBGINAText);
                }
                else {
                    $('#<%=txtClaimDiagnosisCode.ClientID %>').val('');
                    $('#<%=txtClaimDiagnosisName.ClientID %>').val('');
                    $('#<%=txtClaimDiagnosisText.ClientID %>').val('');

                    $('#<%=txtv5DiagnosaID.ClientID %>').val('');
                    $('#<%=txtv5DiagnosaName.ClientID %>').val('');
                    $('#<%=txtv6DiagnosaID.ClientID %>').val('');
                    $('#<%=txtv6DiagnosaName.ClientID %>').val('');
                }
            });
        }
        //#endregion
        
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();

            if (code == 'MR000007' || code == 'MR000005' || code == 'MR000026' || code == 'MR000025' || code == 'MR000021') {
                filterExpression.text = "VisitID = " + visitID;
            }
            else if (code == 'MR000017' || code == 'MR000023' || code == 'MR000039' || code == 'PM-90022' || code == 'PM-90023' || code == 'PM-90035'
                    || code == 'PM-90011') {
                filterExpression.text = visitID;
            }
            else if (code == 'PM-00103' || code == 'PM-00429' || code == 'MR000016' || code == 'PM-00523'
                    || code == 'PM-00524' || code == 'PM-00546' || code == 'PM-00565' || code == 'PM-00117'
                    || code == 'PM-00650') {
                filterExpression.text = registrationID;
            }
            else if (code == 'PM-00488' || code == 'PM-00489' || code == 'PM-002163' || code == 'PM-002136' || code == 'PM-002137' || code == 'PM-002138'
                    || code == 'PM-00282' || code == 'PM-00493' || code == 'PM-00283' || code == 'PM-00494' || code == 'PM-00284' || code == 'PM-00495' || code == 'PM-00285' || code == 'PM-00496') {
                filterExpression.text = "RegistrationID = " + registrationID;
            }
            else {
                filterExpression.text = visitID;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasRecord" runat="server" />
    <input type="hidden" value="0" id="hdnIsMainDiagnosisExists" runat="server" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 200px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="lblPhysician">
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
                            <asp:TextBox ID="txtPhysicianCode" Width="100px" ReadOnly="true" runat="server" />
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
                <label>
                    <%=GetLabel("Tipe Diagnosa (Dokter)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnoseType" ReadOnly="true" Width="100%" runat="server" />
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
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Status Diagnosa (Dokter)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDifferentialStatus" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam Diagnosa (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFinalDate" Width="120px" CssClass="datepicker" runat="server"
                    ReadOnly="true" />
                <asp:TextBox ID="txtFinalTime" Width="60px" CssClass="time" runat="server" Style="text-align: center"
                    ReadOnly="true" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="lblDiagnose">
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
                            <asp:TextBox ID="txtDiagnoseCode" Width="100px" runat="server" ReadOnly="true" />
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
                <asp:TextBox ID="txtDiagnosisText" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Status Diagnosa (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFinalStatus" Width="100%" runat="server" ReadOnly="true" />
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
                    <%=GetLabel("Tipe Diagnosa (Klaim)")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboDiagnoseTypeClaim" ClientInstanceName="cboDiagnoseTypeClaim"
                    Width="450px" />
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
                <asp:TextBox ID="txtClaimDiagnosisDate" Width="120px" runat="server" Style="text-align: center" />
                <asp:TextBox ID="txtClaimDiagnosisTime" Width="60px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblClaimDiagnose">
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
                            <asp:TextBox ID="txtClaimDiagnosisCode" Width="100px" runat="server" />
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
                <asp:TextBox ID="txtClaimDiagnosisText" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Diagnosa V5 (Klaim)")%></label>
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
                            <asp:TextBox ID="txtv5DiagnosaID" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtv5DiagnosaName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Diagnosa V6 (Klaim)")%></label>
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
                            <asp:TextBox ID="txtv6DiagnosaID" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtv6DiagnosaName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Status Diagnosa Klaim")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboClaimStatus" ClientInstanceName="cboClaimStatus"
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
                <label class="lblNormal" id="lblMorphology">
                    <%=GetLabel("Morfologi")%></label>
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
                            <asp:TextBox ID="txtMorphologyCode" Width="100px" ReadOnly="true" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMorphologyName" Width="350px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsFollowUp" Enabled="false" /><%=GetLabel("Kasus Lama")%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsChronic" Enabled="false" /><%=GetLabel("Akut/Kronis")%>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
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
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style='<%#: Eval("cfClaimDiagnosisText") == "" ?  "display:none;":""   %>'>
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
                                                                    <%#: Eval("DiagnoseTypeClaim")%></b></div>
                                                            <div>
                                                                <label>
                                                                    <%=GetLabel("----------------------------------------------------------------------------------------------------------")%>
                                                                </label>
                                                            </div>
                                                            <div>
                                                                <label style="font-style: italic">
                                                                    E-Klaim Diagnosa V5
                                                                </label>
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("cfClaimINACBGLabelName")%></b></div>
                                                            </div>
                                                            <div>
                                                                <label style="font-style: italic">
                                                                    E-Klaim Diagnosa V6
                                                                </label>
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("cfINACBGINAName")%></b></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseType") %>" bindingfield="DiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
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
                                        <input type="hidden" value="<%#:Eval("ClaimINADiagnoseID") %>" bindingfield="ClaimINADiagnoseID" />
                                        <input type="hidden" value="<%#:Eval("ClaimINADiagnoseText") %>" bindingfield="ClaimINADiagnoseText" />
                                        <input type="hidden" value="<%#:Eval("GCClaimStatus") %>" bindingfield="GCClaimStatus" />
                                        <input type="hidden" value="<%#:Eval("ClaimStatus") %>" bindingfield="ClaimStatus" />
                                        <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                        <input type="hidden" value="<%#:Eval("MorphologyName") %>" bindingfield="MorphologyName" />
                                        <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                        <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
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
