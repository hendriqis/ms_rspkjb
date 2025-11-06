<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalTemplateDtEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalTemplateDtEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnEntryID.ClientID %>').val('');
        $('#<%=hdnGLAccountID.ClientID %>').val('');
        $('#<%=txtGLAccountCode.ClientID %>').val('');
        $('#<%=txtGLAccountName.ClientID %>').val('');
        $('#<%=txtAmountPercentage.ClientID %>').val('0');
        $('#<%=txtAmount.ClientID %>').val('0.00');
        $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
        $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
        $('#<%=txtSubLedgerDtName.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('0');

        $('#<%=hdnSubLedgerID.ClientID %>').val('');
        $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');

        onSubLedgerIDChanged();

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

    $('.imgDeleteD.imgLink').die('click');
    $('.imgDeleteD.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });

    $('.imgEditD.imgLink').die('click');
    $('.imgEditD.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=hdnGLAccountID.ClientID %>').val(entity.GLAccountID);
        $('#<%=txtGLAccountCode.ClientID %>').val(entity.GLAccountNo);
        $('#<%=txtGLAccountName.ClientID %>').val(entity.GLAccountName);
        $('#<%=hdnSubLedgerDtID.ClientID %>').val(entity.SubLedgerDtID);
        $('#<%=txtSubLedgerDtCode.ClientID %>').val(entity.SubLedgerDtCode);
        $('#<%=txtSubLedgerDtName.ClientID %>').val(entity.SubLedgerDtName);
        $('#<%=txtAmountPercentage.ClientID %>').val(entity.AmountPercentage);
        $('#<%=txtAmount.ClientID %>').val(entity.Amount);
        $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
        $("#<%=rblPosition.ClientID %> input[value=" + entity.Position + "]").prop('checked', true);

        $('#<%=hdnSubLedgerID.ClientID %>').val(entity.SubLedgerID);
        $('#<%=hdnSearchDialogTypeName.ClientID %>').val(entity.SearchDialogTypeName);
        $('#<%=hdnFilterExpression.ClientID %>').val(entity.FilterExpression);
        $('#<%=hdnIDFieldName.ClientID %>').val(entity.IDFieldName);
        $('#<%=hdnCodeFieldName.ClientID %>').val(entity.CodeFieldName);
        $('#<%=hdnDisplayFieldName.ClientID %>').val(entity.DisplayFieldName);
        $('#<%=hdnMethodName.ClientID %>').val(entity.MethodName);

        $('#<%=hdnHealthcare.ClientID %>').val(entity.HealthcareID);
        cboHealthcare.SetValue("001");

        $('#<%=hdnDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentName.ClientID %>').val(entity.DepartmentName);

        $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
        $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);

        $('#<%=hdnRevenueCostCenterID.ClientID %>').val(entity.RevenueCostCenterID);
        $('#<%=txtRevenueCostCenterCode.ClientID %>').val(entity.RevenueCostCenterCode);
        $('#<%=txtRevenueCostCenterName.ClientID %>').val(entity.RevenueCostCenterName);

        $('#<%=hdnCustomerGroupID.ClientID %>').val(entity.CustomerGroupID);
        $('#<%=txtCustomerGroupCode.ClientID %>').val(entity.CustomerGroupCode);
        $('#<%=txtCustomerGroupName.ClientID %>').val(entity.CustomerGroupName);

        $('#<%=hdnBusinessPartnerID.ClientID %>').val(entity.BusinessPartnerID);
        $('#<%=txtBusinessPartnerCode.ClientID %>').val(entity.BusinessPartnerCode);
        $('#<%=txtBusinessPartnerName.ClientID %>').val(entity.BusinessPartnerName);

        onSubLedgerIDChanged();

        $('#containerPopupEntryData').show();
    });

    $('.imgDeleteK.imgLink').die('click');
    $('.imgDeleteK.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });

    $('.imgEditK.imgLink').die('click');
    $('.imgEditK.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=hdnGLAccountID.ClientID %>').val(entity.GLAccountID);
        $('#<%=txtGLAccountCode.ClientID %>').val(entity.GLAccountNo);
        $('#<%=txtGLAccountName.ClientID %>').val(entity.GLAccountName);
        
        $('#<%=hdnSubLedgerDtID.ClientID %>').val(entity.SubLedgerDtID);
        $('#<%=txtSubLedgerDtCode.ClientID %>').val(entity.SubLedgerDtCode);
        $('#<%=txtSubLedgerDtName.ClientID %>').val(entity.SubLedgerDtName);
        $('#<%=txtAmountPercentage.ClientID %>').val(entity.AmountPercentage);
        $('#<%=txtAmount.ClientID %>').val(entity.Amount);
        $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
        $("#<%=rblPosition.ClientID %> input[value=" + entity.Position + "]").prop('checked', true);

        $('#<%=hdnSubLedgerID.ClientID %>').val(entity.SubLedgerID);
        $('#<%=hdnSearchDialogTypeName.ClientID %>').val(entity.SearchDialogTypeName);
        $('#<%=hdnFilterExpression.ClientID %>').val(entity.FilterExpression);
        $('#<%=hdnIDFieldName.ClientID %>').val(entity.IDFieldName);
        $('#<%=hdnCodeFieldName.ClientID %>').val(entity.CodeFieldName);
        $('#<%=hdnDisplayFieldName.ClientID %>').val(entity.DisplayFieldName);
        $('#<%=hdnMethodName.ClientID %>').val(entity.MethodName);

        $('#<%=hdnHealthcare.ClientID %>').val(entity.HealthcareID);
        cboHealthcare.SetValue("001");

        $('#<%=hdnDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentName.ClientID %>').val(entity.DepartmentName);
        
        $('#<%=hdnServiceUnitID.ClientID %>').val(entity.ServiceUnitID);
        $('#<%=txtServiceUnitCode.ClientID %>').val(entity.ServiceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(entity.ServiceUnitName);
        
        $('#<%=hdnRevenueCostCenterID.ClientID %>').val(entity.RevenueCostCenterID);
        $('#<%=txtRevenueCostCenterCode.ClientID %>').val(entity.RevenueCostCenterCode);
        $('#<%=txtRevenueCostCenterName.ClientID %>').val(entity.RevenueCostCenterName);
        
        $('#<%=hdnCustomerGroupID.ClientID %>').val(entity.CustomerGroupID);
        $('#<%=txtCustomerGroupCode.ClientID %>').val(entity.CustomerGroupCode);
        $('#<%=txtCustomerGroupName.ClientID %>').val(entity.CustomerGroupName);
        
        $('#<%=hdnBusinessPartnerID.ClientID %>').val(entity.BusinessPartnerID);
        $('#<%=txtBusinessPartnerCode.ClientID %>').val(entity.BusinessPartnerCode);
        $('#<%=txtBusinessPartnerName.ClientID %>').val(entity.BusinessPartnerName);

        onSubLedgerIDChanged();

        $('#containerPopupEntryData').show();
    });

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
        hideLoadingPanel();
    }

    function onCboHealthcareValueChanged(s) {
        var value = s.GetValue();
        $('#<%=hdnHealthcare.ClientID %>').val(value);
    }

    //#region GL Account
    function onGetGLAccountFilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblGLAccount.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtGLAccountCode.ClientID %>').val(value);
            onTxtGLAccountNoChanged(value);
        });
    });

    $('#<%=txtGLAccountCode.ClientID %>').change(function () {
        onTxtGLAccountNoChanged($(this).val());
    });

    function onTxtGLAccountNoChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);

                $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                $('#<%=hdnFilterExpression.ClientID %>').val(result.FilterExpression);
                $('#<%=hdnIDFieldName.ClientID %>').val(result.IDFieldName);
                $('#<%=hdnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                $('#<%=hdnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                $('#<%=hdnMethodName.ClientID %>').val(result.MethodName);
                $("#<%=rblPosition.ClientID %> input[value=" + result.Position + "]").prop('checked', true);
                onSubLedgerIDChanged();
            }
            else {
                $('#<%=hdnGLAccountID.ClientID %>').val('');
                $('#<%=txtGLAccountCode.ClientID %>').val('');
                $('#<%=txtGLAccountName.ClientID %>').val('');

                $('#<%=hdnSubLedgerID.ClientID %>').val('');
                $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');
                $('#<%=hdnFilterExpression.ClientID %>').val('');
                $('#<%=hdnIDFieldName.ClientID %>').val('');
                $('#<%=hdnCodeFieldName.ClientID %>').val('');
                $('#<%=hdnDisplayFieldName.ClientID %>').val('');
                $('#<%=hdnMethodName.ClientID %>').val('');
            }

            $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
            $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
            $('#<%=txtSubLedgerDtName.ClientID %>').val('');
        });
    }

    function onSubLedgerIDChanged() {
        if ($('#<%=hdnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID.ClientID %>').val() == '') {
            $('#lblSubLedger').attr('class', 'lblDisabled');
            $('#<%=txtSubLedgerDtCode.ClientID %>').attr('readonly', 'readonly');
        }
        else {
            $('#lblSubLedger').attr('class', 'lblMandatory lblLink');
            $('#<%=txtSubLedgerDtCode.ClientID %>').removeAttr('readonly');
        }
    }
    //#endregion

    //#region Sub Ledger
    function onGetSubLedgerFilterExpression() {
        var filterExpression = $('#<%=hdnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID.ClientID %>').val());
        return filterExpression;
    }

    $('#lblSubLedger.lblLink').die('click');
    $('#lblSubLedger.lblLink').live('click', function () {
        if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
            openSearchDialog($('#<%=hdnSearchDialogTypeName.ClientID %>').val(), onGetSubLedgerFilterExpression(), function (value) {
                $('#<%=txtSubLedgerDtCode.ClientID %>').val(value);
                onTxtSubLedgerDtCodeChanged(value);
            });
        }
    });

    $('#<%=txtSubLedgerDtCode.ClientID %>').change(function () {
        onTxtSubLedgerDtCodeChanged($(this).val());
    });

    function onTxtSubLedgerDtCodeChanged(value) {
        if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
            var filterExpression = onGetSubLedgerFilterExpression() + " AND " + $('#<%=hdnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
            Methods.getObject($('#<%=hdnMethodName.ClientID %>').val(), filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSubLedgerDtID.ClientID %>').val(result[$('#<%=hdnIDFieldName.ClientID %>').val()]);
                    $('#<%=txtSubLedgerDtName.ClientID %>').val(result[$('#<%=hdnDisplayFieldName.ClientID %>').val()]);
                }
                else {
                    $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                }
            });
        }
    }
    //#endregion

    //#region Department
    function onGetDepartmentFilterExpression() {
        var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsActive = 1";
        return filterExpression;
    }

    $('#<%=lblDepartment.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('departmentakunt', onGetDepartmentFilterExpression(), function (value) {
            $('#<%=txtDepartmentID.ClientID %>').val(value);
            ontxtDepartmentIDChanged(value);
        });
    });

    $('#<%=txtDepartmentID.ClientID %>').live('change', function () {
        var param = $('#<%=txtDepartmentID.ClientID %>').val();
        ontxtDepartmentIDChanged(param);
    });

    function ontxtDepartmentIDChanged(value) {
        var filterExpression = onGetDepartmentFilterExpression() + " AND DepartmentID = '" + value + "'";
        Methods.getObject('GetDepartmentList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDepartmentID.ClientID %>').val(result.DepartmentID);
                $('#<%=txtDepartmentName.ClientID %>').val(result.DepartmentName);
            }
            else {
                $('#<%=hdnDepartmentID.ClientID %>').val('');
                $('#<%=txtDepartmentID.ClientID %>').val('');
                $('#<%=txtDepartmentName.ClientID %>').val('');
            }

            checkRevenueCostCenter();
        });
    }
    //#endregion

    //#region Service Unit
    function onGetServiceUnitFilterExpression() {
        var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitakunt', onGetServiceUnitFilterExpression(), function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            ontxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
        var param = $('#<%=txtServiceUnitCode.ClientID %>').val();
        ontxtServiceUnitCodeChanged(param);
    });

    function ontxtServiceUnitCodeChanged(value) {
        var filterExpression = onGetServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetServiceUnitMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }

            checkRevenueCostCenter();
        });
    }
    //#endregion

    function checkRevenueCostCenter() {
        var filter = "IsDeleted = 0";
        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
        var serviceUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();
        var healthcareServiceUnitID = "0";

        if (departmentID != "" && serviceUnitID != "" && serviceUnitID != "0") {
            var filterHSU = "IsDeleted = 0";
            if (departmentID != "") {
                filterHSU += " AND DepartmentID = '" + departmentID + "'";
            }
            if (serviceUnitID != "" && serviceUnitID != "0") {
                filterHSU += " AND ServiceUnitID = " + serviceUnitID;
            }

            Methods.getObject('GetvHealthcareServiceUnitList', filterHSU, function (result) {
                if (result != null) {
                    healthcareServiceUnitID = result.HealthcareServiceUnitID;
                }
            });
        }

        filter += " AND HealthcareServiceUnitID = " + healthcareServiceUnitID;

        Methods.getObject('GetvRevenueCostCenterDtList', filter, function (resultRCC) {
            if (resultRCC != null) {
                $('#<%=hdnRevenueCostCenterID.ClientID %>').val(resultRCC.RevenueCostCenterID);
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val(resultRCC.RevenueCostCenterCode);
                $('#<%=txtRevenueCostCenterName.ClientID %>').val(resultRCC.RevenueCostCenterName);
            }
            else {
                $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                $('#<%=txtRevenueCostCenterName.ClientID %>').val('');

            }
        });
    }

    //#region Revenue Cost Center
    function onRevenueCostCenterFilterExpression() {
        var filter = "IsDeleted = 0";

        return filter;
    }

    $('#lblRevenueCostCenter.lblLink').live('click', function () {
        openSearchDialog('revenuecostcenter', onRevenueCostCenterFilterExpression(), function (value) {
            $('#<%=txtRevenueCostCenterCode.ClientID %>').val(value);
            ontxtRevenueCostCenterCodeChanged(value);
        });
    });

    $('#<%=txtRevenueCostCenterCode.ClientID %>').live('change', function () {
        var param = $('#<%=txtRevenueCostCenterCode.ClientID %>').val();
        ontxtRevenueCostCenterCodeChanged(param);
    });

    function ontxtRevenueCostCenterCodeChanged(value) {
        var filterExpression = "RevenueCostCenterCode = '" + $('#<%=txtRevenueCostCenterCode.ClientID %>').val() + "'";
        Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);

                var filterRCCDT = "RevenueCostCenterID = " + result.RevenueCostCenterID + " AND IsDeleted = 0";
                Methods.getObject('GetvRevenueCostCenterDtList', filterRCCDT, function (resultDt) {
                    if (resultDt != null) {
                        $('#<%=hdnRevenueCostCenterID.ClientID %>').val(resultDt.RevenueCostCenterID);
                        $('#<%=txtRevenueCostCenterName.ClientID %>').val(resultDt.RevenueCostCenterName);

                        var filterDept = "IsActive = 1 AND DepartmentID = '" + resultDt.DepartmentID + "'";
                        Methods.getObject('GetDepartmentList', filterDept, function (resultDept) {
                            if (resultDept != null) {
                                $('#<%=hdnDepartmentID.ClientID %>').val(resultDept.DepartmentID);
                                $('#<%=txtDepartmentID.ClientID %>').val(resultDept.DepartmentID);
                                $('#<%=txtDepartmentName.ClientID %>').val(resultDept.DepartmentName);
                            }
                        });

                        var filterSrvUnit = "IsDeleted = 0 AND ServiceUnitCode = '" + resultDt.ServiceUnitCode + "'";
                        Methods.getObject('GetServiceUnitMasterList', filterSrvUnit, function (resultSrvUnit) {
                            if (resultSrvUnit != null) {
                                $('#<%=hdnServiceUnitID.ClientID %>').val(resultSrvUnit.ServiceUnitID);
                                $('#<%=txtServiceUnitCode.ClientID %>').val(resultSrvUnit.ServiceUnitCode);
                                $('#<%=txtServiceUnitName.ClientID %>').val(resultSrvUnit.ServiceUnitName);
                            }
                        });
                    }
                });
            }
            else {
                $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                $('#<%=txtRevenueCostCenterName.ClientID %>').val('');

            }
        });
    }
    //#endregion

    //#region Customer Group
    $('#lblCustomerGroup.lblLink').live('click', function () {
        openSearchDialog('customergroup', 'IsDeleted = 0', function (value) {
            $('#<%=txtCustomerGroupCode.ClientID %>').val(value);
            ontxtCustomerGroupCodeChanged(value);
        });
    });

    $('#<%=txtCustomerGroupCode.ClientID %>').live('change', function () {
        var param = $('#<%=txtCustomerGroupCode.ClientID %>').val();
        ontxtCustomerGroupCodeChanged(param);
    });

    function ontxtCustomerGroupCodeChanged(value) {
        var filterExpression = "CustomerGroupCode = '" + $('#<%=txtCustomerGroupCode.ClientID %>').val() + "'";
        Methods.getObject('GetCustomerGroupList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnCustomerGroupID.ClientID %>').val(result.CustomerGroupID);
                $('#<%=txtCustomerGroupName.ClientID %>').val(result.CustomerGroupName);
            }
            else {
                $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                $('#<%=txtCustomerGroupName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Business Partner
    function onGetBusinessPartnerFilterExpression() {
        var filterExpression = "IsDeleted = 0";

        if ($('#<%=hdnCustomerGroupID.ClientID %>').val() != "" && $('#<%=hdnCustomerGroupID.ClientID %>').val() != "0") {
            filterExpression += " AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE CustomerGroupID = " + $('#<%=hdnCustomerGroupID.ClientID %>').val() + ")";
        }

        return filterExpression;
    }

    $('#lblBusinessPartner.lblLink').live('click', function () {
        openSearchDialog('businesspartnersakun', onGetBusinessPartnerFilterExpression(), function (value) {
            $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
            ontxtBusinessPartnerCodeChanged(value);
        });
    });

    $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
        var param = $('#<%=txtBusinessPartnerCode.ClientID %>').val();
        ontxtBusinessPartnerCodeChanged(param);
    });

    function ontxtBusinessPartnerCodeChanged(value) {
        var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + $('#<%=txtBusinessPartnerCode.ClientID %>').val() + "'";
        Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);

                if (result.BusinessPartnerID != "1") {
                    if ($('#<%=hdnCustomerGroupID.ClientID %>').val() == "" || $('#<%=hdnCustomerGroupID.ClientID %>').val() == "0") {
                        var filterCust = "CustomerGroupID = (SELECT CustomerGroupID FROM Customer WHERE BusinessPartnerID = " + result.BusinessPartnerID + ")";
                        Methods.getObject('GetCustomerGroupList', filterCust, function (resultCust) {
                            if (resultCust != null) {
                                $('#<%=hdnCustomerGroupID.ClientID %>').val(resultCust.CustomerGroupID);
                                $('#<%=txtCustomerGroupCode.ClientID %>').val(resultCust.CustomerGroupCode);
                                $('#<%=txtCustomerGroupName.ClientID %>').val(resultCust.CustomerGroupName);
                            }
                            else {
                                $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                                $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                                $('#<%=txtCustomerGroupName.ClientID %>').val('');
                            }
                        });
                    }
                }
            }
            else {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                $('#<%=txtBusinessPartnerName.ClientID %>').val('');

                $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                $('#<%=txtCustomerGroupName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnTemplateID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kode Template")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtTemplateCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Template")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtTemplateName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Template")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtTemplateType" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <input type="hidden" runat="server" id="hdnEntryID" />
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblGLAccount"><%=GetLabel("COA")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnGLAccountID" runat="server" />
                                    <input type="hidden" id="hdnSubLedgerID" runat="server" />
                                    <input type="hidden" id="hdnSearchDialogTypeName" runat="server" />
                                    <input type="hidden" id="hdnIDFieldName" runat="server" />
                                    <input type="hidden" id="hdnCodeFieldName" runat="server" />
                                    <input type="hidden" id="hdnDisplayFieldName" runat="server" />
                                    <input type="hidden" id="hdnMethodName" runat="server" />
                                    <input type="hidden" id="hdnFilterExpression" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtGLAccountCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtGLAccountName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblSubLedger"><%=GetLabel("Sub Perkiraan")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnSubLedgerDtID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox runat="server" CssClass="required" ID="txtSubLedgerDtCode" Width="100%" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox runat="server" ID="txtSubLedgerDtName" ReadOnly="true" Width="100%" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblMandatory" id="lblHealthcare">
                                        <%=GetLabel("Healthcare")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboHealthcare" ClientInstanceName="cboHealthcare" Width="40%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                    <input type="hidden" id="hdnHealthcare" runat="server" />
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblDepartment" runat="server">
                                        <%=GetLabel("Department")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDepartmentID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDepartmentID" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDepartmentName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblServiceUnit" runat="server">
                                        <%=GetLabel("Service Unit")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtServiceUnitCode" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtServiceUnitName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblRevenueCostCenter">
                                        <%=GetLabel("Revenue Cost Center")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnRevenueCostCenterID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueCostCenterCode" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueCostCenterName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblCustomerGroup">
                                        <%=GetLabel("Customer Group")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnCustomerGroupID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCustomerGroupCode" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCustomerGroupName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="padding-top: 2px; padding-left: 0px">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblBusinessPartner">
                                        <%=GetLabel("Business Partner")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 3px" />
                                            <col style="width: 180px" />
                                            <col style="width: 3px" />
                                            <col style="width: 50px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAmountPercentage" runat="server">
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah")%></label></td>
                                <td><asp:TextBox ID="txtAmountPercentage" CssClass="number required" runat="server" Width="175px" /> %</td>
                            </tr>
                            <tr id="trAmount" runat="server">
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah (Rp)")%></label></td>
                                <td><asp:TextBox ID="txtAmount" CssClass="txtCurrency" runat="server" Width="175px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Posisi")%></label></td>
                                <td><asp:RadioButtonList ID="rblPosition" runat="server" RepeatDirection="Horizontal" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Cetak")%></label></td>
                                <td><asp:TextBox ID="txtDisplayOrder" CssClass="number required" runat="server" Width="80px" /></td>
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
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center"><%=GetLabel("DEBET") %></h4>
                                            <asp:GridView ID="grdViewD" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                           <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td><img class="imgEditD imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                                    <td style="width:3px">&nbsp;</td>
                                                                    <td><img class="imgDeleteD imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                                </tr>
                                                            </table>
                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountID") %>" bindingfield="GLAccountID" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountNo") %>" bindingfield="GLAccountNo" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountName") %>" bindingfield="GLAccountName" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerID") %>" bindingfield="SubLedgerID" />
                                                            <input type="hidden" value="<%#:Eval("SearchDialogTypeName") %>" bindingfield="SearchDialogTypeName" />
                                                            <input type="hidden" value="<%#:Eval("IDFieldName") %>" bindingfield="IDFieldName" />
                                                            <input type="hidden" value="<%#:Eval("CodeFieldName") %>" bindingfield="CodeFieldName" />
                                                            <input type="hidden" value="<%#:Eval("DisplayFieldName") %>" bindingfield="DisplayFieldName" />
                                                            <input type="hidden" value="<%#:Eval("MethodName") %>" bindingfield="MethodName" />
                                                            <input type="hidden" value="<%#:Eval("FilterExpression") %>" bindingfield="FilterExpression" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtID") %>" bindingfield="SubLedgerDtID" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtCode") %>" bindingfield="SubLedgerDtCode" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtName") %>" bindingfield="SubLedgerDtName" />
                                                            <input type="hidden" value="<%#:Eval("AmountPercentage") %>" bindingfield="AmountPercentage" />
                                                            <input type="hidden" value="<%#:Eval("Amount") %>" bindingfield="Amount" />
                                                            <input type="hidden" value="<%#:Eval("Position") %>" bindingfield="Position" />
                                                            <input type="hidden" value="<%#:Eval("DisplayOrder") %>" bindingfield="DisplayOrder" />
                                                            <input type="hidden" value="<%#:Eval("DepartmentID") %>" bindingfield="DepartmentID" />
                                                            <input type="hidden" value="<%#:Eval("DepartmentName") %>" bindingfield="DepartmentName" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitID") %>" bindingfield="ServiceUnitID" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitCode") %>" bindingfield="ServiceUnitCode" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitName") %>" bindingfield="ServiceUnitName" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterID") %>" bindingfield="RevenueCostCenterID" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterCode") %>" bindingfield="RevenueCostCenterCode" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterName") %>" bindingfield="RevenueCostCenterName" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupID") %>" bindingfield="CustomerGroupID" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupCode") %>" bindingfield="CustomerGroupCode" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupName") %>" bindingfield="CustomerGroupName" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="BusinessPartnerID" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="COA">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;"><%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;"><%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                        
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">DP: <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">SU: <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">RC: <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">CG: <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">BP: <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                        
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center"><%=GetLabel("KREDIT") %></h4>
                                            <asp:GridView ID="grdViewK" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                           <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td><img class="imgEditK imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                                    <td style="width:3px">&nbsp;</td>
                                                                    <td><img class="imgDeleteK imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                                </tr>
                                                            </table>
                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountID") %>" bindingfield="GLAccountID" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountNo") %>" bindingfield="GLAccountNo" />
                                                            <input type="hidden" value="<%#:Eval("GLAccountName") %>" bindingfield="GLAccountName" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerID") %>" bindingfield="SubLedgerID" />
                                                            <input type="hidden" value="<%#:Eval("SearchDialogTypeName") %>" bindingfield="SearchDialogTypeName" />
                                                            <input type="hidden" value="<%#:Eval("IDFieldName") %>" bindingfield="IDFieldName" />
                                                            <input type="hidden" value="<%#:Eval("CodeFieldName") %>" bindingfield="CodeFieldName" />
                                                            <input type="hidden" value="<%#:Eval("DisplayFieldName") %>" bindingfield="DisplayFieldName" />
                                                            <input type="hidden" value="<%#:Eval("MethodName") %>" bindingfield="MethodName" />
                                                            <input type="hidden" value="<%#:Eval("FilterExpression") %>" bindingfield="FilterExpression" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtID") %>" bindingfield="SubLedgerDtID" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtCode") %>" bindingfield="SubLedgerDtCode" />
                                                            <input type="hidden" value="<%#:Eval("SubLedgerDtName") %>" bindingfield="SubLedgerDtName" />
                                                            <input type="hidden" value="<%#:Eval("AmountPercentage") %>" bindingfield="AmountPercentage" />
                                                            <input type="hidden" value="<%#:Eval("Amount") %>" bindingfield="Amount" />
                                                            <input type="hidden" value="<%#:Eval("Position") %>" bindingfield="Position" />
                                                            <input type="hidden" value="<%#:Eval("DisplayOrder") %>" bindingfield="DisplayOrder" />
                                                            <input type="hidden" value="<%#:Eval("DepartmentID") %>" bindingfield="DepartmentID" />
                                                            <input type="hidden" value="<%#:Eval("DepartmentName") %>" bindingfield="DepartmentName" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitID") %>" bindingfield="ServiceUnitID" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitCode") %>" bindingfield="ServiceUnitCode" />
                                                            <input type="hidden" value="<%#:Eval("ServiceUnitName") %>" bindingfield="ServiceUnitName" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterID") %>" bindingfield="RevenueCostCenterID" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterCode") %>" bindingfield="RevenueCostCenterCode" />
                                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterName") %>" bindingfield="RevenueCostCenterName" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupID") %>" bindingfield="CustomerGroupID" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupCode") %>" bindingfield="CustomerGroupCode" />
                                                            <input type="hidden" value="<%#:Eval("CustomerGroupName") %>" bindingfield="CustomerGroupName" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="BusinessPartnerID" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                            <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Perkiraan">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;"><%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;"><%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">DP: <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">SU: <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">RC: <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">CG: <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">BP: <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

