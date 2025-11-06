<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessReferralCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessReferralCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    $(function () {
        $('#<%=rblReferralType.ClientID %> input').die('change');
        $('#<%=rblReferralType.ClientID %> input').live('change', function () {
            ToggleFollowUpVisitControl();
        });

        setDatePicker('<%=txtAppointmentDate.ClientID %>');
        $('#<%=txtAppointmentDate.ClientID %>').datepicker('option', 'minDate', '0');

        ToggleFollowUpVisitControl();
    });

    function ToggleFollowUpVisitControl() {
        var value = $('#<%=rblReferralType.ClientID %> input[type=radio]:checked').val();
        if (value == 'X075^04') {
            $('#trAppointmentDate').attr('style', 'display:none');
            $('#trAppointmentRemarks').attr('style', 'display:none');
            $('#trAppointmentInfo').attr('style', 'display:none');
        }
        else {
            $('#trAppointmentDate').removeAttr('style');
            $('#trAppointmentRemarks').removeAttr('style');
            $('#trAppointmentInfo').removeAttr('style');
        }
    }

    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }


    //#region Physician
    function onGetPatientVisitParamedicFilterExpression() {
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
        if (serviceUnitID != '')
            filterExpression = "GCParamedicMasterType = 'X019^001' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
        return filterExpression;
    }

    $('#lblPatientVisitPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPatientVisitPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPatientVisitPhysicianCodeChanged($(this).val());
    });

    function onTxtPatientVisitPhysicianCodeChanged(value) {
        var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                $('#<%=hdnSpecialtyID.ClientID %>').val(result.SpecialtyID);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
                $('#<%=hdnSpecialtyID.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script> 

<div style="height: auto; overflow-y: auto; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="0" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" id="hdnIsCopyFromSource" runat="server" value="" />
    <input type="hidden" id="hdnFromRegistrationID" runat="server" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td><asp:TextBox ID="txtReferralDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" /></td>
            <td><asp:TextBox ID="txtReferralTime" Width="80px" CssClass="time" runat="server" ReadOnly="true"/></td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="Label2">
                    <%=GetLabel("Dokter Pengirim")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnFromPhysicianID" value="" runat="server" />
                <asp:TextBox ID="txtFromPhysicianCode" ReadOnly="true" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtFromPhysicianName" ReadOnly="true" Width="99%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal" id="Label3"><%=GetLabel("Jenis Rujukan")%></label></td>
            <td colspan="2">
                <asp:RadioButtonList ID="rblReferralType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text=" Kunjungan Langsung" Value="X075^04" />
                    <asp:ListItem Text=" Perjanjian (Appointment)" Value="X075^05"  Selected="True"  />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id = "trAppointmentDate">
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Jadwal")%></label></td>
            <td><asp:TextBox ID="txtAppointmentDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtAppointmentTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="Label1">
                    <%=GetLabel("Unit Rujukan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" ReadOnly="true" />
            </td>
            <td>
                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="99%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblPatientVisitPhysician">
                    <%=GetLabel("Dokter Rujukan")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <input type="hidden" id="hdnSpecialtyID" value="" runat="server" />
                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="99%" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("Jenis Kunjungan") %></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboFollowupVisitType" ClientInstanceName="cboFollowupVisitType"
                    Width="100%" />
            </td>
        </tr>
        <tr id = "trAppointmentRemarks">
            <td class="tdLabel">
                <label class="lblNormal" id="Label4">
                    <%=GetLabel("Catatan Jadwal Kunjungan")%></label>
            </td>
            <td colspan="2"><asp:TextBox ID="txtAppointmentRemarks" Width="99%" runat="server" TextMode="MultiLine" Rows="2" /></td>
        </tr>
        <tr id = "trAppointmentInfo">
            <td colspan="3">
                <table style="width:100%">
                    <colgroup width="70px" />
                    <colgroup />
                    <tr>
                        <td>
                            <img src='<%=ResolveUrl("~/Libs/Images/appointment_info.png")%>' alt="" height="65px" width="65px" />
                        </td>
                        <td style="vertical-align:top;">
                            <h4 style="background-color:transparent;color:black;font-weight:bold"><%=GetLabel("Perjanjian Kunjungan (Appointment)")%></h4>
                            <%=GetLabel("Perjanjian kunjungan harus dikonfirmasi dan diproses oleh Unit Perjanjian Kunjungan Pasien")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
