<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PaymentReceiptReprintDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentReceiptReprintDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPaymentReceiptReprint" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnPaymentReceiptReprint.ClientID %>').live('click', function () {
            showLoadingPanel();
            var reqID = $('#<%=hdnRequestID.ClientID %>').val();
            var url = "~/Libs/Program/Module/PatientManagement/PatientBillDetail/PaymentReceiptReprintList.aspx?id=" + reqID;
            document.location = ResolveUrl(url);
        });

        $('.lnkPaymentReceipt.lblLink').die('click');
        $('.lnkPaymentReceipt.lblLink').live('click', function () {
            var id = $(this).closest('tr').find('.hdnPaymentReceiptID').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptDtCtl.ascx");
            openUserControlPopup(url, id, 'Patient Receipt Detail', 1100, 500);
        });

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

            var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientPaymentReceiptReprintCtl.ascx');
            var receiptID = $('.hdnPaymentReceiptID').val();
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var id = registrationID + '|' + obj.hdnPaymentReceiptID + '|' + departmentID;
            openUserControlPopup(url, id, 'Reprint Receipt', 400, 230);
        });

        $('.imgPrintLegalized.imgLink').die('click');
        $('.imgPrintLegalized.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var obj = rowToObject($row);

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
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%= GetLabel("Daftar Kwitansi")%></div>
        </div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; height: 250px; overflow-y: scroll">
                        <dxcp:ASPxCallbackPanel ID="cbpViewReceipt" runat="server" Width="100%" ClientInstanceName="cbpViewReceipt"
                            ShowLoadingPanel="false" OnCallback="cbpViewReceipt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
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
                                                        <th align="center" style="width: 80px; display: none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Lampiran")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display: none">
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
                                                        <th align="center" style="width: 80px; display: none">
                                                            <div style="text-align: center">
                                                                <%=GetLabel("Lampiran")%>
                                                            </div>
                                                        </th>
                                                        <th align="center" style="width: 80px; display: none">
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
                                                    <td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrint <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Print")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrintLegalized <%#  Eval("IsDeleted").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%=GetLabel("Print")%>' src='<%# Eval("IsDeleted").ToString() == "True" ? ResolveUrl("~/Libs/Images/Icon/tbprint.png") : ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" /></div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div style="padding: 5px; height: 150px; display: none">
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
