<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalListDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalListDtCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
   
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnGLTransactionID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Detail Jurnal")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Jurnal")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtJournalNo" ReadOnly="true" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelompok Jurnal")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtJournalGroup" ReadOnly="true" Width="600px" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("COA")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 14px;">
                                                    <%#:Eval("GLAccountNo") %></div>
                                                <div style="font-size: 12px;">
                                                    <%#:Eval("GLAccountName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Segment") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 12px;">
                                                    DP:
                                                    <%#:Eval("DepartmentID") %></div>
                                                <div style="font-size: 12px;">
                                                    SU:
                                                    <%#:Eval("ServiceUnitName") %></div>
                                                <div style="font-size: 12px;">
                                                    RC:
                                                    <%#:Eval("RevenueCostCenterName") %></div>
                                                <div style="font-size: 12px;">
                                                    CG:
                                                    <%#:Eval("CustomerGroupName") %></div>
                                                <div style="font-size: 12px;">
                                                    BP:
                                                    <%#:Eval("BusinessPartnerName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan Transaksi" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DebitAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Debit" HeaderStyle-Width="120px" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="CreditAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Kredit" HeaderStyle-Width="120px" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="ReferenceNo" HeaderText="No. Referensi" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="130px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 450px;">
                        <div class="lblComponent" style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Informasi Jurnal") %></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="400px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="10px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dibuat Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divCreatedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Dibuat Pada") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divCreatedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trApprovedBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Approved Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trApprovedDate" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Approved Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidBy" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Void Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVoidDate" runat="server">
                                    <td align="left">
                                        <%=GetLabel("Void Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divVoidDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divLastUpdatedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Terakhir Diubah Pada")%>
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td>
                                        <div runat="server" id="divLastUpdatedDate">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td style="float: right;">
                    <table width="300px">
                        <colgroup>
                            <col width="120px" />
                        </colgroup>
                        <tr>
                            <td>
                                <div class="lblComponent" style="text-align: left">
                                    <%=GetLabel("Total Debit") %></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalDebet" runat="server" CssClass="txtCurrency" Width="100%"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="lblComponent" style="text-align: left">
                                    <%=GetLabel("Total Kredit") %></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalKredit" runat="server" CssClass="txtCurrency" Width="100%"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="lblComponent" style="text-align: left">
                                    <%=GetLabel("Total Selisih") %></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalSelisih" runat="server" CssClass="txtCurrency" Width="100%"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
