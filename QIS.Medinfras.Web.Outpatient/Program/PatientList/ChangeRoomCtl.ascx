<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeRoomCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Outpatient.Program.ChangeRoomCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_registrationeditctl">
    $(function () {

    });

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtPhysicianCode.ClientID %>').val();
        return result;
    }

    //#region Room
    $('#lblRoom.lblLink').live('click', function () {
        openSearchDialog('room', "IsDeleted = 0 AND IsWardRoom = 0", function (value) {
            $('#<%=txtRoomCode.ClientID %>').val(value);
            onTxtRoomCodeChanged(value);
        });
    });

    $('#<%=txtRoomCode.ClientID %>').live('change', function () {
        onTxtRoomCodeChanged($(this).val());
    });

    function onTxtRoomCodeChanged(value) {
        var filterExpression = "RoomCode = '" + value + "'";
        Methods.getObject('GetRoomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                $('#<%=txtRoomCode.ClientID %>').val(result.RoomCode);
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
<div style="height: 350px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnMRNCtl" value="" />
    <input type="hidden" runat="server" id="hdnGender" value="" />
    <input type="hidden" runat="server" id="hdnVisitDate" value="" />
    <input type="hidden" id="hdnIsBridgingToGateway" value="0" runat="server" />
    <input type="hidden" id="hdnProviderGatewayService" value="0" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink lblMandatory" id="lblServiceUnit">
                    <%=GetLabel("Klinik")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCode" CssClass="required" Width="100%" runat="server" />
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
                <label class="lblLink lblMandatory" id="lblPhysician">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
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
                <label class="lblLink lblMandatory" id="lblRoom">
                    <%=GetLabel("Kamar")%></label>
            </td>
            <td>
                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
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
    </table>
</div>
