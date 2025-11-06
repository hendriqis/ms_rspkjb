<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemAlternateUnitHistoryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemAlternateUnitHistoryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
<script type="text/javascript" id="dxss_ItemAlternateUnitHistoryCtl">
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnItemIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnAlternateUnitIDCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 50px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b><%=GetLabel("Item")%></b></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpViewItemAlternateUnitHistory" runat="server" Width="100%" ClientInstanceName="cbpViewItemAlternateUnitHistory"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewItemAlternateUnitHistoryEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblViewItemAlternateUnitHistory" runat="server" class="grdSelected" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 120px" rowspan="2" align="center">
                                                    <%=GetLabel("Log Date")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2" align="center">
                                                    <%=GetLabel("User")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("Data Lama")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("Data Baru")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Alternate Unit")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Conversion Factor")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Is Deleted")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Alternate Unit")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Conversion Factor")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Is Deleted")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated Date")%>
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
                                                <th style="width: 150px" rowspan="2" align="center">
                                                    <%=GetLabel("User")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("Data Lama")%>
                                                </th>
                                                <th colspan="7" align="center">
                                                    <%=GetLabel("Data Baru")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Alternate Unit")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Conversion Factor")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Is Deleted")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Alternate Unit")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Conversion Factor")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Is Deleted")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Created Date")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated By")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Last Updated Date")%>
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
                                            <td align="center">
                                                <%#: Eval("UserFullName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldAlternateUnit")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldConversionFactor")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldIsDeleted")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldCreatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldCreatedDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldLastUpdatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("OldLastUpdatedDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewAlternateUnit")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("NewConversionFactor")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewIsDeleted")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewCreatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewCreatedDate")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewLastUpdatedByName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("NewLastUpdatedDate")%>
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