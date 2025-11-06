<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryPayReceiptDetailPrintCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayReceiptDetailPrintCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_patientpaymentreceiptdetailctl">
    $(function () {
        hideLoadingPanel();
        setDatePicker('<%=txtPaymentReceiptDate.ClientID %>');

        var hdnIsAllowBackDate = $('#<%=hdnIsAllowBackDate.ClientID%>').val();
        if (hdnIsAllowBackDate == '0') {
            $('#<%=txtPaymentReceiptDate.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtPaymentReceiptDate.ClientID %>').datepicker('disable');
            $('#<%=txtPaymentReceiptTime.ClientID %>').attr('readonly', 'readonly');
        }
        else {
            $('#<%=txtPaymentReceiptDate.ClientID %>').removeAttr('readonly');
            $('#<%:txtPaymentReceiptDate.ClientID %>').datepicker('enable');
            $('#<%=txtPaymentReceiptTime.ClientID %>').removeAttr('readonly');
        }
    });

    $('#<%=btnReceiptPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPatientPaymentReceiptProcess.PerformCallback('save');
        }
    });

    function onSavePatientPaymentReceiptSuccess(s) {

        var hdnIsPaymentReceiptPrintPreviewFirst = $('#<%=hdnIsPaymentReceiptPrintPreviewFirst.ClientID%>').val();

        var hdnDepartmentID = $('#<%=hdnDepartmentID.ClientID%>').val();

        var hdnReportCodeReceipt = $('#<%=hdnReportCodeReceipt.ClientID%>').val();
        var hdnReportCodeReceiptEnglish = $('#<%=hdnReportCodeReceiptEnglish.ClientID%>').val();

        var hdnReportCodeReceiptOutpatient = $('#<%=hdnReportCodeReceiptOutpatient.ClientID%>').val();
        var hdnReportCodeReceiptEnglishOutpatient = $('#<%=hdnReportCodeReceiptEnglishOutpatient.ClientID%>').val();

        var hdnReportCodeReceiptMCU = $('#<%=hdnReportCodeReceiptMCU.ClientID%>').val();
        var hdnReportCodeReceiptEnglishMCU = $('#<%=hdnReportCodeReceiptEnglishMCU.ClientID%>').val();

        var hdnIsOutpatientAndDotMatrix = $('#<%=hdnIsDotMatrixAndOutpatient.ClientID%>').val();

        var filterExpression = "";
        var reportCode = "";
        var value = s.cpResult;

        if (hdnIsPaymentReceiptPrintPreviewFirst == "1") {
            if (hdnIsOutpatientAndDotMatrix != "1") {
                if (hdnReportCodeReceipt != null && hdnReportCodeReceipt != "" && hdnReportCodeReceipt != "0") {
                    var displayOption = $('#<%=rblReceiptOption.ClientID %>').find(":checked").val();
                    if (displayOption == 0) {
                        if (hdnDepartmentID != Constant.Facility.INPATIENT && hdnDepartmentID != Constant.Facility.MCU) {
                            reportCode = hdnReportCodeReceiptOutpatient;
                        } else if (hdnDepartmentID == Constant.Facility.MCU) {
                            reportCode = hdnReportCodeReceiptMCU;
                        } else {
                            reportCode = hdnReportCodeReceipt;
                        }
                    }
                    else {
                        if (hdnDepartmentID != Constant.Facility.INPATIENT && hdnDepartmentID != Constant.Facility.MCU) {
                            reportCode = hdnReportCodeReceiptEnglishOutpatient;
                        } else if (hdnDepartmentID == Constant.Facility.MCU) {
                            reportCode = hdnReportCodeReceiptEnglishMCU;
                        } else {
                            reportCode = hdnReportCodeReceiptEnglish;
                        }
                    }
                }
                else {
                    reportCode = hdnReportCodeReceipt;
                }

                if (reportCode != '') {
                    var isAllowPrint = true;
                    filterExpression = s.cpRetval;

                    if (reportCode == "PM-00737") {
                        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                        var isAllowPrint = true;
                        filterExpression = "RegistrationID=" + registrationID + '|PaymentReceiptID=' + s.cpRetval;
                    }

                    openReportViewer(reportCode, filterExpression);
                }
                else {
                    showToast('Warning', "ReportCode untuk cetakan kwitansi tidak ditemukan.");
                }
            }
            else {
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
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReceiptPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnIsPaymentReceiptPrintPreviewFirst" runat="server" value="" />
        <input type="hidden" id="hdnIsDotMatrixAndOutpatient" runat="server" value="0" />
        <input type="hidden" id="hdnIsDotMatrixAndInpatien" runat="server" value="0" />
        <input type="hidden" id="hdnPaymentID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnPatientName" runat="server" value="" />
        <input type="hidden" id="hdnPaymentReceipt" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceipt" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglish" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptOutpatient" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishOutpatient" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptMCU" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishMCU" runat="server" value="" />
        <input type="hidden" id="hdnParam1" runat="server" value="" />
        <input type="hidden" id="hdnParam2" runat="server" value="" />
        <input type="hidden" id="hdnIsAllowBackDate" runat="server" value="" />
        <input type="hidden" id="hdnParamReport" runat="server" value="" />
        <input type="hidden" id="hdnIsCumulative" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 170px" />
                <col style="width: 400px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal") %>
                        -
                        <%=GetLabel("Jam") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtPaymentReceiptDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td style="width: 5px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentReceiptTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Cetak Atas Nama")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtReceiptName" Width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Catatan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtRemarks" Width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="chkIsMultiPayment" Checked="false" runat="server" /><%:GetLabel("Multi Payment")%>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="chkIsWithDiagnose" Checked="false" runat="server" /><%:GetLabel("Dengan Diagnosa")%>
                </td>
            </tr>
            <tr id="trReceiptOption" runat="server">
                <td class="tdLabel">
                    <div style="position: relative;">
                        <label class="lblNormal lblMandatory">
                            <%:GetLabel("Receipt Language")%></label></div>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rblReceiptOption" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="B. Indonesia" Value="0" Selected="True" />
                        <asp:ListItem Text="B. Inggris" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptProcess" runat="server" Width="100%"
            ClientInstanceName="cbpPatientPaymentReceiptProcess" ShowLoadingPanel="false"
            OnCallback="cbpPatientPaymentReceiptProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSavePatientPaymentReceiptSuccess(s);
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
