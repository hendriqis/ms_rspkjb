<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfinitResultCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfinitResultCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    
</script>
<input type="hidden" value="" id="hdnSelectedMemberCtl" runat="server" />
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" id="hdnTestOrderID" value="" runat="server" />
<input type="hidden" id="hdnBridgingStatus" value="" runat="server" />
<div style="height: 440px; overflow-y: auto"> 

    <table border="0">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table cellpadding="0" cellspacing="1" border="0">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 150px" />
                            <col style="width: 120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionDate">
                                    <%=GetLabel("Tanggal ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionDate" Width="120px" ReadOnly="true" runat="server"
                                    CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Nama Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Dokter Pengirim")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtOrderPhysician" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="Sender" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Sender")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("Sender")%></div>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                 <asp:BoundField DataField="MessageText" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Message Text")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div align="left"><%#: Eval("MessageText")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:BoundField DataField="MessageStatus" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Message Status")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                         <div align="left"><%#: Eval("MessageStatus")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:BoundField DataField="ErrorMessage" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Error Message")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                         <div><%#: Eval("ErrorMessage")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada data yang ditamplikan")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
