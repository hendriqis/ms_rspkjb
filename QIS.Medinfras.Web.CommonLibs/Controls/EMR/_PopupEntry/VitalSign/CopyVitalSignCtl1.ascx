<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyVitalSignCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CopyVitalSignCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_vitalsignentryctl">

    $(function () {
        setDatePicker('<%=txtObservationDate.ClientID %>');
        $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshVitalSignGrid == 'function')
            onRefreshVitalSignGrid();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshVitalSignGrid();
    }
</script>
<style type="text/css">
    #ulVitalSign
    {
        margin: 0;
        padding: 0;
    }
    #ulVitalSign li
    {
        list-style-type: none;
        display: inline-block;
        margin-right: 10px;
    }
    .tdVitalSignHeader
    {
        padding: 1px;
        background-color: #00a8ff;
        color: Black;
        border: 1px solid gray;
        text-align: center;
    }
</style>
<div>
    <input type="hidden" runat="server" id="hdnAssessmentType" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnETTLogID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnVisitNoteID" value="" />
    <input type="hidden" runat="server" id="hdnMonitoringGroup" value="" />
    <input type="hidden" runat="server" id="hdnLinkedReferenceID" value="" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
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
                            <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("PPA")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <%=GetLabel("Catatan Tambahan") %>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <tr>
            <td style="padding-left: 15px">
                <div style="height: 400px;overflow-y: auto;" class="w3-container w3-border w3-animate-left peGroup">
                    <asp:Repeater ID="rptVitalSign" runat="server" OnItemDataBound="rptVitalSign_ItemDataBound">
                        <HeaderTemplate>
                            <ul id="ulVitalSign">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 140px" />
                                    </colgroup>
                                    <tr>
                                        <td class="tdVitalSignHeader">
                                            <input type="hidden" id="hdnVitalSignID" runat="server" value='<%#:Eval("VitalSignID") %>' />
                                            <input type="hidden" id="hdnVitalSignType" runat="server" value='<%#:Eval("GCValueType") %>' />
                                            <%#:Eval("VitalSignLabel") %>
                                            &nbsp;<span style="color: White"><%#:Eval("ValueUnit") %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divTxt" runat="server" visible="false">
                                                <asp:TextBox ID="txtVitalSignType" Width="97%" runat="server" Style="float: left" />
                                            </div>
                                            <div id="divDdl" runat="server" visible="false">
                                                <dxe:ASPxComboBox ID="cboVitalSignType" Width="100%" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <table id="tblVitalSignType" runat="server" cellpadding="0" cellspacing="1">
                </table>
            </td>
        </tr>
    </table>
    </table>
</div>
