<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintDrugLabelList.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrintDrugLabelList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<div class="toolbarArea">
    <ul>
        <li id="btnPrintLabel">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbPrint.png")%>' alt="" /><div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_printprescriptionlist">
    $('#<%=chkisUsedDispenseQty.ClientID %>').prop('checked', true);

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {

        $('#btnPrintLabel').click(function () {
            if (!getSelectedCheckbox()) {
                displayErrorMessageBox("PRINT", 'Error Message : ' + "Please select the item to be print !");
            }
            else {
                cbpProcessPrint.PerformCallback();
            }
        });

        $('.txtExpiredDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
        });
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedDate = "";
        var tempSelectedPrintNo = "";
        $('.grdPrescriptionOrderDt .chkIsSelected input:checked').each(function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var expiredDate = $(this).closest('tr').find('.txtExpiredDate').val();
            var printNo = $(this).closest('tr').find('.txtPrintNo').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedDate += ",";
                tempSelectedPrintNo += ",";
            }
            tempSelectedID += id;
            tempSelectedDate += expiredDate;
            tempSelectedPrintNo += printNo;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedExpiredDate.ClientID %>').val(tempSelectedDate);
            $('#<%=hdnSelectedPrintNo.ClientID %>').val(tempSelectedPrintNo);
            return true;
        }
        else return false;
    }
</script>
<input type="hidden" runat="server" id="hdnTransactionID" value="0" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnIsUsedDispenseQty" value="0" />
<input type="hidden" runat="server" id="hdnSelectedExpiredDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedPrintNo" value="" />
<div>
    <table width="100%">
        <tr>
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width: 175px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label>
                                No. Resep
                            </label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPresciptionNo" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkisUsedDispenseQty" runat="server" Checked="false" /><%:GetLabel("Cetak Sesuai dengan Jumlah diambil")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Tanggal - Waktu
                            </label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-right: 1px; width: 145px">
                                        <asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server"
                                            ReadOnly="true" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server"
                                            Style="text-align: center" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="position: relative; width: 100%">
                                <asp:ListView ID="lvwView" runat="server">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal grdSelected" cellspacing="0" rules="all">
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
                                            <td class="keyField"><%#: Eval("ID")%></td>
                                            <td class="hiddenColumn"><%#: Eval("TransactionID")%></td>
                                            <td align="center"><asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" /></td>
                                            <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                            <td class="tdDosingQuantity"><label class="lblItemName"><%#: Eval("cfDosingQuantity")%></label></td>
                                            <td class="tdRoute" style="width:100px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                            <td style="width:110px"><asp:TextBox runat="server" ID="txtExpiredDate" CssClass="txtExpiredDate" Width="80px" /></td>
                                            <td style="width:60px"><asp:TextBox runat="server" ID="txtLabelNo" CssClass="txtPrintNo txtNumeric" Width="50px" Text="1" /></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcessPrint" runat="server" Width="100%" ClientInstanceName="cbpProcessPrint"
            ShowLoadingPanel="false" OnCallback="cbpProcessPrint_Callback">
            <ClientSideEvents BeginCallback="function(s,e) {showLoadingPanel();}"
                EndCallback="function(s,e){         
            var printResult = s.cpZebraPrinting;
            if (printResult != '')
                showToast('Warning', printResult);
            hideLoadingPanel();}" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
