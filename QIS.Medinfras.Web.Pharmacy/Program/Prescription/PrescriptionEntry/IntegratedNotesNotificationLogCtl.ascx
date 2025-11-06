<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntegratedNotesNotificationLogCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.IntegratedNotesNotificationLogCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnHealthcareServiceUnitIDCtl" />
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderID" />
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderPhysicianID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                <input type="hidden" value="<%#:Eval("NoteDate") %>" bindingfield="NoteDate" />
                                                <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDateInDatePickerFormat" />
                                                <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                                <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NoteTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                        <asp:TemplateField HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div><span style="color:blue; font-style:italic"><%#:Eval("ParamedicName") %> </span> : <span style="font-style:italic"><%#:Eval("cfLastUpdatedDate") %></span> </div>
                                                <div>
                                                    <textarea style="padding-left:10px;border:0;width:99%; height:150px ; background-color:transparent" readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No record to display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging"></div>
                    </div>
                </div> 
            </td>
        </tr>
    </table>
</div>
