<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="NursingAnamnesis.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingAnamnesis" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
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
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));


            $('#<%=txtServiceDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime1.ClientID %>').val() >= 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 24 && $('#<%=txtServiceTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime2.ClientID %>').val() >= 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60 && $('#<%=txtServiceTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            HourDifference();

            registerCollapseExpandHandler();
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
            var serviceDateInString = $('#<%=txtServiceDate.ClientID %>').val();
            var serviceTime = $('#<%=txtServiceTime1.ClientID %>').val() + ":" + $('#<%=txtServiceTime2.ClientID %>').val();
            var serviceDate = Methods.getDatePickerDate(serviceDateInString);

            //registration difference
            var registrationDateInString = $('#<%=hdnRegistrationDate.ClientID %>').val();
            var registrationTime = $('#<%=hdnRegistrationTime.ClientID %>').val();
            var registrationDate = Methods.getDatePickerDate(registrationDateInString);
            dateDiff = Methods.calculateDateDifference(registrationDate, serviceDate);

            $h1 = parseInt(serviceTime.substring(0, 2), 10);
            $m1 = parseInt(serviceTime.substring(3, 5), 10);

            $h2 = parseInt(registrationTime.substring(0, 2), 10);
            $m2 = parseInt(registrationTime.substring(3, 5), 10);

            var registrationDateDiff = countHour(dateDiff.days, $h1, $m1, $h2, $m2);

            $('#<%=txtServiceDateDiff.ClientID %>').val(registrationDateDiff);
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

            $('#<%=hdnTimeElapsed1hour.ClientID %>').val($h);
            $('#<%=hdnTimeElapsed1minute.ClientID %>').val($m);

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
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
        <input type="hidden" runat="server" id="hdnRegistrationDate" value="00" />
        <input type="hidden" runat="server" id="hdnRegistrationTime" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="0" />
        <input type="hidden" runat="server" id="hdnIsNotAllowNurseFillChiefComplaint" value="" />
        <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="" />
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;" id="tdChiefComplaint" runat="server">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal dan Waktu")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <table>
                                                <colgroup>
                                                    <col style="width: 40px" />
                                                    <col style="width: 10px" />
                                                    <col style="width: 40px" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceTime1" Width="40px" CssClass="number" runat="server"
                                                            Style="text-align: center" MaxLength="2" max="24" min="0" />
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal" />
                                                        <%=GetLabel(":")%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtServiceTime2" Width="40px" CssClass="number" runat="server"
                                                            Style="text-align: center" MaxLength="2" max="59" min="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceDateDiff" ReadOnly="true" Style="text-align: center" Width="100%"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Perawat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                            </td>
                        </tr>
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
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Keluhan Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="3"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Auto Anamnesis" Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Allo Anamnesis" Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                                            Checked="false" />
                                    </td>
                                </table>
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
                            <td class="tdLabel" style="width: 150px; vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Riwayat Penyakit Sekarang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHPISummary" runat="server" Width="99%" TextMode="Multiline"
                                    Rows="10" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDiagnose">
                                    <%=GetLabel("Diagnosa Masuk")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" value="" id="hdnDiagnoseID" />
                                <asp:TextBox ID="txtDiagnose" runat="server" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="display:none">
                    <h4 class="h4collapsed">
                        <%=GetLabel("Status Laktasi")%></h4>
                    <div class="containerTblEntryContent">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsBreastFeeding" runat="server" Text="ASI Eksklusif" Checked="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
