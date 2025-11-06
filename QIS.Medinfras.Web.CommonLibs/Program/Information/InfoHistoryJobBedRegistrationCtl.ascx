<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoHistoryJobBedRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoHistoryJobBedRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationNo" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpViewInfoHistoryRegistration" runat="server" Width="100%" ClientInstanceName="cbpViewInfoHistoryRegistration"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewInfoHistoryRegistrationEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 120px" rowspan="2" align="center">
                                                    <%=GetLabel("Log Date")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2" align="left">
                                                    <%=GetLabel("User")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Old Data")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("New Data")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Reopen")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 120px" rowspan="2" align="center">
                                                    <%=GetLabel("Log Date")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2" align="left">
                                                    <%=GetLabel("User")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("Old Data")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("New Data")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Job Bed Reopen")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <%#: Eval("cfLastUpdatedDate")%>
                                            </td>
                                            <td>
                                                <%#: Eval("LastUpdatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("RegistrationStatusOLD")%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsJobBedClosedOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsJobBedReopenOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("RegistrationStatusNEW")%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsJobBedClosedNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsJobBedReopenNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
