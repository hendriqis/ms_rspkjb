<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="FollowUpPlanList-old.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.FollowUpPlanListOld" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.focus').removeClass('focus');
                $(this).addClass('focus');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtVisitDate.ClientID %>');
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
            $('#containerImgLoadingView').hide();

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
            if (idx < 1)
                idx = 1;
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

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=ddlClinic.ClientID %>').focus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.AppointmentID);
                $('#<%=ddlClinic.ClientID %>').val(entity.HealthcareServiceUnitID);
                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtRemarks.ClientID %>').val(entity.Notes);
                $('#<%=hdnVisitTypeID.ClientID %>').val(entity.VisitTypeID);
                $('#<%=txtVisitDate.ClientID %>').val(entity.StartDate);

                ledPhysician.SetValue(entity.ParamedicID);
                ledVisitType.SetValue(entity.VisitTypeID);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=ddlClinic.ClientID %>').val('');
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtRemarks.ClientID %>').val('');
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitDate.ClientID %>').val('');

                ledPhysician.SetValue('');
                ledVisitType.SetValue('');
            }
            $('#<%=ddlClinic.ClientID %>').change();
            setParamedicTime();
        }
        //#endregion

        function onLedPhysicianLostFocus(value) {
            $('#<%=hdnPhysicianID.ClientID %>').val(value);
            setParamedicTime();
        }

        function onLedVisitTypeLostFocus(value) {
            $('#<%=hdnVisitTypeID.ClientID %>').val(value);
        }

        $(function () {
            $('#<%=ddlClinic.ClientID %>').change(function () {
                var healthcareServiceUnitID = $(this).val();
                ledPhysician.SetFilterExpression("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ")");

                ledVisitType.SetFilterExpression("VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ")");
            });

            $('#<%=txtVisitDate.ClientID %>').change(function () {
                setParamedicTime();
            });

            $('#<%=ddlVisitTime.ClientID %>').change(function () {
                $('#<%=hdnVisitTime.ClientID %>').val($(this).val());
            });
        });

        var lstWorkTimes = [];
        function setParamedicTime() {
            var visitDate = $('#<%=txtVisitDate.ClientID %>').val();            
            var healthcareServiceUnitID = $('#<%=ddlClinic.ClientID %>').val();
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            $('#<%=ddlVisitTime.ClientID %>').empty();
            if (visitDate != '' && healthcareServiceUnitID != '' && paramedicID != '') {
                var date = Methods.dateToString(Methods.getDatePickerDate(visitDate));
                var day = Methods.stringToDate(date).getDay();
                if (day == 0)
                    day = 7;
                Methods.getParamedicWorkTimes(healthcareServiceUnitID, paramedicID, day, date, -1, function (result) {
                    lstWorkTimes = result.WorkTimes;
                    $('#<%=hdnVisitDuration.ClientID %>').val(result.ServiceInterval);

                    var ctr = 0;
                    var isFinish = false;
                    for (var i = 0; i < 7; ++i) {
                        var idx = i + ctr;
                        if (idx == lstWorkTimes.length) {
                            isFinish = true;
                            break;
                        }
                        $option = $("<option value=" + lstWorkTimes[idx] + ">" + lstWorkTimes[idx] + "</option>");
                        $('#<%=ddlVisitTime.ClientID %>').append($option);
                    }
                    if (!isFinish)
                        fillCboVisitTime(7);
                });
            }
        }

        function fillCboVisitTime(ctr) {
            setTimeout(function () {
                var isFinish = false;
                for (var i = 0; i < 7; ++i) {
                    var idx = i + ctr;
                    if (idx == lstWorkTimes.length) {
                        isFinish = true;
                        break;
                    }
                    $option = $("<option value=" + lstWorkTimes[idx] + ">" + lstWorkTimes[idx] + "</option>");
                    $('#<%=ddlVisitTime.ClientID %>').append($option);
                }
                if (!isFinish)
                    fillCboVisitTime(ctr + 7);
            }, 0);
        } 
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" runat="server" id="hdnVisitDuration" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:100px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Clinic")%></label></td>
                        <td><asp:DropDownList runat="server" ID="ddlClinic" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Physician")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
                            <qis:QISSearchTextBox ID="ledPhysician" ClientInstanceName="ledPhysician" runat="server" Width="300px"
                                ValueText="ParamedicID" FilterExpression="1 = 0" DisplayText="ParamedicName" MethodName="GetvParamedicMasterList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedPhysicianLostFocus(s.GetValueText()); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Physician Code" FieldName="ParamedicCode" Description="i.e. HP00001" Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Physician Name" FieldName="ParamedicName" Description="i.e. House" Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtRemarks" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:100px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visit Type")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnVisitTypeID" runat="server" />
                            <qis:QISSearchTextBox ID="ledVisitType" ClientInstanceName="ledVisitType" runat="server" Width="300px"
                                ValueText="VisitTypeID" FilterExpression="1 = 0" DisplayText="VisitTypeName" MethodName="GetVisitTypeList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedVisitTypeLostFocus(s.GetValueText()); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Visit Type Code" FieldName="VisitTypeCode" Description="i.e. VT00001" Width="100px" />
                                    <qis:QISSearchTextBoxColumn Caption="Visit Type Name" FieldName="VisitTypeName" Description="i.e. Consultation" Width="300px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visit Date")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtVisitDate" CssClass="datepicker" Width="120px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visit Time")%></label></td>
                        <td>
                            <input type="hidden" value="" id="hdnVisitTime" runat="server" />
                            <asp:DropDownList runat="server" ID="ddlVisitTime" CssClass="time" Width="80px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDate" />
                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                        <input type="hidden" value="<%#:Eval("Notes") %>" bindingfield="Notes" />
                                        <input type="hidden" value="<%#:Eval("VisitTypeID") %>" bindingfield="VisitTypeID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                         <div>Follow Up Date Time, Physician, Location</div>
                                         <div>VisitType; Notes</div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></div>
                                        <div><%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Data To Display
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
