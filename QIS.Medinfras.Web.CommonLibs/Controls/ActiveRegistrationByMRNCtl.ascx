<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveRegistrationByMRNCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ActiveRegistrationByMRNCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationNoCtl" />
    <input type="hidden" value="" runat="server" id="hdnMRNCtl" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatient" ReadOnly="true" Width="500px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                    OnRowDataBound="grdView_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                    EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DepartmentID" HeaderText="Department" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Service Unit" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Paramedic Name" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="RoomName" HeaderText="Room" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="RegistrationStatus" HeaderText="Status" HeaderStyle-Width="120px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Tanggal - Jam Pulang / Disposisi" DataField="cfDischargeStatus" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Font-Size="9pt" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Status Pembayaran" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img id="imgIsHasOutstanding" runat="server" src='<%# Eval("IsHasOutstanding").ToString() == "0" ? ResolveUrl("~/Libs/Images/Status/coverage_ok.png") : ResolveUrl("~/Libs/Images/Status/coverage_warning.png") %>'
                                                    style="height: 26px"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
