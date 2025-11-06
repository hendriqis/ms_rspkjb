<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingProcessEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessRevenueSharing" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtRevenueSharingDate.ClientID %>');

            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        }

        $(function () {
            getCheckedMember();
        });

        $('#btnRefresh').live('click', function () {
            getCheckedMember();
            cbpView.PerformCallback('refresh');
            calculateTotal();
        });

        $('#<%=btnProcessRevenueSharing.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            else
                onCustomButtonClick('process');
        });

        function onCboDepartmentChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnParamDepartmentID.ClientID %>').val(value);
        }

        //#region Business Partners
        function onGetPayerFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPayer.lblLink').live('click', function () {
            openSearchDialog('payer', onGetPayerFilterExpression(), function (value) {
                $('#<%=txtPayerCode.ClientID %>').val(value);
                onTxtPayerCodeChanged(value);
            });
        });

        $('#<%=txtPayerCode.ClientID %>').live('change', function () {
            onTxtPayerCodeChanged($(this).val());
        });

        function onTxtPayerCodeChanged(value) {
            var filterExpression = onGetPayerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtPayerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtPayerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=txtPayerCode.ClientID %>').val('');
                    $('#<%=hdnPayerID.ClientID %>').val('');
                    $('#<%=txtPayerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Revenue Sharing
        function onGetRevenueSharingFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblRevenueSharing.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', onGetRevenueSharingFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                ontxtRevenueSharingCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            ontxtRevenueSharingCodeChanged($(this).val());
        });

        function ontxtRevenueSharingCodeChanged(value) {
            var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses Honor Dokter Berhasil Dibuat dengan Nomor <b>' + retval + '</b>', function () {
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var result = '';
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedMemberRemarksDt = $('#<%=hdnSelectedMemberRemarksDt.ClientID %>').val().split(',');
            $('.grdRevenueSharing .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemID = $(this).closest('tr').find('.keyItemID').html();
                    var remarksDt = $(this).closest('tr').find('.txtRemarksDt').val();
                    var uniqueKey = key + "|" + itemID;
                    if (lstSelectedMember.indexOf(uniqueKey) < 0) {
                        lstSelectedMember.push(uniqueKey);
                        lstSelectedMemberRemarksDt.push((key.trim() + "|" + remarksDt.trim()));
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var itemID = $(this).closest('tr').find('.keyItemID').html();
                    var remarksDt = $(this).closest('tr').find('.txtRemarksDt').val();
                    var uniqueKey = key + "|" + itemID;
                    if (lstSelectedMember.indexOf(uniqueKey) > -1) {
                        lstSelectedMember.splice(lstSelectedMember.indexOf(uniqueKey), 1);
                        lstSelectedMemberRemarksDt.splice(lstSelectedMemberRemarksDt.indexOf((key.trim() + "|" + remarksDt.trim())), 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberRemarksDt.ClientID %>').val(lstSelectedMemberRemarksDt.join(','));
        }

        $('.grdRevenueSharing .chkIsSelected input').live('click', function () {
            $('.chkCheckAll input').prop('checked', false);
            calculateTotal();
        });

        $('.lblRevenueSharingCode').die('click');
        $('.lblRevenueSharingCode').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();
            var dtID = $tr.find('.DtID').val();
            var dtIDSource = $tr.find('.DtIDSource').val();
            var paramedicID = $tr.find('.ParamedicID').val();
            var itemID = $tr.find('.ItemID').val();
            var param = id + "|" + paramedicID + "|" + itemID + "|" + dtIDSource + "|" + dtID;
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingProcess/ChargesEditRevenueSharingCtl.ascx");
            openUserControlPopup(url, param, 'Ubah Kode Jasa Medis', 800, 300);
        });

        $('.lblParamedicCode').die('click');
        $('.lblParamedicCode').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();
            var dtID = $tr.find('.DtID').val();
            var dtIDSource = $tr.find('.DtIDSource').val();
            var paramedicID = $tr.find('.ParamedicID').val();
            var param = id + "|" + paramedicID + "|" + dtIDSource + "|" + dtID;
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingProcess/ChargesEditParamedicIDCtl.ascx");
            openUserControlPopup(url, param, 'Ubah Dokter', 800, 300);
        });

        function onAfterSaveAddRecordEntryPopup() {
            getCheckedMember();
            cbpView.PerformCallback('refresh');
            calculateTotal();
        }

        function onAfterSaveEditRecordEntryPopup() {
            getCheckedMember();
            cbpView.PerformCallback('refresh');
            calculateTotal();
        }

        $('#chkCheckAll').live('click', function () {
            var isChecked = $(this).is(':checked');
            $('.grdRevenueSharing .chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
            calculateTotal();
        });

        function calculateTotal() {
            var countData = 0;
            var totalBruto = 0;
            var totalSelectedBruto = 0;
            var totalAmount = 0;
            var totalSelectedAmount = 0;

            $('.grdRevenueSharing .chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                totalSelectedBruto += parseFloat($tr.find('.TransactionAmount').val());
                totalSelectedAmount += parseFloat($tr.find('.SharingAmount').val());
            });

            $('.grdRevenueSharing .chkIsSelected').each(function () {
                $tr = $(this).closest('tr');
                countData += 1;
                totalBruto += parseFloat($tr.find('.TransactionAmount').val());
                totalAmount += parseFloat($tr.find('.SharingAmount').val());
            });

            $('#<%=txtCountData.ClientID %>').val(countData).trigger('changeValue');
            $('#<%=txtBrutoAmount.ClientID %>').val(totalBruto).trigger('changeValue');
            $('#<%=txtBrutoAmountSelected.ClientID %>').val(totalSelectedBruto).trigger('changeValue');
            $('#<%=txtTotalAmount.ClientID %>').val(totalAmount).trigger('changeValue');
            $('#<%=txtTotalAmountSelected.ClientID %>').val(totalSelectedAmount).trigger('changeValue');
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                getCheckedMember();
            }
            calculateTotal();
            hideLoadingPanel();
        }
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var ParamedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (code == "prepareRevenueSharing" || code == "changeParamedicMaster") {
                if (ParamedicID != "" && ParamedicID != null && ParamedicID != "0") {
                    return ParamedicID;
                }
            }
        }
    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamDepartmentID" runat="server" />
        <input type="hidden" id="hdnSelectedMember" value="" runat="server" />
        <input type="hidden" id="hdnSelectedMemberRemarksDt" value="" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnIsAllowChangeFilterPaidType" runat="server" />
        <input type="hidden" id="hdnIsFilterBPJSStatus" runat="server" />
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 100px" />
                            <col style="width: 350px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Tanggal Proses Jasa Medis")%></b></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox runat="server" Width="120px" ID="txtRevenueSharingDate" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Department Transaksi") %></b></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="50%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Alokasi Pajak/Cara Bayar")%></b></label>
                            </td>
                            <td colspan="3">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboReduction" ClientInstanceName="cboReduction" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="100%"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Grup Pelayanan") %></b></label>
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkIsFilterClinicGroup" Text=" Is Filter?" />
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboClinicGroup" ClientInstanceName="cboClinicGroup" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Periode")%></b></label>
                            </td>
                            <td colspan="3">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboPeriodeType" ClientInstanceName="cboPeriodeType" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                                    </td>
                                                    <td style="width: 30px; text-align: center">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("s/d")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jam Transaksi")%></label>
                            </td>
                            <td colspan="3">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" Width="80px" ID="txtPeriodeTimeFrom" Text="00:00" Style="text-align: center" />
                                        </td>
                                        <td style="width: 30px; text-align: center">
                                            <label class="lblNormal">
                                                <%=GetLabel("s/d")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="80px" ID="txtPeriodeTimeTo" Text="23:59" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblPayer">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPayerID" runat="server" value="" />
                                <asp:TextBox ID="txtPayerCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayerName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkIsFilterPayerExclusion" Text=" Is Exclusion?" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblRevenueSharing">
                                    <%=GetLabel("Jasa Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                                <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Tampilan Lunas")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboPaidType" ClientInstanceName="cboPaidType" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Tampilan Closed")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboRegistrationStatusFilter" ClientInstanceName="cboRegistrationStatusFilter" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trBPJSFilter" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Tampilan BPJS")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboBPJSStatusFilter" ClientInstanceName="cboBPJSStatusFilter" Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <u>
                                        <%=GetLabel("Urut Bedasarkan")%></u></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboSortBy" ClientInstanceName="cboSortBy" Width="50%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Jumlah Data") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtCountData" CssClass="number" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Nilai Bruto") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtBrutoAmount" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Nilai Bruto Dipilih") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtBrutoAmountSelected" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Nilai JasMed") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtTotalAmount" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                    <%=GetLabel("Total Nilai JasMed Dipilih") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtTotalAmountSelected" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px;">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="position: relative;" id="dView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 400px;
                                        overflow-y: auto">
                                        <table class="grdRevenueSharing grdSelected" id="grdRevenueSharing" cellspacing="0"
                                            width="100%" rules="all">
                                            <tr>
                                                <th class="keyField" rowspan="2">
                                                    &nbsp;
                                                </th>
                                                <th rowspan="2" style="width: 30px">
                                                    <input type="checkbox" id="chkCheckAll" />
                                                </th>
                                                <th rowspan="2" style="width: 250px;" align="left">
                                                    <%=GetLabel("Info Registrasi")%>
                                                </th>
                                                <th rowspan="2" style="width: 250px" align="left">
                                                    <%=GetLabel("Info Transaksi")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px">
                                                    <%=GetLabel("Tanggal/Jam Transaksi")%>
                                                </th>
                                                <th rowspan="2" style="width: 50px" align="center">
                                                    <%=GetLabel("CITO")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Jumlah Transaksi (Bruto)")%>
                                                </th>
                                                <th rowspan="2" style="width: 70px" align="right">
                                                    <%=GetLabel("Kartu Kredit")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Komponen Pendapatan")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Dokter") %>
                                                </th>
                                                <th rowspan="2" style="width: 70px">
                                                    <%=GetLabel("Jasa Medis") %>
                                                </th>
                                                <th rowspan="2" style="width: 70px">
                                                    <%=GetLabel("Kode Dokter") %>
                                                </th>
                                                <th rowspan="2" style="width: 70px">
                                                    <%=GetLabel("Catatan") %>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 80px" align="right">
                                                    <%=GetLabel("Rumah Sakit") %>
                                                </th>
                                                <th style="width: 80px" align="right">
                                                    <%=GetLabel("Instrument/Alat") %>
                                                </th>
                                                <th style="width: 80px" align="right">
                                                    <%=GetLabel("Rujukan") %>
                                                </th>
                                            </tr>
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr <%#: Eval("GCTransactionStatus").ToString() != GetClosedTransactionStatus() ? "class='trClosed'" : ""%>>
                                                        <td class="keyField">
                                                            <%#:Eval("ID") %>
                                                        </td>
                                                        <td class="keyItemID" style="display:none">
                                                            <%#:Eval("ItemID") %>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                                                            <input type="hidden" class="ID" id="ID" runat="server" value='<%#: Eval("ID")%>' />
                                                            <input type="hidden" class="DtID" id="DtID" runat="server" value='<%#: Eval("DtID")%>' />
                                                            <input type="hidden" class="DtIDSource" id="DtIDSource" runat="server" value='<%#: Eval("DtIDSource")%>' />
                                                            <input type="hidden" class="ParamedicID" id="ParamedicID" runat="server" value='<%#: Eval("ParamedicID")%>' />
                                                            <input type="hidden" class="ItemID" id="ItemID" runat="server" value='<%#: Eval("ItemID")%>' />
                                                            <input type="hidden" class="TransactionAmount" value="<%#: Eval("TransactionAmount")%>" />
                                                            <input type="hidden" class="SharingAmount" value="<%#: Eval("SharingAmount")%>" />
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <label class="lblNormal">
                                                                    <%#: Eval("RegistrationNo") %><%#: Eval("cfLinkedToRegInfo") %></label></b>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("Tgl Reg : ")%></label><%#: Eval("RegistrationDateInString") %>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("No RM : ")%></label><%#: Eval("MedicalNo") %>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("Nama Pasien : ")%></label><%#: Eval("PatientName") %>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("Pembayar : ")%></label><%#: Eval("BusinessPartnerName") %>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <label class="lblNormal">
                                                                    <%#: Eval("TransactionNo") %></label></b>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("Kode Item : ")%></label><%#: Eval("ItemCode") %><%=GetLabel(" || ")%><%#: Eval("cfOldItemCode") %>
                                                            <br />
                                                            <label class="lblNormal" style="font-size: smaller; font-style: italic">
                                                                <%=GetLabel("Nama Item : ")%></label><%#: Eval("ItemName1") %>
                                                        </td>
                                                        <td align="center">
                                                            <%#: Eval("TransactionDateDayInString") %>
                                                            <br />
                                                            <%#: Eval("TransactionTime") %>
                                                        </td>
                                                        <td align="center">
                                                            <%#:Eval("cfIsCITOValue") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("TransactionAmount", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("CCFee", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("ComponentSharingAmount1", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("ComponentSharingAmount2", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("ComponentSharingAmount3", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <label id="lblSharingAmount" class="lblSharingAmount">
                                                                <%#:Eval("SharingAmount","{0:N2}")%></label>
                                                        </td>
                                                        <td align="center">
                                                            <label class="lblLink lblRevenueSharingCode" title="<%#:Eval("RevenueSharingName") %>">
                                                                <%#: Eval("RevenueSharingCode") != "" ? Eval("RevenueSharingCode") : "Pilih Jasa Medis" %></label>
                                                        </td>
                                                        <td align="center">
                                                            <label class="lblLink lblParamedicCode" title="<%#:Eval("ParamedicCode") %>">
                                                                <%#: Eval("ParamedicCode") %></label>
                                                        </td>
                                                        <td align="center">
                                                            <input type="text" class="txtRemarksDt" style="width: 100%;" />
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
    </div>
</asp:Content>
