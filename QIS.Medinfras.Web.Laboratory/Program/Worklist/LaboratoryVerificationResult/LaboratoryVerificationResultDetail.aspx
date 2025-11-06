<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="LaboratoryVerificationResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryVerificationResultDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnVerifiedResultBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVerified" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Verified")%></div>
    </li>
    <li id="btnUnverified" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Unverified")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <uc1:patientbannerctl id="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnVerifiedResultBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Worklist/LaboratoryVerificationResult/LaboratoryVerificationResultList.aspx');
            });

            $('#<%=btnUnverified.ClientID %>').click(function () {
                showToastConfirmation('Lakukan proses PEMBATALAN VERIFIKASI hasil pemeriksaan ?', function (result) {
                    if (result) {
                        onCustomButtonClick('unverified');
                        showLoadingPanel();
                    }
                });
            });

            $('#<%=btnVerified.ClientID %>').click(function () {
                showToastConfirmation('Lakukan proses VERIFIKASI hasil pemeriksaan ?', function (result) {
                    if (result) {
                        onCustomButtonClick('verified');
                        showLoadingPanel();
                    }
                });
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == 'unverified')
                $('#<%=btnVerifiedResultBack.ClientID %>').click();
            else if (type == 'verified') {
                showToast('Process Success', 'Proses Verifikasi Sudah Berhasil Dilakukan', function () {
                    $('#<%=btnVerifiedResultBack.ClientID %>').click();
                });
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                getCheckedMember();
                cbpView.PerformCallback('refresh');
            }
        }

        $('#chkSelectAllResult').die('change');
        $('#chkSelectAllResult').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });
            
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedItem" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" value="" id="hdnLabResultID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <dxcp:aspxcallbackpanel id="cbpView" runat="server" width="100%" clientinstancename="cbpView"
                        showloadingpanel="false" oncallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:50px" align="center"><input id="chkSelectAllResult" type="checkbox" /></th>
                                                <th style="width:350px" align="left"><%=GetLabel("Pemeriksaan")%></th>
                                                <th style="width:300px" align="left"><%=GetLabel("Artikel Pemeriksaan")%></th>
                                                <th align="right"><%=GetLabel("Hasil")%></th>                                   
                                                <th style="width:80px" align="left"><%=GetLabel("Satuan")%></th>
                                                <th style="width:200px" align="left"><%=GetLabel("Nilai Referensi")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="6">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:50px" align="center"><input id="chkSelectAllResult" type="checkbox" /></th>
                                                <th style="width:350px" align="left"><%=GetLabel("Pemeriksaan")%></th>
                                                <th style="width:300px" align="left"><%=GetLabel("Artikel Pemeriksaan")%></th>
                                                <th  align="right"><%=GetLabel("Hasil")%></th>                                   
                                                <th style="width:80px" align="left"><%=GetLabel("Satuan")%></th>
                                                <th style="width:200px" align="left"><%=GetLabel("Nilai Referensi")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("CustomID")%>' />
                                            </td>
                                            <td><div><%#: Eval("ItemName1") %></div></td>
                                            <td><div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'><%#: Eval("FractionName1") %></div></td>
                                            <td align="right">
                                                <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'>
                                                    <%# Server.HtmlDecode(Eval("cfTestResultValue").ToString()) %>
                                                </div>
                                            </td>
                                            <td align="left"><div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'><%#: Eval("MetricUnit") %></div></td>
                                            <td><div><%#: Eval("cfTestReferenceValue") %></div></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:aspxcallbackpanel>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>