<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionPageTestOrderOK.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageTestOrderOK" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnOperatingRoomID" runat="server" />
    <input type="hidden" value="" id="hdnIPAddress" runat="server" />
    <input type="hidden" value="6000" id="hdnPort" runat="server" />
    <input type="hidden" value="" id="hdnIsBPJSRegistration" runat="server" />
    <input type="hidden" value="" id="hdnIsOnlyBPJSItem" runat="server" />
</asp:Content>
<%--<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterSaveAddRecordEntryPopup(param) {
            var transactionID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (transactionID == '' || transactionID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(testOrderID) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnTestOrderID.ClientID %>').val(testOrderID);
                var filterExpression = 'TestOrderID = ' + testOrderID;
                Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                    $('#<%=txtTestOderNo.ClientID %>').val(result.TestOrderNo).trigger('change');
                });

                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            if ($('#<%=txtTestOrderDate.ClientID %>').attr('readonly') == null) {
                setDatePicker('<%=txtTestOrderDate.ClientID %>');
                $('#<%=txtTestOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
                setDatePicker('<%=txtScheduledDate.ClientID %>');
                $('#<%=txtScheduledDate.ClientID %>').datepicker('option', 'minDate', '0');

                $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
                $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');
            }
        }

        function onCboToBePerformedChanged() {
            if (cboToBePerformed.GetValue() != null && (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED || cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)) {
                if (cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED || cboToBePerformed.GetValue() == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT) {
                    $('#<%=txtScheduledDate.ClientID %>').removeAttr('readonly');
                    $('#<%=txtScheduledDate.ClientID %>').datepicker('enable');
                    $('#<%=txtScheduledTime.ClientID %>').removeAttr('readonly');

                    if (cboToBePerformed.GetValue() != Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT) {
                        $('#<%:tdScheduledTime.ClientID %>').removeAttr('style');
                        $('#<%:lblPhysician.ClientID %>').removeAttr('style');
                    }
                }

                $('#<%=chkIsCITO.ClientID %>').attr('disabled', true);
                $('#<%=chkIsCITO.ClientID %>').prop("checked", false);
            }
            else {
                $('#<%=txtScheduledDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
                $('#<%=txtScheduledTime.ClientID %>').val($('#<%=hdnTimeToday.ClientID %>').val());
                $('#<%:tdScheduledTime.ClientID %>').removeAttr('style');
                $('#<%=txtScheduledDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtScheduledDate.ClientID %>').datepicker('disable');
                $('#<%=txtScheduledTime.ClientID %>').attr('readonly', 'readonly');

                $('#<%=chkIsCITO.ClientID %>').removeAttr('disabled');
                $('#<%:lblPhysician.ClientID %>').removeAttr('style');
            }
        }

        //#region Service Unit
        function getServiceUnitFilterFilterExpression() {
            var filterExpression = "<%:GetServiceUnitFilterFilterExpression() %>";
            return filterExpression;
        }
        $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
            if ($('#<%=txtTestOderNo.ClientID %>').val() != '')
                return;
            openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = " ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    if (result.IsLaboratoryUnit) {
                        $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("1");
                    } else {
                        $('#<%=hdnIsLaboratoryUnit.ClientID %>').val("0");
                    }
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Transaction No
        $('#lblTestOrderNo.lblLink').click(function () {
            var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
            openSearchDialog('testorderhd', filterExpression, function (value) {
                $('#<%=txtTestOderNo.ClientID %>').val(value);
                onTxtTestOrderNoChanged(value);
            });
        });

        $('#<%=txtTestOderNo.ClientID %>').change(function () {
            onTxtTestOrderNoChanged($(this).val());
        });

        function onTxtTestOrderNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region Procedure Group
        $('#<%=lblProcedureGroup.ClientID %>.lblLink').live('click', function () {
            var filterExpression = 'IsDeleted = 0';
            openSearchDialog('proceduregroup', filterExpression, function (value) {
                $('#<%=txtProcedureGroupCode.ClientID %>').val(value);
                onTxtProcedureGroupCodeChanged(value);
            });
        });

        $('#<%=txtProcedureGroupCode.ClientID %>').change(function () {
            onTxtProcedureGroupCodeChanged($(this).val());
        });

        function onTxtProcedureGroupCodeChanged(value) {
            var filterExpression = "ProcedureGroupCode = '" + value + "'";
            Methods.getObject('GetProcedureGroupList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProcedureGroupID.ClientID %>').val(result.ProcedureGroupID);
                    $('#<%=txtProcedureGroupName.ClientID %>').val(result.ProcedureGroupName);
                }
                else {
                    $('#<%=hdnProcedureGroupID.ClientID %>').val('');
                    $('#<%=txtProcedureGroupCode.ClientID %>').val('');
                    $('#<%=txtProcedureGroupName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Physician
        $('#<%=lblPhysician.ClientID %>.lblLink').click(function () {
            var filterExpression = 'IsDeleted = 0';
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').change(function () {
            onTxtPhysicianCodeChanged($(this).val());
        });
        //#endregion

        function onGetScheduleFilterExpression() {
            var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var orderID = $('#<%=hdnTestOrderID.ClientID %>').val();
            if (serviceUnitID == operatingRoomID) {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var scheduleDate = $('#<%=txtScheduledDate.ClientID %>').val();
                var scheduleDateInDatePicker = Methods.getDatePickerDate(scheduleDate);
                var scheduleDateFormatString = Methods.dateToString(scheduleDateInDatePicker);
                var filterExpression = "VisitID = " + visitID + " AND ScheduledDate = '" + scheduleDateFormatString + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + orderID + "'";
                return filterExpression;
            }
        }

        function onBeforeProposeRecord(errMessage) {
            var resultFinal = true;
            var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == operatingRoomID) {
                var filterExpression = onGetScheduleFilterExpression();
                Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
                    if (result != null) {
                        errMessage.text = "Masih ada outstanding order kamar operasi, ";
                        resultFinal = false;
                    }
                });
            }
            return resultFinal;
        }
        
    </script>
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnIsNotesCopyDiagnose" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnIsLaboratoryUnit" runat="server" />
    <input type="hidden" value="" id="hdnDatePickerToday" runat="server" />
    <input type="hidden" value="" id="hdnTimeToday" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblTestOrderNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOderNo" Width="232px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                -
                                <%=GetLabel("Jam Order") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTestOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Catatan Klinis / Diagnosa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="400px" TextMode="MultiLine" Height="90px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter Pengirim Order")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblServiceUnit">
                                    <%=GetLabel("Unit Penunjang Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblProcedureGroup">
                                    <%=GetLabel("Kelompok Tindakan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProcedureGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProcedureGroupCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProcedureGroupName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Waktu Pengerjaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboToBePerformed" ClientInstanceName="cboToBePerformed"
                                    Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboToBePerformedChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Dilakukan Tanggal")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtScheduledDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td id='tdScheduledTime' runat="server">
                                            <asp:TextBox ID="txtScheduledTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td>
                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text="CITO" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
