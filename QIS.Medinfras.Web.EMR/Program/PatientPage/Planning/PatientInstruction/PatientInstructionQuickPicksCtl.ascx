<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInstructionQuickPicksCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientInstructionQuickPicksCtl" %>

<script id="dxis_patientinstructionquickpicks1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget.js")%>' type='text/javascript'></script>
<script id="dxis_patientinstructionquickpicks3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>
<script type="text/javascript" id="dxss_patientinstructionquickpicksctl">
    $(function () {
        $('#listInstruction').show();
        $('#listInstruction').accordion({ fillSpace: true });
    });
</script>
<asp:Repeater ID="rptInstruction" runat="server" OnItemDataBound="rptInstruction_ItemDataBound">
    <HeaderTemplate>
        <div class="accordion" id="listInstruction" style="height:250px;display:none;">
    </HeaderTemplate>
    <ItemTemplate>
        <a>
            <span style="display:none;" class="GCInstructionGroup"><%#: Eval("GCInstructionGroup")%></span>
            <%#: Eval("InstructionGroup")%>
        </a>                
        <asp:Repeater ID="rptInstructionDt" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>                   
                    <input type="hidden" id="hdnGCInstructionGroup" runat="server" value='<%#:Eval("GCInstructionGroup") %>' />
                    <asp:CheckBox ID="chkInstruction" runat="server" />
                    <span id="spnDescription" runat="server"><%#: Eval("InstructionDescription1")%></span>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </ItemTemplate>
    <FooterTemplate>
        </div>                    
        <div id="divRptEmpty" class="divRptEmpty" runat="server" style="display:none">
            <%=GetLabel("No data to display")%>
        </div>
    </FooterTemplate>
</asp:Repeater>