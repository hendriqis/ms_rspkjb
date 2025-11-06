<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="InfoPatientDiagnose.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientDiagnose" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 100px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 15%">
                                                            <%= GetLabel("Diagnose Information")%>
                                                        </th>
                                                        <th style="width: 55%">
                                                            <%= GetLabel("Diagnose")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created Date")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("Tidak ada informasi diagnosa pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 15%">
                                                            <%= GetLabel("Diagnose Information")%>
                                                        </th>
                                                        <th style="width: 55%">
                                                            <%= GetLabel("Diagnose")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created Date")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trMutationDetail">
                                                    <td>
                                                        <div style="float: left">
                                                            <%#: Eval("cfDifferentialDateInString")%><br>
                                                            <%#: Eval("DifferentialTime")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <span style="color:Blue; font-size:1.1em"><b><%#: Eval("cfDiagnosis")%></b></span></div>
                                                        <div>
                                                            <%#: Eval("DiagnoseType") %> - <%#: Eval("DifferentialStatus") %></div>
                                                    </td>
                                                    <td>
                                                        <div align="center">
                                                            <%#: Eval("UserFullName") %>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="center">
                                                            <%#: Eval("cfCreatedDateInString") %>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divPlanningSummary" runat="server" >
        <table class="tblContentArea" style="width: 100%" >
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 100px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView2" runat="server" Width="100%" ClientInstanceName="cbpView2"
                            ShowLoadingPanel="false" OnCallback="cbpView2_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpView2EndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="pnlService2" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView2" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView2" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 15%">
                                                            <%= GetLabel("Date")%>
                                                        </th>
                                                        <th style="width: 55%">
                                                            <%= GetLabel("Catatan Tindakan Rawat Jalan")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created Date")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="4">
                                                            <%=GetLabel("Tidak ada informasi catatan tindakan pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView2" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 15%">
                                                            <%= GetLabel("Date")%>
                                                        </th>
                                                        <th style="width: 55%">
                                                            <%= GetLabel("Catatan Tindakan Rawat Jalan")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 15%">
                                                            <%=GetLabel("Created Date")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trMutationDetail">
                                                    <td>
                                                        <%#: Eval("ObservationDateInString")%><br>
                                                        <%#: Eval("ObservationTime")%>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("PlanningSummary")%>
                                                    </td>
                                                    <td>
                                                        <div align="center">
                                                            <%#: Eval("CreatedByName")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="center">
                                                            <%#: Eval("cfCreatedDateInString")%>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="Div1">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
