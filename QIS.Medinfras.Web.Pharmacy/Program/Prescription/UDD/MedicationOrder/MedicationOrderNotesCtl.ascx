<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderNotesCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_MedicationOrderDtCtl">
    $('#<%=txtPrescriptionOrderNo.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtPrescriptionOrderDate.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtPrescriptionOrderTime.ClientID %>').attr('readonly', 'readonly');
    $('#<%=txtParamedic.ClientID %>').attr('readonly', 'readonly');

    setDatePicker('<%=txtTransactionDate.ClientID %>');
    $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#btnOrderApprove').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntryPopup')) {
            if ($('#<%=hdnIsOrderDetailExists.ClientID %>').val() == "1") {
                var param = "";
                var selectedUDD = "";
                $('.grdPrescriptioOrderDt .chkIsSelected input:checked').each(function () {
                    var prescriptionOrderDetailID = $(this).closest('tr').find('.keyField').html();
                    var isProcessToUDD = '0';
                    if ($(this).closest('tr').find('.chkIsUsingUDD input').is(':checked')) {
                        isProcessToUDD = '1';
                    }

                    if (param != "") {
                        param += ',';
                        selectedUDD += ',';
                    }
                    param += prescriptionOrderDetailID;
                    selectedUDD += isProcessToUDD;
                });
                $('#<%=hdnLstSelected.ClientID %>').val(param);
                $('#<%=hdnLstSelectedUDD.ClientID %>').val(selectedUDD);
                if (param == "") showToast('Konfirmasi Gagal', 'Paling tidak harus ada 1 Item yang dipilih (check)');
                else cbpEntryPopupView.PerformCallback('approve');
            }
            else {
                cbpEntryPopupView.PerformCallback('approve');
            }
        }
    });

    $('#btnOrderDecline').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntryPopup')) {
            if ($('#<%=hdnIsOrderDetailExists.ClientID %>').val() == "1") {
                var param = "";
                $('.grdPrescriptioOrderDt .chkIsSelected input:checked').each(function () {
                    var prescriptionOrderDetailID = $(this).closest('tr').find('.keyField').html();
                    if (param != "")
                        param += ',';
                    param += prescriptionOrderDetailID;
                });
                $('#<%=hdnLstSelected.ClientID %>').val(param);
                if (param == "") showToast('Proses Gagal', 'Paling tidak harus ada 1 Item yang dipilih (check)');
                else cbpEntryPopupView.PerformCallback('decline');
            }
            else {
                cbpEntryPopupView.PerformCallback('decline');
            }
        }
    });

    $('#btnTestOrderClose').click(function (evt) {
        if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '' || $('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '0') {
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
        if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Proses Pelayanan Resep Gagal', 'Error Message : ' + param[2]);
            else {
                if ($('#<%=hdnPrescriptionOrderID.ClientID %>').val() == '')
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

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
        $('.chkIsUsingUDD').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $('.chkIsSelected input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        $tr.find('.chkIsUsingUDD input').prop('checked', isChecked);
    });

    function onCboVoidReasonValueChanged(s) {
        if (s.GetValue() == 'X221^999')
            $('#<%=txtVoidReason.ClientID %>').show();
        else
            $('#<%=txtVoidReason.ClientID %>').hide();
    }
</script>
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
<input type="hidden" id="hdnLstSelected" value="" runat="server" />
<input type="hidden" id="hdnLstSelectedUDD" value="" runat="server" />
<input type="hidden" id="hdnVisitInfo" value="" runat="server" />
<input type="hidden" id="hdnOrderHdInfo" value="" runat="server" />
<input type="hidden" id="hdnIsHasCompound" value="0" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
<div style="height:500px;overflow-y:auto;">
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>      
            <td style="padding:5px;vertical-align:top">
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
                            <td colspan="5"><dxe:ASPxComboBox runat="server" ID="cboPrescriptionOrderLocation" ClientInstanceName="cboPrescriptionOrderLocation" Width="100%" /></td>
                        </tr>
                        <tr>
                            <td />
                            <td />
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Order Resep")%></label></td>
                            <td colspan="2"><asp:TextBox ID="txtPrescriptionOrderNo" Width="100%" runat="server" /></td> 
                            <td class="tdLabel"><label class="lblNormal" id="lblServiceUnit"><%=GetLabel("Tanggal Order")%></label></td>
                            <td><asp:TextBox ID="txtPrescriptionOrderDate" Width="120px" runat="server" Style="" /></td>
                            <td style="padding-left:10px"><asp:TextBox ID="txtPrescriptionOrderTime" ReadOnly="true" Width="60px" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1"><%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="5"> 
                                <asp:TextBox ID="txtParamedic" Width="100%" runat="server" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top:5px;"><label><%=GetLabel("Catatan Order")%></label></td>
                            <td colspan="5"><asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="110px" runat="server"/></td>
                        </tr>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel"><%=GetLabel("Tanggal") %> / <%=GetLabel("Jam Diproses") %></td>
                            <td><asp:TextBox ID="txtTransactionDate" Width="100px" runat="server" CssClass="datepicker" style="text-align:center" /></td>
                            <td><asp:TextBox ID="txtTransactionTime" Width="60px" CssClass="time" runat="server" Style="text-align:center" /></td>
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
                        </th>
                    </tr>
                    <tr align="center" style="height:50px; vertical-align:middle;">
                        <td colspan="2"><%=GetLabel("No Data To Display")%></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdNormal notAllowSelect grdPrescriptioOrderDt" cellspacing="0" rules="all">
                    <tr>
                        <th style="width:40px;" align="center"><input id="chkSelectAll" type="checkbox" /></th>
                        <th style="width:40px;" align="center">UDD</th>
                        <th align="left" style="padding:3px">
                            <div><%=GetLabel("generik")%> - <%=GetLabel("Nama Obat")%> - <%=GetLabel("Kadar")%> - <%=GetLabel("Bentuk")%></div>
                            <div><div style="color:Blue;width:42px;float:left;"><%=GetLabel("DOSIS")%></div> <%=GetLabel("dosis")%> - <%=GetLabel("Rute")%> - <%=GetLabel("Frekuensi")%></div>
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
                    <td style="vertical-align:middle; text-align:center;"><asp:CheckBox runat="server" ID="chkIsUsingUDD" CssClass="chkIsUsingUDD" /></td>
                    <td style="padding:3px">
                        <div><%#: Eval("InformationLine1")%></div>
                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%> - <%#: Eval("Route")%> - <%#: Eval("cfDoseFrequency")%></div>
                        <div style="font-style:italic"><%#: Eval("MedicationLine")%> </div>
                        <div style="font-style:italic; color:red;font-weight:bold"><%#: Eval("MedicationAdministration")%> </div>
                    </td>
                    <td>
                        <%#: Eval("PrescriptionOrderStatus")%> 
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>

    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>

<div id="divProcessButton" runat="server" style="width:100%;text-align:right;padding-top:10px">
    <table id="tblApproveDecline" runat="server" width="100%">
        <tr>
            <td style="width:80px;">
                <input type="button" id="btnOrderApprove" value='<%= GetLabel("Process")%>' style="width:100px" />
            </td>
            <td>
            <table id="tblVoidReason" runat="server">
                <tr>
                    <td style="width:80px;">
                        <input type="button" id="btnOrderDecline" value='<%= GetLabel("Void")%>'  style="width:100px"/>
                    </td>
                    <td class="tdLabel"><%=GetLabel("Alasan Batal") %></td>
                    <td style="padding-left:10x">
                        <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="200px">
                            <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }"
                                ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                        </dxe:ASPxComboBox>
                    </td>
                    <td><asp:TextBox ID="txtVoidReason" runat="server" Width="150px" Style="display:none" /></td>
                </tr>
            </table>
            </td>
            <td style="width:80px;">
                <input type="button" value='<%= GetLabel("Close")%>' id="btnTestOrderClose" style="width:100px" />
            </td>
        </tr>
    </table>
</div>