<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="TransactionInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionInformation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            onLoadGenerateBill();

            setDatePicker('<%=txtTransactionDateFrom.ClientID %>');
            $('#<%=txtTransactionDateFrom.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtTransactionDateFrom.ClientID %>').change(function (evt) {
                var transDateFrom = $('#<%:txtTransactionDateFrom.ClientID %>').val();
                var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
                var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

                var transDateTo = $('#<%:txtTransactionDateTo.ClientID %>').val();
                var transactionDateTo = Methods.getDatePickerDate(transDateTo);
                var transDateYMDTo = dateToDMYCustom(transactionDateTo);

                if (transDateYMDFrom > transDateYMDTo) {
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                } else {
                    cbpView.PerformCallback('refresh');
                }
            });
            
            setDatePicker('<%=txtTransactionDateTo.ClientID %>');
            $('#<%=txtTransactionDateTo.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtTransactionDateTo.ClientID %>').change(function (evt) {
                var transDateFrom = $('#<%:txtTransactionDateFrom.ClientID %>').val();
                var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
                var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

                var transDateTo = $('#<%:txtTransactionDateTo.ClientID %>').val();
                var transactionDateTo = Methods.getDatePickerDate(transDateTo);
                var transDateYMDTo = dateToDMYCustom(transactionDateTo);

                if (transDateYMDFrom > transDateYMDTo) {
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                } else {
                    cbpView.PerformCallback('refresh');
                }
            });
        });

        $('.lnkTransactionNo').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var type = '';
            var transactionCode = $tr.find('.hdnTransactionCode').val();
            var prescriptionOrderID = parseInt($tr.find('.hdnPrescriptionOrderID').val());
            var prescriptionReturnOrderID = parseInt($tr.find('.hdnPrescriptionReturnOrderID').val());
            var url = '';

            if (prescriptionOrderID > 0) {
                type = 'Prescription'
            } else if (prescriptionReturnOrderID > 0) {
                type = 'PrescriptionReturn'
            } else {
                type = 'All'
            }

            id = id + '|' + type;
            url = ResolveUrl("~/Libs/Controls/PatientInformationTransactionDtCtl.ascx");
            openUserControlPopup(url, id, 'Informasi Detail Transaksi Pasien', 1200, 550);
        });

        $('#<%=rblFilterDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == 'true') {
                $('#trDate').css('display', '');
            }
            else $('#trDate').css('display', 'none');
            cbpView.PerformCallback();
        });

        function onLoadGenerateBill() {
            calculateTotal();
            $('.chkSelectAllVerificationVerification input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotal();
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkSelectAllVerificationVerification').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotal();
            });
            $('.chkSelectAllVerificationVerification input').prop('checked', true);
            $('.chkSelectAllVerificationVerification input').change();
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
            $('.chkSelectAllVerificationVerification input:checked').each(function () {
                $tr = $(this).closest('tr');
                payerAmount += parseFloat($tr.find('.hdnPayerAmount').val());
                patientAmount += parseFloat($tr.find('.hdnPatientAmount').val());
                lineAmount += parseFloat($tr.find('.hdnLineAmount').val());
            });

            $('#tdTotalAllPayer').html(payerAmount.formatMoney(2, '.', ','));
            $('#tdTotalAllPatient').html(patientAmount.formatMoney(2, '.', ','));
            $('#tdTotalAll').html(lineAmount.formatMoney(2, '.', ','));

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);

            calculateBillTotal();
        }

        function calculateBillTotal() {
            var patientAmount = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
            var payerAmount = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());
            var lineAmount = parseFloat($('#<%=hdnTotalAmount.ClientID %>').val());

            $('#<%=hdnTotalPatientAmount.ClientID %>').val(patientAmount);
            $('#<%=hdnTotalPayerAmount.ClientID %>').val(payerAmount);
            $('#<%=hdnTotalAmount.ClientID %>').val(lineAmount);

        }

        function onCboDisplayChanged() {
            cbpView.PerformCallback();
        }

        function onCboServiceUnitChanged() {
            cbpView.PerformCallback();
        }

        function dateToDMYCustom(date) {
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();

            var transDateFrom = $('#<%:txtTransactionDateFrom.ClientID %>').val();
            var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
            var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

            var transDateTo = $('#<%:txtTransactionDateTo.ClientID %>').val();
            var transactionDateTo = Methods.getDatePickerDate(transDateTo);
            var transDateYMDTo = dateToDMYCustom(transactionDateTo);

            if (code == 'PM-002111' || code == 'PM-002112') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            } else {
                if (registrationID == '') {
                    errMessage.text = 'Please Select Registration First!';
                    return false;
                }
                else if (transDateFrom == '' || transDateTo == '') {
                    errMessage.text = 'Please Select Date First!';
                    return false;
                }
                else {
                    //              filterExpression.text = registrationID + ' | ' + transDateYMD;
                    filterExpression.text = registrationID + '|' + transDateYMDFrom + ';' + transDateYMDTo;
                    return true;
                }
            }
        }
    </script>
    <style type="text/css">
        .trFooter td
        {
            height: 10px;
        }
    </style>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnIsControlCoverageLimit" runat="server" />
    <input type="hidden" value="" id="hdnRemainingCoverageAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnIsCustomerPersonal" runat="server" />
    <input type="hidden" value="" id="hdnAdministrationFee" runat="server" />
    <input type="hidden" value="" id="hdnServiceFee" runat="server" />
    <input type="hidden" value="0" id="hdnAdministrationFeeAmount" runat="server" />
    <input type="hidden" value="0" id="hdnServiceFeeAmount" runat="server" />
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%=GetLabel("Informasi Transaksi")%></div>
        </div>
        <table id="tblInfoOutstandingTransfer" runat="server">
            <tr>
                <td>
                    <img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' />
                </td>
                <td>
                    <label class="lblInfo" id="lblInfoOutstandingBill">
                        <%=GetLabel("Masih Ada Tagihan Yang Belum Ditransfer") %></label>
                </td>
            </tr>
        </table>
        <table class="tblEntryContent">
            <colgroup>
                <col style="width: 120px" />
                <col />
                <col style="width: 250px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tampilan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" Width="200px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDisplayChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Uang Muka") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDownPayment" Width="150px" CssClass="txtCurrency"
                                    Enabled="false" />
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
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="200px"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Filter Tanggal") %></label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblFilterDate" RepeatDirection="Horizontal">
                        <asp:ListItem Text="On" Value="true" />
                        <asp:ListItem Text="Off" Value="false" Selected="True" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trDate" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal") %>
                    </label>
                </td>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px;" />
                            <col style="width: 50px;" />
                            <col style="width: 150px;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtTransactionDateFrom" CssClass="datepicker" Width="120px" />
                            </td>
                            <td>
                                &nbsp;s/d&nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTransactionDateTo" CssClass="datepicker" Width="120px" />
                            </td>
                        </tr>
                    </table>
                    <%--<asp:TextBox ID="txtTransactionDate" Width="110px" CssClass="datepicker" runat="server" />--%>
                </td>
            </tr>
        </table>
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
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left" style="width: 150px">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No. Transaksi")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tgl. Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 250px">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Unit Pelayanan")%></div>
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px">
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
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
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
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left" style="width: 150px">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No. Transaksi")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tgl. Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 250px">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Unit Pelayanan")%></div>
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px">
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
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="2">
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
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px; float: right; margin-right: 50px; <%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' />
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblInfo">
                                                                            <%=GetLabel("Pending Recalculated") %></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("TransactionNo")%></a>
                                                            <div>
                                                                <%#: Eval("TransactionDateInString")%>
                                                                <%#: Eval("TransactionTime")%></div>
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%>
                                                                <br />
                                                                <%#: Eval("LastUpdatedByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px;">
                                                            <div>
                                                                <%#: Eval("Remarks")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPayerAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPatientAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center">
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
