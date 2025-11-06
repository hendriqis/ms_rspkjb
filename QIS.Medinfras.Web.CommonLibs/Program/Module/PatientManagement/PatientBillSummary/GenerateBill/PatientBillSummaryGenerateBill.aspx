<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryGenerateBill.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryGenerateBill" %>

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
    <li id="btnProcessGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnCalculateCoverageLimit" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Calculate Coverage")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="0" id="hdnIsFromRegistrationCoverage" runat="server" />
    <input type="hidden" value="0" id="hdnCoverageAmountFromRegistrationCoverage" runat="server" />
    <input type="hidden" value="0" id="hdnIsCoverageAllowBiggerThanBillingAmount" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowOPEN" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowOPENForValidation" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function dateToDMYCustom(date) {
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
        }

        $(function () {
            setCustomButtonEnabled();
            onLoadGenerateBill();

            var filterSetvar = "HealthcareID = '001' AND ParameterCode = 'FN0182'";
            Methods.getObject("GetSettingParameterDtList", filterSetvar, function (resultSetvar) {
                if (resultSetvar != null) {
                    $('#<%:hdnFN0182.ClientID %>').val(resultSetvar.ParameterValue);
                }
            });

            $('#trDate').css('display', 'none');
            setDatePicker('<%=txtFilterTransactionDateFrom.ClientID %>');
            setDatePicker('<%=txtFilterTransactionDateTo.ClientID %>');

            var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
            if (isAllowVariableAdmin) {
                $('.txtAdministrationFee').removeAttr('readonly');
                $('.txtPatientAdministrationFee').removeAttr('readonly');
                $('.txtServiceFee').removeAttr('readonly');
                $('.txtPatientServiceFee').removeAttr('readonly');
            } else {
                $('.txtAdministrationFee').attr('readonly', 'readonly');
                $('.txtPatientAdministrationFee ').attr('readonly', 'readonly');
                $('.txtServiceFee').attr('readonly', 'readonly');
                $('.txtPatientServiceFee').attr('readonly', 'readonly');
            }
        });

        $('#<%=rblFilterDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == 'true') {
                $('#trDate').css('display', '');
            }
            else {
                $('#trDate').css('display', 'none');
            }

            $('.chkSelectAll input').prop('checked', true);
            $('.chkSelectAll input').trigger('click');

            cbpView.PerformCallback();
        });

        $('#<%=txtFilterTransactionDateFrom.ClientID %>').live('change', function (evt) {
            var transDateFrom = $('#<%:txtFilterTransactionDateFrom.ClientID %>').val();
            var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
            var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

            var transDateTo = $('#<%:txtFilterTransactionDateTo.ClientID %>').val();
            var transactionDateTo = Methods.getDatePickerDate(transDateTo);
            var transDateYMDTo = dateToDMYCustom(transactionDateTo);

            if (transDateYMDFrom > transDateYMDTo) {
                showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
            }

            cbpView.PerformCallback();
        });

        $('#<%=txtFilterTransactionDateTo.ClientID %>').live('change', function (evt) {
            var transDateFrom = $('#<%:txtFilterTransactionDateFrom.ClientID %>').val();
            var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
            var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

            var transDateTo = $('#<%:txtFilterTransactionDateTo.ClientID %>').val();
            var transactionDateTo = Methods.getDatePickerDate(transDateTo);
            var transDateYMDTo = dateToDMYCustom(transactionDateTo);

            if (transDateYMDFrom > transDateYMDTo) {
                showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
            }

            cbpView.PerformCallback();
        });

        function onCboDisplayVerificationChanged() {
            $('.chkSelectAll input').prop('checked', true);
            $('.chkSelectAll input').trigger('click');
            cbpView.PerformCallback();
        }

        function onCboServiceUnitChanged() {
            $('.chkSelectAll input').prop('checked', true);
            $('.chkSelectAll input').trigger('click');
            cbpView.PerformCallback();
        }

        function onAfterPopupControlClosing() {
            var coverageLimit = $('#<%=txtCoverageLimit.ClientID %>').val();
            coverageLimit = parseFloat(coverageLimit.replace('.00', '').split(',').join(''));
            if (coverageLimit == 0) {
                var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var filterExpression = "RegistrationID = " + regID + " AND IsDeleted = 0";
                var CoverageAmount = parseFloat(0);
                Methods.getListObject('GetRegistrationCoverageList', filterExpression, function (result) {
                    if (result != null) {
                        for (i = 0; i < result.length; i++) {
                            CoverageAmount += parseFloat(result[i].PayerAmount);
                        }
                    }
                });
                $('#<%=txtCoverageLimit.ClientID %>').val(CoverageAmount).trigger('change');
                $('#<%=txtCoverageLimit.ClientID %>').val(CoverageAmount).trigger('changeValue');
                
                $('#<%=hdnCoverageAmountFromRegistrationCoverage.ClientID %>').val(CoverageAmount);
            }
        }

        $('#<%=btnCalculateCoverageLimit.ClientID %>').live('click', function () {
            showLoadingPanel();
            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/CalculateCoverageLimitCtl.ascx');
            openUserControlPopup(url, regID, 'Hitung Limit Tanggungan ', 1200, 450);
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

            function CheckAllTest() {
                calculateTotalTest();
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
                if ($('#<%=hdnIsAllowProcessBillWhenPendingRecalculated.ClientID %>').val() == '1') {
                    if ($('#<%=hdnIsHasTotalChargesDifferent.ClientID %>').val() == '1') {
                        showToast('Warning', 'Masih ada transaksi yg masih belum balance, harap proses hitung ulang dari transaksi yg belum balance tersebut.');
                    } else {
                        if ($('.chkIsSelected input:checked').length < 1)
                            showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                        else {
                            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                                if ($('#<%=txtCoverageLimit.ClientID %>').val() <= 0 && $('#<%:chkIsEditCoverageBPJSManual.ClientID %>').is(':checked')) {
                                    showToast('Warning', 'Tidak dapat di proses karena nilai INACBG\'s Grouper kurang atau sama dengan 0');
                                }
                                else {
                                    if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                        if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                            var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                            Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                if (resultFilter != null) {
                                                    var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                    showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                        if (resultMessage) {
                                                            onCustomButtonClick('generatebill');
                                                        }
                                                    });
                                                }
                                                else {
                                                    onCustomButtonClick('generatebill');
                                                }
                                            });
                                        }
                                        else {
                                            onCustomButtonClick('generatebill');
                                        }
                                    }
                                    else {
                                        showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                    }
                                }
                            }
                            else {
                                if ($('#<%=hdnBlokPembuatanTagihanSaatAdaTransaksiOpen.ClientID %>').val() == '1') {
                                    if ($('#<%=hdnIsHasTransactionAndOrderOutstanding.ClientID %>').val() == '1') {
                                        showToast('Warning', 'Masih ada transaksi / order yang statusnya masih OUTSTANDING');
                                    } else {
                                        if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                            if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                                var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                                Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                    if (resultFilter != null) {
                                                        var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                        showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                            if (resultMessage) {
                                                                onCustomButtonClick('generatebill');
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        onCustomButtonClick('generatebill');
                                                    }
                                                });
                                            }
                                            else {
                                                onCustomButtonClick('generatebill');
                                            }
                                        }
                                        else {
                                            showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                        }
                                    }
                                } else {
                                    if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                        if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                            var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                            Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                if (resultFilter != null) {
                                                    var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                    showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                        if (resultMessage) {
                                                            onCustomButtonClick('generatebill');
                                                        }
                                                    });
                                                }
                                                else {
                                                    onCustomButtonClick('generatebill');
                                                }
                                            });
                                        }
                                        else {
                                            onCustomButtonClick('generatebill');
                                        }
                                    }
                                    else {
                                        showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    if ($('#<%=hdnIsPendingRecalculated.ClientID %>').val() == '1') {
                        showToast('Warning', 'Masih ada transaksi yang belum di rekalkulasi, harap proses rekalkulasi dari transaksi tersebut.');
                    }
                    else {
                        if ($('#<%=hdnIsHasTotalChargesDifferent.ClientID %>').val() == '1') {
                            showToast('Warning', 'Masih ada transaksi yg masih belum balance, harap proses hitung ulang dari transaksi yg belum balance tersebut.');
                        } else {
                            if ($('.chkIsSelected input:checked').length < 1)
                                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                            else {
                                if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                                    if ($('#<%=txtCoverageLimit.ClientID %>').val() <= 0 && $('#<%:chkIsEditCoverageBPJSManual.ClientID %>').is(':checked')) {
                                        showToast('Warning', 'Tidak dapat di proses karena nilai INACBG\'s Grouper kurang atau sama dengan 0');
                                    }
                                    else {
                                        if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                            if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                                var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                                Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                    if (resultFilter != null) {
                                                        var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                        showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                            if (resultMessage) {
                                                                onCustomButtonClick('generatebill');
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        onCustomButtonClick('generatebill');
                                                    }
                                                });
                                            }
                                            else {
                                                onCustomButtonClick('generatebill');
                                            }
                                        }
                                        else {
                                            showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                        }
                                    }
                                }
                                else {
                                    if ($('#<%=hdnBlokPembuatanTagihanSaatAdaTransaksiOpen.ClientID %>').val() == '1') {
                                        if ($('#<%=hdnIsHasTransactionAndOrderOutstanding.ClientID %>').val() == '1') {
                                            showToast('Warning', 'Masih ada transaksi / order yang statusnya masih OUTSTANDING');
                                        } else {
                                            if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                                if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                                    var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                                    Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                        if (resultFilter != null) {
                                                            var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                            showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                                if (resultMessage) {
                                                                    onCustomButtonClick('generatebill');
                                                                }
                                                            });
                                                        }
                                                        else {
                                                            onCustomButtonClick('generatebill');
                                                        }
                                                    });
                                                }
                                                else {
                                                    onCustomButtonClick('generatebill');
                                                }
                                            }
                                            else {
                                                showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                            }
                                        }
                                    } else {
                                        if ($('#<%=hdnIsLocked.ClientID %>').val() == '1') {
                                            if ($('#<%=hdnNotificationRegistrationIsLinkedToRegistrationInpatient.ClientID %>').val() == '1') {
                                                var filterExpression = "RegistrationID = '" + $('#<%=hdnRegistrationID.ClientID %>').val() + "' AND LinkedToRegistrationID IN (SELECT RegistrationID FROM vConsultVisit WHERE DepartmentID = 'INPATIENT')";
                                                Methods.getObject("GetvRegistrationList", filterExpression, function (resultFilter) {
                                                    if (resultFilter != null) {
                                                        var messageHasLinkedToInpatientRegistration = 'Registrasi ini sudah mempunyai link ke registrasi Rawat Inap ' + '<b>' + resultFilter.LinkedToRegistrationNo + '</b>' + '. Apakah ingin melanjutkan proses?';
                                                        showToastConfirmation(messageHasLinkedToInpatientRegistration, function (resultMessage) {
                                                            if (resultMessage) {
                                                                onCustomButtonClick('generatebill');
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        onCustomButtonClick('generatebill');
                                                    }
                                                });
                                            }
                                            else {
                                                onCustomButtonClick('generatebill');
                                            }
                                        }
                                        else {
                                            showToast('Warning', 'Transaksi harus di lock terlebih dahulu baru bisa di generate');
                                        }
                                    }
                                }
                            }
                        }
                    }
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

                    $('#<%=chkIsEditCoverageBPJSManual.ClientID %>').prop('checked', false);
                    $('#<%=chkIsEditCoverageBPJSManual.ClientID %>').attr('readonly', 'readonly');
                }

                $('#<%=hdnTxtCoverageLimitIsChanged.ClientID %>').val('0');
                UnCheckAll();
            });

            $('#<%:chkIsEditCoverageBPJSManual.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=txtCoverageLimit.ClientID%>').removeAttr('readonly');
                } else {
                    $('#<%=txtCoverageLimit.ClientID%>').attr('readonly', 'readonly');
                }
            });

            $('#<%=txtCoverageLimit.ClientID %>').live('change', function () {
                $('#<%=hdnTxtCoverageLimitIsChanged.ClientID %>').val("1");

                var coverageLimit = $('#<%=txtCoverageLimit.ClientID %>').val();
                coverageLimit = parseFloat(coverageLimit.replace('.00', '').split(',').join(''));

                var hdnFN0182 = $('#<%=hdnFN0182.ClientID %>').val();

                var isHasCheckedTrans = 0;
                $('.chkIsSelected').each(function () {
                    if ($(this).find('input').prop('checked') == true) {
                        isHasCheckedTrans = 1;
                    }
                });

                if (isHasCheckedTrans == 1) {
                    if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                    } else {
                        var payerAmount = parseFloat($('#tdTotalAllPayer').html().replace('.00', '').split(',').join('')) + parseFloat($('.txtAdministrationFee').attr('hiddenVal')) + parseFloat($('.txtServiceFee').attr('hiddenVal'));
                        var patientAmount = parseFloat($('#tdTotalAllPatient').html().replace('.00', '').split(',').join('')) + parseFloat($('.txtPatientAdministrationFee').attr('hiddenVal')) + parseFloat($('.txtPatientServiceFee').attr('hiddenVal'));

                        var maxAmount = payerAmount;

                        if (hdnFN0182 == '1') {
                            maxAmount = payerAmount + patientAmount;
                        }

                        if (coverageLimit > maxAmount) {
                            coverageLimit = maxAmount;
                        }
                    }
                }

                var totalBill = $('#tdBillTotalAll').html();
                if (totalBill == null) {
                    totalBill = "0";
                }
                totalBill = parseFloat(totalBill.replace('.00', '').split(',').join(''));
                var amount = parseFloat($(this).val());
                if (amount > totalBill) {
                    $(this).val(totalBill).trigger('change');
                    $(this).val(totalBill).trigger('changeValue');
                    amount = totalBill;
                }

                //                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(amount);
                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(coverageLimit);
                $('#tdRemainingCoverageAmount').html(amount.formatMoney(2, '.', ','));
                CheckAllTest();

                var coverageAmount = parseFloat($('#<%=txtCoverageLimit.ClientID %>').val());
                if (coverageAmount < 0) {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Batas tanggungan tidak boleh minus !");
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('change');
                    $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('changeValue');
                } else {
                    var isCoverageAllowBiggerThanBillingAmount = $('#<%=hdnIsCoverageAllowBiggerThanBillingAmount.ClientID %>').val();
                    var lineAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                    if (isCoverageAllowBiggerThanBillingAmount == "0") {
                        if (coverageAmount > lineAmount) {
                            displayErrorMessageBox("Silahkan Coba Lagi", "Batas Tanggungan tidak boleh lebih besar dari Total Tagihan !");
                            $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('change');
                            $('#<%=txtCoverageLimit.ClientID %>').val("0").trigger('changeValue');
                        }
                    }
                }
            });

            $('#<%=txtPatientBPJSAmount.ClientID %>').live('change', function () {
                var amount = parseFloat($(this).val());
                $('#<%=hdnPatientBPJSAmount.ClientID %>').val(amount);
                calculateBillTotal();
            });

            $('#<%=txtPatientAmount.ClientID %>').live('change', function () {
                var value = $('#<%=trFilterCoverage.ClientID %>').find('input:checked').val();
                if (value == "patient") {
                    var amount = parseFloat($(this).val());
                    $('#<%=hdnPatientAmount.ClientID %>').val(amount);

                    var totalAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                    var payerAmount = totalAmount - amount;

                    $('#<%=txtCoverageLimit.ClientID %>').val(payerAmount).trigger('change');
                    $('#<%=txtCoverageLimit.ClientID %>').val(payerAmount).trigger('changeValue');
                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(payerAmount);
                    $('#tdRemainingCoverageAmount').html(payerAmount.formatMoney(2, '.', ','));
                }
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

        $('.lnkNursingNote').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var url = '~/Libs/Program/Module/PatientManagement/Information/ViewChargesNursingNotesCtl.ascx';
            openUserControlPopup(url, visitID + '|' + id, 'Catatan Perawat untuk Transaksi', 1100, 500);
        });

        $('.imgRecalChargesHdDt.imgLink').die('click');
        $('.imgRecalChargesHdDt.imgLink').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var transactionNo = $tr.find('.hdnTransationNo').val();
            if (confirm("Proses hitung ulang untuk transaksi nomor " + transactionNo + " ?")) {
                cbpView.PerformCallback("recalchargeshd|" + id);
            }
        });

        $('#btnAdminDetail').live('click', function () {
            //            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            //            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            //            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/BillHistory.ascx");
            //            var id = registrationID + '|' + linkedRegisID;
            //            openUserControlPopup(url, id, 'Billing History', 1100, 500);

            if (confirm("Proses hitung ulang Biaya Administrasi dan Biaya Service ?")) {
                cbpView.PerformCallback("recaladminservice");
            }
        });

        $('.chkIsAllowVariableAdmin input').live('change', function () {
            if ($(this).is(':checked')) {
                $('.txtAdministrationFee').removeAttr('readonly');
                $('.txtPatientAdministrationFee').removeAttr('readonly');
                $('.txtServiceFee').removeAttr('readonly');
                $('.txtPatientServiceFee').removeAttr('readonly');
            } else {
                $('.txtAdministrationFee').attr('readonly', 'readonly');
                $('.txtPatientAdministrationFee ').attr('readonly', 'readonly');
                $('.txtServiceFee').attr('readonly', 'readonly');
                $('.txtPatientServiceFee').attr('readonly', 'readonly');
            }
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
            $('.chkIsAllowVariableAdmin input').prop('checked', false);
            cbpView.PerformCallback();
        }

        $('.txtAdministrationFee').live('change', function () {
            var fee = parseFloat($(this).val());
            var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            if (!isAllowVariableAdmin) {
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');

            var payerAmount = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());
            var adminAmount = parseFloat($('#<%=hdnAdministrationFeeAmount.ClientID %>').val());
            var serviceAmount = parseFloat($('#<%=hdnServiceFeeAmount.ClientID %>').val());

            var coverageLimit = $('#<%=txtCoverageLimit.ClientID %>').val();
            coverageLimit = parseFloat(coverageLimit.replace('.00', '').split(',').join(''));
            if (coverageLimit > 0) {
                var payerAmountCheck = parseFloat($('#tdTotalAllPayer').html().replace('.00', '').split(',').join('')) + parseFloat($('.txtAdministrationFee').attr('hiddenVal')) + parseFloat($('.txtServiceFee').attr('hiddenVal'));
                if (coverageLimit > payerAmountCheck) {
                    payerAmount = parseFloat($('#tdTotalAllPayer').html().replace('.00', '').split(',').join(''));
                    adminAmount = parseFloat($('.txtAdministrationFee').attr('hiddenVal'));
                    serviceAmount = parseFloat($('.txtServiceFee').attr('hiddenVal'));
                }
                else {
                    payerAmount = coverageLimit;
                    adminAmount = 0;
                    serviceAmount = 0;
                }
            }

            var totalPayer = payerAmount + adminAmount + serviceAmount;
            $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);

            calculateBillTotal();
        });

        $('.txtPatientAdministrationFee').live('change', function () {
            var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('minamount'));
            if (!isAllowVariableAdmin) {
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');

            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1") {
                var patientAmount = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
                var adminAmount = parseFloat($('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val());
                var serviceAmount = parseFloat($('#<%=hdnPatientServiceFeeAmount.ClientID %>').val());

                var totalPatient = patientAmount + adminAmount + serviceAmount;
                $('#<%=txtPatientBPJSAmount.ClientID %>').val(totalPatient).trigger('changeValue');
                $('#<%=hdnPatientBPJSAmount.ClientID%>').val(totalPatient);
                $('#<%=hdnTotalPatientAmount.ClientID%>').val(totalPatient);
            }
            else {
                var patientAmount = parseFloat($('#<%=hdnPatientAmount.ClientID %>').val());
                var adminAmount = parseFloat($('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val());
                var serviceAmount = parseFloat($('#<%=hdnPatientServiceFeeAmount.ClientID %>').val());

                var totalPatient = patientAmount + adminAmount + serviceAmount;
                $('#<%=txtPatientAmount.ClientID %>').val(totalPatient).trigger('changeValue');
            }

            calculateBillTotal();
        });

        $('.txtServiceFee').live('change', function () {
            var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            if (!isAllowVariableAdmin) {
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnServiceFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');

            var payerAmount = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());
            var adminAmount = parseFloat($('#<%=hdnAdministrationFeeAmount.ClientID %>').val());
            var serviceAmount = parseFloat($('#<%=hdnServiceFeeAmount.ClientID %>').val());

            var totalPayer = payerAmount + adminAmount + serviceAmount;
            $('#<%=txtPatientAmount.ClientID %>').val("0").trigger('changeValue');
            $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);

            calculateBillTotal();
        });

        $('.txtPatientServiceFee').live('change', function () {
            var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
            var fee = parseFloat($(this).val());
            if (isNaN(fee))
                fee = 0;
            var max = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('minamount'));
            if (!isAllowVariableAdmin) {
                if ((fee < min) && (min > 0))
                    fee = min;
                if ((fee > max) && (max > 0))
                    fee = max;
            }
            $('#<%=hdnPatientServiceFeeAmount.ClientID %>').val(fee);
            resetTotalAmountPatient();
            $(this).val(fee).trigger('changeValue');

            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1") {
                var patientAmount = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
                var adminAmount = parseFloat($('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val());
                var serviceAmount = parseFloat($('#<%=hdnPatientServiceFeeAmount.ClientID %>').val());

                var totalPatient = patientAmount + adminAmount + serviceAmount;
                $('#<%=txtPatientBPJSAmount.ClientID %>').val(totalPatient).trigger('changeValue');
                $('#<%=hdnPatientBPJSAmount.ClientID%>').val(totalPatient);
                $('#<%=hdnTotalPatientAmount.ClientID%>').val(totalPatient);
            }
            else {
                var patientAmount = parseFloat($('#<%=hdnPatientAmount.ClientID %>').val());
                var adminAmount = parseFloat($('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val());
                var serviceAmount = parseFloat($('#<%=hdnPatientServiceFeeAmount.ClientID %>').val());

                var totalPatient = patientAmount + adminAmount + serviceAmount;
                $('#<%=txtPatientAmount.ClientID %>').val(totalPatient).trigger('change');
            }

            calculateBillTotal();
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            $('.txtCurrency').each(function () {
                $(this).blur();
            });

            var param = s.cpResult.split('|');

            if (param[1] == "recalchargeshd") {
                cbpView.PerformCallback();
            } else {
                var patientRounding = parseFloat($('#<%=hdnPatientRoundingBySystem.ClientID %>').val());
                var payerRounding = parseFloat($('#<%=hdnPayerRoundingBySystem.ClientID %>').val());

                $('#<%=hdnPatientRoundingAmount.ClientID %>').val(patientRounding);
                $('#<%=hdnPayerRoundingAmount.ClientID %>').val(payerRounding)

                $('.txtPatientRoundingAmount').val(patientRounding).trigger('changeValue');
                $('.txtPayerRoundingAmount').val(payerRounding).trigger('changeValue');

                if (param[1] == "recaladminservice") {
                    $('.chkIsAllowVariableAdmin input').prop('checked', false);
                }

                onLoadGenerateBill();
            }

            hideLoadingPanel();
        }

        function calculateTotalTest() {
            var payerAmount = 0;
            var patientAmount = 0;
            var lineAmount = 0;
            var param = '';
            var baseAdminPayerAmount = 0;
            var baseAdminPatientAmount = 0;

            var hdnFN0182 = $('#<%=hdnFN0182.ClientID %>').val();

            //TOTAL SEMUA DT

            var isCheckedCharges = 0;
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
                payerAmount = parseFloat(payerAmount.toFixed(2));
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                patientAmount = parseFloat(patientAmount.toFixed(2));
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
                lineAmount = parseFloat(lineAmount.toFixed(2));

                $('#<%=hdnTotalPatientAmountTemp.ClientID %>').val(patientAmount);
                $('#<%=hdnTotalPayerAmountTemp.ClientID %>').val(payerAmount);

                isCheckedCharges += 1;
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
            var payerAdminFeeFormulaVersion = parseFloat($('#<%=hdnPayerAdminFeeFormulaVersion.ClientID %>').val());
            var payerAmountFormulaVersion = parseFloat($('#<%=hdnPayerAmountFormulaVersion.ClientID %>').val());

            var num1 = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').val());
            var num2 = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').val());
            var payerFee = 0;
            var patientFee = 0;

            var payerMin = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            var payerMax = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var patientMin = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('minamount'));
            var patientMax = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('maxamount'));

            if (payerAmount != 0) {
                if ($('#<%=hdnAdministrationFee.ClientID %>').attr('ispercentage') == '1') {
                    if (payerAdminFeeFormulaVersion == "2") {
                        payerFee = baseAdminPayerAmount / (num1 / 100);
                    } else {
                        payerFee = baseAdminPayerAmount * (num1 / 100);
                    }
                }
                else {
                    payerFee = num1;
                }

                if ((baseAdminPayerAmount > 0) && (payerMin > 0) && (payerFee < payerMin))
                    payerFee = payerMin;
                if ((baseAdminPayerAmount > 0) && (payerMax > 0) && payerFee > payerMax)
                    payerFee = payerMax;
            }

            if (patientAmount != 0) {
                if ($('#<%=hdnPatientAdminFee.ClientID %>').attr('ispercentage') == '1') {
                    patientFee = baseAdminPatientAmount * num2 / 100;
                }
                else {
                    patientFee = num2;
                }

                if ((baseAdminPatientAmount > 0) && (patientMin > 0) && (patientFee < patientMin))
                    patientFee = patientMin;
                if ((baseAdminPatientAmount > 0) && (patientMax > 0) && (patientFee > patientMax))
                    patientFee = payerMax;
            }

            $('.txtAdministrationFee').val(payerFee).trigger('changeValue');
            $('.txtPatientAdministrationFee').val(patientFee).trigger('changeValue');
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(payerFee);
            $('#<%=hdnPatientAdministrationFeeAmount.ClientID %>').val(patientFee);
            //#endregion

            //#region Service
            var servicePayerFee = 0;
            var servicePatientFee = 0;
            //#endregion

            patientAmount = patientAmount + patientFee + servicePatientFee;

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);

            var txtCoverageLimitIsChanged = $('#<%=hdnTxtCoverageLimitIsChanged.ClientID %>').val();
            var isCoverageFromRegistration = $('#<%=hdnIsFromRegistrationCoverage.ClientID %>').val();
            var coverageAmountFromRegistrationCoverage = $('#<%=hdnCoverageAmountFromRegistrationCoverage.ClientID %>').val();
            var hdnRemainingCoverageAmount = parseFloat($('#<%=hdnRemainingCoverageAmount.ClientID %>').val());
            var administrationFee = parseFloat($('.txtAdministrationFee').attr('hiddenVal'));
            var serviceFee = parseFloat($('.txtServiceFee').attr('hiddenVal'));
            if (isNaN(administrationFee))
                administrationFee = 0;
            if (isNaN(serviceFee))
                serviceFee = 0;
            var totalPayer = payerAmount + administrationFee + serviceFee;

            if (!$('#<%=chkIsEditCoverageBPJSManual.ClientID %>').is(':checked')) {
                if (totalPayer != 0) {
                    if (hdnRemainingCoverageAmount == 0) {
                        if (isCoverageFromRegistration == 0) {
                            var type = $('#<%=trFilterCoverage.ClientID %>').find('input:checked').val();
                            var totalPatient = parseFloat($('#<%=hdnPatientAmount.ClientID %>').val());
                            if (type == 'patient') {
                                if (totalPatient == totalPayer) {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(0).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(0);
                                }
                                else {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                                }
                            }
                            else {
                                $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                            }
                        } else {
                            $('#<%=txtCoverageLimit.ClientID %>').val(coverageAmountFromRegistrationCoverage).trigger('changeValue');
                            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(coverageAmountFromRegistrationCoverage);
                        }
                    }
                    else {
                        if (txtCoverageLimitIsChanged == "1") {
                            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                            } else {
                                var totalMax = totalPayer;
                                if (hdnFN0182 == '1') {
                                    totalMax = totalPayer + patientAmount
                                }

                                if (hdnRemainingCoverageAmount > totalMax) {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(totalMax).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalMax);
                                }
                                else {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(hdnRemainingCoverageAmount).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(hdnRemainingCoverageAmount);
                                }
                            }
                        } else {
                            $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                        }
                    }
                } else {
                    $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                }
            } else {
                $('#<%=txtCoverageLimit.ClientID %>').val(hdnRemainingCoverageAmount).trigger('changeValue');
                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(hdnRemainingCoverageAmount);
            }

            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                $('#<%=txtPatientBPJSAmount.ClientID %>').val(patientAmount).trigger('changeValue');
                $('#<%=hdnPatientBPJSAmount.ClientID%>').val(patientAmount);
            }

            calculateBillTotal();
        }

        function calculateTotal() {
            var payerAmount = 0;
            var patientAmount = 0;
            var lineAmount = 0;
            var param = '';
            var baseAdminPayerAmount = 0;
            var baseAdminPatientAmount = 0;

            //TOTAL SEMUA DT

            var isCheckedCharges = 0;
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
                payerAmount = parseFloat(payerAmount.toFixed(2));
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                patientAmount = parseFloat(patientAmount.toFixed(2));
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
                lineAmount = parseFloat(lineAmount.toFixed(2));

                $('#<%=hdnTotalPatientAmountTemp.ClientID %>').val(patientAmount);
                $('#<%=hdnTotalPayerAmountTemp.ClientID %>').val(payerAmount);

                isCheckedCharges += 1;
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
            var payerAdminFeeFormulaVersion = parseFloat($('#<%=hdnPayerAdminFeeFormulaVersion.ClientID %>').val());
            var payerAmountFormulaVersion = parseFloat($('#<%=hdnPayerAmountFormulaVersion.ClientID %>').val());

            var num1 = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').val());
            var num2 = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').val());
            var payerFee = 0;
            var patientFee = 0;

            var payerMin = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            var payerMax = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var patientMin = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('minamount'));
            var patientMax = parseFloat($('#<%=hdnPatientAdminFee.ClientID %>').attr('maxamount'));

            if (payerAmount != 0) {
                if ($('#<%=hdnAdministrationFee.ClientID %>').attr('ispercentage') == '1') {
                    if (payerAdminFeeFormulaVersion == "2") {
                        payerFee = baseAdminPayerAmount / (num1 / 100);
                    } else {
                        payerFee = baseAdminPayerAmount * (num1 / 100);
                    }
                }
                else {
                    payerFee = num1;
                }

                if ((baseAdminPayerAmount > 0) && (payerMin > 0) && (payerFee < payerMin))
                    payerFee = payerMin;
                if ((baseAdminPayerAmount > 0) && (payerMax > 0) && payerFee > payerMax)
                    payerFee = payerMax;
            }

            if (patientAmount != 0) {
                if ($('#<%=hdnPatientAdminFee.ClientID %>').attr('ispercentage') == '1') {
                    patientFee = baseAdminPatientAmount * num2 / 100;
                }
                else {
                    patientFee = num2;
                }

                if ((baseAdminPatientAmount > 0) && (patientMin > 0) && (patientFee < patientMin))
                    patientFee = patientMin;
                if ((baseAdminPatientAmount > 0) && (patientMax > 0) && (patientFee > patientMax))
                    patientFee = payerMax;
            }

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

            var payerServiceMin = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            var payerServiceMax = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var patientServiceMin = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('minamount'));
            var patientServiceMax = parseFloat($('#<%=hdnPatientServiceFee.ClientID %>').attr('maxamount'));

            if (payerAmount != 0) {
                if ($('#<%=hdnServiceFee.ClientID %>').attr('ispercentage') == '1') {
                    if (payerAdminFeeFormulaVersion == "2") {
                        servicePayerFee = payerFee * num1 / 100;
                    } else {
                        servicePayerFee = baseAdminPayerAmount * num1 / 100;
                    }
                }
                else {
                    servicePayerFee = num1;
                }

                if ((baseAdminPayerAmount > 0) && (payerServiceMin > 0) && (servicePayerFee < payerServiceMin))
                    servicePayerFee = payerServiceMin;
                if ((baseAdminPayerAmount > 0) && (payerServiceMax > 0) && (servicePayerFee > payerServiceMax))
                    servicePayerFee = payerServiceMax;
            }

            if (patientAmount != 0) {
                if ($('#<%=hdnPatientServiceFee.ClientID %>').attr('ispercentage') == '1') {
                    servicePatientFee = baseAdminPatientAmount * num2 / 100;
                }
                else {
                    servicePatientFee = num2;
                }

                if ((baseAdminPatientAmount > 0) && (patientServiceMin > 0) && (servicePatientFee < patientServiceMin))
                    servicePatientFee = patientServiceMin;
                if ((baseAdminPatientAmount > 0) && (patientServiceMax > 0) && (servicePatientFee > patientServiceMax))
                    servicePatientFee = patientServiceMax;
            }

            $('.txtServiceFee').val(servicePayerFee).trigger('changeValue');
            $('.txtPatientServiceFee').val(servicePatientFee).trigger('changeValue');

            $('#<%=hdnServiceFeeAmount.ClientID %>').val(servicePayerFee);
            $('#<%=hdnPatientServiceFeeAmount.ClientID %>').val(servicePatientFee);
            //#endregion

            patientAmount = patientAmount + patientFee + servicePatientFee;

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);

            var txtCoverageLimitIsChanged = $('#<%=hdnTxtCoverageLimitIsChanged.ClientID %>').val();
            var isCoverageFromRegistration = $('#<%=hdnIsFromRegistrationCoverage.ClientID %>').val();
            var coverageAmountFromRegistrationCoverage = $('#<%=hdnCoverageAmountFromRegistrationCoverage.ClientID %>').val();
            var hdnRemainingCoverageAmount = parseFloat($('#<%=hdnRemainingCoverageAmount.ClientID %>').val());
            var administrationFee = parseFloat($('.txtAdministrationFee').attr('hiddenVal'));
            var serviceFee = parseFloat($('.txtServiceFee').attr('hiddenVal'));
            if (isNaN(administrationFee))
                administrationFee = 0;
            if (isNaN(serviceFee))
                serviceFee = 0;

            var totalPayer = payerAmount + administrationFee + serviceFee;

            if (payerAmountFormulaVersion == "2") {
                totalPayer = administrationFee + serviceFee;
            }

            if (!$('#<%=chkIsEditCoverageBPJSManual.ClientID %>').is(':checked')) {
                if (totalPayer != 0) {
                    if (hdnRemainingCoverageAmount == 0) {
                        if (isCoverageFromRegistration == 0) {
                            var type = $('#<%=trFilterCoverage.ClientID %>').find('input:checked').val();
                            var totalPatient = parseFloat($('#<%=hdnPatientAmount.ClientID %>').val());
                            if (type == 'patient') {
                                if (totalPatient == totalPayer) {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(0).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(0);
                                }
                                else {
                                    $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                                }
                            }
                            else {
                                $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                            }
                        } else {
                            $('#<%=txtCoverageLimit.ClientID %>').val(coverageAmountFromRegistrationCoverage).trigger('changeValue');
                            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(coverageAmountFromRegistrationCoverage);
                        }
                    }
                    else {
                        if (txtCoverageLimitIsChanged == "1") {
                            if (hdnRemainingCoverageAmount > totalPayer) {
                                $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                            }
                            else {
                                $('#<%=txtCoverageLimit.ClientID %>').val(hdnRemainingCoverageAmount).trigger('changeValue');
                                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(hdnRemainingCoverageAmount);
                            }
                        } else {
                            $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                            $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                        }
                    }
                } else {
                    $('#<%=txtCoverageLimit.ClientID %>').val(totalPayer).trigger('changeValue');
                    $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(totalPayer);
                }
            } else {
                $('#<%=txtCoverageLimit.ClientID %>').val(hdnRemainingCoverageAmount).trigger('changeValue');
                $('#<%=hdnRemainingCoverageAmount.ClientID %>').val(hdnRemainingCoverageAmount);
            }

            if ($('#<%=hdnIsBPJS.ClientID %>').val() == "1" && $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val() == "1") {
                $('#<%=txtPatientBPJSAmount.ClientID %>').val(patientAmount).trigger('changeValue');
                $('#<%=hdnPatientBPJSAmount.ClientID%>').val(patientAmount);
            }

            calculateBillTotal();
        }

        function calculateBillTotal() {
            var payerAmountFormulaVersion = parseFloat($('#<%=hdnPayerAmountFormulaVersion.ClientID %>').val());
            var patientAmount = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
            var payerAmount = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());
            var lineAmount = parseFloat($('#<%=hdnTotalAmount.ClientID %>').val());
            var diffPayerAmount = 0;
            var administrationFee = parseFloat($('.txtAdministrationFee').attr('hiddenVal'));
            var serviceFee = parseFloat($('.txtServiceFee').attr('hiddenVal'));
            var patientAdministrationFee = parseFloat($('.txtPatientAdministrationFee').attr('hiddenVal'));
            var patientServiceFee = parseFloat($('.txtPatientServiceFee').attr('hiddenVal'));
            var payerRounding = parseFloat($('.txtPayerRoundingAmount').attr('hiddenVal'));
            var patientRounding = parseFloat($('.txtPatientRoundingAmount').attr('hiddenVal'));
            if (isNaN(administrationFee))
                administrationFee = 0;
            if (isNaN(patientAdministrationFee))
                patientAdministrationFee = 0;
            if (isNaN(serviceFee))
                serviceFee = 0;
            if (isNaN(patientServiceFee))
                patientServiceFee = 0;

            if (payerAmountFormulaVersion == "2") {
                payerAmount = administrationFee + serviceFee;
            }
            else {
                payerAmount = payerAmount + administrationFee + serviceFee;
            }

            if ($('#<%=hdnIsControlCoverageLimit.ClientID %>').val() == '1') {
                var remainingCoverageAmount = parseFloat($('#<%=hdnRemainingCoverageAmount.ClientID %>').val());
                var patientBPJSAmount = parseFloat($('#<%=hdnPatientBPJSAmount.ClientID%>').val());
                var isBPJSClass = $('#<%=hdnIsBPJSClass.ClientID %>').val();
                var isApplyBPJSVariance = $('#<%=hdnIsApplyBPJSClassCareVarianceToPatient.ClientID %>').val();
                var isControlBPJSCoverage = $('#<%=hdnIsControlBPJSCoverage.ClientID %>').val();
                var isBPJSUsedBPJSCoverage = $('#<%=hdnBPJSMenggunakanCaraCoverageBPJS.ClientID %>').val();
                var totalAll = $('#tdTotalAll').html();

                if (totalAll == null || totalAll == "")
                    totalAll = "0.00";

                if (isControlBPJSCoverage == "1" && isBPJSUsedBPJSCoverage == "1") {
                    patientAmount = patientBPJSAmount;
                    diffPayerAmount = (patientBPJSAmount + remainingCoverageAmount).toFixed(2) - (parseFloat(totalAll.split(',').join('')) + administrationFee + serviceFee + patientAdministrationFee + patientServiceFee).toFixed(2);
                }
                else {
                    var patientAmountStr = '0';
                    if ($('#tdTotalAllPatient').html() != null) {
                        var patientAmountStr = $('#tdTotalAllPatient').html();
                    }
                    patientAmount = parseFloat(patientAmountStr.replace('.00', '').split(',').join(''));
                    patientAmount = (patientAmount + patientAdministrationFee + patientServiceFee) + (payerAmount) - remainingCoverageAmount;
                }

                payerAmount = remainingCoverageAmount;
            }
            else {
                if (payerRounding != 0) {
                    $('.txtPayerRoundingAmount').val("0").trigger('changeValue');
                    patientAmount += patientRounding;
                } else {
                    patientAmount += patientRounding;
                }

                var isAllowVariableAdmin = $('.chkIsAllowVariableAdmin input').is(':checked');
                if (isAllowVariableAdmin) {
                    patientAmount = (patientAmount + patientAdministrationFee + patientServiceFee);
                }
            }

            lineAmount = patientAmount + payerAmount;

            $('#tdRemainingCoverageAmount').html(parseFloat($('#<%=hdnRemainingCoverageAmount.ClientID %>').val()).formatMoney(2, '.', ','));

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

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount.toFixed(2));
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount.toFixed(2));

            $('#<%=hdnDiffCoverageAmount.ClientID %>').val(diffPayerAmount);

            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount.toFixed(2));
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var healthcareServiceUnitID = cboServiceUnit.GetValue();

            filterExpression.text = 'RegistrationID = ' + registrationID;

            if (code == 'PM-00224' || code == 'PM-00231' || code == 'PM-00262' || code == 'PM-00263'
                    || code == 'PM-00258' || code == 'PM-00259' || code == 'PM-00260' || code == 'PM-00261' || code == 'PM-00262'
                    || code == 'PM-00263' || code == 'PM-00264' || code == 'PM-00265' || code == 'PM-00266' || code == 'PM-00267'
                    || code == 'PM-00268' || code == 'PM-00269' || code == 'PM-00270' || code == 'PM-00271' || code == 'PM-00276'
                    || code == 'PM-00277' || code == 'PM-00332' || code == 'PM-002168' || code == 'PM-00721') {
                filterExpression.text = registrationID + ';' + healthcareServiceUnitID;
            }

            var allowPrint = $('#<%=hdnAllowPrint.ClientID %>').val();
            if (code == 'PM-00206') {
                if (allowPrint.indexOf("PTR") < 0) {
                    errMessage.text = "Pasien ini tidak memiliki transaksi Transfer";
                    return false;
                } else {
                    if (linkedRegisID != "" && linkedRegisID != "0") {
                        filterExpression.text = '((LinkedToRegistrationID = ' + registrationID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
                    }
                }
            }

            if (code == 'PM-00203' || code == 'PM-00204' || code == 'PM-00205' || code == 'PM-00206'
                || code == 'PM-00207' || code == 'PM-00208' || code == 'PM-00209' || code == 'PM-00210'
                || code == 'PM-00211' || code == 'PM-00212' || code == 'PM-00213' || code == 'PM-00242'
                || code == 'PM-00323' || code == 'PM-00410'
                || code == 'PM-00412' || code == 'PM-00414' || code == 'PM-00416' || code == 'PM-002108'
                || code == 'PM-002109') {
                filterExpression.text = '((LinkedToRegistrationID = ' + registrationID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
            }

            if (code == 'PM-00139' || code == 'PM-00248' || code == 'PM-00415' || code == 'PM-00417' || code == 'PM-00513'
                    || code == 'PM-00523' || code == 'PM-00323' || code == 'PM-00242'
                    || code == 'PM-00243' || code == 'PM-90028') {
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
            if (code == "MR000016") {
                var filter = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                Methods.getObject('GetvRegistration5List', filter, function (result) {
                    if (result != null) {
                        filterExpression.text = result.RegistrationID;
                    }
                });
            }
            if (code == "MR000017" || code == "MR000023" || code == "MR000024") {
                filterExpression.text = $('#<%=hdnVisitID.ClientID %>').val();
            }
            if (code == "PM-002107") {
                filterExpression.text = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
            }
            if (code == "PM-00622") {
                filterExpression.text = registrationID;
            }
            return true;
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'paymentLetter') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + "|pl";
                return param;
            }
            else if (code == 'paymentDifferenceLetter') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val() + "|pdl";
                return param;
            }
            else if (code == 'infoHistoryBilling') {
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
                var param = registrationID + '|' + linkedRegisID;
                return param;
            }
            else if (code == 'registrationClaimHistory') {
                var param = $('#<%:hdnRegistrationID.ClientID %>').val();
                return param;
            }
            else {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }

        function setCustomButtonEnabled() {
            var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var isUsedCalculateCoveragePerBillingGroup = $('#<%=hdnIsUsedCalculateCoveragePerBillingGroup.ClientID %>').val();

            if (isUsedCalculateCoveragePerBillingGroup == "1") {
                if (businessPartnerID == 1) {
                    $('#<%=btnCalculateCoverageLimit.ClientID %>').hide();
                } else {
                    $('#<%=btnCalculateCoverageLimit.ClientID %>').show();
                }
            } else {
                $('#<%=btnCalculateCoverageLimit.ClientID %>').hide();
            }
        }		
    </script>
    <style type="text/css">
        .trFooter td
        {
            height: 10px;
        }
    </style>
    <input type="hidden" value="0" id="hdnTxtCoverageLimitIsChanged" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="0" id="hdnTotalPatientAmountTemp" runat="server" />
    <input type="hidden" value="0" id="hdnTotalPayerAmountTemp" runat="server" />
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
    <input type="hidden" value="1" id="hdnPayerAdminFeeFormulaVersion" runat="server" />
    <input type="hidden" value="1" id="hdnPayerAmountFormulaVersion" runat="server" />
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
    <input type="hidden" value="0" id="hdnIsBPJS" runat="server" />
    <input type="hidden" value="0" id="hdnAllowPrint" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasTotalDifferent" runat="server" />
    <input type="hidden" value="0" id="hdnIsPendingRecalculated" runat="server" />
    <input type="hidden" value="0" id="hdnIsAdministrationFeeUseHigherClass" runat="server" />
    <input type="hidden" value="0" id="hdnIsLocked" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasTransactionAndOrderOutstanding" runat="server" />
    <input type="hidden" value="0" id="hdnIsAdministrationFeeOnlyForInpatient" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedCalculateCoveragePerBillingGroup" runat="server" />
    <input type="hidden" value="0" id="hdnPembuatanTagihanTidakAdaOutstandingOrder" runat="server" />
    <input type="hidden" value="0" id="hdnBlokPembuatanTagihanSaatAdaTransaksiOpen" runat="server" />
    <input type="hidden" value="0" id="hdnBPJSMenggunakanCaraCoverageBPJS" runat="server" />
    <input type="hidden" value="" id="hdnMenggunakanPembulatan" runat="server" />
    <input type="hidden" value="" id="hdnNilaiPembulatan" runat="server" />
    <input type="hidden" value="" id="hdnPembulatanKemana" runat="server" />
    <input type="hidden" value="0" id="hdnPatientRoundingAmount" runat="server" />
    <input type="hidden" value="0" id="hdnPayerRoundingAmount" runat="server" />
    <input type="hidden" value="" id="hdnIntervalFilterDate" runat="server" />
    <input type="hidden" value="" id="hdnIsHasTotalChargesDifferent" runat="server" />
    <input type="hidden" value="" id="hdnIsPembuatanTagihanMenggunakanFilterCoverage"
        runat="server" />
    <input type="hidden" value="" id="hdnFN0182" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowProcessBillWhenPendingRecalculated" runat="server" />
    <input type="hidden" value="" id="hdnNotificationRegistrationIsLinkedToRegistrationInpatient" runat="server" />
    <div>
        <table class="tblEntryContent">
            <colgroup>
                <col style="width: 220px" />
                <col style="width: 450px" />
                <col />
                <col style="width: 450px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Verifikasi")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDisplayVerification" ClientInstanceName="cboDisplayVerification"
                        Width="100%" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDisplayVerificationChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    &nbsp
                </td>
                <td rowspan="5" style="vertical-align: top">
                    <table id="tblInfoWarning" runat="server">
                        <tr>
                            <td style="vertical-align: top" class="blink-alert">
                                <img height="60" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' class="blink-alert" />
                            </td>
                            <td style="vertical-align: middle">
                                <label class="lblWarning" id="lblInfoOutstandingBill" runat="server">
                                    <%=GetLabel("Masih Ada Tagihan yang Belum Ditransfer") %></label>
                                <br />
                                <label class="lblWarning" id="lblInfoOutstandingOrder" runat="server">
                                    <%=GetLabel("Masih Ada Order yang Belum Diproses") %></label>
                                <br />
                                <label class="lblWarning" id="lblInfoTransactionOpen" runat="server" style="color: Purple">
                                    <%=GetLabel("Masih Ada Transaksi yang Masih OPEN") %></label>
                                <br />
                                <label class="lblWarning" id="lblInfoChargesNotBalance" runat="server" style="color: Maroon">
                                    <%=GetLabel("Masih Ada Transaksi yang Masih Belum Balance") %></label>
                                <br />
                                <label class="lblWarning" id="lblInfoOutstandingRegistrationBedCharges" runat="server"
                                    style="color: Orange">
                                    <%=GetLabel("Masih Ada Transaksi Tempat Tidur yang Belum Diproses") %></label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Filter Tanggal Transaksi") %></label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblFilterDate" RepeatDirection="Horizontal">
                        <asp:ListItem Text="On" Value="true" />
                        <asp:ListItem Text="Off" Value="false" Selected="True" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trDate">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Transaksi") %></label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFilterTransactionDateFrom" Width="110px" CssClass="datepicker"
                                    runat="server" />
                            </td>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel(" s/d ") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFilterTransactionDateTo" Width="110px" CssClass="datepicker"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
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
                        <asp:ListItem Text="Patient" Value="patient" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trCoverageLimit" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%:GetCoverageLabel()%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtCoverageLimit" CssClass="txtCurrency" Width="100%" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="chkIsEditCoverageBPJSManual" Checked="false" Width="100%" runat="server" />
                </td>
                <td colspan="2" style="display: none">
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
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Di Tanggung Pasien (BPJS)")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientBPJSAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                </td>
            </tr>
            <tr id="trPatientAmount" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Di Tanggung Pasien")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 420px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <input type="hidden" value="0" id="hdnPatientRoundingBySystem" runat="server" />
                                        <input type="hidden" value="0" id="hdnPayerRoundingBySystem" runat="server" />
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left; width: 180px;">
                                                                <div>
                                                                    <%= GetLabel("No. Transaksi")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal")%></div>
                                                            </div>
                                                            <div style="padding: 3px; float: left; width: 100px;">
                                                                <div>
                                                                    <%= GetLabel("Unit Pelayanan")%></div>
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 20px; float: left; width: 120px">
                                                                <div>
                                                                    <%= GetLabel("Kelas")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 400px;">
                                                                <div>
                                                                    <%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div>
                                                                <%=GetLabel("Hitung Ulang") %></div>
                                                        </th>
                                                        <th rowspan="2">
                                                            <div>
                                                                <%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 50px">
                                                            <div style="text-align: center;">
                                                                <%=GetLabel("Verified")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
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
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left; width: 180px;">
                                                                <div>
                                                                    <%= GetLabel("No. Transaksi")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal")%></div>
                                                            </div>
                                                            <div style="padding: 3px; float: left; width: 100px;">
                                                                <div>
                                                                    <%= GetLabel("Unit Pelayanan")%></div>
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 20px; float: left; width: 120px">
                                                                <div>
                                                                    <%= GetLabel("Kelas")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 400px;">
                                                                <div>
                                                                    <%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div>
                                                                <%=GetLabel("Hitung Ulang") %></div>
                                                        </th>
                                                        <th rowspan="2">
                                                            <div>
                                                                <%=GetLabel("Status") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 50px">
                                                            <div style="text-align: center;">
                                                                <%=GetLabel("Verified")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
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
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPayer">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPatient">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAll">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterAdministrationFee" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Biaya Administrasi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtAdministrationFee" Text="0" runat="server" ReadOnly="true" Width="100%"
                                                                    CssClass="txtCurrency txtAdministrationFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtPatientAdministrationFee" Text="0" runat="server" ReadOnly="true"
                                                                    Width="100%" CssClass="txtCurrency txtPatientAdministrationFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 0px 3px; background-color: Aqua; height: 20px; width: 20px;
                                                                float: left;">
                                                                <img id="btnAdminDetail" src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>'
                                                                    alt="" height="20px" width="20px" style="cursor: pointer" />
                                                            </div>
                                                            <div>
                                                                <asp:CheckBox ID="chkIsAllowVariableAdmin" CssClass="chkIsAllowVariableAdmin" Checked="false"
                                                                    runat="server" /><%:GetLabel("Variable")%>
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterServiceFee" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Biaya Service")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtServiceFee" Text="0" runat="server" ReadOnly="true" Width="100%"
                                                                    CssClass="txtCurrency txtServiceFee" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtPatientServiceFee" Text="0" runat="server" ReadOnly="true" Width="100%"
                                                                    CssClass="txtCurrency txtPatientServiceFee" />
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                            <%--                                                            <div style="text-align:left;padding:3px" id="Div4">
                                                                <input type="button" id="btnApply" value='<%=GetLabel("Apply") %>' />
                                                            </div>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterRemainingCoverageLimit" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px; font-weight: bold">
                                                                <%=GetLabel("Batas Tanggungan")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdRemainingCoverageAmount">
                                                                <%=GetRemainingCoverageAmount()%>
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trRoundingAmount" runat="server" style="display: none">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Pembulatan")%>
                                                            </div>
                                                        </td>
                                                        <td id="tdPayerRounding" runat="server">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtPayerRoundingAmount" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtPayerRoundingAmount" />
                                                            </div>
                                                        </td>
                                                        <td id="tdPatientRounding" runat="server">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <asp:TextBox ID="txtPatientRoundingAmount" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtPatientRoundingAmount" />
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterRemainingTotalBill" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px">
                                                                <%=GetLabel("Total Tagihan")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdBillTotalAllPayer">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdBillTotalAllPatient">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdBillTotalAll">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterDiffCoverageLimit" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px; font-weight: bold">
                                                                <%=GetLabel("Selisih Tagihan RS dan BPJS")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdDiffCoverageAmount">
                                                                <%=GetDiffCoverageAmount()%>
                                                            </div>
                                                        </td>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr style="<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "background-color:#FFE4E1" : ""%>">
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnTransationNo" value="<%#: Eval("TransactionNo")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                            <input type="hidden" class="hdnDepartmentIDChargesDt" value="<%#: Eval("VisitDepartmentID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: right; margin-right: 50px; <%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td class="blink-alert">
                                                                        <img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' />
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblInfo">
                                                                            <%=GetLabel("Pending Recalculated") %></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding: 3px; float: left; width: 200px;">
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("TransactionNo")%></a>
                                                            <div>
                                                                <%#: Eval("TransactionDateInString")%>
                                                                <%#: Eval("TransactionTime")%>
                                                                <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' title="Has Compound"
                                                                    alt="" style='<%# Eval("IsHasCompound").ToString() == "0" ? "display:none;": "" %> max-width:20px;
                                                                    min-width: 20px;' />
                                                            </div>
                                                            <div style='<%# Eval("BPJSTransactionType").ToString() == "" ? "display:none": Eval("GCCustomerType").ToString() != "X004^500" ? "display:none": "" %>'>
                                                                <i>
                                                                    <%=GetLabel("Jenis Transaksi BPJS : ")%></i><b><%#: Eval("BPJSTransactionType")%></b>
                                                            </div>
                                                        </div>
                                                        <div style="padding: 3px; float: left; width: 100px;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                            <div>
                                                                <%#: Eval("LastUpdatedByUserName")%></div>
                                                        </div>
                                                        <div style="padding: 3px; margin-left: 20px; float: left; width: 120px">
                                                            <div>
                                                                <%#: Eval("ChargeClass")%></div>
                                                            <div>
                                                                &nbsp;</div>
                                                        </div>
                                                        <div style="padding: 3px; margin-left: 400px;">
                                                            <div>
                                                                <%#: Eval("Remarks")%></div>
                                                            <div>
                                                                &nbsp;</div>
                                                            <div>
                                                                <a class="lnkNursingNote" style='<%# Eval("IsLinkedToNursingNote").ToString() == "False" ? "display:none;": "" %> max-width:20px;
                                                                    min-width: 20px;'>
                                                                    <%=GetLabel("Catatan Perawat") %></a></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPayerAmount", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPatientAmount", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalAmount", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <img class="imgRecalChargesHdDt <%#: Eval("IsTotalDifferent").ToString() == "True" ? "imgLink" : "imgDisabled"%>"
                                                            title='<%=GetLabel("Hitung Ulang")%>' src='<%# Eval("IsTotalDifferent").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/recalculate.png") : ""%>'
                                                            alt="" style="width: 25px" />
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center;">
                                                            <%#: Eval("TransactionStatus")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center;">
                                                            <asp:CheckBox ID="chkIsVerified" runat="server" Checked='<%# Eval("IsVerified")%>'
                                                                Enabled="false" />
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
