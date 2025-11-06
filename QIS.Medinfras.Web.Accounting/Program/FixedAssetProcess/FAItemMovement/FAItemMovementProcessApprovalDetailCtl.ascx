<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAItemMovementProcessApprovalDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemMovementProcessApprovalDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_FAItemMovementProcessApprovalDetailCtl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            getCheckedMember();
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<input type="hidden" id="hdnMovementID" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Detail Mutasi Gabungan Aset dan Inventaris")%></div>
<div style="height: 510px; overflow-y: auto" id="containerPopup">
    <table style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 15%" />
                        <col style="width: 40%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Mutasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMovementNo" ReadOnly="true" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal Mutasi") %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 120px">
                                        <asp:TextBox ID="txtMovementDate" ReadOnly="true" Width="120px" CssClass="datepicker"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Dari Lokasi")%>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromFALocationCode" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromFALocationName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                                <%=GetLabel("Kepada Lokasi")%>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtToFALocationCode" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToFALocationName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Jenis Mutasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMovementType" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" ReadOnly="true" TextMode="MultiLine"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView">
                                    <asp:ListView runat="server" ID="grdView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 20px" align="center">
                                                    </th>
                                                    <th style="width: 180px">
                                                        <%=GetLabel("Kode Aset dan Inventaris")%>
                                                    </th>
                                                    <th style="width: 200px">
                                                        <%=GetLabel("Nama Aset dan Inventaris")%>
                                                    </th>
                                                    <th style="width: 180px">
                                                        <%=GetLabel("No. Referensi")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Informasi Dibuat")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Informasi Diubah")%>
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
                                            <table id="tblView" runat="server" class="grdFAMovement grdView notAllowSelect"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 20px" align="center">
                                                    </th>
                                                    <th style="width: 180px">
                                                        <%=GetLabel("Kode Aset dan Inventaris")%>
                                                    </th>
                                                    <th style="width: 200px">
                                                        <%=GetLabel("Nama Aset dan Inventaris")%>
                                                    </th>
                                                    <th style="width: 180px">
                                                        <%=GetLabel("No. Referensi")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Informasi Dibuat")%>
                                                    </th>
                                                    <th style="width: 150px" align="center">
                                                        <%=GetLabel("Informasi Diubah")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                </td>
                                                <td>
                                                    <%#:Eval("FixedAssetCode")%>
                                                </td>
                                                <td>
                                                    <%#:Eval("FixedAssetName")%>
                                                </td>
                                                <td>
                                                    <%#:Eval("ReferenceNo")%>
                                                </td>
                                                <td align="center">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("CreatedByName") %></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("cfCreatedDateInString") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("LastUpdatedByName") %></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("cfLastUpdatedDateInString") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
