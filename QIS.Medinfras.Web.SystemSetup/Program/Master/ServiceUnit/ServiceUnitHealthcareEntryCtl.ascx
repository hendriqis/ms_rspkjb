<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitHealthcareEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitHealthcareEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
        $('#<%=txtHealthcareCode.ClientID %>').val('');
        $('#<%=txtHealthcareName.ClientID %>').val('');
        $('#<%=cboChargeClassID.ClientID %>').val('');
        $('#<%=cboClinicGroup.ClientID %>').val('');
        $('#<%=hdnLocationID.ClientID %>').val('');
        $('#<%=txtLocationCode.ClientID %>').val('');
        $('#<%=txtLocationName.ClientID %>').val('');
        $('#<%=hdnLogisticLocationID.ClientID %>').val('');
        $('#<%=txtLogisticLocationCode.ClientID %>').val('');
        $('#<%=txtLogisticLocationName.ClientID %>').val('');
        $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val('');
        $('#<%=txtDispensaryServiceUnitCode.ClientID %>').val('');
        $('#<%=txtDispensaryServiceUnitName.ClientID %>').val('');
        $('#<%=hdnRevenueSharingID.ClientID %>').val('');
        $('#<%=txtRevenueSharingCode.ClientID %>').val('');
        $('#<%=txtRevenueSharingName.ClientID %>').val('');
        $('#<%=hdnDefaultParamedicID.ClientID %>').val('');
        $('#<%=txtDefaultParamedicCode.ClientID %>').val('');
        $('#<%=txtDefaultParamedicName.ClientID %>').val('');
        $('#<%=hdnItemBedChargesID.ClientID %>').val('');
        $('#<%=txtItemBedChargesCode.ClientID %>').val('');
        $('#<%=txtItemBedChargesName.ClientID %>').val('');
        $('#<%=hdnNursingServiceItemID.ClientID %>').val('');
        $('#<%=txtNursingServiceItemCode.ClientID %>').val('');
        $('#<%=txtNursingServiceItemName.ClientID %>').val('');
        $('#<%=txtServiceInterval.ClientID %>').val('0');
        $('#<%=txtServiceUnitOfficer.ClientID %>').val('');
        $('#<%=txtPrinter1Url.ClientID %>').val('');
        $('#<%=txtIPAddress.ClientID %>').val('');
        $('#<%=chkIsInpatientDispensary.ClientID %>').prop('checked', false);
        $('#<%=chkIsAutoCloseRegistration.ClientID %>').prop('checked', false);
        $('#<%=chkIsUseDiagnosisCodingProcess.ClientID %>').prop('checked', false);
        $('#<%=chkIsChargeClassEditableForNonInpatient.ClientID %>').prop('checked', false);
        $('#<%=chkIsAutomaticPrintTracer.ClientID %>').prop('checked', false);

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var healthcareServiceUnitID = $row.find('.hdnHealthcareServiceUnitID').val();
            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(healthcareServiceUnitID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var healthcareServiceUnitID = $row.find('.hdnHealthcareServiceUnitID').val();
        var healthcareID = $row.find('.hdnHealthcareID').val();
        var healthcareName = $row.find('.tdHealthcareName').html();
        var gcClinicGroup = $row.find('.hdnGCClinicGroup').val();
        var chargeClassID = $row.find('.hdnChargeClassID').val();
        var locationID = $row.find('.hdnLocationID').val();
        var locationCode = $row.find('.hdnLocationCode').val();
        var locationName = $row.find('.tdLocationName').html();
        var logisticLocationID = $row.find('.hdnLogisticLocationID').val();
        var logisticLocationCode = $row.find('.hdnLogisticLocationCode').val();
        var logisticlocationName = $row.find('.hdnLogisticLocationName').val();
        var dispensaryServiceUnitID = $row.find('.hdnDispensaryServiceUnitID').val();
        var dispensaryServiceUnitCode = $row.find('.hdnDispensaryServiceUnitCode').val();
        var dispensaryServiceUnitName = $row.find('.tdDispensaryServiceUnitName').html().replace('&nbsp;', '');
        var revenueSharingID = $row.find('.hdnRevenueSharingID').val();
        var revenueSharingCode = $row.find('.hdnRevenueSharingCode').val();
        var revenueSharingName = $row.find('.hdnRevenueSharingName').val();
        var defaultParamedicID = $row.find('.hdnDefaultParamedicID').val();
        var defaultParamedicCode = $row.find('.hdnDefaultParamedicCode').val();
        var defaultParamedicName = $row.find('.hdnDefaultParamedicName').val();
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName1 = $row.find('.hdnItemName1').val();
        var nursingServiceItemID = $row.find('.hdnNursingServiceItemID').val();
        var nursingServiceItemCode = $row.find('.hdnNursingServiceItemCode').val();
        var nursingServiceItemName = $row.find('.hdnNursingServiceItemName').val();
        var serviceInterval = $row.find('.hdnServiceInterval').val();
        var serviceUnitOfficer = $row.find('.hdnServiceUnitOfficer').val().replace('&nbsp;', '');
        var printer1Url = $row.find('.hdnPrinter1Url').val();
        var ipAddress = $row.find('.hdnIPAddress').val();
        var subLedgerDtCode = $row.find('.hdnSubLedgerDtCode').val();
        var subLedgerDtName = $row.find('.hdnSubLedgerDtName').val();
        var isAutoCloseRegistraion = ($row.find('.hdnIsAutoCloseRegistration').val() == 'True');
        var isInpatientDispensary = ($row.find('.hdnIsInpatientDispensary').val() == 'True');
        var isUseDiagnosisCodingProcess = ($row.find('.hdnIsUseDiagnosisCodingProcess').val() == 'True');
        var isChargeClassEditableForNonInpatient = ($row.find('.hdnIsChargeClassEditableForNonInpatient').val() == 'True');
        var isAutomaticPrintTracer = ($row.find('.hdnIsAutomaticPrintTracer').val() == 'True');

        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(healthcareServiceUnitID);
        $('#<%=txtHealthcareCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtHealthcareCode.ClientID %>').val(healthcareID);
        $('#<%=txtHealthcareName.ClientID %>').val(healthcareName);
        cboChargeClassID.SetValue(chargeClassID);
        cboClinicGroup.SetValue(gcClinicGroup);
        $('#<%=hdnLocationID.ClientID %>').val(locationID);
        $('#<%=txtLocationCode.ClientID %>').val(locationCode);
        $('#<%=txtLocationName.ClientID %>').val(locationName);
        $('#<%=hdnLogisticLocationID.ClientID %>').val(logisticLocationID);
        $('#<%=txtLogisticLocationCode.ClientID %>').val(logisticLocationCode);
        $('#<%=txtLogisticLocationName.ClientID %>').val(logisticlocationName);
        $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val(dispensaryServiceUnitID);
        $('#<%=txtDispensaryServiceUnitCode.ClientID %>').val(dispensaryServiceUnitCode);
        $('#<%=txtDispensaryServiceUnitName.ClientID %>').val(dispensaryServiceUnitName);
        $('#<%=hdnRevenueSharingID.ClientID %>').val(revenueSharingID);
        $('#<%=txtRevenueSharingCode.ClientID %>').val(revenueSharingCode);
        $('#<%=txtRevenueSharingName.ClientID %>').val(revenueSharingName);
        $('#<%=hdnDefaultParamedicID.ClientID %>').val(defaultParamedicID);
        $('#<%=txtDefaultParamedicCode.ClientID %>').val(defaultParamedicCode);
        $('#<%=txtDefaultParamedicName.ClientID %>').val(defaultParamedicName);
        $('#<%=hdnItemBedChargesID.ClientID %>').val(itemID);
        $('#<%=txtItemBedChargesCode.ClientID %>').val(itemCode);
        $('#<%=txtItemBedChargesName.ClientID %>').val(itemName1);
        $('#<%=hdnNursingServiceItemID.ClientID %>').val(nursingServiceItemID);
        $('#<%=txtNursingServiceItemCode.ClientID %>').val(nursingServiceItemCode);
        $('#<%=txtNursingServiceItemName.ClientID %>').val(nursingServiceItemName);
        $('#<%=txtServiceInterval.ClientID %>').val(serviceInterval);
        $('#<%=txtServiceUnitOfficer.ClientID %>').val(serviceUnitOfficer);
        $('#<%=txtPrinter1Url.ClientID %>').val(printer1Url);
        $('#<%=txtIPAddress.ClientID %>').val(ipAddress);
        $('#<%=txtSubLedgerCode.ClientID %>').val(subLedgerDtCode);
        $('#<%=txtSubLedgerName.ClientID %>').val(subLedgerDtName);
        $('#<%=chkIsAutoCloseRegistration.ClientID %>').prop('checked', isAutoCloseRegistraion);
        $('#<%=chkIsInpatientDispensary.ClientID %>').prop('checked', isInpatientDispensary);
        $('#<%=chkIsUseDiagnosisCodingProcess.ClientID %>').prop('checked', isUseDiagnosisCodingProcess);
        $('#<%=chkIsChargeClassEditableForNonInpatient.ClientID %>').prop('checked', isChargeClassEditableForNonInpatient);
        $('#<%=chkIsAutomaticPrintTracer.ClientID %>').prop('checked', isAutomaticPrintTracer);

        $('#containerPopupEntryData').show();
    });

    //#region Healthcare
    $('#lblHealthcare.lblLink').die('click');
    $('#lblHealthcare.lblLink').live('click', function () {
        $('#containerPopupEntryData').show();
        var serviceUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
        var filterExpression = "HealthcareID NOT IN (SELECT HealthcareID FROM HealthcareServiceUnit WHERE ServiceUnitID = " + serviceUnitID + " AND IsDeleted = 0)";
        openSearchDialog('healthcare', filterExpression, function (value) {
            $('#<%=txtHealthcareCode.ClientID %>').val(value);
            onTxtSHUHealthcareCodeChanged(value);
        });
    });

    $('#<%=txtHealthcareCode.ClientID %>').die('change');
    $('#<%=txtHealthcareCode.ClientID %>').live('change', function () {
        onTxtSHUHealthcareCodeChanged($(this).val());
    });

    function onTxtSHUHealthcareCodeChanged(value) {
        var filterExpression = "HealthcareID = '" + value + "'";
        Methods.getObject('GetHealthcareList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtHealthcareName.ClientID %>').val(result.HealthcareName);
            else
                $('#<%=txtHealthcareName.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Location
    $('#lblLocation.lblLink').die('click');
    $('#lblLocation.lblLink').live('click', function () {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "HealthcareID = '" + healthcareCode + "' AND (GCLocationGroup = '" + Constant.ItemTypeLocation.DRUG_SUPPLIES + "' OR GCLocationGroup IS NULL) AND (IsHeader = 0 OR (IsHeader = 1 AND IsHasChildren = 1)) AND IsDeleted = 0";
            filterExpression += " AND LocationID NOT IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008')";
            filterExpression += " AND LocationID NOT IN (SELECT ISNULL(ParentID,0) FROM Location WHERE LocationID IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008'))";
            openSearchDialog('location', filterExpression, function (value) {
                $('#<%=txtLocationCode.ClientID %>').val(value);
                onTxtSHULocationCodeChanged(value);
            });
        }
        else {
            showToast('WARNING!', 'Mohon Pilih Rumah Sakit Terlebih Dahulu!');
        }
    });

    $('#lblLogisticLocation.lblLink').die('click');
    $('#lblLogisticLocation.lblLink').live('click', function () {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "HealthcareID = '" + healthcareCode + "' AND (GCLocationGroup = '" + Constant.ItemTypeLocation.LOGISTICS + "' OR GCLocationGroup IS NULL) AND (IsHeader = 0 OR (IsHeader = 1 AND IsHasChildren = 1)) AND IsDeleted = 0";
            filterExpression += " AND LocationID NOT IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008')";
            filterExpression += " AND LocationID NOT IN (SELECT ISNULL(ParentID,0) FROM Location WHERE LocationID IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008'))";
            openSearchDialog('location', filterExpression, function (value) {
                $('#<%=txtLogisticLocationCode.ClientID %>').val(value);
                onTxtSHULogisticLocationCodeChanged(value);
            });
        }
        else {
            showToast('WARNING!', 'Mohon Pilih Rumah Sakit Terlebih Dahulu!');
        }
    });

    $('#<%=txtLocationCode.ClientID %>').die('change');
    $('#<%=txtLocationCode.ClientID %>').live('change', function () {
        onTxtSHULocationCodeChanged($(this).val());
    });

    $('#<%=txtLogisticLocationCode.ClientID %>').die('change');
    $('#<%=txtLogisticLocationCode.ClientID %>').live('change', function () {
        onTxtSHULogisticLocationCodeChanged($(this).val());
    });


    function onTxtSHULocationCodeChanged(value) {
        var filterExpression = "LocationCode = '" + value + "' AND IsDeleted = 0";
        filterExpression += " AND LocationID NOT IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008')";
        filterExpression += " AND LocationID NOT IN (SELECT ISNULL(ParentID,0) FROM Location WHERE LocationID IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008'))";
        Methods.getObject('GetLocationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnLocationID.ClientID %>').val(result.LocationID);
                $('#<%=txtLocationName.ClientID %>').val(result.LocationName);
            }
            else {
                $('#<%=hdnLocationID.ClientID %>').val('');
                $('#<%=txtLocationName.ClientID %>').val('');
            }
        });
    }

    function onTxtSHULogisticLocationCodeChanged(value) {
        var filterExpression = "LocationCode = '" + value + "' AND IsDeleted = 0";
        filterExpression += " AND LocationID NOT IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008')";
        filterExpression += " AND LocationID NOT IN (SELECT ISNULL(ParentID,0) FROM Location WHERE LocationID IN (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WHERE spd.ParameterCode = 'IM0008'))";
        Methods.getObject('GetLocationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnLogisticLocationID.ClientID %>').val(result.LocationID);
                $('#<%=txtLogisticLocationName.ClientID %>').val(result.LocationName);
            }
            else {
                $('#<%=hdnLogisticLocationID.ClientID %>').val('');
                $('#<%=txtLogisticLocationName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Dispensary
    $('#<%:lblDispensaryServiceUnit.ClientID %>.lblLink').live('click', function () {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "DepartmentID = 'PHARMACY' AND ServiceUnitID IN (SELECT ServiceUnitID FROM HealthcareServiceUnit WHERE HealthcareID = '" + healthcareCode + "') AND IsDeleted = 0";
            openSearchDialog('serviceunit', filterExpression, function (value) {
                $('#<%=txtDispensaryServiceUnitCode.ClientID %>').val(value);
                onTxtSHUDispensaryCodeChanged(value);
            });
        }
        else {
            showToast('WARNING!', 'Mohon Pilih Rumah Sakit Terlebih Dahulu!');
        }
    });

    $('#<%=txtDispensaryServiceUnitCode.ClientID %>').live('change', function () {
        onTxtSHUDispensaryCodeChanged($(this).val());
    });

    function onTxtSHUDispensaryCodeChanged(value) {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "HealthcareID = '" + healthcareCode + "' AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtDispensaryServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val('');
                    $('#<%=txtDispensaryServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtDispensaryServiceUnitName.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnDispensaryServiceUnitID.ClientID %>').val('');
            $('#<%=txtDispensaryServiceUnitCode.ClientID %>').val('');
            $('#<%=txtDispensaryServiceUnitName.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Revenue Sharing
    $('#lblRevenueSharing.lblLink').live('click', function () {
        openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
            $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
            onTxtRevenueSharingCodeChanged(value);
        });
    });

    $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
        onTxtRevenueSharingCodeChanged($(this).val());
    });

    function onTxtRevenueSharingCodeChanged(value) {
        var filterExpression = "RevenueSharingCode = '" + value + "'";
        Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
            }
            else {
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                $('#<%=txtRevenueSharingName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Default Paramedic
    $('#lblDefaultParamedic.lblLink').live('click', function () {
        var filterDefaultParamedic = "IsDeleted = 0 AND ServiceUnitID = " + $('#<%=hdnServiceUnitID.ClientID %>').val();
        openSearchDialog('serviceUnitParamedicMaster', filterDefaultParamedic, function (value) {
            $('#<%=txtDefaultParamedicCode.ClientID %>').val(value);
            onTxtDefaultParamedicCodeChanged(value);
        });
    });

    $('#<%=txtDefaultParamedicCode.ClientID %>').live('change', function () {
        onTxtDefaultParamedicCodeChanged($(this).val());
    });

    function onTxtDefaultParamedicCodeChanged(value) {
        var filterExpression = "IsDeleted = 0 AND ServiceUnitID = " + $('#<%=hdnServiceUnitID.ClientID %>').val() + " AND ParamedicID = '" + value + "'";
        Methods.getObject('GetvServiceUnitParamedicList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDefaultParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtDefaultParamedicCode.ClientID %>').val(result.ParamedicCode);
                $('#<%=txtDefaultParamedicName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnDefaultParamedicID.ClientID %>').val('');
                $('#<%=txtDefaultParamedicCode.ClientID %>').val('');
                $('#<%=txtDefaultParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Item
    function onGetItemBedChargesFilterExpression() {
        var filterExpression = "<%:OnGetItemBedChargesFilterExpression() %>";
        return filterExpression;
    }
    $('#lblBedCharges.lblLink').live('click', function () {
        openSearchDialog('item', onGetItemBedChargesFilterExpression(), function (value) {
            $('#<%=txtItemBedChargesCode.ClientID %>').val(value);
            onTxtItemBedChargesChanged(value);
        });
    });

    $('#<%=txtItemBedChargesCode.ClientID %>').live('change', function () {
        onTxtItemBedChargesChanged($(this).val());
    });

    function onTxtItemBedChargesChanged(value) {
        var filterExpression = onGetItemBedChargesFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemBedChargesID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemBedChargesName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemBedChargesID.ClientID %>').val('');
                $('#<%=txtItemBedChargesCode.ClientID %>').val('');
                $('#<%=txtItemBedChargesName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region NursingServiceItem
    function onGetNursingServiceItemFilterExpression() {
        var filterExpression = "<%:onGetNursingServiceItemFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNursingServiceItem.lblLink').live('click', function () {
        openSearchDialog('item', onGetNursingServiceItemFilterExpression(), function (value) {
            $('#<%=txtNursingServiceItemCode.ClientID %>').val(value);
            onTxtNursingServiceItemChanged(value);
        });
    });

    $('#<%=txtNursingServiceItemCode.ClientID %>').live('change', function () {
        onTxtNursingServiceItemChanged($(this).val());
    });

    function onTxtNursingServiceItemChanged(value) {
        var filterExpression = onGetNursingServiceItemFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnNursingServiceItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtNursingServiceItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnNursingServiceItemID.ClientID %>').val('');
                $('#<%=txtNursingServiceItemCode.ClientID %>').val('');
                $('#<%=txtNursingServiceItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Sub Ledger
    $('#lblSubLedger.lblLink').live('click', function () {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('subledgerdt', filterExpression, function (value) {
                $('#<%=txtSubLedgerCode.ClientID %>').val(value);
                ontxtSubLedgerCodeChanged(value);
            });
        }
    });

    $('#<%=txtSubLedgerCode.ClientID %>').live('change', function () {
        ontxtSubLedgerCodeChanged($(this).val());
    });

    function ontxtSubLedgerCodeChanged(value) {
        var healthcareCode = $('#<%=txtHealthcareCode.ClientID %>').val();
        if (healthcareCode != '') {
            var filterExpression = "IsDeleted = 0 AND SubLedgerDtCode = '" + value + "'";
            Methods.getObject('GetSubLedgerDtList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerDtID);
                    $('#<%=txtSubLedgerCode.ClientID %>').val(result.SubLedgerDtCode);
                    $('#<%=txtSubLedgerName.ClientID %>').val(result.SubLedgerDtName);
                }
                else {
                    $('#<%=hdnSubLedgerID.ClientID %>').val('');
                    $('#<%=txtSubLedgerCode.ClientID %>').val('');
                    $('#<%=txtSubLedgerName.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
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
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 100px" />
                                <col style="width: 600px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblHealthcare">
                                        <%=GetLabel("Rumah Sakit")%></label>
                                </td>
                                <td colspan="2">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtHealthcareCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Grup Pelayanan")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboClinicGroup" ClientInstanceName="cboClinicGroup" Width="105"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblLocation">
                                        <%=GetLabel("Lokasi Obat & Alkes")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtLocationCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocationName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblLogisticLocation">
                                        <%=GetLabel("Lokasi Barang Umum")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnLogisticLocationID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtLogisticLocationCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLogisticLocationName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblDispensaryServiceUnit" runat="server">
                                        <%=GetLabel("Unit Farmasi")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDispensaryServiceUnitID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDispensaryServiceUnitCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDispensaryServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblRevenueSharing">
                                        <%=GetLabel("Formula Honor Dokter")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnRevenueSharingID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDefaultParamedic">
                                        <%=GetLabel("Default Paramedic")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDefaultParamedicID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDefaultParamedicCode" CssClass="true" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDefaultParamedicName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trChargeClass" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas Tagihan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboChargeClassID" ClientInstanceName="cboChargeClassID" runat="server" />
                                </td>
                                <td>
                                    <table id="tblInpatientDispensary" runat="server" style="display: none">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsInpatientDispensary" runat="server" />
                                            </td>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Farmasi Rawat Inap")%></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trItemBedCharges" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblBedCharges">
                                        <%=GetLabel("Tarif Kamar")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnItemBedChargesID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemBedChargesCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemBedChargesName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trNursingServiceItem" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblNursingServiceItem">
                                        <%=GetLabel("Tarif Jasa Perawat")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnNursingServiceItemID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNursingServiceItemCode" CssClass="true" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNursingServiceItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lama Pelayanan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtServiceInterval" CssClass="required number" Width="100px" runat="server" /><%=GetLabel("Menit")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penanggung Jawab")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtServiceUnitOfficer" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Alamat Printer Order")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPrinter1Url" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("IP Address (Notification)")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtIPAddress" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblSubLedger">
                                        <%=GetLabel("Sub Perkiraan")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnSubLedgerID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSubLedgerCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubLedgerName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trIsAutoCloseRegistration" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                    </label>
                                </td>
                                <td colspan="2">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 15px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsAutoCloseRegistration" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%=GetLabel("Otomatis Tutup Registrasi Ketika Pembayaran Lunas")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                    </label>
                                </td>
                                <td colspan="2">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 15px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsUseDiagnosisCodingProcess" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%=GetLabel(" Dilakukan proses pengkodean diagnosa atas kunjungan pasien")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trIsChargeClassEditableForNonInpatient" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                    </label>
                                </td>
                                <td colspan="2">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 15px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsChargeClassEditableForNonInpatient" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%=GetLabel(" Pada saat pendaftaran dapat memilih kelas tagihan")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trIsAutomaticPrintTracer" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                    </label>
                                </td>
                                <td colspan="2">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 15px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsAutomaticPrintTracer" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%=GetLabel(" Otomatis Print Tracer Pendaftaran")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' class="w3-btn w3-hover-blue" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="w3-btn w3-hover-blue" />
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
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnHealthcareServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                <input type="hidden" class="hdnHealthcareID" value="<%#: Eval("HealthcareID")%>" />
                                                <input type="hidden" class="hdnGCClinicGroup" value="<%#: Eval("GCClinicGroup")%>" />
                                                <input type="hidden" class="hdnLocationID" value="<%#: Eval("LocationID")%>" />
                                                <input type="hidden" class="hdnLocationCode" value="<%#: Eval("LocationCode")%>" />
                                                <input type="hidden" class="hdnLocationName" value="<%#: Eval("LocationName")%>" />
                                                <input type="hidden" class="hdnLogisticLocationID" value="<%#: Eval("LogisticLocationID")%>" />
                                                <input type="hidden" class="hdnLogisticLocationCode" value="<%#: Eval("LogisticLocationCode")%>" />
                                                <input type="hidden" class="hdnLogisticLocationName" value="<%#: Eval("LogisticLocationName")%>" />
                                                <input type="hidden" class="hdnDispensaryServiceUnitID" value="<%#: Eval("DispensaryServiceUnitID")%>" />
                                                <input type="hidden" class="hdnDispensaryServiceUnitCode" value="<%#: Eval("DispensaryServiceUnitCode")%>" />
                                                <input type="hidden" class="hdnIsInpatientDispensary" value="<%#: Eval("IsInpatientDispensary")%>" />
                                                <input type="hidden" class="hdnRevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                <input type="hidden" class="hdnRevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                <input type="hidden" class="hdnRevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                <input type="hidden" class="hdnDefaultParamedicID" value="<%#: Eval("DefaultParamedicID")%>" />
                                                <input type="hidden" class="hdnDefaultParamedicCode" value="<%#: Eval("DefaultParamedicCode")%>" />
                                                <input type="hidden" class="hdnDefaultParamedicName" value="<%#: Eval("DefaultParamedicName")%>" />
                                                <input type="hidden" class="hdnChargeClassID" value="<%#: Eval("ChargeClassID")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnItemName1" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="hdnNursingServiceItemID" value="<%#: Eval("NursingServiceItemID")%>" />
                                                <input type="hidden" class="hdnNursingServiceItemCode" value="<%#: Eval("NursingServiceItemCode")%>" />
                                                <input type="hidden" class="hdnNursingServiceItemName" value="<%#: Eval("NursingServiceItemName")%>" />
                                                <input type="hidden" class="hdnServiceInterval" value="<%#: Eval("ServiceInterval")%>" />
                                                <input type="hidden" class="hdnServiceUnitOfficer" value="<%#: Eval("ServiceUnitOfficer")%>" />
                                                <input type="hidden" class="hdnIsAutoCloseRegistration" value="<%#: Eval("IsAutoCloseRegistration")%>" />
                                                <input type="hidden" class="hdnIsUseDiagnosisCodingProcess" value="<%#: Eval("IsUseDiagnosisCodingProcess")%>" />
                                                <input type="hidden" class="hdnIsChargeClassEditableForNonInpatient" value="<%#: Eval("IsChargeClassEditableForNonInpatient")%>" />
                                                <input type="hidden" class="hdnIsAutomaticPrintTracer" value="<%#: Eval("IsAutomaticPrintTracer")%>" />
                                                <input type="hidden" class="hdnPrinter1Url" value="<%#: Eval("Printer1Url")%>" />
                                                <input type="hidden" class="hdnIPAddress" value="<%#: Eval("IPAddress")%>" />
                                                <input type="hidden" class="hdnSubLedgerDtCode" value="<%#: Eval("SubLedgerDtCode")%>" />
                                                <input type="hidden" class="hdnSubLedgerDtName" value="<%#: Eval("SubLedgerDtName")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HealthcareName" ItemStyle-CssClass="tdHealthcareName"
                                            HeaderText="Nama Rumah Sakit" />
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="LocationName" HeaderText="Lokasi Obat & Alkes"
                                            ItemStyle-CssClass="tdLocationName" />
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="LogisticLocationName" HeaderText="Lokasi Barang Umum"
                                            ItemStyle-CssClass="tdLogisticLocationName" />
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="DispensaryServiceUnitName" ItemStyle-CssClass="tdDispensaryServiceUnitName"
                                            HeaderText="Unit Farmasi" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
