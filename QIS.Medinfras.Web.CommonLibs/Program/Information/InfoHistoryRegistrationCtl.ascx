<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoHistoryRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoHistoryRegistrationCtl" %>
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
                                                <th colspan="5" align="center">
                                                    <%=GetLabel("Old Data")%>
                                                </th>
                                                <th colspan="5" align="center">
                                                    <%=GetLabel("New Data")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Business Partner")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Lock Down")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Transfered")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Lock Down")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Transfered")%>
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
                                                <th colspan="5" align="center">
                                                    <%=GetLabel("Old Data")%>
                                                </th>
                                                <th colspan="5" align="center">
                                                    <%=GetLabel("New Data")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Business Partner")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Lock Down")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Transfered")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Registration Status")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Lock Down")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Transfered")%>
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
                                                <%# Eval("IsLockDownOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsBillingClosedOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsBillingReopenOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsTransferedOLD").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("RegistrationStatusNEW")%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsLockDownNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsBillingClosedNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsBillingReopenNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td align="center">
                                                <%# Eval("IsTransferedNEW").ToString() == "False" ? "" : "V"%>
                                            </td>
                                            <td>
                                                <%#: Eval("cfBusinessPartnerInfo")%>
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
