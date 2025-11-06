<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PatientBillSummaryDiscountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryDiscountEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        $('.lnkTransactionNo').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscount/PatientBillSummaryDiscountDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Bill', 1100, 500);
        });

        $('.lnkChangeDiscount').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillDiscount/PatientBillSummaryDiscountNewEntryCtl.ascx");
            openUserControlPopup(url, id, 'Ubah Bill Diskon', 1200, 550);
        });

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
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
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No Bill")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 200px;">
                                                                <div>
                                                                    <%= GetLabel("Dibuat Oleh")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 120px">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left" style="width: 250px">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("No Bill")%></div>
                                                                <div>
                                                                    <%= GetLabel("Tanggal")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px" colspan="3">
                                                            <div style="text-align: center; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px" colspan="3">
                                                            <div style="text-align: center; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px" colspan="3">
                                                            <div style="text-align: center; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding-right: 3px">
                                                                <%=GetLabel("Alasan Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 100px">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("PatientBillingNo")%></a>
                                                            <div>
                                                                <%#: Eval("BillingDateTimeInString")%></div>
                                                            <div>
                                                                <i>
                                                                    <%=GetLabel("Dibuat Oleh : ")%></i><%#: Eval("CreatedByFullName")%>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("TotalPayerAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("PayerDiscountAmount", "{0:N}")%></b></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("TotalPayer", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("TotalPatientAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("PatientDiscountAmount", "{0:N}")%></b></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("TotalPatient", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("TotalAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("DiscountAmount", "{0:N}")%></b></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("Total", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px">
                                                            <div>
                                                                <%#: Eval("DiscountReason")%></div>
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <a class="lnkChangeDiscount">
                                                                <%=GetLabel("Ubah Diskon") %></a>
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
