<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPListEntry.master" AutoEventWireup="true"
    CodeBehind="PatientTransferEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.PatientTransferEntry" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientTransferBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowAddRecord" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnPatientTransferBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = document.referrer;
            });

            $('#btnPickBed').click(function () {
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl('~/Controls/BedQuickPicksCtl.ascx');
                openUserControlPopup(url, id, 'Pilih Bed', 1150, 600);
            });
        });

        function onAfterClickBedQuickPicks(healthcareServiceUnitID, serviceUnitCode, serviceUnitName, roomID, roomCode, roomName, classID, classCode, className, chargeClassID, ChargeClassBPJSCode, chargeClassCode, chargeClassName, bedID, bedCode) {

            $('#<%=hdnToClassID.ClientID %>').val(classID);
            $('#<%=txtToClassCode.ClientID %>').val(classCode);
            $('#<%=txtToClassName.ClientID %>').val(className);

            if ('<%=GetIsMoveClassTransfer() %>' != 'True' || '<%=GetIsICUClass() %>' != classID) {
                $('#<%=hdnToChargeClassID.ClientID %>').val(chargeClassID);
                $('#<%=txtToChargeClassCode.ClientID %>').val(chargeClassCode);
                $('#<%=txtToChargeClassName.ClientID %>').val(chargeClassName);
            }
            else {
                $('#<%=hdnToChargeClassID.ClientID %>').val($('#<%=hdnFromClassID.ClientID %>').val());
                $('#<%=txtToChargeClassCode.ClientID %>').val($('#<%=txtFromClassCode.ClientID %>').val());
                $('#<%=txtToChargeClassName.ClientID %>').val($('#<%=txtFromClassName.ClientID %>').val());
            }

            $('#<%=hdnToServiceUnitID.ClientID %>').val(healthcareServiceUnitID);
            $('#<%=txtToServiceUnitCode.ClientID %>').val(serviceUnitCode);
            $('#<%=txtToServiceUnitName.ClientID %>').val(serviceUnitName);

            $('#<%=hdnToRoomID.ClientID %>').val(roomID);
            $('#<%=txtToRoomCode.ClientID %>').val(roomCode);
            $('#<%=txtToRoomName.ClientID %>').val(roomName);

            $('#<%=hdnToBedID.ClientID %>').val(bedID);
            $('#<%=txtToBedCode.ClientID %>').val(bedCode);
        }

        //#region Physician
        $('#<%=lblToPhysician.ClientID %>.lblLink').live('click', function () {
            var serviceUnitID = $('#<%=hdnToServiceUnitID.ClientID %>').val();
            var filterExpression = '';
            if (serviceUnitID != '') {
                if ($('#<%=txtToRoomCode.ClientID %>').val() == '1')
                    filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
            }
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtToPhysicianCode.ClientID %>').val(value);
                ontxtToPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtToPhysicianCode.ClientID %>').live('change', function () {
            ontxtToPhysicianCodeChanged($(this).val());
        });

        function ontxtToPhysicianCodeChanged(value) {
            var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    cboToSpecialty.SetValue(result.SpecialtyID);
                    $('#<%=hdnToParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtToPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    cboToSpecialty.SetValue('');
                    $('#<%=hdnToParamedicID.ClientID %>').val('');
                    $('#<%=txtToPhysicianCode.ClientID %>').val('');
                    $('#<%=txtToPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Class Care
        $('#<%=lblToClass.ClientID %>.lblLink').live('click', function () {
            var serviceUnitID = $('#<%=hdnToServiceUnitID.ClientID %>').val();
            var filterExpression = 'IsDeleted = 0';
            if (serviceUnitID != '')
                filterExpression = 'ClassID IN (SELECT ClassID FROM vServiceUnitRoom WHERE HealthcareServiceUnitID = ' + serviceUnitID + ' AND IsDeleted = 0) AND IsDeleted = 0';

            openSearchDialog('classcare', filterExpression, function (value) {
                $('#<%=txtToClassCode.ClientID %>').val(value);
                ontxtToClassCodeChanged(value);
            });
        });

        $('#<%=txtToClassCode.ClientID %>').live('change', function () {
            ontxtToClassCodeChanged($(this).val());
        });

        function ontxtToClassCodeChanged(value) {
            var filterExpression = "ClassCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnToClassID.ClientID %>').val(result.ClassID);
                    $('#<%=txtToClassName.ClientID %>').val(result.ClassName);

                    if (result.IsUsedInChargeClass) {
                        $('#<%=hdnToChargeClassID.ClientID %>').val(result.ClassID);
                        $('#<%=txtToChargeClassCode.ClientID %>').val(result.ClassCode);
                        $('#<%=txtToChargeClassName.ClientID %>').val(result.ClassName);
                    }
                    else {
                        $('#<%=hdnToChargeClassID.ClientID %>').val('');
                        $('#<%=txtToChargeClassCode.ClientID %>').val('');
                        $('#<%=txtToChargeClassName.ClientID %>').val('');
                    }
                }
                else {
                    $('#<%=hdnToClassID.ClientID %>').val('');
                    $('#<%=txtToClassCode.ClientID %>').val('');
                    $('#<%=txtToClassName.ClientID %>').val('');

                    $('#<%=hdnToChargeClassID.ClientID %>').val('');
                    $('#<%=txtToChargeClassCode.ClientID %>').val('');
                    $('#<%=txtToChargeClassName.ClientID %>').val('');
                }
                if ($('#<%=hdnToServiceUnitID.ClientID %>').val() != '') {
                    $('#<%=hdnToServiceUnitID.ClientID %>').val('');
                    $('#<%=txtToServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtToServiceUnitName.ClientID %>').val('');
                    $('#<%=hdnToIsServiceUnitHasParamedic.ClientID %>').val('0');
                }
                $('#<%=hdnToRoomID.ClientID %>').val('');
                $('#<%=txtToRoomCode.ClientID %>').val('');
                $('#<%=txtToRoomName.ClientID %>').val('');
            });
        }
        //#endregion

        //#region Charge Class
        function onGetChargeClassFilterExpression() {
            return "IsUsedInChargeClass = 1 AND IsDeleted = 0"
        }

        $('#<%=lblToChargeClass.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('classcare', onGetChargeClassFilterExpression(), function (value) {
                $('#<%=txtToChargeClassCode.ClientID %>').val(value);
                ontxtToChargeClassCodeChanged(value);
            });
        });

        $('#<%=txtToChargeClassCode.ClientID %>').live('change', function () {
            ontxtToChargeClassCodeChanged($(this).val());
        });

        function ontxtToChargeClassCodeChanged(value) {
            var filterExpression = onGetChargeClassFilterExpression() + " AND ClassCode = '" + value + "'";
            Methods.getObject('GetClassCareList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnToChargeClassID.ClientID %>').val(result.ClassID);
                    $('#<%=txtToChargeClassName.ClientID %>').val(result.ClassName);
                }
                else {
                    $('#<%=hdnToChargeClassID.ClientID %>').val('');
                    $('#<%=txtToChargeClassCode.ClientID %>').val('');
                    $('#<%=txtToChargeClassName.ClientID %>').val('');
                }
            });
        }
        //#endregion

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
            var filterExpression = '';
            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
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

                            $('#<%=hdnToChargeClassID.ClientID %>').val(result.ClassID);
                            $('#<%=txtToChargeClassCode.ClientID %>').val(result.ClassCode);
                            $('#<%=txtToChargeClassName.ClientID %>').val(result.ClassName);
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


        //#region List
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.focus').removeClass('focus');
                $(this).addClass('focus');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 2)
                idx = 2;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.focus');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }
        //#endregion

        var isAdd = false;
        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                isAdd = false;
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);

                cboTransferType.SetValue(entity.GCPatientTransferType);
                cboToSpecialty.SetValue(entity.ToSpecialtyID);

                $('#<%=hdnToParamedicID.ClientID %>').val(entity.ToParamedicID);
                $('#<%=txtToPhysicianCode.ClientID %>').val(entity.ToParamedicCode);
                $('#<%=txtToPhysicianName.ClientID %>').val(entity.ToParamedicName);

                $('#<%=hdnToClassID.ClientID %>').val(entity.ToClassID);
                $('#<%=txtToClassCode.ClientID %>').val(entity.ToClassCode);
                $('#<%=txtToClassName.ClientID %>').val(entity.ToClassName);

                $('#<%=hdnToRoomID.ClientID %>').val(entity.ToRoomID);
                $('#<%=txtToRoomCode.ClientID %>').val(entity.ToRoomCode);
                $('#<%=txtToRoomName.ClientID %>').val(entity.ToRoomName);

                $('#<%=hdnToChargeClassID.ClientID %>').val(entity.ToChargeClassID);
                $('#<%=txtToChargeClassCode.ClientID %>').val(entity.ToChargeClassCode);
                $('#<%=txtToChargeClassName.ClientID %>').val(entity.ToChargeClassName);

                $('#<%=hdnToBedID.ClientID %>').val(entity.ToBedID);
                $('#<%=txtToBedCode.ClientID %>').val(entity.ToBedCode);

                $('#<%=hdnToServiceUnitID.ClientID %>').val(entity.ToHealthcareServiceUnitID);
                $('#<%=txtToServiceUnitCode.ClientID %>').val(entity.ToServiceUnitCode);
                $('#<%=txtToServiceUnitName.ClientID %>').val(entity.ToServiceUnitName);

                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
                $('#<%=txtTransferDate.ClientID %>').val(entity.TransferDateInDatePickerFormat);
                $('#<%=txtTransferTime.ClientID %>').val(entity.TransferTime);
            }
            else {
                isAdd = true;
                $('#<%=hdnEntryID.ClientID %>').val('');

                $('#<%=txtTransferDate.ClientID %>').val($('#<%=hdnDefaultDate.ClientID %>').val());

                $('#<%=hdnToParamedicID.ClientID %>').val($('#<%=hdnFromParamedicID.ClientID %>').val());
                $('#<%=txtToPhysicianCode.ClientID %>').val($('#<%=txtFromPhysicianCode.ClientID %>').val());
                $('#<%=txtToPhysicianName.ClientID %>').val($('#<%=txtFromPhysicianName.ClientID %>').val());
                cboToSpecialty.SetValue($('#<%=hdnFromSpecialtyID.ClientID %>').val());

                $('#<%=hdnToClassID.ClientID %>').val('');
                $('#<%=txtToClassCode.ClientID %>').val('');
                $('#<%=txtToClassName.ClientID %>').val('');

                $('#<%=hdnToRoomID.ClientID %>').val('');
                $('#<%=txtToRoomCode.ClientID %>').val('');
                $('#<%=txtToRoomName.ClientID %>').val('');

                if ('<%=GetIsMoveClassTransfer() %>' != 'True') {
                    $('#<%=hdnToChargeClassID.ClientID %>').val('');
                    $('#<%=txtToChargeClassCode.ClientID %>').val('');
                    $('#<%=txtToChargeClassName.ClientID %>').val('');
                }
                else {
                    $('#<%=hdnToChargeClassID.ClientID %>').val($('#<%=hdnFromChargeClassID.ClientID %>').val());
                    $('#<%=txtToChargeClassCode.ClientID %>').val($('#<%=txtFromChargeClassCode.ClientID %>').val());
                    $('#<%=txtToChargeClassName.ClientID %>').val($('#<%=txtFromChargeClassName.ClientID %>').val());
                }

                $('#<%=hdnToBedID.ClientID %>').val('');
                $('#<%=txtToBedCode.ClientID %>').val('');

                $('#<%=hdnToServiceUnitID.ClientID %>').val('');
                $('#<%=txtToServiceUnitCode.ClientID %>').val('');
                $('#<%=txtToServiceUnitName.ClientID %>').val('');

                $('#<%=txtRemarks.ClientID %>').val('');
            }
        }
        //#endregion

        function onBeforeEditRecord(selectedObj, errMessage) {
            if (selectedObj.IsTransferred == 'True') {
                errMessage.text = "This Data Has Been Approved.";
                return false;
            }
            return true;
        }

        function onBeforeDeleteRecord(selectedObj, errMessage) {
            if (selectedObj.IsTransferred == 'True') {
                errMessage.text = "This Data Has Been Approved.";
                return false;
            }
            return true;
        }

        function onAfterSaveRecord() {
            if (isAdd)
                $('#<%=hdnIsAllowAddRecord.ClientID %>').val('0');
        }

        function onBeforeAddRecord(errMessage) {
            if ($('#<%=hdnIsAllowAddRecord.ClientID %>').val() == '0') {
                errMessage.text = "Please Transfer / Cancel All Opened Patient Transfer For This Registration First.";
                return false;
            }
            return true;
        }
        function onAfterPatientListEntryDeleteRecord() {
            $('#<%=hdnIsAllowAddRecord.ClientID %>').val('1');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var resultObj = '';
            filterObj = 'RegistrationID = '+ registrationID;
            Methods.getObject('GetvPatientTransferList', filterObj, function (result) {
                if (result != null) {
                    resultObj = '1';
                } else {
                    resultObj = '0';
                }
            });
            if (resultObj == '1') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            } else {
                errMessage.text = 'Tidak Ada Riwayat Pindah';
                return false;
            }
        }   
    </script>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnDefaultDate" runat="server" />
    <input type="hidden" id="hdnEntryID" runat="server" />
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
                        <col style="width: 150px" />
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
                <div class="lblComponent">
                    <%=GetLabel("Ke")%></div>
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td>
                            <input type="button" value="Pilih Tempat Tidur" id="btnPickBed" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblToPhysician">
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
                                        <asp:TextBox ID="txtToPhysicianCode" Width="100%" runat="server" />
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
                            <label class="lblLink lblMandatory" runat="server" id="lblToClass">
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
                                        <asp:TextBox ID="txtToClassCode" Width="100%" runat="server" />
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
                            <label class="lblLink lblMandatory" runat="server" id="lblToChargeClass">
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
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-VerticalAlign="Top">
                                    <HeaderTemplate>
                                        <%=GetLabel("Transfer Information")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="float: left">
                                            <%#: Eval("TransferDateInString")%></div>
                                        <div style="margin-left: 100px">
                                            <%#: Eval("TransferTime")%></div>
                                        <div>
                                            <asp:CheckBox ID="chkIsTransferred" Checked='<%# Eval("IsTransferred").ToString() == "True" ? true : false %>'
                                                Enabled="false" runat="server" /><%=GetLabel("Is Transferred")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="300px">
                                    <HeaderTemplate>
                                        <%=GetLabel("From")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("FromClassName")%></div>
                                        <div>
                                            <%#: Eval("FromServiceUnitName")%>
                                            |
                                            <%#: Eval("FromRoomName")%>
                                            |
                                            <%#: Eval("FromBedCode")%></div>
                                        <div>
                                            <%#: Eval("FromParamedicName")%>
                                            |
                                            <%#: Eval("FromSpecialtyName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="300px">
                                    <HeaderTemplate>
                                        <%=GetLabel("To")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("GCPatientTransferType") %>" bindingfield="GCPatientTransferType" />
                                        <input type="hidden" value="<%#:Eval("IsTransferred") %>" bindingfield="IsTransferred" />
                                        <input type="hidden" value="<%#:Eval("ToClassID") %>" bindingfield="ToClassID" />
                                        <input type="hidden" value="<%#:Eval("ToClassCode") %>" bindingfield="ToClassCode" />
                                        <input type="hidden" value="<%#:Eval("ToClassName") %>" bindingfield="ToClassName" />
                                        <input type="hidden" value="<%#:Eval("ToChargeClassID") %>" bindingfield="ToChargeClassID" />
                                        <input type="hidden" value="<%#:Eval("ToChargeClassCode") %>" bindingfield="ToChargeClassCode" />
                                        <input type="hidden" value="<%#:Eval("ToChargeClassName") %>" bindingfield="ToChargeClassName" />
                                        <input type="hidden" value="<%#:Eval("ToRoomID") %>" bindingfield="ToRoomID" />
                                        <input type="hidden" value="<%#:Eval("ToRoomCode") %>" bindingfield="ToRoomCode" />
                                        <input type="hidden" value="<%#:Eval("ToRoomName") %>" bindingfield="ToRoomName" />
                                        <input type="hidden" value="<%#:Eval("ToBedID") %>" bindingfield="ToBedID" />
                                        <input type="hidden" value="<%#:Eval("ToBedCode") %>" bindingfield="ToBedCode" />
                                        <input type="hidden" value="<%#:Eval("ToParamedicID") %>" bindingfield="ToParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ToParamedicCode") %>" bindingfield="ToParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ToParamedicName") %>" bindingfield="ToParamedicName" />
                                        <input type="hidden" value="<%#:Eval("ToHealthcareServiceUnitID") %>" bindingfield="ToHealthcareServiceUnitID" />
                                        <input type="hidden" value="<%#:Eval("ToServiceUnitCode") %>" bindingfield="ToServiceUnitCode" />
                                        <input type="hidden" value="<%#:Eval("ToServiceUnitName") %>" bindingfield="ToServiceUnitName" />
                                        <input type="hidden" value="<%#:Eval("ToSpecialtyID") %>" bindingfield="ToSpecialtyID" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                        <input type="hidden" value="<%#:Eval("TransferDateInDatePickerFormat") %>" bindingfield="TransferDateInDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("TransferTime") %>" bindingfield="TransferTime" />
                                        <div>
                                            <%#: Eval("ToClassName")%></div>
                                        <div>
                                            <%#: Eval("ToServiceUnitName")%>
                                            |
                                            <%#: Eval("ToRoomName")%>
                                            |
                                            <%#: Eval("ToBedCode")%></div>
                                        <div>
                                            <%#: Eval("ToParamedicName")%>
                                            |
                                            <%#: Eval("ToSpecialtyName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
