<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientDtPage/MPBasePatientDtPageTrx.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePatientProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePatientProcessEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/ARInvoicePatient/ARInvoicePatientToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtInvoiceDate.ClientID %>');

            var oIsAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
            var oIsAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

            if (oIsAllowBackdate != "1") {
                $('#<%=txtInvoiceDate.ClientID %>').datepicker('option', 'minDate', '0');
            }

            if (oIsAllowFuturedate != "1") {
                $('#<%=txtInvoiceDate.ClientID %>').datepicker('option', 'maxDate', '0');
            }

            setDatePicker('<%=txtDocumentDate.ClientID %>');
            setDatePicker('<%=txtARDocumentReceiveDate.ClientID %>');
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');

            $('#btnRefresh').click(function () {
                cbpView.PerformCallback('Refresh');
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    showToast('Warning', 'Silakan Pilih Piutang Terlebih Dahulu');
                }
                else if ($('#<%=txtInvoiceDate.ClientID %>').val() == "") {
                    showToast('Warning', 'Silakan Pilih Tanggal Invoice Terlebih Dahulu');
                }
                else if ($('#<%=txtDocumentDate.ClientID %>').val() == "") {
                    showToast('Warning', 'Silakan Pilih Tanggal Kirim Invoice Terlebih Dahulu');
                }
                else if ($('#<%=txtARDocumentReceiveDate.ClientID %>').val() == "") {
                    showToast('Warning', 'Silakan Pilih Tanggal Terima Dokumen Terlebih Dahulu');
                }
                else if ($('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "" || $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val() == "0") {
                    showToast('Warning', 'Silakan Pilih Bank & VA Terlebih Dahulu');
                } else {
                    onCustomButtonClick('process');
                }
            });
        }

        $('#<%=txtInvoiceDate.ClientID %>').live('change', function () {
            var invoiceDate = $('#<%=txtInvoiceDate.ClientID %>').val();
            var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
            var isAllowBackdate = $('#<%=hdnIsAllowBackDate.ClientID %>').val();
            var isAllowFuturedate = $('#<%=hdnIsAllowFutureDate.ClientID %>').val();

            var from = invoiceDate.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (isAllowBackdate != "1") {
                if (f < t) {
                    $('#<%=txtInvoiceDate.ClientID %>').val(dateToday);
                }
            }

            if (isAllowFuturedate != "1") {
                if (f > t) {
                    $('#<%=txtInvoiceDate.ClientID %>').val(dateToday);
                }
            }
        });

        $('.chkIsSelected input').live('click', function () {
            $('.chkCheckAll input').prop('checked', false);
            calculateTotal();
        });

        $('#chkCheckAll').live('click', function () {
            var isChecked = $(this).is(':checked');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                calculateTotal();
            });
        });

        function calculateTotal() {
            var totalAmount = 0;

            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                totalAmount += parseFloat($tr.find('.PaymentAmount').val());
            });

            $('#<%=txtTotalAmount.ClientID %>').val(totalAmount).trigger('changeValue');
        }

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses Pembuatan Tagihan Piutang Pasien Pribadi Berhasil Dibuat Dengan Nomor <b>' + retval + '</b>', function () {
                $('#<%=hdnSelectedMember.ClientID %>').val('');

                $('#<%=hdnBusinessPartnerVirtualAccountID.ClientID %>').val("");
                $('#<%=txtBankName.ClientID %>').val("");
                $('#<%=txtAccountNo.ClientID %>').val("");
                $('#<%=txtRemarks.ClientID %>').val("");
                $('#<%=txtTotalAmount.ClientID %>').val(0).trigger('changeValue');

                cbpView.PerformCallback('Refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedMemberPaymentID = $('#<%=hdnSelectedMemberPaymentID.ClientID %>').val().split(',');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var paymentID = $tr.find('.paymentID').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (lstSelectedMember.indexOf(key) < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberPaymentID.push(paymentID);
                    }
                    else {
                        lstSelectedMemberPaymentID[idx] = paymentID;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedMember.indexOf(key) > -1) {
                        lstSelectedMember.splice(lstSelectedMember.indexOf(key), 1);
                        lstSelectedMemberPaymentID.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberPaymentID.ClientID %>').val(lstSelectedMemberPaymentID.join(','));
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            getCheckedMember();
        }
        //#endregion

        //#region Bank VA
        $('#lblBankVA.lblLink').live('click', function () {
            var filter = "IsDeleted = 0 AND BusinessPartnerID = 1";
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

    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnEmployeeID" runat="server" value="" />
        <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
        <input type="hidden" id="hdnSelectedMemberPaymentID" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerVirtualAccountID" runat="server" value="" />
        <input type="hidden" id="hdnSetvarLeadTime" runat="server" value="0" />
        <input type="hidden" id="hdnSetvarHitungJatuhTempoDari" runat="server" value="0" />
        <input type="hidden" id="hdnDefaultSelisihHariUntukFilterPeriodeTransaksi" runat="server" value="0" />
        <input type="hidden" value="0" id="hdnDateToday" runat="server" />
        <input type="hidden" value="0" id="hdnIsAllowBackDate" runat="server" />
        <input type="hidden" value="0" id="hdnIsAllowFutureDate" runat="server" />
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4>
                        <%=GetLabel("Informasi Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 25%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td style="width: 100%">
                                    <table>
                                        <colgroup>
                                            <col style="width: 250px" />
                                            <col style="width: 100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td align="left">
                                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                                    Width="100%" />
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox runat="server" ID="chkIsExclusion" Text=" Is Exclusion?" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Status Pendaftaran")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                        Width="250px" runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="text-decoration: underline">
                                        <%=GetLabel("Filter Berdasarkan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboFilterBy" ClientInstanceName="cboFilterBy" Width="250px"
                                        runat="server" BackColor="Yellow">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("Periode Transaksi") %></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 30px" />
                                            <col style="width: 120px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" />
                                            </td>
                                            <td>
                                                <label>
                                                    <%=GetLabel("s/d")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="text-decoration: underline">
                                        <%=GetLabel("Urut Berdasarkan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboSortBy" ClientInstanceName="cboSortBy" Width="250px" runat="server"
                                        BackColor="Pink">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <h4>
                        <%=GetLabel("Data Invoice")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 25%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tgl Invoice")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Width="120px" ID="txtInvoiceDate" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tgl Kirim Invoice")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Width="120px" ID="txtDocumentDate" CssClass="datepicker" />
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
                                    <asp:TextBox runat="server" Width="400px" ID="txtARDocumentReceiveByName" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tempo Invoice")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboTerm" ClientInstanceName="cboTerm" Width="150px" runat="server" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Bank") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboBank" runat="server" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblBankVA">
                                        <%=GetLabel("Bank & VA") %></label>
                                </td>
                                <td>
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
                                        <%=GetLabel("Cetak Atas Nama")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrintAsName" Width="300px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Invoice") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Width="150px" ID="txtTotalAmount" CssClass="txtCurrency"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="position: relative;" id="dView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="50px">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="paymentID" value="<%#: Eval("PaymentID")%>" />
                                                        <input type="hidden" class="PaymentAmount" value="<%#: Eval("PaymentAmount")%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Informasi Pendaftaran" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("RegistrationNo") %></b></div>
                                                        <div>
                                                            <%#: Eval("DepartmentID") %>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("Tgl.Reg : ") %></label>
                                                            <label class="lblNormal">
                                                                <%#: Eval("RegistrationDateInString")%></label></div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("Tgl.Pulang : ") %></label>
                                                            <label class="lblNormal">
                                                                <%#: Eval("DischargeDateInString")%></label></div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Tgl/No Piutang" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("PaymentDateInString")%></div>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("PaymentNo")%></b></div>
                                                        <div>
                                                            <hr style="margin: 0 0 0 0;" />
                                                        </div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("CreatedBy : ") %><%#: Eval("CreatedByName")%></label>
                                                        </div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("CreatedDate : ") %><%#: Eval("cfCreatedDateInString")%></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Pasien/Pegawai" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("MedicalNo")%></div>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("PatientName")%></b></div>
                                                        <div>
                                                            <hr style="margin: 0 0 0 0;" />
                                                        </div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("NIK : ") %><%#: Eval("EmployeeCode")%></label>
                                                        </div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("Nama : ") %><%#: Eval("EmployeeName")%></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PaymentAmount" HeaderText="Total Piutang" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="200px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
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
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
