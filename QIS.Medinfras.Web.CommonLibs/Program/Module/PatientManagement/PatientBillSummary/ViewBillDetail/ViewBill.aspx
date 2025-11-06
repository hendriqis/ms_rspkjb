<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ViewBill.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewBill" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
    <li id="btnProcessGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Review") %></div>
    </li>
    <li id="btnVoidGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cancel") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtFilterTransactionDateFrom.ClientID %>');
            $('#<%=txtFilterTransactionDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFilterTransactionDateFrom.ClientID %>').live('change', function (evt) {
                var transDateFrom = $('#<%:txtFilterTransactionDateFrom.ClientID %>').val();
                var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
                var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

                var transDateTo = $('#<%:txtFilterTransactionDateTo.ClientID %>').val();
                var transactionDateTo = Methods.getDatePickerDate(transDateTo);
                var transDateYMDTo = dateToDMYCustom(transactionDateTo);

                if (transDateYMDFrom > transDateYMDTo) {
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                }
            });

            setDatePicker('<%=txtFilterTransactionDateTo.ClientID %>');
            $('#<%=txtFilterTransactionDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFilterTransactionDateTo.ClientID %>').live('change', function (evt) {
                var transDateFrom = $('#<%:txtFilterTransactionDateFrom.ClientID %>').val();
                var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
                var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

                var transDateTo = $('#<%:txtFilterTransactionDateTo.ClientID %>').val();
                var transactionDateTo = Methods.getDatePickerDate(transDateTo);
                var transDateYMDTo = dateToDMYCustom(transactionDateTo);

                if (transDateYMDFrom > transDateYMDTo) {
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                }
            });

            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        }

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback();
        });

        $('#<%=btnProcessGenerateBill.ClientID %>').live('click', function () {
            $('#<%=hdnVerifyCancel.ClientID %>').val('verify');
            callCustomButton();
        });

        $('#<%=btnVoidGenerateBill.ClientID %>').live('click', function () {
            $('#<%=hdnVerifyCancel.ClientID %>').val('cancel');
            callCustomButton();

        });

        function callCustomButton() {
            var lstSelectedValue = "";
            $('.chkIsSelected input:checked').each(function () {
                var $td = $(this).parent().parent();
                var input = $td.find('.hdnKeyField').val();
                if (lstSelectedValue != "") lstSelectedValue += ",";
                lstSelectedValue += input;
            });
            $('#<%=hdnLstSelectedValue.ClientID %>').val(lstSelectedValue);
            if (lstSelectedValue == "")
                showToast('Warning', 'Pilih transaksi yang ingin di verifikasi');
            else
                onCustomButtonClick('generatebill');
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        $('#<%=rblFilterDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == 'true') {
                $('#trDate').css('display', '');
            }
            else {
                $('#trDate').css('display', 'none');
            }
////            cbpView.PerformCallback();
        });

        function onCboServiceUnitChanged() {
////            cbpView.PerformCallback();
        }

        $('.chkSelectAll input').live('change', function () {
            var value = $(this).prop("checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop("checked", value);
            });
        });

        $('.lnkNursingNote').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.TransactionID').val();
            var visitID = $tr.find('.VisitID').val();
            var url = '~/Libs/Program/Module/PatientManagement/Information/ViewChargesNursingNotesCtl.ascx';
            openUserControlPopup(url, visitID + '|' + id, 'Catatan Perawat untuk Transaksi', 1100, 500);
        });

        function dateToDMYCustom(date) {
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.TransactionID').val();

            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();

            var healthcareServiceUnitID = cboServiceUnit.GetValue();

            var filterDate = $('#<%=rblFilterDate.ClientID %>').find('input:checked').val();

            var transDateFrom = $('#<%:txtFilterTransactionDateFrom.ClientID %>').val();
            var transactionDateFrom = Methods.getDatePickerDate(transDateFrom);
            var transDateYMDFrom = dateToDMYCustom(transactionDateFrom);

            var transDateTo = $('#<%:txtFilterTransactionDateTo.ClientID %>').val();
            var transactionDateTo = Methods.getDatePickerDate(transDateTo);
            var transDateYMDTo = dateToDMYCustom(transactionDateTo);
            
            if (registrationID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
            else if (transDateFrom == '' || transDateTo == '') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else if (code == 'PM-00220') {
                filterExpression.text = id;
                return true;
            }
            else if (code == 'PM-00621') {
                var filterRincianBiaya = "RegistrationID = " + registrationID;

                if (healthcareServiceUnitID != "0") {
                    filterRincianBiaya += " AND HealthcareServiceUnitID = " + healthcareServiceUnitID;
                }

                if (filterDate == "true") {
                    filterRincianBiaya += " AND TransactionDate BETWEEN '" + transDateYMDFrom + "' AND '" + transDateYMDTo + "'";
                }

                filterExpression.text = filterRincianBiaya;
                return true;
            }
            else {
                filterExpression.text = registrationID + '|' + transDateYMDFrom + ';' + transDateYMDTo;
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnLinkedRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnLstSelectedValue" runat="server" value="" />
    <input type="hidden" id="hdnVerifyCancel" runat="server" value="" />
    <input type="hidden" id="hdnIntervalFilterDate" runat="server" value="" />
    <input type="hidden" id="hdnFilterParameter" runat="server" value="" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col width="200px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Unit Pelayanan") %></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="250px"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Filter Tanggal") %></label>
            </td>
            <td>
                <asp:RadioButtonList runat="server" ID="rblFilterDate" RepeatDirection="Horizontal">
                    <asp:ListItem Text="On" Value="true" Selected="True" />
                    <asp:ListItem Text="Off" Value="false" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="trDate">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal") %></label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtFilterTransactionDateFrom" Width="110px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel(" s/d ") %></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilterTransactionDateTo" Width="110px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <tr>
            <td>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(s); onLoad();} " />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <div class="containerUlTabPage">
                                    <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                        <li class="selected" contentid="containerService">
                                            <%=GetLabel("Pelayanan") %>
                                        </li>
                                        <li contentid="containerDrugMS">
                                            <%=GetLabel("Obat & Alkes") %>
                                        </li>
                                        <li contentid="containerLogistics">
                                            <%=GetLabel("Barang Umum") %>
                                        </li>
                                    </ul>
                                </div>
                                <div id="containerService" class="containerBillSummaryDetailAll">
                                    <asp:ListView ID="lvwService" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 40px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Jumlah")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Harga")%>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Created By")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Verified")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 70px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("CITO")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("No Data To Display") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 80px">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 40px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Jumlah")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Harga")%>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Created By")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Verified")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 70px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("CITO")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr id="Tr1" class="trFooter" runat="server">
                                                    <td colspan="7" align="right" style="padding-right: 3px">
                                                        <%=GetLabel("TOTAL") %>
                                                    </td>
                                                    <td align="right" style="padding-right: 9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer"
                                                        runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient"
                                                        runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 9px" id="tdServiceTotal" class="tdServiceTotal"
                                                        runat="server">
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="TransactionID" value="<%#: Eval("TransactionID")%>" />
                                                        <input type="hidden" class="VisitID" value="<%#: Eval("VisitID")%>" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                        <div>
                                                            <span>
                                                                <%#: Eval("ParamedicName")%></span>
                                                        </div>
                                                        <div>
                                                            <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                            <span style="color: Maroon">
                                                                <%#: Eval("TransactionNo")%></span>
                                                        </div>
                                                        <div>
                                                            <a class="lnkNursingNote" style='<%# Eval("IsLinkedToNursingNote").ToString() == "False" ? "display:none;": "" %> max-width:20px;
                                                                min-width: 20px;'>
                                                                <%=GetLabel("Catatan Perawat") %></a></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px;">
                                                        <div>
                                                            <%# Eval("TransactionDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div style="padding: 3px; text-align: right;">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="text-align: left; font-style: italic">
                                                                        Tr
                                                                    </td>
                                                                    <td>
                                                                        <%#: Eval("Tariff", "{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; font-style: italic">
                                                                        C1
                                                                    </td>
                                                                    <td style="padding-left: 2px; text-align: right; font-style: italic">
                                                                        <%#: Eval("TariffComp1", "{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; font-style: italic">
                                                                        C2
                                                                    </td>
                                                                    <td style="padding-left: 2px; text-align: right; font-style: italic">
                                                                        <%#: Eval("TariffComp2", "{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; font-style: italic">
                                                                        C3
                                                                    </td>
                                                                    <td style="padding-left: 2px; text-align: right; font-style: italic">
                                                                        <%#: Eval("TariffComp3", "{0:N}")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("ChargedQuantity")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("CITOAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("LineAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("CreatedByUserName")%></div>
                                                        <div>
                                                            <%#: Eval("CreatedDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsVerified" runat="server" Checked='<%# Eval("IsVerified")%>'
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                                <div id="containerDrugMS" style="display: none" class="containerBillSummaryDetailAll">
                                    <asp:ListView ID="lvwDrugMS" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("HNA+PPn")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 60px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Margin")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Created By")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Dibebankan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="17">
                                                        <%=GetLabel("No Data To Display") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("HNA+PPn")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 60px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Margin")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Created By")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Dibebankan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr id="Tr2" class="trFooter" runat="server">
                                                    <td colspan="8" align="right" style="padding-right: 3px">
                                                        <%=GetLabel("Total") %>
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdDrugMSTotalPayer" runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdDrugMSTotalPatient" runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdDrugMSTotal" runat="server">
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                        <div>
                                                            <span>
                                                                <%#: Eval("ParamedicName")%></span>
                                                        </div>
                                                        <div>
                                                            <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                            <span style="color: Maroon">
                                                                <%#: Eval("TransactionNo")%></span>
                                                        </div>
                                                        <div>
                                                            <a class="lnkNursingNote" style='<%# Eval("IsLinkedToNursingNote").ToString() == "False" ? "display:none;": "" %> max-width:20px;
                                                                min-width: 20px;'>
                                                                <%=GetLabel("Catatan Perawat") %></a></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px;">
                                                        <div>
                                                            <%# Eval("TransactionDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("Tariff", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("cfHNA_PPn", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("cfMarkupMargin", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("ChargedQuantity")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px;">
                                                        <div>
                                                            <%#: Eval("ItemUnit")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("LineAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("CreatedByUserName")%></div>
                                                        <div>
                                                            <%#: Eval("CreatedDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                                <div id="containerLogistics" style="display: none" class="containerBillSummaryDetailAll">
                                    <asp:ListView ID="lvwLogistic" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Created By")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Dibebankan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="17">
                                                        <%=GetLabel("No Data To Display") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" rowspan="2">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Deskripsi")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div style="text-align: left; padding-left: 3px">
                                                            <%=GetLabel("Tanggal Transaksi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px" rowspan="2">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="2" align="center">
                                                        <%=GetLabel("Jumlah")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Harga")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 55px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Diskon")%>
                                                        </div>
                                                    </th>
                                                    <th colspan="3" align="center">
                                                        <%=GetLabel("Total")%>
                                                    </th>
                                                    <th rowspan="2" style="width: 90px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Dientri Oleh")%>
                                                        </div>
                                                    </th>
                                                    <th rowspan="2" style="width: 50px">
                                                        <div style="text-align: center;">
                                                            <%=GetLabel("Reviewed")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Dibebankan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 50px">
                                                        <div style="text-align: center; padding-right: 3px">
                                                            <%=GetLabel("Satuan")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Instansi")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Pasien")%>
                                                        </div>
                                                    </th>
                                                    <th style="width: 80px">
                                                        <div style="text-align: right; padding-right: 3px">
                                                            <%=GetLabel("Total")%>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr id="Tr2" class="trFooter" runat="server">
                                                    <td colspan="8" align="right" style="padding-right: 3px">
                                                        <%=GetLabel("Total") %>
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdLogisticTotalPayer" runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdLogisticTotalPatient" runat="server">
                                                    </td>
                                                    <td align="right" style="padding-right: 3px" id="tdLogisticTotal" runat="server">
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                        <div>
                                                            <span>
                                                                <%#: Eval("ParamedicName")%></span>
                                                        </div>
                                                        <div>
                                                            <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                            <span style="color: Maroon">
                                                                <%#: Eval("TransactionNo")%></span>
                                                        </div>
                                                        <div>
                                                            <a class="lnkNursingNote" style='<%# Eval("IsLinkedToNursingNote").ToString() == "False" ? "display:none;": "" %> max-width:20px;
                                                                min-width: 20px;'>
                                                                <%=GetLabel("Catatan Perawat") %></a></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px;">
                                                        <div>
                                                            <%# Eval("TransactionDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("Tariff", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("ChargedQuantity")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px;">
                                                        <div>
                                                            <%#: Eval("ItemUnit")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("LineAmount", "{0:N}")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: right;">
                                                        <div>
                                                            <%#: Eval("CreatedByUserName")%></div>
                                                        <div>
                                                            <%#: Eval("CreatedDateTimeInString")%></div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                            Enabled="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</asp:Content>
