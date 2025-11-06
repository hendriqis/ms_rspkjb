<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BodyDiagramContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.BodyDiagramContentCtl" %>

<style type="text/css">
    .templatePatientBodyDiagramContentCharmBar
    {
        font-size:16px;
    }
    .templatePatientBodyDiagramContentCharmBar .containerImage
    {
        float:left;
        display: table-cell;
        vertical-align:middle;
        border: 1px solid #9C9898;
        width:400px;
        height:400px;
        margin-right: 20px;
        text-align:center;
        position:relative;
    }
    .templatePatientBodyDiagramContentCharmBar .containerImage img
    {
        max-height:400px;
        max-width:400px;
        position:absolute;top:0;bottom:0;left:0;right:0;margin:auto;
    }
    
    .img:hover
    {
        color: #424242;
        -webkit-transition: all .3s ease-in;
        -moz-transition: all .3s ease-in;
        -ms-transition: all .3s ease-in;
        -o-transition: all .3s ease-in;
        padding-left:100px;
        transition: all .3s ease-in;
        opacity: 1;
        transform: scale(1.75);
        -ms-transform: scale(1.75); /* IE 9 */
        -webkit-transform: scale(1.75); /* Safari and Chrome */
    }   
    
    .grow img{
        transition: 1s ease;
    }

    .grow img:hover{
        -webkit-transform: scale(2.5);
        -ms-transform: scale(2.5);
        transform: scale(2.5);
        transition: 1s ease;
    } 
</style>
<div id="EMRContent" style="width:99%;">
    <div class="templatePatientBodyDiagramContentCharmBar">
        <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />  
        <div class="containerImage boxShadow grow">
            <img src='' alt="" id="imgBodyDiagram" runat="server" />  
        </div>
        <span class="spLabel"><%=GetLabel("Diagram Name") %> &nbsp;</span>: &nbsp;<span class="spValue" id="spnDiagramName" runat="server"></span><br />
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
                <td><img alt="" style="width:16px;height:16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>"/></td>
                <td>:</td>
                <td><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                <td>:</td>
                <td><%#: DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                <td>:</td>
                <td><%#: DataBinder.Eval(Container.DataItem, "Remarks")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <br />
        <span class="spLabel"><%=GetLabel("Physician") %> &nbsp;</span>: &nbsp;<span class="spValue" id="spnParamedicName" runat="server"></span><br />
        <span class="spLabel"><%=GetLabel("Date/Time")%> &nbsp;</span>: &nbsp;<span class="spValue" id="spnObservationDateTime" runat="server"></span><br />
    </div>
</div>
