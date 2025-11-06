<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryDiscountNewEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryDiscountNewEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_patientbilldiscountnewentryctl">

    function onCboDiscountReasonValueChanged() {
        var discountReason = cboDiscountReason.GetValue();

        if (discountReason == "X155^001") {
            $('#<%=trDoctors.ClientID %>').removeAttr('style');
        } else {
            $('#<%=trDoctors.ClientID %>').attr('style', 'display:none');
            $('#<%=trDoctors.ClientID %>').attr('style', 'display:none');
        }
    }

    $('#<%=chkPatientDiscPercent.ClientID %>').live('change', function () {
        if ($('#<%=chkPatientDiscPercent.ClientID %>').is(':checked')) {
            $('#<%=txtPatientEntryDiscountPercent.ClientID %>').removeAttr('readonly');
            $('#<%=txtPatientEntryDiscountPercent.ClientID %>').val("0").trigger('changeValue');
        } else {
            $('#<%=txtPatientEntryDiscountPercent.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtPatientEntryDiscountPercent.ClientID %>').val("0").trigger('changeValue');
        }
        calculateDiscountAmount();
    });

    $('#<%=chkPayerDiscPercent.ClientID %>').live('change', function () {
        if ($('#<%=chkPayerDiscPercent.ClientID %>').is(':checked')) {
            $('#<%=txtPayerEntryDiscountPercent.ClientID %>').removeAttr('readonly');
            $('#<%=txtPayerEntryDiscountPercent.ClientID %>').val("0").trigger('changeValue');
        } else {
            $('#<%=txtPayerEntryDiscountPercent.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtPayerEntryDiscountPercent.ClientID %>').val("0").trigger('changeValue');
        }
        calculateDiscountAmount();
    });

    $('#<%=txtPatientEntryDiscountPercent.ClientID %>').live('change', function () {
        calculateDiscountAmount();
    });

    $('#<%=txtPayerEntryDiscountPercent.ClientID %>').live('change', function () {
        calculateDiscountAmount();
    });

    function calculateDiscountAmount() {
        var totalPatient = parseFloat($('#<%=hdnTotalPatientAmount.ClientID %>').val());
        var totalPayer = parseFloat($('#<%=hdnTotalPayerAmount.ClientID %>').val());

        var patientDiscPercent = parseFloat($('#<%=txtPatientEntryDiscountPercent.ClientID %>').val());
        var payerDiscPercent = parseFloat($('#<%=txtPayerEntryDiscountPercent.ClientID %>').val());

        var patientDisc = 0;
        var payerDisc = 0;

        if (totalPatient != 0) {
            patientDisc = totalPatient * patientDiscPercent / 100;
        }

        if (totalPayer != 0) {
            payerDisc = totalPayer * payerDiscPercent / 100;
        }

        $('#<%=txtPatientEntryDiscountAmount.ClientID %>').val(patientDisc).trigger('changeValue');
        $('#<%=txtPayerEntryDiscountAmount.ClientID %>').val(payerDisc).trigger('changeValue');
    }

    $('#lblAddData').die('click');
    $('#lblAddData').live('click', function () {
        if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
            $('#containerEntry').show();
            showLoadingPanel();
            $('#<%=hdnID.ClientID %>').val("");
            cboDiscountReason.SetValue('');
            cboDoctors.SetValue('');
            $('#<%=txtReason.ClientID %>').val("");
            $('#<%=txtPatientEntryDiscountPercent.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtPayerEntryDiscountPercent.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtPatientEntryDiscountAmount.ClientID %>').val('0').trigger('changeValue');
            $('#<%=txtPayerEntryDiscountAmount.ClientID %>').val('0').trigger('changeValue');

            hideLoadingPanel();
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#containerEntry').show();
        showLoadingPanel();

        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);

        if (obj.GCDiscountReason == "X155^001") {
            $('#<%=trDoctors.ClientID %>').removeAttr('style');
        } else {
            $('#<%=trDoctors.ClientID %>').attr('style', 'display:none');
        }

        $('#<%=hdnID.ClientID %>').val(obj.ID);
        $('#<%=hdnPatientBillingID.ClientID %>').val(obj.PatientBillingID);
        cboDiscountReason.SetValue(obj.GCDiscountReason);
        cboDoctors.SetValue(obj.ParamedicID);
        $('#<%=txtReason.ClientID %>').val(obj.DiscountReason);
        $('#<%=txtPatientEntryDiscountAmount.ClientID %>').val(obj.PatientDiscountAmount).trigger('changeValue');
        $('#<%=txtPayerEntryDiscountAmount.ClientID %>').val(obj.PayerDiscountAmount).trigger('changeValue');

        hideLoadingPanel();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure?', function (result) {
            if (result) {
                var obj = rowToObject($row);
                cbp.PerformCallback('delete|' + obj.ID);
            }
        });
    });

    $('#btnSave').die('click');
    $('#btnSave').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxService', 'mpTrxService')) {
            var discountReason = cboDiscountReason.GetValue();
            var doctors = cboDoctors.GetValue();
            if (discountReason == null) {
                alert("Pilih Jenis Diskon Terlebih Dahulu");
            }
            else {
                if (discountReason == "X155^001") {
                    if (doctors != null && doctors != "") {
                        cbp.PerformCallback('save');
                    } else {
                        alert("Pilih Dokter Terlebih Dahulu");
                    }
                } else {
                    cbp.PerformCallback('save');
                }
            }
        }
        return false;
    });

    $('#btnCancel').die('click');
    $('#btnCancel').live('click', function () {
        //////        $('#<%=trReasonText.ClientID %>').attr('style', 'display:none');
        $('#<%=trDoctors.ClientID %>').attr('style', 'display:none');
        $('#<%=hdnID.ClientID %>').val("");
        cboDiscountReason.SetValue('');
        cboDoctors.SetValue('');
        $('#<%=txtReason.ClientID %>').val("");
        $('#<%=txtPatientEntryDiscountAmount.ClientID %>').val("");
        $('#<%=txtPayerEntryDiscountAmount.ClientID %>').val("");

        $('#containerEntry').hide();
        cbp.PerformCallback('refresh');
    });

    function onCbpEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var billingID = s.cpBillingID;
                onAfterSaveEditRecordEntryPopup(billingID);
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerEntry').hide();
        hideLoadingPanel();
    }

</script>
<input type="hidden" id="hdnPatientBillingID" runat="server" />
<input type="hidden" id="hdnTotalPatientAmount" runat="server" />
<input type="hidden" id="hdnTotalPayerAmount" runat="server" />
<input type="hidden" id="hdnIsDiscountComp2ValidateTariffComp2" runat="server" />
<input type="hidden" id="hdnID" runat="server" value="" />
<div style="height: 420px; overflow-y: auto;">
    <div>
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td style="padding-left: 5px">
                    <label class="lblNormal">
                        <%=GetLabel("No. Tagihan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientBillingNoCtl" Width="100%" runat="server" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </div>
    <div id="containerEntry" style="margin-top: 4px; display: none;">
        <fieldset id="fsTrx" style="margin: 0">
            <table class="tblEntryDetail" style="width: 100%">
                <colgroup>
                    <col style="width: 150px" />
                    <col style="width: 200px" />
                    <col style="width: 200px" />
                    <col style="width: 350px" />
                </colgroup>
                <tr>
                    <td style="padding-left: 5px">
                        <label class="lblMandatory">
                            <%=GetLabel("Jenis Diskon")%></label>
                    </td>
                    <td style="padding-left: 5px">
                        <dxe:ASPxComboBox ID="cboDiscountReason" ClientInstanceName="cboDiscountReason" Width="100%"
                            runat="server">
                            <ClientSideEvents ValueChanged="function(s,e){ onCboDiscountReasonValueChanged(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr id="trDoctors" runat="server" style="display: none">
                    <td style="padding-left: 5px">
                        <label class="lblMandatory" id="lblDokter">
                            <%=GetLabel("Dokter")%></label>
                    </td>
                    <td style="padding-left: 5px">
                        <dxe:ASPxComboBox ID="cboDoctors" ClientInstanceName="cboDoctors" Width="100%" runat="server">
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr id="trReasonText" runat="server">
                    <td style="padding-left: 5px">
                        <label class="lblNormal">
                            <%=GetLabel("Alasan")%></label>
                    </td>
                    <td style="padding-left: 5px">
                        <asp:TextBox ID="txtReason" Width="100%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 5px">
                        <label class="lblNormal">
                            <%=GetLabel("")%></label>
                    </td>
                    <td style="padding-left: 5px;" align="center">
                        <h4>
                            <%=GetLabel("PASIEN")%></h4>
                    </td>
                    <td style="padding-left: 5px;" align="center">
                        <h4>
                            <%=GetLabel("INSTANSI")%></h4>
                    </td>
                </tr>
                <tr id="trDiscountPercent" runat="server">
                    <td style="padding-left: 5px">
                        <label class="lblNormal">
                            <%=GetLabel("Diskon Final [%]")%>
                        </label>
                    </td>
                    <td align="right">
                        <table>
                            <colgroup>
                                <col style="width: 30px" />
                                <col style="width: 100px" />
                            </colgroup>
                            <tr>
                                <td align="right">
                                    <asp:CheckBox ID="chkPatientDiscPercent" Width="100%" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:TextBox ID="txtPatientEntryDiscountPercent" CssClass="txtCurrency" Width="100px"
                                        runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <colgroup>
                                <col style="width: 30px" />
                                <col style="width: 100px" />
                            </colgroup>
                            <tr>
                                <td align="right">
                                    <asp:CheckBox ID="chkPayerDiscPercent" Width="100%" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:TextBox ID="txtPayerEntryDiscountPercent" CssClass="txtCurrency" Width="100px"
                                        runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 5px">
                        <label class="lblNormal">
                            <%=GetLabel("Diskon Final [Rp]")%>
                        </label>
                    </td>
                    <td style="padding-left: 5px">
                        <asp:TextBox ID="txtPatientEntryDiscountAmount" CssClass="txtCurrency" Width="100%"
                            runat="server" />
                    </td>
                    <td style="padding-left: 5px">
                        <asp:TextBox ID="txtPayerEntryDiscountAmount" CssClass="txtCurrency" Width="100%"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <input style="width: 60px" type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                </td>
                                <td>
                                    <input style="width: 60px" type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbp" runat="server" Width="100%" ClientInstanceName="cbp"
        ShowLoadingPanel="false" OnCallback="cbp_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnl" Style="width: 100%; margin-left: auto; margin-right: auto;
                    position: relative; font-size: 0.95em;">
                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" runat="server"
                        id="hdnChargesDt" />
                    <asp:ListView ID="lvw" runat="server">
                        <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grd grdNormal notAllowSelect" cellspacing="0"
                                rules="all">
                                <tr>
                                    <th rowspan="2" style="width: 60px">
                                    </th>
                                    <th rowspan="2" style="width: 150px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Jenis Diskon")%>
                                        </div>
                                    </th>
                                    <th rowspan="2" style="width: 250px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Dokter")%>
                                        </div>
                                    </th>
                                    <th rowspan="2" style="width: 250px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Alasan")%>
                                        </div>
                                    </th>
                                    <th colspan="2">
                                        <div style="text-align: center; padding-right: 3px">
                                            <%=GetLabel("Diskon")%>
                                        </div>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 150px">
                                        <div style="text-align: right; padding-right: 3px">
                                            <%=GetLabel("Pasien")%>
                                        </div>
                                    </th>
                                    <th style="width: 150px">
                                        <div style="text-align: right; padding-right: 3px">
                                            <%=GetLabel("Instansi")%>
                                        </div>
                                    </th>
                                </tr>
                                <tr class="trEmpty">
                                    <td colspan="6">
                                        <%=GetLabel("No Data To Display") %>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblView" runat="server" class="grd grdNormal notAllowSelect" cellspacing="0"
                                rules="all">
                                <tr>
                                    <th rowspan="2" style="width: 60px">
                                    </th>
                                    <th rowspan="2" style="width: 150px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Jenis Diskon")%>
                                        </div>
                                    </th>
                                    <th rowspan="2" style="width: 250px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Dokter")%>
                                        </div>
                                    </th>
                                    <th rowspan="2" style="width: 250px">
                                        <div style="text-align: left; padding-right: 3px">
                                            <%=GetLabel("Alasan")%>
                                        </div>
                                    </th>
                                    <th colspan="2">
                                        <div style="text-align: center; padding-right: 3px">
                                            <%=GetLabel("Diskon")%>
                                        </div>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 150px">
                                        <div style="text-align: right; padding-right: 3px">
                                            <%=GetLabel("Pasien")%>
                                        </div>
                                    </th>
                                    <th style="width: 150px">
                                        <div style="text-align: right; padding-right: 3px">
                                            <%=GetLabel("Instansi")%>
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
                                    <div>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="margin-right: 2px" />
                                                </td>
                                                <td>
                                                    <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="margin-right: 2px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                        <input type="hidden" value='<%#: Eval("PatientBillingID") %>' bindingfield="PatientBillingID" />
                                        <input type="hidden" value='<%#: Eval("GCDiscountReason") %>' bindingfield="GCDiscountReason" />
                                        <input type="hidden" value='<%#: Eval("DiscountReason") %>' bindingfield="DiscountReason" />
                                        <input type="hidden" value='<%#: Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                        <input type="hidden" value='<%#: Eval("PatientDiscountAmount") %>' bindingfield="PatientDiscountAmount" />
                                        <input type="hidden" value='<%#: Eval("PayerDiscountAmount") %>' bindingfield="PayerDiscountAmount" />
                                    </div>
                                </td>
                                <td>
                                    <div style="padding: 3px">
                                        <div>
                                            <%#: Eval("DiscountReasonDetail")%></div>
                                    </div>
                                </td>
                                <td>
                                    <div style="padding: 3px">
                                        <div>
                                            <%#: Eval("ParamedicName")%></div>
                                    </div>
                                </td>
                                <td>
                                    <div style="padding: 3px">
                                        <div>
                                            <%#: Eval("DiscountReason")%></div>
                                    </div>
                                </td>
                                <td align="right">
                                    <div style="padding: 3px">
                                        <div>
                                            <%#: Eval("PatientDiscountAmount", "{0:N2}")%></div>
                                    </div>
                                </td>
                                <td align="right">
                                    <div style="padding: 3px">
                                        <div>
                                            <%#: Eval("PayerDiscountAmount", "{0:N2}")%></div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col style="width: 350px" />
                        </colgroup>
                        <tr>
                            <td style="padding-left: 5px">
                                <label class="lblNormal">
                                    <%=GetLabel("")%></label>
                            </td>
                            <td style="padding-left: 5px;" align="center">
                                <h4>
                                    <%=GetLabel("PASIEN")%></h4>
                            </td>
                            <td style="padding-left: 5px;" align="center">
                                <h4>
                                    <%=GetLabel("PAYER")%></h4>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 5px">
                                <label id="Label1" runat="server">
                                    <%=GetLabel("Total Tagihan")%></label>
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtTotalPatientAmount" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtTotalPayerAmount" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 5px">
                                <label class="lblNormal">
                                    <%=GetLabel("Diskon Final")%>
                                    [Rp]</label>
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtPatientDiscountAmountCtl" ReadOnly="true" CssClass="txtCurrency"
                                    Width="100%" runat="server" />
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtPayerDiscountAmountCtl" ReadOnly="true" CssClass="txtCurrency"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 5px">
                                <label id="Label2" runat="server">
                                    <%=GetLabel("Total Setelah Diskon")%></label>
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                            <td style="padding-left: 5px">
                                <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
