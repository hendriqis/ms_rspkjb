<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganIstirahatKaryawanCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganIstirahatKaryawanCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    setDatePicker('<%=txtValueDateFrom.ClientID %>');

    setDatePicker('<%=txtValueDateTo.ClientID %>');

    setDatePicker('<%=txtValueDay.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpEmployeeRestLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDateFrom.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        $('#<%=hdnTanggalDari.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalDariString.ClientID %>').val(date);
    });

    $('#<%=txtValueDateTo.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = DD + '-' + MM + '-' + YYYY;
        $('#<%=hdnTanggalSampai.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalSampaiString.ClientID %>').val(date);
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var registrationID = $('#<%=hdnRegistrationIDVisit.ClientID %>').val();
        var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
        var from = $('#<%=hdnTanggalDari.ClientID %>').val();
        var to = $('#<%=hdnTanggalSampai.ClientID %>').val();
        var Day = $('#<%=txtDay.ClientID %>').val();
        var ValueDay = $('#<%=txtValueDay.ClientID %>').val();
        var Jam = $('#<%=txtValueJam.ClientID %>').val();
        var ValueHari = $('#<%=txtValueHari.ClientID %>').val();
        var NIK = $('#<%=txtNIK.ClientID %>').val();
        var Unit = $('#<%=txtUnit.ClientID %>').val();

        var errMessage = { text: "" };
        var filterExpression = registrationID + "|" + from + "|" + to + "|" + Day + "|" + ValueDay + "|" + Jam + "|" + ValueHari;
        var reportCode = "PM-00525";

        if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "rsdo-ska") {
            if (departmentID == Constant.Facility.INPATIENT) {
                var filterExpression = registrationID + "|" + from + "|" + to + "|" + ValueHari + "|" + NIK + "|" + Unit;
                reportCode = "PM-00706";
            }
            else {
                var filterExpression = registrationID + "|" + from + "|" + to + "|" + Day + "|" + ValueDay + "|" + Jam + "|" + ValueHari + "|" + NIK + "|" + Unit;
                reportCode = "PM-00705";
            }
        }

        openReportViewer(reportCode, filterExpression);
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
        <input type="hidden" id="hdnRegistrationIDVisit" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <input type="hidden" id="hdnHealthcareInitial" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDari" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampai" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDariString" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampaiString" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 10px" />
                <col style="width: 300px" />
                <col />
            </colgroup>
            <tr id="trNIK" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("NIK :")%></label>
                </td>
                <td>
                    <td class="tdNIK">
                        <asp:TextBox ID="txtNIK" Width="250px" CssClass="required" runat="server" />
                    </td>
                </td>
            </tr>
            <tr id="trUnit" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Unit :")%></label>
                </td>
                <td>
                    <td class="tdUnit">
                        <asp:TextBox ID="txtUnit" Width="250px" CssClass="required" runat="server" />
                    </td>
                </td>
            </tr>
            <tr id="trHari" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Hari")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtDay" CssClass="txtDay" runat="server" Width="100px" />
                    </td>
                </td>
            </tr>
            <tr id="trTanggal" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueDay" CssClass="txtValueDay datepicker" runat="server" Width="100px" />
                    </td>
                </td>
            </tr>
            <tr id="trJam" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jam")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueJam" CssClass="txtValueJam" runat="server" Width="70px"
                            Style="text-align: center" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jumlah Hari")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueHari" CssClass="txtValueHari" runat="server" Width="50px"
                            Style="text-align: center" />
                    </td>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Terhitung tanggal")%></label>
                </td>
                <td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker" runat="server"
                            Width="100px" />
                        -
                        <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker" runat="server"
                            Width="100px" />
                    </td>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpEmployeeRestLeave" runat="server" Width="100%" ClientInstanceName="cbpEmployeeRestLeave"
            ShowLoadingPanel="false" OnCallback="cbpEmployeeRestLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
