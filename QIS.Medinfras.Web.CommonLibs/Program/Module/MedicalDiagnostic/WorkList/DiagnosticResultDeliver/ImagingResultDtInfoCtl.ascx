<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingResultDtInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ImagingResultDtInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ImagingResultDtInfoCtl">
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnImagingResultIDCtl" value="" runat="server" />
    <table width="100%">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td>
                <table class="tblContentArea">
                    <colgroup>
                        <col width="50%" />
                        <col width="50%" />
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <table>
                                <colgroup>
                                    <col style="width: 180px" />
                                    <col style="width: 150px" />
                                    <col style="width: 80px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("No. Registrasi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server"
                                            Style="text-align: center" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <%=GetLabel("Tanggal-Jam Registrasi") %></label>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-right: 1px; width: 145px">
                                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" ReadOnly="true" runat="server"
                                                        Style="text-align: center" />
                                                </td>
                                                <td style="width: 5px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRegistrationTime" Width="80px" ReadOnly="true" runat="server"
                                                        Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Pasien")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPatientInfo" Width="100%" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("No. Transaksi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <%=GetLabel("Tanggal-Jam Hasil") %></label>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-right: 1px; width: 145px">
                                                    <asp:TextBox ID="txtResultDate" Width="120px" ReadOnly="true" runat="server" Style="text-align: center" />
                                                </td>
                                                <td style="width: 5px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtResultTime" Width="80px" ReadOnly="true" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table>
                                <colgroup>
                                    <col style="width: 180px" />
                                    <col style="width: 150px" />
                                    <col style="width: 80px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("No. Order")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <%=GetLabel("Tanggal-Jam Order") %>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-right: 1px; width: 145px">
                                                    <asp:TextBox ID="txtOrderDate" Width="120px" ReadOnly="true" runat="server" Style="text-align: center" />
                                                </td>
                                                <td style="width: 5px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrderTime" Width="80px" ReadOnly="true" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px">
                                        <%=GetLabel("Catatan Hasil")%>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div style="height: 300px; overflow-y: auto">
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; max-height: 300px; overflow-y: auto">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="350px">
                                                <ItemTemplate>
                                                    <div>
                                                        [<%#: Eval("ItemCode") %>]
                                                        <%#: Eval("ItemName") %></div>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
