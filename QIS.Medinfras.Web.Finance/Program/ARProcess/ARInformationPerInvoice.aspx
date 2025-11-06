<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ARInformationPerInvoice.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInformationPerInvoice" %>

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

            setDatePicker('<%=txtDocumentDate.ClientID %>');
            setDatePicker('<%=txtARDocumentReceiveDate.ClientID %>');
        });

        $('#<%=rbFilterInvoiceDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == '0') {
                $('#<%=trFilterInvoiceDate.ClientID %>').attr("style", "display:none");
            } else {
                $('#<%=trFilterInvoiceDate.ClientID %>').removeAttr("style");
            }
        });

        //#region AR Invoice
        function onGetARInvoiceFilterExpression() {

            var filterExpression = "GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";

            var filterInvoiceDate = $('#<%=rbFilterInvoiceDate.ClientID %>').find('input:checked').val();
            if (filterInvoiceDate == "1") {
                var periodeFrom = $('#<%=txtPeriodFrom.ClientID %>').val();
                var periodeTo = $('#<%=txtPeriodTo.ClientID %>').val();

                var from = periodeFrom.split("-");
                var fromInDate = from[2] + "" + from[1] + "" + from[0];

                var to = periodeTo.split("-");
                var toInDate = to[2] + "" + to[1] + "" + to[0];

                filterExpression += " AND ARInvoiceDate BETWEEN '" + fromInDate + "' AND '" + toInDate + "'";
            }

            return filterExpression;
        }

        $('#lblARInvoiceNo.lblLink').live('click', function () {
            openSearchDialog('arinvoicehd1', onGetARInvoiceFilterExpression(), function (value) {
                $('#<%=txtARInvoiceNo.ClientID %>').val(value);
                ontxtARInvoiceNoChanged(value);
            });
        });

        $('#<%=txtARInvoiceNo.ClientID %>').live('change', function () {
            ontxtARInvoiceNoChanged($(this).val());
        });

        function ontxtARInvoiceNoChanged(value) {
            var filter = "ARInvoiceNo = '" + value + "' AND GCTransactionStatus != '" + Constant.TransactionStatus.VOID + "'";
            Methods.getObject('GetvARInvoiceHd1List', filter, function (result) {
                if (result != null) {
                    var BusinessPartner = "(" + result.BusinessPartnerCode + ") " + result.BusinessPartnerName;
                    var BillToAddress = result.CustomerBillToStreetName + " " + result.CustomerBillToCounty + " " + result.CustomerBillToDistrict + " " + result.CustomerBillToCity + " " + result.CustomerBillToState + " " + result.CustomerBillToZipCode;

                    $('#<%=hdnARInvoiceID.ClientID %>').val(result.ARInvoiceID);

                    $('#<%=txtBusinessPartner.ClientID %>').val(BusinessPartner);
                    $('#<%=txtBusinessPartnerBillToAddress.ClientID %>').val(BillToAddress);

                    $('#<%=txtInvoiceDate.ClientID %>').val(result.ARInvoiceDateInDatePickerString);
                    $('#<%=txtDueDate.ClientID %>').val(result.DueDateInDatePickerString);
                    $('#<%=txtDocumentDate.ClientID %>').val(result.DocumentDateInDatePickerString);
                    $('#<%=txtARDocumentReceiveDate.ClientID %>').val(result.ARDocumentReceiveDateInDatePickerString);
                    $('#<%=txtARDocumentReceiveByName.ClientID %>').val(result.ARDocumentReceiveByName);

                    $('#<%=txtTotalClaimed.ClientID %>').val(result.TotalClaimedAmountInString);
                    $('#<%=txtTotalPayment.ClientID %>').val(result.TotalPaymentAmountInString);
                    $('#<%=txtSaldo.ClientID %>').val(result.TotalSaldoInString);

                    $('#<%=txtReferenceNo.ClientID %>').val(result.ARReferenceNo);
                    $('#<%=txtRecipientName.ClientID %>').val(result.RecipientName);
                    $('#<%=txtPrintAsName.ClientID %>').val(result.PrintAsName);

                    $('#<%=txtBankName.ClientID %>').val(result.ARReferenceNo);
                    $('#<%=txtAccountNo.ClientID %>').val(result.RecipientName);
                    $('#<%=txtRemarks.ClientID %>').val(result.Remarks);

                    cbpView.PerformCallback('refresh');
                } else {
                    $('#<%=hdnARInvoiceID.ClientID %>').val("0");

                    $('#<%=txtBusinessPartner.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerBillToAddress.ClientID %>').val('');

                    $('#<%=txtInvoiceDate.ClientID %>').val('');
                    $('#<%=txtDueDate.ClientID %>').val('');
                    $('#<%=txtDocumentDate.ClientID %>').val('');
                    $('#<%=txtARDocumentReceiveDate.ClientID %>').val('');
                    $('#<%=txtARDocumentReceiveByName.ClientID %>').val('');

                    $('#<%=txtTotalClaimed.ClientID %>').val('');
                    $('#<%=txtTotalPayment.ClientID %>').val('');
                    $('#<%=txtSaldo.ClientID %>').val('');

                    $('#<%=txtReferenceNo.ClientID %>').val('');
                    $('#<%=txtRecipientName.ClientID %>').val('');
                    $('#<%=txtPrintAsName.ClientID %>').val('');

                    $('#<%=txtBankName.ClientID %>').val('');
                    $('#<%=txtAccountNo.ClientID %>').val('');
                    $('#<%=txtRemarks.ClientID %>').val('');

                    cbpView.PerformCallback('refresh');
                }
            });
        }
        //#endregion

        $('#btnUpdate').live('click', function () {
            cbpProcess.PerformCallback('update');
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'update') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    ontxtARInvoiceNoChanged($('#<%=txtARInvoiceNo.ClientID %>').val());
                }

            }
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnARInvoiceID" runat="server" value="" />
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
                                    <%=GetLabel("Filter Tgl Invoice") %></label>
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
                                    <%=GetLabel("Periode Tgl Invoice") %></label>
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
                                    <%=GetLabel("Nomor Invoice")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtARInvoiceNo" Width="150px" Style="text-align: center" />
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
                                        <%=GetLabel("Informasi Instansi")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trInstansi" runat="server">
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBusinessPartner" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trAlamatTagih" runat="server">
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Alamat Tagih")%></label>
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
                        <tr id="trHeaderInformasiTagihan" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Tagihan")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trDetailInformasiTagihan" runat="server">
                            <td colspan="2">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 5%" />
                                        <col style="width: 40%" />
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
                                                            <%=GetLabel("Tgl Invoice")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInvoiceDate" Width="150px" CssClass="datepicker"
                                                            ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tgl Jatuh Tempo")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDueDate" Width="150px" CssClass="datepicker" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" style="vertical-align: middle">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tgl Kirim Invoice") %></label>
                                                    </td>
                                                    <td style="width: 300px; vertical-align: middle">
                                                        <asp:TextBox runat="server" Width="150px" ID="txtDocumentDate" CssClass="datepicker" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblMandatory">
                                                            <%=GetLabel("Tgl Terima Dokumen") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="120px" ID="txtARDocumentReceiveDate" CssClass="datepicker" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Terima Dokumen Oleh") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="100%" ID="txtARDocumentReceiveByName" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnUpdate" value="UPDATE" class="btnUpdate w3-button w3-blue w3-border w3-border-blue w3-round-medium" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Total Klaim") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTotalClaimed" ReadOnly="true" Width="150px" CssClass="txtCurrency"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Total Penerimaan") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTotalPayment" ReadOnly="true" Width="150px" CssClass="txtCurrency"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Saldo Tagihan") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSaldo" ReadOnly="true" Width="150px" CssClass="txtCurrency" runat="server"
                                                            ForeColor="Blue" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="padding: 5px; vertical-align: top;">
                                            <table width="100%">
                                                <colgroup>
                                                    <col style="width: 150px" />
                                                    <col style="width: 200px" />
                                                    <col style="width: 200px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <%=GetLabel("No. Referensi") %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReferenceNo" runat="server" Width="100%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Nama UP")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRecipientName" runat="server" Width="100%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Cetak Atas Nama")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPrintAsName" runat="server" Width="100%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal" id="lblBankVA">
                                                            <%=GetLabel("Bank & VA") %></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtBankName" Width="100%" ReadOnly="true" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAccountNo" Width="100%" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Keterangan")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trLine3" runat="server">
                            <td colspan="2">
                                <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr id="trHeaderInformasiAlokasiPembayaran" runat="server">
                            <td colspan="2">
                                <h4>
                                    <label class="lblNormal">
                                        <%=GetLabel("Informasi Alokasi Pembayaran")%></label>
                                </h4>
                            </td>
                        </tr>
                        <tr id="trDetailInformasiAlokasiPembayaran" runat="server">
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
                                                            <th align="left" style="width: 130px">
                                                                <%=GetLabel("No Invoice") %>
                                                            </th>
                                                            <th align="center" style="width: 80px">
                                                                <%=GetLabel("Tgl Invoice") %>
                                                            </th>
                                                            <th align="left" style="width: 130px">
                                                                <%=GetLabel("No Pelunasan") %>
                                                            </th>
                                                            <th align="center" style="width: 80px">
                                                                <%=GetLabel("Tgl Pelunasan") %>
                                                            </th>
                                                            <th align="left" style="width: 130px">
                                                                <%=GetLabel("No Voucher") %>
                                                            </th>
                                                            <th align="center" style="width: 80px">
                                                                <%=GetLabel("Tgl Voucher") %>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("Alokasi Oleh") %>
                                                            </th>
                                                            <th align="center" style="width: 150px">
                                                                <%=GetLabel("Tgl/Jam Alokasi") %>
                                                            </th>
                                                            <th align="right" style="width: 150px">
                                                                <%=GetLabel("Nilai Pelunasan") %>
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
                                                                        <input type="hidden" bindingfield="ARInvoiceID" value='<%#: Eval("ARInvoiceID")%>' />
                                                                        <input type="hidden" bindingfield="ARReceivingID" value='<%#: Eval("ARReceivingID")%>' />
                                                                    </td>
                                                                    <td align="left">
                                                                        <div>
                                                                            <%#: Eval("ARInvoiceNo")%></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div>
                                                                            <%#: Eval("cfARInvoiceDateInString")%></div>
                                                                    </td>
                                                                    <td align="left">
                                                                        <div>
                                                                            <%#: Eval("ARReceivingNo")%></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div>
                                                                            <%#: Eval("cfARReceivingDateInString")%></div>
                                                                    </td>
                                                                    <td align="left">
                                                                        <div>
                                                                            <%#: Eval("JournalNo")%></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div>
                                                                            <%#: Eval("cfJournalDateInString")%></div>
                                                                    </td>
                                                                    <td align="left">
                                                                        <div>
                                                                            <%#: Eval("AlocationByName")%></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div>
                                                                            <%#: Eval("cfAlocationDateInDateTimeString")%></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div>
                                                                            <%#: Eval("cfReceivingAmountInString")%></div>
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
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
