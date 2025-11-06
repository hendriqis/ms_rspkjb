<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PatientDiagnoseInfo.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDiagnoseInfo" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
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
                    <div style="padding: 5px; min-height: 150px;">
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
                                                        <th style="width: 25px">
                                                            <%= GetLabel("Diagnose Information")%>
                                                        </th>
                                                        <th style="width: 250px">
                                                            <%= GetLabel("Diagnose")%>
                                                        </th>
                                                        <th style="width: 20px">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 20px">
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
                                                        <th style="width: 25px">
                                                            <%= GetLabel("Diagnose Information")%>
                                                        </th>
                                                        <th style="width: 250px">
                                                            <%= GetLabel("Diagnose")%>
                                                        </th>
                                                        <th style="width: 20px">
                                                            <%=GetLabel("Created By")%>
                                                        </th>
                                                        <th style="width: 20px">
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
                                                            <span style="color:Blue; font-size:1.1em"><%#: Eval("DiagnoseName")%></span> - (<b><%#: Eval("DiagnoseID")%></b>)</div>
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
</asp:Content>
