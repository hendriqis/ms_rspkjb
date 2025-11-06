<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPaymentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoPaymentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_InfoPaymentCtl">
    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<div style="position: relative;">
    <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 100%;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <%= GetLabel("No. Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%= GetLabel("Waktu Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px" align="right">
                                                            <%=GetLabel("Total Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Status Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("Tidak ada informasi tagihan pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 150px">
                                                                <%= GetLabel("No. Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%= GetLabel("Waktu Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px" align="right">
                                                            <%=GetLabel("Total Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Status Pembayaran")%>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <%=GetLabel("Dibuat Oleh")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trItem">
                                                    <td align="center">
                                                        <div>
                                                                <%#: Eval("PaymentNo") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("PaymentDateTimeInString") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("PaymentType") %></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <%#: Eval("TotalPaymentAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("TransactionStatusWatermark") %></div>
                                                    </td>
                                                    <td align="center">
                                                        <div>
                                                            <%#: Eval("CreatedByUser") %></div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
</div>
