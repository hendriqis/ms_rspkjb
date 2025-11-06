<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryLaboratoryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryLaboratoryCtl" %>

<div style="max-height:450px;overflow-y:auto">
    <input type="hidden" value="2" runat="server" id="hdnIDBodyDiagram" />
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
        OnRowDataBound="grdView_RowDataBound" >
        <Columns>
            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                <HeaderTemplate>
                    <h3><%=GetLabel("Laboratory Result")%></h3>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <%#: Eval("TransactionDateInString")%>,<%#: Eval("TransactionTime") %> <span style="color:blue"><%#: Eval("ItemName1")%></span>
                    </div>
                    <div>
                        <asp:Repeater ID="rptLaboratoryDt" runat="server">
                            <ItemTemplate>
                                <div style="padding-left:10px;">
                                    <strong><%#: DataBinder.Eval(Container.DataItem, "FractionName") %> : </strong>&nbsp;
                                    <span <%# Eval("ResultFlag").ToString() != "N" ? "Style='color:red'":"Style='color:black'" %>>
                                        <%#: DataBinder.Eval(Container.DataItem, "ResultValue") %>     
                                    </span>
                                    <%#: DataBinder.Eval(Container.DataItem, "ResultUnit") %>&nbsp;&nbsp;
                                    <span <%# Eval("RefRange").ToString() == "" ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>(<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate> 
                                <br style="clear:both" />
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("No Data To Display") %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
