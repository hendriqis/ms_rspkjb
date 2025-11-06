<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TariffApprovalNotesCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.TariffApprovalNotesCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_tariffapprovalnotesctl">
    function onCbpTariffNotesEndCallback(s) {
        hideLoadingPanel();
    }

    function onCbpSaveNotesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'success') {
            pcRightPanelContent.Hide();
        }
        else {
            showToast('Save Failed', 'Error Message : ' + param[1]);
        }
    }
</script>

<input type="hidden" value="" id="hdnBookID" runat="server" />
<b><%=GetLabel("Recent Notes") %></b>
<dxcp:ASPxCallbackPanel ID="cbpTariffNotes" runat="server" Width="100%" ClientInstanceName="cbpTariffNotes"
    ShowLoadingPanel="false">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpTariffNotesEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;height: 230px;overflow-y:auto; ">
                <asp:GridView ID="grdNotes" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ID" ItemStyle-CssClass="keyField" HeaderStyle-CssClass="keyField"  />
                        <asp:BoundField DataField="DateInString" HeaderStyle-HorizontalAlign="Center" HeaderText="Date" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Time" HeaderText="Time" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="Text" HeaderText="Text" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>

<b><%=GetLabel("New Notes") %></b>
<asp:TextBox ID="txtNewNotes" runat="server" TextMode="MultiLine" Width="100%" Height="50px" />
<table style="margin-left:auto;margin-right:auto;margin-top:10px;">
    <tr>
        <td>
            <input type="button" id="btnSaveNotes" onclick="cbpSaveNotes.PerformCallback();" value="<%=GetLabel("Save") %>" />
        </td>
        <td>
            <input type="button" onclick="pcRightPanelContent.Hide();" value="<%=GetLabel("Cancel") %>" />
        </td>
    </tr>
</table> 

<dxcp:ASPxCallbackPanel ID="cbpSaveNotes" runat="server" ClientInstanceName="cbpSaveNotes" OnCallback="cbpSaveNotes_Callback" 
    ShowLoadingPanelImage="false">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
        EndCallback="function(s,e) { onCbpSaveNotesEndCallback(s); }"  />
</dxcp:ASPxCallbackPanel>