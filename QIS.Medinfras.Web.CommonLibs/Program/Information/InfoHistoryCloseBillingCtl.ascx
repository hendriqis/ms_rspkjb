<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoHistoryCloseBillingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoHistoryCloseBillingCtl" %>
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
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
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
                                                <th colspan="6" align="center">
                                                    <%=GetLabel("Data Lama")%>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <%=GetLabel("Data Baru")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Ditutup Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Ditutup Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Dibuka Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Dibuka Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Status Billing")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Ditutup Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Ditutup Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Dibuka Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Dibuka Pada")%>
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
                                                <th colspan="6" align="center">
                                                    <%=GetLabel("Data Lama")%>
                                                </th>
                                                <th colspan="6" align="center">
                                                    <%=GetLabel("Data Baru")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Closed")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Ditutup Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Ditutup Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Dibuka Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Dibuka Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Status Billing")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Ditutup Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Ditutup Pada")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Billing Reopen")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Billing Dibuka Oleh")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Billing Dibuka Pada")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <%#: Eval("LogDate")%>
                                            </td>
                                            <td>
                                                <%#: Eval("UserFullName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldIsBillingClosed")%>
                                            </td>
                                            <td>
                                                <%#: Eval("OldBillingClosedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldBillingClosedDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldIsBillingReopen")%>
                                            </td>
                                            <td>
                                                <%#: Eval("OldBillingReopenByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldBillingReopenDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewIsBillingClosed")%>
                                            </td>
                                            <td>
                                                <%#: Eval("NewBillingClosedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewBillingClosedDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewIsBillingReopen")%>
                                            </td>
                                            <td>
                                                <%#: Eval("NewBillingReopenByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewBillingReopenDate")%>
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
