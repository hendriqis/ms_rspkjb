<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HCPScheduleEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.HCPScheduleEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        $('#btnSaveSchedule').click(function (evt) {
            if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
                cbpScheduleProcess.PerformCallback('save');
        });
    });

    $('.chkSchedule input').die('change');
    $('.chkSchedule input').live('change', function () {
        var isChecked = $(this).is(":checked");
        $ddl = $(this).closest('tr').find('.ddlSchedule');
        if (isChecked) {
            $ddl.removeAttr('disabled');
            $ddl.addClass('required');
        }
        else {
            $ddl.attr('disabled', 'disabled');
            $ddl.val('');
            $ddl.removeClass('required');
        }
    });

    function onCbpScheduleProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        hideLoadingPanel();
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                pcRightPanelContent.Hide();
        }
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:135px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dokter")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rumah Sakit")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit Pelayanan")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>
                <fieldset id="fsEntryPopup" style="margin:0"> 
                    <table style="width:100%;" border="0" >     
                        <colgroup>
                            <col style="width:20px"/>
                            <col style="width:110px"/>
                            <col />
                        </colgroup>                              
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime1" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Senin")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime1" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime2" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Selasa")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime2" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime3" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Rabu")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime3" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime4" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Kamis")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime4" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime5" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Jumat")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime5" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime6" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Sabtu")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime6" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>                                
                        <tr>
                            <td><asp:CheckBox ID="chkOperationalTime7" CssClass="chkSchedule" runat="server" /></td>
                            <td><%=GetLabel("Minggu")%></td>
                            <td><asp:DropDownList ID="ddlOperationalTime7" CssClass="ddlSchedule" runat="server" Width="100%" /></td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpScheduleProcess" runat="server" Width="100%" ClientInstanceName="cbpScheduleProcess"
        ShowLoadingPanel="false" OnCallback="cbpScheduleProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpScheduleProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <div style="width:100%;text-align:center">
        <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
            <tr>
                <td><input type="button" value='<%= GetLabel("Save")%>' style="width:70px" id="btnSaveSchedule" /></td>
                <td><input type="button" value='<%= GetLabel("Close")%>' style="width:70px" onclick="pcRightPanelContent.Hide();" /></td>
            </tr>
        </table>
    </div>
</div>

