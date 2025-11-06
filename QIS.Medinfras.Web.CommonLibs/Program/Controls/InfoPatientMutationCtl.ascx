<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPatientMutationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoPatientMutationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_InfoPatientMutationCtl">
    function onGetCurrID() {
        return $('#<%=hdnID.ClientID %>').val();
    }
</script>
<input type="hidden" value="" id="hdnRegistrationID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<div style="position: relative;">
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("No. Registrasi : ")%></label>
                <asp:TextBox ID="txtPrintTotal" runat="server" Width="200px" ReadOnly=true />
            </td>
        </tr>
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" CssClass="pnlContainerGrid" Style="width: 100%; height: 400px; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdMutation"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 80px">
                                                            <%= GetLabel("Transfer Information")%>
                                                        </th>
                                                        <th style="width: 185px">
                                                            <%= GetLabel("From")%>
                                                        </th>
                                                        <th style="width: 185px">
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
                                                        <th style="width: 80px">
                                                            <%= GetLabel("Transfer Information")%>
                                                        </th>
                                                        <th style="width: 185px">
                                                            <%= GetLabel("From")%>
                                                        </th>
                                                        <th style="width: 185px">
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
                                                            <i>
                                                            Kelas - Kelas Tagihan
                                                            </i>
                                                            :
                                                            <%#: Eval("FromClassName")%>
                                                            -
                                                            <%#: Eval("FromChargeClassName")%></div>
                                                        <div>
                                                            Masuk : Tanggal <%#: Eval("CfFromDateIN")%>
                                                            | 
                                                            Jam <%#: Eval("CfFromTimeIN")%></div>
                                                        <div>
                                                            Keluar : Tanggal <%#: Eval("CfFromDateOut")%>
                                                            |
                                                            Jam <%#: Eval("CfFromTimeOUT")%></div>
                                                        <div>                                                            
                                                            LOS In Day : <%#: Eval("LOSInDaysFrom")%>
                                                            |
                                                            LOS In Hours : <%#: Eval("LOSInHoursFrom")%>
                                                            |
                                                            LOS In Minute : <%#: Eval("LOSInMinutesFrom")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <%#: Eval("ToServiceUnitName")%>
                                                            |
                                                            <%#: Eval("ToRoomName")%>
                                                            |
                                                            <%#: Eval("ToBedCode")%></div>
                                                        <div>
                                                            <i>
                                                            Kelas - Kelas Tagihan
                                                            </i>
                                                            :
                                                            <%#: Eval("ToClassName")%>
                                                            -
                                                            <%#: Eval("ToChargeClassName")%></div>
                                                        <div>
                                                            Masuk : Tanggal <%#: Eval("CfToDateIN")%>
                                                            | 
                                                            Jam <%#: Eval("CfToTimeIN")%></div>
                                                        <div>
                                                            Keluar : Tanggal <%#: Eval("CfToDateOUT")%>
                                                            | 
                                                            Jam <%#: Eval("CfToTimeOUT")%></div>
                                                        <div>                                                            
                                                            LOS In Day : <%#: Eval("LOSInDaysTo")%>
                                                            |
                                                            LOS In Hours : <%#: Eval("LOSInHoursTo")%>
                                                            |
                                                            LOS In Minute : <%#: Eval("LOSInMinutesTo")%></div> 
                                                        </div>
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
