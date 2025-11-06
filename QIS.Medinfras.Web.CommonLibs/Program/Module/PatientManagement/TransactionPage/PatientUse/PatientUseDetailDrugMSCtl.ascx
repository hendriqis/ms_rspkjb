<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientUseDetailDrugMSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientUseDetailDrugMSCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ptdrgmsctl">
    //#region Drug MS
    function onLoadDrugMS() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblDrugMSQuickPick').show();
        }
        else {
            $('#lblDrugMSQuickPick').hide();
        }

        $('#btnDrugMSSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxDrugMS', 'mpTrxDrugMS'))
                cbpDrugMS.PerformCallback('save');
            return false;
        });

        $('#btnDrugMSCancel').click(function () {
            $('#containerEntryDrugMS').hide();
        });

        $('#lblDrugMSQuickPick').live('click', function () {
            $('#containerEntryDrugMS').hide();
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/DrugsLogisticsQuickPicksCtl.ascx');
                var transactionID = getTransactionHdID();
                var locationID = getLocationID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var GCItemType = getGCItemType();
                var departmentID = getDepartmentID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var drugTransactionType = $('#<%=hdnIsDrugChargesJustDistribution.ClientID %>').val();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = transactionID + '|' + locationID + '|' + visitID + '|' + registrationID + '|' + GCItemType + '|' + departmentID + '|' + serviceUnitID + '|' + isAccompany + '|' + drugTransactionType;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });

        $('#<%=txtDrugMSQtyUsed.ClientID %>').change(function () {
            var conversionValue = parseFloat($('#<%=hdnDrugMSConversionValue.ClientID %>').val());
            var qty = parseFloat($(this).val());
            $('#<%=txtDrugMSBaseQty.ClientID %>').val(conversionValue * qty);
        });
    }

    //#region Entry Drug MS

    $('.imgDrugMSApprove.imgLink').die('click');
    $('.imgDrugMSApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('approve|' + obj.ID);
    });

    $('.imgDrugMSVoid.imgLink').die('click');
    $('.imgDrugMSVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpDrugMS.PerformCallback('void|' + obj.ID);
    });

    var isEditDrugMS = false;

    $('.imgDrugMSDelete.imgLink').die('click');
    $('.imgDrugMSDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpDrugMS.PerformCallback(param);
        });
    });

    var objDrugMS = null;

    var isEditDrugMS = false;
    $('.imgDrugMSEdit.imgLink').die('click');
    $('.imgDrugMSEdit.imgLink').live('click', function () {
        $('#containerEntryDrugMS').show();
        cboDrugMSLocation.SetEnabled(false);
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cboDrugMSLocation.SetValue(obj.LocationID);
        $('#<%=hdnDrugMSItemID.ClientID %>').val(obj.ItemID);
        $('#<%=txtDrugMSItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtDrugMSItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnDrugMSTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtDrugMSQtyUsed.ClientID %>').val(obj.UsedQuantity);
        $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val(obj.GCBaseUnit);
        $('#<%=txtDrugMSBaseQty.ClientID %>').val(obj.BaseQuantity);
        $('#<%=hdnDrugMSConversionValue.ClientID %>').val(obj.ConversionFactor);
        $('#<%=txtDrugMSQtyUsed.ClientID %>').focus();
        objDrugMS = obj;
        isEditDrugMS = true;
        cboDrugMSUoM.PerformCallback();
    });
    //#endregion

    function onCboDrugMSUomEndCallback() {
        if (!isEditDrugMS) {
            cboDrugMSUoM.SetValue('');
        }
        else {
            cboDrugMSUoM.SetValue(objDrugMS.GCItemUnit);
            var conversionFactor = $('#<%=hdnDrugMSConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
            var currUoM = cboDrugMSUoM.GetValue();
            var fromConversion = getDrugMSItemUnitName(defaultUoM);
            var toConversion = getDrugMSItemUnitName(currUoM);
            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
        }
        hideLoadingPanel();
    }

    function setDrugMSItemUnitConversionText() {
        var defaultUoM = $('#<%=hdnDrugMSDefaultUoM.ClientID %>').val();
        var currUoM = cboDrugMSUoM.GetValue();
        var fromConversion = getDrugMSItemUnitName(defaultUoM);
        if (defaultUoM == currUoM) {
            $('#<%=hdnDrugMSConversionValue.ClientID %>').val('1');
            $('#<%=txtDrugMSQtyUsed.ClientID %>').change();
            var conversion = "1 " + fromConversion + " = 1 " + fromConversion;
            $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
        }
        else {
            var itemID = $('#<%=hdnDrugMSItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + currUoM + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                var toConversion = getDrugMSItemUnitName(currUoM);
                $('#<%=hdnDrugMSConversionValue.ClientID %>').val(result);
                $('#<%=txtDrugMSQtyUsed.ClientID %>').change();
                var conversion = "1 " + toConversion + " = " + result + " " + fromConversion;
                $('#<%=txtDrugMSConversion.ClientID %>').val(conversion);
            });
        }
    }

    function getDrugMSItemUnitName(itemUnitID) {
        var value = cboDrugMSUoM.GetValue();
        cboDrugMSUoM.SetValue(itemUnitID);
        var text = cboDrugMSUoM.GetText();
        cboDrugMSUoM.SetValue(value);
        return text;
    }

    function onCbpDrugMSEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryDrugMS').hide();
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
        hideLoadingPanel();
    }

    //#endregion

</script>
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnIsAllowOverIssued" runat="server" value="0" />
<input type="hidden" id="hdnDrugMSTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnIsDrugChargesJustDistribution" runat="server" value="0" />
<div id="containerEntryDrugMS" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah / Ubah Data")%></div>
    <fieldset id="fsTrxDrugMS" style="margin: 0">
        <table class="tblEntryDetail">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
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
                                <dxe:ASPxComboBox ID="cboDrugMSLocation" ClientInstanceName="cboDrugMSLocation" Width="100%"
                                    runat="server" OnCallback="cboDrugMSLocation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblDrugMSItem">
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
                                            <input type="hidden" value="" id="hdnDrugMSItemID" runat="server" />
                                            <asp:TextBox ID="txtDrugMSItemCode" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDrugMSItemName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
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
                                    <%=GetLabel("Satuan")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel lblMandatory">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSQtyUsed" Width="100%" CssClass="number min" min="0.1" runat="server" />
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnDrugMSDefaultUoM" runat="server" />
                                <input type="hidden" value="" id="hdnDrugMSConversionValue" runat="server" />
                                <dxe:ASPxComboBox ID="cboDrugMSUoM" runat="server" ClientInstanceName="cboDrugMSUoM"
                                    Width="200px" OnCallback="cboDrugMSUoM_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { onCboDrugMSUomEndCallback(); }" ValueChanged="function(s,e){ setDrugMSItemUnitConversionText(); }" />
                                </dxe:ASPxComboBox>
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
                                <asp:TextBox ID="txtDrugMSBaseQty" ReadOnly="true" CssClass="number" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrugMSConversion" ReadOnly="true" Width="100%" runat="server" />
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
                                <input type="button" id="btnDrugMSSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnDrugMSCancel" value='<%= GetLabel("Cancel")%>' />
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
<dxcp:ASPxCallbackPanel ID="cbpDrugMS" runat="server" Width="100%" ClientInstanceName="cbpDrugMS"
    ShowLoadingPanel="false" OnCallback="cbpDrugMS_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpDrugMSEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <input type="hidden" id="hdnDrugMSAllTotalPatient" runat="server" value="" />
                <input type="hidden" id="hdnDrugMSAllTotalPayer" runat="server" value="" />
                <asp:ListView ID="lvwDrugMS" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 80px" rowspan="2">
                                </th>
                                <th rowspan="2">
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th colspan="2" style="width: 200px">
                                    <%=GetLabel("JUMLAH")%>
                                </th>
                                <th rowspan="2" style="width: 230px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Petugas")%>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 100px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Digunakan")%>
                                    </div>
                                </th>
                                <th style="width: 100px">
                                    <div style="text-align: left; padding-right: 3px">
                                        <%=GetLabel("Satuan")%>
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
                                            <td style="width: 24px">
                                                <img class="imgDrugMSVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%>
                                                    title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                                <img class="imgDrugMSApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                    alt="" />
                                                <img class="imgDrugMSVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgDrugMSEdit imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgDrugMSDelete imgLink" <%# Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                    <input type="hidden" value='<%#: Eval("LocationID") %>' bindingfield="LocationID" />
                                    <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                    <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("GCItemUnit") %>' bindingfield="GCItemUnit" />
                                    <input type="hidden" value='<%#: Eval("BaseQuantity") %>' bindingfield="BaseQuantity" />
                                    <input type="hidden" value='<%#: Eval("UsedQuantity") %>' bindingfield="UsedQuantity" />
                                    <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                    <input type="hidden" value='<%#: Eval("ConversionFactor") %>' bindingfield="ConversionFactor" />
                                    <input type="hidden" value='<%#: Eval("GCBaseUnit") %>' bindingfield="GCBaseUnit" />
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
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("UsedQuantity")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px;">
                                    <div>
                                        <%#: Eval("ItemUnit")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding-right: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("CreatedByFullName")%></div>
                                    <div>
                                        <%#: Eval("CreatedDateInString")%></div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <div class="imgLoadingGrdView" id="Div2">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <span class="lblLink" id="lblDrugMSQuickPick">
                        <%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
