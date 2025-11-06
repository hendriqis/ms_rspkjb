<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurgeryOrderRegistrationCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryOrderRegistrationCtl1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $(function () {
        setDatePicker('<%=txtOrderDate.ClientID %>');
        setDatePicker('<%=txtScheduleDate.ClientID %>');
        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');

        //#region Registration

        function getRegistrationFilterExpression() {
            var appointmentID = $('#<%:hdnAppointmentID.ClientID %>').val();
            var filterExpression = '';

            if (appointmentID != '') {
                filterExpression = "AppointmentID = " + appointmentID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "RegistrationNo <> ''";

            return filterExpression;
        }

        $('#<%:lblToRegistrationID.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('appointmentInfo', getRegistrationFilterExpression(), function (value) {
                $('#<%:hdnToRegistrationNo.ClientID %>').val(value);
                $('#<%:txtToRegistrationNo.ClientID %>').val(value);
                onTxtRegistrationNoChanged(value);
            });
        });

        $('#<%:txtToRegistrationNo.ClientID %>').live('change', function () {
            onTxtRegistrationNoChanged($(this).val());
        });

        function onTxtRegistrationNoChanged(value) {
            var filterExpression = "RegistrationNo = '" + value + "'";
            getRegistrationInfo(filterExpression);
        }

        function getRegistrationInfo(_filterExpression) {
            var registrationNo = $('#<%:hdnToRegistrationNo.ClientID %>').val();
            if (registrationNo != "") {
                Methods.getListObject('GetvRegistration1List', _filterExpression, function (result) {
                    if (result.length == 1) {
                        $('#<%:hdnToRegistrationID.ClientID %>').val(result[0].RegistrationID);
                        $('#<%:hdnToVisitID.ClientID %>').val(result[0].VisitID);
                        $('#<%:hdnToHealthcareServiceUnitID.ClientID %>').val(result[0].HealthcareServiceUnitID);
                        var registrationInfo = result[0].MedicalNo + '|' + result[0].PatientName + '|' + result[0].ServiceUnitName;
                        $('#<%:txtToRegistrationInfo.ClientID %>').val(registrationInfo);
                    }
                    else {
                        $('#<%:hdnToRegistrationID.ClientID %>').val('');
                        $('#<%:hdnToVisitID.ClientID %>').val('');
                        $('#<%:hdnToHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%:txtToRegistrationInfo.ClientID %>').val('');
                    }
                });
            } else {
                $('#<%:hdnToRegistrationID.ClientID %>').val('');
                $('#<%:hdnToVisitID.ClientID %>').val('');
                $('#<%:hdnToHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%:txtToRegistrationInfo.ClientID %>').val('');
            }
        }
        //#endregion
    });
</script>

<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnToRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnToVisitID" value="" />
    <input type="hidden" runat="server" id="hdnToHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnToRegistrationNo" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" Enabled="False" />                                
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" Enabled="False" />                                
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" >
                                <colgroup>
                                    <col style="width:120px" />
                                    <col  />
                                </colgroup>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("No. Registrasi Asal")%></label></td>
                                    <td><asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" Enabled="False" /> </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" Enabled="False" />                                
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" Enabled="False" />    
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Estimasi Lama Operasi") %></td>
                        <td><asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" Enabled="False"  /> menit</td>    
                        <td style="padding-left:5px"><asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" Enabled="false" /></td>                                            
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Rencana")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" Enabled="False"  />
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td><asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" /></td>                                      
                                    <td>                               
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="lblRoom">
                                <%:GetLabel("Ruang Operasi")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" id="hdnRoomID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 80px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" Enabled="False"  />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan=>
                                        <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>      
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" runat="server" id="Label1">
                                <%:GetLabel("No. Appointment")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAppointmentNo" Width="100%" runat="server" Enabled="False"  />
                        </td>
                    </tr>                            
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblToRegistrationID">
                                <%:GetLabel("No. Registrasi Kunjungan")%></label>
                        </td>
                        <td colspan="2">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtToRegistrationNo" Width="100%" runat="server" Enabled="False"  />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan=>
                                        <asp:TextBox ID="txtToRegistrationInfo" Width="100%" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                                       
                </table>
            </td>
        </tr>
    </table>
</div>


