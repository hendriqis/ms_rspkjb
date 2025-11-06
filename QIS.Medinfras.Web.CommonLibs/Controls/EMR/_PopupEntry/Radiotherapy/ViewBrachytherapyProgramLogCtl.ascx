<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewBrachytherapyProgramLogCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewBrachytherapyProgramLogCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ViewLogCtl">
    $(function () {
        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }    
    });

    function onCboGCBrachyTherapyTypeChanged(s) {
        var type = s.GetValue();

        if (type != Constant.BrachytherapyType.Interstitial && type != Constant.BrachytherapyType.Intrakaviter_Interstitial) {
            $('#<%=hdnIsUsingForm.ClientID %>').val('0');
            $('#<%=trChannelLayout.ClientID %>').attr('style', 'display:none');
        }
        else {
            $('#<%=hdnIsUsingForm.ClientID %>').val('1');
            $('#<%=trChannelLayout.ClientID %>').removeAttr('style');
        }
    }
</script>
<div style="height: 480px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnProgramLogID" value="" />
    <input type="hidden" runat="server" id="hdnProgramID" value="" />
    <input type="hidden" runat="server" id="hdnTotalFraction" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnIsUsingForm" value="" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Fraksi Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionNo" Width="60px" CssClass="number" runat="server" ReadOnly="True"/>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogDate" Width="120px" runat="server" ReadOnly="True" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" ReadOnly="True" />
                        </td>
                        <td />
                    </tr>
                    <tr id="trBrachytherapy1" runat="server">
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Brakiterapi")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox runat="server" ID="cboGCBrachyTherapyType" ClientInstanceName="cboGCBrachyTherapyType"
                                Width="50%" ToolTip="Jenis Brakiterapi" Enabled="False">
                                <ClientSideEvents ValueChanged="function(s){ onCboGCBrachyTherapyTypeChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trBrachytherapy2" runat="server">
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblMandatory">
                                <%=GetLabel("Aplikator")%></label>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox runat="server" ID="cboGCApplicatorType" ClientInstanceName="cboGCApplicatorType"
                                Width="50%" ToolTip="Aplikator Brakiterapi" Enabled="False">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Total Channel")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTotalChannel" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> 
                        </td>
                    </tr>    
                    <tr id="trChannelLayout" runat="server" style="display:none">
                        <td colspan="4">
                            <div id="divFormContent" runat="server" style="height: 250px; overflow-y: auto;">
                            </div>          
                        </td>                   
                    </tr>     
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Dosis yang diberikan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTotalDosage" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                        </td>
                    </tr>                                                                                                                                                                          
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
