<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PSleaveScheduleEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.PSleaveScheduleEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PSleaveScheduleEntryCtl">
    setDatePicker('<%=txtStartDate.ClientID %>');
    $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', '1000');
    $('#<%=txtStartDate.ClientID %>').datepicker('option', 'minDate', '0');

    setDatePicker('<%=txtEndDate.ClientID %>');
    $('#<%=txtEndDate.ClientID %>').datepicker('option', 'maxDate', '1000');
    $('#<%=txtEndDate.ClientID %>').datepicker('option', 'minDate', '0');

    $('#lblEntryPopupAddDataLeave').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtStartDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
        $('#<%=txtStartTime.ClientID %>').val('00:00');
        $('#<%=txtEndDate.ClientID %>').val($('#<%=hdnDatePickerToday.ClientID %>').val());
        $('#<%=txtEndTime.ClientID %>').val('23:59');
        $('#<%=txtOtherLeaveReason.ClientID %>').val('');
        $('#<%=txtRemarks.ClientID %>').val('');
        $('#<%=chkIsFullDay.ClientID %>').attr('checked', true);
        $('#<%=txtStartTime.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtEndTime.ClientID %>').attr('readonly', 'readonly');

        $('#containerPopupEntryData').show();
    });

//    $('#btnEntryPopupCancel').live('click', function () {
//        $('#containerPopupEntryData').hide();
//    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
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
    //#region Function CheckBox
    $('#<%=chkIsFullDay.ClientID %>').change(function () {
        if ($('#<%=chkIsFullDay.ClientID %>').is(':checked')) {
            $('#<%=txtStartTime.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtEndTime.ClientID %>').attr('readonly', 'readonly');

            $('#<%=txtEndTime.ClientID %>').val('23:59');
            $('#<%=txtStartTime.ClientID %>').val('00:00');
        }
        else {
            $('#<%=txtStartTime.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtEndTime.ClientID %>').removeAttr('readonly', 'readonly');

            $('#<%=txtStartTime.ClientID %>').val('');
            $('#<%=txtEndTime.ClientID %>').val('');
        }
    });
    //#endregion



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

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=txtStartDate.ClientID %>').val(StartDateInString);
        $('#<%=txtEndDate.ClientID %>').val(EndDateInString);
        $('#<%=txtStartTime.ClientID %>').val(StartTime);
        $('#<%=txtEndTime.ClientID %>').val(EndTime);
        cboLeaveReason.SetValue(ParamedicLeaveReason);
        $('#<%=txtRemarks.ClientID %>').val(Remarks);
        $('#containerPopupEntryData').show();


        if (IsFullDay == 'True') {
            $('#<%=chkIsFullDay.ClientID %>').prop('checked', IsFullDay);

            $('#<%=txtStartTime.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtEndTime.ClientID %>').attr('readonly', 'readonly');

            $('#<%=txtEndTime.ClientID %>').val('23:59');
            $('#<%=txtStartTime.ClientID %>').val('00:00');
        } else {
            $('#<%=chkIsFullDay.ClientID %>').prop('checked', false);

            $('#<%=txtStartTime.ClientID %>').removeAttr('readonly', 'readonly');
            $('#<%=txtEndTime.ClientID %>').removeAttr('readonly', 'readonly');

            $('#<%=txtStartTime.ClientID %>').val(StartTime);
            $('#<%=txtEndTime.ClientID %>').val(EndTime);
        }

    });

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

    function oncboLeaveReasonChanged() {
        if (cboLeaveReason.GetValue() != 'X281^999')
            $('#<%=trOtherReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trOtherReason.ClientID %>').removeAttr('style');
    }

    function onchkIsFullDayChange() {

    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnOperationalTimeID" value="" runat="server" />
    <input type="hidden" id="hdnDatePickerToday" value="" runat="server" />
    <input type="hidden" id="hdnBeforeDatePicker" value="" runat="server" />
    <input type="hidden" id="hdnIsFullDay" value="" runat="server"/>
    <input type="hidden" id="hdnServiceUnitID" value="" runat="server"/>
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Klinik")%></label>
                        </td>
                        <td colspan="2">
                            <asp:textbox id="txtServiceUnit" readonly="true" width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <input type="hidden" id="hdnStartDateDatePicker" runat="server" value="" />
                    <input type="hidden" id="hdnEndDateDatePicker" runat="server" value="" />
                    <input type="hidden" id="hdnStartTime" runat="server" value="" />
                    <input type="hidden" id="hdnEndTime" runat="server" value="" />
                    <input type="hidden" id="hdnRemarks" runat="server" value="" />
                    <input type="hidden" id="hdnIsBridgingToGateway" runat="server" value="" />
                    <input type="hidden" id="hdnProviderGatewayService" runat="server" value="" />
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
                                <td class="tdLabel">
                                    <label class="lblMandatory" id="lblStartDate">
                                        <%=GetLabel("Start Date")%></label>
                                </td>
                                <td>
                                    <asp:textbox id="txtStartDate" width="125px" runat="server" cssclass="datepicker" />
                                </td>
                                <td>
                                    <asp:textbox id="txtStartTime" width="50px" maxlength="5" runat="server" cssclass="time"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">

                                    <label class="lblMandatory" id="lblEndDate">
                                        <%=GetLabel("End Date")%></label>
                                </td>
                                <td>
                                    <asp:textbox id="txtEndDate" width="125px" runat="server" cssclass="datepicker" />
                                </td>
                                <td>
                                    <asp:textbox id="txtEndTime" width="50px" maxlength="5" runat="server" cssclass="time"  />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory" id="lblParamedicLeaveReason">
                                        <%=GetLabel("Leave Reason")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboLeaveReason" ClientInstanceName="cboLeaveReason"
                                        Width="180px">
                                        <ClientSideEvents ValueChanged="function(s,e){ oncboLeaveReasonChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <table>
                                        <colgroup>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsFullDay" Width="20px" runat="server" />
                                            </td>
                                            <td class="tdLabel">
                                                <label class="lblIsFullDay" id="lblIsFullDay">
                                                <%=GetLabel("Is Full Day")%></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trOtherReason" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblNormal" id="lblOtherReason">
                                        <%=GetLabel("Reason")%></label>
                                </td>
                                <td>
                                    <asp:textbox id="txtOtherLeaveReason" width="180px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="lblRemarks">
                                        <%=GetLabel("Remarks")%></label>
                                </td>
                                <td>
                                    <asp:textbox id="txtRemarks" height="50px" textmode="MultiLine" width="180px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <%--<td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>--%>
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
                                                <input type="hidden" class="hdnStartDateDatePicker" value="<%#: Eval("cfStartDateInStringDatePicker")%>" />
                                                <input type="hidden" class="hdnEndDateDatePicker" value="<%#: Eval("cfEndDateInStringDatePicker")%>" />
                                                <input type="hidden" class="hdnStartTime" value="<%#: Eval("StartTime")%>" />
                                                <input type="hidden" class="hdnEndTime" value="<%#: Eval("EndTime")%>" />
                                                <input type="hidden" class="hdnParamedicLeaveReason" value="<%#: Eval("GCParamedicLeaveReason")%>" />
                                                <input type="hidden" class="hdnRemarks" value="<%#: Eval("Remarks")%>" />
                                                <input type="hidden" class="hdnIsFullDay" value="<%#: Eval("IsFullDay")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfStartDateTimeInString" ItemStyle-CssClass="tdStartDate" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="cfEndDateTimeInString" ItemStyle-CssClass="tdEndDate" HeaderText="End Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="ParamedicLeaveReason" ItemStyle-CssClass="tdLeaveReason" HeaderText="Leave Reason" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
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
