<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientVentilatorEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientVentilatorEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_vaccinationentryctl1">
    setDatePicker('<%=txtStartDate.ClientID %>');
    $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onBeforeSaveRecordEntryPopup(errMessage) {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        if ($('#<%=txtStartDate.ClientID %>').val() == '') {
            $('#<%=txtStartDate.ClientID %>').val(dateToday);
        }

        var dateSelected = $('#<%=txtStartDate.ClientID %>').val();
        var startDatePicker = Methods.getDatePickerDate(dateSelected);

        if (new Date(startDatePicker).toString() !== 'Invalid Date') {
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (f > t) {
                displayErrorMessageBox('Pemasangan Alat', 'Tanggal Pemasangan tidak bisa dipilih melewati tanggal hari ini!');
                $('#<%=txtStartDate.ClientID %>').val(dateToday);
                return false;
            }
            else {
                return true;
            }
        }
        else {
            $('#<%=txtStartDate.ClientID %>').val(dateToday);
        }
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }
</script>
<div style="height: 300px">
    <input type="hidden" runat="server" id="hdnPopupID" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupTypeID" value="" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table style="width: 100%" class="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 160px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal dan Jam Pemasangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="125px" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="60px" />                 
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Alasan Pemasangan Alat")%></label>
                        </td>
                        <td colspan="3" style="vertical-align:top">
                            <asp:TextBox ID="txtETTReason" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr id="trParamedicInfo" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dipasang oleh")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="235px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trETTSizeInfo" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Size")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtETTSize" CssClass="quantity" Width="60px" runat="server" Text="0" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
