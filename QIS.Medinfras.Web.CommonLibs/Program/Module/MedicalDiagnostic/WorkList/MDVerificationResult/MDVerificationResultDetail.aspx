<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MDVerificationResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDVerificationResultDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnVerificationBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVerified" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Verified")%></div>
    </li>
    <li id="btnUnverified" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Unverified")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <uc1:patientbannerctl id="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var selectedItemID = "";
        $('.lblRefreshOrder').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                selectedItemID = $(this).closest('tr').find('.keyField').val();
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var paramResultTest = selectedItemID + "|" + imagingID;
                var filterExpression = "ID = " + imagingID + " AND ItemID = " + selectedItemID;
                Methods.getObject('GetImagingResultDtList', filterExpression, function (result) {
                    if (result != null) {
                        var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultDetailVerifiedCtl.ascx");
                        openUserControlPopup(url, paramResultTest, 'Test Result Detail', 800, 600);
                    }
                    else {
                        showToast('Warning', 'No data to display');
                    }
                });
            }
        });

        function onLoad() {
            $('#<%=btnVerificationBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDVerificationResult/MDVerificationResultList.aspx");
            });

            $('#<%=btnVerified.ClientID %>').click(function () {
                onCustomButtonClick('verified');
            });

            $('#<%=btnUnverified.ClientID %>').click(function () {
                onCustomButtonClick('unverified');
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == 'unverified')
                $('#<%=btnVerificationBack.ClientID %>').click();
            else if (type == 'verified') {
                showToast('Process Success', 'Proses Verifikasi Sudah Berhasil Dilakukan', function () {
                    $('#<%=btnVerificationBack.ClientID %>').click();
                });
            }
        }

        $('#chkSelectAllResult').die('change');
        $('#chkSelectAllResult').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function onBeforeLoadRightPanelContent(code) {
            var param = '';
            var transactionID = $('#<%:hdnTransactionHdID.ClientID %>').val();

            if (code == 'sendResultToRIS') {
                param = transactionID;
            }

            return param;
        }

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnMDResultID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td colspan="2">
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 360px;
                                    overflow-y: auto">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 50px" align="center">
                                                        <input id="chkSelectAllResult" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pemeriksaan/Pelayanan")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 200px" align="center">
                                                        <%=GetLabel("Hasil")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="6">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 50px" align="center">
                                                        <input id="chkSelectAllResult" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pemeriksaan/Pelayanan")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 200px" align="center">
                                                        <%=GetLabel("Hasil")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ItemID")%>' />
                                                <td>
                                                    <div>
                                                        <%#: Eval("ItemName1") %></div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("ParamedicName") %></div>
                                                </td>
                                                <label>
                                                </label>
                                                <td>
                                                    <div>
                                                        <center>
                                                            <label class="lblRefreshOrder lblLink" id="lblRefreshOrder">
                                                        Hasil</div>
                                                    </center>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
