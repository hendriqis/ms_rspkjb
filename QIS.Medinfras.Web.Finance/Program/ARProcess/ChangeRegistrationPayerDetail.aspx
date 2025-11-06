<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangeRegistrationPayerDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ChangeRegistrationPayerDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangeRegistrationPayerBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnChangeRegistrationPayer" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=GetLabel("Perubahan Pembayar Registrasi")%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=trPayerCompany.ClientID %>').hide();
            $('#<%=trCoverageType.ClientID %>').hide();
            $('#<%=trParticipant.ClientID %>').hide();
            $('#<%=trPayerContract.ClientID %>').hide();
            $('#<%=trControlClassCare.ClientID %>').hide();
        }

        $('#<%=btnChangeRegistrationPayerBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/ARProcess/ChangeRegistrationPayerList.aspx');
        });

        $('#<%=btnChangeRegistrationPayer.ClientID %>').live('click', function (evt) {
            var BusinessPartnerID = $('#<%=hdnPayerID.ClientID %>').val();
            if (BusinessPartnerID != "0" && BusinessPartnerID != "") {
                onCustomButtonClick('changepayer');
            } else {
                showToast('Warning', 'Pilih penjamin bayar terlebih dahulu');
            }
        });

        function onCboPayerValueChanged(s) {
            setTblPayerCompanyVisibility();
            $('#<%=hdnPayerID.ClientID %>').val('');
            $('#<%=txtPayerCompanyCode.ClientID %>').val('');
            $('#<%=txtPayerCompanyName.ClientID %>').val('');

            $('#<%=hdnPayerID.ClientID %>').val('');
            $('#<%=txtPayerCompanyCode.ClientID %>').val('');
            $('#<%=txtPayerCompanyName.ClientID %>').val('');
            $('#<%=hdnContractID.ClientID %>').val('');
            $('#<%=txtContractNo.ClientID %>').val('');
            $('#<%=hdnContractCoverageCount.ClientID %>').val('');

            $('#<%=hdnCoverageTypeID.ClientID %>').val('');
            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
            $('#<%=txtCoverageTypeName.ClientID %>').val('');
            $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
            $('#<%=trControlClassCare.ClientID %>').hide();
        }

        //#region Payer Company
        function getPayerCompanyFilterExpression() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboPayer.GetValue() + "' AND IsActive = 1 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPayerCompany').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
                $('#<%=txtPayerCompanyCode.ClientID %>').val(value);
                onTxtPayerCompanyCodeChanged(value);
            });
        });

        $('#<%=txtPayerCompanyCode.ClientID %>').live('change', function () {
            onTxtPayerCompanyCodeChanged($(this).val());
        });

        function onTxtPayerCompanyCodeChanged(value) {
            var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%=hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);

                    Methods.getValue('GetCustomerContractRowCount', getPayerContractFilterExpression(), function (result) {
                        if (result == 1) {
                            var filterExpression = getPayerContractFilterExpression();
                            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnContractID.ClientID %>').val(result.ContractID);
                                    $('#<%=txtContractNo.ClientID %>').val(result.ContractNo);
                                    onTxtPayerContractNoChanged(result.ContractNo);
                                    $('#<%=hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                    $('#<%=hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                                    if (result.IsControlClassCare) {
                                        $('#<%=trControlClassCare.ClientID %>').show();
                                        if (result.ControlClassID != null && result.ControlClassID != 0) {
                                            cboControlClassCare.SetValue(result.ControlClassID);
                                        } else {
                                            cboControlClassCare.SetValue('');
                                        }
                                    }
                                    else {
                                        $('#<%=trControlClassCare.ClientID %>').hide();
                                        cboControlClassCare.SetValue('');
                                    }

                                    onAfterContractNoChanged();
                                }
                            });
                        }
                        else {
                            $('#<%=hdnContractID.ClientID %>').val('');
                            $('#<%=txtContractNo.ClientID %>').val('');
                            $('#<%=hdnContractCoverageCount.ClientID %>').val('');

                            $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%=txtCoverageTypeName.ClientID %>').val('');
                            $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                            $('#<%=trControlClassCare.ClientID %>').hide();
                            cboControlClassCare.SetValue('');
                        }
                    });
                }
                else {
                    $('#<%=hdnPayerID.ClientID %>').val('');
                    $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                    $('#<%=hdnGCTariffScheme.ClientID %>').val('');

                    $('#<%=hdnContractID.ClientID %>').val('');
                    $('#<%=txtContractNo.ClientID %>').val('');
                    $('#<%=hdnContractCoverageCount.ClientID %>').val('');

                    $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                    $('#<%=trControlClassCare.ClientID %>').hide();
                }
            });
        }
        //#endregion

        //#region Payer Contract
        function getPayerContractFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND BusinessPartnerID = " + $('#<%=hdnPayerID.ClientID %>').val() + "<%=GetCustomerContractFilterExpression() %>";
            return filterExpression;
        }

        $('#<%=lblContract.ClientID %>.lblLink').live('click', function () {
            if ($('#<%=hdnPayerID.ClientID %>').val() != '') {
                openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                    $('#<%=txtContractNo.ClientID %>').val(value);
                    onTxtPayerContractNoChanged(value);
                });
            }
        });

        $('#<%=txtContractNo.ClientID %>').live('change', function () {
            if ($('#<%=hdnPayerID.ClientID %>').val() != '')
                onTxtPayerContractNoChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtPayerContractNoChanged(value) {
            var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnContractID.ClientID %>').val(result.ContractID);
                    $('#<%=hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);

                    $('#<%=hdnIsControlClassCare.ClientID %>').val(result.IsControlClassCare ? '1' : '0');
                    if (result.IsControlClassCare) {
                        $('#<%=trControlClassCare.ClientID %>').show();
                        if (result.ControlClassID != null && result.ControlClassID != 0) {
                            cboControlClassCare.SetValue(result.ControlClassID);
                        } else {
                            cboControlClassCare.SetValue('');
                        }
                    }
                    else {
                        $('#<%=trControlClassCare.ClientID %>').hide();
                        cboControlClassCare.SetValue('');
                    }

                    onAfterContractNoChanged();
                }
                else {
                    $('#<%=hdnContractID.ClientID %>').val('');
                    $('#<%=txtContractNo.ClientID %>').val('');
                    $('#<%=hdnContractCoverageCount.ClientID %>').val('');
                    $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    $('#<%=hdnIsControlClassCare.ClientID %>').val('0');
                    $('#<%=trControlClassCare.ClientID %>').hide();
                    cboControlClassCare.SetValue('');
                }
            });
        }

        function onAfterContractNoChanged() {
            var filterExpression = 'ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val();
            Methods.getValue('GetContractCoverageMemberRowCount', filterExpression, function (result) {
                $('#<%=hdnContractCoverageMemberCount.ClientID %>').val(result);
                if (result == 1) {
                    filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
                    Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                            $('#<%=txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                            $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                        }
                        else {
                            $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                            $('#<%=txtCoverageTypeName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var contractCoverageRowCount = parseInt($('#<%=hdnContractCoverageCount.ClientID %>').val());
                    if (contractCoverageRowCount == 1) {
                        var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
                        Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                            if (result != null) {
                                $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                                $('#<%=txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                                $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                            }
                            else {
                                $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                                $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                                $('#<%=txtCoverageTypeName.ClientID %>').val('');
                            }
                        });
                    }
                    else {
                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                }
            });
        }
        //#endregion

        //#region Coverage Type
        function getCoverageTypeFilterExpression() {
            var contractCoverageMemberRowCount = parseInt($('#<%=hdnContractCoverageMemberCount.ClientID %>').val());
            var contractCoverageRowCount = parseInt($('#<%=hdnContractCoverageCount.ClientID %>').val());
            var filterExpression = '';
            if (contractCoverageMemberRowCount > 0)
                filterExpression = 'CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverageMember WHERE ContractID = ' + $('#<%=hdnContractID.ClientID %>').val() + ' AND MRN = ' + $('#<%=hdnMRN.ClientID %>').val() + ') AND IsDeleted = 0';
            else if (contractCoverageRowCount > 0)
                filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%=hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=lblCoverageType.ClientID %>.lblLink').live('click', function () {
            if ($('#<%=hdnContractID.ClientID %>').val() != '') {
                openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                    $('#<%=txtCoverageTypeCode.ClientID %>').val(value);
                    onTxtCoverageTypeCodeChanged(value);
                });
            }
        });

        $('#<%=txtCoverageTypeCode.ClientID %>').live('change', function () {
            if ($('#<%=hdnContractID.ClientID %>').val() != '')
                onTxtCoverageTypeCodeChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtCoverageTypeCodeChanged(value) {
            var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Employee
        function getEmployeeFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }
        $('#<%=lblEmployee.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
                $('#<%=txtEmployeeCode.ClientID %>').val(value);
                onTxtEmployeeCodeChanged(value);
            });
        });

        $('#<%=txtEmployeeCode.ClientID %>').live('change', function () {
            onTxtEmployeeCodeChanged($(this).val());
        });

        function onTxtEmployeeCodeChanged(value) {
            var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
            Methods.getObject('GetEmployeeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                    $('#<%=txtEmployeeName.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnEmployeeID.ClientID %>').val('');
                    $('#<%=txtEmployeeCode.ClientID %>').val('');
                    $('#<%=txtEmployeeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function setTblPayerCompanyVisibility() {
            if (cboPayer.GetValue() == 'X004^999') {
                $('#<%=trPayerCompany.ClientID %>').hide();
                $('#<%=trPayerContract.ClientID %>').hide();
                $('#<%=trCoverageType.ClientID %>').hide();
                $('#<%=trParticipant.ClientID %>').hide();
            }
            else {
                $('#<%=trPayerCompany.ClientID %>').show();
                $('#<%=trPayerContract.ClientID %>').show();
                $('#<%=trCoverageType.ClientID %>').show();
                $('#<%=trParticipant.ClientID %>').show();
            }
            if (cboPayer.GetValue() == 'X004^400') {
                $('#<%=trEmployee.ClientID %>').show();
            }
            else {
                $('#<%=trEmployee.ClientID %>').hide();
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "changepayer") {
                showToast('Process Success', 'Proses perubahan penjamin ke <b>' + retval + '</b> berhasil!');
            }
        }
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimAfterARInvoice" runat="server" />
    <input type="hidden" value="" id="hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim" runat="server" />
    <input type="hidden" value="" id="hdnIsGrouperAmountClaimDefaultZero" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <h4>
                                    <%=GetLabel("PENJAMIN AWAL :")%></h4>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayerOri" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trPayerCompanyOri" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblPayerCompanyOri">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPayerIDOri" runat="server" />
                                <input type="hidden" id="hdnGCCustomerTypeOri" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyCodeOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyNameOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPayerContractOri" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblContractOri">
                                    <%=GetLabel("Kontrak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContractNoOri" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trCoverageTypeOri" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblCoverageTypeOri">
                                    <%=GetLabel("Tipe Tanggungan")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeCodeOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeNameOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trEmployeeOri" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblEmployeeOri">
                                    <%=GetLabel("Pegawai")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeCodeOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeNameOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trParticipantOri" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Peserta/Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParticipantNoOri" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trControlClassCareOri" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jatah Kelas")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="140px" />
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtControlClassCareOri" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td class="tdLabel">
                                            <label class="lblNormal" style="padding-left: 10px">
                                                <%=GetLabel("Kelas Tagihan RS")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChargeClassOri" Width="140px" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <h4>
                                    <%=GetLabel("PENJAMIN AKHIR :")%></h4>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPayer" ClientInstanceName="cboPayer" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trPayerCompany" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPayerCompany">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                                <input type="hidden" id="hdnPayerID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPayerContract" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                    <%=GetLabel("Kontrak")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnContractID" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                                <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                                <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trCoverageType" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                    <%=GetLabel("Tipe Tanggungan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
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
                                            <asp:TextBox ID="txtCoverageTypeName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trEmployee" runat="server">
                            <td style="width: 150px" class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblEmployee">
                                    <%=GetLabel("Pegawai")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnEmployeeID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
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
                                            <asp:TextBox ID="txtEmployeeName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trParticipant" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Peserta/Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParticipantNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trControlClassCare" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jatah Kelas")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="140px" />
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnIsControlClassCare" value="" runat="server" />
                                            <dxe:ASPxComboBox ID="cboControlClassCare" ClientInstanceName="cboControlClassCare"
                                                Width="100%" runat="server" />
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
</asp:Content>
