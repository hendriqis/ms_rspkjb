<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx3.master"
    CodeBehind="ARPayerReceiveAlocationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARPayerReceiveAlocationEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePayer/ARInvoicePayerToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            var paymentNo = $('#<%=txtARReceivingNo.ClientID %>').val();
            var isFinish = $('#<%=hdnIsFinish.ClientID %>').val();
            if (paymentNo != "" && paymentNo != null) {
                if (isFinish == "0") {
                    $('#lblAddData').removeAttr('style');
                } else {
                    $('#lblAddData').attr('style', 'display:none');
                }
            } else {
                $('#lblAddData').attr('style', 'display:none');
            }
        }

        function onAfterPopupControlClosing() {
            var val = $('#<%=txtARReceivingNo.ClientID %>').val();
            onLoadObject(val);
        }

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        //#region ARReceivingNo
        function onGetFilterExpressionARReceiving() {
            var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
            var customerGroupID = $('#<%=hdnCustomerGroupID.ClientID %>').val();
            var filterExpression = "(VoucherNo IS NULL AND GCTransactionStatus IN ('" + Constant.TransactionStatus.OPEN + "') AND ((BusinessPartnerID = " + businessPartnerID + ") OR (CustomerGroupID = " + customerGroupID + " AND BusinessPartnerID IS NULL) OR (CustomerGroupID = " + customerGroupID + " AND BusinessPartnerID = " + businessPartnerID + "))) OR (VoucherNo IS NOT NULL AND GCTransactionStatus IN ('" + Constant.TransactionStatus.APPROVED + "','" + Constant.TransactionStatus.PROCESSED + "','" + Constant.TransactionStatus.CLOSED + "') AND ((BusinessPartnerID = " + businessPartnerID + ") OR (CustomerGroupID = " + customerGroupID + " AND BusinessPartnerID IS NULL) OR (CustomerGroupID = " + customerGroupID + " AND BusinessPartnerID = " + businessPartnerID + ")))";

            return filterExpression;
        }

        $('#lblARReceivingNo.lblLink').live('click', function () {
            openSearchDialog('arreceivinghdallocation', onGetFilterExpressionARReceiving(), function (value) {
                onTxtPaymentNoChanged(value);
            });
        });

        $('#<%=txtARReceivingNo.ClientID %>').live('change', function () {
            var val = $(this).val();
            var filterExpression = onGetFilterExpressionARReceiving() + " AND ARReceivingNo = '" + val + "'";
            Methods.getObject('GetvARReceivingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnARReceivingID.ClientID %>').val(result.ARReceivingID);
                    onTxtPaymentNoChanged(val);
                }
                else {
                    $('#<%=txtARReceivingNo.ClientID %>').val('');
                    $('#<%=hdnARReceivingID.ClientID %>').val('');
                }
            });
        });

        function onTxtPaymentNoChanged(value) {
            onLoadObject(value);
        }

        //#endregion

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var arReceivingID = $('#<%=hdnARReceivingID.ClientID %>').val();
                var url = ResolveUrl('~/Program/ARInvoicePayer/ARInvoicePayerReceive/ARPayerReceiveAlocationEntryDetailCtl.ascx');
                openUserControlPopup(url, arReceivingID, 'Pilih Tagihan', 1200, 600);
            }
        });

        $('.tdARInvoiceNo a').die('click');
        $('.tdARInvoiceNo a').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var param = entity.ARInvoiceID + "|alocation";
            var url = ResolveUrl("~/Program/ARInvoicePayer/ARInvoicePayerEdit/ARInvoicePayerAlocationCtl.ascx");
            var headerText = "Alokasi Invoice Detail : " + $(this).html();
            openUserControlPopup(url, param, headerText, 1100, 500);
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    $('#<%=hdnDtID.ClientID %>').val(entity.ID);
                    $('#<%=hdnARReceivingID.ClientID %>').val(entity.ARReceivingID);
                    $('#<%=hdnARInvoiceID.ClientID %>').val(entity.ARInvoiceID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpProcess.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpProcess.PerformCallback('refresh');
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    var value = $('#<%=txtARReceivingNo.ClientID %>').val();
                    onLoadObject(value);
                }
            }
            else {
                var value = $('#<%=txtARReceivingNo.ClientID %>').val();
                onLoadObject(value);
            }
        }
        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARReceivingID.ClientID %>').val();
            if (param != "" && param != "0") {
                return param;
            } else {
                showToast('Failed', 'Maaf, pilih No. Penerimaan terlebih dahulu.');
                return false;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var ReceivingID = $('#<%=hdnARReceivingID.ClientID %>').val();
            var GCtransaction = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (ReceivingID == '' || ReceivingID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "ARReceivingID = " + ReceivingID;
                return true;
            }
        }
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
        <input type="hidden" value="" id="hdnCustomerGroupID" runat="server" />
        <input type="hidden" value="" id="hdnDtID" runat="server" />
        <input type="hidden" value="" id="hdnARReceivingID" runat="server" />
        <input type="hidden" value="" id="hdnARInvoiceID" runat="server" />
        <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
        <input type="hidden" value="" id="hdnARreceivingAmount" runat="server" />
        <input type="hidden" value="" id="hdnIsFinish" runat="server" />
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td valign="top" align="left">
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 350px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <div style="position: relative;">
                                    <label class="lblLink lblKey" id="lblARReceivingNo">
                                        <%=GetLabel("No. Pembayaran")%></label></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtARReceivingNo" Width="150px" Style="text-align: center" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tgl. Bayar")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceivingDate" Width="150px" Style="text-align: center" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="350px" runat="server" ReadOnly="true" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Voucher")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherNo" Width="150px" runat="server" ReadOnly="true" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tgl. Voucher")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherDate" Width="150px" runat="server" ReadOnly="true" Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="right" style="width: 100%">
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 350px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nilai Penerimaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentAmount" Width="250px" runat="server" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nilai Alokasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlocationAmount" Width="250px" runat="server" CssClass="txtCurrency"
                                    ReadOnly="true" ForeColor="Blue" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nilai Sisa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemainingAmount" Width="250px" runat="server" CssClass="txtCurrency"
                                    ReadOnly="true" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="height: 300px; overflow-y: auto; overflow-x: hidden" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                        margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                        <table class="grdARReceivingInvoice grdSelected" cellspacing="0" width="100%" rules="all">
                                            <tr>
                                                <th align="center">
                                                </th>
                                                <th align="center" style="width: 150px">
                                                    <%=GetLabel("Alocation Info") %>
                                                </th>
                                                <th align="left" style="width: 140px">
                                                    <%=GetLabel("Invoice No") %>
                                                </th>
                                                <th align="center" style="width: 100px">
                                                    <%=GetLabel("Invoice Date") %>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Customer") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Claimed Amount") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Receiving Amount") %>
                                                </th>
                                                <th align="right" style="width: 150px">
                                                    <%=GetLabel("Outstanding Amount") %>
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
                                                        <input type="hidden" bindingfield="ID" value='<%#: Eval("ID")%>' />
                                                        <input type="hidden" bindingfield="ARReceivingID" value='<%#: Eval("ARReceivingID")%>' />
                                                        <input type="hidden" bindingfield="ARInvoiceID" value='<%#: Eval("ARInvoiceID")%>' />
                                                        <td align="center">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                            alt="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="center">
                                                            <div style="font-size: smaller">
                                                                <%#:Eval("cfCreatedDateInStringFullFormat")%>
                                                            </div>
                                                            <div>
                                                                <%#:Eval("CreatedByName")%>
                                                            </div>
                                                        </td>
                                                        <td align="left" class="tdARInvoiceNo">
                                                            <a type="10">
                                                                <%#:Eval("ARInvoiceNo")%></a>
                                                        </td>
                                                        <td align="center">
                                                            <%#:Eval("cfARInvoiceDateInString")%>
                                                        </td>
                                                        <td align="left">
                                                            <%#:Eval("BusinessPartnerName")%>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("cfTotalClaimedAmountInString")%>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("cfReceivingAmountInString")%>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("cfTotalRemainingAmountInString")%>
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
                    <div style="width: 100%; text-align: center">
                        <div style="width: 100%; text-align: center">
                            <span class="lblLink" id="lblAddData" style="margin-right: 300px; margin-left: 300px">
                                <%= GetLabel("Add Data")%></span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <div style="width: 600px;">
                            <div class="pageTitle" style="text-align: center">
                                <%=GetLabel("Informasi")%></div>
                            <div style="background-color: #EAEAEA;">
                                <table width="600px" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="150px" />
                                        <col width="30px" />
                                    </colgroup>
                                    <tr>
                                        <td align="left">
                                            <%=GetLabel("Dibuat Oleh") %>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divCreatedBy">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%=GetLabel("Dibuat Pada") %>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divCreatedDate">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trApprovedBy" runat="server">
                                        <td align="left">
                                            <%=GetLabel("Approved Oleh") %>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divApprovedBy">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trApprovedDate" runat="server">
                                        <td align="left">
                                            <%=GetLabel("Approved Pada")%>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divApprovedDate">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trVoidBy" runat="server">
                                        <td align="left">
                                            <%=GetLabel("Void Oleh") %>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divVoidBy">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trVoidDate" runat="server">
                                        <td align="left">
                                            <%=GetLabel("Void Pada")%>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divVoidDate">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%=GetLabel("Terakhir Diubah Oleh") %>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divLastUpdatedBy">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%=GetLabel("Terakhir Diubah Pada")%>
                                        </td>
                                        <td align="center">
                                            :
                                        </td>
                                        <td>
                                            <div runat="server" id="divLastUpdatedDate">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
