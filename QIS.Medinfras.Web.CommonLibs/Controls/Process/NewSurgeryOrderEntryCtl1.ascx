<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewSurgeryOrderEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NewSurgeryOrderEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_NewSurgeryOrderEntryCtl1">
    $(function () {
        setDatePicker('<%=txtOrderDate.ClientID %>');
        setDatePicker('<%=txtScheduleDate.ClientID %>');

        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#<%=rblSourcePatientType.ClientID %> input').change(function () {
            if ($(this).val() != "1") {
                $('#<%=chkIsNextVisit.ClientID %>').removeAttr("disabled");
                $('#<%=chkIsNextVisit.ClientID %>').prop('checked', false);
                $('#<%=trNextVisit.ClientID %>').attr("style", "display:none");

                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }
            else {
                $('#<%=chkIsNextVisit.ClientID %>').attr("disabled", "disabled");
                $('#<%=chkIsNextVisit.ClientID %>').prop('checked', false);
                $('#<%=trNextVisit.ClientID %>').attr("style", "display:none");

                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
            }
        });

        $('#<%=rblIsEmergency.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', true);
            }
            else {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', false);
            }
        });

        $('#<%=chkIsNextVisit.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=trNextVisit.ClientID %>').removeAttr("style");

                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
            }
            else {
                $('#<%=trNextVisit.ClientID %>').attr("style", "display:none");

                if ($('#<%=rblSourcePatientType.ClientID %>').val() != "1") {
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '0');
                } else {
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
                    $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'maxDate', '365');
                }
            }
        });

        //#region Room

        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

            return filterExpression;
        }

        $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
                $('#<%:txtRoomCode.ClientID %>').val(value);
                onTxtRoomCodeChanged(value);
            });
        });

        $('#<%:txtRoomCode.ClientID %>').live('change', function () {
            onTxtRoomCodeChanged($(this).val());
        });

        function onTxtRoomCodeChanged(value) {
            var filterExpression = getRoomFilterExpression() + " AND RoomCode = '" + value + "'";
            getRoom(filterExpression);
        }

        function getRoom(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getRoomFilterExpression();
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID != "") {
                Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                    if (result.length == 1) {
                        $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                        $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                        $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                    }
                    else {
                        $('#<%:hdnRoomID.ClientID %>').val('');
                        $('#<%:txtRoomCode.ClientID %>').val('');
                        $('#<%:txtRoomName.ClientID %>').val('');
                    }
                });
            } else {
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Registration No
        function getRegistrationNoFilterExpression() {
            var filterExpression = "DepartmentID = 'INPATIENT' AND GCRegistrationStatus IN ('X020^002','X020^003')";
            var selectedvalue = $('#<%= rblSourcePatientType.ClientID %> input:checked').val()
            if (selectedvalue == "0") {
                filterExpression = "DepartmentID = 'OUTPATIENT' AND GCRegistrationStatus NOT IN ('X020^006')";
            }
            else if (selectedvalue == "2") {
                filterExpression = "DepartmentID = 'EMERGENCY' AND GCRegistrationStatus NOT IN ('X020^006')";
            }
            else if (selectedvalue == "3") {
                filterExpression = "DepartmentID = 'DIAGNOSTIC' AND GCRegistrationStatus NOT IN ('X020^006')";
            }
            return filterExpression;
        }

        $('#<%:lblNoReg.ClientID %>.lblLink').die('click');
        $('#<%:lblNoReg.ClientID %>.lblLink').live('click', function () {
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
            Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                if (result != null) {
                    var patientLocation = result.ServiceUnitName;
                    $('#<%:hdnVisitID.ClientID %>').val(result.VisitID);
                    $('#<%:hdnFromHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%:txtMedicalNo.ClientID %>').val(result.MedicalNo);
                    $('#<%:txtPatientInfo.ClientID %>').val(result.PatientName);
                    $('#<%:txtPatientLocation.ClientID %>').val(patientLocation);
                    cboParamedicID.SetValue(result.ParamedicID);
                }
                else {
                    $('#<%:hdnVisitID.ClientID %>').val('0');
                    $('#<%:hdnFromHealthcareServiceUnitID.ClientID %>').val('0');
                    $('#<%:txtRegistrationNo.ClientID %>').val('');
                    $('#<%:txtMedicalNo.ClientID %>').val('');
                    $('#<%:txtPatientInfo.ClientID %>').val('');
                    $('#<%:txtPatientLocation.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Procedure Group

        function getProcedureGroupExpression() {
            var filterExpression = '';
            filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblProcedureGroupCode.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('proceduregroup', getProcedureGroupExpression(), function (value) {
                $('#<%:txtProcedureGroupCode.ClientID %>').val(value);
                onTxtProcedureGroupCodeChanged(value);
            });
        });

        $('#<%:txtProcedureGroupCode.ClientID %>').live('change', function () {
            onTxtProcedureGroupCodeChanged($(this).val());
        });

        function onTxtProcedureGroupCodeChanged(value) {
            var filterExpression = getProcedureGroupExpression() + " AND ProcedureGroupCode = '" + value + "'";
            getProcedureGroup(filterExpression);
        }

        function getProcedureGroup(_filterExpression) {
            var filterExpression = _filterExpression;
            Methods.getListObject('GetProcedureGroupList', filterExpression, function (result) {
                if (result.length == 1) {
                    $('#<%:hdnEntryProcedureGroupID.ClientID %>').val(result[0].ProcedureGroupID);
                    $('#<%:txtProcedureGroupName.ClientID %>').val(result[0].ProcedureGroupName);
                    $('#<%:txtProcedureGroupCode.ClientID %>').val(result[0].ProcedureGroupCode);
                }
                else {
                    $('#<%:hdnEntryProcedureGroupID.ClientID %>').val('');
                    $('#<%:txtProcedureGroupName.ClientID %>').val('');
                    $('#<%:txtProcedureGroupCode.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion


        $('#<%=rblIsHasInfectiousDisease.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trInfectiousInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trInfectiousInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#<%=rblIsHasComorbidities.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trComorbiditiesInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trComorbiditiesInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#leftPageNavPanel ul li').first().click();

        $('#lblPhysicianNoteID').removeClass('lblLink');
    });

    function onCboGCInfectiousDiseaseChanged(s) {
        var cboGCInfectiousDisease = s.GetValue();

        if (cboGCInfectiousDisease != Constant.InfectiousDisease.OTHERS) {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOtherInfectiousDisease.ClientID %>').val('');
        }
        else {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').removeAttr('readonly');
        }
    }

    function onCboGCComorbiditiesChanged(s) {
        var cboGCComorbidities = s.GetValue();

        if (cboGCComorbidities != Constant.Comorbidities.OTHERS) {
            $('#<%=txtOtherComorbidities.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOtherComorbidities.ClientID %>').val('');
        }
        else {
            $('#<%=txtOtherComorbidities.ClientID %>').removeAttr('readonly');
        }
    }

    function onGetScheduleFilterExpression() {
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var scheduleDate = $('#<%=txtScheduleDate.ClientID %>').val();
        var scheduleDateInDatePicker = Methods.getDatePickerDate(scheduleDate);
        var scheduleDateFormatString = Methods.dateToString(scheduleDateInDatePicker);
        var filterExpression = "VisitID = " + visitID + " AND ScheduleDate = '" + scheduleDateFormatString + "' AND GCScheduleStatus != 'X449^03' AND IsDeleted = 0";
        return filterExpression;
    }

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        var filterExpression = onGetScheduleFilterExpression();
        Methods.getObject('GetRoomScheduleList', filterExpression, function (result) {
            if (result != null) {
                errMessage.text = "Masih ada outstanding jadwal kamar operasi, lanjutkan proses pembuatan jadwal lagi?";
                resultFinal = false;
            }
        });
        return resultFinal;
    }
</script>
<style type="text/css">
    
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnFromHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Jadwal Kamar Operasi" class="w3-hover-red">Jadwal Operasi</li>
                        <li contentid="divPage2" title="Permohonan Khusus" class="w3-hover-red">Permohonan Khusus</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Asal Pasien") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblSourcePatientType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Rawat Inap" Value="1" Selected="True" />
                                    <asp:ListItem Text=" Rawat Darurat" Value="2" />
                                    <asp:ListItem Text=" Penunjang Medis" Value="3" />
                                    <asp:ListItem Text=" Rawat Jalan" Value="0" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblNoReg">
                                    <%:GetLabel("No. Registrasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRegistrationNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblMedicalNo">
                                    <%:GetLabel("No. Rekam Medis")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMedicalNo" Width="100px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblPatientInfo">
                                    <%:GetLabel("Pasien")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientInfo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblPatientLocation">
                                    <%:GetLabel("Lokasi Pasien")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientLocation" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server">
                                    <%=GetLabel("Estimasi Lama Operasi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" />
                                menit
                            </td>
                            <td style="padding-left: 5px">
                                <asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Jadwal Operasi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsEmergency" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Emergency" Value="1" />
                                    <asp:ListItem Text=" Elektif" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Rencana")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                        <td />
                                        <td>
                                            <asp:CheckBox ID="chkIsNextVisit" Width="200px" runat="server" Text=" Kunjungan Berikutnya"
                                                Enabled="false" />
                                        </td>
                                        <td style="display: none">
                                            <div id="divScheduleInfo" runat="server" style="display: none">
                                                <input type="button" class="btnSchedule w3-btn w3-hover-blue" value="Info Jadwal"
                                                    style="background-color: Green; color: White; width: 100px" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trNextVisit" runat="server" style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Kunjungan Berikutnya") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblNextVisitType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" ODS" Value="1" />
                                    <asp:ListItem Text=" Rawat Inap" Value="2" Enabled="false" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblRoom">
                                    <%:GetLabel("Ruang Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="">
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblProcedureGroupCode">
                                    <%:GetLabel("Jenis Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" value="" id="hdnEntryProcedureGroupID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProcedureGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="">
                                            <asp:TextBox ID="txtProcedureGroupName" Width="100%" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Riwayat Penyakit Infeksi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasInfectiousDisease" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trInfectiousInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Penyakit Infeksi")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboGCInfectiousDisease" ClientInstanceName="cboGCInfectiousDisease"
                                                Width="99%" ToolTip="Tipe Penyakit Infeksi">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCInfectiousDiseaseChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="tdLabel" style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherInfectiousDisease" CssClass="txtOtherInfectiousDisease"
                                                runat="server" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Memiliki Komorbid") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasComorbidities" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trComorbiditiesInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Komorbid")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboGCComorbidities" ClientInstanceName="cboGCComorbidities"
                                                Width="99%" ToolTip="Tipe Komorbid">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCComorbiditiesChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="tdLabel" style="padding-left: 5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherComorbidities" CssClass="txtOtherComorbidities" runat="server"
                                                Width="100%" ReadOnly />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingSpecificItem" Width="180px" runat="server" Text=" Penggunaan Alat Tertentu" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Order") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="250px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
</div>
