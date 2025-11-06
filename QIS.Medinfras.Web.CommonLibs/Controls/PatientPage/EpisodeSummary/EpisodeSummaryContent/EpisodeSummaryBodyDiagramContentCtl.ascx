<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryBodyDiagramContentCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryBodyDiagramContentCtl" %>

<style type="text/css">
    .templatePatientBodyDiagramContent
    {
        font-size:12px;
    }
    .templatePatientBodyDiagramContent .containerImage
    {
        float:left;
        display: table-cell;
        vertical-align:middle;
        border: 1px solid #9C9898;
        width:200px;
        height:200px;
        margin-right: 20px;
        text-align:center;
        position:relative;
    }
    .templatePatientBodyDiagramContent .containerImage img
    {
        max-height:200px;
        max-width:200px;
        position:absolute;top:0;bottom:0;left:0;right:0;margin:auto;
    }
</style>
<div id="EMRContent" style="width:99%;">
    <div class="templatePatientBodyDiagramContent">
        <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />  
        <table style="width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:200px" valign="top">                    
                    <div class="containerImage boxShadow">
                        <img src='' alt="" id="imgBodyDiagram" runat="server" />  
                    </div>
                </td>
                <td valign="top">
                    <table style="width:100%">
                        <colgroup style="width:40px"/>
                        <colgroup style="width:5px"/>
                        <tr>
                            <td><%=GetLabel("Name") %></td>
                            <td>:</td>
                            <td><span class="spValue" id="spnDiagramName" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td><%=GetLabel("Remarks")%></td>
                            <td>:</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Repeater ID="rptRemarks" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                        <colgroup width="20px" />
                                        <colgroup width="2px" />
                                        <colgroup width="15px" />
                                        <colgroup width="2px" />
                                        <colgroup width="40px" />
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
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td valign="top"><%=GetLabel("Physician")%></td>
                            <td valign="top">:</td>
                            <td valign="top"><span class="spValue" id="spnParamedicName" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td valign="top"><%=GetLabel("Date/Time")%></td>
                            <td valign="top">:</td>
                            <td valign="top"><span class="spValue" id="spnObservationDateTime" runat="server"></span></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
