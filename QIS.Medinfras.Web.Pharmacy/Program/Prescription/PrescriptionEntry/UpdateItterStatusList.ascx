<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateItterStatusList.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.Prescription.PrescriptionEntry.UpdateItterStatusList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_printprescriptionlist">
    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {
        $('#chkSelectAll').prop('checked', true);

        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', true);
            $(this).change();
        });

        $('#btnSaveItter').click(function () {
            if (getSelectedCheckbox()) {
                cbpProcessItter.PerformCallback();
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
        var tempSelectedGCItter = "";
        var tempSelectedItterNo = "";
        var result = true;

        $('.grdPrescriptionOrderDt .chkIsSelected input:checked').each(function () {
            var prescriptionOrderDtID = $(this).closest('tr').find('.keyField').html();

            var expiredDate = $(this).closest('tr').find('.txtExpiredDate').val();
            var gcItter = $(this).closest('tr').find('.hdnRefillInstruction').val();
            var itterNo = $(this).closest('tr').find('.txtItterNo').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedDate += ",";
                tempSelectedGCItter += ",";
                tempSelectedItterNo += ",";
            }

            tempSelectedID += prescriptionOrderDtID;
            tempSelectedDate += expiredDate;
            tempSelectedGCItter += gcItter;
            tempSelectedItterNo += itterNo;

            if (gcItter == 'X454^001' || gcItter == 'X454^002') {
                if (itterNo == '') {
                    showToast(' Failed', 'Untuk DET ORIG dan DET Harus Isi Itter Number');
                    result = false;
                }
            }
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedDate.ClientID %>').val(tempSelectedDate);
            $('#<%=hdnSelectedGcItter.ClientID %>').val(tempSelectedGCItter);
            $('#<%=hdnSelectedItterNo.ClientID %>').val(tempSelectedItterNo);
        }
        else {
            result = false;
        }
        return result;
    }

    //#region Test Partner
    $td = null;
    $('.lblRefillInstruction.lblLink').live('click', function () {
        $td = $(this).parent();
        var filter = "IsActive = 1 AND IsDeleted = 0 AND ParentID = 'X454'";
        openSearchDialog('standardcodeIter', filter, function (value) {
            onTxtRefillInstructionChanged(value);
        });
    });

    function onTxtRefillInstructionChanged(value) {
        var filterExpression = "StandardCodeID = '" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.hdnRefillInstruction').val(result.StandardCodeID);
                $td.find('.lblRefillInstruction').html(result.StandardCodeName);

                if (result.StandardCodeID == 'X454^002' || result.StandardCodeID == 'X454^001') {
                    $td.find('.txtItterNo').removeAttr('style');
                    $td.find('.txtItterNo').val('');
                }
                else {
                    $td.find('.txtItterNo').attr('style', 'display:none');
                    $td.find('.txtItterNo').val('');
                }
            }
            else {
                $td.find('.hdnRefillInstruction').val('0');
                $td.find('.lblRefillInstruction').html('');
                $td.find('.txtItterNo').attr('style', 'display:none');
                $td.find('.txtItterNo').val('');
            }
        });
    }
    
    //#endregion

    $('.txtItterNo').live('change', function () {
        $td = $(this).parent();
        var refill = $td.find('.hdnRefillInstruction').val();
        var value = $(this).val();

        if (refill == 'X454^002' || refill == 'X454^001') {
            $(this).val(checkMinus(value)).trigger('changeValue');
        }
        else {
            $(this).val('');
            showToast('Warning', 'Pilih Itter terlebih dahulu / ini hanya untuk itter (DET ORIG / DET)');
        }
    });

</script>
<input type="hidden" runat="server" id="hdnPrescriptionOrderID" value="0" />
<input type="hidden" runat="server" id="hdnPrescriptionTypeCtl" value="0" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedGcItter" value="" />
<input type="hidden" runat="server" id="hdnSelectedItterNo" value="" />
<input type="hidden" runat="server" id="hdnIsUsedDispenseQty" value="0" />
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
                                                    <th style="width: 120px;" align="left">
                                                        <%=GetLabel("Itter") %>
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
                                                    <asp:TextBox runat="server" ID="txtExpiredDate" CssClass="txtExpiredDate" Width="80px" />
                                                </td>
                                                <td>
                                                    <input type="hidden" value="0" class="hdnRefillInstruction" id="hdnRefillInstruction"
                                                        runat="server" />
                                                    <label id="lblRefillInstruction" class="lblRefillInstruction lblLink">
                                                        <%# Eval("cfItter") %></label>
                                                    <input type="text" id="txtItterNo" class="txtItterNo" value="" runat="server" size="2" style='display:none' />
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
                <input type="button" id="btnSaveItter" value='<%= GetLabel("Save")%>' style="width: 100px" />
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpProcessItter" runat="server" Width="100%" ClientInstanceName="cbpProcessItter"
            ShowLoadingPanel="false" OnCallback="cbpProcessItter_Callback">
            <ClientSideEvents BeginCallback="function(s,e) {showLoadingPanel();}" EndCallback="function(s,e){         
            hideLoadingPanel();
            pcRightPanelContent.Hide();}" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
