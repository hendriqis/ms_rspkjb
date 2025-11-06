<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DispenseMedicationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.DispenseMedicationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
        <li id="btnMPStockInformation">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
                <%=GetLabel("Informasi Stok")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        $('.tblResultInfo').hide();
        $('#<%=txtDefaultTime.ClientID %>').attr("disabled", "disabled");

        $('#<%=chkUseDefaultTime.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked)
                $('#<%=txtDefaultTime.ClientID %>').removeAttr("disabled");
            else
                $('#<%=txtDefaultTime.ClientID %>').attr("disabled", "disabled");
        });

        SetTransactionDate();
    });

    function SetTransactionDate() {
        var today = Methods.getDatePickerDate($('#<%=txtMedicationDate.ClientID %>').val());
        var transDate = Methods.getDatePickerDate($('#<%=txtMedicationDate.ClientID %>').val());
        var medicationDate = Methods.getDatePickerDate($('#<%=txtMedicationDate.ClientID %>').val());
        transDate.setDate(medicationDate.getDate());

        var medicationDateText = Methods.dateToDatePickerFormat(medicationDate);
        var transDateText = Methods.dateToDatePickerFormat(transDate);
        var todayText = Methods.dateToDatePickerFormat(today);

        $('#<%=txtTransactionDate.ClientID %>').val(transDateText);
        if (transDateText != todayText) {
            $('#<%=txtTransactionTime.ClientID %>').val('00:00');
        }
    }

    $('#chkSelectAll1').die('change');
    $('#chkSelectAll1').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem1').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            var isApplyDefaultTime = $('#<%=chkUseDefaultTime.ClientID %>').is(":checked");
            if (isApplyDefaultTime) {
                $('.txtMedicationTime').each(function () {
                    $(this).val($('#<%=txtDefaultTime.ClientID %>').val());
                });
            }
        }
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedItemID = "";
        var tempSelectedScheduleID = "";
        var tempSelectedTime = "";
        var tempSelectedTimeTransaction = "";
        var tempSelectedMultiDose = "";
        $('.grdPrescriptionOrderDt .chkIsProcessItem1 input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var itemID = $(this).closest('tr').find('.itemID').html();
            var sch = $(this).closest('tr').find('.hiddenColumn').html();
            var time = $tr.find('.txtMedicationTime').val();
            var timeTransaction = $('#<%=txtTransactionTime.ClientID %>').val();
            var isMultiDose = '0';
            if ($tr.find('.chkIsMultiDose').is(':checked')) {
                isMultiDose = '1';
            }
            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedItemID += ",";
                tempSelectedScheduleID += ",";
                tempSelectedTime += ",";
                tempSelectedTimeTransaction += ",";
                tempSelectedMultiDose += ",";
            }
            tempSelectedID += id;
            tempSelectedItemID += itemID;
            tempSelectedScheduleID += sch;
            tempSelectedTime += time;
            tempSelectedTimeTransaction += timeTransaction;
            tempSelectedMultiDose += isMultiDose;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedItemID.ClientID %>').val(tempSelectedItemID);
            $('#<%=hdnSelectedScheduleID.ClientID %>').val(tempSelectedScheduleID);
            $('#<%=hdnSelectedTime.ClientID %>').val(tempSelectedTime);
            $('#<%=hdnSelectedTimeTransaction.ClientID %>').val(tempSelectedTimeTransaction);
            $('#<%=hdnSelectedMultiDose.ClientID %>').val(tempSelectedMultiDose);
            return true;
        }
        else return false;
    }

    var maxDayDispense = $('#<%=hdnMaxDispenseDay.ClientID %>').val();

    setDatePicker('<%=txtMedicationDate.ClientID %>');
    $('#<%=txtMedicationDate.ClientID %>').datepicker('option', 'maxDate', maxDayDispense);

    setDatePicker('<%=txtTransactionDate.ClientID %>');
    $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', maxDayDispense);
    $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'minDate', '-1');

    $('#<%=txtMedicationDate.ClientID %>').die('change');
    $('#<%=txtMedicationDate.ClientID %>').live('change', function (evt) {
        var medicationDate = $('#<%=txtMedicationDate.ClientID %>').val();
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        var from = medicationDate.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);

        var to = dateToday.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (f > t) {
            $('#<%=txtMedicationDate.ClientID %>').val(dateToday);
        }

        SetTransactionDate();
        onRefreshGrid();
    });

    $('#<%=txtTransactionDate.ClientID %>').die('change');
    $('#<%=txtTransactionDate.ClientID %>').live('change', function (evt) {
        var transactionDate = $('#<%=txtTransactionDate.ClientID %>').val();
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        var from = transactionDate.split("-");
        var f = new Date(from[2], from[1] - 1, from[0]);

        var to = dateToday.split("-");
        var t = new Date(to[2], to[1] - 1, to[0]);

        if (f > t) {
            $('#<%=txtTransactionDate.ClientID %>').val(dateToday);
        }
    });

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("UDD - DISPENSE", "Please select the item to be process !");
        }
        else {
            var message = "Process the Medication Order for date <b>" + $('#<%:txtMedicationDate.ClientID %>').val() + "</b> and  <b>Sequence Number " + cboSequence.GetValue() + "</b> ?";

            displayConfirmationMessageBox("UDD - DISPENSE",message, function (result) {
                if (result) cbpPopupProcessDispense.PerformCallback('process');
            });
        }
    });

    $('#btnMPStockInformation').live('click', function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Informasi Stok", "Please select the item to be process !");
        }
        else {
            cbpCalculateItem.PerformCallback('calculate');
        }
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                $('.tblResultInfo').hide();
                displayErrorMessageBox("UDD - DISPENSE", "<br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                $('.tblResultInfo').show();
                $('#<%=txtReferenceNo.ClientID %>').val(param[3]);
                $('#<%=txtTransactionNo.ClientID %>').val(param[4]);
                if (param[5] == "") {
                    displayMessageBox('UDD - DISPENSE', param[2]);
                }
                else {
                    var message = param[2] + "<br/><span style='color:red'>" + param[5] + "</span>"
                }

                onRefreshGrid();
                if (typeof onRefreshDetailList == 'function')
                    onRefreshDetailList();
            }
        }
    }

    function oncbpDispenseViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpDispenseView.PerformCallback('refresh');
    }

    function onCbpCalculateItemViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retVal = s.cpRetval.split('|');
        if (param[0] == 'calculate') {
            if (param[1] == 'fail') {
                var messageBody = param[2];
                displayErrorMessageBox('Informasi Stok', messageBody);
            }
            else {
                $('#<%=hdnBalanceInfo.ClientID %>').val(retVal);
                displayMessageBox('Informasi Stok', $('#<%=hdnBalanceInfo.ClientID %>').val());
            }
        }
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItemID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime" value="" />
<input type="hidden" runat="server" id="hdnSelectedTimeTransaction" value="" />
<input type="hidden" runat="server" id="hdnSelectedMultiDose" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionFeeAmount" value="0" />
<input type="hidden" runat="server" id="hdnLocationID" value="" />
<input type="hidden" runat="server" id="hdnUDDRoundingSystem" value="1" />
<input type="hidden" runat="server" id="hdnBalanceInfo" value="" />
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionDate" value="" />
<input type="hidden" runat="server" id="hdnDateToday" value="0" />
<input type="hidden" runat="server" id="hdnMaxDispenseDay" value="1" />
<input type="hidden" runat="server" id="hdnIsEndingAmountRoundingTo100" value="0" />
<input type="hidden" runat="server" id="hdnIsEndingAmountRoundingTo1" value="0" />
<div>
    <div>
        <table style="width:100%">
            <colgroup>
                <col width="200px" />
                <col width="200px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Farmasi")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDispensary" ClientInstanceName="cboDispensary"
                    runat="server" Width="100%" >
                         <ClientSideEvents ValueChanged="function() { cboLocation.PerformCallback(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Lokasi Dispensing")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboLocation" ClientInstanceName="cboLocation"
                    runat="server" Width="100%" OnCallback="cboLocation_Callback">
                        <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sequence Pemberian")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboSequence" ClientInstanceName="cboSequence"
                    runat="server" Width="100%" >
                        <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Waktu Default Pemberian")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtDefaultTime" runat="server" Width="60px" Text="00:00" CssClass="time" Enabled="false"/>
                    <asp:CheckBox ID="chkUseDefaultTime" runat="server" Text=" Gunakan Waktu Default" Checked="false" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkIsAutoPrintReceipt" runat="server" Text=" Print Daftar Obat Penyerahan" Checked="false" />
                    <asp:CheckBox ID="chkIsPrintDrugLabel" runat="server" Text=" Print Etiket Obat" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Transaksi")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtTransactionDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Waktu Transaksi")%></label>
                </td>
                <td class="tdLabel">
                    <asp:TextBox ID="txtTransactionTime" runat="server" Width="60px" Text="00:00" CssClass="time"/>
                </td>
            </tr>
        </table>
    </div>
    <div style="height:auto; max-height:300px; overflow:auto;overflow-x: hidden;">
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col width="200px" />
                <col width="200px" />
                <col />
            </colgroup>
            <tr>
                <td />
                <td />
                <td />
            </tr>
            <tr>
                <td colspan="3">
                     <dxcp:ASPxCallbackPanel ID="cbpDispenseView" runat="server" Width="100%" ClientInstanceName="cbpDispenseView"
                        ShowLoadingPanel="false" OnCallback="cbpDispenseView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpDispenseViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDispenseContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwDispenseView" OnItemDataBound="lvwDispenseView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblDispenseView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th  align="left"><%=GetLabel("Drug Name")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="2">
                                                        <%=GetLabel("Tidak ada data penjadwalan obat")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblDispenseView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th class="hiddenColumn" >&nbsp;</th>
                                                    <th  align="center" style="width:30px">
                                                        <input id="chkSelectAll1" type="checkbox" />
                                                    </th>
                                                    <th  align="left"><%=GetLabel("Nama Item")%></th>
                                                    <th  align="left"><%=GetLabel("Jumlah Dosis")%></th>
                                                    <th  align="left"><%=GetLabel("Rute Obat")%></th>
                                                    <th  align="center" style="width:70px"><%=GetLabel("Waktu Pemberian")%></th>
                                                    <th  align="center" style="width:70px"><%=GetLabel("Multi Dose")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("ID")%></td>
                                                <td class="hiddenColumn itemID"><%#: Eval("ItemID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsProcessItem1" runat="server" CssClass="chkIsProcessItem1" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                <td class="tdDosingQuantity"><label class="lblItemName"><%#: Eval("cfDosingQuantity")%></label></td>
                                                <td class="tdRoute" style="width:100px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                                <td><input type="text" class="txtMedicationTime" CssClass="time" value="<%#:Eval("MedicationTime") %>" /></td>
                                                <td align="center"><input type="checkbox" class="chkIsMultiDose" style="width:20px" /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="display:none">
                        <table class="tblResultInfo" width="100%" style="display:none">
                            <colgroup>
                                <col width="115px" />
                                <col width="200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nomor Referensi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferenceNo" runat="server" Width="100%" ReadOnly="true"/>
                                </td>                                
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nomor Transaksi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionNo" runat="server" Width="100%" ReadOnly="true"/>
                                </td>  
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <colgroup>
                <col width="200px" />
                <col width="200px" />
                <col />
            </colgroup>
            <tr>
                <td colspan="3">
                    <table style="width:100%">
                        <colgroup width="70px" />
                        <colgroup />
                        <tr>
                            <td>
                                <img src='<%=ResolveUrl("~/Libs/Images/warning.png")%>' alt="" height="65px" width="65px" />
                            </td>
                            <td style="vertical-align:top;">
                                <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("INFORMASI : ")%></h4>
                                <%=GetLabel("- Tanggal dan waktu transaksi yang akan tercatat di Transaksi Pasien")%>
                                <br />
                                <%=GetLabel("- Perubahan Waktu Pemberian pada saat dispensing tidak akan merubah waktu pemberian untuk proses dispensing di hari berikutnya")%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcessDispense" runat="server" Width="100%" ClientInstanceName="cbpPopupProcessDispense"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcessDispense_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>

        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpCalculateItem" runat="server" Width="100%" ClientInstanceName="cbpCalculateItem"
                ShowLoadingPanel="false" OnCallback="cbpCalculateItem_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpCalculateItemViewEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</div>
