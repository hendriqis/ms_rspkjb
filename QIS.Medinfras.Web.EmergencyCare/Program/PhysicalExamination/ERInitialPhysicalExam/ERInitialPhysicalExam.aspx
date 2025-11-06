<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ERInitialPhysicalExam.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.ERInitialPhysicalExam" %>

<%@ Register Src="~/Libs/Program/Module/PhysicalExamination/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnInitialPhysicalExamSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erinitialphysicalexam">
        $(function () {
            setDatePicker('<%=txtObservationDate.ClientID %>');
            $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });
        $('#<%=btnInitialPhysicalExamSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsInitialPhysicalExam', 'mpInitialPhysicalExam'))
                onCustomButtonClick('save');
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
    <style type="text/css">
        #ulVitalSign
        {
            margin: 0;
            padding: 0;
        }
        #ulVitalSign li
        {
            list-style-type: none;
            display: inline-block;
            margin-right: 10px;
        }
    </style>
    <div style="height: 310px; overflow-y: scroll;">
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
        
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                <fieldset id="fsInitialPhysicalExam">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Waktu Pemeriksaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservationDate" Width="100px" CssClass="datepicker" runat="server" />
                                <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPhysician">
                                    <%=GetLabel("Tenaga Profesional")%></label>
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
                                            <asp:TextBox ID="txtPhysicianName" Width="300px" ReadOnly="true" runat="server" />
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
                <td style="padding-left:9px">
                    <asp:Repeater ID="rptVitalSign" runat="server" OnItemDataBound="rptVitalSign_ItemDataBound">
                        <HeaderTemplate>
                            <ul id="ulVitalSign">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 151px" />
                                        <col style="width: 200px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnVitalSignID" runat="server" value='<%#:Eval("VitalSignID") %>' />
                                            <input type="hidden" id="hdnVitalSignType" runat="server" value='<%#:Eval("GCValueType") %>' />
                                            <%#:Eval("VitalSignLabel") %>
                                        </td>
                                        <td>
                                            <div id="divTxt" runat="server" visible="false">
                                                <asp:TextBox ID="txtVitalSignType" Width="100px" runat="server" Style="float: left" />
                                                &nbsp;<%#:Eval("ValueUnit") %>
                                            </div>
                                            <div id="divDdl" runat="server" visible="false">
                                                <dxe:ASPxComboBox runat="server" ID="cboVitalSignType" ClientInstanceName="cboVitalSignType" Width="200px" />
                                            </div>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
