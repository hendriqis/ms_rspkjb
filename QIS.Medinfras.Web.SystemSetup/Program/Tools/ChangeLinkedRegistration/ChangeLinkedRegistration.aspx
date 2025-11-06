<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangeLinkedRegistration.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ChangeLinkedRegistration" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=chkFilterHari.ClientID %>').attr("disabled", true);
            $('#<%=chkFilterHari.ClientID %>').prop('checked', true);
        }

        $('#<%=btnProcess.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                if ($('#<%=hdnRegistrationID.ClientID %>').val() != "") {
                    onCustomButtonClick('process');
                }
                else {
                    showToast('Warning', 'Pilih nomor registrasi terlebih dahulu.');
                }
            }
        });

        //#region Registration No
        function getRegistrationNoFilterExpression() {
            var filterExpression = "GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.CANCELLED + "','" + Constant.RegistrationStatus.CLOSED
                                    + "') AND DepartmentID = '" + Constant.Facility.INPATIENT + "'"
                                    + " AND RegistrationID NOT IN (SELECT r.RegistrationID FROM Registration r INNER JOIN Registration lr ON lr.LinkedToRegistrationID = r.RegistrationID WHERE lr.IsChargesTransfered = 1)";
            return filterExpression;
        }

        $('#lblNoReg.lblLink').live('click', function () {
            openSearchDialog('registration', getRegistrationNoFilterExpression(), function (value) {
                $('#<%:txtRegistrationNo.ClientID %>').val(value);
                onTxtRegistrationNoChanged(value);
            });
        });

        $('#<%:txtRegistrationNo.ClientID %>').live('change', function () {
            onTxtRegistrationNoChanged($(this).val());
        });

        function onTxtRegistrationNoChanged(value) {
            var filterExpression = getRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistrationLinkedList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                    $('#<%:txtRegistrationDate.ClientID %>').val(result.RegistrationDateInString);
                    $('#<%:txtRegistrationHour.ClientID %>').val(result.RegistrationTime);
                    $('#<%:txtMRN.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientName.ClientID %>').val(result.PatientName);
                    $('#<%:txtPreferredName.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender.ClientID %>').val(result.Gender);
                    $('#<%:txtDOB.ClientID %>').val(result.DateOfBirthInString);
                    $('#<%:txtAgeInYear.ClientID %>').val(result.AgeInYear);
                    $('#<%:txtAgeInMonth.ClientID %>').val(result.AgeInMonth);
                    $('#<%:txtAgeInDay.ClientID %>').val(result.AgeInDay);
                    $('#<%:txtAddress.ClientID %>').val(result.HomeAddress);
                    cboAdmissionSource.SetValue(result.GCAdmissionSource);
                    if (cboAdmissionSource.GetValue() != Constant.AdmissionSource.INPATIENT) {
                        $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblLink lblMandatory');
                        $('#<%:txtFromRegistrationNo.ClientID %>').removeAttr('readonly');
                        $('#<%=chkFilterHari.ClientID %>').attr("disabled", false);
                    }
                    else {
                        $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblDisabled');
                        $('#<%:txtFromRegistrationNo.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=chkFilterHari.ClientID %>').attr("disabled", true);
                    }
                    $('#<%:hdnFirstFromRegistrationID.ClientID %>').val(result.LinkedRegistrationID);
                    $('#<%:hdnFromRegistrationID.ClientID %>').val(result.LinkedRegistrationID); 
                    $('#<%:txtFromRegistrationNo.ClientID %>').val(result.LinkedRegistrationNo);
                    $('#<%:txtFromMRN.ClientID %>').val(result.FromMedicalNo);
                    $('#<%:txtFromPatientName.ClientID %>').val(result.FromPatientName);
                    $('#<%:txtFromPreferredName.ClientID %>').val(result.FromPreferredName);
                    $('#<%:txtFromGender.ClientID %>').val(result.FromGender);
                    $('#<%:txtFromDOB.ClientID %>').val(result.FromDateOfBirthInString);
                    $('#<%:txtFromAgeInYear.ClientID %>').val(result.FromAgeInYear);
                    $('#<%:txtFromAgeInMonth.ClientID %>').val(result.FromAgeInMonth);
                    $('#<%:txtFromAgeInDay.ClientID %>').val(result.FromAgeInDay);
                    $('#<%:txtFromAddress.ClientID %>').val(result.FromHomeAddress);

                    //onTxtFromRegistrationNoChanged(result.LinkedRegistrationNo);
                }
                else {
                    $('#<%:hdnRegistrationID.ClientID %>').val('');
                    $('#<%:txtRegistrationNo.ClientID %>').val('');
                    $('#<%:txtMRN.ClientID %>').val('');
                    $('#<%:txtPatientName.ClientID %>').val('');
                    $('#<%:txtPreferredName.ClientID %>').val('');
                    $('#<%:txtGender.ClientID %>').val('');
                    $('#<%:txtDOB.ClientID %>').val('');
                    $('#<%:txtAgeInYear.ClientID %>').val('');
                    $('#<%:txtAgeInMonth.ClientID %>').val('');
                    $('#<%:txtAgeInDay.ClientID %>').val('');
                    $('#<%:txtAddress.ClientID %>').val('');
                    cboAdmissionSource.SetValue('');
                    $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                    $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                    $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');

                    $('#<%:hdnFirstFromRegistrationID.ClientID %>').val('');
                    $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                    $('#<%:txtFromMRN.ClientID %>').val('');
                    $('#<%:txtFromPatientName.ClientID %>').val('');
                    $('#<%:txtFromPreferredName.ClientID %>').val('');
                    $('#<%:txtFromGender.ClientID %>').val('');
                    $('#<%:txtFromDOB.ClientID %>').val('');
                    $('#<%:txtFromAgeInYear.ClientID %>').val('');
                    $('#<%:txtFromAgeInMonth.ClientID %>').val('');
                    $('#<%:txtFromAgeInDay.ClientID %>').val('');
                    $('#<%:txtFromAddress.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region From Registration No
        function getFromRegistrationNoFilterExpression() {
            var departmentID = '';
            var admissionSource = cboAdmissionSource.GetValue();
            var isFilterHari = $('#<%=chkFilterHari.ClientID %>').is(":checked");
            switch (admissionSource) {
                case Constant.AdmissionSource.EMERGENCY: departmentID = Constant.Facility.EMERGENCY; break;
                case Constant.AdmissionSource.DIAGNOSTIC: departmentID = Constant.Facility.DIAGNOSTIC; break;
                default: departmentID = Constant.Facility.OUTPATIENT; break;
            }
            var filterExpression = "DepartmentID = '" + departmentID + "' AND GCRegistrationStatus NOT IN ('" + Constant.RegistrationStatus.OPEN + "','" + Constant.RegistrationStatus.CLOSED + "','" + Constant.RegistrationStatus.CANCELLED + "')";
            filterExpression += " AND LinkedToRegistrationID IS NULL";

            var today = new Date($('#<%:txtRegistrationDate.ClientID %>').val());
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = Math.abs(today.getFullYear());

            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var today = yyyy + '' + mm + '' + dd;

            var yesterday = new Date($('#<%:txtRegistrationDate.ClientID %>').val());
            yesterday.setDate(yesterday.getDate() - 1);
            var dd1 = yesterday.getDate();
            var mm1 = yesterday.getMonth() + 1; //January is 0!
            var yyyy1 = Math.abs(yesterday.getFullYear());

            if (dd1 < 10) {
                dd1 = '0' + dd1;
            }
            if (mm1 < 10) {
                mm1 = '0' + mm1;
            }
            var yesterday = yyyy1 + '' + mm1 + '' + dd1;

            if (isFilterHari) {
                filterExpression += " AND (RegistrationDate BETWEEN '" + yesterday + "' AND '" + today + "')";
            }
            return filterExpression;
        }

        $('#<%:lblFromRegistrationNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('registration', getFromRegistrationNoFilterExpression(), function (value) {
                $('#<%:txtFromRegistrationNo.ClientID %>').val(value);
                onTxtFromRegistrationNoChanged(value);
            });
        });

        $('#<%:txtFromRegistrationNo.ClientID %>').live('change', function () {
            onTxtFromRegistrationNoChanged($(this).val());
        });

        function onTxtFromRegistrationNoChanged(value) {
            var filterExpression = getFromRegistrationNoFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistration7List', filterExpression, function (result) {
                if (result != null) {
                    var blok24jam = $('#<%:hdnRegistrasiAsalDiblokJikaLebih24Jam.ClientID %>').val();

                    var regDate = $('#<%:txtRegistrationDate.ClientID %>').val();
                    var regTime = $('#<%:txtRegistrationHour.ClientID %>').val();
                    var regTemp = Methods.DatePickerToDateFormat(regDate) + "" + regTime.substring(0, 2) + "" + regTime.substring(3, 5);
                    var regTempDate = Methods.stringToDateTime(regTemp);

                    var regFromDate = Methods.DatePickerToDateFormat(result.RegistrationDateInDatePicker);
                    var regFromTime = result.RegistrationTime;
                    var regFromTemp = regFromDate + "" + regFromTime.substring(0, 2) + "" + regFromTime.substring(3, 5);
                    var regFromTempDate = Methods.stringToDateTime(regFromTemp);

                    var diff = Methods.calculateDateTimeDifference(regTempDate, regFromTempDate);

                    if (diff > 24) {
                        var regFromDate = 'Registrasi Asal <b>' + result.RegistrationNo + ' (' + Methods.dateToDMY(regFromTempDate) + ' ' + result.RegistrationTime + '</b>) sudah lebih dari 24 jam lalu.';
                        if (blok24jam == "1") {
                            showToast('GAGAL', regFromDate);

                            $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
                            //cboAdmissionSource.SetValue('');
                            $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                            $('#<%:txtFromMRN.ClientID %>').val('');
                            $('#<%:txtFromPatientName.ClientID %>').val('');
                            $('#<%:txtFromPreferredName.ClientID %>').val('');
                            $('#<%:txtFromGender.ClientID %>').val('');
                            $('#<%:txtFromDOB.ClientID %>').val('');
                            $('#<%:txtFromAgeInYear.ClientID %>').val('');
                            $('#<%:txtFromAgeInMonth.ClientID %>').val('');
                            $('#<%:txtFromAgeInDay.ClientID %>').val('');
                            $('#<%:txtFromAddress.ClientID %>').val('');
                        } else {
                            $('#<%:hdnFromRegistrationID.ClientID %>').val(result.RegistrationID);
                            $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val(result.IsNewPatient ? '1' : '0');
                            $('#<%:txtFromMRN.ClientID %>').val(result.MedicalNo);
                            $('#<%:txtFromPatientName.ClientID %>').val(result.PatientName);
                            $('#<%:txtFromPreferredName.ClientID %>').val(result.PreferredName);
                            $('#<%:txtFromGender.ClientID %>').val(result.Gender);
                            $('#<%:txtFromDOB.ClientID %>').val(result.DateOfBirthInString);
                            $('#<%:txtFromAgeInYear.ClientID %>').val(result.AgeInYear);
                            $('#<%:txtFromAgeInMonth.ClientID %>').val(result.AgeInMonth);
                            $('#<%:txtFromAgeInDay.ClientID %>').val(result.AgeInDay);
                            $('#<%:txtFromAddress.ClientID %>').val(result.HomeAddress);
                            $('#<%:txtRegistrationDate.ClientID %>').val(result.RegistrationDateInString);
                        }
                    } else {
                        $('#<%:hdnFromRegistrationID.ClientID %>').val(result.RegistrationID);
                        $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val(result.IsNewPatient ? '1' : '0');
                        $('#<%:txtFromMRN.ClientID %>').val(result.MedicalNo);
                        $('#<%:txtFromPatientName.ClientID %>').val(result.PatientName);
                        $('#<%:txtFromPreferredName.ClientID %>').val(result.PreferredName);
                        $('#<%:txtFromGender.ClientID %>').val(result.Gender);
                        $('#<%:txtFromDOB.ClientID %>').val(result.DateOfBirthInString);
                        $('#<%:txtFromAgeInYear.ClientID %>').val(result.AgeInYear);
                        $('#<%:txtFromAgeInMonth.ClientID %>').val(result.AgeInMonth);
                        $('#<%:txtFromAgeInDay.ClientID %>').val(result.AgeInDay);
                        $('#<%:txtFromAddress.ClientID %>').val(result.HomeAddress);
                        $('#<%:txtRegistrationDate.ClientID %>').val(result.RegistrationDateInString);
                    }
                }
                else {
                    $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
                    //cboAdmissionSource.SetValue('');
                    $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                    $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                    $('#<%:txtFromMRN.ClientID %>').val('');
                    $('#<%:txtFromPatientName.ClientID %>').val('');
                    $('#<%:txtFromPreferredName.ClientID %>').val('');
                    $('#<%:txtFromGender.ClientID %>').val('');
                    $('#<%:txtFromDOB.ClientID %>').val('');
                    $('#<%:txtFromAgeInYear.ClientID %>').val('');
                    $('#<%:txtFromAgeInMonth.ClientID %>').val('');
                    $('#<%:txtFromAgeInDay.ClientID %>').val('');
                    $('#<%:txtFromAddress.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region CboAdmission
        function onCboAdmissionSourceValueChanged(s) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            if (registrationID != '') {
                $('#<%:hdnFromRegistrationID.ClientID %>').val('');
                $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                if (s.GetValue() != Constant.AdmissionSource.INPATIENT) {
                    $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblLink lblMandatory');
                    $('#<%:txtFromRegistrationNo.ClientID %>').removeAttr('readonly');
                    $('#<%=chkFilterHari.ClientID %>').attr("disabled", false);
                }
                else {
                    $('#<%:lblFromRegistrationNo.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtFromRegistrationNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=chkFilterHari.ClientID %>').attr("disabled", true);
                }
                $('#<%:txtFromRegistrationNo.ClientID %>').val('');
                $('#<%:txtFromMRN.ClientID %>').val('');
                $('#<%:txtFromPatientName.ClientID %>').val('');
                $('#<%:txtFromPreferredName.ClientID %>').val('');
                $('#<%:txtFromGender.ClientID %>').val('');
                $('#<%:txtFromDOB.ClientID %>').val('');
                $('#<%:txtFromAgeInYear.ClientID %>').val('');
                $('#<%:txtFromAgeInMonth.ClientID %>').val('');
                $('#<%:txtFromAgeInDay.ClientID %>').val('');
                $('#<%:txtFromAddress.ClientID %>').val('');
            }
            else {
                showToast('WARNING', 'Silahkan Pilih Registrasi Terlebih Dahulu');
            }
        }
        //#endregion

        $('#<%:chkFilterHari.ClientID %>').live('change', function () {
            onTxtFromRegistrationNoChanged('');
        });

        function onAfterCustomClickSuccess(type, retval) {
            var registrationNo = $('#<%:txtRegistrationNo.ClientID %>').val();
            var registrationFrom = $('#<%:txtFromRegistrationNo.ClientID %>').val();
            var registrationMethod = '';

            var filterExpressionDiagnose = "StandardCodeID = '" + cboAdmissionSource.GetValue() + "'";
            Methods.getObject('GetStandardCodeList', filterExpressionDiagnose, function (resultCheck) {
                if (resultCheck != null) {
                    registrationMethod = resultCheck.StandardCodeName;
                }
            });

            var textMessage = '';
            if (cboAdmissionSource.GetValue() == '0023^X04') {
                textMessage = "Registrasi <b>" + registrationNo + "</b> berhasil diubah cara pendaftarannya menjadi dari <b>" + registrationMethod + "</b>";
            }
            else {
                textMessage = "Registrasi <b>" + registrationNo + "</b> berhasil diubah cara pendaftarannya menjadi dari <b>" + registrationMethod + "</b> di Registrasi <b>" + registrationFrom + "</b>";
            }

            showToast("Ubah Link Registrasi : ", textMessage);

            $('#<%:hdnRegistrationID.ClientID %>').val('');
            $('#<%:txtRegistrationNo.ClientID %>').val('');
            $('#<%:txtMRN.ClientID %>').val('');
            $('#<%:txtPatientName.ClientID %>').val('');
            $('#<%:txtPreferredName.ClientID %>').val('');
            $('#<%:txtGender.ClientID %>').val('');
            $('#<%:txtDOB.ClientID %>').val('');
            $('#<%:txtAgeInYear.ClientID %>').val('');
            $('#<%:txtAgeInMonth.ClientID %>').val('');
            $('#<%:txtAgeInDay.ClientID %>').val('');
            $('#<%:txtAddress.ClientID %>').val('');
            cboAdmissionSource.SetValue('');
            $('#<%:hdnFromRegistrationID.ClientID %>').val('');
            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
            $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');

            $('#<%:hdnFirstFromRegistrationID.ClientID %>').val('');
            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
            $('#<%:txtFromMRN.ClientID %>').val('');
            $('#<%:txtFromPatientName.ClientID %>').val('');
            $('#<%:txtFromPreferredName.ClientID %>').val('');
            $('#<%:txtFromGender.ClientID %>').val('');
            $('#<%:txtFromDOB.ClientID %>').val('');
            $('#<%:txtFromAgeInYear.ClientID %>').val('');
            $('#<%:txtFromAgeInMonth.ClientID %>').val('');
            $('#<%:txtFromAgeInDay.ClientID %>').val('');
            $('#<%:txtFromAddress.ClientID %>').val('');

            $('#<%:hdnFromRegistrationIsNewPatient.ClientID %>').val('0');
            //cboAdmissionSource.SetValue('');
            $('#<%:hdnFromRegistrationID.ClientID %>').val('');
            $('#<%:txtFromRegistrationNo.ClientID %>').val('');
            $('#<%:txtFromMRN.ClientID %>').val('');
            $('#<%:txtFromPatientName.ClientID %>').val('');
            $('#<%:txtFromPreferredName.ClientID %>').val('');
            $('#<%:txtFromGender.ClientID %>').val('');
            $('#<%:txtFromDOB.ClientID %>').val('');
            $('#<%:txtFromAgeInYear.ClientID %>').val('');
            $('#<%:txtFromAgeInMonth.ClientID %>').val('');
            $('#<%:txtFromAgeInDay.ClientID %>').val('');
            $('#<%:txtFromAddress.ClientID %>').val('');
        }
    </script>
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnFirstFromRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnFromRegistrationIsNewPatient" value="" runat="server" />
    <input type="hidden" id="hdnRegistrasiAsalDiblokJikaLebih24Jam" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td style="padding-left: 30px; padding-right: 10px">
                            <%:GetLabel("Jam Pendaftaran")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationHour" CssClass="time" runat="server" Width="60px"
                                Style="text-align: center" MaxLength="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink lblKey" id="lblNoReg">
                                    <%:GetLabel("No. Registrasi")%></label></div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pasien")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trMRN">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("No. Rekam Medis (No.RM)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMRN" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Panggilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPreferredName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtGender" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Lahir")%>
                                /
                                <%:GetLabel("Umur")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 118px" />
                                    <col style="width: 50px" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDOB" Width="118px" runat="server" Style="margin-right: 3px; text-align: left" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInYear" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Tahun")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInMonth" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Bulan")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgeInDay" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Hari")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%:GetLabel("Alamat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trAdmissionSource" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Cara Pendaftaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboAdmissionSource" ClientInstanceName="cboAdmissionSource"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboAdmissionSourceValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkFilterHari" runat="server" /><%:GetLabel("Filter Hari")%>
                        </td>
                    </tr>
                    <tr id="trAdmissionRegistrationNo" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory" id="lblFromRegistrationNo" runat="server">
                                <%:GetLabel("No Registrasi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFromRegistrationID" value="" runat="server" />
                            <asp:TextBox ID="txtFromRegistrationNo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pasien : Registrasi Asal")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trMRN">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("No. Rekam Medis (No.RM)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromMRN" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Nama Panggilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromPreferredName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromGender" Width="175px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Lahir")%>
                                /
                                <%:GetLabel("Umur")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 118px" />
                                    <col style="width: 50px" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col style="width: 10%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromDOB" Width="118px" runat="server" Style="margin-right: 3px;
                                            text-align: left" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromAgeInYear" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Tahun")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromAgeInMonth" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Bulan")%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromAgeInDay" Width="50px" runat="server" Style="margin-right: 3px;
                                            text-align: right" />
                                    </td>
                                    <td style="padding-left: 5px; padding-right: 5px">
                                        <%:GetLabel("Hari")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%:GetLabel("Alamat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromAddress" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
