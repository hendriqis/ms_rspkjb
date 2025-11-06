<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="Appointment.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.Appointment" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#calAppointment").datepicker({
                defaultDate: "w",
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd-mm-yy",
                minDate: "0",
                onSelect: function (dateText, inst) {
                    $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                    cbpPhysician.PerformCallback('refresh');
                    $('#<%=txtAppointmentDate.ClientID %>').val(dateText);
                }
            });

            $('#<%=chkIsNewPatient.ClientID %>').change(function () {
                if ($(this).is(":checked")) {
                    $('#lblMRN').attr('class', 'lblDisabled');
                    $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtFirstName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMiddleName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtFamilyName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtAddress.ClientID %>').removeAttr('readonly');
                    cboSalutation.SetEnabled(true);
                }
                else {
                    $('#lblMRN').attr('class', 'lblLink');
                    $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                    $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                    cboSalutation.SetEnabled(false);
                }

                $('#<%=hdnMRN.ClientID %>').val('');
                $('#<%=txtMRN.ClientID %>').val('');
                cboSalutation.SetValue('');
                $('#<%=txtFirstName.ClientID %>').val('');
                $('#<%=txtMiddleName.ClientID %>').val('');
                $('#<%=txtFamilyName.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtPhoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
            });
            $('#<%=chkIsNewPatient.ClientID %>').change();

            $('#<%=grdAppointment.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function (evt) {
                //var $cell = $(evt.target).closest('td');
                //if (isClickAfterEndCallback || $cell.index() > 1) {
                $('#<%=grdAppointment.ClientID %> > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=txtAppointmentHour.ClientID %>').val($(this).find('.tdTime').html());
                //}
            });

            $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
                $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=txtPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());

                cbpAppointment.PerformCallback();
            });
            $('#<%=grdPhysician.ClientID %> > tbody > tr:eq(1)').click();

            $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
                $imgExpand = $(this).find('img');
                if ($imgExpand.is(":visible")) {
                    var id = $(this).parent().find('.keyField').html();

                    $hdnIsExpand = $(this).find('.hdnIsExpand');
                    var isVisible = true;
                    if ($hdnIsExpand.val() == '0') {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                        $hdnIsExpand.val('1');
                        isVisible = false;
                    }
                    else {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $hdnIsExpand.val('0');
                        isVisible = true;
                    }

                    $('#<%=grdAppointment.ClientID %> input[parentid=' + id + ']').each(function () {
                        if (!isVisible)
                            $(this).closest('tr').show('slow');
                        else
                            $(this).closest('tr').hide('fast');
                    });
                }
            });
        });

        function onCbpAppointmentEndCallback() {
            $('#<%=grdAppointment.ClientID %> tr:eq(1)').click();
            hideLoadingPanel();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingPhysican"), pageCount, function (page) {
                cbpPhysician.PerformCallback('changepage|' + page);
            });
        });

        function onCbpPhysicianEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpPhysician.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region MRN
        $('#lblMRN.lblLink').live('click', function () {
            openSearchDialog('patient', '', function (value) {
                $('#<%=txtMRN.ClientID %>').val(value);
                onTxtMRNChanged(value);
            });
        });
        $('#<%=txtMRN.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val());
        });
        function onTxtMRNChanged(value) {
            var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                    cboSalutation.SetValue(result.Salutation);
                    $('#<%=txtFirstName.ClientID %>').val(result.FirstName);
                    $('#<%=txtMiddleName.ClientID %>').val(result.MiddleName);
                    $('#<%=txtFamilyName.ClientID %>').val(result.LastName);
                    $('#<%=txtAddress.ClientID %>').val(result.HomeAddress);
                    $('#<%=txtPhoneNo.ClientID %>').val(result.PhoneNo1);
                    $('#<%=txtMobilePhone.ClientID %>').val(result.MobilePhoneNo1);
                    $('#<%=txtEmail.ClientID %>').val(result.EmailAddress);
                }
                else {
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtMRN.ClientID %>').val('');
                    cboSalutation.SetValue('');
                    $('#<%=txtFirstName.ClientID %>').val('');
                    $('#<%=txtMiddleName.ClientID %>').val('');
                    $('#<%=txtFamilyName.ClientID %>').val('');
                    $('#<%=txtAddress.ClientID %>').val('');
                    $('#<%=txtPhoneNo.ClientID %>').val('');
                    $('#<%=txtMobilePhone.ClientID %>').val('');
                    $('#<%=txtEmail.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Visit Type
        $('#lblVisitType').live('click', function () {
            var serviceUnitID = cboServiceUnit.GetValue();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + ")";
                    openSearchDialog('visittype', filterExpression, function (value) {
                        $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                        onTxtVisitTypeCodeChanged(value);
                    });
                }
                else {
                    var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                        if (result.IsHasVisitType)
                            filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
                        else
                            filterExpression = '';
                        openSearchDialog('visittype', filterExpression, function (value) {
                            $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                            onTxtVisitTypeCodeChanged(value);
                        });
                    });
                }
            });
        });

        $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var filterExpression = '';

            var serviceUnitID = cboServiceUnit.GetValue();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression += "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + " AND VisitTypeCode = '" + value + "'";
                    Methods.getObject('GetvParamedicVisitTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                            $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                            $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                        }
                        else {
                            $('#<%=hdnVisitTypeID.ClientID %>').val('');
                            $('#<%=txtVisitTypeCode.ClientID %>').val('');
                            $('#<%=txtVisitTypeName.ClientID %>').val('');
                            $('#<%=txtVisitDuration.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                        if (result.IsHasVisitType) {
                            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND VisitTypeCode = '" + value + "'";
                            Methods.getObject('GetvServiceUnitVisitTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                            Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val('15');
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                    });
                }
            });
        }
        //#endregion

        function onCboServiceUnitValueChanged() {
            $('#<%=txtServiceUnit.ClientID %>').val(cboServiceUnit.GetText());
            cbpPhysician.PerformCallback('refresh'); 
        }  
    </script>

    <div class="pageTitle"><%=GetLabel("Patient Appointment")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;border-right: 1px solid #AAA;"> 
                <div style="height:500px; overflow-y: scroll; overflow-x: hidden;">
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:100px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                <div id="calAppointment"></div>
                            </td>
                            <td valign="top">
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                                <input type="hidden" id="hdnParamedicID" runat="server" />
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpPhysician" runat="server" Width="100%" ClientInstanceName="cbpPhysician"
                                        ShowLoadingPanel="false" OnCallback="cbpPhysician_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpPhysicianEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:200px">
                                                    <asp:GridView ID="grdPhysician" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" ItemStyle-CssClass="tdParamedicName" />
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
                                            <div id="pagingPhysican"></div>
                                        </div>
                                    </div> 
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpAppointment" runat="server" Width="100%" ClientInstanceName="cbpAppointment"
                                        ShowLoadingPanel="false" OnCallback="cbpAppointment_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                            EndCallback="function(s,e){ onCbpAppointmentEndCallback(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1">
                                                    <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />                                                        
                                                            <asp:TemplateField ItemStyle-CssClass="tdExpand" HeaderStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <img class="imgExpand" <%# Eval("ParentID").ToString() != "-1" ? "style='display:none;'" : "style='cursor:pointer'"%> src='<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>' alt='' />
                                                                    <input type="hidden" parentid='<%# Eval("ParentID")%>' />
                                                                    <input type="hidden" class="hdnIsExpand" value="1" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Time" HeaderText="Time" ItemStyle-CssClass="tdTime" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No Data To Display")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Visit Information")%></h4>
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:140px"/>
                        <col style="width:30px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblAppointmentNo"><%=GetLabel("Appointment No")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtAppointemntNo" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Appointment Date / Time")%></label></td>
                        <td><asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="120px" runat="server" CssClass="datepicker" /></td>
                        <td style="padding-left:30px;padding-right:10px"><%=GetLabel("Hour")%></td>
                        <td><asp:TextBox ID="txtAppointmentHour" ReadOnly="true" runat="server" Width="60px" CssClass="time" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Clinic")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtServiceUnit" Width="300px" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtPhysician" Width="300px" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblVisitType"><%=GetLabel("Visit Type")%></label></td>
                        <td colspan="3">
                            <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visit Duration")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtVisitDuration" Width="100px" runat="server" CssClass="number" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Remarks")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtVisitRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
                <h4><%=GetLabel("Patient Information")%></h4>
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"></td>
                        <td>
                            <asp:CheckBox ID="chkIsNewPatient" runat="server" /><%=GetLabel("Is New Patient")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblMRN"><%=GetLabel("MRN")%></label></td>
                        <td>
                            <input type="hidden" id="hdnMRN" value="" runat="server" />
                            <asp:TextBox ID="txtMRN" CssClass="NoRM" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Patient Name")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td style="width:11%"><dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%" runat="server" /></td>
                                    <td style="width:3px">&nbsp;</td>
                                    <td style="width:25%"><asp:TextBox ID="txtFirstName" Width="100%" runat="server" /></td>
                                    <td style="width:3px">&nbsp;</td>
                                    <td style="width:25%"><asp:TextBox ID="txtMiddleName" Width="100%" runat="server" /></td>
                                    <td style="width:3px">&nbsp;</td>
                                    <td style="width:35%"><asp:TextBox ID="txtFamilyName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Address")%></label></td>
                        <td><asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Phone No")%></label></td>
                        <td><asp:TextBox ID="txtPhoneNo" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Mobile Phone")%></label></td>
                        <td><asp:TextBox ID="txtMobilePhone" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Email")%></label></td>
                        <td><asp:TextBox ID="txtEmail" CssClass="email" Width="200px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
