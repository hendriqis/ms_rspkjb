<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddAnesthesyMedicationCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AddAnesthesyMedicationCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_AddAnesthesyMedicationCtl1">
    $(function () {
        setDatePicker('<%=txtLogDate.ClientID %>');
        $('#<%=txtLogDate.ClientID %>').datepicker('option', 'minDate', '0');
    });

    function onLedDrugNameLostFocus(led) {
        var drugID = led.GetValueText();
        if (drugID != '') {
            $('#<%=hdnDrugID.ClientID %>').val(drugID);
            $('#<%=hdnDrugName.ClientID %>').val(led.GetDisplayText());
            var filterExpression = "ItemID = " + drugID;
            Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                $('#<%=txtDosingDose.ClientID %>').val('1');
                cboMedicationRoute.SetValue(result.GCMedicationRoute);
                cboDosingUnit.SetValue(result.GCItemUnit);
                $('#<%=txtItemUnit.ClientID %>').val(result.ItemUnit);
                $('#<%=txtDosingDose.ClientID %>').focus();
            });
        }
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshMedicationAdministrationLog == 'function')
            onRefreshMedicationAdministrationLog();
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPopupID" value="" />
<input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
<input type="hidden" runat="server" id="hdnPopupTestOrderID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
<div>
    <div>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 150px" />
                <col width="60px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal ")%>
                        -
                        <%=GetLabel("Jam ")%></label>
                </td>
                <td colspan="3">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtLogDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Obat ")%></label>
                </td>
                <td colspan="2">
                    <input type="hidden" value="" id="hdnDrugID" runat="server" />
                    <input type="hidden" value="" id="hdnDrugName" runat="server" />
                    <qis:QISSearchTextBox ID="ledDrugName" ClientInstanceName="ledDrugName" runat="server"
                        Width="100%" ValueText="ItemID" FilterExpression="IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999'" DisplayText="ItemName1"
                        MethodName="GetvDrugInfoList">
                        <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                        <Columns>
                            <qis:QISSearchTextBoxColumn Caption="Drug Name" FieldName="ItemName" Description="i.e. Panadol"
                                Width="400px" />
                        </Columns>
                    </qis:QISSearchTextBox>              
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Satuan Obat")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemUnit" runat="server" Width="100%" ReadOnly="true"  />
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jumlah Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number"  />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                        Width="100px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory"><%=GetLabel("Rute Pemberian")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute" Width="163px"  />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal"><%=GetLabel("Catatan Pemberian")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
        </table>
    </div>
</div>
