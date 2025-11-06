<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true" 
    CodeBehind="TransactionPageVisitOrderTemp.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageVisitOrderTemp" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />   
    <input type="hidden" value="" id="hdnClassID" runat="server" /> 
    <input type="hidden" value="" id="hdnCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" /> 
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script>
        function OnLoad() {

        }

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

        //#region Physician
        function onGetPatientVisitParamedicFilterExpression() {
            var polyclinicID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = 'IsDeleted = 0';
            if (polyclinicID != '')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
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
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    cboSpecialty.SetValue(result.SpecialtyID);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                    cboSpecialty.SetValue('');
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

        $('#lblVisitType.lblLink').live('click', function () {
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
    </script>

    <div style="height:435px;overflow-y:auto;overflow-x:hidden;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Order Poli Lain")%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:150px"/>
                <col style="width:120px"/>
            </colgroup>
            <tr>
                <td><label class="lblMandatory lblLink" id="lblPatientVisitClinic"><%=GetLabel("Klinik") %></label></td>
                <td>
                    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
                    <asp:TextBox runat="server" ID="txtClinicCode" Width="100%" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtClinicName" ReadOnly="true" Width="200px" />
                </td>
            </tr>
            <tr>
                <td><label class="lblMandatory lblLink" id="lblPatientVisitPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
                <td>
                    <input type="hidden" runat="server" id="hdnPhysicianID" />
                    <asp:TextBox runat="server" ID="txtPhysicianCode" Width="100%" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPhysicianName" Width="200px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td><label class="lblNormal"><%=GetLabel("Spesialisasi") %></label></td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboSpecialty" ClientInstanceName="cboSpecialty" Width="320px" />
                </td>
            </tr>
            <tr>
                <td><label class="lblMandatory lblLink" id="lblVisitType"><%=GetLabel("Jenis Kunjungan") %></label></td>
                <td>
                    <input type="hidden" id="hdnVisitTypeID" runat="server" />
                    <asp:TextBox runat="server" ID="txtVisitTypeCode" Width="100%" />
                </td>
                <td><asp:TextBox runat="server" ID="txtVisitTypeName" Width="200px" ReadOnly="true" /></td>
            </tr>
            <tr>
                <td><label class="lblNormal"><%=GetLabel("Alasan Kunjungan") %></label></td>
                <td colspan="2"><asp:TextBox runat="server" ID="txtVisitReason" Width="320" /></td>
            </tr>
        </table>
    </div>
</asp:Content>