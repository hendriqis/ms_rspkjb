<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurgeryOrderDocumentCheckListCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryOrderDocumentCheckListCtl1" %>

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

<script type="text/javascript" id="dxss_surgeryOrderDocumentCheckListCtl1">
    $(function () {

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
                        <li contentID="divPage1" title="Jadwal Kamar Operasi" class="w3-hover-red">Jadwal Operasi</li>
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
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Estimasi Lama Operasi") %></td>
                            <td><asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" Enabled="false"/> menit</td>    
                            <td style="padding-left:5px"><asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" Enabled="false" /></td>                                            
                        </tr>
                        <tr>
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
                                            <asp:CheckBox ID="chkIsNextVisit" Width="200px" runat="server" Text=" Kunjungan Berikutnya" Enabled="false" />                                       
                                        </td>
                                        <td style="display:none">
                                            <div id = "divScheduleInfo" runat="server" style="display:none"><input type="button" class="btnSchedule w3-btn w3-hover-blue" value="Info Jadwal" style="background-color:Green;color:White; width:100px" /></div>
                                        </td>
                                    </tr>
                                </table>                            
                            </td>
                        </tr>
                        <tr id="trNextVisit" runat="server" style="display:none">
                            <td class="tdLabel"><%=GetLabel("Jenis Kunjungan Berikutnya") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblNextVisitType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" ODS" Value="1" />
                                    <asp:ListItem Text=" Rawat Inap" Value="2" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblRoom">
                                    <%:GetLabel("Ruang Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" Enabled="false" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan=>
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
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
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
