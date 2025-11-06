<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaboratoryTestResultPrintCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryTestResultPrintCtl" %>

<script type="text/javascript" id="dxss_laboratorytestresultprintctl">
    $('#<%=btnLaboratoryResultPrint.ClientID %>').click(function () {
        $rbo = $('input[name=rboLaboratoryResultPrint]:checked');
        if ($rbo.length > 0) {
            var errMessage = { text: "" };
            var reportCode = $rbo.attr('reportcode');
            if (reportCode != '') {
                var isAllowPrint = true;
                if (typeof onBeforeRightPanelPrint == 'function') {
                    isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                }
                if (isAllowPrint) {
                    var filterExpression = $('#<%=hdnFilterExpression.ClientID %>').val();
                    alert(filterExpression);
                    openReportViewer(reportCode, filterExpression);
                }
                else
                    showToast('Warning', errMessage.text);
            }
        }
    });
</script>
<input type="hidden" value="" id="hdnLabResultID" runat="server" />
<input type="hidden" value="" id="hdnFilterExpression" runat="server" />
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnLaboratoryResultPrint" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Print")%></div></li>
    </ul>
</div>

<asp:Repeater ID="rptPrint" runat="server">
    <ItemTemplate>
        <input type="radio" name="rboLaboratoryResultPrint" value="1" reportcode='<%#: Eval("ReportCode")%>' /><%#: Eval("Title")%><br />
    </ItemTemplate>
</asp:Repeater>