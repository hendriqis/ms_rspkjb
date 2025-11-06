<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScanIdentityCardCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ScanIdentityCardCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("Scan")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });

    $('#btnMPEntryProcess').click(function () {
        cbpPopupProcess.PerformCallback('process');
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                $('.tblResultInfo').hide();
                showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
            }
        }
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnMRN" value="" />
<input type="hidden" runat="server" id="hdnMedicalNo" value="" />
<input type="hidden" runat="server" id="hdnPatientName" value="" />
<input type="hidden" runat="server" id="hdnDateOfBirth" value="" />
<input type="hidden" runat="server" id="hdnGender" value="" />
<input type="hidden" runat="server" id="hdnHomeAddress" value="" />
<input type="hidden" value="6000" id="hdnPort" runat="server" />
<div>
    <div>
        <table class="tblEntryContent" style="width:100%">
            <colgroup>
                <col style="width:200px"/>
                <col style="width:150px"/>
                <col style="width:150px"/>
                <col/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal" runat="server" id="lblMRN1"><%=GetLabel("No.RM")%></label></td>
                <td><asp:TextBox ID="txtMRN1" Width="120px" runat="server" ReadOnly="true" /></td>
                <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jenis Kelamin")%></label></td>
                <td><asp:TextBox ID="txtGender1" Width="100%" runat="server" ReadOnly="true" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                <td colspan="3"><asp:TextBox ID="txtPatientName1" Width="100%" runat="server" ReadOnly="true" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tempat Lahir")%></label></td>
                <td><asp:TextBox ID="txtBirthPlace1" Width="100%" runat="server" ReadOnly="true"/></td>
                <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Tanggal Lahir")%></label></td>
                <td><asp:TextBox ID="txtDOB1" Width="100%" runat="server" ReadOnly="true"/></td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Alamat")%></label></td>
                <td colspan="3"><asp:TextBox ID="txtAddress1" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" /></td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
