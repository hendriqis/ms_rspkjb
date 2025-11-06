<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewChargesNursingNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewChargesNursingNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnTransactionID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Catatan Perawat") %></div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <b>
                                                        <%#: Eval("cfJournalDate")%>,
                                                        <%#: Eval("JournalTime") %>,
                                                        <span style="color:Blue"><%#: Eval("ParamedicName") %></span>
                                                    </b>
                                                </div>
                                                <div>
                                                        <%#: Eval("Remarks") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada catatan perawat untuk transaksi ini") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
