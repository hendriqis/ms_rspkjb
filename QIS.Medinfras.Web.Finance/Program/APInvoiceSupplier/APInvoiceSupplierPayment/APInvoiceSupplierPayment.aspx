<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/SupplierPage/MPBaseSupplierPageTrx.master"
    AutoEventWireup="true" CodeBehind="APInvoiceSupplierPayment.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.APInvoiceSupplierPayment" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/APInvoiceSupplier/APInvoiceSupplierToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Propose")%></div>
    </li>
    <li id="btnDecline" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Decline")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var total = 0;
        function onLoad() {
            setDatePicker('<%=txtPaymentDate.ClientID %>');
            setDatePicker('<%=txtReferenceDate.ClientID %>');

            $('.chkIsSelected input').die('change');
            $('.chkIsSelected input').live('change', function () {
                var isChecked = $(this).is(":checked");
                $txt = $(this).closest('tr').find('.txtPembayaran');
                if (isChecked) {
                    $txt.removeAttr('readonly');
                }
                else {
                    $txt.attr('readonly', 'readonly');
                }
                calculateTotalVerification();
            });

            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotalVerification();
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotalVerification();
            });

            if ($('#<%=hdnIsAdd.ClientID %>').val() == "1") {
                $('#<%=panel1.ClientID %>').show();
                $('#<%=panel2.ClientID %>').hide();
            }
            else {
                $('#<%=panel1.ClientID %>').hide();
                $('#<%=panel2.ClientID %>').show();
            }

            $('.txtPembayaran').each(function () {
                $(this).trigger('changeValue');
            });

            $('.txtVerificationAmount').change(function () {
                $(this).trigger('changeValue');
            });

            setCustomToolbarVisibility();

            //#region Supplier Payment No
            $('#lblSupplierPaymentNo.lblLink').click(function () {
                openSearchDialog('supplierpaymenthd', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtPaymentNo.ClientID %>').val(value);
                    onTxtSupplierPaymentNoChanged(value);
                });
            });

            $('#<%=txtPaymentNo.ClientID %>').change(function () {
                onTxtSupplierPaymentNoChanged($(this).val());
            });

            function onTxtSupplierPaymentNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion
        }

        function calculateTotalVerification() {
            var lstSelectedPayment = 0;
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var payment = parseFloat($tr.find('.txtPembayaran').attr('hiddenVal'));
                    lstSelectedPayment += payment;
                }
            });
            $('#<%=txtVerificationAmount.ClientID %>').val(lstSelectedPayment).trigger('changeValue');
        }

        function onCbpProcessDetailEndCallback(s) {
            hideLoadingPanel();
            $('.txtVerificationAmount').val(0).trigger('changeValue');
            var param = s.cpResult.split('|');
        }

        function onCboPaymentMethodValueChanged(evt) {
            var value = cboPaymentMethod.GetValue();
            if (value == '<%=GetSupplierPaymentMethodTransfer() %>' || value == '<%=GetSupplierPaymentMethodGiro() %>' || value == '<%=GetSupplierPaymentMethodCheque() %>') {
                $('#<%=trBank.ClientID %>').removeAttr('style');
                $('#<%=trBankRef.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankRef.ClientID %>').attr('style', 'display:none');
            }
        }

        $('#<%=btnApprove.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                onCustomButtonClick('proposed');
            }
        });

        $('#<%=btnDecline.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                getCheckedPurchaseInvoice();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    onCustomButtonClick('decline');
                }
                else {
                    showToast('Process Failed', 'Please Select Purchase Invoice First');
                }
            }
        });

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if (transactionStatus != 'X121^001') {
                $('#<%=btnApprove.ClientID %>').hide();
            }
            else if (transactionStatus == 'X121^001') {
                $('#<%=btnApprove.ClientID %>').show();
            }
            var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
            if (paymentNo != '') {
                $('#<%=btnDecline.ClientID %>').hide();
            } else {
                $('#<%=btnDecline.ClientID %>').show();
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type != 'decline') {
                onLoadObject(retval);
            } else {
                cbpProcessDetail.PerformCallback('refresh');
            }
        }

        $('#chkSelectAllInvoice').die('change');
        $('#chkSelectAllInvoice').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedPurchaseInvoice() {
            var lstSelectedPurchaseInvoice = '';
            var lstSelectedPayment = '';
            var result = '';
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var payment = parseFloat($tr.find('.txtPembayaran').attr('hiddenVal'));
                    if (lstSelectedPurchaseInvoice != '') {
                        lstSelectedPurchaseInvoice += ',';
                        lstSelectedPayment += ',';
                    }
                    lstSelectedPurchaseInvoice += key;
                    lstSelectedPayment += payment;
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedPurchaseInvoice);
            $('#<%=hdnSelectedPayment.ClientID %>').val(lstSelectedPayment);
        }

        function onBeforeSaveRecord(errMessage) {
            if ($('#<%=hdnSupplierPaymentID.ClientID %>').val() == '0') {
                getCheckedPurchaseInvoice();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    return true;
                }
                else {
                    showToast('Process Failed', 'Please Select Purchase Invoice First');
                    return false;
                }
            } else {
                return true;
            }
        }

        $('.lblPurchaseInvoiceNo').die('click');
        $('.lblPurchaseInvoiceNo').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').val();
            var url = ResolveUrl("~/Program/APInvoiceSupplier/APInvoiceSupplierVerification/APInvoiceSupplierVerificationDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Information', 1100, 500);
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var supplierPaymentID = $('#<%=hdnSupplierPaymentID.ClientID %>').val();
            if (supplierPaymentID == '' || supplierPaymentID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "SupplierPaymentID = " + supplierPaymentID;
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPayment" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
    <input type="hidden" value="" id="hdnSupplierPaymentID" runat="server" />
    <input type="hidden" value="" id="hdnIsAdd" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
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
                                <label id="lblSupplierPaymentNo" class="lblLink">
                                    <%=GetLabel("No. Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Verifikasi") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtPaymentDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td style="padding-right: 1px;">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="250px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPaymentMethodValueChanged(e); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trBank" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Bank")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" Width="250px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBankRef" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No. Cek/Giro") %></label>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtBankReferenceNo" Width="170px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 25%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("No. Referensi")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="170px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Referensi") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtReferenceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Total Verifikasi")%>
                            </td>
                            <td>
                                <asp:TextBox class="txtCurrency" ID="txtVerificationAmount" Width="170px" ReadOnly="true"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                            ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAllInvoice" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("No. Tukar Faktur")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Jatuh Tempo")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Total Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Terbayar")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Sisa Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" align="center" id="thSelectAll">
                                                            <input id="chkSelectAllInvoice" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("No. Tukar Faktur")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tgl. Jatuh Tempo")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Total Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Terbayar")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Sisa Hutang")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Pembayaran")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PurchaseInvoiceID")%>' />
                                                    </td>
                                                    <td>
                                                        <label class="lblLink lblPurchaseInvoiceNo">
                                                            <%#: Eval("PurchaseInvoiceNo") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("DueDateInString")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("TotalNetTransactionAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("PaymentAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("CustomSisaHutang", "{0:N}")%>
                                                    </td>
                                                    <td align="center">
                                                        <input type="hidden" class="hdnPaymentAmount" value='<%#: Eval("PaymentAmount")%>' />
                                                        <asp:TextBox ID="txtPembayaran" Width="80%" runat="server" ReadOnly="true" CssClass="txtPembayaran txtCurrency" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <input type="hidden" class="keyField" value='<%#:Eval("PurchaseInvoiceID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No. Tukar Faktur" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <label class="lblLink lblPurchaseInvoiceNo">
                                                            <%#: Eval("PurchaseInvoiceNo") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DueDateInString" HeaderText="Tgl. Jatuh Tempo" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="TotalNetTransactionAmount" HeaderText="Total Hutang" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                                <asp:BoundField DataField="PaymentAmount" HeaderText="Jumlah Pembayaran" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="Div1">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
