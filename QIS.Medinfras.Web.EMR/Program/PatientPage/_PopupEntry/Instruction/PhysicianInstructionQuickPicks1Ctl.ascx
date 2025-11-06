<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicianInstructionQuickPicks1Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.PhysicianInstructionQuickPicks1Ctl" %>

<script type="text/javascript" id="dxss_patientinstructionquickpicksctl">
    $(function () {
        $('#listInstruction').show();
        $('#listInstruction').accordion({ fillSpace: true });
    });

    function onAfterProcessPopupEntry(param) {
        if (param != '') {
            if (typeof onAfterSelectFromInstructionTemplate == 'function')
                onAfterSelectFromInstructionTemplate(param);
        }
    }
</script>
<style type="text/css">
    div.containerUl ul  { margin:0; padding:0; margin-left:25px;}
    div.containerUl ul:not(:first-child) { margin-top: 10px; }
    div.containerUl ul li  { list-style-type:none; font-size: 12px; padding-bottom:5px; }
    div.containerUl ul li span  { color:#3E3EE3; }
    div.containerUl a        { font-size:12px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
    div.containerUl a:hover  { text-decoration: underline; }
    .divContentTitle { margin-left:25px; font-weight:bold; font-size:11px; text-decoration:underline}
    .divContent { margin-left:25px; font-weight:bold; font-size:11px}    
</style>

<asp:Repeater ID="rptInstruction" runat="server" OnItemDataBound="rptInstruction_ItemDataBound">
    <HeaderTemplate>
        <div class="accordion" id="listInstruction" style="height:450px;display:none;">
    </HeaderTemplate>
    <ItemTemplate>
        <a style="font-size:14px;">
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
                    <span id="spnDescription2" runat="server" style="display:none"><%#: Eval("InstructionDescription2")%></span>
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
            <%=GetLabel("Tidak ada template yang tersedia")%>
        </div>
    </FooterTemplate>
</asp:Repeater>