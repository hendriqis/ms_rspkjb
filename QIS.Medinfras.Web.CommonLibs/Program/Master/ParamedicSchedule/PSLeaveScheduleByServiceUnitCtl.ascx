<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PSLeaveScheduleByServiceUnitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PSLeaveScheduleByServiceUnitCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PSleaveScheduleEntryCtl">
    $('#lblEntryPopupAddDataLeave').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtDateLeave.ClientID %>').val('');
        $('#<%=txtServiceUnitCode.ClientID %>').val('');
        $('#<%=txtServiceUnitName.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if ($('#<%=txtDateLeave.ClientID %>').val() == "") {
            showToast('Warning', 'Harap pilih Jadwal Cuti terlebih dahulu.');
        }
        else {
            if ($('#<%=txtServiceUnitCode.ClientID %>').val() != '') {
                if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
                    cbpEntryPopupView.PerformCallback('save');
            }
            else {
                showToast('Warning', 'Harap pilih Klinik terlebih dahulu.');
            }
        }
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var StartDateInString = $row.find('.hdnStartDateDatePicker').val();
        var EndDateInString = $row.find('.hdnEndDateDatePicker').val();
        var StartTime = $row.find('.hdnStartTime').val();
        var EndTime = $row.find('.hdnEndTime').val();
        var ParamedicLeaveReason = $row.find('.hdnParamedicLeaveReason').val();
        var Remarks = $row.find('.hdnRemarks').val();
        var IsFullDay = $row.find('.hdnIsFullDay').val();
        var ServiceUnitName = $row.find('.hdnServiceUnitName').val();
        var ServiceUnitCode = $row.find('.hdnServiceUnitCode').val();
        var ParamedicName = $row.find('.hdnParamedicName').val();
        var date = StartDateInString + " s/d " + EndDateInString;

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=txtDateLeave.ClientID %>').val(date);
        $('#<%=txtServiceUnitCode.ClientID %>').val(ServiceUnitCode);
        $('#<%=txtServiceUnitName.ClientID %>').val(ServiceUnitName);

        $('#containerPopupEntryData').show();
    });

    //#region Clinic
    $('#lblClinic.lblLink').click(function () {
        var filterExpression = "IsDeleted = 0 AND ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + " AND DepartmentID = '" + Constant.Facility.OUTPATIENT + "'";
        openSearchDialog('serviceunitbyparamedic', filterExpression, function (value) {
            $('#<%=txtServiceUnitCode.ClientID %>').val(value);
            onClinicChanged(value);
        });
    });

    $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
        onClinicChanged($(this).val());
    });

    function onClinicChanged(value) {
        var id = 0;
        if (value != "") {
            id = value;
        }
        else {
            value = 0;
        }
        var filterExpression = "HealthcareServiceUnitID = " + value;
        Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
            if (value != 0 && value != "") {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
            }
            else {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=txtServiceUnitCode.ClientID %>').val('');
                $('#<%=txtServiceUnitName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paramedic Leave Schedule
    $('#lblParamedicLeaveSchedule.lblLink').click(function () {
        var filterExpression = "IsDeleted = 0 AND ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + " AND (StartDate >= CONVERT(VARCHAR, GETDATE(), 112) AND EndDate >= CONVERT(VARCHAR, GETDATE(), 112))";
        openSearchDialog('paramedicleaveschedule', filterExpression, function (value) {
            $('#<%=hdnParamedicLeaveScheduleID.ClientID %>').val(value);
            onTxtParamedicLeaveChanged(value);
        });
    });

    $('#<%=hdnParamedicLeaveScheduleID.ClientID %>').change(function () {
        onTxtParamedicLeaveChanged($(this).val());
    });

    function onTxtParamedicLeaveChanged(value) {
        var filterExpression = "IsDeleted = 0 AND ID ='" + value + "'";
        Methods.getObject('GetvParamedicLeaveSchedule1List', filterExpression, function (result) {
            if (result != null) {
                var date = result.cfStartDateTimeInString + ' s/d ' + result.cfEndDateTimeInString;
                $('#<%=hdnParamedicLeaveScheduleID.ClientID %>').val(result.ID);
                $('#<%=txtDateLeave.ClientID %>').val(date);
            }
            else {
                $('#<%=hdnParamedicLeaveScheduleID.ClientID %>').val('');
                $('#<%=txtDateLeave.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                $('#containerPopupEntryData').hide();
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        $('#containerImgLoadingView').hide();
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicIDSearchDialog" value="" runat="server" />
    <input type="hidden" id="hdnOperationalTimeID" value="" runat="server" />
    <input type="hidden" id="hdnDatePickerToday" value="" runat="server" />
    <input type="hidden" id="hdnBeforeDatePicker" value="" runat="server" />
    <input type="hidden" id="hdnIsFullDay" value="" runat="server"/>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td colspan="2">
                            <asp:textbox id="txtParamedicName" readonly="true" width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:textbox id="txtHealthcareName" readonly="true" width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblParamedicLeaveSchedule">
                                        <%=GetLabel("Jadwal Cuti") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50%" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnParamedicLeaveScheduleID" value="" runat="server"/>
                                                <asp:TextBox runat="server" ID="txtDateLeave" Width="400px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <label class="lblLink lblMandatory" id="lblClinic">
                                        <%=GetLabel("Klinik") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtServiceUnitCode" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtServiceUnitName" Width="250px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:panel runat="server" id="pnlEntryPopupGrdView" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:gridview id="grdView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                    onrowdatabound="grdView_RowDataBound" showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                    <columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnStartDateDatePicker" value="<%#: Eval("cfStartDateTimeInString")%>" />
                                                <input type="hidden" class="hdnEndDateDatePicker" value="<%#: Eval("cfEndDateTimeInString")%>" />
                                                <input type="hidden" class="hdnStartTime" value="<%#: Eval("StartTime")%>" />
                                                <input type="hidden" class="hdnEndTime" value="<%#: Eval("EndTime")%>" />
                                                <input type="hidden" class="hdnParamedicLeaveReason" value="<%#: Eval("GCParamedicLeaveReason")%>" />
                                                <input type="hidden" class="hdnRemarks" value="<%#: Eval("Remarks")%>" />
                                                <input type="hidden" class="hdnIsFullDay" value="<%#: Eval("IsFullDay")%>" />
                                                <input type="hidden" class="hdnServiceUnitName" value="<%#: Eval("ServiceUnitName")%>" />
                                                <input type="hidden" class="hdnServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                <input type="hidden" class="hdnParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfStartDateTimeInString" ItemStyle-CssClass="tdStartDate" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="cfEndDateTimeInString" ItemStyle-CssClass="tdEndDate" HeaderText="End Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="ParamedicLeaveReason" ItemStyle-CssClass="tdLeaveReason" HeaderText="Leave Reason" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="ServiceUnitName" ItemStyle-CssClass="tdServiceUnitName" HeaderText="Clinic" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="Remarks" ItemStyle-CssClass="tdRemarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                                    </columns>
                                    <emptydatatemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </emptydatatemplate>
                                </asp:gridview>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddDataLeave">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
