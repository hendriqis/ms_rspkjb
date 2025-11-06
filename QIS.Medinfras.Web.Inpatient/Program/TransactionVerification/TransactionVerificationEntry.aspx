<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master" AutoEventWireup="true" 
    CodeBehind="TransactionVerificationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
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
        });

        $('.lnkTransactionNo').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var transactionCode = $tr.find('.hdnTransactionCode').val();
            var prescriptionOrderID = parseInt($tr.find('.hdnPrescriptionOrderID').val());
            var prescriptionReturnOrderID = parseInt($tr.find('.hdnPrescriptionReturnOrderID').val());
            var url = '';
            if (prescriptionOrderID > 0) {
                id += '|' + prescriptionOrderID;
                url = ResolveUrl("~/Program/TransactionVerification/TransactionVerificationDtPrescriptionCtl.ascx");
            }
            else if (prescriptionReturnOrderID > 0) {
                id += '|' + prescriptionReturnOrderID;
                url = ResolveUrl("~/Program/TransactionVerification/TransactionVerificationDtPrescriptionReturnCtl.ascx");
            }
            else if (transactionCode == '<%=laboratoryTransactionCode %>')
                url = ResolveUrl("~/Program/TransactionVerification/TransactionVerificationDtLaboratoryCtl.ascx");
            else
                url = ResolveUrl("~/Program/TransactionVerification/TransactionVerificationDtCtl.ascx");
            openUserControlPopup(url, id, 'Verifikasi Transaksi', 1100, 500);
        });

        $('.txtAdministrationFee').live('change', function () {
            var fee = parseFloat($(this).val());
            var max = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnAdministrationFee.ClientID %>').attr('minamount'));
            if (fee < min)
                fee = min;
            if (fee > max)
                fee = max;
            $('#<%=hdnAdministrationFeeAmount.ClientID %>').val(fee);
            $(this).val(fee).trigger('changeValue');

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
        });

        $('.txtServiceFee').live('change', function () {
            var fee = parseFloat($(this).val());
            var max = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('maxamount'));
            var min = parseFloat($('#<%=hdnServiceFee.ClientID %>').attr('minamount'));
            if (fee < min)
                fee = min;
            if (fee > max)
                fee = max;
            $('#<%=hdnServiceFeeAmount.ClientID %>').val(fee);
            $(this).val(fee).trigger('changeValue');

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
        }

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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            //filterExpression.text = 'RegistrationID = ' + registrationID;
            
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            filterExpression.text = 'RegistrationID = ' + registrationID;
            if (linkedRegisID != "" && linkedRegisID != "0") {
                filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
            }
            return true;
        }

        function onCboDisplayChanged() {
            cbpView.PerformCallback();
        }

        function onCboBillingStatusChanged() {
            cbpView.PerformCallback();
        } 

        function onCboServiceUnitChanged() {
            cbpView.PerformCallback();
        }
    </script> 
    <style type="text/css">
        .trFooter td        { height: 10px }
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
        <table id="tblInfoOutstandingTransfer" runat="server">
            <tr>
                <td><img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' /></td>
                <td><label class="lblInfo" id="lblInfoOutstandingBill"><%=GetLabel("Masih Ada Tagihan Yang Belum Ditransfer") %></label></td>
            </tr>
        </table>
        <table class="tblEntryContent">
            <colgroup>
                <col style="width:120px"/>
                <col/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Verifikasi")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" Width="200px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDisplayChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Status Tagihan")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboBillingStatus" ClientInstanceName="cboBillingStatus" Width="200px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboBillingStatusChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit Pelayanan")%></label></td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="200px" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
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
                                        <asp:ListView ID="lvwView" runat="server">
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
                                                                <div><%= GetLabel("No Bukti")%></div>
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
                                                        <td colspan="5">
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
                                                                <div><%= GetLabel("No Bukti")%></div>
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
                                                                <%=GetLabel("Biaya Administrasi Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtAdministrationFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtAdministrationFee" />
                                                            </div>
                                                        </td>
                                                        <td colspan="4"></td>
                                                    </tr>
                                                    <tr class="trFooter" id="trFooterServiceFee" runat="server">  
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <%=GetLabel("Biaya Service Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align:right;padding:0px 3px">
                                                                <asp:TextBox ID="txtServiceFee" Text="0" runat="server" Width="100%" CssClass="txtCurrency txtServiceFee" />
                                                            </div>
                                                        </td>
                                                        <td colspan="4"></td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div style="padding:3px">
                                                            <asp:CheckBox ID="chkSelectAllVerificationVerification" CssClass="chkSelectAllVerificationVerification" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
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
                                                            <div><%#: Eval("TransactionDateInString")%> <%#: Eval("TransactionTime")%></div>                                                    
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