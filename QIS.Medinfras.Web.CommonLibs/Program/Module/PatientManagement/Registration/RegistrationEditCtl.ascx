<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationEditCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_registrationeditctl">
    $(function () {
        if ($('#<%:hdnGender.ClientID %>').val() == '<%:GetGenderFemale() %>') {
            $('#<%:chkIsPregnantCtl.ClientID %>').removeAttr("disabled");
            $('#<%:chkIsParturitionEdit.ClientID %>').removeAttr("disabled");
        }
        else {
            $('#<%:chkIsPregnantCtl.ClientID %>').attr("disabled", true);
            $('#<%:chkIsParturitionEdit.ClientID %>').attr("disabled", true);
        }
    });

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtPhysicianCode.ClientID %>').val();
        return result;
    }

    //#region Registration No
    function getMotherRegistrationNoFilterExpressionCtl() {
        var filterExpression = "<%:OnGetMotherRegistrationNoFilterExpression() %>";
        return filterExpression;
    }

    $('#lblMotherRegNoCtl.lblLink').live('click', function () {
        openSearchDialog('consultvisit', getMotherRegistrationNoFilterExpressionCtl(), function (value) {
            $('#<%=hdnMotherVisitIDCtl.ClientID %>').val(value);
            onTxtMotherVisitIDChangedCtl(value);
        });
    });

    function onTxtMotherVisitIDChangedCtl(value) {
        var filterExpression = getMotherRegistrationNoFilterExpressionCtl() + " AND VisitID = '" + value + "'";
        Methods.getObject('GetvConsultVisitList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnMotherVisitIDCtl.ClientID %>').val(result.VisitID);
                $('#<%:hdnMotherMRNCtl.ClientID %>').val(result.MRN);
                $('#<%:hdnMotherNameCtl.ClientID %>').val(result.PatientName);
                $('#<%:txtMotherRegNoCtl.ClientID %>').val(result.RegistrationNo);
            }
            else {
                $('#<%:hdnMotherNameCtl.ClientID %>').val('');
                $('#<%:hdnMotherVisitIDCtl.ClientID %>').val('');
                $('#<%:hdnMotherMRNCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Partus & New Born
    $('#<%:chkIsParturitionEdit.ClientID %>').live('change', function () {
        $chkIsNewBorn = $('#<%:chkIsNewBornCtl.ClientID %>');
        if ($(this).is(':checked')) {
            $chkIsNewBorn.attr("disabled", true);
            $('#<%:hdnMotherMRNCtl %>').val("");
            $('#<%:txtMotherRegNoCtl %>').val("");
        }
        else {
            $chkIsNewBorn.removeAttr("disabled");

        }
    });

    $('#<%:chkIsNewBornCtl.ClientID %>').live('change', function () {
        $chkIsParturition = $('#<%:chkIsParturitionEdit.ClientID %>');
        $chkIsPregnantCtl = $('#<%:chkIsPregnantCtl.ClientID %>');
        if ($('#<%:hdnGender.ClientID %>').val() == '<%:GetGenderFemale() %>') {
            if ($(this).is(':checked')) {
                $chkIsParturition.attr("disabled", true);
                $chkIsPregnantCtl.attr("disabled", true);
                $('#<%:chkIsParturitionEdit.ClientID %>').prop("checked", false);
                $('#<%:chkIsPregnantCtl.ClientID %>').prop("checked", false);
                $('#<%=trMotherRegNoCtl.ClientID %>').show();
            }
            else {
                $chkIsParturition.removeAttr("disabled");
                $chkIsPregnantCtl.removeAttr("disabled");
                $('#<%:hdnMotherMRNCtl.ClientID %>').val("");
                $('#<%:txtMotherRegNoCtl.ClientID %>').val("");
                $('#<%=trMotherRegNoCtl.ClientID %>').hide();
            }
        }
        else {
            if ($(this).is(':checked')) {
                $('#<%:chkIsParturitionEdit.ClientID %>').prop("checked", false);
                $('#<%:chkIsPregnantCtl.ClientID %>').prop("checked", false);
                $('#<%=trMotherRegNoCtl.ClientID %>').show();
            }
            else {

                $('#<%:hdnMotherMRNCtl.ClientID %>').val("");
                $('#<%:txtMotherRegNoCtl.ClientID %>').val("");
                $('#<%=trMotherRegNoCtl.ClientID %>').hide();
            }
        }
    });

    $('#<%:chkIsPregnantCtl.ClientID %>').live('change', function () {
        $chkIsPregnantCtl = $('#<%:chkIsPregnantCtl.ClientID %>');
        $chkIsNewBornCtl = $('#<%:chkIsNewBornCtl.ClientID %>');
        $chkIsParturition = $('#<%:chkIsParturitionEdit.ClientID %>');
        if ($(this).is(':checked')) {
            $chkIsNewBornCtl.attr("disabled", true);
            $('#<%:chkIsNewBornCtl.ClientID %>').prop("checked", false);
        }
        else {
            $chkIsNewBornCtl.removeAttr("disabled");

        }
    });
    //#endregion

    function onBeforeSaveRecord(errMessage) {
        var visitID = $('#<%:hdnVisitIDCtlPopUp.ClientID %>').val();
        var ParamedicIDORI= $('#<%:hdnPhysicianIDORI.ClientID %>').val();
        var ParamedicIDNow = $('#<%:hdnPhysicianID.ClientID %>').val();
        var isValidCharges = true;

        if (ParamedicIDORI != ParamedicIDNow) {
            var filterExpression = "VisitID = '" + visitID + "' AND IsAutoTransaction = 0 AND GCTransactionStatus != 'X121^999'";
            Methods.getListObject('GetPatientChargesHdList', filterExpression, function (result) {
                for (i = 0; i < result.length; i++) {
                    var filterExpressionDt = "TransactionID = '" + result[i].TransactionID + "' AND ParamedicID = '" + ParamedicIDORI + "' AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != 'X121^999'";
                    filterExpressionDt += " AND ItemID NOT IN (SELECT ItemID FROM ItemMaster WHERE GCItemType IN ('" + Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS + "','" + Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN + "'))";
                    Methods.getObject('GetPatientChargesDtList', filterExpressionDt, function (resultDt) {
                        if (resultDt != null) {
                            isValidCharges = false;
                        }
                    });
                }
            });
        }

        if (!isValidCharges) {
            errMessage.text = 'showconfirm|Masih terdapat transaksi dari dokter saat ini. Lanjutkan ubah dokter ?';
            return false;
        }
        else {
            return true;
        }
    }

    //#region Referral Description
    function getReferralDescriptionFilterExpression() {
        var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
        return filterExpression;
    }

    function getReferralParamedicFilterExpression() {
        var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
        return filterExpression;
    }

    $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
        var referral = cboReferral.GetValue();
        if (referral == Constant.ReferrerGroup.DOKTERRS) {
            openSearchDialog('referrerparamedic', getReferralParamedicFilterExpression(), function (value) {
                $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                onTxtReferralDescriptionCodeChanged(value);
            });
        } else {
            openSearchDialog('referrer', getReferralDescriptionFilterExpression(), function (value) {
                $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                onTxtReferralDescriptionCodeChanged(value);
            });
        }
    });

    $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
        onTxtReferralDescriptionCodeChanged($(this).val());
    });

    function onTxtReferralDescriptionCodeChanged(value) {
        var filterExpression = "";
        var referral = cboReferral.GetValue();
        if (referral == Constant.ReferrerGroup.DOKTERRS) {
            filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                }
                else {
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                }
            });
        } else {
            filterExpression = getReferralDescriptionFilterExpression() + " AND CommCode = '" + value + "'";
            Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                }
                else {
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion

    //#region Room

    function getRoomFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var deptID = $('#<%:hdnDepartmentID.ClientID %>').val();
        var filterExpression = '';

        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
        }

        if (filterExpression != '') {
            filterExpression += " AND ";
        }
        filterExpression += "IsDeleted = 0 AND DepartmentID = '" + deptID + "'";

        return filterExpression;
    }

    $('#<%:lblRoomCtl.ClientID %>.lblLink').live('click', function () {
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
                    $('#<%:hdnRoomIDCtl.ClientID %>').val(result[0].RoomID);
                    $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                    $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                }
                else {
                    $('#<%:hdnRoomIDCtl.ClientID %>').val('');
                    $('#<%:txtRoomCode.ClientID %>').val('');
                    $('#<%:txtRoomName.ClientID %>').val('');
                }
            });
        } else {
            $('#<%:hdnRoomIDCtl.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').val('');
            $('#<%:txtRoomName.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var filterExpression = "<%:OnGetParamedicFilterExpression() %>";
        var isCheckedFilter = $('#<%=chkParamedicHasSchedulePopUpCtl.ClientID %>').is(":checked");

        var date = Methods.getDatePickerDate($('#<%:hdnRegistrationDate.ClientID %>').val());
        var formattedTime = $('#<%:hdnRegistrationHour.ClientID %>').val();
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var registrationDateFormatString = Methods.dateToString(date);
        var daynumber = "<%:GetDayNumber() %>";

        if (isCheckedFilter) {
            filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = '" + serviceUnitID + "' AND DayNumber = '" + daynumber + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)) UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = '" + serviceUnitID + "' AND ScheduleDate = '" + registrationDateFormatString + "' AND (('" + formattedTime + "' BETWEEN StartTime1 AND EndTime1) OR ('" + formattedTime + "' BETWEEN StartTime2 AND EndTime2) OR ('" + formattedTime + "' BETWEEN StartTime3 AND EndTime3) OR ('" + formattedTime + "' BETWEEN StartTime4 AND EndTime4) OR ('" + formattedTime + "' BETWEEN StartTime5 AND EndTime5)))";
        }
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
                if ($('#<%=hdnDepartmentID.ClientID %>').val() == "OUTPATIENT") {
                    var date = Methods.getDatePickerDate($('#<%:hdnRegistrationDate.ClientID %>').val());
                    var registrationDateFormatString = Methods.dateToString(date);
                    var filterExpressionLeave = "IsDeleted = 0 AND ('" + registrationDateFormatString + "' BETWEEN StartDate AND EndDate ) AND ParamedicCode = '" + value + "'";
                    Methods.getObject('GetvParamedicLeaveScheduleList', filterExpressionLeave, function (resultLeave) {
                        if (resultLeave == null) {
                            cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                            $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                            $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                        }
                        else {
                            var info = result.ParamedicName + " Sedang Dalam Masa Cuti";
                            showToast("INFORMASI", info);

                            cboRegistrationEditSpecialty.SetValue('');
                            $('#<%=hdnPhysicianID.ClientID %>').val('');
                            $('#<%=txtPhysicianCode.ClientID %>').val('');
                            $('#<%=txtPhysicianName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
            }
            else {
                cboRegistrationEditSpecialty.SetValue('');
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Visit Type
    function onGetVisitTypeCtlFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == '')
            serviceUnitID = '0';
        var paramedicID = $('#<%:hdnPhysicianID.ClientID %>').val();
        if (paramedicID == '')
            paramedicID = '0';
        var filterExpression = serviceUnitID + ';' + paramedicID + ';';
        return filterExpression;
    }

    $('#lblVisitTypeCtl.lblLink').live('click', function () {
        openSearchDialog('paramedicvisittype', onGetVisitTypeCtlFilterExpression(), function (value) {
            $('#<%:txtVisitTypeCtlCode.ClientID %>').val(value);
            onTxtVisitTypeCtlCodeChanged(value);
        });
    });

    $('#<%:txtVisitTypeCtlCode.ClientID %>').live('change', function () {
        onTxtVisitTypeCtlCodeChanged($(this).val());
    });

    function onTxtVisitTypeCtlCodeChanged(value) {
        var filterExpression = onGetVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnVisitTypeCtlID.ClientID %>').val(result.VisitTypeID);
                $('#<%:txtVisitTypeCtlName.ClientID %>').val(result.VisitTypeName);
            }
            else {
                $('#<%:hdnVisitTypeCtlID.ClientID %>').val('');
                $('#<%:txtVisitTypeCtlCode.ClientID %>').val('');
                $('#<%:txtVisitTypeCtlName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Diagnose
    $('#<%:lblDiagnoseRegEditCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseCodeRegEditCtl.ClientID %>').val(value);
            ontxtDiagnoseCodeRegEditCtlChanged(value);
        });
    });

    $('#<%=txtDiagnoseCodeRegEditCtl.ClientID %>').live('change', function () {
        ontxtDiagnoseCodeRegEditCtlChanged($(this).val());
    });

    function ontxtDiagnoseCodeRegEditCtlChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseNameRegEditCtl.ClientID %>').val(result.DiagnoseName);
                $('#<%=txtDiagnoseTextRegEditCtl.ClientID %>').val(result.DiagnoseName + ' (' + result.DiagnoseID + ')');
                
            }
            else {
                $('#<%=txtDiagnoseCodeRegEditCtl.ClientID %>').val('');
                $('#<%=txtDiagnoseNameRegEditCtl.ClientID %>').val('');
                $('#<%=txtDiagnoseTextRegEditCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%:chkParamedicHasSchedulePopUpCtl.ClientID %>').live('change', function () {
        $('#<%:hdnPhysicianID.ClientID %>').val('');
        $('#<%:txtPhysicianCode.ClientID %>').val('');
        $('#<%:txtPhysicianName.ClientID %>').val('');
    });
</script>
<div style="height: 350px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnPhysicianIDORI" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnVisitIDCtlPopUp" value="" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <input type="hidden" runat="server" id="hdnGender" value="" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <input type="hidden" id="hdnIsQueueChangeAfterEdit" value="0" runat="server" />
    <input type="hidden" id="hdnIsParamedicInRegistrationUseScheduleCtl" value="0" runat="server" />
    <input type="hidden" id="hdnRegistrationDate" value="0" runat="server" />
    <input type="hidden" id="hdnRegistrationHour" value="0" runat="server" />
    <input type="hidden" id="hdnIsOutpatientUsingRoom" value="0" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationID" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trMotherRegNoCtl" style="display: none" runat="server">
            <td class="tdLabel">
                <div style="position: relative;">
                    <label class="lblLink lblKey" id="lblMotherRegNoCtl">
                        <%:GetLabel("No. Registrasi Ibu")%></label></div>
            </td>
            <td>
                <input type="hidden" id="hdnMotherVisitIDCtl" value="" runat="server" />
                <input type="hidden" id="hdnMotherMRNCtl" value="" runat="server" />
                <input type="hidden" id="hdnMotherNameCtl" value="" runat="server" />
                <asp:TextBox ID="txtMotherRegNoCtl" Width="175px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>

        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Klinik")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trRoomCtl" runat="server">
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblRoomCtl" runat="server">
                    <%=GetLabel("Kamar")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnRoomIDCtl" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
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
                        <td>
                            <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trParamedicHasScheduleCtl" runat="server">
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkParamedicHasSchedulePopUpCtl" runat="server" /><%:GetLabel("Tampilkan Hanya Dokter Yang Punya Jadwal")%>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblPatientVisitPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Spesialisasi")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboRegistrationEditSpecialty" ClientInstanceName="cboRegistrationEditSpecialty"
                    Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblVisitTypeCtl">
                    <%=GetLabel("Jenis Kunjungan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnVisitTypeCtlID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtVisitTypeCtlCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitTypeCtlName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Rujukan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" runat="server" id="lblReferralDescription">
                    <%:GetLabel("Deskripsi Rujukan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblDiagnoseRegEditCtl" runat="server">
                    <%=GetLabel("Diagnosa")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDiagnoseCodeRegEditCtl" Width="100%" runat="server" />
                        </td>
                        <td />
                        <td>
                            <asp:TextBox ID="txtDiagnoseNameRegEditCtl" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Diagnosa Text")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtDiagnoseTextRegEditCtl" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsPregnantCtl" runat="server" /><%:GetLabel("Hamil")%>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsParturitionEdit" runat="server" Visible="false" Text="Partus" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsNewBornCtl" runat="server" Visible="false" Text="Bayi Baru Lahir" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsVisitorRestriction" runat="server" Visible="false" Text="Tidak mau dikunjungi (Rahasia)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsNeedPastoralCare" runat="server" Text="Pelayanan Rohani" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsFastTrack" runat="server" Text="Fast Track" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
