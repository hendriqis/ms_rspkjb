<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryBodyDiagramCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryBodyDiagramCtl" %>

<style type="text/css">
    .templatePatientBodyDiagram
    {
        font-size:12px;
    }
    .templatePatientBodyDiagram .containerImage
    {
        float:left;
        display: table-cell;
        vertical-align:middle;
        border: 1px solid #9C9898;
        width:300px;
        height:300px;
        margin-right: 20px;
        text-align:center;
        position:relative;
    }
    .templatePatientBodyDiagram .containerImage img
    {
        max-height:300px;
        max-width:300px;
        position:absolute;top:0;bottom:0;left:0;right:0;margin:auto;
    }
    .templatePatientBodyDiagram .spLabel
    {
        display: inline-block;
        width: 100px;
        font-weight:bolder;
    }
    .templatePatientBodyDiagram .spValue
    {
        margin-left:10px;
    }
</style>
<h3 class="headerContent"><%=GetLabel("Body Diagram Information")%></h3>
<div id="EMRContent" style="width:99%;">
    <div class="templatePatientBodyDiagram">
        <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />  
        <div class="containerImage">
            <img src='' alt="" id="imgBodyDiagram" runat="server" />  
        </div>
        <span class="spLabel"><%=GetLabel("Diagram Name") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
        <span class="spLabel"><%=GetLabel("Remarks") %></span>: <br />
        <asp:Repeater ID="rptRemarks" runat="server">
            <HeaderTemplate>
                <table>
                <colgroup width="20px" />
                <colgroup width="2px" />
                <colgroup width="15px" />
                <colgroup width="2px" />
                <colgroup width="60px" />
                <colgroup width="2px" />
                <colgroup width="*" />
                <colgroup width="16px" />
                <colgroup width="16px" />               
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                <td><img alt="" style="width:16px;height:16px" src="/<%# Page.ResolveClientUrl((string)Eval("SymbolImageUrl"))%>"/></td>
                <td>:</td>
                <td><%# DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                <td>:</td>
                <td><%# DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                <td>:</td>
                <td><%# DataBinder.Eval(Container.DataItem, "Remarks")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <br />
        <span class="spLabel"><%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
        <span class="spLabel"><%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime" runat="server"></span><br />
    </div>
</div>
