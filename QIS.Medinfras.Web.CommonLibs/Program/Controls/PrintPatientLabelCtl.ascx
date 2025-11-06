<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintPatientLabelCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrintPatientLabelCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpPrintPatientLabel.PerformCallback('Print');
        }
    });

    $('#<%=txtPrintTotal.ClientID %>').change(function () {
        var jumlahPrint = $('#<%=txtPrintTotal.ClientID %>').val();
        $('#<%=hdnJumlahPrint.ClientID %>').val(jumlahPrint);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
        var printTotal = $('#<%=hdnJumlahPrint.ClientID %>').val();
        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
        var isLab = $('#<%=hdnIsLaboratoryUnit.ClientID %>').val();
        var isRad = $('#<%=hdnIsImagingUnit.ClientID %>').val();
        var registrationStatus = $('#<%=hdnRegistrationStatus.ClientID %>').val();
        var initial = $('#<%=hdnInitialHealthcare.ClientID %>').val();

        var errMessage = { text: "" };
        var filterExpression = registrationID + "|" + printTotal;
        var reportCode = "";

        if (initial == "RSSK") {
            if (registrationStatus != "X020^006") {
                reportCode = "PM-00796"
            } else {
                showToast('Warning', 'Registrasi sudah dibatalkan.');
                return false;
            }
        }
        else {
            if (registrationStatus != "X020^006") {
                if (departmentID == Constant.Facility.OUTPATIENT || departmentID == Constant.Facility.MCU ||
                departmentID == Constant.Facility.EMERGENCY || departmentID == Constant.Facility.PHARMACY) {
                    reportCode = "PM-00424";
                }
                else if (departmentID == Constant.Facility.INPATIENT) {
                    reportCode = "PM-00425";
                }
                else if (departmentID == Constant.Facility.DIAGNOSTIC) {
                    if (isLab == "True") {
                        reportCode = "PM-00424";
                    }
                    else if (isRad == "True") {
                        reportCode = "PM-00427";
                    }
                    else {
                        reportCode = "PM-00431";
                    }
                }
            } else {
                showToast('Warning', 'Registrasi sudah dibatalkan.');
                return false;
            }
        }

        if (reportCode != "") {
            if (printTotal < "1" || printTotal > "4") {
                showToast('Warning', 'Jumlah tidak bisa kurang dari 1 atau lebih dari 4');
                return false;
            }
            else {
                openReportViewer(reportCode, filterExpression);
            }
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnIsLaboratoryUnit" runat="server" value="" />
        <input type="hidden" id="hdnIsImagingUnit" runat="server" value="" />
        <input type="hidden" id="hdnJumlahPrint" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationStatus" runat="server" value="" />
        <input type="hidden" id="hdnInitialHealthcare" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 50px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Jumlah print per 3 label")%></label>
                </td>
                <td>
                    <td class="tdPrintTotal">
                        <asp:TextBox ID="txtPrintTotal" runat="server" Width="120px" />
                    </td>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    <%=GetLabel("(min = 1, max = 4)")%></label>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpPrintPatientLabel" runat="server" Width="100%" ClientInstanceName="cbpPrintPatientLabel"
            ShowLoadingPanel="false" OnCallback="cbpPrintPatientLabel_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
