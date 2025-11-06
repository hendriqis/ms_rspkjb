<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderVoidDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderVoidDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseRequestBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnPurchaseRequestBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Procurement/PurchaseOrder/PurchaseOrderVoid/PurchaseOrderVoidList.aspx');
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Success', 'Approve All Success', function () {
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedMember.indexOf(key) < 0)
                        lstSelectedMember.push(key);
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedMember.indexOf(key) > -1)
                        lstSelectedMember.splice(lstSelectedMember.indexOf(key), 1);
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

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
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <div style="height: 435px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 25%" />
                <col style="width: 40%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 40%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Pemesanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemOrderDate" Width="100%" CssClass="datepicker" runat="server"
                                    ReadOnly="true" />
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
                            <td class="tdLabel">
                                <%=GetLabel("Nama Supplier") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2"
                                    ReadOnly="true" />
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
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Jumlah Nilai")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJumlahNilai" Width="230px" ReadOnly="true" runat="server" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Diskon Final") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiskonFinal" Width="230px" ReadOnly="true" runat="server" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("PPN (10%)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPPN" Width="230px" ReadOnly="true" runat="server" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Saldo Nilai") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSaldoNilai" Width="230px" ReadOnly="true" runat="server" Style="text-align: right" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Item Name" />
                                            <asp:BoundField DataField="BaseUnit" ItemStyle-HorizontalAlign="Center" HeaderText="Unit"
                                                HeaderStyle-Width="80px" />
                                            <asp:BoundField DataField="QtyMin" ItemStyle-HorizontalAlign="Center" HeaderText="Min"
                                                HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="QtyMax" ItemStyle-HorizontalAlign="Center" HeaderText="Max"
                                                HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="QtyOnHandAll" ItemStyle-HorizontalAlign="Center" HeaderText="Stock RS"
                                                HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="PurchaseRequestQty" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="PR" HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Center" HeaderText="PO"
                                                HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="CustomConversion" ItemStyle-HorizontalAlign="Center" HeaderText="Konversi"
                                                HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="CustomUnitPrice" ItemStyle-HorizontalAlign="Right" HeaderText="Harga"
                                                HeaderStyle-Width="130px" />
                                            <asp:BoundField DataField="DiscountPercentage1" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diskon 1 [%]" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="DiscountPercentage2" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diskon 2 [%]" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="CustomSubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                HeaderText="Sub Total" HeaderStyle-Width="140px" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
