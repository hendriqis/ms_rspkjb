<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderDocumentCheckListCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderDocumentCheckListCtl1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_testOrderDocumentCheckListCtl1">
    $(function () {
        setDatePicker('<%=txtOrderDate.ClientID %>');
        setDatePicker('<%=txtScheduleDate.ClientID %>');
        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtScheduleDate.ClientID %>').datepicker('option', 'minDate', '0');

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        var operatingRoomID = $('#<%=hdnOperatingRoomID.ClientID %>').val();
        var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        if (serviceUnitID == operatingRoomID) {
            $('#<%:trOperationSchedule.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%:trOperationSchedule.ClientID %>').attr('style', 'display:none');
        }

        //#region Form Values
        if ($('#<%=hdnDocumentCheckListValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnDocumentCheckListValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });

    function onBeforeSaveRecord() {
        var values = getDivContent1FormValues();
        return true;
    }

    //#region Get Form Values
    function getDivContent1FormValues() {
        var controlValues = '';
        $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });

        $('#<%=hdnDocumentCheckListValue.ClientID %>').val(controlValues);

        return controlValues;
    }
    //#endregion  
</script>

<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnOperatingRoomID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnDocumentCheckListLayout" value="" />
    <input type="hidden" runat="server" id="hdnDocumentCheckListValue" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Informasi Order" class="w3-hover-red">Informasi Order</li>
                        <li contentID="divPage2" title="Daftar Berkas/Dokumen" class="w3-hover-red">Daftar Berkas/Dokumen</li>                                                
                    </ul>     
                </div> 
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left"> 
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientName" Width="100%" runat="server" Enabled="False" />                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" Enabled="False" />                                
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" >
                                    <colgroup>
                                        <col style="width:120px" />
                                        <col  />
                                    </colgroup>
                                    <tr>
                                        <td><label class="lblNormal"><%=GetLabel("No. Registrasi")%></label></td>
                                        <td><asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" Enabled="False" /> </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" Enabled="False" />                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr id="trOperationSchedule" runat="server" style="display:none">
                            <td class="tdLabel"><%=GetLabel("Jadwal Operasi") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsEmergency" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" Emergency" Value="1" />
                                    <asp:ListItem Text=" Elektif" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Rencana")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td><asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" /></td>                                      
                                        <td>
                                        </td>
                                        <td style="display:none">
                                        </td>
                                    </tr>
                                </table>                            
                            </td>
                        </tr>                   
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display:none"> 
                    <table class="tblContentArea">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2" style="vertical-align:top">
                                <div id="divFormContent1" runat="server" style="height: 480px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pengecekan Terakhir")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDocumentChecklistDateTime" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr> 
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
