<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratPerintahRawatInapCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratPerintahRawatInapCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    if ($('#<%:txtPhysicianCode.ClientID %>').val() != '') {
        Methods.getObject('GetvParamedicMasterList', "ParamedicCode = '" + $('#<%:txtPhysicianCode.ClientID %>').val() + "'", function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val("");
                $('#<%:txtPhysicianCode.ClientID %>').val("");
                $('#<%:txtPhysicianName.ClientID %>').val("");
            }
        });
    }

    if ($('#<%:txtServiceUnitCode.ClientID %>').val() != '') {
        Methods.getObject('GetvHealthcareServiceUnitCustomList', "ServiceUnitCode = '" + $('#<%:txtServiceUnitCode.ClientID %>').val() + "'", function (result) {
            if (result != null) {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val("");
                $('#<%:txtServiceUnitCode.ClientID %>').val("");
                $('#<%:txtServiceUnitName.ClientID %>').val("");
            }
        });
    }

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    //#region Dokter DPJP
    function getPhysicianBPJSFilterExpression() {
        var filterExpression = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('physician', getPhysicianBPJSFilterExpression(), function (value) {
            $('#<%:txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onTxtPhysicianCodeChanged(value) {
        filterExpression = getPhysicianBPJSFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%:txtPhysicianCode.ClientID %>').val(result.ParamedicCode);
                $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val("");
                $('#<%:txtPhysicianCode.ClientID %>').val("");
                $('#<%:txtPhysicianName.ClientID %>').val("");
            }
        });
    }
    //#endregion

    //#region Ruangan
    function getServiceUnitFilterExpression() {
        var filterExpression = "DepartmentID = 'INPATIENT' AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblServiceUnit.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterExpression(), function (value) {
            $('#<%:txtServiceUnitCode.ClientID %>').val(value);
            onTxtServiceUnitCodeChanged(value);
        });
    });

    $('#<%:txtServiceUnitCode.ClientID %>').live('change', function () {
        onTxtServiceUnitCodeChanged($(this).val());
    });

    function onTxtServiceUnitCodeChanged(value) {
        filterExpression = getServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
        Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                $('#<%:txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                $('#<%:txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val("");
                $('#<%:txtServiceUnitCode.ClientID %>').val("");
                $('#<%:txtServiceUnitName.ClientID %>').val("");
            }
        });
    }
    //#endregion

    //#region Room

    function getRoomFilterExpression() {
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = '';

        if (serviceUnitID != '') {
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
        }

        if (filterExpression != '') {
            filterExpression += " AND ";
        }
        filterExpression += "IsDeleted = 0 AND DepartmentID = 'INPATIENT'";

        return filterExpression;
    }

    $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
            $('#<%:txtRoomCode.ClientID %>').val(value);
            onTxtRoomCodeChanged(value);
        });
    });

    $('#<%:txtRoomCode.ClientID %>').live('change', function () {
        onTxtRoomCodeChanged($(this).val());
    });

    function onTxtRoomCodeChanged(value) {
        var filterExpression = getRoomFilterExpression() + " AND RoomCode = '" + value + "'";
        getRoom(filterExpression);
    }

    function getRoom(_filterExpression) {
        var filterExpression = _filterExpression;
        if (filterExpression == '') filterExpression = getRoomFilterExpression();
        var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID != "") {
            Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result.length == 1) {
                    $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                    $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                    $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                }
                else {
                    $('#<%:hdnRoomID.ClientID %>').val('');
                    $('#<%:txtRoomCode.ClientID %>').val('');
                    $('#<%:txtRoomName.ClientID %>').val('');
                }
            });
        } else {
            $('#<%:hdnRoomID.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').val('');
            $('#<%:txtRoomName.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Bed
    $('#<%:lblBed.ClientID %>.lblLink').live('click', function () {
        var roomID = $('#<%:hdnRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND GCBedStatus = '0116^U'";
            openSearchDialog('bed', filterExpression, function (value) {
                $('#<%:txtBedCode.ClientID %>').val(value);
                onTxtBedCodeChanged(value);
            });
        }
    });

    $('#<%:txtBedCode.ClientID %>').live('change', function () {
        onTxtBedCodeChanged($(this).val());
    });

    function onTxtBedCodeChanged(value) {
        var roomID = $('#<%:hdnRoomID.ClientID %>').val();
        var filterExpression = '';
        if (roomID != '') {
            filterExpression = "RoomID = " + roomID + " AND ";
            filterExpression += "BedCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvBedList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnBedID.ClientID %>').val(result.BedID);
                }
                else {
                    $('#<%:hdnBedID.ClientID %>').val('');
                    $('#<%:txtBedCode.ClientID %>').val('');
                }
            });
        }
        else {
            $('#<%:hdnBedID.ClientID %>').val('');
            $('#<%:txtBedCode.ClientID %>').val('');
        }
    }
    //#endregion

    function onReprintPatientPaymentReceiptSuccess() {

        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var diagnoseText = $('#<%=txtDiagnoseText.ClientID %>').val();
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
        var roomID = $('#<%=hdnRoomID.ClientID %>').val();
        var bedID = $('#<%=hdnBedID.ClientID %>').val();
        var reportCode = "PM-00709";

        var filterExpression = visitID + "|" + diagnoseText + "|" + paramedicID + "|" + serviceUnitID + "|" + roomID + "|" + bedID;
        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
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
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnTanggalString" runat="server" value="" />
        <input type="hidden" id="hdnReportCode" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" runat="server" id="lblServiceUnit">
                        <%:GetLabel("Ruangan")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" runat="server" id="lblRoom">
                        <%:GetLabel("Kamar")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnRoomID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRoomName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" runat="server" id="lblBed">
                        <%:GetLabel("Tempat Tidur")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnBedID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtBedCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" runat="server" id="lblPhysician">
                        <%:GetLabel("Dokter yang merawat")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Diagnosa")%></label>
                </td>
                 <td>
                    <asp:TextBox ID="txtDiagnoseText" Width="300px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
