<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryDtChiefComplaintDiagnosisCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EmergencyCare.Program.EpisodeSummaryDtChiefComplaintDiagnosisCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_chiefcomplaintdiagnosisctl">
    //#region Paging Chief Complaint
    var pageCountDtChiefComplaint = parseInt('<%=PageCountDtChiefComplaint %>');
    $(function () {
        setPaging($("#pagingDtChiefComplaint"), pageCountDtChiefComplaint, function (page) {
            cbpDtChiefComplaint.PerformCallback('changepage|' + page);
        });
    });

    function onCbpDtChiefComplaintEndCallback(s) {
        $('#containerImgLoadingDtChiefComplaint').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDtChiefComplaint.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDtChiefComplaint"), pageCount, function (page) {
                cbpDtChiefComplaint.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDtChiefComplaint.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region Paging Diagnosis
    var pageCountDtDiagnosis = parseInt('<%=PageCountDtDiagnosis %>');
    $(function () {
        setPaging($("#pagingDtDiagnosis"), pageCountDtDiagnosis, function (page) {
            cbpDtDiagnosis.PerformCallback('changepage|' + page);
        });
    });

    function onCbpDtDiagnosisEndCallback(s) {
        $('#containerImgLoadingDtDiagnosis').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDtDiagnosis.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDtDiagnosis"), pageCount, function (page) {
                cbpDtDiagnosis.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDtDiagnosis.ClientID %> tr:eq(1)').click();
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
        <td><h4><%=GetLabel("Chief Complaint")%></h4></td>
        <td>&nbsp;</td>
        <td><h4><%=GetLabel("Different Diagnosis")%></h4></td>
    </tr>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpDtChiefComplaint" runat="server" Width="100%" ClientInstanceName="cbpDtChiefComplaint"
                    ShowLoadingPanel="false" OnCallback="cbpDtChiefComplaint_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingDtChiefComplaint').show(); }"
                        EndCallback="function(s,e){ onCbpDtChiefComplaintEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdDtChiefComplaint" runat="server" CssClass="grdView notAllowSelect grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <b>
                                                        <span style="float:left;width:50px;">Date</span>
                                                        <span style="float:left;width:50px;margin-left:80px">Time</span>
                                                        <span style="margin-left:40px">Physician</span>
                                                    </b>
                                                </div>
                                                <div>Chief Complaint</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <div>
                                                        <b>
                                                            <span style="float:left;width:50px;"><%#: Eval("ObservationDateInString")%></span>
                                                            <span style="float:left;width:50px;margin-left:80px"><%#: Eval("ObservationTime")%></span>
                                                            <span style="margin-left:40px"><%#: Eval("ParamedicName")%></span>
                                                        </b>
                                                    </div>
                                                    <div><%#: Eval("ChiefComplaintText")%></div>
                                                    <div>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width:90px"/>
                                                                <col style="width:200px"/>
                                                                <col style="width:90px"/>
                                                                <col style="width:200px"/>
                                                            </colgroup>
                                                            <tr>
                                                                <td>Location</td>
                                                                <td>: <%#: Eval("Location")%></td>
                                                                <td>Quality</td>
                                                                <td>: <%#: Eval("DisplayQuality")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Relieved By</td>
                                                                <td>: <%#: Eval("DisplayRelieved")%></td>
                                                                <td>Onset</td>
                                                                <td>: <%#: Eval("DisplayOnset")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Severity</td>
                                                                <td>: <%#: Eval("DisplaySeverity")%></td>
                                                                <td>Provocation</td>
                                                                <td>: <%#: Eval("DisplayProvocation")%></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Time</td>
                                                                <td>: <%#: Eval("DisplayCourse")%></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingDtChiefComplaint" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtChiefComplaint"></div>
                    </div>
                </div> 
            </div>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpDtDiagnosis" runat="server" Width="100%" ClientInstanceName="cbpDtDiagnosis"
                    ShowLoadingPanel="false" OnCallback="cbpDtDiagnosis_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingDtDiagnosis').show(); }"
                        EndCallback="function(s,e){ onCbpDtDiagnosisEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdDtDiagnosis" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("Diagnose Information")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div><%#: Eval("DifferentialDateInString")%>, <%#: Eval("DifferentialTime")%></div>
                                                <div><b><%#: Eval("DiagnosisText")%> (<%#: Eval("DiagnoseID")%>)</b></div>
                                                <div><%#: Eval("ICDBlockName")%></div>
                                                <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
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
                <div class="imgLoadingGrdView" id="containerImgLoadingDtDiagnosis" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingDtDiagnosis"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>