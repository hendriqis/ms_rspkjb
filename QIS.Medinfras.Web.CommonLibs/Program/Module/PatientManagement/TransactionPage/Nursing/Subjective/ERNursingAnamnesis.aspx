<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ERNursingAnamnesis.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ERNursingAnamnesis" %>

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
            setDatePicker('<%=txtEmergencyCaseDate.ClientID %>');
            $('#<%=txtEmergencyCaseDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtEmergencyCaseDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtEmergencyCaseTime1.ClientID %>').change(function () {
                if ($('#<%=txtEmergencyCaseTime1.ClientID %>').val() >= 0 && $('#<%=txtEmergencyCaseTime1.ClientID %>').val() < 24 && $('#<%=txtEmergencyCaseTime1.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtEmergencyCaseTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtEmergencyCaseTime2.ClientID %>').change(function () {
                if ($('#<%=txtEmergencyCaseTime2.ClientID %>').val() >= 0 && $('#<%=txtEmergencyCaseTime2.ClientID %>').val() < 60 && $('#<%=txtEmergencyCaseTime2.ClientID %>').val().length == 2) {
                    HourDifference();
                } else {
                    $('#<%=txtEmergencyCaseTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceDate.ClientID %>').change(function () {

                HourDifference();
            });

            $('#<%=txtServiceTime1.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime1.ClientID %>').val() > 0 && $('#<%=txtServiceTime1.ClientID %>').val() < 24) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime1.ClientID %>').val($('#<%=hdnTimeNow1.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtServiceTime2.ClientID %>').change(function () {
                if ($('#<%=txtServiceTime2.ClientID %>').val() > 0 && $('#<%=txtServiceTime2.ClientID %>').val() < 60) {
                    HourDifference();
                } else {
                    $('#<%=txtServiceTime2.ClientID %>').val($('#<%=hdnTimeNow2.ClientID %>').val());
                    showToast('GAGAL', "Waktu yang terisi tidak valid.");
                }
            });

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                HourDifference();
            });

            $('#<%=txtRegistrationTime.ClientID %>').change(function () {
                HourDifference();
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
            var EmergencyCaseTime = $('#<%=txtEmergencyCaseTime1.ClientID %>').val() + ":" + $('#<%=txtEmergencyCaseTime2.ClientID %>').val();

            //service difference
            var serviceDateInString = $('#<%=txtServiceDate.ClientID  %>').val();
            var serviceTime = $('#<%=txtServiceTime1.ClientID %>').val() + ":" + $('#<%=txtServiceTime2.ClientID %>').val();

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
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER || cboVisitReason.GetValue() == Constant.VisitReason.ACCIDENT)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        $('#btnBedQuickPicks').live('click', function () {
            var url = ResolveUrl('~/Controls/BedQuickPicksCtl.ascx');
            openUserControlPopup(url, '', 'Pilih Tempat Tidur', 1150, 550);
        });
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();

            if (code == 'ER090314') {
                filterExpression.text = registrationID;
                    return true;
                }
            
        }

        function onAfterClickBedQuickPicks(healthcareServiceUnitID, serviceUnitCode, serviceUnitName, roomID, roomCode, roomName, classID, classCode, className, chargeClassID, chargeClassBPJSCode, chargeClassCode, chargeClassName, bedID, bedCode, chargeClassBPJSType) {
            $('#<%:hdnClassID.ClientID %>').val(classID);
            $('#<%:txtClassCode.ClientID %>').val(classCode);
            $('#<%:txtClassName.ClientID %>').val(className);
            $('#<%:hdnChargeClassID.ClientID %>').val(classID);
            $('#<%:txtChargeClassCode.ClientID %>').val(classCode);
            $('#<%:txtChargeClassName.ClientID %>').val(className);
            $('#<%:hdnRoomID.ClientID %>').val(roomID);
            $('#<%:txtRoomCode.ClientID %>').val(roomCode);
            $('#<%:txtRoomName.ClientID %>').val(roomName);
            $('#<%:hdnBedID.ClientID %>').val(bedID);
            $('#<%:txtBedCode.ClientID %>').val(bedCode);
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
        <input type="hidden" runat="server" id="hdnTimeNow1" value="00" />
        <input type="hidden" runat="server" id="hdnTimeNow2" value="00" />
        <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="0" />
        <input type="hidden" id="hdnRegistrationID" runat="server" />
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
                                    <%=GetLabel("Anamnesa Perawat")%></label>
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
                                    <td style="width: 27%">
                                        <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text=" Auto Anamnesis" Checked="false" />
                                    </td>
                                    <td style="width: 27%">
                                        <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text=" Allo Anamnesis" Checked="false" />
                                    </td>
                                    <td style="width: 27%">
                                        <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text=" Tidak ada Alergi"
                                            Checked="false" />
                                    </td>
                                    <td style="width: 27%">
                                        <asp:CheckBox ID="chkIsFastTrack" runat="server" Text=" Fast Track" />
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
                                <label class="lblMandatory">
                                    <%=GetLabel("Perawat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBedQuickPicks" runat="server">
                            <td style="width: 30%">
                                <input type="button" id="btnBedQuickPicks" value='<%:("Pilih Tempat Tidur") %>' />
                            </td>
                        </tr>
                        <tr id="trRoom" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" runat="server" id="lblRoom">
                                    <%:GetLabel("Kamar")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trBed" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" runat="server" id="lblBed">
                                    <%:GetLabel("Tempat Tidur")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBedID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 0px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBedCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" runat="server" id="lblClass">
                                    <%:GetLabel("Kelas")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnClassID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtClassCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClassName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trChargeClass" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" runat="server" id="lblChargeClass">
                                    <%:GetLabel("Kelas Tagihan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnChargeClassID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="5">
                                <h4 class="h4expanded">
                                    <%=GetLabel("SURVEY PRIMER")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col width="150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Airway")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboAirway" ClientInstanceName="cboAirway" Width="100%" />
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Breathing")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboBreathing" ClientInstanceName="cboBreathing"
                                                    Width="100%" />
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Circulation")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboCirculation" ClientInstanceName="cboCirculation"
                                                    Width="100%" />
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Disability")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboDisability" ClientInstanceName="cboDisability"
                                                    Width="100%" />
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Exposure")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboExposure" ClientInstanceName="cboExposure"
                                                    Width="100%" />
                                            </td>
                                            <td />
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="5">
                                <h4 class="h4expanded">
                                    <%=GetLabel("Riwayat Penyakit Dahulu dan Terapi")%></h4>
                                <div class="containerTblEntryContent">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col width="150px" />
                                            <col />
                                            <col width="150px" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top">
                                                <label class="lblLink" id="lblMedicalHistory">
                                                    <%=GetLabel("Riwayat Penyakit Dahulu")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtMedicalHistory" runat="server" TextMode="MultiLine" Rows="3"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top">
                                                <label class="lblLink" id="lblMedicationHistory">
                                                    <%=GetLabel("Riwayat Pengobatan")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtMedicationHistory" runat="server" TextMode="MultiLine" Rows="3"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td valign="top">
                                <h4 class="h4expanded">
                                    <%=GetLabel("TRIASE")%></h4>
                                <div class="containerTblEntryContent">
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
                                                <table>
                                                    <colgroup>
                                                        <col style="width: 40px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 40px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtEmergencyCaseTime1" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="24" min="0" />
                                                        </td>
                                                        <td>
                                                            <label class="lblNormal" />
                                                            <%=GetLabel(":")%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmergencyCaseTime2" Width="40px" CssClass="number" runat="server"
                                                                Style="text-align: center" MaxLength="2" max="59" min="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%=GetLabel("Waktu Registrasi")%></div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker"
                                                    ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationTime" Width="80px" runat="server" CssClass="time"
                                                    ReadOnly="true" />
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
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Cara Datang")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboAdmissionRoute" ClientInstanceName="cboAdmissionRoute"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Triage")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Keadaan Datang")%></label>
                                            </td>
                                            <td colspan="3">
                                                <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                                    Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <%=GetLabel("Lokasi dan Mekanisme Trauma") %>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtEmergencyCase" runat="server" Width="100%" TextMode="Multiline"
                                                    Rows="10" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
