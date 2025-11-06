<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryDiagnoseCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryDiagnoseCtl" %>

<div style="max-height:450px;overflow-y:auto">
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
        <Columns>
            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" >
                <HeaderTemplate>
                   <h3><%=GetLabel("Diagnosis")%></h3> 
                </HeaderTemplate>
                <ItemTemplate>
                    <div><%#: Eval("DifferentialDateInString")%>, <%#: Eval("DifferentialTime")%>, <b><%#: Eval("cfParamedicNameEarlyDiagnosis")%></b></div>
                    <div>
                            <span style="color:Blue; font-size:1.1em"><%#: Eval("DiagnosisText")%></span>
                            (<b><%#: Eval("DiagnoseID")%></b>)
                    </div>
                    <div><%#: Eval("ICDBlockName")%></div>
                    <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                <ItemTemplate>
                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("No Data To Display") %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>