<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingSummaryEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditableCustom.ClientID %>').val() == '1') {
                setDatePicker('<%=txtRSSummaryDate.ClientID %>');
                $('#lblAddTransaction').show();
            }
            else {
                $('#lblAddTransaction').hide();
            }
        }

        //#region RS Summary No
        $('#lblRSSummaryNo.lblLink').live('click', function () {
            var filterExpression = "<%=GetFilterExpression() %>";
            openSearchDialog('transrevenuesharingsummaryhd', filterExpression, function (value) {
                $('#<%=txtRSSummaryNo.ClientID %>').val(value);
                ontxtRSSummaryNoChanged(value);
            });
        });

        $('#<%=txtRSSummaryNo.ClientID %>').live('change', function () {
            ontxtRSSummaryNoChanged($(this).val());
        });

        function ontxtRSSummaryNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        $('#lblAddTransaction').live('click', function () {
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (transactionStatus == 'X121^001' || transactionStatus == '') {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var id = $('#<%=hdnRSSummaryID.ClientID %>').val();
                    var url = ResolveUrl('~/Program/RevenueSharing/RevenueSharingSummary/RevenueSharingSummaryEntryDetailCtl.ascx');
                    openUserControlPopup(url, id, 'Pilih Transaksi', 1200, 400);
                }
            } else {
                showToast('Process Failed', 'Error Message : Status transaksi rekap jasa medis sudah BUKAN OPEN lagi, tidak dapat menambah detail.');
            }
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (transactionStatus == 'X121^001') {
                $row = $(this).closest('tr').parent().closest('tr');
                showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                    if (result) {
                        var entity = rowToObject($row);
                        $('#<%=hdnRSSummaryDtID.ClientID %>').val(entity.ID);
                        cbpProcess.PerformCallback('delete');
                    }
                });
            } else {
                showToast('Process Failed', 'Error Message : Status transaksi rekap jasa medis sudah BUKAN OPEN lagi, tidak dapat menghapus detail.');
            }
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            if (param != null && param != "" && param != "0") {
                $('#<%=hdnRSSummaryID.ClientID %>').val(param);
                var filterExpression = 'RSSummaryID = ' + param;
                Methods.getObject('GetTransRevenueSharingSummaryHdList', filterExpression, function (result) {
                    $('#<%=txtRSSummaryNo.ClientID %>').val(result.RSSummaryNo);
                    onLoadObject(result.RSSummaryNo);
                });
            }
            cbpView.PerformCallback('refresh');
        }

        function cbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }

            if ($('#<%=hdnIsEditableCustom.ClientID %>').val() == '1') {
                setDatePicker('<%=txtRSSummaryDate.ClientID %>');
                $('#lblAddTransaction').show();
            }
            else {
                $('#lblAddTransaction').hide();
            }
        }
        function onCbpProcesEndCallback(s) {
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

            if ($('#<%=hdnRSSummaryID.ClientID %>').val() != null && $('#<%=hdnRSSummaryID.ClientID %>').val() != "" && $('#<%=hdnRSSummaryID.ClientID %>').val() != "0") {
                var filterExpression = 'RSSummaryID = ' + $('#<%=hdnRSSummaryID.ClientID %>').val();
                Methods.getObject('GetTransRevenueSharingSummaryHdList', filterExpression, function (result) {
                    $('#<%=txtRSSummaryNo.ClientID %>').val(result.RSSummaryNo);
                    onLoadObject(result.RSSummaryNo);
                });
            }
        }

        //#region Paging
        var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var rsSummaryID = $('#<%=hdnRSSummaryID.ClientID %>').val();
            if (rsSummaryID == '' || rsSummaryID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else if (code == 'FN-00098') {
                filterExpression.text = "RSSummaryID = " + rsSummaryID;
                return true;
            }
            else if (code == 'FN-00228') {
                filterExpression.text = rsSummaryID;
                return true;
            }
            else {
                filterExpression.text = "RSSummaryID = " + rsSummaryID;
                return true;
            }
        }
    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnPageCount" runat="server" value="" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnRSSummaryID" runat="server" value="" />
        <input type="hidden" id="hdnRSSummaryDtID" runat="server" value="" />
        <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="" />
        <input type="hidden" id="hdnIsEditableCustom" runat="server" value="1" />
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <label id="lblRSSummaryNo" class="lblLink">
                                    <%=GetLabel("Nomor Rekap")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRSSummaryNo" Width="150px" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Rekap") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRSSummaryDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRemarks" Width="350px" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Transaksi Bruto") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBrutoAmount" Width="180px" ReadOnly="true" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Rekap") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSummaryAmount" Width="180px" ReadOnly="true" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Penyesuaian") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSummaryAdjustmentAmount" Width="180px" ReadOnly="true"
                                    Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Akhir") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSummaryEndAmount" Width="180px" ReadOnly="true"
                                    Style="text-align: right" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <table class="grdView grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th>
                                            </th>
                                            <th class="keyField">
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("Nomor Bukti") %>
                                            </th>
                                            <th style="width: 120px" align="left">
                                                <%=GetLabel("Tanggal Proses") %>
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("Alokasi Pajak") %>
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("Rencana Pembayaran") %>
                                            </th>
                                            <th style="width: 150px" align="left">
                                                <%=GetLabel("Jenis Periode") %>
                                            </th>
                                            <th style="width: 200px" align="left">
                                                <%=GetLabel("Periode") %>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Total Transaksi Bruto") %>
                                            </th>
                                            <th align="right">
                                                <%=GetLabel("Total Jasa Medis") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%#ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                            alt="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <input type="hidden" bindingfield="ID" value='<%#: Eval("ID")%>' />
                                                            <input type="hidden" bindingfield="RSTransactionID" value='<%#: Eval("RSTransactionID")%>' />
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <label class="lblNormal">
                                                            <%#:Eval("RevenueSharingNo") %></label>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("cfProcessedDateInString")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("Reduction")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("PaymentMethod")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("PeriodeType")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#:Eval("cfPeriodeText")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("cfBrutoTransactionAmountInString")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("cfRevenueSharingAmountInString")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddTransaction">
                            <%= GetLabel("Tambah Transaksi")%>
                        </span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
