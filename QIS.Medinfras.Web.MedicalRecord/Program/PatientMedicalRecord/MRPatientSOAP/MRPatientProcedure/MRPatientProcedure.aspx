<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="MRPatientProcedure.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MRPatientProcedure" %>

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
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtFinalProcedureDate.ClientID %>');
            $('#<%=txtFinalProcedureDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

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

        //#region Physician
        $('#lblPhysician.lblLink').live('click', function () {
            var filterExpression = "";
            if ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val() == '1')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "')";
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = "ParamedicCode = '" + value + "'";
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

        //#region Procedures
        $('#lblFinalProcedure.lblLink').live('click', function () {
            openSearchDialog('procedures', "IsDeleted = 0", function (value) {
                $('#<%=txtFinalProcedureCode.ClientID %>').val(value);
                onTxtFinalProcedureCodeChanged(value);
            });
        });

        $('#<%=txtFinalProcedureCode.ClientID %>').live('change', function () {
            onTxtFinalProcedureCodeChanged($(this).val());
        });

        function onTxtFinalProcedureCodeChanged(value) {
            var filterExpression = "ProcedureID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetProceduresList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtFinalProcedureName.ClientID %>').val(result.ProcedureName);
                }
                else {
                    $('#<%=hdnFinalProcedureID.ClientID %>').val('');
                    $('#<%=txtFinalProcedureCode.ClientID %>').val('');
                    $('#<%=txtFinalProcedureName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=txtFinalProcedureDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);

                if (entity.FinalProcedureDate != "") {
                    $('#<%=txtFinalProcedureDate.ClientID %>').val(entity.FinalProcedureDate);
                }
                else {
                    $('#<%=txtFinalProcedureDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                }
                if (entity.FinalProcedureTime != "") {
                    $('#<%=txtFinalProcedureTime.ClientID %>').val(entity.FinalProcedureTime);
                }
                else {
                    $('#<%=txtFinalProcedureTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                }
                $('#<%=txtFinalProcedureCode.ClientID %>').val(entity.FinalProcedureID);
                $('#<%=txtFinalProcedureName.ClientID %>').val(entity.FinalProcedureName);
                $('#<%=txtFinalProcedureText.ClientID %>').val(entity.FinalProcedureText);

                $('#<%=txtClaimProcedureDate.ClientID %>').val(entity.ClaimProcedureDate);
                $('#<%=txtClaimProcedureTime.ClientID %>').val(entity.ClaimProcedureTime);
                $('#<%=txtClaimProcedureCode.ClientID %>').val(entity.ClaimProcedureID);
                $('#<%=txtClaimProcedureName.ClientID %>').val(entity.ClaimProcedureName);
                $('#<%=txtClaimProcedureText.ClientID %>').val(entity.ClaimProcedureText);

                $('#<%=txtObservationDate.ClientID %>').val(entity.ProcedureDate);
                $('#<%=txtObservationTime.ClientID %>').val(entity.ProcedureTime);
                $('#<%=txtProcedureCode.ClientID %>').val(entity.ProcedureID);
                $('#<%=txtProcedureName.ClientID %>').val(entity.ProcedureName);
                $('#<%=txtProcedureText.ClientID %>').val(entity.ProcedureText);

                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnPhysicianID.ClientID %>').val($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtPhysicianCode.ClientID %>').val($('#<%=hdnDefaultParamedicCode.ClientID %>').val());
                $('#<%=txtPhysicianName.ClientID %>').val($('#<%=hdnDefaultParamedicName.ClientID %>').val());

                $('#<%=txtFinalProcedureDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                $('#<%=txtFinalProcedureTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                $('#<%=txtFinalProcedureCode.ClientID %>').val('');
                $('#<%=txtFinalProcedureName.ClientID %>').val('');
                $('#<%=txtFinalProcedureText.ClientID %>').val('');

                $('#<%=txtClaimProcedureDate.ClientID %>').val('');
                $('#<%=txtClaimProcedureTime.ClientID %>').val('');
                $('#<%=txtClaimProcedureCode.ClientID %>').val('');
                $('#<%=txtClaimProcedureName.ClientID %>').val('');
                $('#<%=txtClaimProcedureText.ClientID %>').val('');

                $('#<%=txtObservationDate.ClientID %>').val('');
                $('#<%=txtObservationTime.ClientID %>').val('');
                $('#<%=txtProcedureCode.ClientID %>').val('');
                $('#<%=txtProcedureName.ClientID %>').val('');
                $('#<%=txtProcedureText.ClientID %>').val('');

                $('#<%=txtRemarks.ClientID %>').val('');
            }
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDateToday" runat="server" />
    <input type="hidden" value="" id="hdnDefaultTimeToday" runat="server" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 230px" />
            <col style="width: 150px" />
        </colgroup>
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
                            <asp:TextBox ID="txtPhysicianName" Width="300px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam (Dokter)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server"
                    ReadOnly="true" Style="text-align: center" />
                <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                    ReadOnly="true" Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan (Dokter)")%></label>
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
                            <asp:TextBox ID="txtProcedureCode" Width="100px" ReadOnly="true" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcedureName" Width="300px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan Text (Dokter)")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtProcedureText" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
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
                    <%=GetLabel("Jam (Klaim)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtClaimProcedureDate" Width="120px" CssClass="datepicker" runat="server"
                    ReadOnly="true" Style="text-align: center" />
                <asp:TextBox ID="txtClaimProcedureTime" Width="80px" CssClass="time" runat="server"
                    ReadOnly="true" Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan (Klaim)")%></label>
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
                            <asp:TextBox ID="txtClaimProcedureCode" Width="100px" ReadOnly="true" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimProcedureName" Width="300px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan Text (Klaim)")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtClaimProcedureText" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
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
                    <%=GetLabel("Jam (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFinalProcedureDate" Width="120px" CssClass="datepicker" runat="server"
                    Style="text-align: center" />
                <asp:TextBox ID="txtFinalProcedureTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblFinalProcedure">
                    <%=GetLabel("Prosedur/Tindakan (RM)")%></label>
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
                            <input type="hidden" value="" id="hdnFinalProcedureID" runat="server" />
                            <asp:TextBox ID="txtFinalProcedureCode" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFinalProcedureName" Width="300px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan Text (RM)")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtFinalProcedureText" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                <label class="lblNormal">
                    <%=GetLabel("Catatan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" Width="400px" runat="server" TextMode="MultiLine" />
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
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                        <input type="hidden" value="<%#:Eval("ProcedureName") %>" bindingfield="ProcedureName" />
                                        <input type="hidden" value="<%#:Eval("ProcedureText") %>" bindingfield="ProcedureText" />
                                        <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                        <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                        <input type="hidden" value="<%#:Eval("FinalProcedureID") %>" bindingfield="FinalProcedureID" />
                                        <input type="hidden" value="<%#:Eval("FinalProcedureName") %>" bindingfield="FinalProcedureName" />
                                        <input type="hidden" value="<%#:Eval("FinalProcedureText") %>" bindingfield="FinalProcedureText" />
                                        <input type="hidden" value="<%#:Eval("cfFinalProcedureDateInDatePickerFormat") %>"
                                            bindingfield="FinalProcedureDate" />
                                        <input type="hidden" value="<%#:Eval("FinalProcedureTime") %>" bindingfield="FinalProcedureTime" />
                                        <input type="hidden" value="<%#:Eval("ClaimProcedureID") %>" bindingfield="ClaimProcedureID" />
                                        <input type="hidden" value="<%#:Eval("ClaimProcedureName") %>" bindingfield="ClaimProcedureName" />
                                        <input type="hidden" value="<%#:Eval("ClaimProcedureText") %>" bindingfield="ClaimProcedureText" />
                                        <input type="hidden" value="<%#:Eval("cfClaimProcedureDateInDatePickerFormat") %>"
                                            bindingfield="ClaimProcedureDate" />
                                        <input type="hidden" value="<%#:Eval("ClaimProcedureTime") %>" bindingfield="ClaimProcedureTime" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                        <input type="hidden" value="<%#:Eval("IsCreatedBySystem") %>" bindingfield="IsCreatedBySystem" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Prosedur/Tindakan oleh Dokter") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='<%# Eval("ProcedureDateInString") == "" ? "display:none;": "" %>'>
                                            <div style="float: left; width: 120px;">
                                                <%#: Eval("ProcedureDateInString")%>,
                                                <%#: Eval("ProcedureTime")%></div>
                                            <div style="float: left; width: 300px;">
                                                <%#: Eval("ParamedicName")%></div>
                                            <div style="clear: both; font-weight: bold">
                                                <%#: Eval("cfProcedureText")%>
                                                (<%#: Eval("ProcedureID")%>)</div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Prosedur/Tindakan oleh Rekam Medis") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='<%# Eval("cfFinalProcedureDateInString") == "" ? "display:none;": "" %>'>
                                            <div style="float: left; width: 120px;">
                                                <%#: Eval("cfFinalProcedureDateInString")%>,
                                                <%#: Eval("FinalProcedureTime")%></div>
                                            <div style="clear: both; font-weight: bold">
                                                <%#: Eval("FinalProcedureName")%>
                                                (<%#: Eval("FinalProcedureID")%>)</div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Prosedur/Tindakan Klaim") %>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='<%# Eval("cfClaimProcedureDateInString") == "" ? "display:none;": "" %>'>
                                            <div style="float: left; width: 120px;">
                                                <%#: Eval("cfClaimProcedureDateInString")%>,
                                                <%#: Eval("ClaimProcedureTime")%></div>
                                            <div style="clear: both; font-weight: bold">
                                                <%#: Eval("ClaimProcedureName")%>
                                                (<%#: Eval("ClaimProcedureID")%>)</div>
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
