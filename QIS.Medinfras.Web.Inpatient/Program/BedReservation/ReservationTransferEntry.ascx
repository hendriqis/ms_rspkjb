<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationTransferEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReservationTransferEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="toolbarArea">
    <ul>
        <li id="btnProcess" crudmode="R">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
            <div>
                <%=GetLabel("Process") %></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_reservationctl">
    $('#btnProcess').live('click', function () {
        cbpPopupProcess.PerformCallback('process');
    });

    //#region Service Unit
    $('#<%=lblToServiceUnit.ClientID %>.lblLink').live('click', function () {
        var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + Constant.Facility.INPATIENT + "'";
        var classID = $('#<%=hdnToClassID.ClientID %>').val();
        if (classID != '')
            filterExpression += ' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitRoom WHERE ClassID = ' + classID + ')';
        var paramedicID = $('#<%=hdnToParamedicID.ClientID %>').val();
        if (paramedicID != '') {
            filterExpression += ' AND (HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + ') OR IsHasParamedic = 0)';
        }
        openSearchDialog('serviceunitparamedicvisittypeperhealthcare', filterExpression, function (value) {
            $('#<%=txtToServiceUnitCode.ClientID %>').val(value);
            ontxtToPolyclinicCodeChanged(value);
        });
    });

    $('#<%=txtToServiceUnitCode.ClientID %>').live('change', function () {
        ontxtToPolyclinicCodeChanged($(this).val());
    });

    function ontxtToPolyclinicCodeChanged(value) {
        var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + Constant.Facility.INPATIENT + "' AND ServiceUnitCode = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnToServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%=txtToServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                $('#<%=hdnToIsServiceUnitHasParamedic.ClientID %>').val(result.IsHasParamedic ? '1' : '0');
            }
            else {
                $('#<%=hdnToServiceUnitID.ClientID %>').val('');
                $('#<%=txtToServiceUnitCode.ClientID %>').val('');
                $('#<%=txtToServiceUnitName.ClientID %>').val('');
                $('#<%=hdnToIsServiceUnitHasParamedic.ClientID %>').val('0');
            }
            $('#<%=hdnToRoomID.ClientID %>').val('');
            $('#<%=txtToRoomCode.ClientID %>').val('');
            $('#<%=txtToRoomName.ClientID %>').val('');
            $('#<%=hdnToBedID.ClientID %>').val('');
            $('#<%=txtToBedCode.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Room
    $('#<%=lblToRoom.ClientID %>.lblLink').live('click', function () {
        var serviceUnitID = $('#<%=hdnToServiceUnitID.ClientID %>').val();
        var classID = $('#<%:hdnToClassID.ClientID %>').val();
        var filterExpression = "IsDeleted = 0";
        if (serviceUnitID != '') {
            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "HealthcareServiceUnitID = " + serviceUnitID;
            if (classID != '0' && classID != '') {
                if (filterExpression != '') {
                    filterExpression += " AND ";
                }
                filterExpression += "ClassID = " + classID;
            }

            openSearchDialog('serviceunitroom', filterExpression, function (value) {
                $('#<%=txtToRoomCode.ClientID %>').val(value);
                ontxtToRoomCodeChanged(value);
            });
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

                    if ($('#<%=hdnToClassID.ClientID %>').val() == '') {
                        $('#<%=hdnToClassID.ClientID %>').val(result.ClassID);
                        $('#<%=txtToClassCode.ClientID %>').val(result.ClassCode);
                        $('#<%=txtToClassName.ClientID %>').val(result.ClassName);

                        $('#<%=hdnToChargeClassID.ClientID %>').val(result.ChargeClassID);
                        $('#<%=txtToChargeClassCode.ClientID %>').val(result.ChargeClassCode);
                        $('#<%=txtToChargeClassName.ClientID %>').val(result.ChargeClassName);
                    }
                }
                else {
                    $('#<%=hdnToRoomID.ClientID %>').val('');
                    $('#<%=txtToRoomCode.ClientID %>').val('');
                    $('#<%=txtToRoomName.ClientID %>').val('');
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
            filterExpression = "RoomID = " + roomID;
            openSearchDialog('bed', filterExpression, function (value) {
                $('#<%=txtToBedCode.ClientID %>').val(value);
                ontxtToBedCodeChanged(value);
            });
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
            filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0";
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
    function onCbpPopupProcesEndCallback(s) {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div style="padding: 5px 0;">
    <input type="hidden" id="hdnDefaultDate" runat="server" />
    <input type="hidden" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnReservationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowAddRecord" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table class="tblContentArea" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal")%>
                                -
                                <%=GetLabel("Jam")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransferDate" Width="120px" ReadOnly="true" CssClass="datepicker"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransferTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Transfer")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboTransferType" ClientInstanceName="cboTransferType" Width="200px"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col width="60px" />
                        <col width="150px" />
                        <col width="10px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("DARI :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFromParamedicID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromPhysicianCode" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Spesialisasi")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFromSpecialtyID" value="" runat="server" />
                            <asp:TextBox ID="txtFromSpecialtyName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col width="60px" />
                        <col width="150px" />
                        <col width="10px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("KE :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnToParamedicID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtToPhysicianCode" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                           <label class="lblNormal">
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
                            <label class="lblLink lblMandatory" runat="server" id="lblToServiceUnit">
                                <%=GetLabel("Ruang Perawatan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnToIsServiceUnitHasParamedic" value="" runat="server" />
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
                            <label class="lblNormal">
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
                                        <asp:TextBox ID="txtToChargeClassCode" Width="100%" runat="server" />
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Spesialisasi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboToSpecialty" ClientInstanceName="cboToSpecialty" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
