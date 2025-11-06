<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintPrescriptionList.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.Prescription.PrescriptionEntry.PrintPrescriptionList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
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
        $('#btnPrintPrescription').click(function () {
            if (getSelectedCheckbox()) {
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
        $('.grdPrescriptionOrderDt .chkIsSelected input:checked').each(function () {
            var prescriptionOrderDtID = $(this).closest('tr').find('.keyField').html();

            var expiredDate = $(this).closest('tr').find('.txtExpiredDate').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedDate += ",";
            }
            tempSelectedID += prescriptionOrderDtID;
            tempSelectedDate += expiredDate;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedDate.ClientID %>').val(tempSelectedDate);
            return true;
        }
        else return false;
    }
</script>
<input type="hidden" runat="server" id="hdnPrescriptionOrderID" value="0" />
<input type="hidden" runat="server" id="hdnPrescriptionTypeCtl" value="0" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedDate" value="" />
<input type="hidden" runat="server" id="hdnIsDotMatrix" value="0" />
<input type="hidden" runat="server" id="hdnIsUsedDispenseQty" value="0" />
<input type="hidden" runat="server" id="hdnPrinterType" value="0" />
<input type="hidden" runat="server" id="hdnIsUsedlastPurchaseExpiredDate" value="0" />
<div>
    <table width="100%">
        <tr>
            <td>
                <table width="75%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 5px" />
                        <col style="width: 150px" />
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
                        <td><%:GetLabel("Jumlah Cetak")%><asp:TextBox runat="server" ID="txtJmlLabelCtl" Text="1" Width="55%" TextMode="Number"></asp:TextBox></td>
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
                    <tr id="trPrescriptionType" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Resep")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                Width="233px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="height: 400px; overflow: auto">
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="position: relative; width: 100%">
                                    <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwViewPrint_RowDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px">
                                                    </th>
                                                    <th align="left">
                                                        <div>
                                                            <%=GetLabel("generik") %>
                                                            -
                                                            <%=GetLabel("Nama Obat") %>
                                                            -
                                                            <%=GetLabel("Kadar") %>
                                                            -
                                                            <%=GetLabel("Bentuk") %></div>
                                                        <div>
                                                            <div style="color: blue; width: 42px; float: left;">
                                                                <%=GetLabel("DOSIS") %></div>
                                                            -
                                                            <%=GetLabel("Rute") %>
                                                            -
                                                            <%=GetLabel("Frekuensi") %>
                                                        </div>
                                                    </th>
                                                    <th style="width: 70px;" align="center">
                                                        <%=GetLabel("Jumlah Order") %>
                                                    </th>
                                                    <th style="width: 70px;" align="center">
                                                        <%=GetLabel("Jumlah Diambil") %>
                                                    </th>
                                                    <th style="width: 140px;" align="left">
                                                    </th>
                                                </tr>
                                                <tr align="center" style="height: 50px; vertical-align: middle;">
                                                    <td colspan="2">
                                                        <%=GetLabel("Tidak ada data obat dalam nomor resep ini") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdNormal notAllowSelect grdPrescriptionOrderDt"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th align="left" style="padding: 3px">
                                                        <div>
                                                            <%=GetLabel("generik") %>
                                                            -
                                                            <%=GetLabel("Nama Obat") %>
                                                            -
                                                            <%=GetLabel("Kadar") %>
                                                            -
                                                            <%=GetLabel("Bentuk") %>
                                                        </div>
                                                        <div>
                                                            <div style="color: blue; width: 42px; float: left;">
                                                                <%=GetLabel("DOSIS") %></div>
                                                            -
                                                            <%=GetLabel("Rute") %>
                                                            -
                                                            <%=GetLabel("Frekuensi") %>
                                                        </div>
                                                    </th>
                                                    <th style="width: 70px;" align="center">
                                                        <%=GetLabel("Jumlah Order") %>
                                                    </th>
                                                    <th style="width: 70px;" align="center">
                                                        <%=GetLabel("Jumlah Diambil") %>
                                                    </th>
                                                    <th style="width: 120px;" align="center">
                                                        <%=GetLabel("Expired Date") %>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#:Eval("PrescriptionOrderDetailID") %>
                                                </td>
                                                <td style="vertical-align: middle; text-align: center;">
                                                    <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                                                </td>
                                                <td style="padding: 3px">
                                                    <div>
                                                        <div id="divItemName" runat="server" style="font-weight: bold">
                                                            <span class="itemName">
                                                                <%#: Eval("cfMedicationName")%></span></div>
                                                        <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                            <%#: Eval("cfCompoundDetail")%></div>
                                                        <div>
                                                            <div style="color: Blue; width: 35px; float: left;">
                                                                <%=GetLabel("DOSE")%></div>
                                                            <%#: Eval("NumberOfDosage")%>
                                                            <%#: Eval("DosingUnit")%>
                                                            -
                                                            <%#: Eval("Route")%>
                                                            -
                                                            <%#: Eval("cfDoseFrequency")%></div>
                                                </td>
                                                <td style="text-align: right">
                                                    <%#: Eval("DispenseQty")%>
                                                </td>
                                                <td style="text-align: right">
                                                    <%#: Eval("TakenQty")%>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtExpiredDate" CssClass="txtExpiredDate"
                                                        Width="80px" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnPrintPrescription" value='<%= GetLabel("Print")%>' style="width: 100px" />
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcessPrint" runat="server" Width="100%" ClientInstanceName="cbpProcessPrint"
            ShowLoadingPanel="false" OnCallback="cbpProcessPrint_Callback">
            <ClientSideEvents BeginCallback="function(s,e) {showLoadingPanel();}" EndCallback="function(s,e){         
            var printResult = s.cpZebraPrinting;
            if (printResult != '')
                showToast('Warning', printResult);
            hideLoadingPanel();}" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
