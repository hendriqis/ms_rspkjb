<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoTransactionDetailPharmacyCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InfoTransactionDetailPharmacyCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_infochargepharmacy">

</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("Registration No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="170px" runat="server" ReadOnly="true"
                                Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("SEP No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNo" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Patient")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatient" Width="350px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th rowspan="2">
                                                    <%=GetLabel("No. Transaksi")%>
                                                </th>
                                                <th rowspan="2" style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                </th>
                                                <th colspan="4" align="center">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Paket")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Ditagihkan")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Dibayar Pasien")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Lainnya")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th rowspan="2">
                                                    <%=GetLabel("No. Transaksi")%>
                                                </th>
                                                <th rowspan="2" style="width: 100px" align="center">
                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                </th>
                                                <th colspan="4" align="center">
                                                    <%=GetLabel("Nilai")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Paket")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Ditagihkan")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Dibayar Pasien")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Lainnya")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#: Eval("TransactionNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfTransactionDateInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfNilaiPaketInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfNilaiDitagihkanInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfNilaiDibayarPasienInString")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfNilaiLainnyaInString")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
