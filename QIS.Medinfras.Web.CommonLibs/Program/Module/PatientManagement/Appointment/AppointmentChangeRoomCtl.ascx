<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentChangeRoomCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentChangeRoomCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_changeroomappointmentctl">
    //#region Room
    function onGetServiceUnitRoomFilterExpression() {
        var serviceUnitID = $('#<%=hdnServiceUnitIDCtl.ClientID %>').val();
        var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID + ' AND IsDeleted = 0';
        return filterExpression;
    }

    $('#lblRoom.lblLink').live('click', function () {
        openSearchDialog('serviceunitroom', onGetServiceUnitRoomFilterExpression(), function (value) {
            $('#<%=txtRoomCode.ClientID %>').val(value);
            onTxtServiceUnitRoomCodeChanged(value);
        });
    });

    $('#<%=txtRoomCode.ClientID %>').live('change', function () {
        onTxtServiceUnitRoomCodeChanged($(this).val());
    });

    function onTxtServiceUnitRoomCodeChanged(value) {
        var filterExpression = onGetServiceUnitRoomFilterExpression() + " AND RoomCode = '" + value + "'";
        Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
            }
            else {
                $('#<%=hdnRoomID.ClientID %>').val('');
                $('#<%=txtRoomCode.ClientID %>').val('');
                $('#<%=txtRoomName.ClientID %>').val('');
            }
        });
    }
    //#endregion 
</script>
<div style="height: 150px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnParamedicIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnSelectedDateCtl" value="" />
    <input type="hidden" runat="server" id="hdnServiceUnitIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnStartTimeCtl" value="" />
    <input type="hidden" runat="server" id="hdnEndTimeCtl" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 500px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory" id="lblPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 5px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory" id="lblServiceUnit">
                    <%=GetLabel("Klinik")%></label>
            </td>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 5px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCode" CssClass="required" ReadOnly="true" Width="100%"
                                runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblRoom">
                    <%=GetLabel("Room")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnRoomID" runat="server" value="" />
                <table>
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 5px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtRoomCode" CssClass="required" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory" id="Label1">
                    <%=GetLabel("Tanggal / Sesi")%></label>
            </td>
            <td>
                <input type="hidden" id="Hidden1" runat="server" value="" />
                <table>
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 5px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDate" CssClass="required" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtSession" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
