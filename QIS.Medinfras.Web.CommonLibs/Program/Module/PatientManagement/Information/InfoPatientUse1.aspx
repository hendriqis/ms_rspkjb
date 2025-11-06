<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="InfoPatientUse1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientUse1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.lnkReq').click(function () {

                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/InfoPatientRequestCtl1.ascx");
                openUserControlPopup(url, id, 'Permintaan Pasien', 1000, 200);
            });


            $('.lnkDisc').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/InfoPatientDistribusiCtl1.ascx");
                openUserControlPopup(url, id, 'Distribusi', 1000, 200);
            });

            $('.lnkCahrge').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/InfoPatientChargesCtl1.ascx");
                openUserControlPopup(url, id, 'Digunakan oleh Pasien', 1000, 200);
            });
            $('.lnkUse').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Information/InfoPatientUseCtl1.ascx");
                openUserControlPopup(url, id, 'Digunakan oleh Pasien', 1000, 200);
            });


            $('.grdBilling tr:gt(0):not(.trEmpty)').click(function () {
                if ($('.grdBilling tr').index($(this)) > 1) {
                    $('.grdBilling tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.hdnKeyField').val());
                }
            });

            $('.grdBilling tr:eq(2)').click();
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            if (registrationID != ''  || registrationID !='0') {
                if (code == 'IP-00206') {
                    filterExpression.text = registrationID;
                    return true;
                } else {
                    errMessage.text = "ERROR";
                    return false;
                }
            } else {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
        }
        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
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
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 100px">
                                                            <%= GetLabel("Code Item")%>
                                                        </th>
                                                         <th style="width: 500px">
                                                            <%= GetLabel("Nama Item")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%= GetLabel("Qty Minta")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Distribusi")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Charges")%>
                                                        </th>
                                                          <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Use")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("Tidak ada informasi pemakaian pasien pada saat ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdBilling"
                                                    cellspacing="0" rules="all">
                                                    <tr>
                                                        <th style="width: 100px">
                                                                <%= GetLabel("Code Item")%>
                                                        </th>
                                                        <th style="width: 500px">
                                                                <%= GetLabel("Nama Item")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%= GetLabel("Qty Minta")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Distribusi")%>
                                                        </th>
                                                        <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Charges")%>
                                                        </th>
                                                          <th style="width: 20px" align="right">
                                                            <%=GetLabel("Qty Use")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trItem">
                                             <td align="left">
                                                        <div>
                                                            <%#: Eval("ItemCode") %></div>
                                                    </td>
                                                 <td align="left">
                                                        <div>
                                                            <%#: Eval("ItemName1") %></div>
                                                    </td>
                                                     <td align="right">
                                                        <div>
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("ItemID")%>" />
                                                            <a class="lnkReq" >
                                                                <%#: Eval("ItemRequestQtyOnOrder","{0:N2}") %></div>
                                                    </td>
                                                   <td align="right">
                                                        <div>
                                                            <a class="lnkDisc">
                                                                <%#: Eval("ItemOnDistributionInQty","{0:N2}") %></div>
                                                    </td>
                                                   <td align="right">
                                                        <div>
                                                            <a class="lnkCahrge" >
                                                                <%#: Eval("ChargedQty", "{0:N2}") %></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <a class="lnkUse">
                                                                <%#: Eval("ItemOnUseInQty", "{0:N2}") %></div>
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
