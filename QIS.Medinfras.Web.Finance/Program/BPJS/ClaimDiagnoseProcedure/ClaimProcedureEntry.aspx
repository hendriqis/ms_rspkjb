<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="ClaimProcedureEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ClaimProcedureEntry" %>

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
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtClaimProcedureDate.ClientID %>');
            $('#<%=txtClaimProcedureDate.ClientID %>').datepicker('option', 'maxDate', '0');
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

        //#region Procedures
        $('#lblClaimProcedure.lblLink').live('click', function () {
            openSearchDialog('procedures', "IsDeleted = 0", function (value) {
                $('#<%=txtClaimProcedureCode.ClientID %>').val(value);
                onTxtClaimProcedureCodeChanged(value);
            });
        });

        $('#<%=txtClaimProcedureCode.ClientID %>').live('change', function () {
            onTxtClaimProcedureCodeChanged($(this).val());
        });

        function onTxtClaimProcedureCodeChanged(value) {
            var filterExpression = "ProcedureID = '" + value + "'";
            Methods.getObject('GetvProceduresList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtClaimProcedureName.ClientID %>').val(result.ProcedureName);

                    $('#<%=txtProcedurev5Code.ClientID %>').val(result.INACBGLabel);
                    $('#<%=txtProcedurev5Name.ClientID %>').val(result.INACBGText);
                    $('#<%=txtProcedurev6Code.ClientID %>').val(result.INACBGINALabel);
                    $('#<%=txtProcedurev6Name.ClientID %>').val(result.INACBGINALabelName);
                }
                else {
                    $('#<%=hdnClaimProcedureID.ClientID %>').val('');
                    $('#<%=txtClaimProcedureCode.ClientID %>').val('');
                    $('#<%=txtClaimProcedureName.ClientID %>').val('');
                    $('#<%=txtProcedurev5Code.ClientID %>').val("");
                    $('#<%=txtProcedurev5Name.ClientID %>').val("");
                    $('#<%=txtProcedurev6Code.ClientID %>').val("");
                    $('#<%=txtProcedurev6Name.ClientID %>').val("");
                }
            });
        }
        //#endregion

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=txtClaimProcedureDate.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');

                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);

                $('#<%=txtFinalProcedureDate.ClientID %>').val(entity.FinalProcedureDate);
                $('#<%=txtFinalProcedureTime.ClientID %>').val(entity.FinalProcedureTime);
                $('#<%=txtFinalProcedureCode.ClientID %>').val(entity.FinalProcedureID);
                $('#<%=txtFinalProcedureName.ClientID %>').val(entity.FinalProcedureName);
                $('#<%=txtFinalProcedureText.ClientID %>').val(entity.FinalProcedureText);

                if (entity.ClaimProcedureDate != "") {
                    $('#<%=txtClaimProcedureDate.ClientID %>').val(entity.ClaimProcedureDate);
                } else {
                    $('#<%=txtClaimProcedureDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                }
                if (entity.ClaimProcedureTime != "") {
                    $('#<%=txtClaimProcedureTime.ClientID %>').val(entity.ClaimProcedureTime);
                } else {
                    $('#<%=txtClaimProcedureTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                }
                $('#<%=txtClaimProcedureCode.ClientID %>').val(entity.ClaimProcedureID);
                $('#<%=txtClaimProcedureName.ClientID %>').val(entity.ClaimProcedureName);
                $('#<%=txtClaimProcedureText.ClientID %>').val(entity.ClaimProcedureText);
                $('#<%=txtProcedurev5Code.ClientID %>').val(entity.ClaimINAProcedureID);
                $('#<%=txtProcedurev5Name.ClientID %>').val(entity.ClaimProcedureName);
                $('#<%=txtProcedurev6Code.ClientID %>').val(entity.ClaimINACBGINAProcedureID);
                $('#<%=txtProcedurev6Name.ClientID %>').val(entity.ClaimProcedureName);

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

                $('#<%=txtFinalProcedureDate.ClientID %>').val('');
                $('#<%=txtFinalProcedureTime.ClientID %>').val('');
                $('#<%=txtFinalProcedureCode.ClientID %>').val('');
                $('#<%=txtFinalProcedureName.ClientID %>').val('');
                $('#<%=txtFinalProcedureText.ClientID %>').val('');

                $('#<%=txtClaimProcedureDate.ClientID %>').val($('#<%=hdnDefaultDateToday.ClientID %>').val());
                $('#<%=txtClaimProcedureTime.ClientID %>').val($('#<%=hdnDefaultTimeToday.ClientID %>').val());
                $('#<%=txtClaimProcedureCode.ClientID %>').val('');
                $('#<%=txtClaimProcedureName.ClientID %>').val('');
                $('#<%=txtClaimProcedureText.ClientID %>').val('');
                $('#<%=txtProcedurev5Code.ClientID %>').val("");
                $('#<%=txtProcedurev5Name.ClientID %>').val("");
                $('#<%=txtProcedurev6Code.ClientID %>').val("");
                $('#<%=txtProcedurev6Name.ClientID %>').val("");

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
                <label class="lblNormal" id="lblPhysician">
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
                            <asp:TextBox ID="txtPhysicianCode" Width="100px" ReadOnly="true" runat="server" />
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
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal")%>
                    -
                    <%=GetLabel("Jam (RM)")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFinalProcedureDate" Width="120px" CssClass="datepicker" runat="server"
                    Style="text-align: center" ReadOnly="true" />
                <asp:TextBox ID="txtFinalProcedureTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" ReadOnly="true" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="lblFinalProcedure">
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
                            <asp:TextBox ID="txtFinalProcedureCode" Width="100px" ReadOnly="true" runat="server" />
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
                            <asp:TextBox ID="txtFinalProcedureText" Width="100%" ReadOnly="true" runat="server" />
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
                    Style="text-align: center" />
                <asp:TextBox ID="txtClaimProcedureTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblClaimProcedure">
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
                            <input type="hidden" value="" id="hdnClaimProcedureID" runat="server" />
                            <asp:TextBox ID="txtClaimProcedureCode" Width="100px" runat="server" />
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
                            <asp:TextBox ID="txtClaimProcedureText" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan (E-Klaim V5)")%></label>
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
                            <asp:TextBox ID="txtProcedurev5Code" ReadOnly="true" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcedurev5Name" Width="300px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prosedur/Tindakan (E-Klaim V6)")%></label>
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
                            <asp:TextBox ID="txtProcedurev6Code" ReadOnly="true" Width="100px" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcedurev6Name" Width="300px" ReadOnly="true" runat="server" />
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
            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                <label class="lblNormal">
                    <%=GetLabel("Catatan")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRemarks" Width="400px" runat="server" TextMode="MultiLine" ReadOnly="true" />
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
                                        <input type="hidden" value="<%#:Eval("ClaimINAProcedureID") %>" bindingfield="ClaimINAProcedureID" />
                                        <input type="hidden" value="<%#:Eval("ClaimINACBGINAProcedureID") %>" bindingfield="ClaimINACBGINAProcedureID" />
                                        <input type="hidden" value="<%#:Eval("cfClaimProcedureDateInDatePickerFormat") %>"
                                            bindingfield="ClaimProcedureDate" />
                                        <input type="hidden" value="<%#:Eval("ClaimProcedureTime") %>" bindingfield="ClaimProcedureTime" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                        <input type="hidden" value="<%#:Eval("IsCreatedBySystem") %>" bindingfield="IsCreatedBySystem" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                    <HeaderTemplate>
                                        <%=GetLabel("Prosedur Tindakan") %>
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
                                            <div style="float: left; width: 300px;">
                                                <%#: Eval("ParamedicName")%></div>
                                            <div style="clear: both; font-weight: bold">
                                                <%#: Eval("ClaimProcedureName")%>
                                                (<%#: Eval("ClaimProcedureID")%>)</div>
                                        </div>
                                        <div>
                                            <label>
                                                <%=GetLabel("----------------------------------------------------------------------------------------------------------")%>
                                            </label>
                                        </div>
                                        <div>
                                            <label style="font-style: italic">
                                                E-Klaim Prosedur/Tindakan V5
                                            </label>
                                            <div>
                                                <b>
                                                    <%#: Eval("cfClaimINAProcedureName")%></b></div>
                                            <label style="font-style: italic">
                                                E-Klaim Prosedur/Tindakan V6
                                            </label>
                                            <div>
                                                <b>
                                                    <%#: Eval("cfClaimINACBGINAProcedureName")%></b></div>
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
