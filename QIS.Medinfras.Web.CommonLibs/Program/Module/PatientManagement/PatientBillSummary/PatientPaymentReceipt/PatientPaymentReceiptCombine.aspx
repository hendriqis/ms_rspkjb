<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientPaymentReceiptCombine.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPaymentReceiptCombine" %>

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
    <li id="btnProcessPaymentReceipt" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationIDCurrent" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnPaymentID" runat="server" />
    <input type="hidden" value="" id="hdnPaymentAmount" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            onLoadPaymentReceiptPrint();

            $('#<%=btnProcessPaymentReceipt.ClientID %>').click(function () {
                showLoadingPanel();
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Payment First');
                    hideLoadingPanel();
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var paymentID = $(this).closest('tr').find('.hdnKeyField').val();
                        if (param != '')
                            param += '|';
                        param += paymentID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);

                    var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptDetailPrintCtl.ascx');
                    var registrationID = $('#<%=hdnRegistrationIDCurrent.ClientID %>').val();
                    var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                    var paymentAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                    var patientName = $('#<%=hdnPatientName.ClientID %>').val();
                    var paymentID = $('#<%=hdnParam.ClientID %>').val();
                    var id = registrationID + '|' + departmentID + '|' + paymentAmount + '|' + patientName + '|1;' + paymentID;
                    openUserControlPopup(url, id, 'Print Receipt', 520, 250);
                }
            });

            var isChecked = true;
            $('.chkSelectAll').find('input').prop('checked', isChecked);
            $('.chkIsSelected').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
            calculateTotal();

            $('.lnkPaymentNo.lblLink').die('click');
            $('.lnkPaymentNo.lblLink').live('click', function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayDetailCtl.ascx");
                openUserControlPopup(url, id, 'Patient Payment', 1100, 500);
            });

            $('.lnkPaymentReceipt.lblLink').die('click');
            $('.lnkPaymentReceipt.lblLink').live('click', function () {
                var id = $(this).closest('tr').find('.hdnPaymentReceiptID').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptDtCtl.ascx");
                openUserControlPopup(url, id, 'Patient Receipt Detail', 1100, 500);
            });

            //#region filter Registration
            function GetRegFilterExpression() {
                var mrn = $('#<%=hdnMRN.ClientID %>').val();
                var filterExpression = "MRN = '" + mrn + "' AND GCRegistrationStatus NOT IN ('X020^006')";
                return filterExpression;
            }

            $('#<%:lblNoReg.ClientID %>.lblLink').click(function () {
                openSearchDialog('registration', GetRegFilterExpression(), function (value) {
                    $('#<%=txtRegNoFilter.ClientID %>').val(value);
                    onTxtRegNoFilterChanged(value);
                });
            });

            $('#<%=txtRegNoFilter.ClientID %>').change(function () {
                onTxtRegNoFilterChanged($(this).val());
            });

            function onTxtRegNoFilterChanged(value) {
                var filterExpression = GetRegFilterExpression() + " AND RegistrationNo = '" + value + "'";
                Methods.getObject('GetRegistrationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                        $('#<%=txtRegNoFilter.ClientID %>').val(result.RegistrationNo);
                    }
                    else {
                        $('#<%=hdnRegistrationID.ClientID %>').val('');
                        $('#<%=txtRegNoFilter.ClientID %>').val('');
                    }
                });

                cbpView2.PerformCallback();
            }
            //#endregion
        });

        function onLoadPaymentReceiptPrint() {
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

        function calculateTotal() {
            var total = 0;
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                total += parseFloat($tr.find('.hdnPaymentAmount').val());
            });

            $('#<%=hdnTotalAmount.ClientID %>').val(total);
            $('#<%=txtTotalKwitansi.ClientID %>').val(total.formatMoney(2, '.', ','));
        }

        function onCbpViewEndCallback(s) {
            onLoadPaymentReceiptPrint();
            hideLoadingPanel();
            cbpView2.PerformCallback();
        }

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

            showLoadingPanel();
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientPaymentReceiptReprintCtl.ascx');
            var receiptID = $('.hdnPaymentReceiptID').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = obj.hdnRegID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
            openUserControlPopup(url, id, 'Reprint Receipt', 400, 230);
        });

        $('.imgVoid.imgLink').die('click');
        $('.imgVoid.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

            showLoadingPanel();
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptVoidCtl.ascx');
            var receiptID = $('.hdnPaymentReceiptID').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = obj.hdnRegID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
            openUserControlPopup(url, id, 'Void Receipt', 400, 230);
        });

        $('.imgPrintLegalized.imgLink').die('click');
        $('.imgPrintLegalized.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

            showLoadingPanel();
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientPaymentReceiptPrintLegalizedCtl.ascx');
            var receiptID = $('.hdnPaymentReceiptID').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = obj.hdnRegID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
            openUserControlPopup(url, id, 'Print Legalized Receipt', 400, 230);
        });

        $('.imgPrintAddRekap.imgLink').die('click');
        $('.imgPrintAddRekap.imgLink').live('click', function () {
            var reportCode = "PM-00237";
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var filterExpression = { text: "" };

            filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';

            openReportViewer(reportCode, filterExpression.text);
        });

        $('.imgPrintAddDetail.imgLink').die('click');
        $('.imgPrintAddDetail.imgLink').live('click', function () {
            var reportCode = "PM-00238";
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var filterExpression = { text: "" };

            filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';

            openReportViewer(reportCode, filterExpression.text);
        });

        $('.imgPrintCopyKwitansi.imgLink').die('click');
        $('.imgPrintCopyKwitansi.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var id = obj.hdnPaymentReceiptID;
            $('#<%=hdnPrintCopy.ClientID %>').val(id);
            cbpViewPrint.PerformCallback('copyKwitansi');
        });

        $('.imgPrintCopyTransaksi.imgLink').die('click');
        $('.imgPrintCopyTransaksi.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var id = obj.hdnPaymentReceiptID;
            $('#<%=hdnPrintCopy.ClientID %>').val(id);
            cbpViewPrint.PerformCallback('copyTransaksi');
        });

        $('.imgPrintKwitansiHaloDoc.imgLink').die('click');
        $('.imgPrintKwitansiHaloDoc.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            var id = obj.hdnPaymentReceiptID;
            $('#<%=hdnPrintCopy.ClientID %>').val(id);
            cbpViewPrint.PerformCallback('halodocreceipt');
        });

        function onCbpViewPrintEndCallback(s) {
            hideLoadingPanel();
            var value = s.cpResult;
            var e_id = 'id_' + new Date().getTime();
            if (window.chrome) {
                $('body').append('<a id=\"' + e_id + '\"></a>');
                $('#' + e_id).attr('href', 'PDClient:' + value);
                var a = $('a#' + e_id)[0];
                var evObj = document.createEvent('MouseEvents');
                evObj.initEvent('click', true, true);
                a.dispatchEvent(evObj)
            } else {
                $('body').append('<iframe name=\"' + e_id + '\" id=\"' + e_id + '\" width=\"1\" height=\"1\" style=\"visibility:hidden;position:absolute\" />');
                $('#' + e_id).attr('src', 'PDClient:' + value)
            }
            setTimeout(function () {
                $('#' + e_id).remove()
            }, 5000)
        }
    </script>
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnStatusKwitansi" runat="server" />
    <div>
        <table>
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 60%" />
            </colgroup>
            <tr>
                <td>
                    <label class="lblLink" id="lblNoReg" runat="server">
                        <%=GetLabel("Filter Nomor Registrasi")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRegNoFilter" Width="250px" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%= GetLabel("Daftar Kwitansi")%></div>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; height: 150px; overflow-y:scroll">
                        <dxcp:ASPxCallbackPanel ID="cbpView2" runat="server" Width="100%" ClientInstanceName="cbpView2"
                            ShowLoadingPanel="false" OnCallback="cbpView2_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView_receipt" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView_receipt" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 100px">
                                                                <div>
                                                                    <%= GetLabel("No. Kwitansi")%><BR>
                                                                    <%= GetLabel("No. Registrasi")%>
                                                                    </div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <div style="text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Tanggal & Jam")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 150px">
                                                                <div>
                                                                    <%= GetLabel("Atas Nama")%></div>
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align: right; width: 100px">
                                                                <%=GetLabel("Nilai Kwitansi")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 50px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Jumlah Cetak")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Terakhir Cetak")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 150px">
                                                                <div>
                                                                    <%= GetLabel("Status Kwitansi")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Proses")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 50px">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Legalisir")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display:none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Lampiran")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display:none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Copy")%>
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
                                                <table id="tblView_receipt" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 100px">
                                                                <div>
                                                                    <%= GetLabel("No. Kwitansi")%><BR>
                                                                    <%= GetLabel("No. Registrasi")%>
                                                                    </div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <div style="text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Tanggal & Jam")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 150px">
                                                                <div>
                                                                    <%= GetLabel("Atas Nama")%></div>
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align: right; width: 100px">
                                                                <%=GetLabel("Nilai Kwitansi")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 50px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Jumlah Cetak")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Terakhir Cetak")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 150px">
                                                                <div>
                                                                    <%= GetLabel("Status Kwitansi")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Proses")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 50px">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Legalisir")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display:none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Lampiran")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display:none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Copy")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnPaymentReceiptID" value='<%#: Eval("PaymentReceiptID") %>'
                                                                bindingfield="hdnPaymentReceiptID" />
                                                            <input type="hidden" class="hdnRegID" value='<%#: Eval("RegistrationID") %>'
                                                                bindingfield="hdnRegID" />
                                                            <span class="lnkPaymentReceipt <%#  Eval("IsDeleted").ToString() == "True" ? "lblNormal" : "lblLink"%>"
                                                                id="lnkPaymentReceipt">
                                                                <%#: Eval("PaymentReceiptNo")%></span><BR>
                                                                <%#: Eval("RegistrationNo")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center;">
                                                            <div>
                                                                <%#: Eval("ReceiptDateInString")%>
                                                                <br />
                                                                <%#: Eval("ReceiptTime")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <div>
                                                                <%#: Eval("PrintAsName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnReceiptAmount" value='<%#: Eval("ReceiptAmount")%>' />
                                                            <div>
                                                                <%#: Eval("ReceiptAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <div>
                                                                <%#: Eval("CreatedByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <div>
                                                                <%#: Eval("PrintNumber")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <div>
                                                                <%#: Eval("LastPrintedDateInString")%>
                                                                <br />
                                                                <%#: Eval("LastPrintedTime")%>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: left;">
                                                            <div>
                                                                <%#: Eval("StatusKwitansi")%>
                                                                <br />
                                                                <%#: Eval("DetailStatusKwitansi")%>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrint <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Print")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                            <img class="imgVoid <%# Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Void")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrintLegalized <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Print")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                    </td>
                                                    <%--<td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrintAddRekap <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Lampiran Rekap")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                            <img class="imgPrintAddDetail <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Lampiran Detail")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                    </td>--%>
                                                    <%--<td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrintCopyKwitansi <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Copy Kwitansi")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                            <img class="imgPrintCopyTransaksi <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Copy Transaksi")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                            <img class="imgPrintKwitansiHaloDoc <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Halodoc Receipt")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                    </td>--%>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%= GetLabel("Daftar Transaksi Pembayaran")%></div>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <table class="tblContentArea" style="width: 100%">
                        <colgroup>
                            <col width="8%" />
                            <col width="14%" />
                            <col width="50%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Kwitansi")%></label>
                            </td>
                            <td id="tdTotalKwitansi">
                                <asp:TextBox ID="txtTotalKwitansi" Width="100%" CssClass="txtCurrency" ReadOnly="true"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding: 5px; height: 150px; overflow-y:scroll"">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView_payment" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView_payment" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 100px">
                                                                <div>
                                                                    <%= GetLabel("No. Pembayaran")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 80px">
                                                                <div>
                                                                    <%= GetLabel("No. Registrasi")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 200px">
                                                            <div style="text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Tanggal & Jam Bayar")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left">
                                                                <div>
                                                                    <%= GetLabel("Tipe Bayar")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 150px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Total Bayar")%>
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
                                                <table id="tblView_payment" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 100px">
                                                                <div>
                                                                    <%= GetLabel("No. Pembayaran")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left; width: 80px">
                                                                <div>
                                                                    <%= GetLabel("No. Registrasi")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 200px">
                                                            <div style="text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Tanggal & Jam Bayar")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding: 3px; text-align: left">
                                                                <div>
                                                                    <%= GetLabel("Tipe Bayar")%></div>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 150px">
                                                            <div style="padding: 3px; text-align: center">
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align: right">
                                                                <%=GetLabel("Total Bayar")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PaymentID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <span class="lnkPaymentNo lblLink" id="lnkPaymentNo">
                                                                <%#: Eval("PaymentNo")%></span>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <span class="lblNormal" id="lnkRegistrationNo">
                                                                <%#: Eval("RegistrationNo")%></span>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center;">
                                                            <div>
                                                                <%#: Eval("PaymentDateInString")%>
                                                                ||
                                                                <%#: Eval("PaymentTime")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <div>
                                                                <%#: Eval("PaymentType")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center">
                                                            <div>
                                                                <%#: Eval("CreatedByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPaymentAmount" value='<%#: Eval("ReceiptAmount")%>' />
                                                            <div>
                                                                <%#: Eval("ReceiptAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div style="padding: 5px; height: 150px; overflow-y:scroll"">
                        <dxcp:ASPxCallbackPanel ID="cbpViewPrint" runat="server" Width="100%" ClientInstanceName="cbpViewPrint"
                            ShowLoadingPanel="false" OnCallback="cbpViewPrint_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewPrintEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                    <asp:Panel runat="server" ID="Panel3" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                            <input type="hidden" value="" id="hdnPrintCopy" runat="server" />
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
