<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientPaymentReceipt.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPaymentReceipt" %>

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
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
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
            openSetpar();
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
                    var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                    var paymentAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                    var patientName = $('#<%=hdnPatientName.ClientID %>').val();
                    var paymentID = $('#<%=hdnParam.ClientID %>').val();
                    var id = registrationID + '|' + departmentID + '|' + paymentAmount + '|' + patientName + '|0;' + paymentID;
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

        });
        function openSetpar() {
            //setpar  FN0206 
            var FN0206 = $('#<%=hdnFN0206.ClientID %>').val();
            var deptID = $('#<%=hdnDepartmentID.ClientID %>').val();

            if (FN0206 == "1") {
                if (deptID != "INPATIENT") {
                    $('.imgPrintLampiran').show();
                } else {
                    $('.imgPrintLampiran').hide();
                }
            } else {
                $('.imgPrintLampiran').hide();
            }
        }

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
        $('.imgPrintLampiran.imgLink').die('click');
        $('.imgPrintLampiran.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);
            showLoadingPanel();
            var receiptID = $(this).closest('tr').find('.hdnPaymentReceiptID').val();
            $('#<%=hdnPaymentReceiptIDVal.ClientID %>').val(receiptID);
            cbpView2.PerformCallback('printKwitansiPaymentDetail');
            pcRightPanelContent.Hide();

        });

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

            showLoadingPanel();
            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientPaymentReceiptReprintCtl.ascx');
            var receiptID = $('.hdnPaymentReceiptID').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = registrationID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
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
            var id = registrationID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
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
            var id = registrationID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            var businesspartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();

            if (code == 'PM-90008' || code == 'PM-90009' || code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014' || code == 'PM-90015'
                || code == 'PM-90016' || code == 'PM-90017' || code == 'PM-00572' || code == 'PM-90019' || code == 'PM-90020' || code == 'PM-00565' || code == 'PM-90022' || code == 'PM-90023') {
                filterExpression.text = visitID;
                return true;
            }

            if (hdnID == '' || hdnID == '0') {
                if (code == 'PM-00425' || code == 'PM-00160' || code == 'PM-00524' || code == 'PM-00583' || code == 'PM-00585' || code == 'PM-00586' || code == 'PM-00641' || code == 'PM-00645'
                    || code == 'PM-00642' || code == 'PM-00644' || code == 'PM-00565' || code == 'PM-00650' || code == 'PM-00654' || code == 'PM-00184'
                    || code == 'MR000016') {
                    filterExpression.text = registrationID;
                    return true;
                }
                else if (code == 'MR000013' || code == 'MR000030' || code == 'MR000017' || code == 'PM-00618') {
                    filterExpression.text = visitID;
                    return true;
                }
                else if (code == 'MR000028') {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
                else {
                    errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                    return false;
                }
            }
            else if (code == 'MR000013' || code == 'MR000014' || code == 'MR000017' || code == 'MR000019' || code == 'MR000024' || code == 'MR000032' || code == 'PM-00618' || code == 'PM-00565') {
                filterExpression.text = visitID;
                return true;
            }
            else if (code == 'PM-00425' || code == 'MR000016' || code == 'MR000030' || code == 'PM-00522' || code == 'PM-00523' || code == 'PM-00159' || code == 'PM-00564'
                     || code == 'PM-00160' || code == 'PM-00524' || code == 'PM-00583' || code == 'PM-00585' || code == 'PM-00586' || code == 'PM-00546' || code == 'PM-00642' || code == 'PM-00565' || code == 'PM-00644'
                     || code == 'PM-00570' || code == 'PM-00645' || code == 'PM-00654' || code == 'PM-00184' || code == 'PM-00650') {
                filterExpression.text = registrationID;
                return true;
            }
            else if (code == 'PM-00571' || code == 'PM-00583' || code == 'PM-00585' || code == 'PM-00586') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            }
            else if (code == 'PM-00178') {
                if (hdnID == '' || hdnID == '0') {
                    if (businesspartnerID == '2') {
                        filterExpression.text = registrationID;
                        return true;
                    }
                }
                else {
                    errMessage.text = 'Bukan Pasien BPJS';
                    return false;
                }
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }

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
        function onCbpViewHdEndCallback(s) {
            openSetpar();
            var param = s.cpResult.split('|');
            if (param[0] == "printKwitansiPaymentDetail") {
                var receiptID = $('#<%=hdnPaymentReceiptIDVal.ClientID %>').val();
                 
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var filterExpression = ""; 
                var reportCode = $('#<%=hdnFN0209.ClientID %>').val(); //// "PM-00737";
                if (reportCode != '') {
                    var isAllowPrint = true;
                    if (reportCode == "PM-00737") {
                        filterExpression = "RegistrationID=" + registrationID + '|PaymentReceiptID=' + receiptID;
                        openReportViewer(reportCode, filterExpression);
                    } else {
                        filterExpression = receiptID;
                        openReportViewer(reportCode, filterExpression);
                    }
                   

                }
                cbpView2.PerformCallback('refresh');
            }
            hideLoadingPanel(s);
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnStatusKwitansi" runat="server" />
    <input type="hidden" value="0" id="hdnFN0206" runat="server" />
    <input type="hidden" value="0" id="hdnPaymentReceiptIDVal" runat="server" />
    <input type="hidden" value="" id="hdnFN0209" runat="server" />
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
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewHdEndCallback(s); }" />
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
                                                                    <%= GetLabel("No. Kwitansi")%></div>
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
                                                                    <%= GetLabel("No. Kwitansi")%></div>
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
                                                            <span class="lnkPaymentReceipt <%#  Eval("IsDeleted").ToString() == "True" ? "lblNormal" : "lblLink"%>"
                                                                id="lnkPaymentReceipt">
                                                                <%#: Eval("PaymentReceiptNo")%></span>
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
                                                   <td style="width:100px">
                                                        <div style="text-align: center">
                                                            <img class="imgPrint <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Cetak Kwitansi")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                            <img style="display:none;" class="imgPrintLampiran <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Cetak Lampiran Kwitansi")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
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
                                                        <td colspan="6">
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
