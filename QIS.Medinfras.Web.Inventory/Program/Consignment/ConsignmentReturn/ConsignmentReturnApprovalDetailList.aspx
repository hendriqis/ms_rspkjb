<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ConsignmentReturnApprovalDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ConsignmentReturnApprovalDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseReturnDetailApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnPurchaseReturnListBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">   
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnPurchaseReturnListBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Warehouse/PurchaseReturn/PurchaseReturnApprovalList.aspx?id=to');
            });

            $('#<%=btnPurchaseReturnDetailApprove.ClientID %>').click(function () {
                getCheckedMember();
                onCustomButtonClick('approve');
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

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
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
    <input type="hidden" value="" id="hdnPurchaseReturnID" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <div style="height: 435px; overflow-y: auto; overflow-x: hidden;">
        <%--<div class="pageTitle">
            <div style="font-size: 1.1em">
                <%=GetLabel("Retur Barang Konsinyasi")%></div>
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
                                <label class="lblNormal" id="lblReturnNo">
                                    <%=GetLabel("No. Retur")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReturnNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReturnDate" Width="120px" CssClass="datepicker" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblSupplier" runat="server">
                                    <%=GetLabel("Supplier/Penyedia")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" ReadOnly="true" Width="100%" runat="server" />
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
                                <label class="lblNormal" id="lblPurchaseReceiveNo" runat="server">
                                    <%=GetLabel("No. BPB")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="150px" ReadOnly="true" runat="server" />
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
                                <label class="lblNormal" runat="server" id="lblLocation">
                                    <%=GetLabel("Ke Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Penggantian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReturnType" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No Referensi") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="120px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal di Faktur") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkPPN" Width="100%" runat="server" />&nbsp;<%=GetLabel("PPN")%>
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
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="280px" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="CustomUnitPrice" HeaderText="Harga / Satuan" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="CustomTotalDiscount" HeaderText="Total Discount" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="CustomSubTotal" HeaderText="SubTotal" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" DataFormatString="{0:N}" />
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
