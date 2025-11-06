<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPatientTransferCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientTransferCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationNo" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div style="padding: 5px; max-height: 400px;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewInfoPatientTransfer" runat="server" Width="100%" ClientInstanceName="cbpViewInfoPatientTransfer"
                        ShowLoadingPanel="false" OnCallback="cbpViewInfoPatientTransfer_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { oncbpViewInfoPatientTransferEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView ID="lvwView" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("Transfer Information")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("From")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("To")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="5">
                                                        <%=GetLabel("Tidak ada informasi mutasi pasien pada saat ini") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("Transfer Information")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%= GetLabel("From")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("To")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trMutationDetail">
                                                <td>
                                                    <div style="float: left">
                                                        <%#: Eval("TransferDateInString")%><br>
                                                        <%#: Eval("TransferTime")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("FromServiceUnitName")%>
                                                        |
                                                        <%#: Eval("FromRoomName")%>
                                                        |
                                                        <%#: Eval("FromBedCode")%></div>
                                                    <div>
                                                        <i>Kelas - Kelas Tagihan </i>:
                                                        <%#: Eval("FromClassName")%>
                                                        -
                                                        <%#: Eval("FromChargeClassName")%></div>
                                                    <div>
                                                        Masuk : Tanggal
                                                        <%#: Eval("CfFromDateIN")%>
                                                        | Jam
                                                        <%#: Eval("CfFromTimeIN")%></div>
                                                    <div>
                                                        Keluar : Tanggal
                                                        <%#: Eval("CfFromDateOut")%>
                                                        | Jam
                                                        <%#: Eval("CfFromTimeOUT")%></div>
                                                    <div>
                                                        LOS In Day :
                                                        <%#: Eval("LOSInDaysFrom")%>
                                                        | LOS In Hours :
                                                        <%#: Eval("LOSInHoursFrom")%>
                                                        | LOS In Minute :
                                                        <%#: Eval("LOSInMinutesFrom")%></div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("ToServiceUnitName")%>
                                                        |
                                                        <%#: Eval("ToRoomName")%>
                                                        |
                                                        <%#: Eval("ToBedCode")%></div>
                                                    <div>
                                                        <i>Kelas - Kelas Tagihan </i>:
                                                        <%#: Eval("ToClassName")%>
                                                        -
                                                        <%#: Eval("ToChargeClassName")%></div>
                                                    <div>
                                                        Masuk : Tanggal
                                                        <%#: Eval("CfToDateIN")%>
                                                        | Jam
                                                        <%#: Eval("CfToTimeIN")%></div>
                                                    <div>
                                                        Keluar : Tanggal
                                                        <%#: Eval("CfToDateOUT")%>
                                                        | Jam
                                                        <%#: Eval("CfToTimeOUT")%></div>
                                                    <div>
                                                        LOS In Day :
                                                        <%#: Eval("LOSInDaysTo")%>
                                                        | LOS In Hours :
                                                        <%#: Eval("LOSInHoursTo")%>
                                                        | LOS In Minute :
                                                        <%#: Eval("LOSInMinutesTo")%></div>
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
