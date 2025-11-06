<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryImagingCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryImagingCtl" %>

<style type="text/css">
.warnaHeader
{
    color:#016482;
}
</style>
<div style="max-height:380px;overflow-y:auto">
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
        OnRowDataBound="grdView_RowDataBound" >
        <Columns>
            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                <HeaderTemplate>
                    <h3><%=GetLabel("Imaging Result")%></h3>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <%#: Eval("TransactionDateInString")%>,<%#: Eval("TransactionTime") %> <span style="color:blue"><%#: Eval("ItemName1")%></span>
                    </div>
                    <div>
                        <asp:Literal ID="literal" Mode="PassThrough" runat="server" />
                        <%--<textarea id="taResultValue" runat="server" style="padding-left:10px;border:0;width:535px; height:150px" readonly>
                        </textarea>--%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("No Data To Display") %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
