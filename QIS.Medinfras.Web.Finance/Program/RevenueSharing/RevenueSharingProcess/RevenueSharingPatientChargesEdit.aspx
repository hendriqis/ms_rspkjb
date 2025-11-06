<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingPatientChargesEdit.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingPatientChargesEdit" %>

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
    <li id="btnProcessEditRevenueCharges" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        }

        $(function () {
            getCheckedMember();
        });

        $('.lnkTransactionNo a').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingProcess/RevenueSharingEntryCtl.ascx");
            openUserControlPopup(url, id, 'Transaction Detail', 1100, 450);
        });

        $('#btnRefresh').live('click', function () {
            getCheckedMember();
            cbpView.PerformCallback('refresh');
            calculateTotal();
        });

        $('#<%=btnProcessEditRevenueCharges.ClientID %>').live('click', function () {
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
            var revid = $('#<%=hdnRevenueSharingID.ClientID %>');
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    if (revid != '' || revid != null) {

                        $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                    }
                    else {
                        $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                    }
                }
                else {

                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Revenue Sharing UPDATE TO
        $('#lblRevenueSharingUpdateTo.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', onGetRevenueSharingFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingUpdateToCode.ClientID %>').val(value);
                ontxtRevenueSharingUpdateToCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingUpdateToCode.ClientID %>').live('change', function () {
            ontxtRevenueSharingUpdateToCodeChanged($(this).val());
        });

        function ontxtRevenueSharingUpdateToCodeChanged(value) {
            var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingUpdateToID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingUpdateToName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingUpdateToID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingUpdateToCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingUpdateToName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Process Success', 'Proses Ubah Jasa Medis per Transaksi Berhasil.', function () {
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var result = '';
            $('#<%=hdnSelectedMemberRevenueSharingID.ClientID %>').val('');
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedMemberRevenueSharingID = $('#<%=hdnSelectedMemberRevenueSharingID.ClientID %>').val().split(',');
            $('.grdRevenueSharing .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html().trim();
                    var revenueSharingID = $(this).closest('tr').find('.hdnGridRevenueSharingID').val().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberRevenueSharingID.push(revenueSharingID);
                    }
                    else {
                        lstSelectedMemberRevenueSharingID[idx] = revenueSharingID;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html().trim();
                    var revenueSharingID = $(this).closest('tr').find('.hdnGridRevenueSharingID').val().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberRevenueSharingID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberRevenueSharingID.ClientID %>').val(lstSelectedMemberRevenueSharingID.join(','));
        }

        $('.grdRevenueSharing .chkIsSelected input').live('click', function () {
            $('.chkCheckAll input').prop('checked', false);
            calculateTotal();
        });

        //#region Revenue Sharing per Detail

        $td = null;

        $('.lblRevenueSharingCode').die('click');
        $('.lblRevenueSharingCode').live('click', function () {
            $td = $(this).parent();
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();

            var filterExpression = 'IsDeleted = 0';
            openSearchDialog('revenuesharing', filterExpression, function (value) {
                onlblRevenueSharingCodeChanged(value);
            });

        });

        function onlblRevenueSharingCodeChanged(value) {
            var filterExpression = "RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null || result != '') {
                    $td.find('.hdnGridRevenueSharingID').val(result.RevenueSharingID);
                    $td.find('.lblRevenueSharingCode').html(result.RevenueSharingCode);
                }
                else {
                    $td.find('.hdnGridRevenueSharingID').val();
                    $td.find('.lblRevenueSharingCode').val('');
                }
            });
        }

        //#endregion

        $('#chkCheckAll').live('click', function () {
            var isChecked = $(this).is(':checked');
            $('.grdRevenueSharing .chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
            calculateTotal();
        });

        function calculateTotal() {
            var countData = 0;
            var totalAmount = 0;
            var totalSelectedAmount = 0;

            $('.grdRevenueSharing .chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                totalSelectedAmount += parseFloat($tr.find('.SharingAmount').val());
            });

            $('.grdRevenueSharing .chkIsSelected').each(function () {
                $tr = $(this).closest('tr');
                countData += 1;
                totalAmount += parseFloat($tr.find('.SharingAmount').val());
            });

            $('#<%=txtCountData.ClientID %>').val(countData).trigger('changeValue');
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

    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamDepartmentID" runat="server" />
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <input type="hidden" id="hdnGridRevenueSharingID" runat="server" />
        <input type="hidden" id="hdnSelectedMemberRevenueSharingID" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
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
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Department Transaksi") %></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="50%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Grup Pelayanan") %></label>
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
                                    <%=GetLabel("Periode")%></label>
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
                                                        s/d
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
                                            s/d
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
                                <asp:CheckBox runat="server" ID="chkIsFilter" Text="Is Exclusion?" />
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
                                    <%=GetLabel("Status Tampilan")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboPaidType" ClientInstanceName="cboPaidType" Width="50%" runat="server" />
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
                        <tr>
                            <td colspan="2" style="font-size: large; color: Red; font-style: italic; font-weight: bold">
                                <%=GetLabel("Perhatian")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="font-size: small; color: Red;">
                                <%=GetLabel("Jika isian jasa medis ini terisi, maka semua yang sudah dipilih per detail di list table, akan diabaikan.")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblRevenueSharingUpdateTo">
                                    <%=GetLabel("Ubah Jasa Medis Ke")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRevenueSharingUpdateToID" runat="server" value="" />
                                <asp:TextBox ID="txtRevenueSharingUpdateToCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRevenueSharingUpdateToName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" valign="top">
                    <table class="tblEntryContent" style="width: 100%">
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jumlah Data") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtCountData" CssClass="number" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Jasa Medis") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtTotalAmount" CssClass="txtCurrency"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Jasa Medis Dipilih") %></label>
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
                                                <th rowspan="2" style="width: 120px;" align="left">
                                                    <%=GetLabel("Info Registrasi")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px" align="left">
                                                    <%=GetLabel("Nomor Transaksi")%>
                                                </th>
                                                <th rowspan="2" style="width: 100px">
                                                    <%=GetLabel("Tanggal/Jam Transaksi")%>
                                                </th>
                                                <th rowspan="2" align="left">
                                                    <%=GetLabel("Nama Pasien")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px" align="left">
                                                    <%=GetLabel("Pembayar")%>
                                                </th>
                                                <th rowspan="2" style="width: 50px" align="center">
                                                    <%=GetLabel("CITO")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Jumlah Transaksi (Bruto)")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Kartu Kredit")%>
                                                </th>
                                                <th colspan="3">
                                                    <%=GetLabel("Komponen Pendapatan")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Dokter") %>
                                                </th>
                                                <th rowspan="2" style="width: 120px">
                                                    <%=GetLabel("Jasa Medis") %>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 90px" align="right">
                                                    <%=GetLabel("Rumah Sakit") %>
                                                </th>
                                                <th style="width: 90px" align="right">
                                                    <%=GetLabel("Instrument/Alat") %>
                                                </th>
                                                <th style="width: 90px" align="right">
                                                    <%=GetLabel("Rujukan") %>
                                                </th>
                                            </tr>
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr <%#: Eval("GCTransactionStatus").ToString() != GetClosedTransactionStatus() ? "class='trClosed'" : ""%>>
                                                        <td class="keyField">
                                                            <%#:Eval("ID") %>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                                                            <input type="hidden" class="ID" id="ID" runat="server" value='<%#: Eval("ID")%>' />
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <%#:Eval("RegistrationNo") %>
                                                            </div>
                                                        </td>
                                                        <td class="lnkTransactionNo">
                                                            <a>
                                                                <%#:Eval("TransactionNo")%></a>
                                                        </td>
                                                        <td align="center">
                                                            <%#:Eval("TransactionDateInString") %>
                                                            <br />
                                                            <%#:Eval("TransactionTime") %>
                                                        </td>
                                                        <td>
                                                            <%#:Eval("PatientName") %>
                                                        </td>
                                                        <td>
                                                            <%#:Eval("BusinessPartnerName") %>
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
                                                            <input type="hidden" class="SharingAmount" value="<%#: Eval("SharingAmount")%>" />
                                                            <input type="hidden" class="hdnGridRevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                            <label class="lblLink lblRevenueSharingCode" title="<%#:Eval("RevenueSharingName") %>">
                                                                <%#: Eval("RevenueSharingCode") %></label>
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
