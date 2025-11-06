<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentRescheduleV2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentRescheduleV2Ctl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_sappointmentchangedatectl">
    var numInit = 0;
    function onBeforeSaveRecord(errMessage) {
        var paramedicFrom = parseInt($('#<%=hdnParamedicIDCtlFrom.ClientID %>').val());
        var paramedicTo = parseInt($('#<%=hdnParamedicIDCtlTo.ClientID %>').val());
        if (paramedicFrom > 0 && paramedicTo > 0) {
            return true;
        }
        else {
            errMessage.text = 'Please Select Paramedic First';
            return false;
        }
    }

    function onLoad() {
        $("#calAppointmentChangeDateFrom").datepicker({
            defaultDate: Methods.getDatePickerDate($('#<%=hdnCalAppointmentSelectedDateRescheduleCtlFrom.ClientID %>').val()),
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            minDate: "0",
            onSelect: function (dateText, inst) {
                $('#<%=hdnCalAppointmentSelectedDateRescheduleCtlFrom.ClientID %>').val(dateText);
                cbpPhysicianRescheduleAppointmentFrom.PerformCallback('refresh');
                $('#<%=txtNewAppointmentDate.ClientID %>').val(dateText);
            }
        });

        $("#calAppointmentChangeDateTo").datepicker({
            defaultDate: Methods.getDatePickerDate($('#<%=hdnCalAppointmentSelectedDateRescheduleCtlTo.ClientID %>').val()),
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            minDate: "0",
            onSelect: function (dateText, inst) {
                $('#<%=hdnCalAppointmentSelectedDateRescheduleCtlTo.ClientID %>').val(dateText);
                cbpPhysicianRescheduleAppointmentTo.PerformCallback('refresh');
                $('#<%=txtNewAppointmentDateTo.ClientID %>').val(dateText);
            }
        });


        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').die('click');
        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnParamedicIDCtlFrom.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=txtNewPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());
            cboSessionRescheduleCtlFrom.PerformCallback();
        });
        $('#<%=grdPhysician.ClientID %> > tbody > tr').each(function () {
            $(this).click();
        });

        $('#<%=grdPhysicianTo.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').die('click');
        $('#<%=grdPhysicianTo.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            $('#<%=grdPhysicianTo.ClientID %> > tbody > tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnParamedicIDCtlTo.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=txtNewPhysicianTo.ClientID %>').val($(this).find('.tdParamedicName').html());
        });
        $('#<%=grdPhysicianTo.ClientID %> > tbody > tr').each(function () {
            $(this).click();
        });
    }

    function oncboServiceUnitRescheduleFromValueChanged() {
        $('#<%=txtNewServiceUnitFrom.ClientID %>').val(cboServiceUnitRescheduleFrom.GetText());
        cbpPhysicianRescheduleAppointmentFrom.PerformCallback('refresh');
    }

    function oncboServiceUnitRescheduleToValueChanged() {
        $('#<%=txtNewServiceUnitTo.ClientID %>').val(cboServiceUnitRescheduleTo.GetText());
        cbpPhysicianRescheduleAppointmentTo.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
            cbpPhysicianRescheduleAppointmentFrom.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPhysicianRescheduleAppointmentFromEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
                cbpPhysicianRescheduleAppointmentFrom.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
    }

    var pageCount1 = parseInt('<%=PageCount1 %>');
    $(function () {
        setPaging($("#pagingPhysicanChangeAppointment1"), pageCount1, function (page) {
            cbpPhysicianRescheduleAppointmentTo.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPhysicianRescheduleAppointmentToEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount1 = parseInt(param[1]);
            if (pageCount1 > 0)
                $('#<%=grdPhysicianTo.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPhysicanChangeAppointment1"), pageCount1, function (page) {
                cbpPhysicianRescheduleAppointmentTo.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPhysicianTo.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function oncboChangeReasonValueChanged() {
        if (cboChangeReason.GetValue() == Constant.DeleteReason.OTHER) {
            $('#trChangeOtherReason').removeAttr('style');
        }
        else {
            $('#trChangeOtherReason').attr('style', 'display:none');
        }
    }

    function onCboSessionRescheduleCtlFrom() {
    }
</script>
<input type="hidden" runat="server" id="hdnID" value="" />
<input type="hidden" id="hdnSelectedAppointmentID" runat="server" />
<input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
<input type="hidden" id="hdnDepartmentIDCtlReschedule" runat="server" />
<input type="hidden" id="hdnOldParamedicID" runat="server" />
<input type="hidden" id="hdnOldHealthcareServiceUnitID" runat="server" />
<input type="hidden" id="hdnMaxAppoitment" runat="server" />
<input type="hidden" id="hdnIsBridgingToGateway" runat="server" />
<input type="hidden" id="hdnChangeAppointmentCreateNewAppointment" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 50%" />
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
            <div style="height: 500px; overflow-y: scroll; overflow-x: hidden;">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <h4 class="h4expanded">
                        <%=GetLabel("From")%></h4>
                    <tr>
                        <td valign="top">
                            <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDateRescheduleCtlFrom" />
                            <div id="calAppointmentChangeDateFrom">
                            </div>
                        </td>
                        <td valign="top">
                            <dxe:ASPxComboBox ID="cboServiceUnitRescheduleFrom" ClientInstanceName="cboServiceUnitRescheduleFrom"
                                Width="100%" runat="server">
                                <ClientSideEvents Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }" ValueChanged="function(s,e) { oncboServiceUnitRescheduleFromValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                            <input type="hidden" id="hdnParamedicIDCtlFrom" runat="server" />
                            <input type="hidden" id="hdnParamedicIDCtlTo" runat="server" />
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpPhysicianRescheduleAppointmentFrom"
                                    ShowLoadingPanel="false" OnCallback="cbpPhysicianRescheduleAppointmentFrom_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                        EndCallback="function(s,e){ onCbpPhysicianRescheduleAppointmentFromEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:panel runat="server" id="pnlView2" cssclass="pnlContainerGrid" style="height: 200px">
                                                <asp:gridview id="grdPhysician" runat="server" cssclass="grdSelected" autogeneratecolumns="false"
                                                    showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                                    <columns>
                                                        <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" ItemStyle-CssClass="tdParamedicName" />
                                                    </columns>
                                                    <emptydatatemplate>
                                                        <%=GetLabel("No Data To Display")%>
                                                    </emptydatatemplate>
                                                </asp:gridview>
                                            </asp:panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingPhysicanChangeAppointment">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="position: relative;">
                                <h4>
                                    <%=GetLabel("Slot Time") %></h4>
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 20%" />
                                        <col style="width: 140px" />
                                        <col style="width: 30px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Date / Time")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtNewAppointmentDate" readonly="true" width="120px" runat="server"
                                                cssclass="datepicker" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Clinic")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:textbox id="txtNewServiceUnitFrom" readonly="true" width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Physician")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:textbox id="txtNewPhysician" readonly="true" width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td class="tdLabel">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:checkbox id="ChkIsByTimeSlotFrom" runat="server" style="margin-left: 5px" /><%:GetLabel("Is By Time Slot ?")%>
                                        </td>
                                    </tr>
                                    <tr id="trTimeSlotCtlFrom" runat="server">
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Session") %></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboSessionRescheduleCtlFrom" ClientInstanceName="cboSessionRescheduleCtlFrom"
                                                runat="server" Width="50%" OnCallback="cboSessionRescheduleCtlFrom_Callback">
                                                <ClientSideEvents EndCallback="onCboSessionRescheduleCtlFrom" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td style="padding: 5px; vertical-align: top">
            <h4 class="h4expanded">
                <%=GetLabel("To")%></h4>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 100px" />
                    <col />
                </colgroup>
                <tr>
                    <td valign="top">
                        <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDateRescheduleCtlTo" />
                        <div id="calAppointmentChangeDateTo">
                        </div>
                    </td>
                    <td valign="top">
                        <dxe:ASPxComboBox ID="cboServiceUnitRescheduleTo" ClientInstanceName="cboServiceUnitRescheduleTo"
                            Width="100%" runat="server">
                            <ClientSideEvents Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }" ValueChanged="function(s,e) { oncboServiceUnitRescheduleToValueChanged(s); }" />
                        </dxe:ASPxComboBox>
                        <input type="hidden" id="Hidden2" runat="server" />
                        <div style="position: relative;">
                            <dxcp:ASPxCallbackPanel ID="cbpPhysicianRescheduleAppointmentTo" runat="server" Width="100%"
                                ClientInstanceName="cbpPhysicianRescheduleAppointmentTo" ShowLoadingPanel="false"
                                OnCallback="cbpPhysicianRescheduleAppointmentTo_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                    EndCallback="function(s,e){ onCbpPhysicianRescheduleAppointmentToEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:panel runat="server" id="pnlView3" cssclass="pnlContainerGrid" style="height: 200px">
                                            <asp:gridview id="grdPhysicianTo" runat="server" cssclass="grdSelected" autogeneratecolumns="false"
                                                showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                                <columns>
                                                        <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" ItemStyle-CssClass="tdParamedicName" />
                                                    </columns>
                                                <emptydatatemplate>
                                                        <%=GetLabel("No Data To Display")%>
                                                    </emptydatatemplate>
                                            </asp:gridview>
                                        </asp:panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView" id="containerImgLoadingView2">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingPhysicanChangeAppointment1">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <h4>
                <%=GetLabel("Slot Time") %></h4>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 140px" />
                    <col style="width: 30px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory">
                            <%=GetLabel("Date / Time")%></label>
                    </td>
                    <td>
                        <asp:textbox id="txtNewAppointmentDateTo" readonly="true" width="120px" runat="server"
                            cssclass="datepicker" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Clinic")%></label>
                    </td>
                    <td colspan="3">
                        <asp:textbox id="txtNewServiceUnitTo" readonly="true" width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Physician")%></label>
                    </td>
                    <td colspan="3">
                        <asp:textbox id="txtNewPhysicianTo" readonly="true" width="100%" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
