<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PhysicalCountUncheckedDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PhysicalCountUncheckedDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseReceiveBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnItemRequestDetailConfirm" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div><%=GetLabel("Void")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">   
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnPurchaseReceiveBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Warehouse/PurchaseReceive/PhysicalCountChecked/PhysicalCountUncheckedList.aspx?id=to');
            });

            $('#<%=btnItemRequestDetailConfirm.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Item First');
                else
                    onCustomButtonClick('approve');
            });
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var result = '';
            $('.grdItem .chkIsSelected input').each(function () {
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
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/Warehouse/PurchaseReceive/PhysicalCountChecked/PhysicalCountUncheckedList.aspx?id=to');
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
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnReceiveID" runat="server" />
    <input type="hidden" value="0" id="hdnIsAutoUpdatePO" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <div style="height: 435px; overflow-y: auto; overflow-x: hidden;">
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
                                <label class="lblNormal" id="lblPurchaseReceiveNo">
                                    <%=GetLabel("No. BPB")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseReceiveNo" Width="156px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu Penerimaan") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 100px">
                                            <asp:TextBox ID="txtPurchaseReceiveDate" Width="100px" CssClass="datepicker" runat="server"
                                                ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseReceiveTime" Width="50px" CssClass="time" runat="server"
                                                Style="text-align: center" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
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
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                                runat="server" ReadOnly="true" />
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
                                <label class="lblNormal">
                                    <%=GetLabel("No.Faktur/Kirim")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col style="width: 125px" />
                                        <col  />
                                        <col style="width: 125px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFacturNo" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                                runat="server" ReadOnly="true" />
                                        </td>
                                        <td style="padding-left:50px">
                                            <%=GetLabel("Tanggal Faktur") %>
                                        </td>
                                        <td style="padding-right: 1px;">
                                            <asp:TextBox ID="txtDateReferrence" Width="100%" CssClass="datepicker" runat="server"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Waktu Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTerm" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocation">
                                    <%=GetLabel("Ke Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" ReadOnly="true" />
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
                        <tr style="display:none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Mata Uang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrency" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel">
                                <%=GetLabel("Nilai Kurs (Rp)") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKurs" Width="120px" runat="server" ReadOnly="true" />
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
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">                                                       
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; text-align: center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th rowspan="2" align="left"><%=GetLabel("NAMA BARANG")%></th>
                                                    <th colspan="6"><%=GetLabel("DATA PEMESANAN")%></th>
                                                    <th colspan="5"><%=GetLabel("DATA PENERIMAAN")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 150px" align="left"><%=GetLabel("No. Pemesanan")%></th>
                                                    <th style="width: 80px" align="right"><%=GetLabel("Qty Pesan")%></th>
                                                    <th style="width: 70px" align="left"><%=GetLabel("Satuan")%></th>
                                                    <th style="width: 100px" align="right"><%=GetLabel("Harga")%></th>
                                                    <th style="width: 70px" align="right"><%=GetLabel("Disc1 (%)")%></th>
                                                    <th style="width: 70px" align="right"><%=GetLabel("Disc2 (%)")%></th>

                                                    <th style="width: 80px" align="right"><%=GetLabel("Qty Terima")%></th>
                                                    <th style="width: 70px" align="left"><%=GetLabel("Satuan")%></th>
                                                    <th style="width: 100px" align="right"><%=GetLabel("Harga")%></th>
                                                    <th style="width: 70px" align="right"><%=GetLabel("Disc1 (%)")%></th>
                                                    <th style="width: 70px" align="right"><%=GetLabel("Disc2 (%)")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("Tidak ada data penerimaan barang yang perlu dikonfirmasi")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdItem grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">                                                       
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; text-align: center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th rowspan="2" align="left"><%=GetLabel("NAMA BARANG")%></th>
                                                    <th colspan="8"><%=GetLabel("DATA PEMESANAN")%></th>
                                                    <th colspan="5"><%=GetLabel("DATA PENERIMAAN")%></th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 140px" align="left"><%=GetLabel("No. Pemesanan")%></th>
                                                    <th style="width: 45px" align="right"><%=GetLabel("Qty Pesan")%></th>
                                                    <th style="width: 45px" align="right"><%=GetLabel("Qty Terima")%></th>
                                                    <th style="width: 45px" align="right"><%=GetLabel("Qty Sisa")%></th>
                                                    <th style="width: 65px" align="left"><%=GetLabel("Satuan")%></th>
                                                    <th style="width: 80px" align="right"><%=GetLabel("Harga")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("Disc1 (%)")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("Disc2 (%)")%></th>

                                                    <th style="width: 50px" align="right"><%=GetLabel("Qty Terima")%></th>
                                                    <th style="width: 65px" align="left"><%=GetLabel("Satuan")%></th>
                                                    <th style="width: 80px" align="right"><%=GetLabel("Harga")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("Disc1 (%)")%></th>
                                                    <th style="width: 60px" align="right"><%=GetLabel("Disc2 (%)")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("ID")%>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemName1")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("PurchaseOrderNo")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderQuantity")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderQuantity")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderQuantity")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("OrderPurchaseUnit")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderUnitPrice")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderDisc1")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CustomOrderDisc2")%>
                                                </td>
                                               <td align="right">
                                                    <label id="lblPurchaseReceiveQty" runat="server" class="lblPurchaseReceiveQty"><%#: Eval("CustomReceiveQuantity")%></label>
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemUnit")%>
                                                </td>
                                                <td align="right">
                                                    <label id="lblPurchaseReceivePrice" runat="server" class="lblPurchaseReceivePrice"><%#: Eval("UnitPriceText")%></label>
                                                </td>
                                                <td align="right">
                                                    <label id="lblDiscountAmount1Pct" runat="server" class="lblDiscountAmount1Pct"><%#: Eval("DiscountAmount1Pct")%></label>
                                                </td>
                                                <td align="right">
                                                    <label id="lblDiscountAmount2Pct" runat="server" class="lblDiscountAmount2Pct"><%#: Eval("DiscountAmount2Pct")%></label>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
