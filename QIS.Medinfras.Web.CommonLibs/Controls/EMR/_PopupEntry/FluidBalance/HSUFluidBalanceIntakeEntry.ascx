<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HSUFluidBalanceIntakeEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.HSUFluidBalanceIntakeEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_HSUFluidBalanceIntakeEntry">
    $(function () {
        setDatePicker('<%=txtLogDate.ClientID %>');
        $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });
</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentType" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialize" value="0" />
    <input type="hidden" runat="server" id="hdnIntakeGroup" value="1" />
    <input type="hidden" runat="server" id="hdnGCItemUnit" value="X003^ML" />
    <input type="hidden" runat="server" id="hdnIsCopy" value="0" />

    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 170px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Perawat/Tenaga Medis")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Cairan")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboFluidType" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Cairan")%></label>
                        </td>
                        <td colspan="2"><asp:TextBox ID="txtFluidName" Width="350px" runat="server" /></td>                                             
                    </tr>
                    <tr id="trIntakeAmount" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Cairan Masuk")%></label>
                        </td>
                        <td><asp:TextBox ID="txtFluidAmount" Width="80px" CssClass="number" runat="server" /> ml</td>          
                        <td />                                  
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top"><%=GetLabel("Catatan") %></td>
                        <td colspan="2"><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" Width="100%" /></td>                                            
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
