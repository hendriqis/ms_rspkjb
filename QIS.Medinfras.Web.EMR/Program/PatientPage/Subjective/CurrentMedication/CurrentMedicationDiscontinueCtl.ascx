<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentMedicationDiscontinueCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.CurrentMedicationDiscontinueCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_currentmedicationdiscontinuectl">
    setDatePicker('<%=txtDiscontinueDate.ClientID %>');
    $('#<%=txtDiscontinueDate.ClientID %>').datepicker('option', 'maxDate', '0');
    var startDateDiscontinueMedication = Methods.stringToDate('<%=startDateInString %>');

    function onCboDiscontinueValueChanged() {
        if (cboDiscontinueReason.GetValue() == Constant.DiscontinueMedicationReason.OTHER)
            $('#<%=txtOtherDiscontinueReason.ClientID %>').show();
        else
            $('#<%=txtOtherDiscontinueReason.ClientID %>').hide();
    }

    $(function () {

        $('#<%=txtDiscontinueDate.ClientID %>').change(function () {
            var diffDate = Methods.calculateDateDifference(startDateDiscontinueMedication, Methods.getDatePickerDate($(this).val()));
            var courseDuration = '';
            if (diffDate.years > 0)
                courseDuration += diffDate.years + "year(s) ";
            if (diffDate.months > 0)
                courseDuration += diffDate.months + "month(s) ";
            if (diffDate.days > 0 || courseDuration == '')
                courseDuration += diffDate.days + "day(s) ";
            $('#<%=txtCourseDuration.ClientID %>').val(courseDuration);
        });
        $('#<%=txtDiscontinueDate.ClientID %>').change();
    });
</script>
<input type="hidden" runat="server" id="hdnID" value="" />
<table style="text-align: left; width: 100%; padding-bottom: 10px; border-bottom: 1px solid #AAA;"
    cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 100px; vertical-align: top;">
            <div id="divDate" runat="server">
            </div>
        </td>
        <td>
            <div id="divInformationLine" runat="server">
            </div>
            <div>
                <div style="color: Blue; width: 40px; float: left;"><%=GetLabel("DOSE") %></div>
                <span id="spnDose" runat="server" style="margin-left:10px;"></span>
            </div>
        </td>
    </tr>
</table>

<table style="margin-top: 10px; text-align: left; width: 100%;">
    <colgroup style="width: 150px" />
    <colgroup style="width: 200px" />
    <tr>
        <td class="tdLabel"><%=GetLabel("Discontinue Date")%></td>
        <td><asp:TextBox ID="txtDiscontinueDate" CssClass="datepicker" runat="server" Width="120px" /></td>
    </tr>
    <tr>
        <td class="tdLabel"><%=GetLabel("Course Duration")%></td>
        <td><asp:TextBox ID="txtCourseDuration" ReadOnly="true" runat="server" Width="100%" /></td>
    </tr>
    <tr>
        <td class="tdLabel"><%=GetLabel("Reason For Discontinue")%></td>
        <td>
            <dxe:ASPxComboBox ID="cboDiscontinueReason" ClientInstanceName="cboDiscontinueReason" runat="server" Width="100%">
                <ClientSideEvents ValueChanged="function(s,e){ onCboDiscontinueValueChanged(); }"
                    Init="function(s,e){ onCboDiscontinueValueChanged(); }" />
            </dxe:ASPxComboBox>
        </td>
        <td><asp:TextBox ID="txtOtherDiscontinueReason" CssClass="required" runat="server" Width="100%" /></td>
    </tr>
</table>
