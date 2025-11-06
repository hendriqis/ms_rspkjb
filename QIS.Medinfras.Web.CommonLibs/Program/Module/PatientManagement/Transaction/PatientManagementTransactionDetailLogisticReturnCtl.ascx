<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientManagementTransactionDetailLogisticReturnCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientManagementTransactionDetailLogisticReturnCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ptdrgmsctl">
    function onLoadLogisticReturn() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblLogisticReturnQuickPick').show();

            if ($('#<%=hdnIsAIOTransactionLogisticReturnCtl.ClientID %>').val() == '1' || $('#<%=hdnIsChargesGenerateMCULogisticReturnCtl.ClientID %>').val() == '1') {
                $('#lblLogisticReturnQuickPick').hide();
            }
        }
        else {
            $('#lblLogisticReturnQuickPick').hide();
        }

        $('#btnLogisticReturnSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxLogisticReturn', 'mpTrxLogisticReturn'))
                cbpLogisticReturn.PerformCallback('save');
            return false;
        });

        $('#btnLogisticReturnCancel').click(function () {
            $('#containerEntryLogisticReturn').hide();

            var totalAllPayer = parseFloat($('#<%=hdnTempTotalPayer.ClientID %>').val());
            $('#<%=hdnLogisticReturnAllTotalPayer.ClientID %>').val(totalAllPayer);
            $('.tdLogisticReturnTotalPayer').html(totalAllPayer.formatMoney(2, '.', ','));

            var totalAllPatient = parseFloat($('#<%=hdnTempTotalPatient.ClientID %>').val());
            $('#<%=hdnLogisticReturnAllTotalPatient.ClientID %>').val(totalAllPatient);
            $('.tdLogisticReturnTotalPatient').html(totalAllPatient.formatMoney(2, '.', ','));

            var totalLineAmount = totalAllPatient + totalAllPayer;
            $('.tdLogisticReturnTotal').html(totalLineAmount.formatMoney(2, '.', ','));

            calculateAllTotal();
        });

        $('#lblLogisticReturnQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/LogisticsReturnQuickPicksCtl.ascx');
                var visitID = getVisitID();
                var locationID = getLogisticLocationID();
                var transactionID = getTransactionHdID();
                var departmentID = getDepartmentID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = visitID + '|' + locationID + '|' + transactionID + '|' + departmentID + '|' + serviceUnitID + '|' + registrationID + '|' + isAccompany;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });

        $('#<%=txtLogisticReturnPatient.ClientID %>').change(function () {
            var patientTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtLogisticReturnTotal.ClientID %>').attr('hiddenVal'));
            var payerTotal = total - patientTotal;
            $('#<%=txtLogisticReturnPayer.ClientID %>').val(payerTotal).trigger('changeValue');
        });

        $('#<%=txtLogisticReturnPayer.ClientID %>').change(function () {
            var payerTotal = parseFloat($(this).val());
            var total = parseFloat($('#<%=txtLogisticReturnTotal.ClientID %>').attr('hiddenVal'));
            var patientTotal = total - payerTotal;
            $('#<%=txtLogisticReturnPatient.ClientID %>').val(patientTotal).trigger('changeValue');
        });
    }

    function onLogisticReturnAIOClicked(param) {
        if (param) {
            $('#lblLogisticReturnQuickPick').hide();
        }
        else {
            $('#lblLogisticReturnQuickPick').show();
        }
    }

    $('.imgLogisticReturnSwitch.imgLink').die('click');
    $('.imgLogisticReturnSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpLogisticReturn.PerformCallback('switch|' + obj.ID);
    });

    $('.imgLogisticReturnApprove.imgLink').die('click');
    $('.imgLogisticReturnApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogisticReturn.PerformCallback('approve|' + obj.ID);
    });

    $('.imgLogisticReturnVoid.imgLink').die('click');
    $('.imgLogisticReturnVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogisticReturn.PerformCallback('void|' + obj.ID);
    });

    $('.imgLogisticReturnDelete.imgLink').die('click');
    $('.imgLogisticReturnDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpLogisticReturn.PerformCallback(param);
        });
    });

    $('.imgLogisticReturnEdit.imgLink').die('click');
    $('.imgLogisticReturnEdit.imgLink').live('click', function () {
        $('#containerEntryLogisticReturn').show();

        $('#<%=hdnTempTotalPatient.ClientID %>').val(getLogisticReturnTotalPatient());
        $('#<%=hdnTempTotalPayer.ClientID %>').val(getLogisticReturnTotalPayer());
        showLoadingPanel();
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);

        $('#<%=txtLogisticReturnLocationName.ClientID %>').val(obj.LocationName);
        $('#<%=txtLogisticReturnItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtLogisticReturnItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnLogisticReturnTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtLogisticReturnPatient.ClientID %>').val(obj.PatientAmount).trigger('changeValue');
        $('#<%=txtLogisticReturnPayer.ClientID %>').val(obj.PayerAmount).trigger('changeValue');
        $('#<%=txtLogisticReturnTotal.ClientID %>').val(obj.LineAmount).trigger('changeValue');
        $('#<%=txtLogisticReturnChargeClass.ClientID %>').val(obj.ChargeClassName);
        $('#<%=txtLogisticReturnQtyUoM.ClientID %>').val(obj.ItemUnit);

        $('#<%=txtLogisticReturnUnitTariff.ClientID %>').val(parseFloat(obj.Tariff)).trigger('changeValue');
        $('#<%=txtLogisticReturnQtyUsed.ClientID %>').val(obj.UsedQuantity);
        $('#<%=txtLogisticReturnQtyCharged.ClientID %>').val(obj.ChargedQuantity);
        $('#<%=txtLogisticReturnBaseQty.ClientID %>').val(obj.BaseQuantity);
        $('#<%=txtLogisticReturnPriceTariff.ClientID %>').val(parseFloat(obj.GrossLineAmount)).trigger('changeValue');
        $('#<%=txtLogisticReturnPriceDiscount.ClientID %>').val(obj.DiscountAmount).trigger('changeValue');
        $('#<%=txtLogisticReturnConversion.ClientID %>').val('1 ' + obj.ItemUnit + ' = ' + obj.ConversionFactor + ' ' + obj.BaseUnit);
        hideLoadingPanel();
    });

    function onCbpLogisticReturnEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryLogisticReturn').hide();
                setCustomToolbarVisibility();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail')
                showToast('Void Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
        }

        if ($('#<%=hdnIsAIOTransactionLogisticReturnCtl.ClientID %>').val() == '1' || $('#<%=hdnIsChargesGenerateMCULogisticReturnCtl.ClientID %>').val() == '1') {
            $('#lblLogisticReturnQuickPick').hide();
        } else {
            $('#lblLogisticReturnQuickPick').show();
        }

        calculateAllTotal();
        hideLoadingPanel();
    }

    function getLogisticReturnTotalPatient() {
        return parseFloat($('#<%=hdnLogisticReturnAllTotalPatient.ClientID %>').val());
    }
    function getLogisticReturnTotalPayer() {
        return parseFloat($('#<%=hdnLogisticReturnAllTotalPayer.ClientID %>').val());
    }
</script>
<input type="hidden" id="hdnIsAIOTransactionLogisticReturnCtl" runat="server" value="" />
<input type="hidden" id="hdnIsChargesGenerateMCULogisticReturnCtl" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPatient" runat="server" value="" />
<input type="hidden" id="hdnTempTotalPayer" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnLogisticReturnTransactionDtID" runat="server" value="" />
<div id="containerEntryLogisticReturn" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah / Ubah Data")%></div>
    <fieldset id="fsTrxLogisticReturn" style="margin: 0">
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 100px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                            <col style="width: 200px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtLogisticReturnLocationName" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Obat")%></label>
                            </td>
                            <td colspan="3">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLogisticReturnItemCode" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLogisticReturnItemName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Kelas Tagihan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnChargeClass" runat="server" Width="200px" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Harga Satuan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnUnitTariff" ReadOnly="true" CssClass="txtCurrency"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Digunakan")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Dibebankan")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Satuan")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnQtyUsed" Width="100%" ReadOnly="true" CssClass="number min"
                                    min="0.01" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnQtyCharged" Width="100%" ReadOnly="true" CssClass="number min"
                                    min="0.01" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnQtyUoM" Width="100%" ReadOnly="true" CssClass="number min"
                                    min="0.01" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Jumlah")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Konversi")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah Satuan Kecil")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnBaseQty" ReadOnly="true" CssClass="number" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnConversion" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Harga")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Diskon")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Harga")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnPriceTariff" ReadOnly="true" CssClass="txtCurrency"
                                    Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnPriceDiscount" ReadOnly="true" CssClass="txtCurrency"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Pasien")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Instansi")%></div>
                            </td>
                            <td>
                                <div class="lblComponent">
                                    <%=GetLabel("Total")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Total")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnPatient" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnPayer" CssClass="txtCurrency" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticReturnTotal" ReadOnly="true" CssClass="txtCurrency" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="button" id="btnLogisticReturnSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnLogisticReturnCancel" value='<%= GetLabel("Cancel")%>' />
                            </td>
                        </tr>
                    </table>
                    <img style="float: left; margin-right: 10px;" src='<%= ResolveUrl("~/Libs/Images/Button/info.png")%>'
                        alt='' />
                    <label class="lblInfo">
                        Obat dan Alkes yang di-Save akan langsung di-Approve.</label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpLogisticReturn" runat="server" Width="100%" ClientInstanceName="cbpLogisticReturn"
    ShowLoadingPanel="false" OnCallback="cbpLogisticReturn_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpLogisticReturnEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <input type="hidden" id="hdnLogisticReturnAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnLogisticReturnAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwLogisticReturn" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdLogisticReturn grdNormal notAllowSelect"
                            cellspacing="0" rules="all">
                            <tr>
                                <th style="width: 80px" rowspan="2">
                                </th>
                                <th rowspan="2">
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width: 70px">
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Kelas Tagihan")%>
                                    </div>
                                </th>
                                <th style="width: 80px" rowspan="2">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Harga Satuan")%>
                                    </div>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("Jumlah")%>
                                </th>
                                <th colspan="2" style="display: none">
                                    <%=GetLabel("Jumlah Satuan Kecil")%>
                                </th>
                                <th rowspan="2" style="width: 55px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Harga")%>
                                    </div>
                                </th>
                                <th rowspan="2" style="width: 55px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Diskon")%>
                                    </div>
                                </th>
                                <th colspan="3">
                                    <%=GetLabel("Total")%>
                                </th>
                                <th rowspan="2" style="width: 90px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th>
                                <th rowspan="2">
                                    &nbsp;
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 50px">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Digunakan")%>
                                    </div>
                                </th>
                                <th style="width: 50px">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Dibebankan")%>
                                    </div>
                                </th>
                                <th style="width: 70px">
                                    <div style="text-align: left; padding-right: 3px">
                                        <%=GetLabel("Satuan")%>
                                    </div>
                                </th>
                                <th style="width: 50px; display: none">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Jumlah")%>
                                    </div>
                                </th>
                                <th style="width: 150px; display: none">
                                    <div style="text-align: center; padding-right: 3px">
                                        <%=GetLabel("Konversi")%>
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
                            <tr id="Tr1" class="trFooter" runat="server">
                                <td colspan="9" align="right" style="padding-right: 3px">
                                    <%=GetLabel("TOTAL") %>
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdLogisticReturnTotalPayer" class="tdLogisticReturnTotalPayer"
                                    runat="server">
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdLogisticReturnTotalPatient" class="tdLogisticReturnTotalPatient"
                                    runat="server">
                                </td>
                                <td align="right" style="padding-right: 9px" id="tdLogisticReturnTotal" class="tdLogisticReturnTotal"
                                    runat="server">
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 24px">
                                                <img class="imgLogisticReturnVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%>
                                                    title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                                <img class="imgLogisticReturnApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                    alt="" />
                                                <img class="imgLogisticReturnVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgLogisticReturnEdit imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgLogisticReturnDelete imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                    <input type="hidden" value='<%#: Eval("LocationID") %>' bindingfield="LocationID" />
                                    <input type="hidden" value='<%#: Eval("LocationName") %>' bindingfield="LocationName" />
                                    <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                    <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                    <input type="hidden" value='<%#: Eval("ChargeClassName") %>' bindingfield="ChargeClassName" />
                                    <input type="hidden" value='<%#: Eval("GCItemUnit") %>' bindingfield="GCItemUnit" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("BaseQuantity") %>' bindingfield="BaseQuantity" />
                                    <input type="hidden" value='<%#: Eval("UsedQuantity") %>' bindingfield="UsedQuantity" />
                                    <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                    <input type="hidden" value='<%#: Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                    <input type="hidden" value='<%#: Eval("Tariff") %>' bindingfield="Tariff" />
                                    <input type="hidden" value='<%#: Eval("ConversionFactor") %>' bindingfield="ConversionFactor" />
                                    <input type="hidden" value='<%#: Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                    <input type="hidden" value='<%#: Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                    <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                    <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                    <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                    <input type="hidden" value='<%#: Eval("GCBaseUnit") %>' bindingfield="GCBaseUnit" />
                                    <input type="hidden" value='<%#: Eval("ItemUnit") %>' bindingfield="ItemUnit" />
                                    <input type="hidden" value='<%#: Eval("BaseUnit") %>' bindingfield="BaseUnit" />
                                    <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("ItemName1")%></div>
                                    <div>
                                        <span style="font-style: italic">
                                            <%#: Eval("ItemCode") %>
                                        </span>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("ChargeClassName")%></div>
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
                                        <%#: Eval("UsedQuantity")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("ChargedQuantity")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("ItemUnit")%></div>
                                </div>
                            </td>
                            <td style="display: none">
                                <div style="padding: 3px; text-align: right">
                                    <div>
                                        <%#: Eval("BaseQuantity")%></div>
                                </div>
                            </td>
                            <td style="display: none">
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("Conversion")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("GrossLineAmount", "{0:N}")%></div>
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
                                <div style="padding-right: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("CreatedByUserName")%></div>
                                    <div>
                                        <%#: Eval("CreatedDateInString")%></div>
                                </div>
                            </td>
                            <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %>
                                valign="middle">
                                <img style="margin-left: 2px" class="imgLogisticReturnSwitch imgLink" title='<%=GetLabel("Switch")%>'
                                    src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>' alt="" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <span class="lblLink" id="lblLogisticReturnQuickPick">
                        <%= GetLabel("Quick Pick")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
