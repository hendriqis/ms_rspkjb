<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
CodeBehind="PatientDischarge.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientDischarge" %>

<%@ Register Src="~/Program/Process/BPJS/INACBGSToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
    <asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientDischargeProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            setDatePicker('<%=txtDischargeDate.ClientID %>');

            $('#<%=btnPatientDischargeProcess.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    onCustomButtonClick('process');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();
        });

        function dateDiff(date1, date2) {
            var diff = date2 - date1;
            return isNaN(diff) ? NaN : {
                diff: diff,
                ms: Math.floor(diff % 1000),
                s: Math.floor(diff / 1000 % 60),
                m: Math.floor(diff / 60000 % 60),
                h: Math.floor(diff / 3600000 % 24),
                d: Math.floor(diff / 86400000)
            };
        }

        function onDischargeDateTimeChange() {
            var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDate.ClientID %>').val());
            var dischargeTime = $('#<%=txtDischargeTime.ClientID %>').val();
            var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
            var diff = dateDiff(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
            $('#<%=hdnLOSInDay.ClientID %>').val(diff.d);
            $('#<%=hdnLOSInHour.ClientID %>').val(diff.h);
            $('#<%=hdnLOSInMinute.ClientID %>').val(diff.m);
            $('#<%=txtLengthOfVisit.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');
        }

//        function onAfterCustomClickSuccess(type) {
//            exitPatientPage();
//        }
    </script>

    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <fieldset id="fsPatientDischarge">  
        <table class="tblEntryContent" style="width:500px">
            <colgroup>
                <col style="width:150px"/>
                <col style="width:150px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal")%> - <%=GetLabel("Jam")%></label></td>
                <td><asp:TextBox ID="txtDischargeDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                <td><asp:TextBox ID="txtDischargeTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Lama Kunjungan")%></label></td>
                <td colspan="2"><asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kondisi Keluar")%></label></td>
                <td colspan="2"><dxe:ASPxComboBox ID="cboPatientOutcome" Width="100%" runat="server" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Cara Keluar")%></label></td>
                <td colspan="2"><dxe:ASPxComboBox ID="cboDischageRoutine" Width="100%" runat="server" /></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
