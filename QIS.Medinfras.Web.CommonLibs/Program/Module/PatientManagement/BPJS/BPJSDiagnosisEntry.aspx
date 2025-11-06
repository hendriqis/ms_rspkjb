<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="BPJSDiagnosisEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BPJSDiagnosisEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        //#region DateTimePicker
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtDifferentialDate.ClientID %>');
            $('#<%=txtDifferentialDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

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
            $('#<%=txtDifferentialDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=txtDifferentialDate.ClientID %>').val(entity.DifferentialDate);
                $('#<%=txtDifferentialTime.ClientID %>').val(entity.DifferentialTime);
                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);
                cboDiagnoseType.SetValue(entity.GCDiagnoseType);
                cboDiagnoseType.SetText(entity.DiagnoseType);
                $('#<%=txtDiagnoseCode.ClientID %>').val(entity.DiagnoseID);
                $('#<%=txtDiagnoseName.ClientID %>').val(entity.DiagnoseName);
                $('#<%=txtDiagnosisText.ClientID %>').val(entity.DiagnosisText);
                $('#<%=txtMorphologyCode.ClientID %>').val(entity.MorphologyID);
                $('#<%=txtMorphologyName.ClientID %>').val(entity.MorphologyName);
                cboStatus.SetValue(entity.GCDifferentialStatus);
                cboStatus.SetText(entity.DifferentialStatus);
                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', (entity.IsFollowUpCase == 'True'));
                $('#<%=chkIsChronic.ClientID %>').prop('checked', (entity.IsChronicDisease == 'True'));
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnPhysicianID.ClientID %>').val($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtPhysicianCode.ClientID %>').val($('#<%=hdnDefaultParamedicCode.ClientID %>').val());
                $('#<%=txtPhysicianName.ClientID %>').val($('#<%=hdnDefaultParamedicName.ClientID %>').val());
                cboDiagnoseType.SetValue('<%=GetDefaultDiagnosisType() %>');
                $('#<%=txtDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
                $('#<%=txtDiagnosisText.ClientID %>').val('');
                $('#<%=txtMorphologyCode.ClientID %>').val('');
                $('#<%=txtMorphologyName.ClientID %>').val('');
                cboStatus.SetValue('<%=GetDefaultDifferentialDiagnosisStatus() %>');
                $('#<%=chkIsFollowUp.ClientID %>').prop('checked', false);
                $('#<%=chkIsChronic.ClientID %>').prop('checked', false);
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
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion
        //#region Diagnose
        $('#lblDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
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
            var filterExpression = "DiagnoseID = '" + $('#<%=txtDiagnoseCode.ClientID %>').val() + "' AND IsDeleted = 0";
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
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory"><%=GetLabel("Tanggal")%> - <%=GetLabel("Jam")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDifferentialDate" Width="120px" CssClass="datepicker" runat="server" />
                <asp:TextBox ID="txtDifferentialTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
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
                <label class="lblLink" id="lblDiagnose">
                    <%=GetLabel("Diagnosa")%></label>
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
                <label><%=GetLabel("Diagnosa Text")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnosisText" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblMorphology">
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
                <label class="lblNormal">
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
                <asp:CheckBox runat="server" ID="chkIsFollowUp" /><%=GetLabel("Follow Up")%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox runat="server" ID="chkIsChronic" /><%=GetLabel("Kronis")%>
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
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("DifferentialDateInString")%>,
                                            <%#: Eval("DifferentialTime")%></div>
                                        <div>
                                        <div>
                                            <span style="color:Blue; font-size:1.1em"><%#: Eval("cfDiagnosisText")%></span>
                                            (<b><%#: Eval("DiagnoseID")%></b>)
                                        </div>
                                        <div><%#: Eval("ICDBlockName")%></div>
                                        <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseName") %>" bindingfield="DiagnoseName" />
                                        <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                        <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseType") %>" bindingfield="DiagnoseType" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                        <input type="hidden" value="<%#:Eval("DifferentialStatus") %>" bindingfield="DifferentialStatus" />
                                        <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                        <input type="hidden" value="<%#:Eval("MorphologyName") %>" bindingfield="MorphologyName" />
                                        <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                        <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
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
