<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="InformasiPembayaranSupplierDetailInvoice.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InformasiPembayaranSupplierDetailInvoice" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        function onLoad() {
            $(function () {
                var grd = new customGridView2();
                grd.init('grdView', 'pnlView', cbpView, 'paging');

                $('#<%=btnRefresh.ClientID %>').click(function () {
                    cbpView.PerformCallback('refresh');
                });
            });

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:filterExpressionSupplier %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').click(function () {
                openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                    $('#<%=txtSupplierCode.ClientID %>').val(value);
                    onTxtSupplierChanged(value);
                });
            });

            $('#<%=txtSupplierCode.ClientID %>').change(function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        cboTerm.SetValue('');
                    }
                });
            }

            //#endregion
        }

        $('.lblDetail.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var purchaseInvoiceID = $tr.find('.hiddenColumn').html();
            var url = ResolveUrl("~/Program/Information/InformasiPembayaranSupplierCtl.ascx");
            openUserControlPopup(url, purchaseInvoiceID, 'Detail Pembayaran', 1000, 450);
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdView tr:eq(2)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdView tr:eq(2)').click();
        }
        //#endregion

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        function onCboTypeValueChanged() {
            onRefreshGrid();
        }

        function onCboDepartmentValueChanged() {
            onRefreshGrid();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width: 650px;">
                    <colgroup>
                        <col style="width: 250px" />
                        <col style="width: 600px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblSupplier" runat="server">
                                <%=GetLabel("Supplier")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                            runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblNormal lblMandatory">
                                    <%:GetLabel("Pencarian Berdasarkan")%></label></div>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblDataSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text=" No.Tukar Faktur" Value="1" Selected="True" />
                                <asp:ListItem Text=" No.Penerimaan" Value="2" />
                                <asp:ListItem Text=" No.Referensi" Value="3" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trTransactionNo" runat="server">
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblNormal" id="lblNoReg" runat="server">
                                    <%:GetLabel("Nomor Pencarian")%></label></div>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSearchNo" Width="150px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PurchaseInvoiceID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                        <asp:TemplateField HeaderText="No. Tukar Faktur" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="170px">
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-weight: bold">
                                                    <%#:Eval("PurchaseInvoiceNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Tukar Faktur = ")%><%#:Eval("cfPurchaseInvoiceDateInString")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Jatuh Tempo = ")%><%#:Eval("cfDueDateInString")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="170px">
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("BusinessPartnerCode")%></label>
                                                <br />
                                                <label class="lblNormal">
                                                    <%#:Eval("BusinessPartnerName")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Line" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("ProductLineCode")%></label>
                                                <br />
                                                <label class="lblNormal">
                                                    <%#:Eval("ProductLineName")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Informasi Penerimaan" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("PurchaseReceiveNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Penerimaan = ")%><%#:Eval("cfReceivedDateInString")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("No. Ref = ")%><%#:Eval("ReferenceNo")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Informasi Konsinyasi" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("PurchaseOrderNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Pemesanan = ")%><%#:Eval("cfOrderDateInString")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Informasi Nota Kredit" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("CreditNoteNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Nota Kredit = ")%><%#:Eval("cfCreditNoteDateInString")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("No. Retur = ")%><%#:Eval("PurchaseReturnNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                    <%:GetLabel("Tgl. Retur = ")%><%#:Eval("cfReturnDateInString")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Transaksi" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("LineAmount", "{0:N}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status Bayar" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <label class="lblDetail lblLink">
                                                    <%#:Eval("Status")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
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
            </td>
        </tr>
    </table>
</asp:Content>
