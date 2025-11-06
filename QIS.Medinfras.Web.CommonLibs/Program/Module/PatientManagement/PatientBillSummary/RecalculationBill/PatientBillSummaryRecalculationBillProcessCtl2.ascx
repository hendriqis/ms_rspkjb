<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryRecalculationBillProcessCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryRecalculationBillProcessCtl2" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<script type="text/javascript" id="dxss_patientrecalculate1ctl">
    $(function () {
        setDatePicker('<%=txtTransactionDateFrom.ClientID %>');
        setDatePicker('<%=txtTransactionDateTo.ClientID %>');

        var param = '<li id="btnRecalculate"><img src="<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>" alt="" /><div>Recalculate</div></li>';
        addToolbarButton(param);

        onLoadRecalculation();
    });

    function onLoadRecalculation() {
        var lstParamSelected = $('#<%=hdnParam.ClientID %>').val().split('|');
        $('.chkIsSelectedCtl').each(function () {
            var $td = $(this).parent().parent();
            var input = $td.find('.hdnKeyField').val();
            var idx = lstParamSelected.indexOf(input);
            if (idx < 0) {
                $(this).find('input').prop('checked', false);
            }
            else {
                $(this).find('input').prop('checked', true);            
            }
        });

        $('.chkSelectAllCtl input').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelectedCtl').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        $('#<%=chkIsUsedLastHNA.ClientID %>').live('change', function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=chkIsResetItemTariff.ClientID %>').prop("checked", true);
                $('#<%=chkIsResetItemTariff.ClientID %>').attr("disabled", true);
            } else {
                $('#<%=chkIsResetItemTariff.ClientID %>').prop("checked", false);
                $('#<%=chkIsResetItemTariff.ClientID %>').removeAttr("disabled");
            }
        });
    }

    function onCboRecalculateTypeChanged() {
        var type = cboRecalculateType.GetValue();
        if (type == "0") {
            $('#<%:trRecalByTransactionDate.ClientID %>').removeAttr('style');
            $('#<%:trRecalByChargeClass.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByItemType.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByClass.ClientID %>').attr('style', 'display:none');
        } else if (type == "1") {
            $('#<%:trRecalByTransactionDate.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByChargeClass.ClientID %>').removeAttr('style');
            $('#<%:trRecalByServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByItemType.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByClass.ClientID %>').attr('style', 'display:none');
        } else if (type == "2") {
            $('#<%:trRecalByTransactionDate.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByChargeClass.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByServiceUnit.ClientID %>').removeAttr('style');
            $('#<%:trRecalByItemType.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByClass.ClientID %>').attr('style', 'display:none');
        } else if (type == "3") {
            $('#<%:trRecalByTransactionDate.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByChargeClass.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByItemType.ClientID %>').removeAttr('style');
            $('#<%:trRecalByClass.ClientID %>').attr('style', 'display:none');
        } else if (type == "4") {
            $('#<%:trRecalByTransactionDate.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByChargeClass.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByItemType.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByClass.ClientID %>').removeAttr('style');
        } else {
            $('#<%:trRecalByTransactionDate.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByChargeClass.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByServiceUnit.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByItemType.ClientID %>').attr('style', 'display:none');
            $('#<%:trRecalByClass.ClientID %>').attr('style', 'display:none');
        }
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    }

    function onBeforeSaveRecord(errMessage) {
        var param = '';
        $('.chkIsSelectedCtl input:checked').each(function () {
            var $td = $(this).parent().parent();
            var input = $td.find('.hdnKeyField').val();
            if (param != "") {
                param += "|";
            }
            param += input;
        });
        if (param != "") {
            $('#<%=hdnParam.ClientID %>').val(param);
            return true;
        } 
        else {
            errMessage.text = 'Pilih Item Terlebih Dahulu';
            return false;
        }
    }

    $('#<%:txtTransactionDateFrom.ClientID %>').live('change', function () {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    });

    $('#<%:txtTransactionDateTo.ClientID %>').live('change', function () {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    });

    function onCboChargesClassChanged() {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    }

    function onCboToChargesClassChanged() {

        $('#<%:hdnToClassID.ClientID %>').val(cboToChargesClass.GetValue());
        $('#<%:hdnToClassName.ClientID %>').val(cboToChargesClass.GetText());

        if (cboToChargesClass.GetValue() != "" && cboToChargesClass.GetValue() != "0") {
            $('#<%=chkIsUsedLastHNA.ClientID %>').prop("checked", true);
            $('#<%=chkIsUsedLastHNA.ClientID %>').attr("disabled", true);

            $('#<%=chkIsResetItemTariff.ClientID %>').prop("checked", true);
            $('#<%=chkIsResetItemTariff.ClientID %>').attr("disabled", true);
        } else {
            $('#<%=chkIsUsedLastHNA.ClientID %>').prop("checked", false);
            $('#<%=chkIsUsedLastHNA.ClientID %>').removeAttr("disabled");

            $('#<%=chkIsResetItemTariff.ClientID %>').prop("checked", false);
            $('#<%=chkIsResetItemTariff.ClientID %>').removeAttr("disabled");
        }
    }

    function onCboServiceUnitChanged() {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    }

    function onCboItemTypeChanged() {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    }

    function onCboClassChanged() {
        cbpRecalculationPatientBillProcess.PerformCallback('refresh|');
    }

    $('#btnRecalculate').live('click', function () {
        var isIncludeVariableTariff = $('#<%=chkIsIncludeVariableTariff.ClientID %>').is(":checked");
        if (isIncludeVariableTariff) {
            showToastConfirmation('Proses Rekalkulasi Hitung Ulang "Tariff Variable" ?', function (resultVariable) {
                if (resultVariable) {
                    showToastConfirmation('Proses Rekalkulasi Sekarang?', function (result) {
                        if (result) {
                            var param = '';
                            $('.chkIsSelectedCtl input:checked').each(function () {
                                var $td = $(this).parent().parent();
                                var input = $td.find('.hdnKeyField').val();
                                if (param != "") {
                                    param += "|";
                                }
                                param += input;
                            });
                            if (param != "") {
                                $('#<%=hdnParam.ClientID %>').val(param);
                                cbpRecalculationPatientBillProcess.PerformCallback('recal|');
                            } else {
                                showToast('Warning', 'Pilih transaksi terlebih dahulu.');
                            }
                        }
                    });
                }
            });
        } else {
            showToastConfirmation('Proses Rekalkulasi Sekarang?', function (result) {
                if (result) {
                    var param = '';
                    $('.chkIsSelectedCtl input:checked').each(function () {
                        var $td = $(this).parent().parent();
                        var input = $td.find('.hdnKeyField').val();
                        if (param != "") {
                            param += "|";
                        }
                        param += input;
                    });
                    if (param != "") {
                        $('#<%=hdnParam.ClientID %>').val(param);
                        cbpRecalculationPatientBillProcess.PerformCallback('recal|');
                    } else {
                        showToast('Warning', 'Pilih transaksi terlebih dahulu.');
                    }
                }
            });
        }
    });

    function onCbpRecalculationPatientBillProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == "recal") {
            if (param[1] == "success") {
            } else {
                showToast('Warning', 'Tidak ada transaksi yang untuk rekalkulasi.');
            }
        }
        onLoadRecalculation();
        hideLoadingPanel();
    }
</script>
<style type="text/css">
    .containerRecalculationProcess
    {
        height: 280px;
        overflow-y: auto;
        border: 1px solid #EAEAEA;
    }
</style>
<input type="hidden" value="" id="hdnParam" runat="server" />
<div>
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnLinkedRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnListTempTransactionDtID" value="" />
    <input type="hidden" runat="server" id="hdnToClassID" value="" />
    <input type="hidden" runat="server" id="hdnToClassName" value="" />
    <div style="text-align: left; width: 100%; margin-bottom: 10px;">
        <table>
            <colgroup>
                <col style="width: 500px" />
                <col style="width: 500px" />
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <table style="border: 3px double black; margin: 3px">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <h4 style="text-align: center; font-size: medium">
                                    <%=GetLabel("Filter")%></h4>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("By")%>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboRecalculateType" ClientInstanceName="cboRecalculateType"
                                    runat="server" Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboRecalculateTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRecalByTransactionDate" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Transaction Date")%></label>
                            </td>
                            <td style="float: left">
                                <table>
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 30px" />
                                        <col style="width: 150px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtTransactionDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("s/d")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionDateTo" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trRecalByChargeClass" runat="server" style="display: none">
                            <td>
                                <%=GetLabel("Charge Class")%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboChargesClass" ClientInstanceName="cboChargesClass" runat="server"
                                    Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboChargesClassChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRecalByServiceUnit" runat="server" style="display: none">
                            <td>
                                <%=GetLabel("Service Unit")%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server"
                                    Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRecalByItemType" runat="server" style="display: none">
                            <td>
                                <%=GetLabel("Item Type")%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboItemType" ClientInstanceName="cboItemType" runat="server"
                                    Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboItemTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trRecalByClass" runat="server" style="display: none">
                            <td>
                                <%=GetLabel("Class")%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboClass" ClientInstanceName="cboClass" runat="server" Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboClassChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="tdRecalTo" runat="server" valign="top">
                    <table style="border: 3px double black; margin: 3px" id="tbCalculateTo" runat="server">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr id="trRecalToChargeClassCaption" runat="server">
                            <td colspan="2">
                                <h4 style="text-align: center; font-size: medium">
                                    <%=GetLabel("Calculate To")%></h4>
                            </td>
                        </tr>
                        <tr id="trRecalToChargeClass" runat="server">
                            <td>
                                <%=GetLabel("Charge Class")%>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboToChargesClass" ClientInstanceName="cboToChargesClass" runat="server"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboToChargesClassChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="color: Red">
                                <%=GetLabel("WARNING !")%>
                                <br />
                                <%=GetLabel("Ubah kelas akan berdampak hitung berdasarkan Buku Tariff.")%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="tdPackageInfo" colspan="2" runat="server" valign="top">
                    <table style="border: 3px double black; margin: 3px" id="tbPackageInfo" runat="server">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr id="tr1" runat="server">
                            <td colspan="2">
                                <h4 style="text-align: center; font-size: medium">
                                    <%=GetLabel("Informasi Paket")%></h4>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td>
                                <asp:TextBox runat="server" ID="txtItemName1" ReadOnly="true" TextMode="MultiLine"
                                    Rows="2" Width="700px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="color: Red">
                                <%=GetLabel("WARNING !")%>
                                <br />
                                <%=GetLabel("Paket yang menggunakan paket item tidak bisa menggunakan Hitung Berdasarkan HNA Terakhir.")%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: left; width: 100%; margin-bottom: 10px; height: auto; overflow: hidden">
        <table width="100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width: 250px" />
                            <col style="width: 50px" />
                            <col style="width: 350px" />
                        </colgroup>
                        <tr>
                            <td>
                                <%=GetLabel("Hitung Berdasarkan HNA Terakhir")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUsedLastHNA" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Hitung Ulang Tarif Variabel")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsIncludeVariableTariff" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Hitung Ulang Berdasarkan Buku Tarif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsResetItemTariff" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="color: Red">
                                <%=GetLabel("WARNING !")%>
                                <br />
                                <%=GetLabel("Hitung ulang berdasarkan buku tariff, akan menimpa semua nilai diskon dan nilai cito.")%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="middle" align="right">
                    <div id="divWarningPendingSave" runat="server">
                        <table width="350px" align="right">
                            <tr>
                                <td align="right" style="vertical-align: middle;" class="blink-alert">
                                    <img height="40px" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
                                </td>
                                <td align="right" style="vertical-align: middle;">
                                    <label class="lblWarning">
                                        <%=GetLabel("JANGAN LUPA KLIK *SIMPAN* SETELAH RECALCULATE")%></label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 240px; overflow-y: auto; overflow-x: hidden">
        <dxcp:ASPxCallbackPanel ID="cbpRecalculationPatientBillProcess" runat="server" ClientInstanceName="cbpRecalculationPatientBillProcess"
            ShowLoadingPanel="false" OnCallback="cbpRecalculationPatientBillProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpRecalculationPatientBillProcessEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlRecal" Style="width: 100%; font-size: 0.95em;">
                        <asp:ListView ID="lvwRecal" runat="server">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                    <tr>
                                        <th rowspan="2">
                                            <div style="padding: 3px">
                                                <asp:CheckBox ID="chkSelectAllCtl" runat="server" CssClass="chkSelectAllCtl" />
                                            </div>
                                        </th>
                                        <th rowspan="2">
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("Deskripsi")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width: 30px">
                                            <div style="text-align: center; padding-left: 3px">
                                                <%=GetLabel("Kelas Tagihan")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width: 30px">
                                            <div style="text-align: center; padding-left: 3px;">
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
                                        <th rowspan="2" style="width: 60px">
                                            <div style="text-align: center; padding-right: 3px">
                                                <%=GetLabel("Dibuat Oleh")%>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 60px">
                                            <div style="text-align: right; padding-right: 3px">
                                                <%=GetLabel("CITO")%>
                                            </div>
                                        </th>
                                        <th style="width: 60px">
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
                                <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                    <tr>
                                        <th rowspan="2" style="width: 20px">
                                            <div style="padding: 3px">
                                                <asp:CheckBox ID="chkSelectAllCtl" runat="server" CssClass="chkSelectAllCtl" />
                                            </div>
                                        </th>
                                        <th rowspan="2">
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("Deskripsi")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width: 30px">
                                            <div style="text-align: center; padding-left: 3px">
                                                <%=GetLabel("Kelas Tagihan")%>
                                            </div>
                                        </th>
                                        <th rowspan="2" style="width: 30px">
                                            <div style="text-align: center; padding-left: 3px;">
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
                                        <th rowspan="2" style="width: 60px">
                                            <div style="text-align: center; padding-right: 3px">
                                                <%=GetLabel("Dibuat Oleh")%>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 60px">
                                            <div style="text-align: right; padding-right: 3px">
                                                <%=GetLabel("CITO")%>
                                            </div>
                                        </th>
                                        <th style="width: 60px">
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
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <div style="padding: 3px">
                                            <asp:CheckBox ID="chkIsSelectedCtl" CssClass="chkIsSelectedCtl" runat="server" />
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
                                                <span style="color: Maroon">
                                                    <%#: Eval("TransactionNo")%></span><br />
                                                <span style="color: Maroon">
                                                    <%#: Eval("ChargesServiceUnitName")%></span>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding: 3px; text-align: center">
                                            <div>
                                                <%# Eval("ChargeClassName")%></div>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="padding: 3px; text-align: center">
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
                                        <div style="padding: 3px; text-align: center;">
                                            <div>
                                                <%#: Eval("CreatedByUserName")%></div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div>
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <colgroup>
                <col style="width: 15%" />
                <col style="width: 35%" />
                <col style="width: 15%" />
                <col style="width: 35%" />
            </colgroup>
            <tr>
                <td>
                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                        <%=GetLabel("Grand Total :") %>
                    </div>
                </td>
                <td style="text-align: right; padding-right: 10px;">
                    <%=GetLabel("Rp ") %>
                    <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                </td>
            </tr>
            <tr>
                <td>
                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                        <%=GetLabel("Grand Total Instansi :") %>
                    </div>
                </td>
                <td style="text-align: right; padding-right: 10px;">
                    <%=GetLabel("Rp ") %>
                    <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server"
                        Width="200px" />
                </td>
                <td>
                    <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                        <%=GetLabel("Grand Total Pasien :") %>
                    </div>
                </td>
                <td style="text-align: right; padding-right: 10px;">
                    <%=GetLabel("Rp ") %>
                    <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server"
                        Width="200px" />
                </td>
            </tr>
        </table>
    </div>
</div>
