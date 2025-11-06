<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ClaimOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ClaimOrder" %>

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
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClaimOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Order")%></div>
    </li>
    <li id="btnCancelClaimOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Batal Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnClaimOrder.ClientID %>').click(function () {
                onCustomButtonClick('claimorder');
            });

            $('#<%=btnCancelClaimOrder.ClientID %>').click(function () {
                onCustomButtonClick('cancelclaimorder');
            });
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
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
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 100px" align="left">
                                                            <%=GetLabel("No RM")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th style="width: 150px" align="left">
                                                            <%=GetLabel("No SEP")%>
                                                        </th>
                                                        <th style="width: 150px" align="center">
                                                            <%=GetLabel("Tgl-Jam SEP")%>
                                                        </th>
                                                        <th style="width: 50px" align="center">
                                                            <%=GetLabel("Dx")%>
                                                        </th>
                                                        <th style="width: 50px" align="center">
                                                            <%=GetLabel("Order Klaim")%>
                                                        </th>
                                                        <th style="width: 150px" align="right">
                                                            <%=GetLabel("Order Klaim Terakhir")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("Tidak ada informasi pendaftaran pasien pada tanggal yang dipilih")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 100px" align="left">
                                                            <%=GetLabel("No RM")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th style="width: 150px" align="left">
                                                            <%=GetLabel("No SEP")%>
                                                        </th>
                                                        <th style="width: 150px" align="center">
                                                            <%=GetLabel("Tgl-Jam SEP")%>
                                                        </th>
                                                        <th style="width: 50px" align="center">
                                                            <%=GetLabel("Dx")%>
                                                        </th>
                                                        <th style="width: 50px" align="center">
                                                            <%=GetLabel("Order Klaim")%>
                                                        </th>
                                                        <th style="width: 150px" align="center">
                                                            <%=GetLabel("Order Klaim Terakhir")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#: Eval("MedicalNo") %>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("PatientName") %>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("NoSEP") %>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("cfSEPDateTimeInString") %>
                                                    </td>
                                                    <td align="center">
                                                        <div id="divDiagnosis" runat="server" style="text-align: center; color: blue">
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div id="divOrderClaim" runat="server" style="text-align: center; color: blue">
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("OrderCodingByName") %> <br />
                                                        <%#: Eval("cfOrderCodingDateTimeInString") %>
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
