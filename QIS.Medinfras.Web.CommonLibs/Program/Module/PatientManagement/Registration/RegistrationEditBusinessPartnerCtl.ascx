<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationEditBusinessPartnerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationEditBusinessPartnerCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_registrationeditbusinesspartnerctl">
    function onCboPayerValueChangedCtl(s) {
        setTblPayerCompanyVisibilityCtl();
        getPayerCompanyCtl('');
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '') {
            getCoverageTypeCtl('');
        }
    }

    function setTblPayerCompanyVisibilityCtl() {
        var customerType = cboRegistrationPayerCtl.GetValue();
        if (customerType == "<%:GetCustomerTypePersonal() %>") {
            $('#<%:tblPayerCompanyCtl.ClientID %>').hide();
            $('#<%:chkUsingCOBCtl.ClientID %>').attr('style', 'display:none');
        }
        else {
            if (customerType == "<%:GetCustomerTypeHealthcare() %>") {
                $('#<%:trEmployeeCtl.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trEmployeeCtl.ClientID %>').attr('style', 'display:none');
            }
            $('#<%:tblPayerCompanyCtl.ClientID %>').show();
            $('#<%:chkUsingCOBCtl.ClientID %>').removeAttr('style');
            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
        }
    }

    //#region Payer Company
    function getPayerCompanyFilterExpressionCtl() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayerCtl.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    $('#<%:lblPayerCompanyCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('payer', getPayerCompanyFilterExpressionCtl(), function (value) {
            $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val(value);
            ontxtPayerCompanyCodeCtlChanged(value);
        });
    });

    $('#<%:txtPayerCompanyCodeCtl.ClientID %>').live('change', function () {
        ontxtPayerCompanyCodeCtlChanged($(this).val());
    });

    function ontxtPayerCompanyCodeCtlChanged(value) {
        var filterExpression = getPayerCompanyFilterExpressionCtl() + " AND BusinessPartnerCode = '" + value + "'";
        getPayerCompanyCtl(filterExpression);
    }

    function getPayerCompanyCtl(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getPayerCompanyFilterExpressionCtl();
        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';

            if (result != null) {
                $('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val(result.IsBlackList);
                if ($('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val() == 'false') {
                    $('#<%:hdnPayerIDCtl.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%:txtPayerCompanyNameCtl.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%:hdnGCTariffSchemeCtl.ClientID %>').val(result.GCTariffScheme);

                    var filterExpression = getPayerContractFilterExpression();
                    Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                        if (result == 1) {
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%:hdnContractIDCtl.ClientID %>').val(result.ContractID);
                                    $('#<%:txtContractNoCtl.ClientID %>').val(result.ContractNo);
                                    $('#<%:hdnContractCoverageCountCtl.ClientID %>').val(result.ContractCoverageCount);

                                    if (result.IsControlCoverageLimit) {
                                        $('#<%:trCoverageLimitCtl.ClientID %>').show();
                                        if ($('#<%:hdnDepartmentIDCtl.ClientID %>').val() == Constant.Facility.INPATIENT)
                                            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
                                    }
                                    else {
                                        $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                                        $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                                        $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                                    }
                                    $('#<%:hdnIsControlClassCareCtl.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%:trControlClassCareCtl.ClientID %>').show();
                                        if (result.ControlClassID != '0') {
                                            cboControlClassCareCtl.SetValue(result.ControlClassID);
                                        }
                                        else {
                                            var deptID = $('#<%:hdnDepartmentIDCtl.ClientID %>').val();
                                            var classID = $('#<%:hdnClassIDCtl.ClientID %>').val();
                                            var chargeClassID = $('#<%:hdnChargeClassIDCtl.ClientID %>').val();

                                            if (chargeClassID == "") {
                                                if (deptID != "INPATIENT") {
                                                    chargeClassID = classID;
                                                }
                                            }
                                            cboControlClassCareCtl.SetValue(chargeClassID);
                                        }
                                    }
                                    else {
                                        $('#<%:trControlClassCareCtl.ClientID %>').hide();
                                        cboControlClassCareCtl.SetValue('');
                                    }
                                    onAfterContractNoChanged();
                                    onCheckCustomerMember($('#<%:hdnPayerIDCtl.ClientID %>').val(), $('#<%:txtMRNCtl.ClientID %>').val());
                                }
                            });
                        }
                        else {
                            $('#<%:hdnContractIDCtl.ClientID %>').val('');
                            $('#<%:txtContractNoCtl.ClientID %>').val('');
                            $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                            $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                            $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                            $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                            $('#<%:trControlClassCareCtl.ClientID %>').hide();

                            $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                        }
                    });
                }
                else {
                    showToast(messageBlacklistPayer);
                    cboRegistrationPayerCtl.SetValue("<%:GetCustomerTypePersonal() %>");
                    $('#<%:chkUsingCOBCtl.ClientID %>').hide();
                    $('#<%:tblPayerCompanyCtl.ClientID %>').hide();
                }
            }
            else {
                $('#<%:hdnIsBlacklistPayerCtl.ClientID %>').val('0');
                $('#<%:hdnPayerIDCtl.ClientID %>').val('');
                $('#<%:txtPayerCompanyCodeCtl.ClientID %>').val('');
                $('#<%:txtPayerCompanyNameCtl.ClientID %>').val('');
                $('#<%:hdnGCTariffSchemeCtl.ClientID %>').val('');

                $('#<%:hdnContractIDCtl.ClientID %>').val('');
                $('#<%:txtContractNoCtl.ClientID %>').val('');
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                $('#<%:trControlClassCareCtl.ClientID %>').hide();
                $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');

                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
            }
        });
    }

    function onCheckCustomerMember(payerID, medicalNoID) {
        if (payerID != '' && medicalNoID != '') {
            var filterExpression = "MedicalNo = '" + medicalNoID + "' AND BusinessPartnerID = '" + payerID + "'";
            Methods.getObject('GetvCustomerMemberList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:txtParticipantNoCtl.ClientID %>').val(result.MemberNo);
                    $('#<%:txtParticipantNoCtl.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%:txtParticipantNoCtl.ClientID %>').val('');
                    $('#<%:txtParticipantNoCtl.ClientID %>').removeAttr('readonly');
                }
            });
        }
    }
    //#endregion

    //#region Employee
    function getEmployeeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }
    $('#<%:lblEmployeeCtl.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
            $('#<%:txtEmployeeCodeCtl.ClientID %>').val(value);
            ontxtEmployeeCodeCtlChanged(value);
        });
    });

    $('#<%:txtEmployeeCodeCtl.ClientID %>').change(function () {
        ontxtEmployeeCodeCtlChanged($(this).val());
    });

    function ontxtEmployeeCodeCtlChanged(value) {
        var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
        Methods.getObject('GetEmployeeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnEmployeeIDCtl.ClientID %>').val(result.EmployeeID);
                $('#<%:txtEmployeeNameCtl.ClientID %>').val(result.FullName);
            }
            else {
                $('#<%:hdnEmployeeIDCtl.ClientID %>').val('');
                $('#<%:txtEmployeeCodeCtl.ClientID %>').val('');
                $('#<%:txtEmployeeNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Payer Contract
    function getPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerIDCtl.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblContractCtl.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnPayerIDCtl.ClientID %>').val() != '') {
            openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                $('#<%:txtContractNoCtl.ClientID %>').val(value);
                onTxtPayerContractNoChanged(value);
            });
        }
    });

    $('#<%:txtContractNoCtl.ClientID %>').live('change', function () {
        if ($('#<%:hdnPayerIDCtl.ClientID %>').val() != '')
            onTxtPayerContractNoChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtPayerContractNoChanged(value) {
        var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnContractIDCtl.ClientID %>').val(result.ContractID);
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val(result.ContractCoverageCount);
                if (result.IsControlCoverageLimit) {
                    $('#<%:trCoverageLimitCtl.ClientID %>').show();
                    if ($('#<%:hdnDepartmentIDCtl.ClientID %>').val() == Constant.Facility.INPATIENT)
                        $('#<%:trCoverageLimitPerDayCtl.ClientID %>').show();
                }
                else {
                    $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                    $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                    $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                }

                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                if (result.IsControlClassCare) {
                    $('#<%:trControlClassCareCtl.ClientID %>').show();
                    cboControlClassCareCtl.SetValue(result.ControlClassID);
                }
                else {
                    $('#<%:trControlClassCareCtl.ClientID %>').hide();
                    cboControlClassCareCtl.SetValue('');
                }

                onAfterContractNoChanged();
            }
            else {
                $('#<%:hdnContractIDCtl.ClientID %>').val('');
                $('#<%:txtContractNoCtl.ClientID %>').val('');
                $('#<%:hdnContractCoverageCountCtl.ClientID %>').val('');
                $('#<%:trCoverageLimitCtl.ClientID %>').hide();
                $('#<%:trCoverageLimitPerDayCtl.ClientID %>').hide();
                $('#<%:trControlClassCareCtl.ClientID %>').hide();
                $('#<%:txtCoverageLimitCtl.ClientID %>').val('0').trigger('changeValue');
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                $('#<%:hdnIsControlClassCareCtl.ClientID %>').val('0');
            }
        });
    }

    function onAfterContractNoChanged() {
        var MRN = $('#<%:hdnMRNCtl.ClientID %>').val();
        if (MRN != '') {
            var filterExpression = 'ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val();
            Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                $('#<%:hdnContractCoverageMemberCountCtl.ClientID %>').val(result);
                if (result == 1) {
                    filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val() + ') AND IsDeleted = 0';
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                            $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCountCtl.ClientID %>').val());
                    if (contractCoverageRowCount == 1) {
                        var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractIDCtl.ClientID %>').val() + ") AND IsDeleted = 0";
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                        $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                        $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
                    }
                }
            });
        }
    }
    //#endregion

    //#region Coverage Type
    function getCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnContractCoverageMemberCountCtl.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCountCtl.ClientID %>').val());
        var payerID = parseInt($('#<%:hdnPayerIDCtl.ClientID %>').val());

        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnContractIDCtl.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnMRNCtl.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractIDCtl.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";

        filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
        return filterExpression;
    }

    $('#<%:lblCoverageTypeCtl.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '') {
            openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(value);
                ontxtCoverageTypeCodeCtlChanged(value);
            });
        }
    });

    $('#<%:txtCoverageTypeCodeCtl.ClientID %>').live('change', function () {
        if ($('#<%:hdnContractIDCtl.ClientID %>').val() != '')
            ontxtCoverageTypeCodeCtlChanged($(this).val());
        else
            $(this).val('');
    });

    function ontxtCoverageTypeCodeCtlChanged(value) {
        var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
        getCoverageTypeCtl(filterExpression);
    }

    function getCoverageTypeCtl(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val(result.CoverageTypeID);
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val(result.CoverageTypeCode);
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                $('#<%:hdnCoverageTypeIDCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeCodeCtl.ClientID %>').val('');
                $('#<%:txtCoverageTypeNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 400px; overflow-y: scroll; overflow-x: hidden">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnClassIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnChargeClassIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnGCCustomerTypeCtl" value="" />
    <input type="hidden" runat="server" id="hdnGCTariffSchemePersonalCtl" />
    <input type="hidden" runat="server" id="hdnParamedicID" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col />
            <col style="width: 100px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNoCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("No RM")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMRNCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Nama Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatientNameCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
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
                            <dxe:ASPxComboBox ID="cboRegistrationPayerCtl" ClientInstanceName="cboRegistrationPayerCtl"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChangedCtl(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                        </td>
                        <td id="tdChkUsingCOBCtl" runat="server">
                            <asp:CheckBox ID="chkUsingCOBCtl" Checked="false" runat="server" Text="Peserta COB" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblEntryContent" runat="server" style="width: 100%;" id="tblPayerCompanyCtl">
        <tr>
            <td style="width: 30%" class="tdLabel">
                <label class="lblLink lblMandatory" runat="server" id="lblPayerCompanyCtl">
                    <%:GetLabel("Instansi")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPayerIDCtl" value="" runat="server" />
                <input type="hidden" id="hdnGCTariffSchemeCtl" value="" runat="server" />
                <input type="hidden" id="hdnIsBlacklistPayerCtl" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPayerCompanyCodeCtl" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayerCompanyNameCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 30%" class="tdLabel">
                <label class="lblLink lblMandatory" runat="server" id="lblContractCtl">
                    <%:GetLabel("Kontrak")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnContractIDCtl" value="" runat="server" />
                <input type="hidden" id="hdnContractCoverageCountCtl" value="" runat="server" />
                <input type="hidden" id="hdnContractCoverageMemberCountCtl" value="" runat="server" />
                <asp:TextBox ID="txtContractNoCtl" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 30%" class="tdLabel">
                <label class="lblLink lblMandatory" runat="server" id="lblCoverageTypeCtl">
                    <%:GetLabel("Skema Penjaminan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnCoverageTypeIDCtl" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCoverageTypeCodeCtl" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoverageTypeNameCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("No. Peserta")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtParticipantNoCtl" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trEmployeeCtl" runat="server">
            <td class="tdLabel">
                <label class="lblLink lblMandatory" runat="server" id="lblEmployeeCtl">
                    <%:GetLabel("Pegawai")%></label>
            </td>
            <td>
                <input type="hidden" runat="server" id="hdnEmployeeIDCtl" value="" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEmployeeCodeCtl" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeNameCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trControlClassCareCtl" runat="server">
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%:GetLabel("Jatah Kelas")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnIsControlClassCareCtl" value="" runat="server" />
                <dxe:ASPxComboBox ID="cboControlClassCareCtl" ClientInstanceName="cboControlClassCareCtl"
                    Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trCoverageLimitCtl" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Batas Tanggungan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtCoverageLimitCtl" CssClass="txtCurrency" Width="100%" runat="server" />
            </td>
        </tr>
        <tr id="trCoverageLimitPerDayCtl" runat="server">
            <td class="tdLabel">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="chkIsCoverageLimitPerDayCtl" runat="server" /><%:GetLabel("Coverage Limit Per Hari")%>
            </td>
        </tr>
    </table>
</div>
