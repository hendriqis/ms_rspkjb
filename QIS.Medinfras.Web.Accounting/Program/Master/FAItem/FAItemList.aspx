<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="FAItemList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setRightPanelButtonEnabled();
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lnkFADepreciation a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/FAItem/FADepreciationEntryCtl.ascx");
            openUserControlPopup(url, id, 'Proses Depresiasi', 900, 500);
        });

        $('.lnkPartsDetail a').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);

            if (entity.ParentID != 0) {
                displayMessageBox('INFORMATION', "Data tidak ditemukan karena aset ini merupakan parts/detail dari aset <b> " + entity.ParentFAName + " (" + entity.ParentFACode + ") </b>.");
            } else {
                var id = $row.find('.keyField').html();
                var url = ResolveUrl("~/Program/Master/FAItem/FAPartsDetailCtl.ascx");
                openUserControlPopup(url, id, 'Parts/Detail', 1000, 500);
            }
        });

        function onCboStatusFAAcceptanceValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        function setRightPanelButtonEnabled() {
            $('#btnFAItemChangeAfterAcceptance').attr('enabled', 'false');
        }

        function onBeforeLoadRightPanelContent(code, error) {
            var oFixedAssetID = $('#<%=hdnID.ClientID %>').val();
            if (code == 'FAItemChangeAfterAcceptance') {
                return oFixedAssetID;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var itemID = $('#<%=hdnID.ClientID %>').val();
            var menuCode = $('#<%=hdnMenuCode.ClientID %>').val();
            if (code == "AC-00004") {
                filterExpression.text = itemID + '|' + menuCode;
                return true;
            } else if (code == "AC-00016") {
                filterExpression.text = hdnID;
                return true;
            }
        }

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnMenuCode" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <colgroup>
            <col style="width: 70%" />
            <col style="width: 30%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="tdLabel">
                                <%=GetLabel("Status Berita Acara")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboStatusFAAcceptance" ClientInstanceName="cboStatusFAAcceptance"
                                Width="180px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { onCboStatusFAAcceptanceValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
            </td>
            <td>
            </td>
        </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="FixedAssetID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderText="Aset & Inventaris" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="font-size: 14px; font-weight:bold">
                                            <%#:Eval("FixedAssetCode") %></div>
                                        <div style="font-size: 14px;">
                                            <%#:Eval("FixedAssetName") %></div>
                                        <div style="font-size: 12px;">
                                            S/N:<%#:Eval("SerialNumber") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("Remarks") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Penerimaan Barang")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("ProcurementNumber") %>|<%#:Eval("ProcurementDateInString") %></div>
                                        <div style="font-size: 12px;">
                                            No. Faktur:
                                            <%#:Eval("ProcurementReferenceNumber") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("BusinessPartnerName") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Kelompok & Lokasi")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 12px;">
                                            [<%#:Eval("FAGroupCode") %>]
                                            <%#:Eval("FAGroupName") %></div>
                                        <div style="font-size: 12px;">
                                            [<%#:Eval("FALocationCode") %>]
                                            <%#:Eval("FALocationName") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("BudgetCategory") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("BudgetPlanNo") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Berita Acara")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 14px; font-weight:bold">
                                            <%#:Eval("FAAcceptanceNo") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("cfAcceptanceDateInString") %></div>
                                        <div style="font-size: 12px;">
                                            <%#:Eval("FAAcceptanceTransactionStatus") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Garansi")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="font-size: 12px;">
                                            Mulai:
                                            <%#:Eval("GuaranteeStartDateInString") %></div>
                                        <div style="font-size: 12px;">
                                            Berakhir:
                                            <%#:Eval("GuaranteeEndDateInString") %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="gridColumnLink lnkPartsDetail" HeaderText="Parts/Detail"
                                    HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ParentID") %>" bindingfield="ParentID" />
                                        <input type="hidden" value="<%#:Eval("ParentFACode") %>" bindingfield="ParentFACode" />
                                        <input type="hidden" value="<%#:Eval("ParentFAName") %>" bindingfield="ParentFAName" />
                                        <a>
                                            <%=GetLabel("Parts/Detail")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="gridColumnLink lnkFADepreciation" HeaderText="Penyusutan"
                                    HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a>
                                            <%=GetLabel("Proses") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
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
</asp:Content>
