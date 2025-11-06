<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionReturnOrderNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionReturnOrderNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#<%=tblVoidReason.ClientID %>').hide();

    $('#<%=rblProcessType.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%=tblVoidReason.ClientID %>').hide();
            $('#<%=hdnProcessType.ClientID %>').val('approve');
        }
        else {
            $('#<%=tblVoidReason.ClientID %>').show();
            $('#<%=hdnProcessType.ClientID %>').val('decline');
        }
    });

    function onCboVoidReasonValueChanged(s) {
        if (s.GetValue() == 'X221^999')
            $('#<%=txtVoidReason.ClientID %>').show();
        else
            $('#<%=txtVoidReason.ClientID %>').hide();
    }

    $('#btnProcess').live('click', function (evt) {
        var param = "";
        $('.grdPrescriptioOrderDt .chkIsSelected input:checked').each(function () {
            var PrescriptionReturnOrderDetailID = $(this).closest('tr').find('.keyField').html().trim();
            if (param != "") {
                param += ',';
            }
            param += PrescriptionReturnOrderDetailID;
        });
        $('#<%=hdnLstSelected.ClientID %>').val(param);

        if ($('#<%=hdnLstSelected.ClientID %>').val() == "") {
            var messageBody = "Harus ada item yang dipilih untuk diproses";
            displayMessageBox('Konfirmasi Order Retur', messageBody);
        }
        else {
            cbpEntryPopupView.PerformCallback($('#<%=hdnProcessType.ClientID %>').val());
        }
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else {
                onLoadObject(s.cpRetval);
                pcRightPanelContent.Hide();
            }
        } else if (param[0] == 'decline') {
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else {
                if (s.cpRetval != null && s.cpRetval != "") {
                    onLoadObject(s.cpRetval);
                }
                pcRightPanelContent.Hide();
            }
        }
    }
</script>
<input type="hidden" id="hdnChargeClassID" value="" runat="server" />
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<input type="hidden" id="hdnVisitID" value="" runat="server" />
<input type="hidden" id="hdnDepartmentID" value="" runat="server" />
<input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" />
<input type="hidden" id="hdnImagingServiceUnitID" runat="server" />
<input type="hidden" id="hdnPrescriptionReturnOrderID" value="" runat="server" />
<input type="hidden" id="hdnChargesTransactionID" value="" runat="server" />
<input type="hidden" id="hdnLstSelected" value="" runat="server" />
<input type="hidden" id="hdnProcessType" value="approve" runat="server" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
<input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
<div style="height: 450px; overflow-y: auto;">
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 180px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Order Retur")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPrescriptionReturnOrderNo" Width="205px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" id="lblServiceUnit">
                    <%=GetLabel("Tanggal Order")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtOrderDate" Width="120px" runat="server" Style="text-align: center" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderTime" Width="80px" runat="server" Style="text-align: center" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dokter / Paramedis")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtParamedic" Width="296px" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Lokasi")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboPrescriptionReturnOrderLocation" ClientInstanceName="cboPrescriptionReturnOrderLocation"
                    Width="300px" Enable="False" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Status Transaksi Order Retur")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtTransactionStatus" Width="205px" runat="server" ReadOnly="true" />
            </td>
        </tr>
    </table>
    <div>
        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
            <EmptyDataTemplate>
                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                    rules="all">
                    <tr>
                        <th>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 40px; text-align: center">
                            <input id="chkSelectAll" type="checkbox" />
                        </th>
                        <th align="left">
                            <%=GetLabel("Nama Item") %>
                        </th>
                        <th style="width: 90px;" align="right">
                            <%=GetLabel("Jumlah diretur") %>
                        </th>
                    </tr>
                    <tr align="center" style="height: 50px; vertical-align: middle;">
                        <td colspan="5">
                            <%=GetLabel("Tidak ada informasi obat yang diretur")%>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdNormal notAllowSelect grdPrescriptioOrderDt"
                    cellspacing="0" rules="all">
                    <tr>
                        <th style="width: 40px; text-align: center">
                            <input id="chkSelectAll" type="checkbox" />
                        </th>
                        <th align="left">
                            <%=GetLabel("Nama Item") %>
                        </th>
                        <th style="width: 100px;" align="right">
                            <%=GetLabel("Jumlah diretur") %>
                        </th>
                        <th style="width: 150px;" align="center">
                            <%=GetLabel("Status") %>
                        </th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="keyField">
                        <%#:Eval("PrescriptionReturnOrderDtID")%>
                    </td>
                    <td style="vertical-align: middle; text-align: center;">
                        <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                    </td>
                    <td>
                        <%#:Eval("ItemName1")%>
                        <%#:Eval("ItemUnit") %>
                    </td>
                    <td align="right">
                        <%#:Eval("ItemQty") %>
                    </td>
                    <td align="center">
                        <%#:Eval("PrescriptionReturnOrderStatus")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <div style="width: 100%">
        <table>
            <tr>
                <td class="tdLabel">
                    <input class="w3-btn w3-hover-blue" type="button" id="btnProcess" value='<%= GetLabel("Process")%>'
                        style="background-color: Red; width: 100px;" />
                </td>
                <td style="width: 350px;">
                    <asp:RadioButtonList ID="rblProcessType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text=" Dikerjakan" Value="1" Selected="True" />
                        <asp:ListItem Text=" Dibatalkan" Value="2" />
                    </asp:RadioButtonList>
                </td>
                <td>
                    <table id="tblVoidReason" runat="server" style="width: 100%;">
                        <tr>
                            <td class="tdLabel" style="font-weight: bold; color: Red; width:100px">
                                <%=GetLabel("Alasan Batal") %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="100%">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td style="width: 140px">
                                <asp:TextBox ID="txtVoidReason" runat="server" Width="100%" Style="display: none" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
