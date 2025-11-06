<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MRPatientSOAP.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MRPatientSOAP" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientSOAPSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtNoteDate.ClientID %>');
            $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtNoteDate.ClientID %>').change(function () {
                onRefresh();
            });
            $('#<%=btnPatientSOAPSave.ClientID %>').click(function () {
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onCustomButtonClick('save');
            });
        });
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
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <table style="width: 100%" class="tblEntryDetail">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal")%>
                                    -
                                    <%=GetLabel("Jam")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoteDate" TabIndex="1" Width="120px" CssClass="datepicker" runat="server" />
                                <asp:TextBox ID="txtNoteTime" TabIndex="2" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
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
                                            <asp:TextBox ID="txtPhysicianCode" TabIndex="3" Width="100px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="400px" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 99%">
                    <colgroup>
                        <col style="width: 120px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Subjective")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubjective" TabIndex="4" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="8" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Assessment")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessment" TabIndex="6" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="8" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="width: 99%">
                    <colgroup>
                        <col style="width: 120px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Objective")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObjective" TabIndex="5" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="8" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Planning")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlanning" TabIndex="7" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="8" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
