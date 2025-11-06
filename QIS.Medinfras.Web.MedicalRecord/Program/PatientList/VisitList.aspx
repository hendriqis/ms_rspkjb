<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="VisitList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.VisitList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientMedicalRecordHaveDiagCtl.ascx"
    TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientMedicalRecordCtl.ascx" TagName="ctlGrdRegisteredPatient2"
    TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientMedicalRecordCodificationCtl.ascx"
    TagName="ctlGrdRegisteredPatient3" TagPrefix="uc1" %>
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
        var lastContentID = '';

        $(function () {
            if ($('#<%=hdnLastContentID.ClientID %>').val() == '') {
                $('#containerBelumDiagnosa').show();
                $('#containerSudahDiagnosa').hide();
                $('#containerTidakPerluKodefikasi').hide();
            }
            else {
                var lastPage = $('#<%=hdnLastPage.ClientID %>').val();
                if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerSudahDiagnosa') {
                    $('#containerBelumDiagnosa').hide();
                    $('#containerSudahDiagnosa').show();
                    $('#containerTidakPerluKodefikasi').hide();

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
                else if ($('#<%=hdnLastContentID.ClientID %>').val() == 'containerBelumDiagnosa') {
                    $('#containerBelumDiagnosa').show();
                    $('#containerSudahDiagnosa').hide();
                    $('#containerTidakPerluKodefikasi').hide();
                }
                else {
                    $('#containerBelumDiagnosa').hide();
                    $('#containerSudahDiagnosa').hide();
                    $('#containerTidakPerluKodefikasi').show();

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
            }

            $('#ulTabDiagnose li').click(function () {
                var name = $(this).attr('contentid');
                $(this).addClass('selected');
                lastContentID = $('#<%=hdnLastContentID.ClientID %>').val(name);
                $('#ulTabDiagnose li ').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('selected');
                        $('#' + tempNameContainer).hide();
                    }
                    else {
                        $('#' + tempNameContainer).show();
                    }
                });
            });

            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            setDatePicker('<%=txtFromRegistrationDate2.ClientID %>');
            setDatePicker('<%=txtToRegistrationDate2.ClientID %>');
            setDatePicker('<%=txtFromRegistrationDate3.ClientID %>');
            setDatePicker('<%=txtToRegistrationDate3.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtFromRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtFromRegistrationDate3.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToRegistrationDate3.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#btnRefresh').click(function (evt) {
                onRefreshGridView();
            });

            $('#btnRefresh2').click(function (evt) {
                onRefreshGridView2();
            });

            $('#btnRefresh3').click(function (evt) {
                onRefreshGridView3();
            });

            //#region Download
            $('#btnDownload').click(function (evt) {
                __doPostBack('<%=btnDownloadProcess.UniqueID%>', '');
            });

            $('#btnDownload2').click(function (evt) {
                __doPostBack('<%=btnDownloadProcess2.UniqueID%>', '');
            });

            $('#btnDownload3').click(function (evt) {
                __doPostBack('<%=btnDownloadProcess3.UniqueID%>', '');
            });
            //#endregion

            //#region Service Unit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "' AND IsUseDiagnosisCodingProcess = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            function getHealthcareServiceUnitFilterExpression2() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom2.GetValue() + "' AND IsUseDiagnosisCodingProcess = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            function getHealthcareServiceUnitFilterExpression3() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom3.GetValue() + "' AND IsUseDiagnosisCodingProcess = 1 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare1', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#lblServiceUnit2.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare1', getHealthcareServiceUnitFilterExpression2(), function (value) {
                    $('#<%=txtServiceUnitCode2.ClientID %>').val(value);
                    onTxtServiceUnitCode2Changed(value);
                });
            });

            $('#lblServiceUnit3.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare1', getHealthcareServiceUnitFilterExpression3(), function (value) {
                    $('#<%=txtServiceUnitCode3.ClientID %>').val(value);
                    onTxtServiceUnitCode3Changed(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            $('#<%=txtServiceUnitCode2.ClientID %>').change(function () {
                onTxtServiceUnitCode2Changed($(this).val());
            });

            $('#<%=txtServiceUnitCode3.ClientID %>').change(function () {
                onTxtServiceUnitCode3Changed($(this).val());
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

            function onTxtServiceUnitCode3Changed(value) {
                var filterExpression3 = getHealthcareServiceUnitFilterExpression3() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression3, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID3.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode3.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName3.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID3.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode3.ClientID %>').val('');
                        $('#<%=txtServiceUnitName3.ClientID %>').val('');
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

        function onCboPatientFrom3ValueChanged() {
            $('#<%=hdnServiceUnitID3.ClientID %>').val('');
            $('#<%=txtServiceUnitCode3.ClientID %>').val('');
            $('#<%=txtServiceUnitName3.ClientID %>').val('');
        }

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                refreshGrdRegisteredPatientHaveDiagnose();
        }

        function onRefreshGridView2() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                refreshGrdRegisteredPatient();
        }

        function onRefreshGridView3() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                refreshGrdRegisteredPatientCodification();
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

        function onTxtSearchView3SearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch3.ClientID %>').val(txtSearchView3.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView3();
            }, 0);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var logDate = Methods.DatePickerToDateFormat($('#<%=txtFromRegistrationDate.ClientID %>').val());
            if (logDate == '' || logDate == '0') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else {
                filterExpression.text = "ActualVisitDate = '" + logDate + "'";
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch2" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch3" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnQuickText2" runat="server" />
    <input type="hidden" value="" id="hdnQuickText3" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnLastContentID" runat="server" value="" />
    <input type="hidden" id="hdnLastPage" runat="server" value="" />
    <div style="padding: 15px">
        <div class="containerUlTabPage">
            <ul class="ulTabPage" id="ulTabDiagnose">
                <li class="selected" contentid="containerBelumDiagnosa">
                    <%=GetLabel("Belum Diagnosa RM")%></li>
                <li contentid="containerSudahDiagnosa">
                    <%=GetLabel("Sudah Diagnosa RM")%></li>
                <li contentid="containerTidakPerluKodefikasi">
                    <%=GetLabel("Tidak Perlu Kodefikasi")%></li>
            </ul>
        </div>
        <div style="padding: 2px; display; none" id="containerSudahDiagnosa" class="containerDiagnosa">
            <div class="pageTitle">
                <%=GetLabel("Registrasi Pasien yang Sudah Diagnosa oleh Rekam Medis")%></div>
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 60%" />
                    <col style="width: 40%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 80%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Tanggal")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDateFilter" ClientInstanceName="cboDateFilter" Width="150px"
                                        runat="server" BackColor="Pink">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromRegistrationDate" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                            <td>
                                                <%=GetLabel("s/d") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToRegistrationDate" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                        Width="100%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="99%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="435px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="Dokter" FieldName="ParamedicName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                                    Width="150px" runat="server">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsControlDocument" runat="server" Checked="false" /><%:GetLabel(" Status Berkas : Diproses")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tidak Lengkap")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1000px">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkAllEmpty" runat="server" Checked="false" /><%:GetLabel(" All Empty")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkChiefComplaint" runat="server" Checked="false" /><%:GetLabel(" Chief Complaint")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPatientDiagnosis" runat="server" Checked="false" /><%:GetLabel(" Physician Diagnose")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPhysicianDischarge" runat="server" Checked="false" /><%:GetLabel(" Physician Discharge")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDischarge" runat="server" Checked="false" /><%:GetLabel(" Discharge")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tampilan Hasil")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="150px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kondisi Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientCondition" ClientInstanceName="cboPatientCondition"
                                        Width="150px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnDownload" value="D o w n l o a d" class="btnDownload w3-button w3-teal w3-border w3-border-blue w3-round-large" />
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 30px" />
                                            <col style="width: 10px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--<td align="center" style="vertical-align: top">
                        <div style="border-color: Red; border-style: solid; border-width: 1px; padding: 5px;">
                            <font color="red"><b>I N F O R M A S I</b></font><br />
                            Sudah Diagnosa di sini adalah jika bagian <b><u>Rekam Medis</u></b> sudah mengisi
                            Diagnosa.
                        </div>
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:ctlGrdRegisteredPatient runat="server" ID="grdRegisteredPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 2px;" id="containerBelumDiagnosa" class="containerDiagnosa">
            <div class="pageTitle">
                <%=GetLabel("Registrasi Pasien yang Belum Diagnosa oleh Rekam Medis")%></div>
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 60%" />
                    <col style="width: 40%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 80%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Tanggal")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDateFilter2" ClientInstanceName="cboDateFilter2" Width="150px"
                                        runat="server" BackColor="Pink">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromRegistrationDate2" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                            <td>
                                                <%=GetLabel("s/d") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToRegistrationDate2" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom2" ClientInstanceName="cboPatientFrom2" runat="server"
                                        Width="100%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFrom2ValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblServiceUnit2">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID2" value="0" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode2" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName2" ReadOnly="true" Width="99%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView2" ID="txtSearchView2"
                                        Width="435px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchView2SearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="Dokter" FieldName="ParamedicName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationStatus2" ClientInstanceName="cboRegistrationStatus2"
                                                    Width="150px" runat="server">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsControlDocument2" runat="server" Checked="false" /><%:GetLabel(" Status Berkas : Diproses")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tidak Lengkap")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1000px">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkAllEmpty2" runat="server" Checked="false" /><%:GetLabel(" All Empty")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkChiefComplaint2" runat="server" Checked="false" /><%:GetLabel(" Chief Complaint")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPatientDiagnosis2" runat="server" Checked="false" /><%:GetLabel(" Physician Diagnose")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPhysicianDischarge2" runat="server" Checked="false" /><%:GetLabel(" Physician Discharge")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDischarge2" runat="server" Checked="false" /><%:GetLabel(" Discharge")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tampilan Hasil")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboResultType2" ClientInstanceName="cboResultType" Width="150px"
                                        runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kondisi Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientCondition2" ClientInstanceName="cboPatientCondition"
                                        Width="150px" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnDownload2" value="D o w n l o a d" class="btnDownload2 w3-button w3-teal w3-border w3-border-blue w3-round-large" />
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 10px" />
                                            <col style="width: 10px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnRefresh2" value="R e f r e s h" class="btnRefresh2 w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--<td align="center" style="vertical-align: top">
                        <div style="border-color: Red; border-style: solid; border-width: 1px; padding: 5px;">
                            <font color="red"><b>I N F O R M A S I</b></font><br />
                            Belum Diagnosa di sini adalah jika bagian <b><u>Rekam Medis</u></b> belum mengisi
                            Diagnosa.
                        </div>
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:ctlGrdRegisteredPatient2 runat="server" ID="grdRegisteredPatient2" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 2px; display: none" id="containerTidakPerluKodefikasi" class="containerDiagnosa">
            <div class="pageTitle">
                <%=GetLabel("Registrasi Pasien yang Tidak perlu Kodefikasi")%></div>
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 60%" />
                    <col style="width: 40%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 80%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Tanggal")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDateFilter3" ClientInstanceName="cboDateFilter3" Width="150px"
                                        runat="server" BackColor="Pink">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromRegistrationDate3" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                            <td>
                                                <%=GetLabel("s/d") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToRegistrationDate3" Width="120px" CssClass="datepicker" runat="server"
                                                    BackColor="Pink" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom3" ClientInstanceName="cboPatientFrom3" runat="server"
                                        Width="100%">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFrom3ValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblServiceUnit3">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID3" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode3" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName3" ReadOnly="true" Width="99%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView3" ID="txtSearchView3"
                                        Width="435px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchView3SearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="Dokter" FieldName="ParamedicName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationStatus3" ClientInstanceName="cboRegistrationStatus3"
                                                    Width="150px" runat="server">
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsControlDocument3" runat="server" Checked="false" /><%:GetLabel(" Status Berkas : Diproses")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tidak Lengkap")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1000px">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkAllEmpty3" runat="server" Checked="false" /><%:GetLabel(" All Empty")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkChiefComplaint3" runat="server" Checked="false" /><%:GetLabel(" Chief Complaint")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPatientDiagnosis3" runat="server" Checked="false" /><%:GetLabel(" Physician Diagnose")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPhysicianDischarge3" runat="server" Checked="false" /><%:GetLabel(" Physician Discharge")%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDischarge3" runat="server" Checked="false" /><%:GetLabel(" Discharge")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tampilan Hasil")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboResultType3" ClientInstanceName="cboResultType" Width="150px"
                                        runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kondisi Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientCondition3" ClientInstanceName="cboPatientCondition"
                                        Width="150px" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnDownload3" value="D o w n l o a d" class="btnDownload3 w3-button w3-teal w3-border w3-border-blue w3-round-large" />
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 30px" />
                                            <col style="width: 10px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnRefresh3" value="R e f r e s h" class="btnRefresh3 w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--<td align="center" style="vertical-align: top">
                        <div style="border-color: Red; border-style: solid; border-width: 1px; padding: 5px;">
                            <font color="red"><b>I N F O R M A S I</b></font><br />
                            Tidak Perlu Kodefikasi Disini adalah jika bagian <b><u>Rekam Medis</u></b> setting
                            registrasi sebagai tidak diperlukan kodefikasi di menu pasien pulang (Rekam Medis).
                        </div>
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:ctlGrdRegisteredPatient3 runat="server" ID="grdRegisteredPatient3" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
            txtSearchView2.SetText($('#<%=hdnQuickText2.ClientID %>').val());
            txtSearchView3.SetText($('#<%=hdnQuickText3.ClientID %>').val());
        });
    </script>
    <div style="display: none;">
        <asp:Button ID="btnDownloadProcess3" Visible="true" runat="server" OnClick="btnDownloadProcess3_Click"
            Text="Download3" UseSubmitBehavior="false" OnClientClick="return true;" />
    </div>
    <div style="display: none;">
        <asp:Button ID="btnDownloadProcess2" Visible="true" runat="server" OnClick="btnDownloadProcess2_Click"
            Text="Download2" UseSubmitBehavior="false" OnClientClick="return true;" />
    </div>
    <div style="display: none;">
        <asp:Button ID="btnDownloadProcess" Visible="true" runat="server" OnClick="btnDownloadProcess_Click"
            Text="Download" UseSubmitBehavior="false" OnClientClick="return true;" />
    </div>
</asp:Content>
