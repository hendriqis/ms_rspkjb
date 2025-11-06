<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master" AutoEventWireup="true" 
    CodeBehind="PatientBillSummaryGenerateBillAR.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryGenerateBillAR" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">   
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessGenerateBill" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowOPEN" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />  
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {
            onLoadGenerateBill();
        });

        function onLoadGenerateBill() {
            calculateTotal();

            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotal();
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotal();
            });

            function CheckAll() {
                $('.chkSelectAll input').prop('checked', false);
                $('.chkSelectAll input').trigger('click');
            }

            function UnCheckAll() {
                $('.chkSelectAll input').prop('checked', true);
                $('.chkSelectAll input').trigger('click');
            }

            function RecalculateTotal() {
                UnCheckAll();
                CheckAll();
                calculateBillTotal();
            }

            $('#<%=btnProcessGenerateBill.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1)
                    showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                else {
                    if ($('#<%=hdnIsAllowGenerateBill.ClientID %>').val() == '1') {
                        if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                            if ($('#<%=hdnBusinessPartnerID.ClientID %>').val() != '1') {
                                showToastConfirmation('Apakah akan langsung membuat piutang instansi?', function (result) {
                                    if (result) {
                                        onCustomButtonClick('generatebillwithar');
                                    } else {
                                        onCustomButtonClick('generatebill');
                                    }
                                });
                            } else {
                                onCustomButtonClick('generatebill');
                            }
                        }
                        else {
                            showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                        }
                    }
                    else showToast('Warning', 'Masih ada tagihan atau order yang outstanding');
                }
            });

            $('#<%=trFilterCoverage.ClientID %>').live('change', function () {
                var value = $(this).find('input:checked').val();
                if (value == "patient") {
                    $('#<%=txtPatientAmount.ClientID %>').removeAttr('readonly');
                    $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('change');
                    $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('changeValue');

                    $('#<%=txtCoverageLimit.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('change');
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('changeValue');
                } else {
                    $('#<%=txtPatientAmount.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('change');
                    $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('changeValue');

                    $('#<%=txtCoverageLimit.ClientID %>').removeAttr('readonly');
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('change');
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('changeValue');
                }
            });

            $('#<%=txtCoverageLimit.ClientID %>').live('change', function () {
                var amount = parseFloat($(this).val());
                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(amount);
                $('#tdRemainingCoverageAmount').html(amount.formatMoney(2, '.', ','));
                CheckAll();
            });

            $('#<%=txtPatientBPJSAmount.ClientID %>').live('change', function () {
                var amount = parseFloat($(this).val());
                $('#<%=hdnPatientBPJSAmount.ClientID %>').val(amount);
                CheckAll();
            });

            $('#<%=txtPatientAmount.ClientID %>').live('change', function () {
                var amount = parseFloat($(this).val());
                $('#<%=hdnPatientAmount.ClientID %>').val(amount);

                var totalAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                var payerAmount = totalAmount - amount;

                $('#<%=txtCoverageLimit.ClientID %>').val(payerAmount).trigger('change');
                $('#<%=txtCoverageLimit.ClientID %>').val(payerAmount).trigger('changeValue');
                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(payerAmount);
                $('#tdRemainingCoverageAmount').html(payerAmount.formatMoney(2, '.', ','));
            });
        }

        $('.lnkTransactionNo').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var transactionCode = $tr.find('.hdnTransactionCode').val();
            var prescriptionOrderID = parseInt($tr.find('.hdnPrescriptionOrderID').val());
            var prescriptionReturnOrderID = parseInt($tr.find('.hdnPrescriptionReturnOrderID').val());
            var url = '';
            if (prescriptionOrderID > 0) {
                id = prescriptionOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionCtl.ascx");
            }
            else if (prescriptionReturnOrderID > 0) {
                id = prescriptionReturnOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionReturnCtl.ascx");
            }
            else if ($('#<%=hdnDepartmentID.ClientID %>').val() == '<%=GetDepartmentPharmacy() %>' || transactionCode == '<%=laboratoryTransactionCode %>')
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtLaboratoryCtl.ascx");
            else if ($('#<%=hdnDepartmentID.ClientID %>').val() == '<%=GetDepartmentMedicalCheckup() %>')
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtMedicalCheckupCtl.ascx");
            else
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtCtl.ascx");
            
            openUserControlPopup(url, id, 'Pembuatan Tagihan Pasien', 1100, 500);
        });

        $('#btnAdminDetail').live('click', function () {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/BillHistory.ascx");
            var id = registrationID + '|' + linkedRegisID;
            openUserControlPopup(url, id, 'Billing History', 1100, 500);
        });

        function resetTotalAmountPatient() {
            var payerAmount = 0;
            var patientAmount = 0;
            var lineAmount = 0;
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                payerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });
            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);
        }

        function onAfterLockUnlock(param) {
            if (param == 'lock') $('#<%=hdnIsLocked.ClientID %>').val('1');
            else $('#<%=hdnIsLocked.ClientID %>').val('0');
            cbpView.PerformCallback();
        }

        $('.txtAdministrationFee').live('change', function () {
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            if(!$('#isAllowVariableAdmin').is(':checked')){
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');
            calculateBillTotal();
        });

        $('.txtPatientAdministrationFee').live('change', function () {
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            if (!$('#isAllowVariableAdmin').is(':checked')) {
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');
            calculateBillTotal();
        });

        $('.txtServiceFee').live('change', function () {
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;                  
            var max = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            if ((fee < min) && (min > 0))
                fee = min;
            if ((fee > max) && (max > 0))
                fee = max;
            $('#<%=hdnServiceFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');
            calculateBillTotal();
        });

        $('.txtPatientServiceFee').live('change', function () {
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            if ((fee < min) && (min > 0))
                fee = min;
            if ((fee > max) && (max > 0))
                fee = max;
            $('#<%=hdnPatientServiceFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');
            calculateBillTotal();
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            $('.txtCurrency').each(function () {
                $(this).blur();
            });
            onLoadGenerateBill();
            hideLoadingPanel();
        }

        function calculateTotal() {
            var payerAmount = 0;
            var patientAmount = 0;
            var lineAmount = 0;
            var param = '';
            var baseAdminPayerAmount = 0;
            var baseAdminPatientAmount = 0;

            //TOTAL SEMUA DT

            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (param != '')
                    param += '|';
                param += $tr.find('.hdnKeyField').val();
                if ($tr.find('.hdnDepartmentIDChargesDt').val() == 'INPATIENT') {
                    baseAdminPatientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                    baseAdminPayerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                }
                payerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });


            if ($('#<%=hdnIsAdministrationFeeOnlyForInpatient.ClientID %>').val() == '0') {
                baseAdminPatientAmount = patientAmount;
                baseAdminPayerAmount = payerAmount;
            }

            $('#<%=hdnParam.ClientID %>').val(param);
            $('#tdTotalAllPayer').html(payerAmount.formatMoney(2, '.', ','));
            $('#tdTotalAllPatient').html(patientAmount.formatMoney(2, '.', ','));
            $('#tdTotalAll').html(lineAmount.formatMoney(2, '.', ','));

            //#region Administration
            var num1 = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').val());
            var num2 = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').val());
            var payerFee = 0;
            var patientFee = 0;
            if ($('#<%=hdnAdministrationFee.ClientID %>').attr('ispercentage') == '1') {
                payerFee = baseAdminPayerAmount * num1 / 100;
            }
            else {
                payerFee = num1;
            }

            if ($('#<%=hdnPatientAdminFee.ClientID %>').attr('ispercentage') == '1') {
                patientFee = baseAdminPatientAmount * num2 / 100;
            }
            else {
                patientFee = num2;
            }

            var payerMin = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            var payerMax = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var patientMin = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('minamount'));
            var patientMax = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('maxamount'));

            if ((baseAdminPayerAmount > 0) && (payerMin > 0) && (payerFee < payerMin))
                payerFee = payerMin;
            if ((baseAdminPayerAmount > 0) && (payerMax > 0) && payerFee > payerMax)
                payerFee = payerMax;
            if ((baseAdminPatientAmount > 0) && (patientMin > 0) && (patientFee < patientMin))
                patientFee = patientMin;
            if ((baseAdminPatientAmount > 0) && (patientMax > 0) && patientFee > patientMax)
                patientFee = payerMax;

            $('.txtAdministrationFee').val(payerFee).trigger('changeValue');
            $('.txtPatientAdministrationFee').val(patientFee).trigger('changeValue');
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(payerFee);
            $('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val(patientFee);
            //#endregion

            //#region Service
            var num1 = parseFloat($('#<%=hdnServiceFee.ClientID %>').val());
            var num2 = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').val());
            var servicePayerFee = 0;
            var servicePatientFee = 0;
            if ($('#<%=hdnServiceFee.ClientID %>').attr('ispercentage') == '1') {
                servicePayerFee = baseAdminPayerAmount * num1 / 100;
            }
            else {
                servicePayerFee = num1;
            }

            if ($('#<%=hdnPatientServiceFee.ClientID %>').attr('ispercentage') == '1') {
                servicePatientFee = baseAdminPatientAmount * num2 / 100;
            }
            else {
                servicePatientFee = num2;
            }

            var payerMin = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            var payerMax = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var patientMin = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('minamount'));
            var patientMax = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('maxamount'));

            if ((baseAdminPayerAmount > 0) && (payerMin > 0) && (servicePayerFee < payerMin))
                servicePayerFee = payerMin;
            if ((baseAdminPayerAmount > 0) && (payerMax > 0) && (servicePayerFee > payerMax))
                servicePayerFee = payerMax;

            if ((baseAdminPatientAmount > 0) && (patientMin > 0) && (servicePatientFee < patientMin))
                servicePatientFee = patientMin;
            if ((baseAdminPatientAmount > 0) && (patientMax > 0) && (servicePatientFee > patientMax))
                servicePatientFee = patientMax;

            $('.txtServiceFee').val(servicePayerFee).trigger('changeValue');
            $('.txtPatientServiceFee').val(servicePatientFee).trigger('changeValue');

            $('#<%=hdnServiceFeeAmount.ClientID %>').val(servicePayerFee);
            $('#<%=hdnPatientServiceFeeAmount.ClientID %>').val(servicePatientFee);
            //#endregion

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);
            calculateBillTotal();
        }

        function calculateBillTotal() {
            var patientAmount = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
            var payerAmount = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());
            var lineAmount = parseFloat($('#<%=hdnTotalAmount.ClientID %>').val());
            var diffPayerAmount = 0;
            var administrationFee = parseFloat($('.txtAdministrationFee').attr('hiddenVal'));
            var serviceFee = parseFloat($('.txtServiceFee').attr('hiddenVal'));
            var patientAdministrationFee = parseFloat($('.txtPatientAdministrationFee').attr('hiddenVal'));
            var patientServiceFee = parseFloat($('.txtPatientServiceFee').attr('hiddenVal'));

            if (isNaN(administrationFee))
                administrationFee = 0;
            if (isNaN(patientAdministrationFee))
                patientAdministrationFee = 0;
            if (isNaN(serviceFee))
                serviceFee = 0;
            if (isNaN(patientServiceFee))
                patientServiceFee = 0;

            payerAmount += administrationFee + serviceFee;
            patientAmount += patientAdministrationFee + patientServiceFee;
            //lineAmount += administrationFee + serviceFee + patientAdministrationFee + patientServiceFee;
            if ($('#<%=hdnIsControlCoverageLimit.ClientID %>').val() == '1') {
                var remainingCoverageAmount = parseFloat($('#<%=hdnRemainingCoverageAmount.ClientID %>').val());
                var patientBPJSAmount = parseFloat($('#<%=hdnPatientBPJSAmount.ClientID%>').val());
                var isBPJSClass = $('#<%=hdnIsBPJSClass.ClientID %>').val();
                var isApplyBPJSVariance = $('#<%=hdnIsApplyBPJSClassCareVarianceToPatient.ClientID %>').val();
                var isControlBPJSCoverage = $('#<%=hdnIsControlBPJSCoverage.ClientID %>').val();

                if (isControlBPJSCoverage == '1') {
                    //if (patientBPJSAmount > 0) {
                        diffPayerAmount = Math.ceil((patientBPJSAmount + remainingCoverageAmount) - (patientAmount + payerAmount));
                        patientAmount = patientBPJSAmount;
                    //}
                }
                else {
                    patientAmount = patientAmount + payerAmount - remainingCoverageAmount;
                }
                payerAmount = remainingCoverageAmount;
            }
            
            lineAmount = patientAmount + payerAmount;
            
            $('#tdBillTotalAllPayer').html(payerAmount.formatMoney(2, '.', ','));
            $('#tdBillTotalAllPatient').html(patientAmount.formatMoney(2, '.', ','));
            $('#tdBillTotalAll').html(lineAmount.formatMoney(2, '.', ','));
            if (diffPayerAmount < 0) {
                $('#tdDiffCoverageAmount').css("color", "red");
            }
            $('#tdDiffCoverageAmount').html(diffPayerAmount.formatMoney(2, '.', ','));
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(administrationFee);
            $('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val(patientAdministrationFee);
            $('#<%=hdnServiceFeeAmount.ClientID %>').val(serviceFee);
            $('#<%=hdnPatientServiceFeeAmount.ClientID %>').val(patientServiceFee);
            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnDiffCoverageAmount.ClientID %>').val(diffPayerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);

        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var healthcareServiceUnitID = cboServiceUnit.GetValue();

            filterExpression.text = 'RegistrationID = ' + registrationID;

            if (code == 'PM-00224' || code == 'PM-00231') {
                filterExpression.text = registrationID + ';' + healthcareServiceUnitID;
            }

            var allowPrint = $('#<%=hdnAllowPrint.ClientID %>').val();
            if (code == 'PM-00206') {
                if (allowPrint.indexOf("PTR") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Transfer";
                    return false;
                } else {
                    if (linkedRegisID != "" && linkedRegisID != "0") {
                        filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
                    } 
                }
            }

            if (code == 'PM-00203' || code == 'PM-00204' || code == 'PM-00205' || code == 'PM-00206' || code == 'PM-00207' || code == 'PM-00208' || code == 'PM-00209' || code == 'PM-00210' || code == 'PM-00211' || code == 'PM-00212' || code == 'PM-00213' || code == 'PM-00242' || code == 'PM-00243' || code == 'PM-00323' || code == 'PM-00410' || code == 'PM-00412' || code == 'PM-00414' || code == 'PM-00416') {
                filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
            }

            if (code == 'PM-00139' || code == 'PM-00248' || code == 'PM-00415' || code == 'PM-00417') {
                filterExpression.text = registrationID;
            }

            if (code == 'PM-00205' || code == 'PM-00248' || code == 'PM-00415' || code == 'PM-00417') {
                if (allowPrint.indexOf("DRUG") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Obat dan Alkes";
                    return false;
                }
            }
            if (code == 'PM-00207') {
                if (allowPrint.indexOf("LAB") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Laboratorium";
                    return false;
                }
            }
            if (code == 'PM-00208') {
                if (allowPrint.indexOf("IMA") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Radiologi";
                    return false;
                }
            }
            if (code == 'PM-00209') {
                if (allowPrint.indexOf("MDO") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Penunjang Lainnya";
                    return false;
                } else {
                    filterExpression.text += " AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = 'DIAGNOSTIC')";
                }
            }
            return true;
        }

        function onCboDisplayChanged() {
            cbpView.PerformCallback();
        }

        function onCboServiceUnitChanged() {
            cbpView.PerformCallback();
        }
    </script> 
    <style type="text/css">
        .trFooter td        { height: 10px }
    </style>
    <input type="hidden" value="" id="hdnListShift" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnIsControlCoverageLimit" runat="server" />
    <input type="hidden" value="" id="hdnIsControlBPJSCoverage" runat="server" />
    <input type="hidden" value="0" id="hdnOldRemainingCoverageAmount" runat="server" />
    <input type="hidden" value="0" id="hdnRemainingCoverageAmount" runat="server" />
    <input type="hidden" value="0" id="hdnDiffCoverageAmount" runat="server" />
    <input type="hidden" value="0" id="hdnPatientBPJSAmount" runat="server" />
    <input type="hidden" value="0" id="hdnPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />  
    <input type="hidden" value="" id="hdnIsCustomerPersonal" runat="server" />  
    <input type="hidden" value="0" id="hdnAdministrationFee" runat="server" />  
    <input type="hidden" value="0" id="hdnPatientAdminFee" runat="server" />  
    <input type="hidden" value="0" id="hdnServiceFee" runat="server" />
    <input type="hidden" value="0" id="hdnPatientServiceFee" runat="server" />  
    <input type="hidden" value="0" id="hdnAdministrationFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnServiceFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnPatientAdministrationFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnPatientServiceFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnIsApplyBPJSClassCareVarianceToPatient" runat="server" />
    <input type="hidden" value="0" id="hdnIsBPJSClass" runat="server" />
    <input type="hidden" value="0" id="hdnAllowPrint" runat="server" />
    <input type="hidden" value="0" id="hdnIsAdministrationFeeUseHigherClass" runat="server" />
    <input type="hidden" value="0" id="hdnIsLocked" runat="server" />
    <input type="hidden" value="0" id="hdnIsAllowGenerateBill" runat="server" />
    <input type="hidden" value="0" id="hdnIsAdministrationFeeOnlyForInpatient" runat="server" />    
    <div>
         <table class="tblEntryContent">
            <colgroup>
                <col style="width:200px"/>
                <col style="width:250px"/>
                <col />
                <col style="width:450px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Verifikasi")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" Width="100%" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDisplayChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td>&nbsp</td>
                <td rowspan="2" style="vertical-align:top">
                    <table id="tblInfoOutstandingTransfer" runat="server">
                        <tr>
                            <td style="vertical-align:top"><img height="48" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' /></td>
                            <td style="vertical-align:top">
                                <label class="lblWarning" id="lblInfoOutstandingBill" runat="server"><%=GetLabel("Masih Ada Tagihan Yang Belum Ditransfer") %></label>
                                <label class="lblWarning" id="lblInfoOutstandingOrder" runat="server"><%=GetLabel("Masih Ada Order Yang Belum Diproses") %></label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit Pelayanan")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trFilterCoverage" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Filter Coverage") %></label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblFilterCoverage" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Payer" Value="payer" Selected="True" />
                        <asp:ListItem Text="Patient" Value="patient"  />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trCoverageLimit" runat="server">
                <td class="tdLabel"><label class="lblNormal"><%:GetCoverageLabel()%></label></td>
                <td>
                    <asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="120px" runat="server" />
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtINACBGSCode" Width="120px" placeholder="Kode" runat="server" />
                            </td>       
                            <td>
                                <asp:TextBox ID="txtINACBGSName" Width="300px" placeholder="Nama" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPatientBPJSAmount" runat="server">
                
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Di Tanggung Pasien (BPJS)")%></label></td>
                <td>
                    <asp:TextBox ID="txtPatientBPJSAmount" CssClass="txtCurrency" Width="120px" runat="server" />
                </td>
            </tr>
            <tr id="trPatientAmount" runat="server">
                
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Di Tanggung Pasien")%></label></td>
                <td>
                    <asp:TextBox ID="txtPatientAmount" CssClass="txtCurrency" Width="120px" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div style="height:435px;overflow-y:auto;">
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td>
                    <div style="padding: 5px;min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th style="width:40px" rowspan="2">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding:3px;float:left; width: 180px;">
                                                                <div><%= GetLabel("No. Transaksi")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                 
                                                            </div>
                                                            <div style="padding:3px;float:left; width: 100px;">
                                                                <div><%= GetLabel("Unit Pelayanan")%></div>
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                            <div style="padding:3px;margin-left: 20px;float:left; width: 120px">
                                                                <div><%= GetLabel("Kelas")%></div>
                                                            </div>
                                                            <div style="padding:3px;margin-left: 400px;">
                                                                <div><%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                                        <th rowspan="2">
                                                            <div><%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width:50px">
                                                            <div style="text-align:center;">
                                                                <%=GetLabel("Verified")%>
                                                            </div>
                                                        </th>          
                                                    </tr>
                                                    <tr>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>                                
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th style="width:40px" rowspan="2">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding:3px;float:left; width: 180px;">
                                                                <div><%= GetLabel("No. Transaksi")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                 
                                                            </div>
                                                            <div style="padding:3px;float:left; width: 100px;">
                                                                <div><%= GetLabel("Unit Pelayanan")%></div>
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                            <div style="padding:3px;margin-left: 20px;float:left; width: 120px">
                                                                <div><%= GetLabel("Kelas")%></div>
                                                            </div>
                                                            <div style="padding:3px;margin-left: 400px;">
                                                                <div><%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                                        <th rowspan="2">
                                                            <div><%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width:50px">
                                                            <div style="text-align:center;">
                                                                <%=GetLabel("Verified")%>
                                                            </div>
                                                        </th>          
                                                    </tr>
                                                    <tr>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:100px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                                    <tr class="trFooter">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px" id="tdTotalAllPayer">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px" id="tdTotalAllPatient">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px" id="tdTotalAll">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterAdministrationFee" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <%=GetLabel("Biaya Administrasi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtAdministrationFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtAdministrationFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtPatientAdministrationFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtPatientAdministrationFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:0px 3px; background-color:Aqua; height:20px; width:20px; float:left;">
                                                                <img id="btnAdminDetail" src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" height="20px" width="20px" style="cursor:pointer"/>
                                                            </div>
                                                            <div>
                                                                <input id="isAllowVariableAdmin" type="checkbox" />Variable
                                                            </div>
                                                        </td>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterServiceFee" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <%=GetLabel("Biaya Service")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtServiceFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtServiceFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtPatientServiceFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtPatientServiceFee" />
                                                            </div>
                                                        </td>
                                                        <td colspan="3">
<%--                                                            <div style="text-align:left;padding:3px" id="Div4">
                                                                <input type="button" id="btnApply" value='<%=GetLabel("Apply") %>' />
                                                            </div>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterRemainingCoverageLimit" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px; font-weight:bold">
                                                                <%=GetLabel("Batas Tanggungan")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px" id="tdRemainingCoverageAmount">
                                                                <%=GetRemainingCoverageAmount()%>
                                                            </div>
                                                        </td>
                                                        <td colspan="4">
                                                        </td> 
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterRemainingTotalBill" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px">
                                                                <%=GetLabel("Total Tagihan")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px" id="tdBillTotalAllPayer">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px" id="tdBillTotalAllPatient">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px" id="tdBillTotalAll">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterDiffCoverageLimit" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px; font-weight:bold">
                                                                <%=GetLabel("Selisih Tagihan RS dan BPJS")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:3px" id="tdDiffCoverageAmount">
                                                                <%=GetDiffCoverageAmount()%>
                                                            </div>
                                                        </td>
                                                        <td colspan="4">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div style="padding:3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server"/>
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                            <input type="hidden" class="hdnDepartmentIDChargesDt" value="<%#: Eval("VisitDepartmentID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;float:right;margin-right:50px;<%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td><img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' /></td>
                                                                    <td><label class="lblInfo"><%=GetLabel("Pending Recalculated") %></label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding:3px;float:left; width: 180px;">
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <a class="lnkTransactionNo"><%#: Eval("TransactionNo")%></a>
                                                            <div>
                                                                <%#: Eval("TransactionDateInString")%> <%#: Eval("TransactionTime")%>
                                                                <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' title="Has Compound" alt="" style='<%# Eval("IsHasCompound").ToString() == "0" ? "display:none;": "" %> max-width:20px;min-width: 20px;' />
                                                            </div>
                                                        </div>
                                                        <div style="padding:3px;float:left; width: 100px;">
                                                            <div><%#: Eval("ServiceUnitName")%></div>
                                                            <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                                        </div>
                                                        <div style="padding:3px;margin-left: 20px;float:left; width: 120px">
                                                            <div><%#: Eval("ChargeClass")%></div>
                                                            <div>&nbsp;</div>
                                                        </div>
                                                        <div style="padding:3px;margin-left: 400px;">
                                                            <div><%#: Eval("Remarks")%></div>
                                                            <div>&nbsp;</div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div><%#: Eval("TotalPayerAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div><%#: Eval("TotalPatientAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div><%#: Eval("TotalAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;"><%#: Eval("TransactionStatus")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:center;">
                                                            <asp:CheckBox ID="chkIsVerified" runat="server" Checked='<%# Eval("IsVerified")%>' Enabled="false" />
                                                        </div>
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