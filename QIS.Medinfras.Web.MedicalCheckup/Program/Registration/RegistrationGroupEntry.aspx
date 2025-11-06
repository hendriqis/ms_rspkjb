<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RegistrationGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.RegistrationGroupEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" id="hdnGCTariffSchemePersonal" runat="server" />
    <input type="hidden" id="hdnItemCardFee" runat="server" />
    <input type="hidden" id="hdnIsControlPatientCardPayment" runat="server" />
    <input type="hidden" id="hdnIsControlAdmCost" runat="server" />
    <input type="hidden" id="hdnDefaultGCAdmissionSource" runat="server" />
    <input type="hidden" id="hdnLastParamedicID" runat="server" />
    <input type="hidden" id="hdnLastParamedicCode" runat="server" />
    <input type="hidden" id="hdnLastParamedicName" runat="server" />
    <input type="hidden" id="hdnLastSpecialty" runat="server" />
    <input type="hidden" id="hdnClassID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnIsServiceUnitHasParamedic" runat="server" />
    <input type="hidden" id="hdnIsServiceUnitHasVisitType" runat="server" />
    <input type="hidden" id="hdnIsAdd" runat="server" />
    <input type="hidden" id="hdnIsControlAdministrationCharges" runat="server" />
    <input type="hidden" id="hdnIsWarningPatientHaveAR" runat="server" />
    <input type="hidden" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
    <input type="hidden" id="hdnIsOutpatientUsingRoom" value="0" runat="server" />
    <input type="hidden" id="hdnIsCheckNewPatient" value="0" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setRightPanelButtonEnabled();
        });

        $('#btnUploadPatientData').live('click', function () {
            upcFileUploadExcel.Upload();
        });

        function onAfterCustomClickSuccess(type, paramUrl) {
            var url = ResolveUrl(paramUrl);
            showLoadingPanel();
            window.location.href = url;
        }

        function onChangeHideLoadingPanel() {
            return false;
        }

        var isOpenPatientIdentityPopupFromAppointment = false;

        var maxBackDate = parseInt('<%:maxBackDate %>');
        function onLoad() {
            setCustomToolbarVisibility();
            var attr = $('#<%:txtRegistrationDate.ClientID %>').attr('readonly');

            if (typeof attr !== 'undefined' && attr !== false) { }
            else {
                setDatePicker('<%:txtRegistrationDate.ClientID %>');
                $('#<%:txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
                $('#<%:txtRegistrationDate.ClientID %>').datepicker('option', 'minDate', '-' + maxBackDate);
            }

            var vt = $('#<%:txtVisitTypeCode.ClientID %>').val();
            if (vt != '') {
                $('#<%:txtVisitTypeCode.ClientID %>').trigger('change');
            }
        }

        function setRightPanelButtonEnabled() {
            
        }

        function setCustomToolbarVisibility() {
           
        }
        
        //#region Physician
        function onGetPhysicianFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var filterExpression = 'IsDeleted = 0';
            if (serviceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ") AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%:txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    cboSpecialty.SetValue(result.SpecialtyID);
                    $('#<%:hdnLastParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:hdnLastParamedicCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%:hdnLastParamedicName.ClientID %>').val(result.ParamedicName);
                    $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    var healthcareServiceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                    if ($('#<%:txtVisitTypeCode.ClientID %>').val() == '') {
                        var filterExpression = onGetVisitTypeFilterExpression();
                        Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                $('#<%:txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                                $('#<%:txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                            }
                            else {
                                $('#<%:hdnVisitTypeID.ClientID %>').val('');
                                $('#<%:txtVisitTypeCode.ClientID %>').val('');
                                $('#<%:txtVisitTypeName.ClientID %>').val('');
                            }
                        });
                    }
                }
                else {
                    cboSpecialty.SetValue('');
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
        }

        function onCboSpecialtyValueChanged() {
            $('#<%:hdnLastSpecialty.ClientID %>').val(cboSpecialty.GetValue());
        }
        //#endregion

        //#region Item
        function getItemMasterFilterExpression() {
            var filterExpression = "<%:GetItemMasterFilterExpression() %>";
            return filterExpression;
        }

        $('#<%:lblItemMCU.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('item', getItemMasterFilterExpression(), function (value) {
                $('#<%:txtItemCode.ClientID %>').val(value);
                onTxtItemMasterCodeChanged(value);
            });
        });

        $('#<%:txtItemCode.ClientID %>').live('change', function () {
            onTxtItemMasterCodeChanged($(this).val());
        });

        function onTxtItemMasterCodeChanged(value) {
            var filterExpression = getItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%:txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%:hdnItemID.ClientID %>').val('');
                    $('#<%:txtItemCode.ClientID %>').val('');
                    $('#<%:txtItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion

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

        //#region Payer Company
        function getPayerCompanyFilterExpression() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblPayerCompany.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
                $('#<%:txtPayerCompanyCode.ClientID %>').val(value);
                onTxtPayerCompanyCodeChanged(value);
            });
        });

        $('#<%:txtPayerCompanyCode.ClientID %>').live('change', function () {
            onTxtPayerCompanyCodeChanged($(this).val());
        });

        function onTxtPayerCompanyCodeChanged(value) {
            var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            getPayerCompany(filterExpression);
        }

        function getPayerCompany(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getPayerCompanyFilterExpression();
            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                    $('#btnPayerNotesDetail').removeAttr('enabled');
                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
                                    $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                    if (result.IsControlCoverageLimit) {
                                        $('#<%:trCoverageLimit.ClientID %>').show();
                                        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                                            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
                                    }
                                    else {
                                        $('#<%:trCoverageLimit.ClientID %>').hide();
                                        $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                                        $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                                    }
                                    $('#<%:hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%:trControlClassCare.ClientID %>').show();
                                        cboControlClassCare.SetValue(result.ControlClassID);
                                    }
                                    else {
                                        $('#<%:trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractID.ClientID %>').val('');
                            $('#<%:txtContractNo.ClientID %>').val('');
                            $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                            $('#<%:trCoverageLimit.ClientID %>').hide();
                            $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                            $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                            $('#<%:trControlClassCare.ClientID %>').hide();

                            $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                            $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%:hdnPayerID.ClientID %>').val('');
                    $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                    $('#<%:txtPayerCompanyName.ClientID %>').val('');
                    $('#btnPayerNotesDetail').attr('enabled', 'false');
                    $('#<%:hdnGCTariffScheme.ClientID %>').val('');

                    $('#<%:hdnContractID.ClientID %>').val('');
                    $('#<%:txtContractNo.ClientID %>').val('');
                    $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                    $('#<%:trCoverageLimit.ClientID %>').hide();
                    $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                    $('#<%:trControlClassCare.ClientID %>').hide();
                    $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');

                    $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Employee
        function getEmployeeFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }
        $('#<%:lblEmployee.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
                $('#<%:txtEmployeeCode.ClientID %>').val(value);
                onTxtEmployeeCodeChanged(value);
            });
        });

        $('#<%:txtEmployeeCode.ClientID %>').change(function () {
            onTxtEmployeeCodeChanged($(this).val());
        });

        function onTxtEmployeeCodeChanged(value) {
            var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
            Methods.getObject('GetEmployeeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                    $('#<%:txtEmployeeName.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%:hdnEmployeeID.ClientID %>').val('');
                    $('#<%:txtEmployeeCode.ClientID %>').val('');
                    $('#<%:txtEmployeeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnPayerNotesDetail').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var id = $('#<%:hdnPayerID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/CustomerNotesDetailCtl.ascx");
                openUserControlPopup(url, id, 'Notes', 500, 400);
            }
        });

        //#region Payer Contract
        function getPayerContractFilterExpression() {
            var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblContract.ClientID %>.lblLink').live('click', function () {
            if ($('#<%:hdnPayerID.ClientID %>').val() != '') {
                openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                    $('#<%:txtContractNo.ClientID %>').val(value);
                    onTxtPayerContractNoChanged(value);
                });
            }
        });

        $('#<%:txtContractNo.ClientID %>').live('change', function () {
            if ($('#<%:hdnPayerID.ClientID %>').val() != '')
                onTxtPayerContractNoChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtPayerContractNoChanged(value) {
            var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                    $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                    if (result.IsControlCoverageLimit) {
                        $('#<%:trCoverageLimit.ClientID %>').show();
                        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT)
                            $('#<%:trCoverageLimitPerDay.ClientID %>').show();
                    }
                    else {
                        $('#<%:trCoverageLimit.ClientID %>').hide();
                        $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                        $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                    }

                    $('#<%:hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                    if (result.IsControlClassCare) {
                        $('#<%:trControlClassCare.ClientID %>').show();
                        cboControlClassCare.SetValue(result.ControlClassID);
                    }
                    else {
                        $('#<%:trControlClassCare.ClientID %>').hide();
                        cboControlClassCare.SetValue('');
                    }

                    onAfterContractNoChanged();
                }
                else {
                    $('#<%:hdnContractID.ClientID %>').val('');
                    $('#<%:txtContractNo.ClientID %>').val('');
                    $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                    $('#<%:trCoverageLimit.ClientID %>').hide();
                    $('#<%:trCoverageLimitPerDay.ClientID %>').hide();
                    $('#<%:trControlClassCare.ClientID %>').hide();
                    $('#<%:txtCoverageLimit.ClientID %>').val('0').trigger('changeValue');
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                    $('#<%:hdnIsControlClassCare.ClientID %>').val('0');
                }
            });
        }

        function onAfterContractNoChanged() {
            var MRN = $('#<%:hdnMRN.ClientID %>').val();
            if (MRN != '') {
                var filterExpression = 'ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val();
                Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                    $('#<%:hdnContractCoverageMemberCount.ClientID %>').val(result);
                    if (result == 1) {
                        filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                                $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                                $('#<%:txtCoverageTypeName.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
                        if (contractCoverageRowCount == 1) {
                            var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
                            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                                }
                                else {
                                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%:txtCoverageTypeName.ClientID %>').val('');
                        }
                    }
                });
            }
        }
        //#endregion

        //#region Diagnose
        $('#<%:lblDiagnose.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                onTxtDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
            onTxtDiagnoseCodeChanged($(this).val());
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val(result.INACBGLabel);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                    $('#<%=hdnBPJSDiagnoseCode.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Coverage Type
        function getCoverageTypeFilterExpression() {
            var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCount.ClientID %>').val());
            var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
            var filterExpression = '';
            if (contractCoverageMemberRowCount > 0)
                filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
            else if (contractCoverageRowCount > 0)
                filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblCoverageType.ClientID %>.lblLink').live('click', function () {
            if ($('#<%:hdnContractID.ClientID %>').val() != '') {
                openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(value);
                    onTxtCoverageTypeCodeChanged(value);
                });
            }
        });

        $('#<%:txtCoverageTypeCode.ClientID %>').live('change', function () {
            if ($('#<%:hdnContractID.ClientID %>').val() != '')
                onTxtCoverageTypeCodeChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtCoverageTypeCodeChanged(value) {
            var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
            getCoverageType(filterExpression);
        }

        function getCoverageType(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboReferralValueChanged(s) {
            if (getIsAdd()) {
                if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                    $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
                    $('#<%:txtReferralNo.ClientID %>').removeAttr('readonly');
                }
                else {
                    $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:txtReferralNo.ClientID %>').attr('readonly', 'readonly');
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    $('#<%:txtReferralNo.ClientID %>').val('');

                }
            }
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%:txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%:txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        function onCboPayerValueChanged(s) {
            setTblPayerCompanyVisibility();
            getPayerCompany('');
            if ($('#<%:hdnContractID.ClientID %>').val() != '') {
                getCoverageType('');
            }
        }

        function setTblPayerCompanyVisibility() {
            var customerType = cboRegistrationPayer.GetValue();
            if (customerType == "<%:GetCustomerTypePersonal() %>") {
                $('#<%:tblPayerCompany.ClientID %>').hide();
                $('#<%:chkUsingCOB.ClientID %>').hide();
            }
            else {
                if (customerType == "<%:GetCustomerTypeHealthcare() %>")
                    $('#<%:trEmployee.ClientID %>').removeAttr('style');
                else
                    $('#<%:trEmployee.ClientID %>').attr('style', 'display:none');
                $('#<%:tblPayerCompany.ClientID %>').show();
                $('#<%:chkUsingCOB.ClientID %>').show();
                $('#<%:trCoverageLimitPerDay.ClientID %>').show();
            }
        }

        function onAfterSaveAddRecord(param) {
            
        }

        function onBeforeOpenTransaction() {
            return ($('#<%=hdnVisitID.ClientID %>').val() != "");
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" value="0" runat="server" />
    <input type="hidden" id="hdnIsNewPatient" value="0" runat="server" />
    <input type="hidden" id="hdnIsAllowBackDate" value="0" runat="server" />
    <input type="hidden" id="hdnMRN" value="" runat="server" />
    <input type="hidden" id="hdnIsBlacklist" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnGuestID" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCSalutation" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCTitle" value="" />
    <input type="hidden" runat="server" id="hdnGuestFirstName" value="" />
    <input type="hidden" runat="server" id="hdnGuestMiddleName" value="" />
    <input type="hidden" runat="server" id="hdnGuestLastName" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCSuffix" value="" />
    <input type="hidden" runat="server" id="hdnGuestSuffix" value="" />
    <input type="hidden" runat="server" id="hdnGuestTitle" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCGender" value="" />
    <input type="hidden" runat="server" id="hdnGuestDateOfBirth" value="" />
    <input type="hidden" runat="server" id="hdnGuestStreetName" value="" />
    <input type="hidden" runat="server" id="hdnGuestCounty" value="" />
    <input type="hidden" runat="server" id="hdnGuestDistrict" value="" />
    <input type="hidden" runat="server" id="hdnGuestCity" value="" />
    <input type="hidden" runat="server" id="hdnGuestPhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestMobilePhoneNo" value="" />
    <input type="hidden" runat="server" id="hdnGuestEmailAddress" value="" />
    <input type="hidden" runat="server" id="hdnGuestGCIdentityNoType" value="" />
    <input type="hidden" runat="server" id="hdnGuestSSN" value="" />
    <input type="hidden" runat="server" id="hdnExtensionNo" value="" />
    <input type="hidden" runat="server" id="hdnAdminID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationStatus" value="" />
    <input type="hidden" runat="server" id="hdnPatientSearchDialogType" value="patient1" />
    <input type="hidden" runat="server" id="hdnGCGender" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowVoid" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Tanggal Pendaftaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td style="padding-left: 30px; padding-right: 10px">
                            <%:GetLabel("Jam Pendaftaran")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationHour" CssClass="time" runat="server" Width="60px"
                                Style="text-align: center" MaxLength="5" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pasien")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                             <asp:CheckBox ID="chkCardFee" runat="server" /><%:GetLabel("Cetak Kartu Pasien")%>
                        </td>
                    </tr>
                    <tr id="trMRN" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblImportFile">
                                <%:GetLabel("Data Pasien (.xls)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 175px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxuc:ASPxUploadControl ID="upcFileUploadExcel" runat="server" ClientInstanceName="upcFileUploadExcel" OnFileUploadComplete="upcFileUploadExcel_FileUploadComplete">
                                            <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(e); }" />
                                        </dxuc:ASPxUploadControl>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <input type="button" id="btnUploadPatientData" value="Upload" style="margin-left: 10px;
                                            width: 150px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <dxcp:ASPxCallbackPanel ID="cbpPatientData" runat="server" Width="100%" ClientInstanceName="cbpPatientData"
                                ShowLoadingPanel="false" OnCallback="cbpPatientData_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientDataEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                            position: relative; font-size: 0.95em;">
                                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="CustomItemUnit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderText="Diminta" HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="BaseUnit" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Satuan Dasar" HeaderStyle-Width="150px" />
                                                    <asp:BoundField DataField="CustomConversion" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Konversi" />
                                                    <asp:BoundField DataField="CustomItemRequest" HeaderStyle-HorizontalAlign="Right"
                                                        ItemStyle-HorizontalAlign="Right" HeaderText="Total Diminta" HeaderStyle-Width="150px" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("Belum Ada Data Pasien Di Upload")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Petugas")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrar" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Kunjungan")%></h4>
                <table class="tblEntryContent" id="tblVisitData" runat="server" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trPhysician" runat="server">
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                <%:GetLabel("Dokter / Tenaga Medis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnParamedicID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Spesialisasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboSpecialty" ClientInstanceName="cboSpecialty" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){
                                    onCboSpecialtyValueChanged();
                                }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trItem" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblItemMCU">
                                <%:GetLabel("Paket MCU")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnItemID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblVisitType">
                                <%:GetLabel("Jenis Kunjungan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
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
                                        <asp:TextBox ID="txtVisitTypeName" Width="100%" runat="server" />
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
                                    <col style="width: 80px" />
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
                </table>
                <fieldset id="fsGenerateSEP" style="margin: 0">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("No. Rujukan")%></label>
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
                                            <asp:TextBox ID="txtReferralNo" Width="100%" runat="server" />
                                        </td>
                                        <td id="txReferral" runat="server">
                                            <input type="button" id="btnReferral" value="Data Rujukan" style="margin-left: 10px;
                                                width: 150px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDiagnose" runat="server">
                                    <%=GetLabel("Diagnosa")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="80px" />
                                        <col width="3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnBPJSDiagnoseCode" value="" runat="server" />
                                            <asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" />
                                        </td>
                                        <td />
                                        <td>
                                            <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" />
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
                                <asp:TextBox ID="txtDiagnoseText" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr id="trAdmissionCondition" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Keadaan Datang")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trVisitReason" runat="server">
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblNormal">
                                <%:GetLabel("Alasan Kunjungan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 30%">
                            <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                            </label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitNotes" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%:GetLabel("Data Pembayar")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                        <col style="width: 100px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Pembayar")%></label>
                        </td>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboRegistrationPayer" ClientInstanceName="cboRegistrationPayer"
                                            Width="100%" runat="server">
                                            <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td id="chkUsingCOB" runat="server">
                                        <asp:CheckBox ID="chkIsUsingCOB" Checked="false" runat="server" /><%:GetLabel("Is Using COB")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPayerCompany">
                    <tr>
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblPayerCompany">
                                <%:GetLabel("Instansi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPayerID" value="" runat="server" />
                            <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                    <col style="width: 3px" />
                                    <col style="width: 20px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input type="button" id="btnPayerNotesDetail" value="..." />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                <%:GetLabel("Kontrak")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnContractID" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                            <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                            <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                <%:GetLabel("Tipe Coverage")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoverageTypeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("No Partisipan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trEmployee" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblEmployee">
                                <%:GetLabel("Pegawai")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trControlClassCare" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Jatah Kelas")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnIsControlClassCare" value="" runat="server" />
                            <dxe:ASPxComboBox ID="cboControlClassCare" ClientInstanceName="cboControlClassCare"
                                Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageLimit" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Batas Tanggungan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trCoverageLimitPerDay" runat="server">
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCoverageLimitPerDay" runat="server" /><%:GetLabel("Coverage Limit Per Hari")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
