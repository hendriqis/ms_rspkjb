<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentChangeDateCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Outpatient.Program.AppointmentChangeDateCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_appointmentchangedatectl">
    var numInit = 0;
    function onBeforeSaveRecord(errMessage) {
        var appointmentID = parseInt($('#<%=hdnSelectedAppointmentID.ClientID %>').val());
        if (appointmentID < 0)
            return true;
        errMessage.text = '<%=GetErrorMsgAppointmentSlot() %>';
        return false;
    }

    function onLoad() {
        $("#calAppointmentChangeDate").datepicker({
            defaultDate: Methods.getDatePickerDate($('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val()),
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd-mm-yy",
            minDate: "0",
            onSelect: function (dateText, inst) {
                $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                cbpPhysicianChangeAppointment.PerformCallback('refresh');
                $('#<%=txtNewAppointmentDate.ClientID %>').val(dateText);
            }
        });


        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').die('click');
        $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=txtNewPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());

            cbpAppointmentChangeAppointment.PerformCallback();
        });
        var paramedicID = $('#<%=hdnOldParamedicID.ClientID %>').val();
        $('#<%=grdPhysician.ClientID %> > tbody > tr').each(function () {
            if ($(this).find('.keyField').html() == paramedicID)
                $(this).click();
        });

        $('#<%=grdAppointment.ClientID %> td.tdAppointment li').die('click');
        $('#<%=grdAppointment.ClientID %> td.tdAppointment li').live('click', function (evt) {
            $tr = $(this).closest('tr');
            var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
            if (appointmentID > -2) {
                $('#<%=grdAppointment.ClientID %> li.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=txtNewAppointmentTime.ClientID %>').val($tr.find('.tdTime').html());

                $('#<%=hdnSelectedAppointmentID.ClientID %>').val(appointmentID);
            }
        });

        $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdExpand').die('click');
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
    }

    function onCbpAppointmentChangeAppointmentEndCallback(s) {
        var idx = parseInt(s.cpResult);
        if (idx == 0)
            idx = 1;
        $('#<%=grdAppointment.ClientID %> > tbody > tr:eq(' + idx + ') td.tdAppointment li').click();
        hideLoadingPanel();
    }

    function onCboServiceUnitChangeAppointmentValueChanged() {
        $('#<%=txtNewServiceUnit.ClientID %>').val(cboServiceUnitChangeAppointment.GetText());
        cbpPhysicianChangeAppointment.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
            cbpPhysicianChangeAppointment.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPhysicianChangeAppointmentEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPhysicanChangeAppointment"), pageCount, function (page) {
                cbpPhysicianChangeAppointment.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<input type="hidden" runat="server" id="hdnID" value="" />
<input type="hidden" id="hdnSelectedAppointmentID" runat="server" />
<input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
<input type="hidden" id="hdnOldParamedicID" runat="server" />
<input type="hidden" id="hdnMaxAppoitment" runat="server" />

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
                            <div id="calAppointmentChangeDate"></div>
                        </td>
                        <td valign="top">
                            <dxe:ASPxComboBox ID="cboServiceUnitChangeAppointment" ClientInstanceName="cboServiceUnitChangeAppointment" Width="100%" runat="server">
                                <ClientSideEvents Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                    ValueChanged="function(s,e) { onCboServiceUnitChangeAppointmentValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                            <input type="hidden" id="hdnParamedicID" runat="server" />
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpPhysicianChangeAppointment" runat="server" Width="100%" ClientInstanceName="cbpPhysicianChangeAppointment"
                                    ShowLoadingPanel="false" OnCallback="cbpPhysicianChangeAppointment_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                        Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                        EndCallback="function(s,e){ onCbpPhysicianChangeAppointmentEndCallback(s); }" />
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
                                        <div id="pagingPhysicanChangeAppointment"></div>
                                    </div>
                                </div> 
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpAppointmentChangeAppointment" runat="server" Width="100%" ClientInstanceName="cbpAppointmentChangeAppointment"
                                    ShowLoadingPanel="false" OnCallback="cbpAppointmentChangeAppointment_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                        Init="function(s,e){ numInit++; if(numInit == 3) onLoad(); }"
                                        EndCallback="function(s,e){ onCbpAppointmentChangeAppointmentEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1">
                                                <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdAppointment_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />                                                        
                                                        <asp:TemplateField ItemStyle-CssClass="tdExpand" HeaderStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <img class="imgExpand" <%#: Eval("ParentID").ToString() != "-1" ? "style='display:none;'" : "style='cursor:pointer'"%> src='<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>' alt='' />
                                                                <input type="hidden" parentid='<%#: Eval("ParentID")%>' />
                                                                <input type="hidden" class="hdnIsExpand" value="1" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="70px" >
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Time") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="tdTime"><%#: Eval("Time") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="tdAppointment" >
                                                            <ItemTemplate>
                                                                <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                                    <HeaderTemplate>
                                                                        <ol>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <li>
                                                                            <div class="tdAppointmentInformation">
                                                                                <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                                <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                                <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete" <%#: Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%>  />
                                                                                <div class="divPatientName"><%#: Eval("PatientName") %></div>
                                                                                <div class="divAppointmentInformationDt">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td><img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" /></td>
                                                                                            <td valign="top">
                                                                                                <div><%#: Eval("AppointmentNo") %></div>
                                                                                                <div><b><%#: Eval("PatientName") %></b></div>
                                                                                                <div><%#: Eval("VisitTypeName") %></div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>     
                                                                            </div>   
                                                                        </li>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </ol>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>                                                                                                                            
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
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </td>        
        <td style="padding:5px;vertical-align:top">
            <table style="width: 100%;">
                <colgroup>
                    <col style="width:30%"/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Appointment No")%></label></td>
                    <td><asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="160px" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Patient Name")%></label></td>
                    <td><asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visit Type")%></label></td>
                    <td><asp:TextBox ID="txtVisitType" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>
            <h4><%=GetLabel("From") %></h4>
            <table style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col style="width:140px"/>
                    <col style="width:30px"/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date / Time")%></label></td>
                    <td><asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="120px" runat="server" CssClass="datepicker" /></td>
                    <td style="padding-left:30px;padding-right:10px"><%=GetLabel("Hour")%></td>
                    <td><asp:TextBox ID="txtAppointmentHour" ReadOnly="true" runat="server" Width="60px" CssClass="time" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Clinic")%></label></td>
                    <td colspan="3"><asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Physician")%></label></td>
                    <td colspan="3"><asp:TextBox ID="txtPhysician" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>
            <h4><%=GetLabel("To") %></h4>
            <table style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col style="width:140px"/>
                    <col style="width:30px"/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date / Time")%></label></td>
                    <td><asp:TextBox ID="txtNewAppointmentDate" ReadOnly="true" Width="120px" runat="server" CssClass="datepicker" /></td>
                    <td style="padding-left:30px;padding-right:10px"><%=GetLabel("Hour")%></td>
                    <td><asp:TextBox ID="txtNewAppointmentTime" ReadOnly="true" runat="server" Width="60px" CssClass="time" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Clinic")%></label></td>
                    <td colspan="3"><asp:TextBox ID="txtNewServiceUnit" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Physician")%></label></td>
                    <td colspan="3"><asp:TextBox ID="txtNewPhysician" ReadOnly="true" Width="100%" runat="server" /></td>
                </tr>  
            </table>
        </td>
    </tr>
</table>