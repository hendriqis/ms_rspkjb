<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="ClaimDiagnoseProcedureList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ClaimDiagnoseProcedureList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientMedRecClaimHaveDiagListCtl.ascx"
    TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientMedRecClaimListCtl.ascx"
    TagName="ctlGrdRegisteredPatient2" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == '') {
                $('#containerBelumDiagnosa').show();
                $('#containerSudahDiagnosa').hide();
            }
            else {
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerSudahDiagnosa') {
                    $('#containerBelumDiagnosa').hide();
                    $('#containerSudahDiagnosa').show();
                }
                else {
                    $('#containerBelumDiagnosa').show();
                    $('#containerSudahDiagnosa').hide();
                }

                var name = $('#<%=hdnLastContentID.ClientID %>').val();
                $('#ulTabDiagnose li ').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('selected');
                        $('#' + tempNameContainer).hide();
                    }
                    else {
                        $(this).addClass('selected');
                        $('#' + tempNameContainer).show();
                    }
                });
            }

            $('#ulTabDiagnose li').click(function () {
                $('#ulTabDiagnose li.selected').removeAttr('class');
                $('.containerDiagnosa').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnLastContentID.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            setDatePicker('<%=txtFromRegistrationDate2.ClientID %>');
            setDatePicker('<%=txtToRegistrationDate2.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtFromRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtToRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#btnRefresh').live('click', function (evt) {
                onRefreshGridView();
            });

            $('#btnRefresh2').live('click', function (evt) {
                onRefreshGridView2();
            });

            //#region Service Unit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "'";
                return filterExpression;
            }

            function getHealthcareServiceUnitFilterExpression2() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom2.GetValue() + "'";
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#lblServiceUnit2.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression2(), function (value) {
                    $('#<%=txtServiceUnitCode2.ClientID %>').val(value);
                    onTxtServiceUnitCode2Changed(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            $('#<%=txtServiceUnitCode2.ClientID %>').change(function () {
                onTxtServiceUnitCode2Changed($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }

            function onTxtServiceUnitCode2Changed(value) {
                var filterExpression2 = getHealthcareServiceUnitFilterExpression2() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression2, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID2.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode2.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName2.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID2.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode2.ClientID %>').val('');
                        $('#<%=txtServiceUnitName2.ClientID %>').val('');
                    }
                });
            }
            //#endregion

        });

        function onCboPatientFromValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
        }

        function onCboPatientFrom2ValueChanged() {
            $('#<%=hdnServiceUnitID2.ClientID %>').val('');
            $('#<%=txtServiceUnitCode2.ClientID %>').val('');
            $('#<%=txtServiceUnitName2.ClientID %>').val('');
        }

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                refreshGrdRegisteredPatientHaveDiagnose();
            }
        }

        function onRefreshGridView2() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                refreshGrdRegisteredPatient();
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        function onTxtSearchView2SearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch2.ClientID %>').val(txtSearchView2.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView2();
            }, 0);
        }

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch2" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnQuickText2" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnLastContentID" runat="server" value="" />
    <div style="padding: 15px">
        <div class="containerUlTabPage">
            <ul class="ulTabPage" id="ulTabDiagnose">
                <li class="selected" contentid="containerBelumDiagnosa">
                    <%=GetLabel("Belum Diagnosa Klaim")%></li>
                <li contentid="containerSudahDiagnosa">
                    <%=GetLabel("Sudah Diagnosa Klaim")%></li>
            </ul>
        </div>
        <div style="padding: 2px; display: none" id="containerSudahDiagnosa" class="containerDiagnosa">
            <div class="pageTitle">
                <%=GetLabel("Pasien Sudah Diagnosa Klaim")%></div>
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 50%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 25px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromRegistrationDate" Width="80%" CssClass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <%=GetLabel("s/d") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToRegistrationDate" Width="80%" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                        Width="90%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                    <label class="lblLink lblNormal" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="4">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="90%" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                        Width="90%" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" style="width: 90%" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large"
                                        value='<%= GetLabel("R e f r e s h")%>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:ctlGrdRegisteredPatient runat="server" ID="grdRegisteredPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 2px;" id="containerBelumDiagnosa" class="containerDiagnosa">
            <div class="pageTitle">
                <%=GetLabel("Pasien Belum Diagnosa Klaim")%></div>
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 80%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 50%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 25px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromRegistrationDate2" Width="80%" CssClass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <%=GetLabel("s/d") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToRegistrationDate2" Width="80%" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboPatientFrom2" ClientInstanceName="cboPatientFrom2" runat="server"
                                        Width="90%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFrom2ValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <input type="hidden" id="hdnServiceUnitID2" value="" runat="server" />
                                    <label class="lblLink lblNormal" id="lblServiceUnit2">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtServiceUnitCode2" Width="100%" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtServiceUnitName2" ReadOnly="true" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td colspan="4">
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView2" ID="txtSearchView2"
                                        Width="90%" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchView2SearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboRegistrationStatus2" ClientInstanceName="cboRegistrationStatus2"
                                        Width="90%" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh2" style="width: 90%" class="btnRefresh2 w3-button w3-blue w3-border w3-border-blue w3-round-large"
                                        value='<%= GetLabel("R e f r e s h")%>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:ctlGrdRegisteredPatient2 runat="server" ID="grdRegisteredPatient2" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
            txtSearchView2.SetText($('#<%=hdnQuickText2.ClientID %>').val());
        });
    </script>
</asp:Content>
