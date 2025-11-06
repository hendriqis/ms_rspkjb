﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAllergyInfoCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientAllergyInfoCtl" %>
<div align="center">
    <asp:PlaceHolder ID="plhContainer" runat="server">
    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Width="100%">
        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
            <Columns>
                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                    <ItemTemplate>
                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                        <input type="hidden" value="<%#:Eval("LogDateInString") %>" bindingfield="LogDateInString" />
                        <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                        <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                        <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                        <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                        <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                        <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LogDateInString" HeaderText="Log Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <%=GetLabel("Created By")%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div><%#: Eval("BindDate")%></div>
                        <div><%#: Eval("BindUser")%></div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <%=GetLabel("No patient allergy record available")%>
            </EmptyDataTemplate>
        </asp:GridView>
       </asp:Panel>
    </asp:PlaceHolder>
</div>