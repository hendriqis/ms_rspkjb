<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryNursingNotesCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryNursingNotesCtl" %>

<h3 class="headerContent"><%=GetLabel("Nursing Notes")%></h3>
<style type="text/css">
.warnaHeader
{
    color:#016482;
}
</style>
<div style="max-height:420px;overflow-y:auto;width:95%">
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
        ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
        <Columns>
            <asp:BoundField DataField="NoteDate" HeaderText="Date" HeaderStyle-Width="100px"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Notes" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div>
                        <%# Eval("NursingNote") != null ? Eval("NursingNote").ToString().Replace("\n", "<br />") : ""%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("Tidak ada catatan perawat untuk pasien ini") %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
