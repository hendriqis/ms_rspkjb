<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangePatientTransactionStatusEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangePatientTransactionStatusEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangeTransactionStatusBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnChangeTransactionStatus" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=GetLabel("Perubahan Status Transaksi Pasien")%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            onLoadGenerateBill();

            $('#<%=btnChangeTransactionStatusBack.ClientID %>').click(function () {
                showLoadingPanel();
                if ($('#<%=hdnDepartmentID.ClientID %>').val() == 'INPATIENT') {
                    document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=ct');
                }
                else {
                    document.location = ResolveUrl('~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusList.aspx');
                }
            });
        });

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
            else if (transactionCode == '<%=laboratoryTransactionCode %>')
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtLaboratoryCtl.ascx");
            else
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Item', 1100, 500);
        });

        $('#<%=btnChangeTransactionStatus.ClientID %>').live('click', function (evt) {
            if ($('.chkIsSelected input:checked').length < 1) {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
            else {
                var param = '';
                $('.chkIsSelected input:checked').each(function () {
                    var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                    if (param != '')
                        param += ',';
                    param += trxID;
                });
                $('#<%=hdnParam.ClientID %>').val(param);

                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusReopenCtl.ascx');
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var transactionID = $('#<%=hdnParam.ClientID %>').val();
                var id = registrationID + '|' + transactionID;
                openUserControlPopup(url, id, 'Reopen Transaction', 400, 230);
            }
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
        }

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            onLoadGenerateBill();
            hideLoadingPanel();
        }

        function calculateTotal() {
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

            $('#tdTotalAllPayer').html(payerAmount.formatMoney(2, '.', ','));
            $('#tdTotalAllPatient').html(patientAmount.formatMoney(2, '.', ','));
            $('#tdTotalAll').html(lineAmount.formatMoney(2, '.', ','));
        }

        function onRefreshGrdReg() {
            cbpView.PerformCallback();
        }

        //#region Department
        function onCboDepartmentChanged() {
            $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
            $('#<%=hdnCboDepartmentID.ClientID %>').val(cboDepartment.GetValue());
            onRefreshGrdReg();
        }
        //#endregion

        //#region Service Unit
        function getHealthcareServiceUnitOrderFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "'"
                                    + " AND DepartmentID = '" + cboDepartment.GetValue() + "'"
                                    + " AND " + $('#<%=hdnFilterServiceUnitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblServiceUnitOrder.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitOrderFilterExpression(), function (value) {
                $('#<%=txtServiceUnitOrderCode.ClientID %>').val(value);
                onTxtServiceUnitOrderCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitOrderCode.ClientID %>').live('change', function () {
            onTxtServiceUnitOrderCodeChanged($(this).val());
        });

        function onTxtServiceUnitOrderCodeChanged(value) {
            var filterExpression = getHealthcareServiceUnitOrderFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitOrderID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitOrderCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitOrderName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
                    $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
                }
                onRefreshGrdReg();
            });
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 100px" />
                <col style="width: 150px" />
            </colgroup>
            <tr id="trDepartment" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Department")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                        Width="350px">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblNormal" id="lblServiceUnitOrder">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <input type="hidden" id="hdnServiceUnitOrderID" value="" runat="server" />
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderCode" Width="100%" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderName" ReadOnly="true" Width="250%" runat="server" />
                </td>
                <td>
                    &nbsp;
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
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date/Time")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 200px;">
                                                                <div>
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                                <div>
                                                                    <%= GetLabel("LastUpdated By")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Amount")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Payer")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Patient")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
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
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date/Time")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 200px;">
                                                                <div>
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                                <div>
                                                                    <%= GetLabel("LastUpdated By")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Amount")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Payer")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Patient")%>
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
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdTotalAllPayer">
                                                                <%=GetLabel("Payer")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdTotalAllPatient">
                                                                <%=GetLabel("Patient")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdTotalAll">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                        </div>
                                                    </td>
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
                                                                <%#: Eval("cfTransactionDateTimeInString")%></div>
                                                        </div>
                                                        <div style="padding: 3px; margin-left: 200px;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                            <div>
                                                                <i><%=GetLabel("CreatedBy : ") %></i><%#: Eval("CreatedByName")%></div>
                                                            <div>
                                                                <i><%=GetLabel("LastUpdatedBy : ") %></i><%#: Eval("LastUpdatedByName")%></div>
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
