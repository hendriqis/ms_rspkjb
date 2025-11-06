<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingCheckdPRSDoubleChargesViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingCheckdPRSDoubleChargesViewCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_RevenueSharingCheckdPRSDoubleChargesViewCtl">
    function oncbpViewDoubleChargesPopUpCtlEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div>
    <input type="hidden" id="hdnRSSummaryID" value="" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <div style="position: relative;" id="doubleView">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDoubleChargesPopUpCtl" runat="server" Width="100%"
                        ClientInstanceName="cbpViewDoubleChargesPopUpCtl" ShowLoadingPanel="false" OnCallback="cbpViewDoubleChargesPopUpCtl_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewDoubleChargesPopUpCtlEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <table class="grdRevenueSharing grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th class="keyField">
                                                &nbsp;
                                            </th>
                                            <th style="text-align: left; width: 150px">
                                                <%=GetLabel("Informasi PRS") %>
                                            </th>
                                            <th style="text-align: left; width: 200px">
                                                <%=GetLabel("Informasi Registrasi") %>
                                            </th>
                                            <th style="text-align: left; width: 150px">
                                                <%=GetLabel("Informasi Transaksi") %>
                                            </th>
                                            <th style="text-align: left">
                                                <%=GetLabel("Item") %>
                                            </th>
                                            <th style="text-align: center; width: 100px">
                                                <%=GetLabel("Kode JasMed") %>
                                            </th>
                                            <th style="text-align: right; width: 150px">
                                                <%=GetLabel("Nilai Transaksi") %>
                                            </th>
                                            <th style="text-align: right; width: 150px">
                                                <%=GetLabel("Nilai Dokter") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#:Eval("PatientChargesDtID") %>
                                                    </td>
                                                    <td>
                                                        <label style="font-weight: bold">
                                                            <%#:Eval("RevenueSharingNo") %>
                                                        </label>
                                                        <br />
                                                        <label style="font-size: smaller; font-style: italic">
                                                            <%=GetLabel("Tgl PRS = ")%>
                                                        </label>
                                                        <label style="font-size: smaller;">
                                                            <%#:Eval("cfProcessedDateInString") %>
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <label style="font-weight: bold">
                                                            <%#:Eval("RegistrationNo") %>
                                                        </label>
                                                        <br />
                                                        <label style="font-weight: bold; font-size: x-small; font-style: italic">
                                                            <%=GetLabel("(")%><%#:Eval("MedicalNo") %><%=GetLabel(") ")%>
                                                        </label>
                                                        <label style="font-weight: bold; font-size: smaller">
                                                            <%#:Eval("PatientName") %>
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <label style="font-weight: bold">
                                                            <%#:Eval("PatientChargesTransactionNo") %>
                                                        </label>
                                                        <br />
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%=GetLabel("Dibuat Oleh = ")%><%#:Eval("PatientChargesDtCreatedByName") %>
                                                            <br />
                                                            <%=GetLabel("Tgl Dibuat = ")%><%#:Eval("cfPatientChargesDtCreatedDateInFullString") %>
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <label style="font-weight: bold; font-size: x-small; font-style: italic">
                                                            <%=GetLabel("(")%><%#:Eval("ItemCode") %><%=GetLabel(") ")%>
                                                        </label>
                                                        <label style="font-weight: bold; font-size: smaller">
                                                            <%#:Eval("ItemName1") %>
                                                        </label>
                                                    </td>
                                                    <td align="center">
                                                        <%#:Eval("RevenueSharingCode") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("cfTransactionAmountInString") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("cfRevenueSharingAmountInString") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
