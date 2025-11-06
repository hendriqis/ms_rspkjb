<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicEntryPopupCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ParamedicEntryPopupCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_paramedic3entrypopupctl">
        $('#<%=btnSetClinicParamedic.ClientID %>').die('click');
        $('#<%=btnSetClinicParamedic.ClientID %>').live('click', function () {
            var oServiceUnitCode = $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').val();
            var oParamedicCode = $('#<%:txtPhysicianCodeCtlPopup.ClientID %>').val();
            if (oServiceUnitCode != '' && oParamedicCode != '') {
                onAfterSaveParamedicFromCtlSchedule(oServiceUnitCode, oParamedicCode);
                pcRightPanelContent.Hide();
            }
            else {
                showToast('Harap Lengkapi Isian Klinik dan Dokter / Tenaga Medis');
            }
        });

    //#region Service Unit
    function getServiceUnitFilterFilterExpressionCtlPopup() {
        var filterExpression = '';
        if ($('#<%:hdnDeptIDCtlPopup.ClientID %>').val() == Constant.Facility.DIAGNOSTIC) {
            filterExpression = '<%:filterExpressionOtherMedicalDiagnosticCtlPopup %>';
        }

        if ($('#<%:hdnDiagnosticTypeCtlPopup.ClientID %>').val() == "0") {
            filterExpression = "IsLaboratoryUnit = 1";
        }
        else if ($('#<%:hdnDiagnosticTypeCtlPopup.ClientID %>').val() == "1") {
            filterExpression = 'HealthcareServiceUnitID = ' + $('#<%:hdnHealthcareServiceUnitIDCtlPopup.ClientID %>').val();
        }

        if (filterExpression != '') {
            filterExpression += ' AND ';
        }
        filterExpression += 'IsUsingRegistration = 1';
        return filterExpression;
    }

    $('#<%:lblServiceUnitCtlPopup.ClientID %>.lblLink').live('click', function () {
        var parameter = "<%:GetServiceUnitUserParameter() %>" + getServiceUnitFilterFilterExpressionCtlPopup();
        openSearchDialog('serviceunitroleuser', parameter, function (value) {
            $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').val(value);
            onTxtClinicCodeCtlPopupChanged(value);
        });
    });

    $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').live('change', function () {
        onTxtClinicCodeCtlPopupChanged($(this).val());
    });

    function onTxtClinicCodeCtlPopupChanged(value) {
        var filterExpression = getServiceUnitFilterFilterExpressionCtlPopup();
        if (filterExpression != '')
            filterExpression += ' AND ';
        filterExpression += "ServiceUnitCode = '" + value + "'";
        var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
        Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
            if (result != null) {
                $('#<%:hdnHealthcareServiceUnitIDCtlPopup.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitNameCtlPopup.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%:hdnHealthcareServiceUnitIDCtlPopup.ClientID %>').val('');
                $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').val('');
                $('#<%:txtServiceUnitNameCtlPopup.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Physician
    function onGetPhysicianFilterExpressionCtlPopup() {
        var date = new Date();
        var hourInString = date.getHours().toString();
        var minutesInString = date.getMinutes().toString();

        if (hourInString.length == 1) {
            hourInString = '0' + hourInString;
        }

        if (minutesInString.length == 1) {
            minutesInString = '0' + minutesInString;
        }
        
        //var formattedTime = hourInString + ":" + minutesInString;
        var formattedTime = $('#<%:txtRegistrationHourCtlPopup.ClientID %>').val();
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitIDCtlPopup.ClientID %>').val();
        var registrationDate = $('#<%:txtRegistrationDateCtlPopup.ClientID %>').val();
        var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
        var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
        var daynumber = "<%:GetDayNumber() %>";

        filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + serviceUnitID + "') AND IsDeleted = 0 AND (IsHasPhysicianRole = 1) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + serviceUnitID + "' AND DayNumber = '" + daynumber + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)) UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + serviceUnitID + "' AND ScheduleDate = '" + registrationDateFormatString + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)))";
        return filterExpression;
    }

    $('#<%:lblPhysicianCtlPopup.ClientID %>.lblLink').live('click', function () {
        var hsuCode = $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').val();
        if (hsuCode != '') {
            openSearchDialog('paramedic', onGetPhysicianFilterExpressionCtlPopup(), function (value) {
                $('#<%:txtPhysicianCodeCtlPopup.ClientID %>').val(value);
                onTxtPhysicianCodeCtlPopupChanged(value);
            });
        }
        else {
            showToast('Harap Pilih Klinik Terlebih dahulu');
        }
    });

    $('#<%:txtPhysicianCodeCtlPopup.ClientID %>').live('change', function () {
        var hsuCode = $('#<%:txtServiceUnitCodeCtlPopup.ClientID %>').val();
        if (hsuCode != '') {
            onTxtPhysicianCodeCtlPopupChanged($(this).val());
        }
        else {
            showToast('Harap Pilih Klinik Terlebih dahulu');
        }
    });

    function onTxtPhysicianCodeCtlPopupChanged(value) {
        var filterExpression = onGetPhysicianFilterExpressionCtlPopup() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                var registrationDate = $('#<%:txtRegistrationDateCtlPopup.ClientID %>').val();
                var registrationDateInDatePicker = Methods.getDatePickerDate(registrationDate);
                var registrationDateFormatString = Methods.dateToString(registrationDateInDatePicker);
                var filterExpressionLeave = "IsDeleted = 0 AND ('" + registrationDateFormatString + "' BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                    if (resultLeave == null) {
                        $('#<%:hdnParamedicIDCtlPopup.ClientID %>').val(result.ParamedicID);
                        $('#<%:txtPhysicianNameCtlPopup.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%:hdnParamedicIDCtlPopup.ClientID %>').val('');
                        $('#<%:txtPhysicianCodeCtlPopup.ClientID %>').val('');
                        $('#<%:txtPhysicianNameCtlPopup.ClientID %>').val('');
                        var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                        showToast("INFORMASI", info);
                    }
                });
            }
            else {
                $('#<%:hdnParamedicIDCtlPopup.ClientID %>').val('');
                $('#<%:txtPhysicianCodeCtlPopup.ClientID %>').val('');
                $('#<%:txtPhysicianNameCtlPopup.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnSetClinicParamedic">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
                <%=GetLabel("Set")%></div>
        </li>
    </ul>
</div>
<input type="hidden" runat="server" id="hdnParam" />
<input type="hidden" runat="server" id="hdnDeptIDCtlPopup" />
<input type="hidden" runat="server" id="hdnDiagnosticTypeCtlPopup" />
<div style="height: 100px">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 0px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 18%" />
                        <col style="width: 135px" />
                        <col style="width: 10px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDateCtlPopup" Width="120px" Style="text-align: center"
                                ReadOnly="true" runat="server" />
                        </td>
                        <td style="padding-left: 30px;">
                            <%:GetLabel("Jam")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationHourCtlPopup" CssClass="time" runat="server" Width="120px"
                                Style="text-align: center" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trServiceUnitCtlPopup" runat="server">
            <td style="padding: 0px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 18%" />
                        <col style="width: 200px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblServiceUnitCtlPopup">
                                <%:GetServiceUnitLabel()%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnHealthcareServiceUnitIDCtlPopup" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 75px" />
                                    <col style="width: 500px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitCodeCtlPopup" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceUnitNameCtlPopup" Width="100%" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trPhysician" runat="server">
            <td style="padding: 0px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 18%" />
                        <col style="width: 200px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblPhysicianCtlPopup">
                                <%:GetLabel("Dokter / Tenaga Medis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnParamedicIDCtlPopup" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 75px" />
                                    <col style="width: 500px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCodeCtlPopup" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianNameCtlPopup" Width="100%" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
