<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalHistoryGridContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.MedicalHistoryGridContentCtl" %>

<asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage grdMedicalHistory" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
    <Columns>
        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
        <asp:TemplateField>
            <HeaderTemplate>
                <%=GetLabel("Visit Information")%>
            </HeaderTemplate>
            <ItemTemplate>
                <div><%#: Eval("VisitDateInString")%>, <%#: Eval("VisitTime")%></div>
                <div><%#: Eval("ServiceUnitName")%> - <%#: Eval("ParamedicName")%></div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="DisplayPatientDiagnosis" HeaderText="Diagnosis" HeaderStyle-Width="120px" />
        <asp:HyperLinkField HeaderText="Summary" Text="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-CssClass="lnkMedicalRecordSummary" />
    </Columns>
    <EmptyDataTemplate>
        <%=GetLabel("No Patient Medical History To Display")%>
    </EmptyDataTemplate>
</asp:GridView>