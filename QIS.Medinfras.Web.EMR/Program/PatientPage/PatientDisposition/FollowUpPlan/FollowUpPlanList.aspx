<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="FollowUpPlanList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.FollowUpPlanList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

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
            $('#<%=txtVisitDate.ClientID %>').datepicker('option', 'minDate', '0');
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
            cboClinic.SetFocus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.AppointmentID);
                cboClinic.SetValue(entity.HealthcareServiceUnitID);
                $('#<%=hdnPhysicianID.ClientID %>').val(entity.ParamedicID);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);
                $('#<%=txtRemarks.ClientID %>').val(entity.Notes);
                $('#<%=hdnVisitTypeID.ClientID %>').val(entity.VisitTypeID);
                $('#<%=txtVisitTypeCode.ClientID %>').val(entity.VisitTypeCode);
                $('#<%=txtVisitTypeName.ClientID %>').val(entity.VisitTypeName);
                $('#<%=txtVisitDate.ClientID %>').val(entity.StartDate);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnPhysicianID.ClientID %>').val($('#<%=hdnPhysicianID.ClientID %>').val());
                $('#<%=txtPhysicianCode.ClientID %>').val($('#<%=hdnPhysicianCode.ClientID %>').val());
                onTxtPatientVisitPhysicianCodeChanged($('#<%=hdnPhysicianCode.ClientID %>').val());
                $('#<%=txtRemarks.ClientID %>').val('');
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitTypeCode.ClientID %>').val('');
                $('#<%=txtVisitTypeName.ClientID %>').val('');
                $('#<%=txtVisitDate.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Physician
        function getPhysicianFilterExpression() {
            var polyclinicID = cboClinic.GetValue();
            var filterExpression = '';
            if (polyclinicID != '' && polyclinicID != null)
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', getPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        });

        function onTxtPatientVisitPhysicianCodeChanged(value) {
            var filterExpression = getPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Visit Type
        $('#lblVisitType.lblLink').live('click', function () {
            var polyclinicID = cboClinic.GetValue();
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            if (polyclinicID == "" || paramedicID == "") {
                openSearchDialog('visittype', '', function (value) {
                    $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                    onTxtVisitTypeCodeChanged(value);
                });
            }
            else {
                var filterExpression = "HealthcareServiceUnitID = " + polyclinicID + " AND ParamedicID = " + paramedicID;
                Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                    var filterExpression = '';
                    if (result > 0)
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + polyclinicID + " AND ParamedicID = " + paramedicID + ")";
                    else
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
                    openSearchDialog('visittype', filterExpression, function (value) {
                        $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                        onTxtVisitTypeCodeChanged(value);
                    });
                });
            }

        });

        $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var polyclinicID = cboClinic.GetValue();
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            if (polyclinicID == "" || paramedicID == "") {
                var filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                        $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                    }
                    else {
                        $('#<%=hdnVisitTypeID.ClientID %>').val('');
                        $('#<%=txtVisitTypeCode.ClientID %>').val('');
                        $('#<%=txtVisitTypeName.ClientID %>').val('');
                    }
                });
            }
            else {
                var filterExpression = "HealthcareServiceUnitID = " + polyclinicID + " AND ParamedicID = " + paramedicID;
                Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                    var filterExpression = '';
                    if (result > 0)
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + polyclinicID + " AND ParamedicID = " + paramedicID + ")";
                    else
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
                    filterExpression += " AND VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                            $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                        }
                        else {
                            $('#<%=hdnVisitTypeID.ClientID %>').val('');
                            $('#<%=txtVisitTypeCode.ClientID %>').val('');
                            $('#<%=txtVisitTypeName.ClientID %>').val('');
                        }
                    });
                });
            }
        }
        //#endregion

        //#region Paramedic Work Time
        $('#lblVisitTime.lblLink').live('click', function () {
            var visitDate = $('#<%=txtVisitDate.ClientID %>').val();
            var healthcareServiceUnitID = cboClinic.GetValue();
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();

            if (healthcareServiceUnitID != "" && paramedicID != "" && visitDate != "") {
                var date = Methods.dateToString(Methods.getDatePickerDate(visitDate));
                var day = Methods.stringToDate(date).getDay();
                if (day == 0)
                    day = 7;

                var param = healthcareServiceUnitID + ';' + paramedicID + ';' + day + ';' + visitDate + ';-1';

                openSearchDialog('paramedicworktime', param, function (value) {
                    $('#<%=txtVisitTime.ClientID %>').val(value);
                });
            }
        });
        //#endregion
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Clinic")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboClinic" ClientInstanceName="cboClinic" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPhysician"><%=GetLabel("Physician")%></label></td>
                        <td>
                            <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                            <table style="width:300px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
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
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblVisitType"><%=GetLabel("VisitType")%></label></td>
                        <td>
                            <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                            <table style="width:300px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Visit Date")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtVisitDate" CssClass="datepicker" Width="120px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory" id="lblVisitTime"><%=GetLabel("Visit Time")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtVisitTime" ReadOnly="true" CssClass="time" Width="80px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPhysicianCode" runat="server" value="" />
    <input type="hidden" id="hdnPhysicianName" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value = "" />

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
                                <asp:BoundField DataField="AppointmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("AppointmentID") %>" bindingfield="AppointmentID" />
                                        <input type="hidden" value="<%#:Eval("StartDateInDatePickerFormat") %>" bindingfield="StartDate" />
                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                        <input type="hidden" value="<%#:Eval("Notes") %>" bindingfield="Notes" />
                                        <input type="hidden" value="<%#:Eval("VisitTypeID") %>" bindingfield="VisitTypeID" />
                                        <input type="hidden" value="<%#:Eval("VisitTypeCode") %>" bindingfield="VisitTypeCode" />
                                        <input type="hidden" value="<%#:Eval("VisitTypeName") %>" bindingfield="VisitTypeName" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                         <div><%=GetLabel("Follow Up Date Time")%>, <%=GetLabel("Physician")%>, <%=GetLabel("Location")%></div>
                                         <div><%=GetLabel("VisitType")%>; <%=GetLabel("Notes")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></div>
                                        <div><%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
