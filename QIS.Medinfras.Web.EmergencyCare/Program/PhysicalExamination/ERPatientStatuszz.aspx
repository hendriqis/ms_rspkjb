<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="ERPatientStatuszz.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.ERPatientStatus" %>

<%@ Register Src="~/Program/PhysicalExamination/PhysicalExaminationToolbarCtl.ascx"
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

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientStatus', 'mpPatientStatus'))
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

            var serviceDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2);

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

            var registrationDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2);

            $('#<%=txtRegistrationDateDiff.ClientID %>').val(registrationDateDiff);
        }

        function countHour(days, h1, m1, h2, m2) {
            if ($m1 < $m2) {
                $m = $m1 + 60 - $m2;
                $h1 -= 1;
            }
            else $m = $m1 - $m2;

            if (days > 0)
                $h1 = days * 24 + $h1;
            $h = $h1 - $h2;

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
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top;">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Keluhan Utama")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="5"
                                        Width="100%" />
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
                                <td>
                                    <%=GetLabel("Diagnosa") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnose" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Triage") %>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <%=GetLabel("Lokasi Kejadian (Kasus di luar penyakit)") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmergencyCase" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="10" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Keadaan Datang")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                        Width="100%" runat="server" />
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
                        </table>
                    </td>
                    <td valign="top" style="padding-top:10px">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col width="150px" />
                                <col width="150px" />
                            </colgroup>
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
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
