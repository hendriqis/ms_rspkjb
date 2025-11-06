<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ARReceiptEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARReceiptEntry" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPrintARReceipt" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print")%></div>
    </li>
    <li id="btnVoidARReceipt" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Delete")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();

            setDatePicker('<%=txtReceiptDate.ClientID %>');
            $('#<%=txtReceiptDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        }

        function setCustomToolbarVisibility() {
            var hdnIsEditable = $('#<%=hdnIsEditable.ClientID %>').val();
            var txtReceiptNo = $('#<%=txtReceiptNo.ClientID %>').val();

            if (txtReceiptNo == "") {
                $('#<%=btnPrintARReceipt.ClientID %>').hide();
                $('#<%=btnVoidARReceipt.ClientID %>').hide();
            } else {
                if (hdnIsEditable == "1") {
                    $('#<%=btnPrintARReceipt.ClientID %>').show();
                    $('#<%=btnVoidARReceipt.ClientID %>').show();
                } else {
                    $('#<%=btnPrintARReceipt.ClientID %>').hide();
                    $('#<%=btnVoidARReceipt.ClientID %>').hide();
                }
            }
        }

        $('#<%=txtReceiptTime.ClientID %>').live('change', function () {
            var checkTime = $('#<%=txtReceiptTime.ClientID %>').val();
            checkTimeFormat(checkTime);
        });

        function checkTimeFormat(value) {
            if (value.substr(2, 1) == ':') {
                if (!value.match(/^\d\d:\d\d/)) {
                    displayErrorMessageBox('ERROR', "Format jam salah !");
                }
                else if (parseInt(value.substr(0, 2)) >= 24 || parseInt(value.substr(3, 2)) >= 60) {
                    displayErrorMessageBox('ERROR', "Format jam salah !");
                }
            }
            else {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
        }

        $('#<%=btnPrintARReceipt.ClientID %>').live('click', function () {
            showLoadingPanel();
            var arReceiptID = $('#<%=hdnARReceiptID.ClientID %>').val();
            var printNumber = $('#<%=hdnPrintNumber.ClientID %>').val();
            var reportCode = $('#<%=hdnReportCode.ClientID%>').val();

            var filterExpression = 'ARReceiptID = ' + arReceiptID;
            Methods.getObject('GetARReceiptHdList', filterExpression, function (result) {
                printNumber = result.PrintNumber;
                $('#<%=hdnPrintNumber.ClientID %>').val(printNumber);
            });

            if (printNumber == "0") {
                cbpView.PerformCallback('firstprint');
                var filterExpression = "ARReceiptID = " + arReceiptID;
                openReportViewer(reportCode, filterExpression);
            } else {
                var url = ResolveUrl('~/Program/ARReceipt/ARReceiptReprintCtl.ascx');
                openUserControlPopup(url, arReceiptID, 'Re-print', 400, 230);
            }
        });

        $('#<%=btnVoidARReceipt.ClientID %>').live('click', function () {
            showLoadingPanel();
            var url = ResolveUrl('~/Program/ARReceipt/ARReceiptVoidCtl.ascx');
            var arReceiptID = $('#<%=hdnARReceiptID.ClientID %>').val();
            openUserControlPopup(url, arReceiptID, 'Delete Reason', 400, 230);
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Anda yakin akan menghapus detail kwitansi ini?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnARReceiptDtID.ClientID %>').val(entity.ID);
                    cbpView.PerformCallback('delete');
                }
            });
        });

        $('#lblQuickPick.lblLink').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Program/ARReceipt/ARReceiptQuickPicksCtl.ascx');
                var arReceiptID = $('#<%=hdnARReceiptID.ClientID %>').val();
                openUserControlPopup(url, arReceiptID, 'Quick Picks', 1000, 400);
            }
        });

        //#region Receipt No
        $('#lblReceiptNo.lblLink').live('click', function () {
            var filterExpression = "1=1";
            openSearchDialog('arreceipthd', filterExpression, function (value) {
                $('#<%=txtReceiptNo.ClientID %>').val(value);
                ontxtReceiptNoChanged(value);
            });
        });

        $('#<%=txtReceiptNo.ClientID %>').live('change', function () {
            ontxtReceiptNoChanged($(this).val());
        });

        function ontxtReceiptNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region Business Partner Bill To
        function getBusinessPartnerBillToFilter() {
            var filterExpression = "GCBusinessPartnerType = 'X017^002' AND IsDeleted = 0 AND IsActive = 1";
            return filterExpression;
        }

        $('#lblBusinessPartnerBillToID.lblLink').live('click', function () {
            openSearchDialog('businesspartners', getBusinessPartnerBillToFilter(), function (value) {
                $('#<%=txtBusinessPartnerBillToCode.ClientID %>').val(value);
                ontxtBusinessPartnerBillToCodeChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerBillToCode.ClientID %>').live('change', function () {
            ontxtBusinessPartnerBillToCodeChanged($(this).val());
        });

        function ontxtBusinessPartnerBillToCodeChanged(value) {
            var filterExpression = getBusinessPartnerBillToFilter() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerBillToID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerBillToName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%=txtPrintAsName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerBillToID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerBillToCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerBillToName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Bank VA
        $('#lblBankVA.lblLink').live('click', function () {
            var bpID = $('#<%=hdnBusinessPartnerBillToID.ClientID %>').val();
            if (bpID == "") {
                bpID = "0";
            }
            var filter = "IsDeleted = 0 AND BusinessPartnerID = " + bpID;
            openSearchDialog('businesspartnervirtualaccount', filter, function (value) {
                var filterID = filter + " AND ID = " + value;
                Methods.getObject('GetvBusinessPartnerVirtualAccountList', filterID, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val(result.ID);
                        $('#<%=txtBankName.ClientID %>').val(result.BankName);
                        $('#<%=txtAccountNo.ClientID %>').val(result.AccountNo);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val("");
                        $('#<%=txtBankName.ClientID %>').val("");
                        $('#<%=txtAccountNo.ClientID %>').val("");
                    }
                });
            });
        });
        //#endregion

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    cbpView.PerformCallback('refresh');
                }
            }
        }
        //#endregion

        function onBeforeSaveRecord(errMessage) {
            if ($('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "" || $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "0") {
                errMessage.text = "Silakan Pilih Bank & VA Terlebih Dahulu";
                return false;
            } else {
                return true;
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "" || $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "0") {
                showToast('Warning', 'Silakan Pilih Bank & VA Terlebih Dahulu');
            } else {
                cbpView.PerformCallback('refresh');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = $('#<%:hdnARReceiptID.ClientID %>').val();
            if (param != "" && param != 0) {
                return param;
            } else {
                showToast('Failed', 'Maaf, pilih No. Kwitansi terlebih dahulu.');
                return false;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var arReceiptID = $('#<%=hdnARReceiptID.ClientID %>').val();
            if (arReceiptID == '' || arReceiptID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "ARReceiptID = " + arReceiptID;
                return true;
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="0" />
    <input type="hidden" id="hdnARReceiptID" runat="server" value="0" />
    <input type="hidden" id="hdnARReceiptDtID" runat="server" value="0" />
    <input type="hidden" id="hdnPrintNumber" runat="server" value="0" />
    <input type="hidden" id="hdnReportCode" runat="server" value="0" />
    <div style="overflow-x: hidden;">
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
                                <label id="lblReceiptNo" class="lblLink">
                                    <%=GetLabel("No. Kwitansi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Kwitansi")%></label>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtReceiptDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jam Kwitansi")%></label>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtReceiptTime" Width="120px" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblBusinessPartnerBillToID" class="lblLink lblMandatory">
                                    <%=GetLabel("Tagih ke-")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerBillToID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerBillToCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerBillToName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblBankVA">
                                    <%=GetLabel("Bank & VA") %></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" value="" runat="server" />
                                <table>
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBankName" Width="200px" ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAccountNo" Width="200px" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cetak Atas Nama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintAsName" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="400px" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nilai Klaim")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptAmount" Width="300px" runat="server" ReadOnly="true" Style="text-align: right" />
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
                                    <%=GetLabel("Jumlah Cetak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrintNumber" Width="100px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Alasan Cetak Ulang (terakhir)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReprintReason" Width="300px" ReadOnly="true" runat="server" Multiline="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cetak Ulang (terakhir) Oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReprintBy" Width="300px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cetak Ulang (terakhir) Pada")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReprintDateTime" Width="300px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
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
                                                    <input type="hidden" value="<%#:Eval("ARReceiptID") %>" bindingfield="ARReceiptID" />
                                                    <input type="hidden" value="<%#:Eval("ARInvoiceID") %>" bindingfield="ARInvoiceID" />
                                                    <input type="hidden" value="<%#:Eval("ARInvoiceNo") %>" bindingfield="ARInvoiceNo" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerID") %>" bindingfield="BusinessPartnerID" />
                                                    <input type="hidden" value="<%#:Eval("MRN") %>" bindingfield="MRN" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ARInvoiceNo" HeaderText="No. Invoice" HeaderStyle-Width="170px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfARInvoiceDateInString" HeaderText="Tgl. Invoice" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cfTotalClaimedAmountInString" HeaderText="Nilai Klaim"
                                                HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfARInfo" HeaderText="Informasi Piutang" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfCreatedInfo" HeaderText="Informasi Dibuat" HeaderStyle-Width="180px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cfLastUpdatedInfo" HeaderText="Informasi Terakhir Diubah"
                                                HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No data to display.")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink lblQuickPick" id="lblQuickPick">
                            <%= GetLabel("Tambah Invoice")%></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
