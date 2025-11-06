<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/CustomerPage/MPBaseCustomerPageTrx.master" AutoEventWireup="true" 
    CodeBehind="ARInvoicePayerProcessEntry2.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerProcessEntry2" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePayer/ARInvoicePayerToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1')
                $('#lblCopyPayment').show();
            else
                $('#lblCopyPayment').hide();

            setDatePicker('<%=txtInvoiceDate.ClientID %>');

            $('#lblCopyPayment').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var id = $('#<%=hdnARInvoiceID.ClientID %>').val();
                    var url = ResolveUrl('~/Program/ARInvoicePayer/ARInvoicePayerProcess/ARInvoicePayerProcessEntryDtCtl.ascx');
                    openUserControlPopup(url, id, 'Saling Piutang', 1200, 600);
                }
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=hdnARInvoiceID.ClientID %>').val() == '0') {
                $('#<%=hdnARInvoiceID.ClientID %>').val(param);
                var filterExpression = 'ARInvoiceID = ' + param;
                Methods.getObject('GetARInvoiceHdList', filterExpression, function (result) {
                    $('#<%=txtARInvoiceNo.ClientID %>').val(result.ARInvoiceNo);
                    onLoadObject(result.ARInvoiceNo);
                });
                onAfterCustomSaveSuccess();
            }
            else
                cbpView.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses Pembuatan Tagihan Piutang Pasien Instansi Berhasil Dibuat Dengan Nomor <b>' + retval + '</b>', function () {
                cbpView.PerformCallback('Refresh');
            });
        }

        //#region AR Invoice
        function onGetARInvoiceFilterExpression() {
            var filterExpression = "<%:onGetARInvoiceFilterExpression() %>";
            return filterExpression;
        }

        $('#lblARInvoiceNo.lblLink').live('click', function () {
            openSearchDialog('arinvoicehd', onGetARInvoiceFilterExpression(), function (value) {
                $('#<%=txtARInvoiceNo.ClientID %>').val(value);
                onTxtProcessedDateChanged(value);
            });
        });

        $('#<%=txtARInvoiceNo.ClientID %>').live('change', function () {
            onTxtProcessedDateChanged($(this).val());
        });

        function onTxtProcessedDateChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion
    </script>
    <div>
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <input type="hidden" id="hdnARInvoiceID" runat="server" />
        <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
        <input type="hidden" value="" id="hdnPageCount" runat="server" />
        <div class="pageTitle"><div style="font-size: 1.1em"><%=GetLabel("Proses Piutang Pasien Instansi")%></div></div>
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col style="width:50%" />
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal lblLink" id="lblARInvoiceNo"><%=GetLabel("Nomor Invoice") %></label></td>
                            <td><asp:TextBox ID="txtARInvoiceNo" Width="150px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal Invoice") %></label></td>
                            <td><asp:TextBox runat="server" Width="120px" ID="txtInvoiceDate" CssClass="datepicker" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tempo Invoice")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="200px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:120px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Bank") %></label></td>
                            <td><dxe:ASPxComboBox ID="cboBank" runat="server" Width="150px" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top; padding-top:5px;"><%=GetLabel("Keterangan") %></td>
                            <td><asp:TextBox ID="txtRemarks" Width="400px" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="position:relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" 
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="PaymentNo" HeaderText="No Piutang" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"  />
                                                <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                                <asp:BoundField DataField="TransactionAmount" HeaderText="Total Piutang" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="200px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
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
                        <div style="width:100%;text-align:center">
                            <span class="lblLink" id="lblAddInvoice" style="display:none; margin-right: 300px;" > <%= GetLabel("Tambah Faktur Tanpa No. Piutang")%></span>
                            <span class="lblLink" id="lblCopyPayment" > <%= GetLabel("Salin Piutang")%></span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>