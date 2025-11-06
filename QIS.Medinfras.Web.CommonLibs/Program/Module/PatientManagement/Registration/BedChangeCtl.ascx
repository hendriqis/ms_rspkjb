<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedChangeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BedChangeCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_bedchangeentryctl">

    //#region Service Unit
    var serviceUnitUserCount = parseInt('<%:serviceUnitUserCount %>');
    function getServiceUnitFilterFilterExpression() {
        var filterExpression = '';
        if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.INPATIENT) {
            var classID = $('#<%:hdnToClassID.ClientID %>').val();
            if (classID != '') {
                filterExpression = 'HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';
            }
        }
        else if ($('#<%:hdnDepartmentID.ClientID %>').val() == Constant.Facility.DIAGNOSTIC) {
            filterExpression = '<%:filterExpressionOtherMedicalDiagnostic %>';
        }

        var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();
        if (paramedicID != '') {
            if (filterExpression != '')
                filterExpression += ' AND ';
            filterExpression += '(HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + '))';
        }

        if (filterExpression != '') {
            filterExpression += ' AND ';
        }

        filterExpression += 'IsUsingRegistration = 1';

        return filterExpression;
    }

    $('#<%:lblToServiceUnit.ClientID %>.lblLink').live('click', function () {
        var parameter = "<%:GetServiceUnitUserParameter() %>" + getServiceUnitFilterFilterExpression();
        openSearchDialog('serviceunitroleuser', parameter, function (value) {
            $('#<%:txtToServiceUnitCode.ClientID %>').val(value);
            onTxtToServiceUnitCodeChanged(value);
        });
    });

    $('#<%:txtToServiceUnitCode.ClientID %>').live('change', function () {
        onTxtToServiceUnitCodeChanged($(this).val());
    });

    function onTxtToServiceUnitCodeChanged(value) {
        var filterExpression = "ServiceUnitCode = '" + value + "'";
        var parameter = "<%:GetServiceUnitUserParameter() %>" + filterExpression;
        Methods.getObject('GetServiceUnitUserAccessList', parameter, function (result) {
            if (result != null) {
                $('#<%:hdnToServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtToServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%:hdnToServiceUnitID.ClientID %>').val('');
                $('#<%:txtToServiceUnitCode.ClientID %>').val('');
                $('#<%:txtToServiceUnitName.ClientID %>').val('');
            }

            $('#<%=hdnToRoomID.ClientID %>').val('');
            $('#<%=txtToRoomCode.ClientID %>').val('');
            $('#<%=txtToRoomName.ClientID %>').val('');
            $('#<%=hdnToClassID.ClientID %>').val('');
            $('#<%=txtToClassCode.ClientID %>').val('');
            $('#<%=txtToClassName.ClientID %>').val('');
            $('#<%=hdnToChargeClassID.ClientID %>').val('');
            $('#<%=txtToChargeClassCode.ClientID %>').val('');
            $('#<%=txtToChargeClassName.ClientID %>').val('');
            $('#<%=hdnToBedID.ClientID %>').val('');
            $('#<%=txtToBedCode.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Room
    $('#<%=lblToRoom.ClientID %>.lblLink').live('click', function () {
        var serviceUnitID = $('#<%=hdnToServiceUnitID.ClientID %>').val();
        var filterExpression = '';
        if (serviceUnitID != '') {
            filterExpression = "IsDeleted = 0 AND HealthcareServiceUnitID = " + serviceUnitID;
            openSearchDialog('serviceunitroom', filterExpression, function (value) {
                $('#<%=txtToRoomCode.ClientID %>').val(value);
                ontxtToRoomCodeChanged(value);
            });
        } else {
            showToast('Warning', 'Pilih Ruang Perawatan Terlebih Dahulu !');
        }
    });

    $('#<%=txtToRoomCode.ClientID %>').live('change', function () {
        ontxtToRoomCodeChanged($(this).val());
    });

    function ontxtToRoomCodeChanged(value) {
        var filterExpression = '';
        var serviceUnitID = $('#<%=hdnToServiceUnitID.ClientID %>').val();
        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ";
            filterExpression += "RoomCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnToRoomID.ClientID %>').val(result.RoomID);
                    $('#<%=txtToRoomName.ClientID %>').val(result.RoomName);
                    $('#<%=hdnToClassID.ClientID %>').val(result.ClassID);
                    $('#<%=txtToClassCode.ClientID %>').val(result.ClassCode);
                    $('#<%=txtToClassName.ClientID %>').val(result.ClassName);
                    $('#<%=hdnToChargeClassID.ClientID %>').val(result.ChargeClassID);
                    $('#<%=txtToChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                    $('#<%=txtToChargeClassName.ClientID %>').val(result.ChargeClassName);
                }
                else {
                    $('#<%=hdnToRoomID.ClientID %>').val('');
                    $('#<%=txtToRoomCode.ClientID %>').val('');
                    $('#<%=txtToRoomName.ClientID %>').val('');
                    $('#<%=hdnToClassID.ClientID %>').val('');
                    $('#<%=txtToClassCode.ClientID %>').val('');
                    $('#<%=txtToClassName.ClientID %>').val('');
                    $('#<%=hdnToChargeClassID.ClientID %>').val('');
                    $('#<%=txtToChargeClassCode.ClientID %>').val('');
                    $('#<%=txtToChargeClassName.ClientID %>').val('');
                }
                $('#<%=hdnToBedID.ClientID %>').val('');
                $('#<%=txtToBedCode.ClientID %>').val('');
            });
        }
        else {
            $('#<%=hdnToRoomID.ClientID %>').val('');
            $('#<%=txtToRoomCode.ClientID %>').val('');
            $('#<%=txtToRoomName.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Bed
    $('#<%=lblToBed.ClientID %>.lblLink').live('click', function () {
        var roomID = $('#<%=hdnToRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "GCBedStatus = '0116^U' AND RoomID = " + roomID;
            openSearchDialog('bed', filterExpression, function (value) {
                $('#<%=txtToBedCode.ClientID %>').val(value);
                ontxtToBedCodeChanged(value);
            });
        } else {
            showToast('Warning', 'Pilih Kamar Perawatan Terlebih Dahulu !');
        }
    });

    $('#<%=txtToBedCode.ClientID %>').live('change', function () {
        ontxtToBedCodeChanged($(this).val());
    });

    function ontxtToBedCodeChanged(value) {
        var roomID = $('#<%=hdnToRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND ";
            filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0 AND GCBedStatus = '0116^U'";
            Methods.getObject('GetvBedList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnToBedID.ClientID %>').val(result.BedID);
                }
                else {
                    $('#<%=hdnToBedID.ClientID %>').val('');
                    $('#<%=txtToBedCode.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%=hdnToBedID.ClientID %>').val('');
            $('#<%=txtToBedCode.ClientID %>').val('');
        }
    }
    //#endregion

</script>
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
<input type="hidden" value="" id="hdnDepartmentID" runat="server" />
<input type="hidden" value="" id="hdnParamedicID" runat="server" />
<div>
    <table class="tblContentArea">
        <tr>
            <td>
                <div>
                    <table class="tblEntryDetail" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Registrasi")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server" Style="text-align: left" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Rekam Medis")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMedicalNo" Width="100px" ReadOnly="true" runat="server" Style="text-align: left" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Nama Pasien")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientName" Width="300px" ReadOnly="true" runat="server" Style="text-align: left" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div class="lblComponent">
                                    <%=GetLabel("Dari")%></div>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 400px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Ruang Perawatan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnFromServiceUnitID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFromServiceUnitCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kamar")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnFromRoomID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFromRoomCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromRoomName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Tempat Tidur")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnFromBedID" value="" runat="server" />
                                            <asp:TextBox ID="txtFromBedCode" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kelas")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnFromClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFromClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromClassName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Kelas Tagihan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnFromChargeClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFromChargeClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromChargeClassName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <div class="lblComponent">
                                    <%=GetLabel("Ke")%></div>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 400px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblToServiceUnit">
                                                <%=GetLabel("Ruang Perawatan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToServiceUnitID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtToServiceUnitCode" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblToRoom">
                                                <%=GetLabel("Kamar")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToRoomID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtToRoomCode" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToRoomName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" runat="server" id="lblToBed">
                                                <%=GetLabel("Tempat Tidur")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToBedID" value="" runat="server" />
                                            <asp:TextBox ID="txtToBedCode" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="lblToClass">
                                                <%=GetLabel("Kelas")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtToClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToClassName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" runat="server" id="lblToChargeClass">
                                                <%=GetLabel("Kelas Tagihan")%></label>
                                        </td>
                                        <td>
                                            <input type="hidden" id="hdnToChargeClassID" value="" runat="server" />
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtToChargeClassCode" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToChargeClassName" ReadOnly="true" Width="100%" runat="server" />
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
            </td>
        </tr>
    </table>
</div>
