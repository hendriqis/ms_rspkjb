<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderHeaderCtl1.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.TestOrderHeaderCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    setDatePicker('<%=txtOrderDate.ClientID %>');
    $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtRemarks.ClientID %>').focus();

    function onAfterSaveRecordPatientPageEntry(value) {
        if ($('#<%=hdnTestOrderType.ClientID %>').val() == 'X001^004') {
            if (typeof onRefreshLaboratoryGrid == 'function')
                onRefreshLaboratoryGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddLabTestOrder == 'function')
                    onAfterAddLabTestOrder(param[1]);
            }
        }
        else {
            if (typeof onRefreshImagingGrid == 'function')
                onRefreshImagingGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddImagingTestOrder == 'function')
                    onAfterAddImagingTestOrder(param[1]);
            }
        }
    }
</script>

<div style="height: 400px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" id="hdnTestOrderType" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnDiagnosisSummary" value="0" runat="server" />
    <input type="hidden" id="hdnChiefComplaint" value="0" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Order")%></label></td>
            <td><asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter Pengirim")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td />
            <td>
                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Catatan Pemeriksaan /  Klinis")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtRemarks" Width="95%" runat="server" TextMode="MultiLine" Rows="8" /></td>
        </tr>
    </table>
</div>
