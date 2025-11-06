<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionOrderDtCtl2.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionOrderDt2Ctl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#<%=txtPrescriptionOrderNo.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtPrescriptionOrderDate.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtPrescriptionOrderTime.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtParamedic.ClientID %>').attr('readonly', 'readonly');

    setDatePicker('<%=txtTransactionDate.ClientID %>');
    $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=tblVoidReason.ClientID %>').hide();

    function getCheckedMember() {
        var lstSelectedMember1 = "";
        var lstSelectedMember2 = "";
        var lstSelectedMemberTakenQty1 = "";
        var lstSelectedMemberTakenQty2 = "";

        $('.grdPrescriptioOrderDt .chkIsSelected input:checked').each(function () {
            var prescriptionOrderDetailID = $(this).closest('tr').find('.keyField').html();
            var takenQty1 = $(this).closest('tr').find('.txtTakenQty1').val();
            var takenQty2 = $(this).closest('tr').find('.txtTakenQty2').val();

            if (lstSelectedMember1 != "")
                lstSelectedMember1 += ',';
            if (parseFloat(takenQty1) >= 0) {
                if (lstSelectedMemberTakenQty1 != "")
                    lstSelectedMemberTakenQty1 += ',';
                lstSelectedMember1 += prescriptionOrderDetailID;
                lstSelectedMemberTakenQty1 += takenQty1;
            }

            if (parseFloat(takenQty2) >= 0) {
                if (lstSelectedMember2 != "")
                    lstSelectedMember2 += ',';
                if (lstSelectedMemberTakenQty2 != "")
                    lstSelectedMemberTakenQty2 += ',';
                lstSelectedMember2 += prescriptionOrderDetailID;
                lstSelectedMemberTakenQty2 += takenQty2;
            }
        });
        $('#<%=hdnLstSelected1.ClientID %>').val(lstSelectedMember1);
        $('#<%=hdnLstSelectedTakenQty1.ClientID %>').val(lstSelectedMemberTakenQty1);
        $('#<%=hdnLstSelected2.ClientID %>').val(lstSelectedMember2);
        $('#<%=hdnLstSelectedTakenQty2.ClientID %>').val(lstSelectedMemberTakenQty2);
    }

    $('#<%=rblProcessType.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%=tblVoidReason.ClientID %>').hide();
            $('#<%=hdnIsNotTakenByPatient.ClientID %>').val('0');
            $('#<%=hdnProcessType.ClientID %>').val('approve');
        } else if (value == '2') {
            $('#<%=tblVoidReason.ClientID %>').hide();
            $('#<%=hdnIsNotTakenByPatient.ClientID %>').val('1');
            $('#<%=hdnProcessType.ClientID %>').val('nottaken');
        }
        else {
            $('#<%=tblVoidReason.ClientID %>').show();
            $('#<%=hdnIsNotTakenByPatient.ClientID %>').val('0');
            $('#<%=hdnProcessType.ClientID %>').val('decline'); 
        }
    });

    $('#btnProcess').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntryPopup')) {
            if ($('#<%=hdnIsOrderDetailExists.ClientID %>').val() == "1") {
                getCheckedMember();
                if ($('#<%=hdnLstSelected1.ClientID %>').val() == "") {
                    var messageBody = "Harus ada item yang dipilih untuk diproses";
                    displayMessageBox('ERROR : Konfirmasi Order', messageBody);
                }
                else {
                    cbpEntryPopupView.PerformCallback($('#<%=hdnProcessType.ClientID %>').val());
                }
            }
            else {
                cbpEntryPopupView.PerformCallback($('#<%=hdnProcessType.ClientID %>').val());
            }
        }
    });


    $('#btnCalculate').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntryPopup')) {
            if ($('#<%=hdnIsOrderDetailExists.ClientID %>').val() == "1") {
                getCheckedMember();
                if ($('#<%=hdnLstSelected1.ClientID %>').val() == "") {
                    var messageBody = "Harus ada item yang dipilih untuk diproses";
                    displayMessageBox('ERROR : Konfirmasi Order', messageBody);
                }
                else {
                    cbpCalculateItem.PerformCallback('calculate');
                }                             
            }
            else {
                cbpCalculateItem.PerformCallback($('#<%=hdnProcessType.ClientID %>').val());
            }
        }
    });


    $('#btnTestOrderClose').click(function (evt) {
        if ($('#<%=hdnChargesTransactionID.ClientID %>').val() == '' || $('#<%=hdnChargesTransactionID.ClientID %>').val() == '0') {
            showLoadingPanel();
            document.location = document.referrer;
        }
        else {
            pcRightPanelContent.Hide();
        }
    });
    
    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'approve' || param[0] == 'nottaken') {
            if (param[1] == 'fail')
                showToast('Proses Pelayanan Resep Gagal', 'Error Message : ' + param[2]);
            else {
                if ($('#<%=hdnChargesTransactionID.ClientID %>').val() == '')
                    onAfterAddRecordAddRowCount();
                onLoadObject(s.cpRetval);
                pcRightPanelContent.Hide();
            }
        }
        else if (param[0] == 'decline') {
            if (param[1] == 'fail')
                showToast('Proses Pembatalan Gagal', 'Error Message : ' + param[2]);
            else {
                pcRightPanelContent.Hide();
            }
        }
        else
            pcRightPanelContent.Hide();
    }

    function onCbpCalculateItemViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retVal = s.cpRetval.split('|');
        if (param[0] == 'calculate') {
            if (param[1] == 'fail') {
                var messageBody = param[2];
                displayMessageBox('ERROR : Kalkulasi Nilai Transaksi', messageBody);
                $('#<%=txtTotalAmount.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTotalPayer.ClientID %>').val('0').trigger('changeValue');
                $('#<%=txtTotalPatient.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                $('#<%=txtTotalAmount.ClientID %>').val(retVal[0]).trigger('changeValue');
                $('#<%=txtTotalPayer.ClientID %>').val(retVal[1]).trigger('changeValue');
                $('#<%=txtTotalPatient.ClientID %>').val(retVal[2]).trigger('changeValue');
            }
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
            $(this).find('input').change();
        });
    });

    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtTakenQty1').removeAttr('readonly');
            $tr.find('.txtTakenQty2').removeAttr('readonly');
        }
        else {
            $tr.find('.txtTakenQty1').attr('readonly', 'readonly');
            $tr.find('.txtTakenQty2').attr('readonly', 'readonly');
        }
    });

    function onCboVoidReasonValueChanged(s) {
        if (s.GetValue() == 'X221^999')
            $('#<%=txtVoidReason.ClientID %>').show();
        else
            $('#<%=txtVoidReason.ClientID %>').hide();
    }

    $('.txtTakenQty1').live('change', function () {
        $tr = $(this).closest('tr').closest('tr');
        var itemName = parseInt($tr.find('.itemName').html());
        var orderQty = parseInt($tr.find('.txtOrderQuantity').val());
        var takenqty1 = parseInt($tr.find('.txtTakenQty1').val());
        var takenqty2 = parseInt($tr.find('.txtTakenQty2').val());
        if ((takenqty1+takenqty2) > orderQty) {
            var messageBody = "Maaf, Jumlah obat <b>" + itemName + "</b> yang diambil tidak boleh lebih besar dari jumlah yang diorderkan (<b>" + orderQty + "</b>)."
            displayMessageBox('Warning', messageBody);
            $tr.find('.txtTakenQty1').val(orderQty-takenqty2);
        }
    });

    $('.txtTakenQty2').live('change', function () {
        $tr = $(this).closest('tr').closest('tr');
        var itemName = parseInt($tr.find('.itemName').html());
        var orderQty = parseInt($tr.find('.txtOrderQuantity').val());
        var takenqty1 = parseInt($tr.find('.txtTakenQty1').val());
        var takenqty2 = parseInt($tr.find('.txtTakenQty2').val());
        if ((takenqty1 + takenqty2) > orderQty) {
            var messageBody = "Maaf, Jumlah obat <b>" + itemName + "</b> yang diambil tidak boleh lebih besar dari jumlah yang diorderkan (<b>" + orderQty + "</b>)."
            displayMessageBox('Warning', messageBody);
            $tr.find('.txtTakenQty2').val(orderQty - takenqty1);
        }
    });

    $(function () {
        $('#rightPanelCtl ul li').live('click', function () {
            $('#rightPanelCtl ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var id = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var url = $(this).attr('url');
            $('#containerLeftPanelContent').html('');
            if (url != '#') {
                $('#divLeftPanelContentLoading').show();
                Methods.getHtmlControl(ResolveUrl(url), id, function (result) {
                    $('#divLeftPanelContentLoading').hide();
                    $('#containerLeftPanelContent').html(result.replace(/\VIEWSTATE/g, ''));
                }, function () {
                    $('#divLeftPanelContentLoading').hide();
                });
            }
        });
        $('#rightPanelCtl ul li').first().click();
    });

    $('#imgDrugAlert').click(function (e) {
        pcNotes.Show();
    });

    $("#btnClose").click(cancelPcNotes);
    function cancelPcNotes() {
        pcNotes.Hide();
    }
</script>

<style type="text/css">
    #rightPanelCtl
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #rightPanelCtl > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #rightPanelCtl > ul > li
    {
        list-style-type: none;
        font-size: 12px;
        display: inline-block;
        border: 1px solid #848484;
        padding: 5px 8px;
        cursor: pointer;
    }
    #rightPanelCtl > ul > li.selected
    {
        background-color: blue;
        color: White;
    }
</style>

<input type="hidden" id="hdnChargeClassID" value="" runat="server" />
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<input type="hidden" id="hdnVisitID" value="" runat="server" />
<input type="hidden" id="hdnDepartmentID" value="" runat="server" />
<input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" />
<input type="hidden" id="hdnImagingServiceUnitID" runat="server" />
<input type="hidden" id="hdnPrescriptionOrderID" value="" runat="server" />
<input type="hidden" id="hdnIsOrderDetailExists" value="0" runat="server" />
<input type="hidden" id="hdnIsEntryMode" value="0" runat="server" />
<input type="hidden" id="hdnChargesTransactionID" value="" runat="server" />
<input type="hidden" id="hdnDefaultEmbalaceIDPopUp" value="" runat="server" />
<input type="hidden" id="hdnLstSelected1" value="" runat="server" />
<input type="hidden" id="hdnLstSelected2" value="" runat="server" />
<input type="hidden" id="hdnLstSelectedTakenQty1" value="" runat="server" />
<input type="hidden" id="hdnLstSelectedTakenQty2" value="" runat="server" />
<input type="hidden" id="hdnVisitInfo" value="" runat="server" />
<input type="hidden" id="hdnOrderHdInfo" value="" runat="server" />
<input type="hidden" id="hdnIsHasCompound" value="0" runat="server" />
<input type="hidden" id="hdnIsAutoGenerateReferenceNo" value="0" runat="server" />
<input type="hidden" id="hdnIsAutoInsertEmbalacePopUp" value="" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
<input type="hidden" value="approve" id="hdnProcessType" runat="server" />
<input type="hidden" value="0" id="hdnIsNotTakenByPatient" runat="server" />
<input type="hidden" value="0" id="hdnDispensaryInitial" runat="server" />
<input type="hidden" id="hdnIsGenerateQueueLabel" value="0" runat="server" />
<input type="hidden" id="hdnItemQtyWithSpecialQueuePrefix" value="0" runat="server" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<input type="hidden" id="hdnIsDisplayQueueOrder" runat="server" value="0" />
<input type="hidden" id="hdnIsUsingDrugAlert" runat="server" value="0" />
<input type="hidden" id="hdnIsGenerateTicket" runat="server" value="0" />

<div>
    <table style="width:100%">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>      
            <td style="vertical-align:top">
                <fieldset id="fsMPPopupEntry">  
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col style="width:140px"/>
                            <col style="width:60px"/>
                            <col style="width:120px;"/>
                            <col style="width:100px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Lokasi Pengambilan")%></label></td>
                            <td colspan="3"><dxe:ASPxComboBox runat="server" ID="cboPrescriptionOrderLocation" ClientInstanceName="cboPrescriptionOrderLocation" Width="100%" /></td>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pembayar")%></label></td>
                            <td colspan="2"><asp:TextBox ID="txtPayerName" Width="100%" runat="server" ReadOnly="true" /></td>
                        </tr>
                        <tr>
                            <td />
                            <td />
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Order Resep")%></label></td>
                            <td colspan="2"><asp:TextBox ID="txtPrescriptionOrderNo" Width="100%" runat="server" /></td> 
                            <td>
                                <div id="DivImageDrugAlertInfo" runat="server">
                                    <img class="imgDrugAlert imgLink blink-alert" id="imgDrugAlert" height="25px" src='<%= ResolveUrl("~/Libs/Images/Status/drug_alert.png")%>'
                                        alt='' title='Drug Alert Information' />
                                </div>  
                            </td>
                            <td class="tdLabel"><label class="lblNormal" id="lblServiceUnit"><%=GetLabel("Tanggal Order")%></label></td>
                            <td><asp:TextBox ID="txtPrescriptionOrderDate" Width="120px" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                            <td style=""><asp:TextBox ID="txtPrescriptionOrderTime" ReadOnly="true" Width="60px" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1"><%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="3"> 
                                <asp:TextBox ID="txtParamedic" Width="100%" runat="server" ReadOnly="true"  />
                            </td>
                            <td class="tdLabel"><label class="lblNormal" id="lblSendOrderInfo"><%=GetLabel("Order Dikirim")%></label></td>
                            <td><asp:TextBox ID="txtSendOrderDate" Width="120px" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                            <td style=""><asp:TextBox ID="txtSendOrderTime" ReadOnly="true" Width="60px" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Nomor Lisensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLicenseNo" ReadOnly="true" Width="60px" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Berakhir Lisensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLicenseExpiredDate" ReadOnly="true" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top:5px;"><label><%=GetLabel("Catatan Order")%></label></td>
                            <td colspan="6"><asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="110px" runat="server"/></td>
                        </tr>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel"><%=GetLabel("Tanggal") %> / <%=GetLabel("Jam Diproses") %></td>
                            <td><asp:TextBox ID="txtTransactionDate" Width="100px" runat="server" CssClass="datepicker" style="text-align:center" /></td>
                            <td><asp:TextBox ID="txtTransactionTime" Width="60px" CssClass="time" runat="server" Style="text-align:center" /></td>
                            <td colspan="3"> 
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td><label class="lblNormal" id="Label2"><%=GetLabel("Nomor Itter")%></label></td>
                                        <td><asp:TextBox ID="txtReferenceNo1" Width="60px" runat="server" /></td>
                                        <td class="tdLabel"><label class="lblNormal" id="lblReferenceNo"><%=GetLabel("Nomor Antrian")%></label></td>
                                        <td><asp:TextBox ID="txtReferenceNo2" Width="60px" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
<%--                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Refill Instruction")%></label></td>
                            <td colspan="2"><dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType" Width="208px" runat="server" /></td>
                        </tr>--%>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
<div style="height:300px;overflow-y:auto;">
    <div>
        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
            <EmptyDataTemplate>
                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all">
                    <tr>
                        <th style="width:40px;"></th>
                        <th align="left">
                            <div><%=GetLabel("generik")%> - <%=GetLabel("Nama Obat")%> - <%=GetLabel("Kadar")%> - <%=GetLabel("Bentuk")%></div>
                            <div><div style="color:Blue;width:42px;float:left;"><%=GetLabel("DOSIS")%></div> <%=GetLabel("dosis")%> - <%=GetLabel("Rute")%> - <%=GetLabel("Frekuensi")%></div>
                        </th>
                        <th style="width:100px;" align="left">
                            <div><%=GetLabel("Rak")%></div>
                        </th>
                        <th style="width:100px;" align="left">
                            <div><%=GetLabel("Jumlah Order")%></div>
                        </th>
                        <th style="width:60px;" align="left">
                            <div><%=GetLabel("Jumlah Paket")%></div>
                        </th>
                        <th style="width:60px;" align="left">
                            <div><%=GetLabel("Jumlah Ditagihkan")%></div>
                        </th>
                        <th style="width:60px;" align="left">
                            <div><%=GetLabel("Satuan")%></div>
                        </th>
                        <th style="width:100px;" align="left">
                        </th>
                    </tr>
                    <tr align="center" style="height:50px; vertical-align:middle;">
                        <td colspan="3"><%=GetLabel("Tidak ada informasi order")%></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdNormal notAllowSelect grdPrescriptioOrderDt" cellspacing="0" rules="all">
                    <tr>
                        <th style="width:40px;" align="center"><input id="chkSelectAll" type="checkbox" /></th>
                        <th align="left" style="padding:3px">
                            <div><%=GetLabel("Nama Obat")%></div>
                            <div><div style="float:left;"><%=GetLabel("Signa")%></div>
                        </th>
                        <th style="width:80px;" align="left">
                            <div><%=GetLabel("Rak")%></div>
                        </th>
                        <th style="width:80px;" align="right">
                            <div><%=GetLabel("Jumlah Order")%></div>
                        </th>
                        <th style="width:60px;" align="right">
                            <div><%=GetLabel("Jumlah Paket")%></div>
                        </th>
                        <th style="width:60px;" align="right">
                            <div><%=GetLabel("Jumlah Ditagihkan")%></div>
                        </th>
                        <th style="width:60px;" align="left">
                            <div><%=GetLabel("Satuan")%></div>
                        </th>
                        <th style="width:100px;" align="left">
                        </th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder" ></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="keyField"><%#:Eval("PrescriptionOrderDetailID")%></td>
                    <td style="vertical-align:middle; text-align:center;"><asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" /></td>
                    <td style="vertical-align:middle; padding:3px">
                        <div id="divItemName" runat="server" style="font-weight:bold"><%#: Eval("cfMedicationName")%></div>
                        <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'><%#: Eval("cfCompoundDetail")%></div>
                        <div><%#: Eval("cfConsumeMethod2")%></div>
                        <div><%#: Eval("Route")%></div>
                        <div><%#: Eval("MedicationAdministration")%></div>
                    </td>
                    <td style="vertical-align:middle; padding:3px">
                        <%#: Eval("BinLocationName")%> 
                    </td>
                    <td style="vertical-align:middle; text-align:right">
                        <asp:TextBox ID="txtOrderQuantity" Width="60px" runat="server" CssClass="txtOrderQuantity txtNumeric" ReadOnly="true" Text='<%#: Eval("DispenseQtyInString")%>' />                        
                    </td>
                    <td style="vertical-align:middle; text-align:right;">
                        <asp:TextBox ID="txtTakenQty1" Width="60px" runat="server" CssClass="txtTakenQty1 txtNumeric" ReadOnly="true" />
                   
                    </td>
                    <td style="vertical-align:middle; text-align:right;">
                        <asp:TextBox ID="txtTakenQty2" Width="60px" runat="server" CssClass="txtTakenQty2 txtNumeric" ReadOnly="true" />              
                    </td>
                    <td style="vertical-align:middle; padding:3px">
                        <%#: Eval("ItemUnit")%>
                    </td>
                    <td style="vertical-align:middle; padding:3px">
                        <%#: Eval("PrescriptionOrderStatus")%> 
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>

<div id="divProcessButton" runat="server" style="width:100%;text-align:right;padding-top:10px">
    <table id="tblApproveDecline" runat="server" width="100%">
        <tr>
            <td class="tdLabel"><input class="w3-btn w3-hover-blue" type="button" id="btnProcess" value='<%= GetLabel("Process")%>' style="width:100px;" /></td>
            <td style="width:350px;">
                <asp:RadioButtonList ID="rblProcessType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text=" Dikerjakan" Value="1" Selected="True" />
                    <asp:ListItem Text=" Tidak Diambil Pasien" Value="2"  />
                    <asp:ListItem Text=" Dibatalkan" Value="3"  />
                </asp:RadioButtonList>
            </td>
            <td  colspan="3">
                <table id="tblVoidReason" runat="server" style="width:100%;">
                    <tr>
                        <td class="tdLabel" style="font-weight:bold;color:Red"><%=GetLabel("Alasan Batal") %></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="100%">
                                <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }"
                                    ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td style="width:140px"><asp:TextBox ID="txtVoidReason" runat="server" Width="100%" Style="display:none" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width:80px;">
                <input class="w3-btn w3-hover-blue" type="button" value='<%= GetLabel("Calculate")%>' id="btnCalculate" style="width:100px;"" />
            </td>
            <td>
                <div id="divTotalTransaction" runat="server" style="font-weight:bold">   
                    <table border="0" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col width="60px" />
                            <col width="100px" />
                            <col width="60px" />
                            <col width="100px" />
                            <col width="60px" />
                            <col width="100px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Total") %></td>
                            <td style="padding-left:2px; padding-left:5px">
                                <asp:TextBox ID="txtTotalAmount" Width="100px" runat="server" CssClass="txtCurrency" ReadOnly="true" TabIndex="999" />
                            </td>
                            <td class="tdLabel"><%=GetLabel("Instansi") %></td>
                            <td style="padding-left:2px; padding-left:5px">
                                <asp:TextBox ID="txtTotalPayer" Width="100px" runat="server"  CssClass="txtCurrency" ReadOnly="true" TabIndex="999"/>
                            </td>
                            <td class="tdLabel"><%=GetLabel("Pasien") %></td>
                            <td style="padding-left:2px; padding-left:5px">
                                <asp:TextBox ID="txtTotalPatient" Width="100px" runat="server"  CssClass="txtCurrency" ReadOnly="true" TabIndex="999"/>
                            </td>
                        </tr>
                    </table>                    
                </div>
            </td>
            <td>
            </td>
            <td style="width:80px;">
                <input class="w3-btn w3-hover-blue" type="button" value='<%= GetLabel("Close")%>' id="btnTestOrderClose" style="width:100px;" />
            </td>
        </tr>
    </table>

    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpCalculateItem" runat="server" Width="100%" ClientInstanceName="cbpCalculateItem"
            ShowLoadingPanel="false" OnCallback="cbpCalculateItem_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpCalculateItemViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
<dx:ASPxPopupControl ID="pcNotes" runat="server" ClientInstanceName="pcNotes" Height="150px"
    HeaderText="Drug Alert Information" CloseAction="None" Width="650px" Modal="True" PopupAction="None"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseButtonImage-Width="0">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" ID="pccc1">
            <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <div style="text-align: center; width: 100%;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col />
                                    <col style="width: 10px" />
                                    <col style="width: 36%" />
                                </colgroup>
                                <tr id="Tr1" style="height: 300px" runat="server">
                                    <td style="height: 450px; width: 100%; vertical-align: top">
                                        <div id="rightPanelCtl">
                                            <ul style="display:none">
                                                <li url="~/Libs/Program/Information/DrugInteractionSummaryViewCtl.ascx" title="Drug Alert">
                                                    Drug Alert</li>
                                            </ul>
                                            <div id="containerLeftPanelContent">
                                            </div>
                                            <div id="divLeftPanelContentLoading" style="position: absolute; top: 30%; left: 48%;
                                                display: none">
                                                <div style="margin: 0 auto">
                                                    <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="text-align:center;width:100%;">                           
                            <table style="margin-left:auto;margin-right:auto;margin-top:5px;">
                                <tr>
                                    <td><input type="button" id="btnClose" style="width:100px" value='<%= GetLabel("Close")%>' /></td>
                                </tr>
                            </table> 
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
