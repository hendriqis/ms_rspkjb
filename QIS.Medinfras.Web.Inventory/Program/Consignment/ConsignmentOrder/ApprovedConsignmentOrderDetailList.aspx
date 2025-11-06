<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ApprovedConsignmentOrderDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ApprovedConsignmentOrderDetailList" %>

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
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnPurchaseRequestBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Consignment/ConsignmentOrder/ApprovedConsignmentOrderList.aspx?id=to');
            });
        }

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
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
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
    <div style="overflow-x: hidden;">
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
                                <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal Pengiriman") %></td>
                            <td>
                                <asp:TextBox ID="txtDeliveryDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal Expired") %></td>
                            <td>
                                <asp:TextBox ID="txtExpiredDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Nama Supplier") %></td>
                            <td>
                                <asp:TextBox ID="txtSupplierName" Width="100%" runat="server" ReadOnly="true"/>
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
                            <td class="tdLabel"><%=GetLabel("Jenis Persediaan") %></td>
                            <td>
                                <asp:TextBox ID="txtPurchaseOrderType" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Waktu Pembayaran") %></td>
                            <td>
                                <asp:TextBox ID="txtTermCondition" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tipe Franco") %></td>
                            <td>
                                <asp:TextBox ID="txtFrancoRegion" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel"><%=GetLabel("Mata Uang") %></td>
                            <td>
                                <asp:TextBox ID="txtCurrencyCode" Width="100%" runat="server" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel"><%=GetLabel("Nilai Kurs") %></td>
                            <td>
                                <asp:TextBox ID="txtCurrencyRate" CssClass="number" Width="80px" runat="server" ReadOnly="true"/>
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
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Button/verify.png") %>' 
                                                    <%#:Eval("IsReceived").ToString() == "True" ? "" : "Style ='display:none'" %> 
                                                    title='<%=GetLabel("Diterima") %>' alt="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Item Name" />
                                            <asp:BoundField DataField="CustomPurchaseUnit" ItemStyle-HorizontalAlign="Right" HeaderText="Di Pesan" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="CustomUnitPrice" ItemStyle-HorizontalAlign="Right" HeaderText="Harga" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="CustomConversion" ItemStyle-HorizontalAlign="Center" HeaderText="Konversi" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="DiscountPercentage1" ItemStyle-HorizontalAlign="Right" HeaderText="Diskon 1 (%)" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="DiscountPercentage2" ItemStyle-HorizontalAlign="Right" HeaderText="Diskon 2 (%)" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="CustomSubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Sub Total" HeaderStyle-Width="150px"/>
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
            <tr>
                <td colspan="2">
                    <div id="containerTotalOrder" style="margin-top: 20px;">
                        <fieldset id="fsTotalOrder" style="margin: 0">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 40px" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal" id="lblPaymentRemarks"><%=GetLabel("Syarat Pembayaran")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="width: 120px; vertical-align: top; padding-top: 5px;">
                                                    <label class="lblNormal"><%=GetLabel("Keterangan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        <table style="width: 100%;">
                                            <colgroup>
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("Jumlah Nilai Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrder" CssClass="number" ReadOnly="true" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("Diskon Final %")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscountInPercentage" CssClass="number" ReadOnly="true" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("Diskon Final")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFinalDiscount" CssClass="number" ReadOnly="true" Width="180px" runat="server" hiddenVal="0"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("PPN (10%)")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPPN" CssClass="number" ReadOnly="true" Width="180px" runat="server" hiddenVal="0"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("Uang Muka")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDP" ReadOnly="true" CssClass="number" Width="180px" runat="server" hiddenVal="0"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal"><%=GetLabel("Saldo Nilai Pemesanan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalOrderSaldo" CssClass="number" ReadOnly="true" Width="180px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
