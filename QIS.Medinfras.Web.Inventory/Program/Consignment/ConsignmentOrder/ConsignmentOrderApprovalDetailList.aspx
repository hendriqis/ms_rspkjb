<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ConsignmentOrderApprovalDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentOrderApprovalDetailList" %>

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
    <li id="btnPurchaseRequestDetailApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
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
                document.location = ResolveUrl('~/Program/Consignment/ConsignmentOrder/ConsignmentOrderApprovalList.aspx?id=to');
            });

            $('#<%=btnPurchaseRequestDetailApprove.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    onCustomButtonClick('approve');
                }
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
        <%--<div class="pageTitle">
            <div style="font-size: 1.1em">
                <%=GetLabel("Pemesanan Barang Konsinyasi")%></div>
        </div>--%>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Pemesanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server"
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
            </tr>
            <tr>
                <td colspan="2">
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
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomPurchaseUnit" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Di Pesan" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomUnitPrice" ItemStyle-HorizontalAlign="Right" HeaderText="Harga"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomConversion" ItemStyle-HorizontalAlign="Left" HeaderText="Konversi"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DiscountPercentage1" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diskon 1" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DiscountPercentage2" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Diskon 2" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomSubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                HeaderText="Sub Total" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Right" />
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
