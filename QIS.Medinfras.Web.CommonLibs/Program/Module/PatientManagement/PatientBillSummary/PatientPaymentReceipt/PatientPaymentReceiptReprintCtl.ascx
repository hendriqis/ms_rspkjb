<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientPaymentReceiptReprintCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPaymentReceiptReprintCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_patientpaymentreceiptReprintctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnReprintReceipt.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            var hdnIsDotMatrixAndOutpatientReprint = $('#<%=hdnIsDotMatrixAndOutpatientReprint.ClientID%>').val();
            if (hdnIsDotMatrixAndOutpatientReprint != "1") {
                cbpPatientPaymentReceiptReprint.PerformCallback('Reprint');
            }
            else {
                var deptID = $('#<%=hdnDepartmentID.ClientID%>').val();
                if (deptID != "INPATIENT") {
                    $('#<%=hdnParamReport.ClientID%>').val($('#<%=hdnPaymentReceiptID.ClientID%>').val());
                    cbpPatientPaymentReceiptReprintDotMatrix.PerformCallback();
                }
                else {
                    cbpPatientPaymentReceiptReprint.PerformCallback('Reprint');
                }
            }
        }
    });

    function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
        var receiptID = $('#<%=hdnPaymentReceiptID.ClientID%>').val();
        var registrationID = $('#<%=hdnRegistrationID.ClientID%>').val();
        if (receiptID == '') {
            errMessage.text = 'Cannot Print Payment Receipt';
            return false;
        }
        else {
            if (code == 'PM-00737') {
                filterExpression.text = "RegistrationID=" + registrationID + '|PaymentReceiptID=' + receiptID;
                return true;
            } else {
                filterExpression.text = receiptID;
                return true;
            }
            return true;
        }
    }

    function onCbpPatientPaymentReceiptReprintDotMatrixEndCallback(s) {
        hideLoadingPanel();
        pcRightPanelContent.Hide();

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

    function onReprintPatientPaymentReceiptSuccess() {
        var errMessage = { text: "" };
        var filterExpression = { text: "" };

        var hdnDepartmentID = $('#<%=hdnDepartmentID.ClientID%>').val();

        var hdnReportCodeReceipt = $('#<%=hdnReportCodeReceiptReprint.ClientID%>').val();
        var hdnReportCodeReceiptEnglish = $('#<%=hdnReportCodeReceiptEnglishReprint.ClientID%>').val();

        var hdnReportCodeReceiptOutpatient = $('#<%=hdnReportCodeReceiptOutpatientReprint.ClientID%>').val();
        var hdnReportCodeReceiptEnglishOutpatient = $('#<%=hdnReportCodeReceiptEnglishOutpatientReprint.ClientID%>').val();

        var hdnReportCodeReceiptMCU = $('#<%=hdnReportCodeReceiptMCU.ClientID%>').val();
        var hdnReportCodeReceiptEnglishMCU = $('#<%=hdnReportCodeReceiptEnglishMCU.ClientID%>').val();

        var hdnIsOutpatientAndDotMatrix = $('#<%=hdnIsDotMatrixAndOutpatientReprint.ClientID%>').val();

        var reportCode = "";

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
        
        if (reportCode != "") {
            var isAllowPrint = true;
            if (typeof onBeforeRightPanelPrint == 'function') {
                isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
            }

            if (isAllowPrint) {
               
                openReportViewer(reportCode, filterExpression.text);
            }
            else {
                showToast('Warning', errMessage.text);
            }
        }
        else {
            showToast('Warning', "ReportCode untuk cetakan kwitansi tidak ditemukan.");
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

    function onCboReprintReasonChanged() {
        if (cboReprintReason.GetValue() != 'X236^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnReprintReceipt" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnIsDotMatrixAndOutpatientReprint" runat="server" value="" />
        <input type="hidden" id="hdnPaymentReceiptID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptReprint" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishReprint" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptOutpatientReprint" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishOutpatientReprint" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptMCU" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishMCU" runat="server" value="" />
        <input type="hidden" id="hdnParamReport" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Reprint Reason")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboReprintReason" ClientInstanceName="cboReprintReason"
                        Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboReprintReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Reason")%></label>
                </td>
                <td>
                    <asp:textbox id="txtReason" width="200px" runat="server" />
                </td>
            </tr>
            <tr id="trReceiptOption" runat="server">
                <td class="tdLabel">
                    <div style="position: relative;">
                        <label class="lblNormal lblMandatory">
                            <%:GetLabel("Receipt Language")%></label></div>
                </td>
                <td colspan="2">
                    <asp:radiobuttonlist id="rblReceiptOption" runat="server" repeatdirection="Horizontal">
                        <asp:listitem text="B. Indonesia" value="0" selected="True" />
                        <asp:listitem text="B. Inggris" value="1" />
                    </asp:radiobuttonlist>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptReprint" runat="server" Width="100%"
            ClientInstanceName="cbpPatientPaymentReceiptReprint" ShowLoadingPanel="false"
            OnCallback="cbpPatientPaymentReceiptReprint_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptReprintDotMatrix" runat="server"
            Width="100%" ClientInstanceName="cbpPatientPaymentReceiptReprintDotMatrix" ShowLoadingPanel="false"
            OnCallback="cbpPatientPaymentReceiptReprintDotMatrix_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPatientPaymentReceiptReprintDotMatrixEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
