<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryPhysicianNotesCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryPhysicianNotesCtl" %>

<h3 class="headerContent"><%=GetLabel("Physician Notes")%></h3>
<style type="text/css">
.warnaHeader
{
    color:#016482;
}
</style>
<div style="max-height:420px;overflow-y:auto">
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
        ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
        <Columns>
            <asp:BoundField DataField="NoteDate" HeaderText="Date" HeaderStyle-Width="100px"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Notes" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div>
                        <%# Eval("PhysicianNote") != null ? Eval("PhysicianNote").ToString().Replace("\n","<br />") : ""%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("Tidak ada catatan dokter untuk pasien ini") %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
