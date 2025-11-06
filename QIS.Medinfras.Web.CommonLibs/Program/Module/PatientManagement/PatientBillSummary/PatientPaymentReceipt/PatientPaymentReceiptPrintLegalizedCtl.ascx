<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientPaymentReceiptPrintLegalizedCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPaymentReceiptPrintLegalizedCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientPaymentReceiptPrintLegalizedCtl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrintLegalized.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            var hdnIsDotMatrixAndOutpatientLegalized = $('#<%=hdnIsDotMatrixAndOutpatientLegalized.ClientID%>').val();
            if (hdnIsDotMatrixAndOutpatientLegalized != "1") {
                cbpPatientPaymentReceiptLegalized.PerformCallback('Legalized');
            }
            else {
                $('#<%=hdnParamReport.ClientID%>').val($('#<%=hdnPaymentReceiptIDLegalized.ClientID%>').val());
                cbpPatientPaymentReceiptLegalizedDotMatrix.PerformCallback();
            }
        }
    });

    function oncbpPatientPaymentReceiptLegalizedDotMatrixEndCallback(s) {
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

    function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
        var receiptID = $('#<%=hdnPaymentReceiptIDLegalized.ClientID%>').val();

        if (receiptID == '') {
            errMessage.text = 'Cannot Print Payment Receipt';
            return false;
        }
        else {
            filterExpression.text = receiptID;
            return true;
        }
    }

    function onLegalizedPatientPaymentReceiptSuccess() {
        var errMessage = { text: "" };
        var filterExpression = { text: "" };

        var hdnDepartmentIDLegalized = $('#<%=hdnDepartmentIDLegalized.ClientID%>').val();

        var hdnReportCodeReceipt = $('#<%=hdnReportCodeReceiptLegalized.ClientID%>').val();
        var hdnReportCodeReceiptEnglish = $('#<%=hdnReportCodeReceiptEnglishLegalized.ClientID%>').val();

        var hdnReportCodeReceiptOutpatient = $('#<%=hdnReportCodeReceiptOutpatientLegalized.ClientID%>').val();
        var hdnReportCodeReceiptEnglishOutpatient = $('#<%=hdnReportCodeReceiptEnglishOutpatientLegalized.ClientID%>').val();

        var hdnIsOutpatientAndDotMatrix = $('#<%=hdnIsDotMatrixAndOutpatientLegalized.ClientID%>').val();

        var reportCode = "";

        if (hdnReportCodeReceipt != null && hdnReportCodeReceipt != "" && hdnReportCodeReceipt != "0") {
            var displayOption = $('#<%=rblReceiptOption.ClientID %>').find(":checked").val();
            if (displayOption == 0) {
                if (hdnDepartmentIDLegalized != Constant.Facility.INPATIENT) {
                    reportCode = hdnReportCodeReceiptOutpatient;
                } else {
                    reportCode = hdnReportCodeReceipt;
                }
            }
            else {
                if (hdnDepartmentIDLegalized != Constant.Facility.INPATIENT) {
                    reportCode = hdnReportCodeReceiptEnglishOutpatient;
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
            showToast('Warning', "ReportCode untuk cetakan kwitansi legalisir tidak ditemukan.");
        }
        pcRightPanelContent.Hide();
        hideLoadingPanel();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrintLegalized" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnIsDotMatrixAndOutpatientLegalized" runat="server" value="" />
        <input type="hidden" id="hdnPaymentReceiptIDLegalized" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentIDLegalized" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptLegalized" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishLegalized" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptOutpatientLegalized" runat="server" value="" />
        <input type="hidden" id="hdnReportCodeReceiptEnglishOutpatientLegalized" runat="server" value="" />
        <input type="hidden" id="hdnParamReport" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
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
        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptLegalized" runat="server" Width="100%"
            ClientInstanceName="cbpPatientPaymentReceiptLegalized" ShowLoadingPanel="false"
            OnCallback="cbpPatientPaymentReceiptLegalized_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onLegalizedPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
        <dxcp:ASPxCallbackPanel ID="cbpPatientPaymentReceiptLegalizedDotMatrix" runat="server"
            Width="100%" ClientInstanceName="cbpPatientPaymentReceiptLegalizedDotMatrix" ShowLoadingPanel="false"
            OnCallback="cbpPatientPaymentReceiptLegalizedDotMatrix_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ oncbpPatientPaymentReceiptLegalizedDotMatrixEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
