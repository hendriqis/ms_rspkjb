<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="InfoPatientMutation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientMutation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFromParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnToParamedicID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var resultObj = '';
            filterObj = 'RegistrationID = ' + registrationID;
            Methods.getObject('GetvPatientTransferList', filterObj, function (result) {
                if (result != null) {
                    resultObj = '1';
                } else {
                    resultObj = '0';
                }
            });
            if (resultObj == '1') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            } else {
                errMessage.text = 'Tidak Ada Riwayat Pindah';
                return false;
            }
        }   
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
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
</asp:Content>
