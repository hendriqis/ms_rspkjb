<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAccompanyEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientAccompanyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        $('#<%=chkIsFamily.ClientID %>').change(function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#lblFamily').attr('class', 'lblLink');
                $('#<%=txtContactName.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtFamily.ClientID %>').removeAttr('readonly');
                cboRelationship.SetValue('');
                cboRelationship.SetEnabled(false);

            }
            else {
                $('#lblFamily').attr('class', 'lblDisabled');
                $('#<%=txtContactName.ClientID %>').removeAttr('readonly');
                $('#<%=txtFamily.ClientID %>').attr('readonly', 'readonly');
                cboRelationship.SetEnabled(true);
                $('#<%=txtFamily.ClientID %>').val('');
                $('#<%=hdnFamilyID.ClientID %>').val('');
                $('#<%=txtContactName.ClientID %>').val('');
                cboRelationship.SetValue('');
                $('#<%=txtTelephoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
            }
        });
    });

    //#region Popup Search
    //#region Family
    function onGetEmergencyContactFamilyFilterExpression() {
        var filterExpression = "MRN = " + $('#<%=hdnMRN.ClientID %>').val() + " AND FamilyID NOT IN (SELECT FamilyID FROM PatientAccompany WHERE FamilyID IS NOT NULL AND IsDeleted = 0 AND RegistrationID =" + $('#<%=hdnRegistrationID.ClientID %>').val() + ")";
        return filterExpression;
    }

    $('#lblFamily.lblLink').die('click');
    $('#lblFamily.lblLink').live('click', function () {
        openSearchDialog('patientFamily', onGetEmergencyContactFamilyFilterExpression(), function (value) {
            $('#<%=hdnFamilyID.ClientID %>').val(value);
            onTxtFamilyChanged(value);
        });
    });

    $('#<%:txtFamily.ClientID %>').live('change', function () {
        onTxtFamilyChangedMRN($(this).val());
    });

    function onTxtFamilyChanged(value) {
        var filterExpression = onGetEmergencyContactFamilyFilterExpression() + " AND FamilyID = " + value;
        Methods.getObject('GetvPatientFamilyList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtFamily.ClientID %>').val(result.FamilyMedicalNo);
                $('#<%=txtContactName.ClientID %>').val(result.FullName);
                cboRelationship.SetValue(result.GCFamilyRelation);
                $('#<%=txtTelephoneNo.ClientID %>').val(result.PhoneNo1);
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val(result.Email);
            }
            else {
                $('#<%=hdnFamilyID.ClientID %>').val('');
                $('#<%=txtFamily.ClientID %>').val('');
                $('#<%=txtContactName.ClientID %>').val('');
                cboRelationship.SetValue('');
                $('#<%=txtTelephoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');

            }
        });
    }

    function onTxtFamilyChangedMRN(value) {
        var filterExpression = onGetEmergencyContactFamilyFilterExpression() + " AND FamilyMedicalNo = '" + value + "'";
        Methods.getObject('GetvPatientFamilyList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtFamily.ClientID %>').val(result.FamilyMedicalNo);
                $('#<%=txtContactName.ClientID %>').val(result.FullName);
                cboRelationship.SetValue(result.GCFamilyRelation);
                $('#<%=txtTelephoneNo.ClientID %>').val(result.PhoneNo1);
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val(result.Email);
            }
            else {
                $('#<%=hdnFamilyID.ClientID %>').val('');
                $('#<%=txtFamily.ClientID %>').val('');
                $('#<%=txtContactName.ClientID %>').val('');
                cboRelationship.SetValue('');
                $('#<%=txtTelephoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');

            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddDataPatientAccompanyCtl').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
        $('#<%=chkIsFamily.ClientID %>').prop('checked', false);
        $('#lblFamily').attr('class', 'lblDisabled');
        $('#<%=txtFamily.ClientID %>').attr('readonly', 'readonly');
        cboRelationship.SetValue('');
        $('#<%:txtPatientAccompanyNo.ClientID %>').val('');
        $('#<%:txtFamily.ClientID %>').val('');
        $('#<%:txtContactName.ClientID %>').val('');
        $('#<%:txtTelephoneNo.ClientID %>').val('');
        $('#<%:txtMobilePhone.ClientID %>').val('');
        $('#<%:txtEmail.ClientID %>').val('');
        $('#<%:txtRemarks.ClientID %>').val('');
        $('#<%:hdnClassID.ClientID %>').val('');
        $('#<%:txtClassCode.ClientID %>').val('');
        $('#<%:txtClassName.ClientID %>').val('');
        $('#<%:txtServiceUnitCode.ClientID %>').val('');
        $('#<%:txtServiceUnitName.ClientID %>').val('');
        $('#<%:hdnRoomID.ClientID %>').val('');
        $('#<%:txtRoomCode.ClientID %>').val('');
        $('#<%:txtRoomName.ClientID %>').val('');
        $('#<%:hdnBedID.ClientID %>').val('');
        $('#<%:txtBedCode.ClientID %>').val('');
        $('#<%:hdnExtensionNo.ClientID %>').val('');
        $('#<%:hdnChargeClassID.ClientID %>').val('');
        $('#<%:txtChargeClassCode.ClientID %>').val('');
        $('#<%:txtChargeClassName.ClientID %>').val('');
        $('#containerPopupEntryData').hide();
        $('#containerPopupEntryData').show();
    });

    //#region Save and Cancel
    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupCancel').live('click', function () {
        cboRelationship.SetValue('');
        $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
        $('#<%=chkIsFamily.ClientID %>').prop('checked', false);
        $('#lblFamily').attr('class', 'lblDisabled');
        $('#<%=txtFamily.ClientID %>').attr('readonly', 'readonly');
        $('#<%:txtPatientAccompanyNo.ClientID %>').val('');
        $('#<%:txtFamily.ClientID %>').val('');
        $('#<%:txtContactName.ClientID %>').val('');
        $('#<%:txtTelephoneNo.ClientID %>').val('');
        $('#<%:txtMobilePhone.ClientID %>').val('');
        $('#<%:txtEmail.ClientID %>').val('');
        $('#<%:txtRemarks.ClientID %>').val('');
        $('#<%:hdnClassID.ClientID %>').val('');
        $('#<%:txtClassCode.ClientID %>').val('');
        $('#<%:txtClassName.ClientID %>').val('');
        $('#<%:txtServiceUnitCode.ClientID %>').val('');
        $('#<%:txtServiceUnitName.ClientID %>').val('');
        $('#<%:hdnRoomID.ClientID %>').val('');
        $('#<%:txtRoomCode.ClientID %>').val('');
        $('#<%:txtRoomName.ClientID %>').val('');
        $('#<%:hdnBedID.ClientID %>').val('');
        $('#<%:txtBedCode.ClientID %>').val('');
        $('#<%:hdnExtensionNo.ClientID %>').val('');
        $('#<%:hdnChargeClassID.ClientID %>').val('');
        $('#<%:txtChargeClassCode.ClientID %>').val('');
        $('#<%:txtChargeClassName.ClientID %>').val('');
        $('#containerPopupEntryData').hide();
        $('#containerPopupEntryData').hide();
    });
    //#endregion

    //#region Edit Delete
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                cboRelationship.SetValue('');
                $('#<%=chkIsFamily.ClientID %>').prop('checked', false);
                $('#lblFamily').attr('class', 'lblDisabled');
                $('#<%=txtFamily.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtPatientAccompanyNo.ClientID %>').val('');
                $('#<%:txtFamily.ClientID %>').val('');
                $('#<%:txtContactName.ClientID %>').val('');
                $('#<%:txtTelephoneNo.ClientID %>').val('');
                $('#<%:txtMobilePhone.ClientID %>').val('');
                $('#<%:txtEmail.ClientID %>').val('');
                $('#<%:txtRemarks.ClientID %>').val('');
                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
                $('#<%:hdnExtensionNo.ClientID %>').val('');
                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        hideLoadingPanel();
    }

    //#region Class Care
    function getClassCareFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = 'IsDeleted = 0';
        if (serviceUnitID != '')
            filterExpression = 'ClassID IN (SELECT ClassID FROM vServiceUnitRoom WHERE HealthcareServiceUnitID = ' + serviceUnitID + ' AND IsDeleted = 0) AND IsDeleted = 0';
        return filterExpression;
    }

    $('#<%:lblClass.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', getClassCareFilterExpression(), function (value) {
            $('#<%:txtClassCode.ClientID %>').val(value);
            onTxtClassCodeChanged(value);
        });
    });

    $('#<%:txtClassCode.ClientID %>').live('change', function () {
        onTxtClassCodeChanged($(this).val());
    });

    function onTxtClassCodeChanged(value) {
        var filterExpression = getClassCareFilterExpression() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%:txtClassName.ClientID %>').val(result.ClassName);

                if (result.IsUsedInChargeClass) {
                    $('#<%:hdnChargeClassID.ClientID %>').val(result.ClassID);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result.ClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%:hdnChargeClassID.ClientID %>').val('');
                    $('#<%:txtChargeClassCode.ClientID %>').val('');
                    $('#<%:txtChargeClassName.ClientID %>').val('');
                }
            }
            else {
                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');
            }
            if ($('#<%:hdnHealthcareServiceUnitID.ClientID %>').val() != '') {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
                $('#<%:hdnIsServiceUnitHasParamedic.ClientID %>').val('0');
                $('#<%:hdnIsServiceUnitHasVisitType.ClientID %>').val('0');
            }
            $('#<%:hdnRoomID.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').val('');
            $('#<%:txtRoomName.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Service Unit
    var serviceUnitUserCount = parseInt('<%:serviceUnitUserCount %>');
    function getServiceUnitFilterFilterExpression() {
        var filterExpression = '';
        var classID = $('#<%:hdnClassID.ClientID %>').val();

        if (classID != '')
            filterExpression = 'HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';

        if (filterExpression != '')
            filterExpression += ' AND ';
        filterExpression += 'IsUsingRegistration = 1';
        return filterExpression;
    }

    $('#<%:lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        var parameter = "<%:GetServiceUnitUserParameter() %>" + getServiceUnitFilterFilterExpression();
        openSearchDialog('serviceunitroleuser', parameter, function (value) {
            $('#<%:txtServiceUnitCode.ClientID %>').val(value);
            onTxtClinicCodeChanged(value);
        });
    });

    $('#<%:txtServiceUnitCode.ClientID %>').live('change', function () {
        onTxtClinicCodeChanged($(this).val());
    });

    function onTxtClinicCodeChanged(value) {
        var filterExpression = getServiceUnitFilterFilterExpression();
        if (filterExpression != '')
            filterExpression += ' AND ';
        filterExpression += "ServiceUnitCode = '" + value + "'";
        var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
        Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
            if (result != null) {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);

                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
                $('#<%:hdnExtensionNo.ClientID %>').val('');
            }
            else {
                $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtServiceUnitCode.ClientID %>').val('');
                $('#<%:txtServiceUnitName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Room
    function getRoomFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var classID = $('#<%:hdnClassID.ClientID %>').val();
        var filterExpression = '';
        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
        }
        if (classID != '0' && classID != '') {
            if (filterExpression != '') filterExpression += " AND ";
            filterExpression += 'ClassID = ' + classID;
        }
        if (filterExpression != '') filterExpression += " AND ";
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
        Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
            if (result.length == 1) {
                $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                if ($('#<%:hdnClassID.ClientID %>').val() == '') {
                    $('#<%:hdnClassID.ClientID %>').val(result[0].ClassID);
                    $('#<%:txtClassCode.ClientID %>').val(result[0].ClassCode);
                    $('#<%:txtClassName.ClientID %>').val(result[0].ClassName);
                    $('#<%:hdnChargeClassID.ClientID %>').val(result[0].ChargeClassID);
                    $('#<%:txtChargeClassCode.ClientID %>').val(result[0].ChargeClassCode);
                    $('#<%:txtChargeClassName.ClientID %>').val(result[0].ChargeClassName);
                }

                $('#<%:hdnBedID.ClientID %>').val('');
                $('#<%:txtBedCode.ClientID %>').val('');
                $('#<%:hdnExtensionNo.ClientID %>').val('');
            }
            else {
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
            }
            $('#<%:hdnBedID.ClientID %>').val('');
            $('#<%:txtBedCode.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Bed
    $('#<%:lblBed.ClientID %>.lblLink').live('click', function () {
        var roomID = $('#<%:hdnRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND GCBedStatus = '0116^U'";
            openSearchDialog('bed', filterExpression, function (value) {
                $('#<%:txtBedCode.ClientID %>').val(value);
                onTxtBedCodeChanged(value);
            });
        }
        else {
            showToast('Warning', 'Pilih kamar terlebih dahulu.');
        }
    });

    $('#<%:txtBedCode.ClientID %>').live('change', function () {
        onTxtBedCodeChanged($(this).val());
    });

    function onTxtBedCodeChanged(value) {
        var roomID = $('#<%:hdnRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND ";
            filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvBedList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnBedID.ClientID %>').val(result.BedID);
                    $('#<%:hdnExtensionNo.ClientID %>').val(result.ExtensionNo);
                }
                else {
                    $('#<%:hdnBedID.ClientID %>').val('');
                    $('#<%:txtBedCode.ClientID %>').val('');
                    $('#<%:hdnExtensionNo.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%:hdnBedID.ClientID %>').val('');
            $('#<%:txtBedCode.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Charge Class
    function onGetChargeClassFilterExpression() {
        return "IsUsedInChargeClass = 1 AND IsDeleted = 0";
    }

    $('#<%:lblChargeClass.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', onGetChargeClassFilterExpression(), function (value) {
            $('#<%:txtChargeClassCode.ClientID %>').val(value);
            onTxtChargeClassCodeChanged(value);
        });
    });

    $('#<%:txtChargeClassCode.ClientID %>').live('change', function () {
        onTxtChargeClassCodeChanged($(this).val());
    });

    function onTxtChargeClassCodeChanged(value) {
        var filterExpression = onGetChargeClassFilterExpression() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnChargeClassID.ClientID %>').val(result.ClassID);
                $('#<%:txtChargeClassName.ClientID %>').val(result.ClassName);
            }
            else {
                $('#<%:hdnChargeClassID.ClientID %>').val('');
                $('#<%:txtChargeClassCode.ClientID %>').val('');
                $('#<%:txtChargeClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" runat="server" id="hdnExtensionNo" value="" />
    <input type="hidden" id="hdnGCRegistrationStatus" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnFamilyID" value="" />
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnMainParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Registrasi")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtRegistrationNo" readonly="true" width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtPatientName" readonly="true" width="500px" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Tambah Data")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td style="vertical-align: top">
                                    <table>
                                        <colgroup>
                                            <col style="width: 140px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No Penunggu")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtPatientAccompanyNo" readonly="true" width="180px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblDisabled" id="lblFamily">
                                                    <%=GetLabel("No Rekam Medis")%></label>
                                            </td>
                                            <td>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 180px" />
                                                        <col style="width: 20px" />
                                                        <col style="width: 100px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtFamily" readonly="true" width="180px" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:checkbox id="chkIsFamily" runat="server" /><%=GetLabel("Keluarga")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal lblMandatory" runat="server" id="lblContactName">
                                                    <%=GetLabel("Nama")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtContactName" width="250px" cssclass="required" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal lblMandatory" runat="server" id="Label1">
                                                    <%=GetLabel("Hubungan")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRelationship" ClientInstanceName="cboRelationship" Width="180px"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Telepon")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtTelephoneNo" width="180px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No HP")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtMobilePhone" width="180px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Email")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtEmail" cssclass="email" width="250px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Catatan")%></label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtRemarks" width="250px" runat="server" textmode="MultiLine" rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top">
                                    <table>
                                        <colgroup>
                                            <col style="width: 140px" />
                                            <col />
                                        </colgroup>
                                        <tr id="trClass" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" id="lblClass" runat="server">
                                                    <%:GetLabel("Kelas")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnClassID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtClassCode" width="100px" cssclass="required" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:textbox id="txtClassName" width="200px" cssclass="required" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trServiceUnit" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                                    <%:GetLabel("Ruang Perawatan")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnIsServiceUnitHasParamedic" value="" runat="server" />
                                                <input type="hidden" id="hdnIsServiceUnitHasVisitType" value="" runat="server" />
                                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtServiceUnitCode" width="100px" cssclass="required" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:textbox id="txtServiceUnitName" width="200px" cssclass="required" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trRoom" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblRoom">
                                                    <%:GetLabel("Kamar")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtRoomCode" width="100px" cssclass="required" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:textbox id="txtRoomName" width="200px" cssclass="required" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trBed" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblBed">
                                                    <%:GetLabel("Tempat Tidur")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnBedID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtBedCode" width="100px" cssclass="required" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trChargeClass" runat="server">
                                            <td class="tdLabel">
                                                <label class="lblLink lblMandatory" runat="server" id="lblChargeClass">
                                                    <%:GetLabel("Kelas Tagihan")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnChargeClassID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 100px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:textbox id="txtChargeClassCode" width="100px" cssclass="required" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:textbox id="txtChargeClassName" width="200px" cssclass="required" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:panel runat="server" id="pnlPatientVisitTransHdGrdView" style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:gridview id="grdView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                    showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                    <columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                               <table cellpadding="0" cellspacing="0">
                                                    <tr>
<%--                                                <img class='imgPatientVisitEdit <%#: IsAllowEditPatientVisit.ToString() == "False"  ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Edit")%>' 
                                                    src='<%# IsAllowEditPatientVisit.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;margin-right: 2px;" />--%>
                                                <img class='imgDelete <%#: IsAllowEditPatientVisit.ToString() == "False" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Delete")%>' 
                                                    src='<%# IsAllowEditPatientVisit.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("PatientAccompanyID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("RegistrationID") %>" bindingfield="RegistrationID" />
                                                <input type="hidden" value="<%#:Eval("PatientAccompanyName") %>" bindingfield="PatientAccompanyName" />
                                                <input type="hidden" value="<%#:Eval("RelationShip") %>" bindingfield="RelationShip" />
                                                <input type="hidden" value="<%#:Eval("RoomCode") %>" bindingfield="RoomCode" />
                                                <input type="hidden" value="<%#:Eval("RoomName") %>" bindingfield="RoomName" />
                                                <input type="hidden" value="<%#:Eval("BedCode") %>" bindingfield="BedCode" />
                                            </ItemTemplate>

                                        <HeaderStyle Width="20px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PatientAccompanyNo" HeaderText="No Penunggu" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                            ItemStyle-CssClass="tdPatientAccompanyNo" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="150px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" CssClass="tdPatientAccompanyNo"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PatientAccompanyName" HeaderText="Nama Penunggu" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-CssClass="tdPatientAccompanyName" ItemStyle-HorizontalAlign="Left">
                                        <ItemStyle HorizontalAlign="Left" CssClass="tdPatientAccompanyName"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RelationShip" HeaderText="Hubungan" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" >
                                        <HeaderStyle Width="150px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RoomName" HeaderText="Ruang" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" >
                                        <HeaderStyle Width="200px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BedCode" HeaderText="Tempat Tidur" HeaderStyle-HorizontalAlign="Center" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" >
                                        <HeaderStyle Width="80px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    </columns>
                                    <emptydatarowstyle cssclass="trEmpty"></emptydatarowstyle>
                                    <emptydatatemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </emptydatatemplate>
                                </asp:gridview>
                            </asp:panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddDataPatientAccompanyCtl">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
