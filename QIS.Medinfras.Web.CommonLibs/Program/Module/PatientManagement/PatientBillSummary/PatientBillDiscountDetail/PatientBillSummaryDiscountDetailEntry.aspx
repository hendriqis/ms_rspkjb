<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryDiscountDetailEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryDiscountDetailEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });
        }

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedTransactionDtID.ClientID %>').val() != "" && $('#<%=hdnSelectedTransactionDtID.ClientID %>').val() != ",") {
                onCustomButtonClick('editdiscount');
            }
            else {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
        });

        //#region Filter

        //#region Charges Department
        function onCboDepartmentChanged() {
            $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%=txtChargesServiceUnitCode.ClientID %>').val('');
            $('#<%=txtChargesServiceUnitName.ClientID %>').val('');
            $('#<%=hdnCboChargesDepartmentID.ClientID %>').val(cboDepartment.GetValue());
        }
        //#endregion

        //#region Charges Service Unit
        function getChargesHealthcareServiceUnitFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "'"
                                    + " AND DepartmentID = '" + cboDepartment.GetValue() + "'"
                                    + " AND " + $('#<%=hdnFilterChargesHealthcareServiceUnitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblChargesHealthcareServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getChargesHealthcareServiceUnitFilterExpression(), function (value) {
                $('#<%=txtChargesServiceUnitCode.ClientID %>').val(value);
                ontxtChargesServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtChargesServiceUnitCode.ClientID %>').live('change', function () {
            ontxtChargesServiceUnitCodeChanged($(this).val());
        });

        function ontxtChargesServiceUnitCodeChanged(value) {
            var filterExpression = getChargesHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtChargesServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtChargesServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtChargesServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtChargesServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Charges Paramedic
        function getChargesParamedicMasterFilterExpression() {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var filterExpression = "IsDeleted = 0";

            filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM vPatientChargesDt10 WHERE ((RegistrationID = " + registrationID + " OR (LinkedToRegistrationID = " + registrationID + " AND IsChargesTransfered = 1))" + " AND GCTransactionStatus IN ('X121^001','X121^002') AND GCTransactionDetailStatus IN ('X121^001','X121^002') AND IsDeleted = 0))"

            if ($('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() != "" && $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() != "0") {
                filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() + "')";
            }

            return filterExpression;
        }

        $('#lblChargesParamedic.lblLink').live('click', function () {
            openSearchDialog('physician', getChargesParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtChargesParamedicCode.ClientID %>').val(value);
                ontxtChargesParamedicCodeChanged(value);
            });
        });

        $('#<%=txtChargesParamedicCode.ClientID %>').live('change', function () {
            ontxtChargesParamedicCodeChanged($(this).val());
        });

        function ontxtChargesParamedicCodeChanged(value) {
            var filterExpression = getChargesParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnChargesParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtChargesParamedicCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%=txtChargesParamedicName.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnChargesParamedicID.ClientID %>').val('');
                    $('#<%=txtChargesParamedicCode.ClientID %>').val('');
                    $('#<%=txtChargesParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Item Type
        function onCboGCItemTypeChanged() {
            $('#<%=hdnCboGCItemType.ClientID %>').val(cboGCItemType.GetValue());
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        //#endregion

        //#region Edit TXT Discount Comp Per Row Detail

        $('.txtLVDiscComp1').live('change', function () {
            $tr = $(this).closest('tr');
            var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));

            var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
            var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
            var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
            var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

            $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
            calculateLineAmount();
        });

        $('.txtLVDiscComp2').live('change', function () {
            $tr = $(this).closest('tr');
            var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));

            var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
            var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
            var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
            var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

            $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
            calculateLineAmount();
        });

        $('.txtLVDiscComp3').live('change', function () {
            $tr = $(this).closest('tr');
            var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));

            var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
            var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
            var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
            var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

            $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
            calculateLineAmount();
        });

        //#endregion

        //#region Edit TXT Discount Comp

        $('#<%=txtDiscComp1.ClientID %>').live('change', function () {
            var discComp1 = parseFloat($('#<%=txtDiscComp1.ClientID %>').val());

            $('.chkIsSelected input').each(function () {
                $tr = $(this).closest('tr');
                if ($(this).is(':checked')) {
                    var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));
                    var tariffComp1 = parseFloat($tr.find('.TariffComp1').val().replace(".00", "").split(',').join(''));
                    var discComp1Final = 0;

                    if (tariffComp1 != 0) {
                        if ($('#<%=chkIsDiscountInPercentage.ClientID %>').is(':checked')) {
                            discComp1Final = parseFloat(tariffComp1 * discComp1 / 100);
                            discComp1Final = discComp1Final;
                        }
                        else {
                            discComp1Final = discComp1;
                        }

                        $tr.find('.txtLVDiscComp1').val(discComp1Final).trigger('changeValue');
                    }

                    var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
                    var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
                    var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
                    var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

                    $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
                }
            });

            calculateLineAmount();
        });

        $('#<%=txtDiscComp2.ClientID %>').live('change', function () {
            var discComp2 = parseFloat($('#<%=txtDiscComp2.ClientID %>').val());

            $('.chkIsSelected input').each(function () {
                $tr = $(this).closest('tr');
                if ($(this).is(':checked')) {
                    var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));
                    var tariffComp2 = parseFloat($tr.find('.TariffComp2').val().replace(".00", "").split(',').join(''));
                    var discComp2Final = 0;

                    if (tariffComp2 != 0) {
                        if ($('#<%=chkIsDiscountInPercentage.ClientID %>').is(':checked')) {
                            discComp2Final = parseFloat(tariffComp2 * discComp2 / 100);
                            discComp2Final = discComp2Final;
                        }
                        else {
                            discComp2Final = discComp2;
                        }

                        $tr.find('.txtLVDiscComp2').val(discComp2Final).trigger('changeValue');
                    }

                    var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
                    var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
                    var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
                    var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

                    $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
                }
            });

            calculateLineAmount();
        });

        $('#<%=txtDiscComp3.ClientID %>').live('change', function () {
            var discComp3 = parseFloat($('#<%=txtDiscComp3.ClientID %>').val());

            $('.chkIsSelected input').each(function () {
                $tr = $(this).closest('tr');
                if ($(this).is(':checked')) {
                    var chargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));
                    var tariffComp3 = parseFloat($tr.find('.TariffComp3').val().replace(".00", "").split(',').join(''));
                    var discComp3Final = 0;

                    if (tariffComp3 != 0) {
                        if ($('#<%=chkIsDiscountInPercentage.ClientID %>').is(':checked')) {
                            discComp3Final = parseFloat(tariffComp3 * discComp3 / 100);
                            discComp3Final = discComp3Final;
                        }
                        else {
                            discComp3Final = discComp3;
                        }

                        $tr.find('.txtLVDiscComp3').val(discComp3Final).trigger('changeValue');
                    }

                    var idiscComp1 = parseFloat($tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join(''));
                    var idiscComp2 = parseFloat($tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join(''));
                    var idiscComp3 = parseFloat($tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join(''));
                    var discAmount = parseFloat((idiscComp1 + idiscComp2 + idiscComp3) * chargedQuantity);

                    $tr.find('.txtDiscountAmount').val(discAmount).trigger('changeValue');
                }
            });

            calculateLineAmount();
        });

        function calculateLineAmount() {
            $('.chkIsSelected input').each(function () {
                $tr = $(this).closest('tr');
                if ($(this).is(':checked')) {
                    var oTariff = parseFloat($tr.find('.Tariff').val().replace(".00", "").split(',').join(''));
                    var oChargedQuantity = parseFloat($tr.find('.ChargedQuantity').val().replace(".00", "").split(',').join(''));

                    var discAmount = parseFloat($tr.find('.txtDiscountAmount').val().replace(".00", "").split(',').join(''));

                    var DiscountAmount_ORI = parseFloat($tr.find('.DiscountAmount').val().replace(".00", "").split(',').join(''));

                    var oPayerAmount = parseFloat($tr.find('.PayerAmount').val().replace(".00", "").split(',').join(''));
                    var oPatientAmount = parseFloat($tr.find('.PatientAmount').val().replace(".00", "").split(',').join(''));
                    var oLineAmount = parseFloat($tr.find('.LineAmount').val().replace(".00", "").split(',').join(''));

                    var isPersonalCustomer = "1";
                    if ($('#<%=hdnBusinessPartnerID.ClientID %>').val() != 1) {
                        isPersonalCustomer = "0";
                    }

                    var newPayerAmount = 0;
                    var newPatientAmount = 0;
                    if (oPayerAmount == 0 && oPatientAmount == 0) {
                        if (isPersonalCustomer == "0") {
                            newPayerAmount = oPayerAmount - (discAmount - DiscountAmount_ORI);
                        } else {
                            newPatientAmount = oPatientAmount - (discAmount - DiscountAmount_ORI);
                        }
                    }
                    else if (oPayerAmount != 0 || oPatientAmount != 0) {
                        if (oPayerAmount != 0) {
                            newPayerAmount = oPayerAmount - ((discAmount - DiscountAmount_ORI) * oPayerAmount / oLineAmount);
                        }

                        if (oPatientAmount != 0) {
                            newPatientAmount = oPatientAmount - ((discAmount - DiscountAmount_ORI) * oPatientAmount / oLineAmount);
                        }
                    }
                    else {
                        if (oPayerAmount != 0) {
                            newPayerAmount = (oTariff * oChargedQuantity) - discAmount;
                        }

                        if (oPatientAmount != 0) {
                            newPatientAmount = (oTariff * oChargedQuantity) - discAmount;
                        }
                    }

                    var newLineAmount = newPayerAmount + newPatientAmount;

                    $tr.find('.txtPayerAmount').val(newPayerAmount).trigger('changeValue');
                    $tr.find('.txtPatientAmount').val(newPatientAmount).trigger('changeValue');
                    $tr.find('.txtLineAmount').val(newLineAmount).trigger('changeValue');
                }
            });
        }

        //#endregion

        //#region DiscountReason
        function oncboGCDiscountReasonChargesDtValueChanged() {
            var discountReason = cboGCDiscountReasonChargesDt.GetValue();
            ontxtDiscountHeaderChangedValue(discountReason);
            if (discountReason == "X550^999") {
                $('#<%=txtDiscountReasonChargesDtHeader.ClientID %>').removeAttr('readonly');
            } else {
                $('#<%=txtDiscountReasonChargesDtHeader.ClientID %>').val("");
                $('#<%=txtDiscountReasonChargesDtHeader.ClientID %>').attr('readonly', 'readonly');
            }
            $('#<%=txtDiscComp1.ClientID %>').trigger('change');
            $('#<%=txtDiscComp2.ClientID %>').trigger('change');
            $('#<%=txtDiscComp3.ClientID %>').trigger('change');
        }

        $('#<%=txtDiscountReasonChargesDtHeader.ClientID %>').live('change', function () {
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    $tr.find('.txtDiscountReasonChargesDt').val($('#<%=txtDiscountReasonChargesDtHeader.ClientID %>').val());
                }
            });
        });

        function ontxtDiscountHeaderChangedValue(value) {
            var filterExpression = "IsDeleted = 0 AND IsActive = 1 AND ParentID = 'X550' AND StandardCodeID = '" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null) {
                    $('.chkIsSelected input').each(function () {
                        if ($(this).is(':checked')) {
                            $tr = $(this).closest('tr');
                            $tr.find('.hdnGCDiscountReasonChargesDt').val(result.StandardCodeID);
                            $tr.find('.lblGCDiscountReasonChargesDt').html(result.StandardCodeName);

                            if (result.StandardCodeID == "X550^999") {
                                $tr.find('.txtDiscountReasonChargesDt').removeAttr('readonly');
                            }
                            else {
                                $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                            }
                        }
                    });
                }
                else {
                    $('.chkIsSelected input').each(function () {
                        if ($(this).is(':checked')) {
                            $tr = $(this).closest('tr');
                            $tr.find('.hdnGCDiscountReasonChargesDt').val('');
                            $tr.find('.lblGCDiscountReasonChargesDt').html('Pilih Alasan Diskon');
                            $tr.find('.txtDiscountReasonChargesDt').val('');
                            $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                        }
                    });
                }
            });
        }

        $('.lblGCDiscountReasonChargesDt.lblLink').die('click');
        $('.lblGCDiscountReasonChargesDt.lblLink').live('click', function () {
            $td = $(this).parent();
            $tr = $td.closest('tr');
            openSearchDialog('standardcode', "IsDeleted = 0 AND IsActive = 1 AND ParentID = 'X550'", function (value) {
                var filterExpression = "StandardCodeID = '" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null) {
                        $tr.find('.hdnGCDiscountReasonChargesDt').val(result.StandardCodeID);
                        if (result.StandardCodeName != "")
                            $tr.find('.lblGCDiscountReasonChargesDt').html(result.StandardCodeName);
                        else
                            $tr.find('.lblGCDiscountReasonChargesDt').html('Pilih Alasan Diskon');

                        if (result.StandardCodeID == "X550^999") {
                            $tr.find('.txtDiscountReasonChargesDt').removeAttr('readonly');
                        } else {
                            $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                        }
                    }
                    else {
                        $td.find('.lblGCDiscountReasonChargesDt').html('Pilih Alasan Diskon');
                        $td.find('.hdnGCDiscountReasonChargesDt').val('');
                        $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                    }
                });
            });
        });
        //#endregion

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            var oIsAllowDiscountTariffComp1 = $tr.find('.IsAllowDiscountTariffComp1').val();
            var oIsAllowDiscountTariffComp2 = $tr.find('.IsAllowDiscountTariffComp2').val();
            var oIsAllowDiscountTariffComp3 = $tr.find('.IsAllowDiscountTariffComp3').val();
            var IsPackageItem = $tr.find('.IsPackageItem').val();

            if ($(this).is(':checked')) {
                var countAllowDiscount = 0;
                if (IsPackageItem != 'True') {
                    if (oIsAllowDiscountTariffComp1 == "True") {
                        $tr.find('.txtLVDiscComp1').removeAttr('readonly');
                        countAllowDiscount = countAllowDiscount + 1;
                    } else {
                        $tr.find('.txtLVDiscComp1').attr('readonly', 'readonly');
                    }

                    if (oIsAllowDiscountTariffComp2 == "True") {
                        $tr.find('.txtLVDiscComp2').removeAttr('readonly');
                        countAllowDiscount = countAllowDiscount + 1;
                    } else {
                        $tr.find('.txtLVDiscComp2').attr('readonly', 'readonly');
                    }

                    if (oIsAllowDiscountTariffComp3 == "True") {
                        $tr.find('.txtLVDiscComp3').removeAttr('readonly');
                        countAllowDiscount = countAllowDiscount + 1;
                    } else {
                        $tr.find('.txtLVDiscComp3').attr('readonly', 'readonly');
                    }
                }
                else {
                    if (oIsAllowDiscountTariffComp1 == "True") {
                        countAllowDiscount = countAllowDiscount + 1;
                    }

                    if (oIsAllowDiscountTariffComp2 == "True") {
                        countAllowDiscount = countAllowDiscount + 1;
                    }

                    if (oIsAllowDiscountTariffComp3 == "True") {
                        countAllowDiscount = countAllowDiscount + 1;
                    }
                }

                if (countAllowDiscount > 0) {
                    $tr.find('.lblGCDiscountReasonChargesDt').addClass('lblLink lblGCDiscountReasonChargesDt');
                    var discReason = $tr.find('.hdnGCDiscountReasonChargesDt').val();
                    if (discReason == "X550^999") {
                        $tr.find('.txtDiscountReasonChargesDt').removeAttr('readonly');
                    } else {
                        $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                    }
                } else {
                    $tr.find('.lblGCDiscountReasonChargesDt').removeClass('lblLink');
                    $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
                }
            }
            else {
                $tr.find('.txtLVDiscComp1').attr('readonly', 'readonly');
                $tr.find('.txtLVDiscComp2').attr('readonly', 'readonly');
                $tr.find('.txtLVDiscComp3').attr('readonly', 'readonly');
                $tr.find('.lblGCDiscountReasonChargesDt').removeClass('lblLink');
                $tr.find('.txtDiscountReasonChargesDt').attr('readonly', 'readonly');
            }
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val().split(',');
            var lstSelectedMemberDiscAmount = $('#<%=hdnSelectedDiscountAmount.ClientID %>').val().split(',');
            var lstSelectedMemberDiscComp1 = $('#<%=hdnSelectedDiscountComp1.ClientID %>').val().split(',');
            var lstSelectedMemberDiscComp2 = $('#<%=hdnSelectedDiscountComp2.ClientID %>').val().split(',');
            var lstSelectedMemberDiscComp3 = $('#<%=hdnSelectedDiscountComp3.ClientID %>').val().split(',');
            var lstSelectedMemberGCDiscountReason = $('#<%=hdnSelectedGCDiscountReason.ClientID %>').val().split(',');
            var lstSelectedMemberDiscountReason = $('#<%=hdnSelectedDiscountReason.ClientID %>').val().split(',');
            var lstSelectedMemberPayerAmount = $('#<%=hdnSelectedPayerAmount.ClientID %>').val().split(',');
            var lstSelectedMemberPatientAmount = $('#<%=hdnSelectedPatientAmount.ClientID %>').val().split(',');
            var lstSelectedMemberLineAmount = $('#<%=hdnSelectedLineAmount.ClientID %>').val().split(',');

            var result = '';

            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).parent().closest('tr');
                    var key = $tr.find('.keyField').val();
                    var txtDiscountAmount = $tr.find('.txtDiscountAmount').val().replace(".00", "").split(',').join('');
                    var txtLVDiscComp1 = $tr.find('.txtLVDiscComp1').val().replace(".00", "").split(',').join('');
                    var txtLVDiscComp2 = $tr.find('.txtLVDiscComp2').val().replace(".00", "").split(',').join('');
                    var txtLVDiscComp3 = $tr.find('.txtLVDiscComp3').val().replace(".00", "").split(',').join('');
                    var gcDiscountReasonChargesDt = $tr.find('.hdnGCDiscountReasonChargesDt').val();
                    var txtDiscountReasonChargesDt = $tr.find('.txtDiscountReasonChargesDt').val();
                    var txtPayerAmount = $tr.find('.txtPayerAmount').val().replace(".00", "").split(',').join('');
                    var txtPatientAmount = $tr.find('.txtPatientAmount').val().replace(".00", "").split(',').join('');
                    var txtLineAmount = $tr.find('.txtLineAmount').val().replace(".00", "").split(',').join('');
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberDiscAmount.push(txtDiscountAmount);
                        lstSelectedMemberDiscComp1.push(txtLVDiscComp1);
                        lstSelectedMemberDiscComp2.push(txtLVDiscComp2);
                        lstSelectedMemberDiscComp3.push(txtLVDiscComp3);
                        lstSelectedMemberGCDiscountReason.push(gcDiscountReasonChargesDt);
                        lstSelectedMemberDiscountReason.push(txtDiscountReasonChargesDt);
                        lstSelectedMemberPayerAmount.push(txtPayerAmount);
                        lstSelectedMemberPatientAmount.push(txtPatientAmount);
                        lstSelectedMemberLineAmount.push(txtLineAmount);
                    }
                    else {
                        lstSelectedMemberDiscAmount[idx] = txtDiscountAmount;
                        lstSelectedMemberDiscComp1[idx] = txtLVDiscComp1;
                        lstSelectedMemberDiscComp2[idx] = txtLVDiscComp2;
                        lstSelectedMemberDiscComp3[idx] = txtLVDiscComp3;
                        lstSelectedMemberGCDiscountReason[idx] = gcDiscountReasonChargesDt;
                        lstSelectedMemberDiscountReason[idx] = txtDiscountReasonChargesDt;
                        lstSelectedMemberPayerAmount[idx] = txtPayerAmount;
                        lstSelectedMemberPatientAmount[idx] = txtPatientAmount;
                        lstSelectedMemberLineAmount[idx] = txtLineAmount;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberDiscAmount.splice(idx, 1);
                        lstSelectedMemberDiscComp1.splice(idx, 1);
                        lstSelectedMemberDiscComp2.splice(idx, 1);
                        lstSelectedMemberDiscComp3.splice(idx, 1);
                        lstSelectedMemberGCDiscountReason.splice(idx, 1);
                        lstSelectedMemberDiscountReason.splice(idx, 1);
                        lstSelectedMemberPayerAmount.splice(idx, 1);
                        lstSelectedMemberPatientAmount.splice(idx, 1);
                        lstSelectedMemberLineAmount.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedDiscountAmount.ClientID %>').val(lstSelectedMemberDiscAmount.join(','));
            $('#<%=hdnSelectedDiscountComp1.ClientID %>').val(lstSelectedMemberDiscComp1.join(','));
            $('#<%=hdnSelectedDiscountComp2.ClientID %>').val(lstSelectedMemberDiscComp2.join(','));
            $('#<%=hdnSelectedDiscountComp3.ClientID %>').val(lstSelectedMemberDiscComp3.join(','));
            $('#<%=hdnSelectedGCDiscountReason.ClientID %>').val(lstSelectedMemberGCDiscountReason.join(','));
            $('#<%=hdnSelectedDiscountReason.ClientID %>').val(lstSelectedMemberDiscountReason.join(','));
            $('#<%=hdnSelectedPayerAmount.ClientID %>').val(lstSelectedMemberPayerAmount.join(','));
            $('#<%=hdnSelectedPatientAmount.ClientID %>').val(lstSelectedMemberPatientAmount.join(','));
            $('#<%=hdnSelectedLineAmount.ClientID %>').val(lstSelectedMemberLineAmount.join(','));
        }

        $('.imgIsEditPackageItem.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var id = $row.find('.keyField').val();
            var itemID = $row.find('.ItemID').val();
            var param = id + "|" + itemID;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscountDetail/PatientBillSummaryDiscPackageInfoCtl.ascx");
            openUserControlPopup(url, param, 'Item Paket', 1000, 500);
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp1" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp2" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp3" runat="server" />
    <input type="hidden" value="" id="hdnSelectedGCDiscountReason" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountReason" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedLineAmount" runat="server" />
    <input type="hidden" value="" id="hdnCboChargesDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnFilterChargesHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowDiscountInDetailWhenAlreadyHasPrescriptionReturn"
        runat="server" />
    <input type="hidden" value="0" id="hdnIsEndingAmountRoundingTo100" runat="server" />
    <input type="hidden" value="0" id="hdnIsEndingAmountRoundingTo1" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%; vertical-align: top">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <table class="tblContentArea" style="width: 100%; vertical-align: top">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 100px" />
                            <col style="width: 350px" />
                            <col />
                        </colgroup>
                        <tr id="trDepartment" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Department")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnChargesHealthcareServiceUnitID" value="" runat="server" />
                                <label class="lblLink lblNormal" id="lblChargesHealthcareServiceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesServiceUnitCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnChargesParamedicID" value="" runat="server" />
                                <label class="lblLink lblNormal" id="lblChargesParamedic">
                                    <%=GetLabel("Dokter/Paramedis")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesParamedicCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesParamedicName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Item")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboGCItemType" ClientInstanceName="cboGCItemType" runat="server"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboGCItemTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Item JasMed")%></label>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsHasRevenueSharing" CssClass="chkIsHasRevenueSharing" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                    <table class="tblContentArea" style="width: 100%; vertical-align: top">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                                <label class="lblNormal" style="font-weight: bold">
                                    <%=GetLabel("Sarana")%></label>
                            </td>
                            <td align="right">
                                <label class="lblNormal" style="font-weight: bold">
                                    <%=GetLabel("Pelayanan")%></label>
                            </td>
                            <td align="right">
                                <label class="lblNormal" style="font-weight: bold">
                                    <%=GetLabel("Lainnya")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Diskon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscComp1" Width="100%" runat="server" CssClass="txtCurrency"
                                    min="0" Text="0" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscComp2" Width="100%" runat="server" CssClass="txtCurrency"
                                    min="0" Text="0" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscComp3" Width="100%" runat="server" CssClass="txtCurrency"
                                    min="0" Text="0" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" align="left" valign="middle">
                                <asp:CheckBox ID="chkIsDiscountInPercentage" CssClass="chkIsDiscountInPercentage"
                                    runat="server" />
                                <label class="lblNormal">
                                    <%=GetLabel("Diskon dalam Persen (%)")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" align="left" valign="middle">
                                <asp:CheckBox ID="chkIsProcessDetailPackageOnly" CssClass="chkIsProcessDetailPackageOnly"
                                    runat="server" />
                                <label class="lblNormal" style="color: Maroon">
                                    <%=GetLabel("Proses diskon juga untuk <b>Transaksi Detail Paket</b> ?")%></label>
                                <br />
                                <label class="blink-alert" style="color: Maroon">
                                    <%=GetLabel("(nilai diskon per komponen di detail paket akan mengikuti isian nilai diskon per komponen di atas)")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Discount Reason")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboGCDiscountReasonChargesDt" ClientInstanceName="cboGCDiscountReasonChargesDt"
                                        Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ oncboGCDiscountReasonChargesDtValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDiscountReasonChargesDtHeader" runat="server" ReadOnly="true"
                                        Width="100%" CssClass="txtDiscountReasonChargesDtHeader" />
                                </td>
                            </tr>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 20px" rowspan="2">
                                                            &nbsp;
                                                        </th>
                                                        <th style="width: 120px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" rowspan="2" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Jumlah")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("HARGA")%>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("TOTAL")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga @satuan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("CITO")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="25">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 20px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <input id="chkSelectAll" type="checkbox" />
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" rowspan="2" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Jumlah")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("HARGA")%>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("TOTAL")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga @satuan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("CITO")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 130px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr style="<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "background-color:#FFE4E1" : ""%>">
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="keyField" value="<%#: Eval("ID")%>" />
                                                            <input type="hidden" class="TransactionID" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                            <input type="hidden" class="Tariff" value="<%#: Eval("Tariff")%>" />
                                                            <input type="hidden" class="TariffComp1" value="<%#: Eval("TariffComp1")%>" />
                                                            <input type="hidden" class="TariffComp2" value="<%#: Eval("TariffComp2")%>" />
                                                            <input type="hidden" class="TariffComp3" value="<%#: Eval("TariffComp3")%>" />
                                                            <input type="hidden" class="DiscountAmount" value="<%#: Eval("DiscountAmount")%>" />
                                                            <input type="hidden" class="DiscountComp1" value="<%#: Eval("DiscountComp1")%>" />
                                                            <input type="hidden" class="DiscountComp2" value="<%#: Eval("DiscountComp2")%>" />
                                                            <input type="hidden" class="DiscountComp3" value="<%#: Eval("DiscountComp3")%>" />
                                                            <input type="hidden" class="GCDiscountReason" value="<%#: Eval("GCDiscountReason")%>" />
                                                            <input type="hidden" class="DiscountReasonName" value="<%#: Eval("DiscountReasonName")%>" />
                                                            <input type="hidden" class="DiscountReason" value="<%#: Eval("DiscountReason")%>" />
                                                            <input type="hidden" class="PayerAmount" value="<%#: Eval("PayerAmount")%>" />
                                                            <input type="hidden" class="PatientAmount" value="<%#: Eval("PatientAmount")%>" />
                                                            <input type="hidden" class="LineAmount" value="<%#: Eval("LineAmount")%>" />
                                                            <input type="hidden" class="ChargedQuantity" value="<%#: Eval("ChargedQuantity")%>" />
                                                            <input type="hidden" class="IsAllowDiscount" value="<%#: Eval("IsAllowDiscount")%>" />
                                                            <input type="hidden" class="IsAllowDiscountTariffComp1" value="<%#: Eval("IsAllowDiscountTariffComp1")%>" />
                                                            <input type="hidden" class="IsAllowDiscountTariffComp2" value="<%#: Eval("IsAllowDiscountTariffComp2")%>" />
                                                            <input type="hidden" class="IsAllowDiscountTariffComp3" value="<%#: Eval("IsAllowDiscountTariffComp3")%>" />
                                                            <input type="hidden" class="IsPackageItem" value="<%#: Eval("IsPackageItem")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("TransactionNo")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: oblique">
                                                            <%#: Eval("TransactionDateInString")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: oblique">
                                                            <%#: Eval("ChargesServiceUnitName")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller;">
                                                            <%=GetLabel("Status : ")%><%#: Eval("TransactionDetailStatus")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("ItemName1")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                            <%#: Eval("ItemCode")%></div>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div>
                                                            <img class="imgIsEditPackageItem imgLink" <%# Eval("IsPackageItem").ToString() == "True" ?  "" : "style='display:none'" %>
                                                                title='<%=GetLabel("Info Paket")%>' src='<%# Eval("IsPackageItem").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/package.png") : ResolveUrl("~/Libs/Images/Button/package_disabled.png")%>'
                                                                alt="" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("ChargedQuantityInString")%>
                                                            <%#: Eval("ItemUnit")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <b>
                                                                <%#: Eval("Tariff", "{0:N2}")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i><%#: Eval("TariffComp1", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i><%#: Eval("TariffComp2", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i><%#: Eval("TariffComp3", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("CITOAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("Alasan Diskon : ")%></i>
                                                            <br />
                                                            <input type="hidden" value="" class="hdnGCDiscountReasonChargesDt" id="hdnGCDiscountReasonChargesDt"
                                                                runat="server" />
                                                            <b>
                                                                <label class="lblLink lblGCDiscountReasonChargesDt" id="lblGCDiscountReasonChargesDt"
                                                                    runat="server">
                                                                    <%#: Eval("DiscountReasonName") != "" ? Eval("DiscountReasonName") : "Pilih Alasan Diskon" %></label></b>
                                                            <br />
                                                            <asp:TextBox ID="txtDiscountReasonChargesDt" runat="server" CssClass="txtDiscountReasonChargesDt"
                                                                Width="90%" ReadOnly="true" />
                                                        </div>
                                                        <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="txtCurrency txtDiscountAmount"
                                                            Width="90%" ReadOnly="true" />
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i>
                                                            <asp:TextBox ID="txtLVDiscComp1" runat="server" CssClass="txtCurrency txtLVDiscComp1"
                                                                Width="90px" ReadOnly="true" /></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i>
                                                            <asp:TextBox ID="txtLVDiscComp2" runat="server" CssClass="txtCurrency txtLVDiscComp2"
                                                                Width="90px" ReadOnly="true" /></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i>
                                                            <asp:TextBox ID="txtLVDiscComp3" runat="server" CssClass="txtCurrency txtLVDiscComp3"
                                                                Width="90px" ReadOnly="true" /></div>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPayerAmount" runat="server" CssClass="txtCurrency txtPayerAmount"
                                                            Width="90%" ReadOnly="true" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPatientAmount" runat="server" CssClass="txtCurrency txtPatientAmount"
                                                            Width="90%" ReadOnly="true" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLineAmount" runat="server" CssClass="txtCurrency txtLineAmount"
                                                            Width="90%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
