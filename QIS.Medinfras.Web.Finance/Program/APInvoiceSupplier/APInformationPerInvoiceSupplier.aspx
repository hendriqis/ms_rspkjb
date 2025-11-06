<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="APInformationPerInvoiceSupplier.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.APInformationPerInvoiceSupplier" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        });

        $('#<%=rbFilterInvoiceDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == '0') {
                $('#<%=trFilterInvoiceDate.ClientID %>').attr("style", "display:none");
            } else {
                $('#<%=trFilterInvoiceDate.ClientID %>').removeAttr("style");
            }
        });

        //#region Purchase Invoice
        function onGetPurchaseInvoiceHdFilterExpression() {

            var filterExpression = "GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";

            var filterInvoiceDate = $('#<%=rbFilterInvoiceDate.ClientID %>').find('input:checked').val();
            if (filterInvoiceDate == "1") {
                var periodeFrom = $('#<%=txtPeriodFrom.ClientID %>').val();
                var periodeTo = $('#<%=txtPeriodTo.ClientID %>').val();

                var from = periodeFrom.split("-");
                var fromInDate = from[2] + "" + from[1] + "" + from[0];

                var to = periodeTo.split("-");
                var toInDate = to[2] + "" + to[1] + "" + to[0];

                filterExpression += " AND PurchaseInvoiceDate BETWEEN '" + fromInDate + "' AND '" + toInDate + "'";
            }

            return filterExpression;
        }

        $('#lblARInvoiceNo.lblLink').live('click', function () {
            openSearchDialog('purchaseinvoicehd', onGetPurchaseInvoiceHdFilterExpression(), function (value) {
                $('#<%=txtPurchaseInvoiceNo.ClientID %>').val(value);
                ontxtPurchaseInvoiceNoChanged(value);
            });
        });

        $('#<%=txtPurchaseInvoiceNo.ClientID %>').live('change', function () {
            ontxtPurchaseInvoiceNoChanged($(this).val());
        });

        function ontxtPurchaseInvoiceNoChanged(value) {
            var filter = "PurchaseInvoiceNo = '" + value + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";
            Methods.getObject('GetvPurchaseInvoiceHdCustomList', filter, function (result) {
                if (result != null) {
                    var BusinessPartner = "(" + result.BusinessPartnerCode + ") " + result.BusinessPartnerName;
                    var BusinessPartnerAddress = result.SupplierStreetName + " " + result.SupplierCounty + " " + result.SupplierDistrict + " " + result.SupplierCity + " " + result.SupplierState + " " + result.SupplierZipCode;

                    $('#<%=hdnPurchaseInvoiceID.ClientID %>').val(result.PurchaseInvoiceID);

                    $('#<%=txtBusinessPartner.ClientID %>').val(BusinessPartner);
                    $('#<%=txtBusinessPartnerBillToAddress.ClientID %>').val(BusinessPartnerAddress);

                    $('#<%=txtPurchaseInvoiceDate.ClientID %>').val(result.PInvoiceDateInString);
                    $('#<%=txtDueDate.ClientID %>').val(result.DueDateInString);
                    $('#<%=txtProductLineCode.ClientID %>').val(result.ProductLineCode);
                    $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                    $('#<%=txtRemarks.ClientID %>').val(result.Remarks);
                    $('#<%=txtReferenceNo.ClientID %>').val(result.ReferenceNo);

                    cbpView.PerformCallback('refresh');
                    cbpView1.PerformCallback('refresh');
                } else {
                    $('#<%=hdnPurchaseInvoiceID.ClientID %>').val();

                    $('#<%=txtBusinessPartner.ClientID %>').val("");
                    $('#<%=txtBusinessPartnerBillToAddress.ClientID %>').val("");

                    $('#<%=txtPurchaseInvoiceDate.ClientID %>').val("");
                    $('#<%=txtDueDate.ClientID %>').val("");
                    $('#<%=txtProductLineCode.ClientID %>').val("");
                    $('#<%=txtProductLineName.ClientID %>').val("");
                    $('#<%=txtRemarks.ClientID %>').val("");
                    $('#<%=txtReferenceNo.ClientID %>').val("");

                    cbpView.PerformCallback('refresh');
                    cbpView1.PerformCallback('refresh');
                }
            });
        }
        //#endregion

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
        function onCbpViewEndCallback1(s) {
            hideLoadingPanel();
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnSetvarLeadTime" runat="server" value="0" />
    <input type="hidden" id="hdnSetvarHitungJatuhTempoDari" runat="server" value="0" />
    <div style="overflow-x: hidden;">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top; width: 100%">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 190px" />
                            <col />
                        </colgroup>
                        <tr id="trFilterInvoiceDatePeriode" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblFilterInvoiceDate" runat="server">
                                    <%=GetLabel("Filter Tgl. Tanda Terima Faktur") %></label>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rbFilterInvoiceDate" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ya" Value="1" />
                                    <asp:ListItem Text="Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trFilterInvoiceDate" runat="server" style="display: none">
                            <td>
                                <label class="lblMandatory">
                                    <%=GetLabel("Periode Tgl. Tanda Terima Faktur") %></label>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 50px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                        </td>
                                        <td style="text-align: center;">
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInvoiceNo" runat="server">
                            <td>
                                <label class="lblMandatory lblLink" id="lblARInvoiceNo">
                                    <%=GetLabel("Tanda Terima Faktur")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPurchaseInvoiceNo" Width="150px" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr id="trLine1" runat="server">
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr id="trHeaderInformasiInstansi" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Supplier")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trInstansi" runat="server">
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Supplier")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBusinessPartner" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trAlamatTagih" runat="server">
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Alamat")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBusinessPartnerBillToAddress" ReadOnly="true"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr id="trLine2" runat="server">
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr id="trLine3" runat="server">
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr id="trHeaderInformasiTagihan" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Hutang Supplier")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trDetailInformasiTagihan" runat="server">
                            <td colspan="2">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 10%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding: 5px; vertical-align: top;">
                                            <table width="100%">
                                                <colgroup>
                                                    <col style="width: 150px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tgl Tukar Faktur")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPurchaseInvoiceDate" Width="150px" CssClass="datepicker"
                                                            ReadOnly="true" />
                                                    </td>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Jatuh Tempo Faktur s/d")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDueDate" Width="150px" CssClass="datepicker" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Product Line")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtProductLineCode" ReadOnly="true" Width="100%" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtProductLineName" ReadOnly="true" Width="250%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                        <%=GetLabel("Catatan") %>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemarks" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                        <%=GetLabel("No. Referensi") %>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trHeaderInformasiDetailInvoice" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Detail Invoice")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trInformasiDetailInvoice" runat="server">
                            <td colspan="3">
                                <div style="position: relative;" id="divView">
                                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                                    margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                                    <table class="grdView grdSelected" cellspacing="0" width="100%" rules="all">
                                                        <tr>
                                                            <th class="keyField">
                                                            </th>
                                                            <th style="width: 120px" align="left">
                                                                <%=GetLabel("No Penerimaan") %>
                                                            </th>
                                                            <th style="width: 80px" align="left">
                                                                <%=GetLabel("No Faktur") %>
                                                            </th>
                                                            <th style="width: 80px" align="left">
                                                                <%=GetLabel("No Faktur Pajak") %>
                                                            </th>
                                                            <th style="width: 100px" align="right">
                                                                <%=GetLabel("Jumlah") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Diskon Transaksi") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Diskon Final") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("PPN") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Materai") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Ongkos Kirim") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Uang Muka") %>
                                                            </th>
                                                            <th style="width: 120px" align="right">
                                                                <%=GetLabel("Nota Kredit") %>
                                                            </th>
                                                            <th style="width: 120px" align="right">
                                                                <%=GetLabel("SubTotal") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Pembulatan") %>
                                                            </th>
                                                            <th align="right" style="width: 120px">
                                                                <%=GetLabel("Total") %>
                                                            </th>
                                                        </tr>
                                                        <asp:ListView runat="server" ID="lvwView">
                                                            <EmptyDataTemplate>
                                                                <tr class="trEmpty">
                                                                    <td colspan="15">
                                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="keyField">
                                                                        <input type="hidden" bindingfield="PurchaseInvoiceID" value='<%#: Eval("PurchaseInvoiceID")%>' />
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("cfReceiveOrderNo") %></label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("ReferenceNo")%>
                                                                        <br />
                                                                        <%#:Eval("ReferenceDateInString")%>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("TaxInvoiceNo")%>
                                                                        <br />
                                                                        <%#:Eval("TaxInvoiceDateInString")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("TransactionAmount","{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("DiscountAmount","{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("FinalDiscountAmount","{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("VATAmount","{0:N}") %>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("StampAmount", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("ChargesAmount", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("DownPaymentAmount", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("CreditNoteAmount", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("cfNetBeforeRounding", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("RoundingAmount", "{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("LineAmount", "{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </table>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr id="trHeaderDetailInformasiPayment" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Detail Pembayaran")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trInformasiPaymentDetail" runat="server">
                            <td colspan="3">
                                <div style="position: relative;" id="div1">
                                    <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                                        ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback1(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="width: 100%;
                                                    margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                                    <table class="grdView grdSelected" cellspacing="0" width="100%" rules="all">
                                                        <tr>
                                                            <th class="keyField">
                                                            </th>
                                                            <th style="width: 120px" align="left">
                                                                <%=GetLabel("Group Jurnal") %>
                                                            </th>
                                                            <th style="width: 80px" align="left">
                                                                <%=GetLabel("No Jurnal") %>
                                                            </th>
                                                            <th style="width: 80px" align="left">
                                                                <%=GetLabel("Tgl Jurnal") %>
                                                            </th>
                                                            <th style="width: 100px" align="right">
                                                                <%=GetLabel("Tipe Treasury") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Group Treasury") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Debit") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("Credit") %>
                                                            </th>
                                                            <th style="width: 80px" align="right">
                                                                <%=GetLabel("SupplierPaymentNo") %>
                                                            </th>
                                                        </tr>
                                                        <asp:ListView runat="server" ID="lvwView1">
                                                            <EmptyDataTemplate>
                                                                <tr class="trEmpty">
                                                                    <td colspan="15">
                                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="keyField">
                                                                        <input type="hidden" bindingfield="PurchaseInvoiceID" value='<%#: Eval("PurchaseInvoiceID")%>' />
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("JournalGroup") %></label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("JournalNo")%>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("JournalDateInString")%>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("TreasuryType")%>
                                                                    </td>
                                                                    <td align="left">
                                                                        <%#:Eval("TreasuryGroup")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("DebitAmount","{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("CreditAmount","{0:N}")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#:Eval("SupplierPaymentNo","{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </table>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="Div2">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
