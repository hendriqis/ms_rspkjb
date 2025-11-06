<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExternalMedicationInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ExternalMedicationInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDetailID" value="" />
<input type="hidden" runat="server" id="hdnOldFrequency" value="" />
<div>
    <div style="height:300px; overflow-y:scroll;">
        <asp:ListView runat="server" ID="lvwView">
            <EmptyDataTemplate>
                <table id="tblView" runat="server" class="grdPurchaseRequest grdSelected" cellspacing="0" rules="all">
                    <tr>
                        <th class="keyField" rowspan="2">&nbsp;</th>
                        <th  align="left"><%=GetLabel("Drug Name")%></th>
                    </tr>
                    <tr class="trEmpty">
                        <td colspan="2">
                            <%=GetLabel("Tidak ada data informasi obat")%>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                    <tr>
                        <th rowspan="2" class="keyField">&nbsp;</th>
                        <th rowspan="2" class="hiddenColumn" >&nbsp;</th>
                        <th rowspan="2" align="left">
                            <div>
                                <%=GetLabel("Drug Name")%>
                            <div>
                        </th>
                        <th colspan="5" align="center" style="width:200px">
                            <div>
                                <%=GetLabel("Signa")%></div>
                        </th>
                        <th rowspan="2" align="center" style="padding: 3px; width: 80px;">
                            <div>
                                <%=GetLabel("Start Date")%></div>
                        </th>
                        <th rowspan="2" align="right" style="width:60px"><%=GetLabel("Duration")%></th>
                    </tr>
                    <tr>
                        <th style="width: 40px;">
                            <div style="text-align:right">
                                <%=GetLabel("Frequency") %></div>
                        </th>
                        <th style="width: 40px;text-align:left">
                            <div>
                                <%=GetLabel("Timeline") %></div>
                        </th>
                        <th style="width: 40px;">
                            <div style="text-align:right">
                                <%=GetLabel("Dose") %></div>
                        </th>
                        <th style="width: 50px;">
                            <div style="text-align:left">
                                <%=GetLabel("Unit") %></div>
                        </th>
                        <th style="width: 80px;">
                            <div style="text-align:left">
                                <%=GetLabel("Route") %></div>
                        </th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="keyField"><%#: Eval("ID")%></td>
                    <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                    <td align="right" style="width: 40px;">
                        <input type="text" class="txtFrequency number min" min = "1" validationgroup="mpDrugEntry"  value="<%#:Eval("Frequency") %>" style="width:100%" readonly="readonly"/>
                    </td>
                    <td align="left" style="width: 40px;text-align:left"><div><%#: Eval("DosingFrequency")%></div></td>
                    <td align="right">
                        <input type="text" class="txtNumberOfDosage number" value="<%#:Eval("cfNumberOfDosage") %>" style="width:100%" readonly="readonly"/>
                    </td>
                    <td align="left">
                        <div> <%#: Eval("DosingUnit")%></div>
                    </td>
                    <td class="tdRoute" style="width:80px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                    <td style="width:80px"><input type="text" class="txtStartDate" value="<%#:Eval("StartDateInDatePickerFormat") %>" style="width:100%; text-align:center" readonly="readonly" /></td>
                    <td style="width:60px"><input type="text" class="txtDuration number" value="<%#:Eval("DosingDuration") %>" style="width:60px" readonly="readonly"/></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>
