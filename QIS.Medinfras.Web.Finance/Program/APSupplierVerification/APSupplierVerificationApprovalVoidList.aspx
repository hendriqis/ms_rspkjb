<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="APSupplierVerificationApprovalVoidList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.APSupplierVerificationApprovalVoidList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnReopen" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Re-Open")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var total = 0;

        function onLoad() {
            $('#lblRefresh').click(function () {
                onRefreshGridView();
            });

            $('#<%=btnReopen.ClientID %>').click(function (evt) {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                    var id = '';

                    for (var i = 0; i < lstSelectedMember.length; i++) {
                        if (id == '') {
                            id = lstSelectedMember[i];
                        }
                        else {
                            id += "," + lstSelectedMember[i];
                        }
                    }

                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        var filterExpression = "SupplierPaymentID IN (" + id + ")";
                        var suppPaymentNo = '';
                        Methods.getListObject('GetvSupplierPaymentHdList', filterExpression, function (resultCheck) {
                            for (i = 0; i < resultCheck.length; i++) {
                                if (suppPaymentNo == '') {
                                    suppPaymentNo = resultCheck[i].SupplierPaymentNo;
                                }
                                else {
                                    suppPaymentNo += ", " + resultCheck[i].SupplierPaymentNo;
                                }
                            }
                        });
                        onCustomButtonClick('reopen');
                        showToast('Re-Open Success', "Proses Re-Open pada No. Pembayaran <b>" + suppPaymentNo + "</b> Sudah Berhasil Dilakukan");
                    }
                }
            });
            
            $('.txtTotalToBeVerified').change(function () {
                $(this).trigger('changeValue');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedPaymentMethod = $('#<%=hdnSelectedMemberPaymentMethod.ClientID %>').val().split(',');
            var lstSelectedBankID = $('#<%=hdnSelectedMemberBankID.ClientID %>').val().split(',');
            var lstSelectedReference = $('#<%=hdnSelectedMemberReference.ClientID %>').val().split(',');
            $('.lvwView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var paymentMethod = $tr.find('.hdnPaymentMethod').val();
                    var bankID = $tr.find('.hdnBankID').val();
                    var referenceNo = $tr.find('.txtBankReferenceNo').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedPaymentMethod.push(paymentMethod);
                        lstSelectedBankID.push(bankID);
                        lstSelectedReference.push(referenceNo);
                    }
                    else {
                        lstSelectedPaymentMethod[idx] = paymentMethod;
                        lstSelectedBankID[idx] = bankID;
                        lstSelectedReference[idx] = referenceNo;
                    }

                    $('#<%=hdnSelectedMemberPaymentMethod.ClientID %>').val();
                    $('#<%=hdnSelectedMemberBankID.ClientID %>').val();
                    $('#<%=hdnSelectedMemberReference.ClientID %>').val();
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedPaymentMethod.splice(idx, 1);
                        lstSelectedBankID.splice(idx, 1);
                        lstSelectedReference.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberPaymentMethod.ClientID %>').val(lstSelectedPaymentMethod.join(','));
            $('#<%=hdnSelectedMemberBankID.ClientID %>').val(lstSelectedBankID.join(','));
            $('#<%=hdnSelectedMemberReference.ClientID %>').val(lstSelectedReference.join(','));
        }

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            $('#<%=hdnSelectedMemberPaymentMethod.ClientID %>').val('');
            $('#<%=hdnSelectedMemberBankID.ClientID %>').val('');
            $('#<%=hdnSelectedMemberReference.ClientID %>').val('');
            $('#<%=txtTotalToBeVerified.ClientID %>').val('0.00');
            if (type == 'reopen') {
                onRefreshGridView();
            }
            else {
                onRefreshGridView();
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            getCheckedMember();
            onLoad();
        }

        function onRefreshGridView() {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        $('.chkIsSelected').die('change');
        $('.chkIsSelected').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).find('input').is(':checked')) {
                total += parseFloat($tr.find('.hdnTotal').val());
                $('#<%=txtTotalToBeVerified.ClientID %>').val(total).trigger('changeValue');
            }
            else {
                total -= parseFloat($(this).closest('tr').find('.hdnTotal').val());
                $('#<%=txtTotalToBeVerified.ClientID %>').val(total).trigger('changeValue');
            }
        });

        $('#chkSelectAllPayment').die('change');
        $('#chkSelectAllPayment').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected').each(function () {
                $chk = $(this).find('input');
                $chk.prop('checked', isChecked);
                $chk.change();
            });
        });

        $('.lblSupplierPaymentNo').die('click');
        $('.lblSupplierPaymentNo').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').val();
            var url = ResolveUrl("~/Program/APSupplierVerification/APSupplierPaymentVerificationDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Information', 1100, 500);
        });
            
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPurchaseInvoiceID" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPaymentMethod" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberBankID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberReference" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" id="hdnListID" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowApproved" value="" runat="server" />
    <div>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent">
                        <colgroup>
                            <col style="width: 270px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jumlah Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox class="txtCurrency" ID="txtTotalToBeVerified" Width="170px" runat="server"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span class="lblLink" id="lblRefresh">[refresh]</span>
                    <br />
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center">
                                                        <input id="chkSelectAllPayment" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No. Pembayaran")%>
                                                    </th>
                                                    <th style="width: 80px" align="center">
                                                        <%=GetLabel("Cara Pembayaran")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Bank")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("No. Cek/Giro")%>
                                                    </th>
                                                    <th style="width: 90px" align="center">
                                                        <%=GetLabel("Status Transaksi")%>
                                                    </th>
                                                    <th style="width: 90px" align="center">
                                                        <%=GetLabel("Status Approval")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Approval Proposed Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Approval Approve Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Dibuat Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Verifikasi Oleh")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Nilai")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="lvwView grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center">
                                                        <input id="chkSelectAllPayment" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No. Pembayaran")%>
                                                    </th>
                                                    <th style="width: 80px" align="center">
                                                        <%=GetLabel("Cara Pembayaran")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Bank")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%=GetLabel("No. Cek/Giro")%>
                                                    </th>
                                                    <th style="width: 90px" align="center">
                                                        <%=GetLabel("Status Transaksi")%>
                                                    </th>
                                                    <th style="width: 90px" align="center">
                                                        <%=GetLabel("Status Approval")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Approval Proposed Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Approval Approve Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Dibuat Oleh")%>
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Verifikasi Oleh")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Nilai")%>
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
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("SupplierPaymentID")%>' />
                                                    <input type="hidden" class="hdnTotal" id="hdnTotal" runat="server" value='<%#: Eval("TotalPaymentAmount")%>' />
                                                </td>
                                                <td>
                                                    <label class="lblLink lblSupplierPaymentNo">
                                                        <%#: Eval("SupplierPaymentNo") %></label>
                                                </td>
                                                <td align="center">
                                                    <input type="hidden" class="hdnPaymentMethod" id="hdnPaymentMethod" runat="server" value='0' />
                                                    <label runat="server" class="lblNormal">
                                                        <%#: Eval("PaymentMethod") %></label>
                                                </td>
                                                <td align="center">
                                                    <input type="hidden" class="hdnBankID" id="hdnBankID" runat="server" value='0' />
                                                    <label runat="server" class="lblNormal">
                                                        <%#: Eval("BankName") %></label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtBankReferenceNo" CssClass="txtBankReferenceNo" Text='<%#: Eval("BankReferenceNo") %>'
                                                        Width="90%" runat="server" Enabled="false"/>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("TransactionStatusWatermark") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("ApprovalTransactionStatusWatermark") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("ApprovalProposedByName") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("ApprovalApprovedByName") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("CreatedByName") %>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("VerifiedByName") %>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("TotalPaymentAmount", "{0:N}")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging" style="display: none">
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
