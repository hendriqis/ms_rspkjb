<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingVerification.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingVerification" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var total = 0;
        function onLoad() {
            setDatePicker('<%=txtPaymentDate.ClientID %>');
            setDatePicker('<%=txtVerificationDate.ClientID %>');

            setDatePicker('<%=txtRSSummaryDateFrom.ClientID %>');
            setDatePicker('<%=txtRSSummaryDateTo.ClientID %>');

            $('#<%=txtRSSummaryDateFrom.ClientID %>').live('change', function (evt) {
                cbpProcessDetail.PerformCallback('refresh');
            });

            $('#<%=txtRSSummaryDateTo.ClientID %>').live('change', function (evt) {
                cbpProcessDetail.PerformCallback('refresh');
            });

            if ($('#<%=hdnIsAdd.ClientID %>').val() == "1") {
                $('#<%=panel1.ClientID %>').show();
                $('#<%=panel2.ClientID %>').hide();
                $('#<%=divInf.ClientID %>').hide();
            }
            else {
                $('#<%=panel1.ClientID %>').hide();
                $('#<%=panel2.ClientID %>').show();
                $('#<%=divInf.ClientID %>').show();
            }

            setCustomToolbarVisibility();
        }

        //#region TransRevenueSharingPaymentNo
        $('#lblTransRevenueSharingPaymentNo.lblLink').live('click', function () {
            openSearchDialog('transrevenuesharingpaymenthd', "", function (value) {
                $('#<%=txtPaymentNo.ClientID %>').val(value);
                onTxtTransRevenueSharingPaymentNoChanged(value);
            });
        });

        $('#<%=txtPaymentNo.ClientID %>').live('change', function () {
            onTxtTransRevenueSharingPaymentNoChanged($(this).val());
        });

        function onTxtTransRevenueSharingPaymentNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        function onCboPaymentMethodValueChanged(evt) {
            var value = cboPaymentMethod.GetValue();
            if (value == '<%=GetRevenuePaymentMethodTransfer() %>') {
                $('#<%=trBank.ClientID %>').removeAttr('style');
                $('#<%=trBankRef.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trBank.ClientID %>').attr('style', 'display:none');
                $('#<%=trBankRef.ClientID %>').attr('style', 'display:none');
            }

            if ($('#<%=txtPaymentNo.ClientID %>').val() == "" || $('#<%=txtPaymentNo.ClientID %>').val() == null) {
                cbpProcessDetail.PerformCallback('refresh');
            }
        }

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            showDeleteConfirmation(function (data) {
                var param = 'justvoid;' + data.GCDeleteReason + ';' + data.Reason;
                onCustomButtonClick(param);
            });
        });

        function setCustomToolbarVisibility() {
            var paymentNo = $('#<%=txtPaymentNo.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            $('#<%=btnVoidByReason.ClientID %>').hide();
            if (paymentNo != '') {
                if (isVoid == 1) {
                    if ($('#<%=hdnTransactionStatus.ClientID %>').val() == "X121^001") {
                        $('#<%=btnVoidByReason.ClientID %>').show();
                    }
                }
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type.includes("justvoid")) {
                onLoadObject(retval);
            } else {
                cbpProcessDetail.PerformCallback('refresh');
            }

            $('#<%=hdnSelectedMember.ClientID %>').val('');
            $('#<%=hdnSelectedMember2.ClientID %>').val('');
        }

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $('.chkSelectAll input').prop('checked', false);
            calculateTotalVerification();
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
            calculateTotalVerification();
        });

        function calculateTotalVerification() {
            var lstSelectedPayment = 0;
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var payment = parseFloat($tr.find('.TotalRevenueSharingAmount').val());
                    lstSelectedPayment += payment;
                }
            });
            $('#<%=txtRSPaymentAmount.ClientID %>').val(lstSelectedPayment).trigger('changeValue');
        }


        function onCbpProcessDetailEndCallback(s) {
            hideLoadingPanel();
            $('.txtPembayaran').each(function () {
                $(this).trigger('changeValue');
            });
            $('.txtRSPaymentAmount').val(0).trigger('changeValue');

            var param = s.cpResult.split('|');
            if (param == "delete,success") {
                onLoadObject($('#<%=txtPaymentNo.ClientID %>').val());
            }
        }

        function getCheckedTransRevenueSharingSummary() {
            var lstSelectedTransRevenueSharingSummary = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedTransRevenueSharingSummary2 = $('#<%=hdnSelectedMember2.ClientID %>').val().split(',');
            var result = '';
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var RevenueType = ($tr.find('.hdnRevenue').val())

                    if (RevenueType == 1) {
                        var idx = lstSelectedTransRevenueSharingSummary.indexOf(key);
                        if (idx < 0) {
                            lstSelectedTransRevenueSharingSummary.push(key);
                        }
                    } else {
                        var idx = lstSelectedTransRevenueSharingSummary2.indexOf(key);
                        if (idx < 0) {
                            lstSelectedTransRevenueSharingSummary2.push(key);
                        }
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var RevenueType = $(this).closest('tr').find('.hdnRevenue').val();

                    if (RevenueType = 1) {
                        var idx = lstSelectedTransRevenueSharingSummary.indexOf(key);
                        if (idx > -1) {
                            lstSelectedTransRevenueSharingSummary.splice(idx, 1);
                        }
                    } else {
                        var idx = lstSelectedTransRevenueSharingSummary2.indexOf(key);
                        if (idx > -1) {
                            lstSelectedTransRevenueSharingSummary2.splice(idx, 1);
                        }
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedTransRevenueSharingSummary.join(','));
            $('#<%=hdnSelectedMember2.ClientID %>').val(lstSelectedTransRevenueSharingSummary2.join(','));
        }

        $('.imgDelete').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($tr);
                    $('#<%=hdnRSSummaryID.ClientID %>').val(entity.ID);
                    cbpProcessDetail.PerformCallback('delete');
                }
            });
        });

        function onBeforeSaveRecord(errMessage) {
            var value = cboPaymentMethod.GetValue();
            if (value == "X391^000") {
                showToast('Process Failed', "Pilih cara pembayaran terlebih dahulu.");
                return false;
            } else {
                if ($('#<%=hdnRSPaymentID.ClientID %>').val() == '0') {
                    getCheckedTransRevenueSharingSummary();
                    if ($('#<%=hdnSelectedMember.ClientID %>').val() != '' || $('#<%=hdnSelectedMember2.ClientID %>').val() != '') {
                        return true;
                    }
                    else {
                        showToast('Process Failed', "Pilih nomor rekap jasa medis terlebih dahulu");
                        return false;
                    }
                } else {
                    return true;
                }
            }
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            getCheckedTransRevenueSharingSummary();
            cbpProcessDetail.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        //#region lblTransRevenueSharingSummaryNo
        $('.lblTransRevenueSharingSummaryNo').die('click');
        $('.lblTransRevenueSharingSummaryNo').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').val();
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingSummary/RevenueSharingSummaryPreviewCtl.ascx");
            openUserControlPopup(url, id, 'Detail Information', 1200, 500);
        });
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var RSPaymentID = $('#<%=hdnRSPaymentID.ClientID %>').val();
            if (RSPaymentID == '' || RSPaymentID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                if (code == 'FN-00091') {
                    filterExpression.text = RSPaymentID;
                    return true;
                }

                else {
                    filterExpression.text = "RSPaymentID = " + RSPaymentID;
                    return true;
                }
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember2" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPayment" runat="server" value="" />
    <input type="hidden" id="hdnSelectedSupplier" runat="server" value="" />
    <input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnRSPaymentID" runat="server" value="0" />
    <input type="hidden" id="hdnRSSummaryID" runat="server" value="" />
    <input type="hidden" id="hdnIsAdd" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="1" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnIsAllowVoidByReason" value="" runat="server" />
    <div>
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
                                <label id="lblTransRevenueSharingPaymentNo" class="lblLink">
                                    <%=GetLabel("No. Pembayaran")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="Label1" class="lblMandatory">
                                    <%=GetLabel("Tanggal Verifikasi")%></label>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtVerificationDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblTransRevenueSharingPaymentDate" class="lblMandatory">
                                    <%=GetLabel("Tanggal Pembayaran")%></label>
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
                        <tr id="trBank" runat="server" style="display:none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Bank")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBank" ClientInstanceName="cboBank" Width="250px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBankRef" runat="server" style="display:none">
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
                                <%=GetLabel("Total Bayar")%>
                            </td>
                            <td>
                                <asp:TextBox class="txtCurrency" ID="txtRSPaymentAmount" Width="170px" ReadOnly="true"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr style="margin: 0 0 0 0;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Rekap JasMed")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 145px" />
                                        <col style="width: 3px" />
                                        <col style="width: 145px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRSSummaryDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRSSummaryDateTo" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Quick Search")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="RSSummaryNo" FieldName="RSSummaryNo" />
                                        <qis:QISIntellisenseHint Text="ParamedicName" FieldName="ParamedicName" />
                                        <qis:QISIntellisenseHint Text="ParamedicCode" FieldName="ParamedicCode" />
                                        <qis:QISIntellisenseHint Text="RSSummaryDate(dd-mm-yyyy)" FieldName="RSSummaryDate" />
                                        <qis:QISIntellisenseHint Text="BankName" FieldName="BankName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
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
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 1px">
                                                            <%=GetLabel("")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No. Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 120px">
                                                            <%=GetLabel("Tgl. Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Dokter/Paramedis")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Nilai Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                            <%=GetLabel("Informasi Dibuat")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
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
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width: 1px">
                                                            <%=GetLabel("")%>
                                                        </th>
                                                        <th align="left" style="width: 150px">
                                                            <%=GetLabel("No. Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 120px">
                                                            <%=GetLabel("Tgl. Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 150px">
                                                            <%=GetLabel("Cara Pembayaran")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Dokter/Paramedis")%>
                                                        </th>
                                                        <th align="right">
                                                            <%=GetLabel("Nilai Rekap JasMed")%>
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                            <%=GetLabel("Informasi Dibuat")%>
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
                                                        <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("RSSummaryID")%>' />
                                                        <input type="hidden" class="TotalRevenueSharingAmount" id="TotalRevenueSharingAmount"
                                                            runat="server" value='<%#: Eval("TotalRevenueSharingAmount")%>' />
                                                    </td>
                                                    <td>
                                                        <input type="hidden" class="hdnRevenue" value='<%#: Eval("Revenue")%>' />
                                                    </td>
                                                    <td>
                                                        <label class="lblLink lblTransRevenueSharingSummaryNo">
                                                            <%#: Eval("RSSummaryNo") %></label>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("cfRSSummaryDateInString") %>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("RevenueSharingPayment") %>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("ParamedicName") %>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("cfTotalRevenueSharingAmountInString") %>
                                                    </td>
                                                    <td align="center">
                                                        <%#: Eval("CreatedByName") %>
                                                        <br />
                                                        <%#: Eval("cfCreatedDateInString") %>
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
                                                <asp:TemplateField HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <input type="hidden" class="keyField" value='<%#:Eval("RSSummaryID") %>' />
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                        src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No. Rekap Jasa Medis" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <label class="lblLink lblTransRevenueSharingSummaryNo">
                                                            <%#: Eval("RSSummaryNo") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cfRSSummaryDateInString" HeaderText="Tgl. Rekap Jasa Medis"
                                                    HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Paramedis" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="cfTotalRevenueSharingAmountInString" HeaderText="Nilai Rekap Jasa Medis"
                                                    HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" />
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
    <div id="divInf" runat="server">
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
                                        <%=GetLabel(":")%>
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
                                        <%=GetLabel(":")%>
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
                                        <div runat="server" id="div2">
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
                                        <div runat="server" id="div3">
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
                                        <%=GetLabel(":")%>
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
                                        <%=GetLabel(":")%>
                                    </td>
                                    <td>
                                        <div runat="server" id="divLastUpdatedDate">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Approved Oleh") %>
                                    </td>
                                    <td align="center">
                                        <%=GetLabel(":")%>
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedBy">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%=GetLabel("Approved Pada")%>
                                    </td>
                                    <td align="center">
                                        <%=GetLabel(":")%>
                                    </td>
                                    <td>
                                        <div runat="server" id="divApprovedDate">
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
</asp:Content>
