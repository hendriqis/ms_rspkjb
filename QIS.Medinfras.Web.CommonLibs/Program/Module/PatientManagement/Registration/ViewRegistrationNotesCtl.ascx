<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewRegistrationNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewRegistrationNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_viewregistrationnotesctl">
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPatientVisitNotes" runat="server" Width="100%" ClientInstanceName="cbpPatientVisitNotes"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onPatientVisitNotesEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdVisitNotes" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("GCNoteType") %>" bindingfield="GCNoteType" />
                                                <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                                <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDate" />
                                                <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NoteDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NoteTime" HeaderText="Waktu" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NoteType" HeaderText="Jenis Catatan" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="NoteText" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewRegistrationNotes">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
