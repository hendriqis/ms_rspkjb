<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChiefComplaintEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ChiefComplaintEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onCboOnsetChanged(s) {
        $txt = $('#<%=txtOnset.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }

    function onCboProvocationChanged(s) {
        $txt = $('#<%=txtProvocation.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }

    function onCboQualityChanged(s) {
        $txt = $('#<%=txtQuality.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }

    function onCboSeverityChanged(s) {
        $txt = $('#<%=txtSeverity.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }

    function onCboTimeChanged(s) {
        $txt = $('#<%=txtTime.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }

    function onCboRelievedByChanged(s) {
        $txt = $('#<%=txtRelievedBy.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
            $txt.show();
        else
            $txt.hide();
    }
</script>
<div style="height:410px;overflow-y:scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
                        <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                        <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Chief Complaint")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtChiefComplaint" Width="100%" runat="server" TextMode="MultiLine" Rows="8" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Location")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtLocation" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Onset")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboOnset" ClientInstanceName="cboOnset" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboOnsetChanged(s); }" Init="function(s,e){ onCboOnsetChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td><asp:TextBox ID="txtOnset" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Provocation")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboProvocation" ClientInstanceName="cboProvocation" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboProvocationChanged(s); }" Init="function(s,e){ onCboProvocationChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>                      
                        <td><asp:TextBox ID="txtProvocation" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Quality")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboQuality" ClientInstanceName="cboQuality" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboQualityChanged(s); }" Init="function(s,e){ onCboQualityChanged(s); }" />
                            </dxe:ASPxComboBox>                        
                        </td>                      
                        <td><asp:TextBox ID="txtQuality" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Severity")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboSeverityChanged(s); }" Init="function(s,e){ onCboSeverityChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>                      
                        <td><asp:TextBox ID="txtSeverity" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Time")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboTime" ClientInstanceName="cboTime" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTimeChanged(s); }" Init="function(s,e){ onCboTimeChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>                      
                        <td><asp:TextBox ID="txtTime" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Relieved By")%></label></td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboRelievedBy" ClientInstanceName="cboRelievedBy" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRelievedByChanged(s); }" Init="function(s,e){ onCboRelievedByChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>   
                        <td><asp:TextBox ID="txtRelievedBy" CssClass="txtChief" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
