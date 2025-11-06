<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDocumentContentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientDocumentContentCtl" %>
<style type="text/css">
    .templatePatientDocumentContentCharmBar
    {
        font-size: 16px;
    }
    .templatePatientDocumentContentCharmBar .containerImage
    {
        float: left;
        display: table-cell;
        vertical-align: middle;
        border: 1px solid #9C9898;
        width: 400px;
        height: 500px;
        margin-right: 20px;
        text-align: center;
        position: relative;
    }
    .templatePatientDocumentContentCharmBar .containerImage img
    {
        max-height: 500px;
        max-width: 400px;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
    }
    .spLabel
    {
        font-weight: bold;
    }
</style>
<script type="text/javascript">
    $('img').bind('contextmenu', function (e) {
        return false;
    }); 
</script>

<div id="EMRContent" style="width: 99%;">
    <div class="templatePatientDocumentContentCharmBar">
        <input type="hidden" id="hdnDocumentID" runat="server" value='' />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <colgroup>
                <col width="400px" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div class="containerImage boxShadow">
                        <img class='img' src='' alt="" id="imgDocument" runat="server" />
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <colgroup>
                                <col width="135px" />
                                <col width="30px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <span class="spLabel">
                                        <%=GetLabel("Document Name") %></span>
                                </td>
                                <td>
                                    <span class="spLabel">:</span>
                                </td>
                                <td>
                                    <span class="spValue" id="spnDocumentName" runat="server"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="spLabel">
                                        <%=GetLabel("Document Type") %></span>
                                </td>
                                <td>
                                    <span class="spLabel">:</span>
                                </td>
                                <td>
                                    <span class="spValue" id="spnDocumentType" runat="server"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="spLabel">
                                        <%=GetLabel("Document Date") %></span>
                                </td>
                                <td>
                                    <span class="spLabel">:</span>
                                </td>
                                <td>
                                    <span class="spValue" id="spnDocumentDate" runat="server"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <span class="spLabel">
                                        <%=GetLabel("Remarks") %></span>
                                </td>
                                <td style="vertical-align: top">
                                    <span class="spLabel">:</span>
                                </td>
                                <td style="vertical-align: top">
                                    <textarea id="txtRemarks" runat="server" style="border: 0; width: 450px; height: 350px"
                                        readonly></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <%--        <asp:Repeater ID="rptRemarks" runat="server">
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
--%>
    </div>
</div>
