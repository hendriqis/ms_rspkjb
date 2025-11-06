<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintMedicationLabelCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrintMedicationLabelCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbPrint.png")%>' alt="" /><div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        $('.txtExpiredDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
        });
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedScheduleID = "";
        var tempSelectedDate = "";
        var tempSelectedPrintNo = "";
        $('.grdPrescriptionOrderDt .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var sch = $(this).closest('tr').find('.hiddenColumn').html();
            var expiredDate = $(this).closest('tr').find('.txtExpiredDate').val();
            var printNo = $(this).closest('tr').find('.txtPrintNo').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedScheduleID += ",";
                tempSelectedDate += ",";
                tempSelectedPrintNo += ",";
            }
            tempSelectedID += id;
            tempSelectedScheduleID += sch;
            tempSelectedDate += expiredDate;
            tempSelectedPrintNo += printNo;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedScheduleID.ClientID %>').val(tempSelectedScheduleID);
            $('#<%=hdnSelectedExpiredDate.ClientID %>').val(tempSelectedDate);
            $('#<%=hdnSelectedPrintNo.ClientID %>').val(tempSelectedPrintNo);
            return true;
        }
        else return false;
    }

    setDatePicker('<%=txtMedicationDate.ClientID %>');
    $('#<%=txtMedicationDate.ClientID %>').datepicker('option', 'maxDate', '1');

    $('#<%=txtMedicationDate.ClientID %>').change(function (evt) {
        onRefreshGrid();
    });

    $('#<%=chkIsUDDOnly.ClientID %>').change(function () {
        onRefreshGrid();
    });

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Print Medication Label", 'Error Message : ' + "Please select the item to be print !");
        }
        else {
            var message = "Print the Medication Label for date <b>" + $('#<%:txtMedicationDate.ClientID %>').val() + "</b> and  <b>Sequence Number " + cboSequence.GetValue() + "</b> ?";
            displayConfirmationMessageBox("Print Medication Label",message, function (result) {
                if (result) cbpPopupProcessPrint.PerformCallback('process');
            });
        }
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'print') {
            if (param[1] == '0') {
                displayErrorMessageBox("Print Medication Label", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
            }
        }
    }

    function oncbpPrintViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpPrintView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnSelectedExpiredDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedPrintNo" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnReferenceNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionDate" value="" />
<div>
    <div>
        <table>
            <colgroup>
                <col width="150px" />
                <col width="185px" />
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
                <td>
                    <asp:CheckBox ID="chkIsUDDOnly" runat="server" Text=" hanya obat UDD saja"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" />
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td><asp:CheckBox ID="chkIsPrintReceipt" runat="server" Text=" Print Daftar Obat Penyerahan" Checked="false" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkIsPrintDrugLabel" runat="server" Text=" Print Etiket Obat" /></td>
                        </tr>
                    </table>                                        
                </td>
            </tr>
        </table>
    </div>
    <div style="height:400px; overflow:scroll;overflow-x: hidden;">
        <table class="tblContentArea" width="100%">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpPrintView" runat="server" Width="100%" ClientInstanceName="cbpPrintView"
                        ShowLoadingPanel="false" OnCallback="cbpPrintView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpPrintViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelPrintContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwPrintView" OnItemDataBound="lvwPrintView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
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
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th class="hiddenColumn" >&nbsp;</th>
                                                    <th  align="center" style="width:30px">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th  align="left"><%=GetLabel("Nama Obat")%></th>
                                                    <th  align="left"><%=GetLabel("Dosis Pemberian")%></th>
                                                    <th  align="left"><%=GetLabel("Rute Obat")%></th>
                                                    <th  align="left"><%=GetLabel("Tgl Expired")%></th>
                                                    <th  align="left"><%=GetLabel("Jumlah Etiket")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("ID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                <td style="width:80px" class="tdDosingQuantity"><label class="lblItemName"><%#: Eval("cfDosingQuantity")%></label></td>
                                                <td style="width:80px" class="tdRoute"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                                <td style="width:110px"><asp:TextBox runat="server" ID="txtExpiredDate" CssClass="txtExpiredDate" Width="80px" /></td>
                                                <td style="width:60px"><asp:TextBox runat="server" ID="txtLabelNo" CssClass="txtPrintNo txtNumeric" Width="50px" Text="1" /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcessPrint" runat="server" Width="100%" ClientInstanceName="cbpPopupProcessPrint"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcessPrint_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
