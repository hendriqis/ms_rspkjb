<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PatientStatus.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientStatus" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientAssessment/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtEmergencyCaseDate.ClientID %>');
            $('#<%=txtEmergencyCaseDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtEmergencyCaseDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtEmergencyCaseTime.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtServiceDate.ClientID %>').change(function () {

                HourDifference();
            });

            $('#<%=txtServiceTime.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtRegistrationTime.ClientID %>').change(function () {
                HourDifference();
            });

            HourDifference();
        });

        //#region Visit Type
        function onGetVisitTypeFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
            if (paramedicID == '')
                paramedicID = '0';
            var filterExpression = serviceUnitID + ';' + paramedicID + ';';
            return filterExpression;
        }

        $('#<%:lblVisitType.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedicvisittype', onGetVisitTypeFilterExpression(), function (value) {
                $('#<%:txtVisitTypeCode.ClientID %>').val(value);
                onTxtVisitTypeCodeChanged(value);
            });
        });

        $('#<%:txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var filterExpression = onGetVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                    $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                }
                else {
                    $('#<%:hdnVisitTypeID.ClientID %>').val('');
                    $('#<%:txtVisitTypeCode.ClientID %>').val('');
                    $('#<%:txtVisitTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Diagnose
        $('#lblDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                onTxtDiagnoseCodeChanged(value);
            });
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnDiagnoseID.ClientID %>').val(result.DiagnoseID);
                    $('#<%=txtDiagnose.ClientID %>').val(result.DiagnoseName + ' (' + result.DiagnoseID + ')');
                }
                else {
                    $('#<%=hdnDiagnoseID.ClientID %>').val('');
                    $('#<%=txtDiagnose.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry'))
                onCustomButtonClick('save');
        });


        function HourDifference() {
            var EmergencyCaseDateInString = $('#<%=txtEmergencyCaseDate.ClientID %>').val();
            var EmergencyCaseTime = $('#<%=txtEmergencyCaseTime.ClientID %>').val();

            //service difference
            var serviceDateInString = $('#<%=txtServiceDate.ClientID  %>').val();
            var serviceTime = $('#<%=txtServiceTime.ClientID  %>').val();

            var EmergencyCaseDate = Methods.getDatePickerDate(EmergencyCaseDateInString);
            var serviceDate = Methods.getDatePickerDate(serviceDateInString);

            var dateDiff = Methods.calculateDateDifference(EmergencyCaseDate, serviceDate);
            $h1 = parseInt(serviceTime.substring(0, 2), 10);
            $m1 = parseInt(serviceTime.substring(3, 5), 10);

            $h2 = parseInt(EmergencyCaseTime.substring(0, 2), 10);
            $m2 = parseInt(EmergencyCaseTime.substring(3, 5), 10);

            var serviceDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2, "1");

            $('#<%=txtServiceDateDiff.ClientID %>').val(serviceDateDiff);


            //registration difference
            var registrationDateInString = $('#<%=txtRegistrationDate.ClientID %>').val();
            var registrationTime = $('#<%=txtRegistrationTime.ClientID %>').val();
            var registrationDate = Methods.getDatePickerDate(registrationDateInString);
            dateDiff = Methods.calculateDateDifference(EmergencyCaseDate, registrationDate);

            $h1 = parseInt(registrationTime.substring(0, 2), 10);
            $m1 = parseInt(registrationTime.substring(3, 5), 10);

            $h2 = parseInt(EmergencyCaseTime.substring(0, 2), 10);
            $m2 = parseInt(EmergencyCaseTime.substring(3, 5), 10);

            var registrationDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2, "0");

            $('#<%=txtRegistrationDateDiff.ClientID %>').val(registrationDateDiff);
        }

        function countHour(days, h1, m1, h2, m2, index) {
            if ($m1 < $m2) {
                $m = $m1 + 60 - $m2;
                $h1 -= 1;
            }
            else $m = $m1 - $m2;

            if (days > 0)
                $h1 = days * 24 + $h1;
            $h = $h1 - $h2;

            if (index == "1") {
                $('#<%=hdnTimeElapsed1hour.ClientID %>').val($h);
                $('#<%=hdnTimeElapsed1minute.ClientID %>').val($m);
            }
            else {
                $('#<%=hdnTimeElapsed0hour.ClientID %>').val($h);
                $('#<%=hdnTimeElapsed0minute.ClientID %>').val($m);
            }
            return $h + " Jam " + $m + " Menit";
        }

        function onCboOnsetChanged(s) {
            $txt = $('#<%=txtOnset.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboProvocationChanged(s) {
            $txt = $('#<%=txtProvocation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboQualityChanged(s) {
            $txt = $('#<%=txtQuality.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboSeverityChanged(s) {
            $txt = $('#<%=txtSeverity.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboTimeChanged(s) {
            $txt = $('#<%=txtTime.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboRelievedByChanged(s) {
            $txt = $('#<%=txtRelievedBy.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        function onAfterSavePatientPhoto() {
            var MRN = $('#<%:hdnMRN.ClientID %>').val();
            var filterExpression = 'MRN = ' + MRN;
            hideLoadingPanel();
            pcRightPanelContent.Hide();
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientPhoto') {
                var param = $('#<%:hdnMRN.ClientID %>').val();
                return param;
            }
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnMRN" value="" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;" id="tdChiefComplaint" runat="server">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keluhan Utama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Auto Anamnesis" Checked="false" />
                                    </td>
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Allo Anamnesis" Checked="false" />
                                    </td>
                                </table>
                                <%--                                    <asp:RadioButtonList ID="rblAnamnesis" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Auto Anamnesis" Value="true" Selected="True" />
                                        <asp:ListItem Text="Allo Anamnesis" Value="false" />
                                    </asp:RadioButtonList>--%>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Location")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocation" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Onset")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboOnset" ClientInstanceName="cboOnset" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboOnsetChanged(s); }" Init="function(s,e){ onCboOnsetChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOnset" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Provocation")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboProvocation" ClientInstanceName="cboProvocation"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboProvocationChanged(s); }"
                                        Init="function(s,e){ onCboProvocationChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProvocation" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Quality")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboQuality" ClientInstanceName="cboQuality"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboQualityChanged(s); }"
                                        Init="function(s,e){ onCboQualityChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuality" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Severity")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboSeverityChanged(s); }"
                                        Init="function(s,e){ onCboSeverityChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSeverity" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Time")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboTime" ClientInstanceName="cboTime" Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTimeChanged(s); }" Init="function(s,e){ onCboTimeChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTime" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Relieved By")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboRelievedBy" ClientInstanceName="cboRelievedBy"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRelievedByChanged(s); }"
                                        Init="function(s,e){ onCboRelievedByChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRelievedBy" CssClass="txtChief" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Alasan Kunjungan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                    runat="server">
                                    <ClientSideEvents Init="function(s,e){ onCboVisitReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVisitNotes" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDiagnose">
                                    <%=GetLabel("Diagnosa")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" value="" id="hdnDiagnoseID" />
                                <asp:TextBox ID="txtDiagnose" runat="server" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col width="150px" />
                            <col width="150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                    <%:GetLabel("Jenis Kunjungan")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="width: 100%;">
                                    <%=GetLabel("Waktu Kejadian") %></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmergencyCaseDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmergencyCaseTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="width: 100%;">
                                    <%=GetLabel("Waktu Registrasi")%></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationTime" Width="80px" runat="server" CssClass="time" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationDateDiff" ReadOnly="true" Style="text-align: center"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="width: 100%;">
                                    <%=GetLabel("Waktu Pelayanan")%></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceDateDiff" ReadOnly="true" Style="text-align: center" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Triage") %>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keadaan Datang")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Lokasi Kejadian (Kasus di luar penyakit)") %>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtEmergencyCase" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="10" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
