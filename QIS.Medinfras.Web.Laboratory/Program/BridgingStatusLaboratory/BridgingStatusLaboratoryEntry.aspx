<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="BridgingStatusLaboratoryEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.BridgingStatusLaboratoryEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnResendBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnResend" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Resend")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            onLoadSelectedChecked();

            $('#<%=btnResendBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/BridgingStatusLaboratory/BridgingStatusLaboratoryList.aspx');
            });
            $('#<%=btnResend.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                        if (param != '')
                            param += '|';
                        param += trxID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('resend');
                }
            });

            $('.lnkTransactionNo').click(function () {
                var $tr = $(this).closest('tr');
                var id = $tr.find('.hdnKeyField').val();
                var url = '';
                url = ResolveUrl("~/Program/BridgingStatusLaboratory/TransactionDetailLaboratoryCtl.ascx");
                openUserControlPopup(url, id, 'Detail Item', 600, 500);
            });
        });

        function onLoadSelectedChecked() {
            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
            });
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            onLoadSelectedChecked();
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTrxCode" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
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
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" Visible="true" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 200px;">
                                                                <div>
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: left;">
                                                               <%= GetLabel("Bridging Status")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="6">
                                                            <%=GetLabel("Tidak ada transaksi pemeriksaan untuk pasien ini") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll"  Visible="true" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date")%></div>
                                                            </div>
                                                            <div style="padding: 3px; margin-left: 200px;">
                                                                <div>
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center;">
                                                               <%= GetLabel("Bridging Status")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: right; margin-right: 50px; <%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' />
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblInfo">
                                                                            <%=GetLabel("Pending Recalculated") %></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("TransactionNo")%></a>
                                                            <div>
                                                                <%#: Eval("TransactionDateInString")%></div>
                                                        </div>
                                                        <div style="padding: 3px; margin-left: 200px;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                            <div>
                                                                <%#: Eval("LastUpdatedByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center;">
                                                            <div>
                                                                <%#: Eval("LISBridgingStatus")%></div>
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
