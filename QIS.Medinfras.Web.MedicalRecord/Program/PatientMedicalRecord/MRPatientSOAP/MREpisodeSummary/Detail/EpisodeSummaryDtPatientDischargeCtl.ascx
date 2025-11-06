<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryDtPatientDischargeCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeSummaryDtPatientDischargeCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_FollowUpVisitPatientInstructionctl">
    //#region Paging Follow Up Visit
    var pageCountDtFollowUpVisit = parseInt('<%=PageCountDtFollowUpVisit %>');
    $(function () {
        setPaging($("#pagingDtFollowUpVisit"), pageCountDtFollowUpVisit, function (page) {
            cbpDtFollowUpVisit.PerformCallback('changepage|' + page);
        });
    });

    function onCbpDtFollowUpVisitEndCallback(s) {
        $('#containerImgLoadingDtFollowUpVisit').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDtFollowUpVisit.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDtFollowUpVisit"), pageCount, function (page) {
                cbpDtFollowUpVisit.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDtFollowUpVisit.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region Paging Patient Instruction
    var pageCountDtPatientInstruction = parseInt('<%=PageCountDtPatientInstruction %>');
    $(function () {
        setPaging($("#pagingDtPatientInstruction"), pageCountDtPatientInstruction, function (page) {
            cbpDtPatientInstruction.PerformCallback('changepage|' + page);
        });
    });

    function onCbpDtPatientInstructionEndCallback(s) {
        $('#containerImgLoadingDtPatientInstruction').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDtPatientInstruction.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDtPatientInstruction"), pageCount, function (page) {
                cbpDtPatientInstruction.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDtPatientInstruction.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<table style="width:100%"> 
    <colgroup>
        <col style="width:48%"/>
        <col />
        <col style="width:48%"/>
    </colgroup>
    <tr>
        <td><h4><%=GetLabel("Follow Up Plan")%></h4></td>
        <td>&nbsp;</td>
        <td><h4><%=GetLabel("Patient Instruction")%></h4></td>
    </tr>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpDtFollowUpVisit" runat="server" Width="100%" ClientInstanceName="cbpDtFollowUpVisit"
                    ShowLoadingPanel="false" OnCallback="cbpDtFollowUpVisit_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingDtFollowUpVisit').show(); }"
                        EndCallback="function(s,e){ onCbpDtFollowUpVisitEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdDtFollowUpVisit" runat="server" CssClass="grdView notAllowSelect grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                    <div>Follow Up Date Time, Physician, Location</div>
                                                    <div>VisitType; Notes</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></div>
                                                <div><%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Data To Display
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingDtFollowUpVisit" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtFollowUpVisit"></div>
                    </div>
                </div> 
            </div>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpDtPatientInstruction" runat="server" Width="100%" ClientInstanceName="cbpDtPatientInstruction"
                    ShowLoadingPanel="false" OnCallback="cbpDtPatientInstruction_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingDtPatientInstruction').show(); }"
                        EndCallback="function(s,e){ onCbpDtPatientInstructionEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdDtPatientInstruction" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="InstructionGroup" HeaderText="Instruction Group" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="Description" HeaderText="Instruction" />
                                        <asp:BoundField DataField="AdditionalText" HeaderText="Additional Text" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Data To Display
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingDtPatientInstruction" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtPatientInstruction"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>