<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryVoidBill.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryVoidBill" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessTransactionVerification" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />

    <input type="hidden" value="" id="hdnIsBridgingToPaymentGateway" runat="server" />
    <input type="hidden" value="" id="hdnPaymenGatewayMethod" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
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

            $('#<%=btnProcessTransactionVerification.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                }
                else {
                    var param = '';
                    var lstBillID = "";
                    $('.chkIsSelected input:checked').each(function () {
                        var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                        if (param != '')
                            param += '|';
                        param += trxID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    var billID = param.replace("|", ",");
                    var lstBilling = "";

                    if ($('#<%=hdnIsBridgingToPaymentGateway.ClientID %>').val() == "1") {
                        if ($('#<%=hdnPaymenGatewayMethod.ClientID %>').val() == "X483^001") {
                            var lstChosen = "";
                            var filter = "ReferenceNo IN (SELECT a.ReferenceNo FROM PatientPaymentDtVirtual a WHERE a.PatientBillingID IN (" + billID + ") AND IsDeleted = 0) AND PatientBillingID NOT IN (" + billID + ")";
                            var filterChosen = "PatientBillingID IN (" + billID + ")";

                            Methods.getListObject('GetvPatientPaymentDtVirtualList', filterChosen, function (resultCh) {
                                if (resultCh.length > 0) {
                                    for (i = 0; i < resultCh.length; i++) {
                                        lstChosen += "<b>" + resultCh[i].PatientBillingNo + "(Reference Number : " + resultCh[i].ReferenceNo + ") </b> <br />";
                                    };

                                }
                            });

                            Methods.getListObject('GetvPatientPaymentDtVirtualList', filter, function (result) {
                                if (result.length > 0) {
                                    for (i = 0; i < result.length; i++) {
                                        lstBilling += "<b>" + result[i].PatientBillingNo + "(Reference Number : " + result[i].ReferenceNo + ") </b> <br />";
                                        lstBillID += "|" + result[i].PatientBillingID;
                                    };

                                    displayConfirmationMessageBox("CONFIRMATION", "Nomor Tagihan yang dipilih : <br /> " + lstChosen + " <br/> Nomor tagihan dibawah ini sudah pernah dikirim ke payment gateway secara bersamaan dengan Reference Number yang sama. <br />" + lstBilling + " Membatalkan semua tagihan yang bersangkutan?", function (answer) {
                                        if (answer) {
                                            param = param + lstBillID;
                                            $('#<%=hdnParam.ClientID %>').val(param);
                                            onCustomButtonClick('voidbilling');
                                        }
                                    });
                                }
                                else {
                                    Methods.getListObject('GetPatientBillList', "PatientBillingID IN (" + billID + ")", function (resultBill) {
                                        if (resultBill.length > 0) {
                                            for (i = 0; i < resultBill.length; i++) {
                                                lstBilling += "<b>" + resultBill[i].PatientBillingNo + "</b> <br />";
                                            };
                                        };
                                    });

                                    displayConfirmationMessageBox("CONFIRMATION", "Nomor Tagihan yang dipilih : <br /> " + lstBilling + " <br/> Membatalkan semua tagihan yang bersangkutan?", function (answer) {
                                        if (answer) {
                                            onCustomButtonClick('voidbilling');
                                        }
                                    });
                                }
                            });
                        }
                        else {
                            Methods.getListObject('GetPatientBillList', "PatientBillingID IN (" + billID + ")", function (resultBill) {
                                if (resultBill.length > 0) {
                                    for (i = 0; i < resultBill.length; i++) {
                                        lstBilling += "<b>" + resultBill[i].PatientBillingNo + "</b> <br />";
                                    };
                                };
                            });

                            displayConfirmationMessageBox("CONFIRMATION", "Nomor Tagihan yang dipilih : <br /> " + lstBilling + " <br/> Membatalkan semua tagihan yang bersangkutan?", function (answer) {
                                if (answer) {
                                    onCustomButtonClick('voidbilling');
                                }
                            });
                        }
                    }
                    else {
                        Methods.getListObject('GetPatientBillList', "PatientBillingID IN (" + billID + ")", function (resultBill) {
                            if (resultBill.length > 0) {
                                for (i = 0; i < resultBill.length; i++) {
                                    lstBilling += "<b>" + resultBill[i].PatientBillingNo + "</b> <br />";
                                };
                            };
                        });

                        displayConfirmationMessageBox("CONFIRMATION", "Nomor Tagihan yang dipilih : <br /> " + lstBilling + " <br/> Membatalkan semua tagihan yang bersangkutan?", function (answer) {
                            if (answer) {
                                onCustomButtonClick('voidbilling');
                            }
                        });
                    }
                }
            });

            $('.lnkTransactionNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscount/PatientBillSummaryDiscountDtCtl.ascx");
                openUserControlPopup(url, id, 'Pembatalan Tagihan Pasien Item', 1100, 500);
            });
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
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

        function onCboVoidReasonValueChanged(s) {
            if (s.GetValue() == 'X129^999')
                $('#<%=txtVoidReason.ClientID %>').show();
            else
                $('#<%=txtVoidReason.ClientID %>').hide();
        }
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Alasan Pembatalan Tagihan Pasien") %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="200px">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoidReason" runat="server" Width="200px" Style="display: none" />
                            </td>
                        </tr>
                    </table>
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
                                                                    <%= GetLabel("No Bill Pasien")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal Bill")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
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
                                                                    <%= GetLabel("No Bill Pasien")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal Bill")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
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
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 3px" id="tdTotalAllPatient">
                                                                <%=GetLabel("Pasien")%>
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
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("PatientBillingNo")%></a>
                                                            <div>
                                                                <%#: Eval("BillingDateInString")%>
                                                                <%#: Eval("BillingTime")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div>
                                                                <%#: Eval("PayerRemainingAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div>
                                                                <%#: Eval("PatientRemainingAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div>
                                                                <%#: Eval("RemainingAmount", "{0:N}")%></div>
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
