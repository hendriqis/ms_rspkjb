<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignEntry1Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientPage.VitalSignEntry1Ctl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onAfterSaveRecordPatientPageEntry(result) {
        if (typeof onRefreshVitalSignGrid == 'function')
            onRefreshVitalSignGrid();

        if (result != "") {
            var param = result.split("|");
            if (typeof onAfterAddVitalSign == 'function')
                onAfterAddVitalSign(param[1]);
        }
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
        padding:1px;
        background-color:#91D100;
        color: black; 
        border: 1px solid gray;
        text-align:center;
    }
</style>
<div>
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Date")%>
                    -
                    <%=GetLabel("Time")%></label>
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
                    <%=GetLabel("Physician")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboParamedicID" Width="235px" runat="server" Enabled="false" />
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
</div>
<div style="height: 410px; overflow-y: scroll; border:0px">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnIsDischargeVitalSign" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="0" />
    <input type="hidden" runat="server" id="hdnPatientVisitNoteID" value="0" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="0" />
    <input type="hidden" runat="server" id="hdnLinkMedicalResumeID" value="0" />
    <input type="hidden" runat="server" id="hdnVitalSignSummary" value="" />
    <input type="hidden" runat="server" id="hdnLinkPreSurgeryAssessmentID" value="0" />
    <input type="hidden" runat="server" id="hdnLinkPreAnesthesyAssessmentID" value="0" />
    <input type="hidden" runat="server" id="hdnLinkSurgeryAnesthesyStatusID" value="0" />
    <input type="hidden" runat="server" id="hdnLinkChiefComplaintID" value="0" />
    <table class="tblContentArea" border="0">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding-left: 15px">
                <asp:Repeater ID="rptVitalSign" runat="server" OnItemDataBound="rptVitalSign_ItemDataBound" >
                    <HeaderTemplate>
                        <ul id="ulVitalSign">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 145px" />
                                </colgroup>
                                <tr>
                                    <td class="tdVitalSignHeader">
                                        <input type="hidden" id="hdnVitalSignID" runat="server" value='<%#:Eval("VitalSignID") %>' />
                                        <input type="hidden" id="hdnVitalSignType" runat="server" value='<%#:Eval("GCValueType") %>' />
                                        <input type="hidden" id="hdnVitalSignLabel" runat="server" value='<%#:Eval("VitalSignLabel") %>' />
                                        <%#:Eval("VitalSignLabel") %> &nbsp;<span style="color:White"><%#:Eval("ValueUnit") %></span>
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
                <table id="tblVitalSignType" runat="server" cellpadding="0" cellspacing="1">
                </table>
            </td>
        </tr>
    </table>
</div>
