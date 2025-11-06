<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayerDetailEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PayerDetailEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_payerdetailentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        clearContainerEntryPopUp();
        $('#<%=hdnRegistrationPayerID.ClientID %>').val("");
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

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        clearContainerEntryPopUp();
        var registrationPayerID = $row.find('.hdnRegistrationPayerID').val();
        var customerType = $row.find('.hdnCustomerType').val();
        var isPrimaryPayer = $row.find('.hdnIsPrimaryPayer').val();
        var businessPartnerID = $row.find('.hdnBusinessPartnerID').val();
        var businessPartnerCode = $row.find('.hdnBusinessPartnerCode').val();
        var businessPartnerName = $row.find('.hdnBusinessPartnerName').val();
        var contractID = $row.find('.hdnContractID').val();
        var contractNo = $row.find('.hdnContractNo').val();
        var coverageTypeID = $row.find('.hdnCoverageTypeID').val();
        var coverageTypeCode = $row.find('.hdnCoverageTypeCode').val();
        var coverageTypeName = $row.find('.hdnCoverageTypeName').val();
        var corporateAccountNo = $row.find('.hdnCorporateAccountNo').val();
        var corporateAccountName = $row.find('.hdnCorporateAccountName').val();
        var coverageLimitAmount = $row.find('.hdnCoverageLimitAmount').val();
        var isCoverageLimitPerDay = $row.find('.hdnIsCoverageLimitPerDay').val();
        var isGuaranteeLetterExists = $row.find('.hdnIsGuaranteeLetterExists').val();
        var controlClassID = $row.find('.hdnControlClassID').val();
        var isControlClassCare = $row.find('.hdnIsControlClassCare').val();

        $('#<%=hdnRegistrationPayerID.ClientID %>').val(registrationPayerID);
        cboCtlRegistrationPayer.SetValue(customerType);
        $('#<%=chkIsPrimary.ClientID %>').attr('checked', false);
        if (isPrimaryPayer == "True") $('#<%=chkIsPrimary.ClientID %>').attr('checked', true);
        $('#<%=hdnCtlPayerCompanyID.ClientID %>').val(businessPartnerID);
        $('#<%=txtCtlPayerCompanyCode.ClientID %>').val(businessPartnerCode);
        $('#<%=txtCtlPayerCompanyName.ClientID %>').val(businessPartnerName);
        $('#<%=hdnCtlContractID.ClientID %>').val(contractID);
        $('#<%=txtCtlContractNo.ClientID %>').val(contractNo);
        $('#<%=hdnCtlCoverageTypeID.ClientID %>').val(coverageTypeID);
        $('#<%=txtCtlCoverageTypeCode.ClientID %>').val(coverageTypeCode);
        $('#<%=txtCtlCoverageTypeName.ClientID %>').val(coverageTypeName);
        $('#<%=txtCorporateAccountNo.ClientID %>').val(corporateAccountNo);
        $('#<%=txtCorporateAccountName.ClientID %>').val(corporateAccountName);
        $('#<%=txtCtlCoverageLimitAmount.ClientID %>').val(coverageLimitAmount).trigger('changeValue');
        $('#<%=chkCtlCoverageLimit.ClientID %>').attr('checked', false);
        if (isCoverageLimitPerDay == "True") $('#<%=chkCtlCoverageLimit.ClientID %>').attr('checked', true);
        if (isControlClassCare == "True") {
            $('#trCtlControlClassCare').show();
            if (controlClassID != 0) {
                cboCtlControlClassCare.SetValue(controlClassID);
            }
        }
        else {
            $('#trCtlControlClassCare').hide();
        }

        $('#<%=chkIsGuaranteeLetterExistsCtlAdd.ClientID %>').attr('checked', false);
        if (isGuaranteeLetterExists == "True") {
            $('#<%=chkIsGuaranteeLetterExistsCtlAdd.ClientID %>').attr('checked', true);
        }
        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var registrationPayerID = $row.find('.hdnRegistrationPayerID').val();
            $('#<%=hdnRegistrationPayerID.ClientID %>').val(registrationPayerID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    function onCboCtlRegistrationPayerValueChanged(s) {
        clearPayerCompany();
        $('#<%=txtCorporateAccountNo.ClientID %>').val('');
        $('#<%=txtCorporateAccountName.ClientID %>').val('');
        $('#<%=chkIsPrimary.ClientID %>').attr('checked', false);
        $('#<%=txtCtlCoverageLimitAmount.ClientID %>').val('0').trigger('changeValue');
    }

    function clearContainerEntryPopUp() {
        cboCtlRegistrationPayer.SetValue('');
        clearPayerCompany();
        $('#<%=txtCorporateAccountNo.ClientID %>').val('');
        $('#<%=txtCorporateAccountName.ClientID %>').val('');
        $('#<%=chkIsPrimary.ClientID %>').attr('checked', false);
        $('#<%=txtCtlCoverageLimitAmount.ClientID %>').val('0').trigger('changeValue');
    }

    function clearPayerCompany() {
        $('#<%:hdnCtlPayerCompanyID.ClientID %>').val('');
        $('#<%:txtCtlPayerCompanyCode.ClientID %>').val('');
        $('#<%:txtCtlPayerCompanyName.ClientID %>').val('');
        clearContract();
    }

    function clearContract() {
        $('#<%:hdnCtlContractID.ClientID %>').val('');
        $('#<%:txtCtlContractNo.ClientID %>').val('');
        $('#<%:hdnCtlContractCoverageCount.ClientID %>').val('');
        $('#<%:hdnCtlIsControlClassCare.ClientID %>').val('');
        clearCoverageType();
    }

    function clearCoverageType() {
        $('#<%:hdnCtlCoverageTypeID.ClientID %>').val('');
        $('#<%:txtCtlCoverageTypeCode.ClientID %>').val('');
        $('#<%:txtCtlCoverageTypeName.ClientID %>').val('');
    }

    //#region Payer Company
    $('#lblCtlPayerCompany.lblLink').live('click', function () {
        openSearchDialog('payer', getCtlPayerCompanyFilterExpression(), function (value) {
            $('#<%:txtCtlPayerCompanyCode.ClientID %>').val(value);
            onTxtCtlPayerCompanyCodeChanged(value);
        });
    });

    function getCtlPayerCompanyFilterExpression() {
        var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboCtlRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
        return filterExpression;
    }

    function onTxtCtlPayerCompanyCodeChanged(value) {
        var filterExpression = getCtlPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        getCtlPayerCompany(filterExpression);
    }

    function getCtlPayerCompany(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCtlPayerCompanyFilterExpression();

        Methods.getObject('GetvCustomerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCtlPayerCompanyID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%:txtCtlPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                $('#<%:txtCtlPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                $('#<%:hdnCtlGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                var filterExpression = getCtlPayerContractFilterExpression();
                Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                    if (result == 1) {
                        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCtlContractID.ClientID %>').val(result.ContractID);
                                $('#<%:txtCtlContractNo.ClientID %>').val(result.ContractNo);
                                $('#<%:hdnCtlContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                $('#<%:hdnCtlIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                if (result.IsControlClassCare) {
                                    $('#trCtlControlClassCare').show();
                                    if (result.ControlClassID != 0) {
                                        cboCtlControlClassCare.SetValue(result.ControlClassID);
                                    }
                                }
                                else {
                                    $('#trCtlControlClassCare').hide();
                                }
                                onAfterCtlContractNoChanged();
                            }
                        });
                    }
                    else {
                        clearContract();
                    }
                });
            }
            else {
                clearPayerCompany();
            }
        });

    }
    //#endregion

    //#region Payer Contract
    function getCtlPayerContractFilterExpression() {
        var filterExpression = "BusinessPartnerID = " + $('#<%:hdnCtlPayerCompanyID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblCtlContract.lblLink').live('click', function () {
        if ($('#<%:hdnCtlPayerCompanyID.ClientID %>').val() != '') {
            openSearchDialog('contract', getCtlPayerContractFilterExpression(), function (value) {
                $('#<%:txtCtlContractNo.ClientID %>').val(value);
                onTxtCtlPayerContractNoChanged(value);
            });
        }
    });

    $('#<%:txtCtlContractNo.ClientID %>').live('change', function () {
        if ($('#<%:hdnCtlPayerCompanyID.ClientID %>').val() != '')
            onTxtCtlPayerContractNoChanged($(this).val());
        else
            $(this).val('');
    });

    function onTxtCtlPayerContractNoChanged(value) {
        var filterExpression = getCtlPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
        Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCtlContractID.ClientID %>').val(result.ContractID);
                $('#<%:hdnCtlContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                onAfterCtlContractNoChanged();
            }
            else {
                clearContract();
            }
        });
    }

    function onAfterCtlContractNoChanged() {
        var MRN = $('#<%:hdnCtlMRN.ClientID %>').val();
        if (MRN != '') {
            var filterExpression = 'ContractID = ' + $('#<%:hdnCtlContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnCtlMRN.ClientID %>').val();
            Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                $('#<%:hdnCtlContractCoverageMemberCount.ClientID %>').val(result);
                if (result == 1) {
                    filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnCtlContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnCtlMRN.ClientID %>').val() + ') AND IsDeleted = 0';
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnCtlCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                            $('#<%:txtCtlCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%:txtCtlCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            clearCoverageType();
                        }
                    });
                }
                else {
                    var contractCoverageRowCount = parseInt($('#<%:hdnCtlContractCoverageCount.ClientID %>').val());
                    if (contractCoverageRowCount == 1) {
                        var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnCtlContractID.ClientID %>').val() + ") AND IsDeleted = 0";
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%:hdnCtlCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                                $('#<%:txtCtlCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%:txtCtlCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                clearCoverageType();
                            }
                        });
                    }
                    else {
                        clearCoverageType();
                    }
                }
            });
        }
    }
    //#endregion

    function getCtlCoverageTypeFilterExpression() {
        var contractCoverageMemberRowCount = parseInt($('#<%:hdnCtlContractCoverageMemberCount.ClientID %>').val());
        var contractCoverageRowCount = parseInt($('#<%:hdnCtlContractCoverageCount.ClientID %>').val());
        var filterExpression = '';
        if (contractCoverageMemberRowCount > 0)
            filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%:hdnCtlContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%:hdnCtlMRN.ClientID %>').val() + ') AND IsDeleted = 0';
        else if (contractCoverageRowCount > 0)
            filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnCtlContractID.ClientID %>').val() + ") AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblCtlCoverageType.ClientID %>.lblLink').live('click', function () {
        if ($('#<%:hdnCtlContractID.ClientID %>').val() != '') {
            openSearchDialog('coveragetype', getCtlCoverageTypeFilterExpression(), function (value) {
                $('#<%:txtCtlCoverageTypeCode.ClientID %>').val(value);
                onTxtCtlCoverageTypeCodeChanged(value);
            });
        }
    });

    function onTxtCtlCoverageTypeCodeChanged(value) {
        var filterExpression = getCtlCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
        getCtlCoverageType(filterExpression);
    }

    function getCtlCoverageType(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getCtlCoverageTypeFilterExpression();
        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnCtlCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                $('#<%:txtCtlCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                $('#<%:txtCtlCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
            }
            else {
                clearCoverageType();
            }
        });
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                if (param[2] == 'primaryChange') {
                    hideLoadingPanel();
                    pcRightPanelContent.Hide();
                }
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }

    $(function () {
        $('#ulTabPayerDetail li').click(function () {
            $('#ulTabPayerDetail li.selected').removeAttr('class');
            $('.containerTransDt').filter(':visible').hide();

            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });

        $('#<%=txtAmount.ClientID %>').trigger('changeValue');
    });

</script>
<div style="height: 400px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtlEditPayer" value="" />
    <input type="hidden" runat="server" id="hdnCtlMRN" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationPayerID" value="" />
    <input type="hidden" runat="server" id="hdnIsControlCovLimit" value="" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <div class="containerUlTabPage">
                    <ul class="ulTabPage" id="ulTabPayerDetail">
                        <li class="selected" contentid="containerPayerInfo">
                            <%=GetLabel("Informasi Pembayar")%></li>
                        <li contentid="containerSummary">
                            <%=GetLabel("Ringkasan Kontrak")%></li>
                        <li contentid="containerCOB">
                            <%=GetLabel("COB")%></li>
                    </ul>
                </div>
                <div id="containerPayerInfo" class="containerTransDt">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 25%" />
                            <col style="width: 75%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblRegNo">
                                    <%=GetLabel("No Registrasi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblBusinessPartner">
                                    <%=GetLabel("Instansi") %></label>
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
                                            <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trEmployee">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Pegawai") %></label>
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblCorporateAccNo">
                                    <%=GetLabel("No Peserta") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblCorporateAccName">
                                    <%=GetLabel("Nama Peserta") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label1">
                                    <%=GetLabel("Email") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="Label2">
                                    <%=GetLabel("Telepon") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelepon" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                    <%:GetLabel("Tipe Coverage")%></label>
                            </td>
                            <td>
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
                        <tr id="trCoverageLimit" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblAmount">
                                    <%=GetLabel("Batas Tanggungan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmount" CssClass="txtCurrency" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trControlClassCare" runat="server">
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jatah Kelas")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnIsControlClassCare" value="" runat="server" />
                                <dxe:ASPxComboBox ID="cboPayerControlClassCare" ClientInstanceName="cboPayerControlClassCare"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trCoverageLimitPerDay" runat="server">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCoverageLimitPerDay" runat="server" /><label class="lblNormal"
                                    runat="server" id="lblChkIsCovLimit"><%=GetLabel("Batas Tanggungan Per Hari")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblContractPeriod">
                                    <%=GetLabel("Periode Kontrak") %></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <asp:TextBox ID="txtContractPeriodStart" CssClass="datepicker" Width="120px" runat="server"
                                                ReadOnly="true" />
                                        </td>
                                        <td>
                                            s/d
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtContractPeriodEnd" CssClass="datepicker" Width="120px" runat="server"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsGuaranteeLetterExistsCtl" runat="server" /><%:GetLabel("Memiliki Kontrol Surat Jaminan")%>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="containerSummary" class="containerTransDt" style="display: none">
                    <div id="divContractSummary" runat="server">
                    </div>
                </div>
                <div id="containerCOB" class="containerTransDt" style="display: none">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 500px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%:GetLabel("Pembayar")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col style="width: 3px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboCtlRegistrationPayer" ClientInstanceName="cboCtlRegistrationPayer"
                                                        runat="server">
                                                        <ClientSideEvents ValueChanged="function(s){ onCboCtlRegistrationPayerValueChanged(s); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsPrimary" Checked="false" runat="server" /><%:GetLabel("Is Primary")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblCtlPayerCompany">
                                            <%=GetLabel("Instansi")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnCtlPayerCompanyID" value="" runat="server" />
                                        <input type="hidden" id="hdnCtlGCTariffScheme" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCtlPayerCompanyCode" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCtlPayerCompanyName" ReadOnly="true" Width="250px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblMandatory" id="lblCtlContract">
                                            <%:GetLabel("Kontrak")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="0" id="hdnCtlContractID" runat="server" />
                                        <input type="hidden" id="hdnCtlContractCoverageCount" value="" runat="server" />
                                        <input type="hidden" id="hdnCtlContractCoverageMemberCount" value="" runat="server" />
                                        <asp:TextBox ID="txtCtlContractNo" CssClass="required" Width="350px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" class="tdLabel">
                                        <label class="lblLink lblMandatory" runat="server" id="lblCtlCoverageType">
                                            <%:GetLabel("Tipe Coverage")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnCtlCoverageTypeID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCtlCoverageTypeCode" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCtlCoverageTypeName" ReadOnly="true" Width="250px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Corporate Account No.")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorporateAccountNo" CssClass="required number" Width="200px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Corporate Account Name.")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorporateAccountName" CssClass="required" Width="200px" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trCtlControlClassCare">
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%:GetLabel("Jatah Kelas")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnCtlIsControlClassCare" value="" runat="server" />
                                        <dxe:ASPxComboBox ID="cboCtlControlClassCare" ClientInstanceName="cboCtlControlClassCare"
                                            Width="200px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Batas Tanggungan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCtlCoverageLimitAmount" CssClass="txtCurrency" Width="200px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr id="trCtlIsCoverageLimitPerDay" runat="server">
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCtlCoverageLimit" runat="server" /><%:GetLabel("Coverage Limit Per Hari")%>
                                    </td>
                                </tr>
                                <tr id="trGuaranteeLetterExistsCtlAdd" runat="server">
                                    <td class="tdLabel">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsGuaranteeLetterExistsCtlAdd" runat="server" /><%:GetLabel("Memiliki Kontrol Surat Jaminan")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlCOB" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class='imgEdit <%# Eval("IsPrimaryPayer").ToString() == "True" ? "imgDisabled" : "imgLink"%>'
                                                        src='<%# Eval("IsPrimaryPayer").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                    <img class='imgDelete <%# Eval("IsPrimaryPayer").ToString() == "True" ? "imgDisabled" : "imgLink"%>'
                                                        src='<%# Eval("IsPrimaryPayer").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="hdnRegistrationPayerID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="hdnCustomerType" value="<%#: Eval("GCCustomerType")%>" />
                                                    <input type="hidden" class="hdnIsPrimaryPayer" value="<%#: Eval("IsPrimaryPayer")%>" />
                                                    <input type="hidden" class="hdnBusinessPartnerID" value="<%#: Eval("BusinessPartnerID")%>" />
                                                    <input type="hidden" class="hdnBusinessPartnerCode" value="<%#: Eval("BusinessPartnerCode")%>" />
                                                    <input type="hidden" class="hdnBusinessPartnerName" value="<%#: Eval("BusinessPartnerName")%>" />
                                                    <input type="hidden" class="hdnContractID" value="<%#: Eval("ContractID")%>" />
                                                    <input type="hidden" class="hdnContractNo" value="<%#: Eval("ContractNo")%>" />
                                                    <input type="hidden" class="hdnCoverageTypeID" value="<%#: Eval("CoverageTypeID")%>" />
                                                    <input type="hidden" class="hdnCoverageTypeCode" value="<%#: Eval("CoverageTypeCode")%>" />
                                                    <input type="hidden" class="hdnCoverageTypeName" value="<%#: Eval("CoverageTypeName")%>" />
                                                    <input type="hidden" class="hdnCorporateAccountNo" value="<%#: Eval("CorporateAccountNo")%>" />
                                                    <input type="hidden" class="hdnCorporateAccountName" value="<%#: Eval("CorporateAccountName")%>" />
                                                    <input type="hidden" class="hdnCoverageLimitAmount" value="<%#: Eval("CoverageLimitAmount")%>" />
                                                    <input type="hidden" class="hdnIsCoverageLimitPerDay" value="<%#: Eval("IsCoverageLimitPerDay")%>" />
                                                    <input type="hidden" class="hdnIsGuaranteeLetterExists" value="<%#: Eval("IsGuaranteeLetterExists")%>" />
                                                    <input type="hidden" class="hdnControlClassID" value="<%#: Eval("ControlClassID")%>" />
                                                    <input type="hidden" class="hdnIsControlClassCare" value="<%#: Eval("IsControlClassCare")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CheckBoxField DataField="IsPrimaryPayer" ReadOnly="True" HeaderStyle-Width="50px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Is Primary Payer" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%:GetLabel("Customer")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-size: smaller; font-style: oblique">
                                                        <%#: Eval("CustomerType")%></label>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                        <%#: Eval("BusinessPartnerName")%></label>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller; font-style: oblique">
                                                        <%:GetLabel("No. Kontrak = ")%></label><%#: Eval("ContractNo")%>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller; font-style: oblique">
                                                        <%:GetLabel("Skema Penjaminan = ")%></label><b><%#: Eval("CoverageTypeName")%></b>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller; font-style: oblique">
                                                        <%:GetLabel("No. Peserta = ")%></label><%#: Eval("CorporateAccountNo")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfCoverageLimitAmountInString" HeaderText="Coverage Limit Amount"
                                                HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%:GetLabel("Created Information")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                        <%#: Eval("CreatedByName")%></label>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller;">
                                                        <%#: Eval("cfCreatedDateInStringFullFormat")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%:GetLabel("Last Updated Information")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                        <%#: Eval("LastUpdatedByName")%></label>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller;">
                                                        <%#: Eval("cfLastUpdatedDateInStringFullFormat")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                </div>
            </td>
        </tr>
    </table>
</div>
