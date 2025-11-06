<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientUseDetailLogisticCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientUseDetailLogisticCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ptlgctl">
    //#region Logistic
    function onLoadLogistic() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblLogisticQuickPick').show();
        }
        else {
            $('#lblLogisticQuickPick').hide();
        }

        $('#btnLogisticSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxLogistic', 'mpTrxLogistic'))
                cbpLogistic.PerformCallback('save');
            return false;
        });

        $('#btnLogisticCancel').click(function () {
            $('#containerEntryLogistic').hide();
        });

        $('#lblLogisticQuickPick').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/DrugsLogisticsQuickPicksCtl.ascx');
                var transactionID = getTransactionHdID();
                var locationID = getLogisticLocationID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var GCItemType = getGCItemType();
                var departmentID = getDepartmentID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }
                var id = transactionID + '|' + locationID + '|' + visitID + '|' + registrationID + '|' + GCItemType + '|' + departmentID + '|' + serviceUnitID + '|' + isAccompany;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });

        $('#<%=txtLogisticQtyUsed.ClientID %>').change(function () {
            var conversionValue = parseFloat($('#<%=hdnLogisticConversionValue.ClientID %>').val());
            var qty = parseFloat($(this).val());
            $('#<%=txtLogisticBaseQty.ClientID %>').val(conversionValue * qty);
        });
    }

    //#region Entry Drug MS
    var isEditLogistic = false;

    $('.imgLogisticApprove.imgLink').die('click');
    $('.imgLogisticApprove.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogistic.PerformCallback('approve|' + obj.ID);
    });

    $('.imgLogisticVoid.imgLink').die('click');
    $('.imgLogisticVoid.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cbpLogistic.PerformCallback('void|' + obj.ID);
    });

    $('.imgLogisticDelete.imgLink').die('click');
    $('.imgLogisticDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpLogistic.PerformCallback(param);
        });
    });

    $('.imgLogisticEdit.imgLink').die('click');
    $('.imgLogisticEdit.imgLink').live('click', function () {
        $('#containerEntryLogistic').show();
        cboLogisticLocation.SetEnabled(false);
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        cboLogisticLocation.SetValue(obj.LocationID);
        $('#<%=hdnLogisticItemID.ClientID %>').val(obj.ItemID);
        $('#<%=txtLogisticItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtLogisticItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnLogisticTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtLogisticQtyUsed.ClientID %>').val(obj.UsedQuantity);
        $('#<%=txtLogisticBaseQty.ClientID %>').val(obj.BaseQuantity);
        $('#<%=hdnLogisticConversionValue.ClientID %>').val(obj.ConversionFactor);
        $('#<%=hdnLogisticDefaultUoM.ClientID %>').val(obj.GCBaseUnit);
        $('#<%=txtLogisticQtyUsed.ClientID %>').focus();
        isEditLogistic = true;
        objLogistic = obj;
        cboLogisticUoM.PerformCallback();

    });
    //#endregion

    function onCboLogisticUomEndCallback() {
        if (!isEditLogistic) {
            cboLogisticUoM.SetValue('');
        }
        else {
            cboLogisticUoM.SetValue(objLogistic.GCItemUnit);
            var conversionFactor = $('#<%=hdnLogisticConversionValue.ClientID %>').val();
            var defaultUoM = $('#<%=hdnLogisticDefaultUoM.ClientID %>').val();
            var currUoM = cboLogisticUoM.GetValue();
            var fromConversion = getLogisticItemUnitName(defaultUoM);
            var toConversion = getLogisticItemUnitName(currUoM);
            var conversion = "1 " + toConversion + " = " + conversionFactor + " " + fromConversion;
            $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
        }
        hideLoadingPanel();
    }

    function setLogisticItemUnitConversionText() {
        var defaultUoM = $('#<%=hdnLogisticDefaultUoM.ClientID %>').val();
        var currUoM = cboLogisticUoM.GetValue();
        var fromConversion = getLogisticItemUnitName(defaultUoM);
        if (defaultUoM == currUoM) {
            $('#<%=hdnLogisticConversionValue.ClientID %>').val('1');
            $('#<%=txtLogisticQtyUsed.ClientID %>').change();
            var conversion = "1 " + fromConversion + " = 1 " + fromConversion;
            $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
        }
        else {
            var itemID = $('#<%=hdnLogisticItemID.ClientID %>').val();
            var filterExpression = "ItemID = " + itemID + " AND GCAlternateUnit = '" + currUoM + "'";
            Methods.getObjectValue('GetvItemAlternateUnitList', filterExpression, 'ConversionFactor', function (result) {
                var toConversion = getLogisticItemUnitName(currUoM);
                $('#<%=hdnLogisticConversionValue.ClientID %>').val(result);
                $('#<%=txtLogisticQtyUsed.ClientID %>').change();
                var conversion = "1 " + toConversion + " = " + result + " " + fromConversion;
                $('#<%=txtLogisticConversion.ClientID %>').val(conversion);
            });
        }
    }

    function getLogisticItemUnitName(itemUnitID) {
        var value = cboLogisticUoM.GetValue();
        cboLogisticUoM.SetValue(itemUnitID);
        var text = cboLogisticUoM.GetText();
        cboLogisticUoM.SetValue(value);
        return text;
    }

    function onCbpLogisticEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryLogistic').hide();
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
<input type="hidden" id="hdnLogisticTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" value="0" id="hdnIsAllowOverIssued" runat="server" />
<div id="containerEntryLogistic" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah / Ubah Data")%></div>
    <fieldset id="fsTrxLogistic" style="margin: 0">
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
                                <dxe:ASPxComboBox ID="cboLogisticLocation" ClientInstanceName="cboLogisticLocation"
                                    Width="100%" runat="server" OnCallback="cboLogisticLocation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" id="lblLogisticItem">
                                    <%=GetLabel("Barang Umum")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" value="" id="hdnLogisticItemID" runat="server" />
                                <input type="hidden" value="" id="hdnLogisticConversionValue" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLogisticItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLogisticItemName" ReadOnly="true" Width="100%" runat="server" />
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
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticQtyUsed" Width="100%" CssClass="number min" min="0.1"
                                    runat="server" />
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnLogisticDefaultUoM" runat="server" />
                                <dxe:ASPxComboBox ID="cboLogisticUoM" runat="server" ClientInstanceName="cboLogisticUoM"
                                    Width="200px" OnCallback="cboLogisticUoM_Callback">
                                    <ClientSideEvents EndCallback="function(s,e) { onCboLogisticUomEndCallback(); }"
                                        ValueChanged="function(s,e){ setLogisticItemUnitConversionText(); }" />
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
                                    <%=GetLabel("Base Quantity")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticBaseQty" ReadOnly="true" CssClass="number" Width="100%"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLogisticConversion" ReadOnly="true" Width="100%" runat="server" />
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
                                <input type="button" id="btnLogisticSave" value='<%= GetLabel("Save")%>' />
                            </td>
                            <td>
                                <input type="button" id="btnLogisticCancel" value='<%= GetLabel("Cancel")%>' />
                            </td>
                        </tr>
                    </table>
                    <img style="float: left; margin-right: 10px;" src='<%= ResolveUrl("~/Libs/Images/Button/info.png")%>'
                        alt='' />
                    <label class="lblInfo">
                        Barang Umum yang di-Save akan langsung di-Approve.</label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpLogistic" runat="server" Width="100%" ClientInstanceName="cbpLogistic"
    ShowLoadingPanel="false" OnCallback="cbpLogistic_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpLogisticEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent2" runat="server">
            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                position: relative; font-size: 0.95em;">
                <asp:ListView ID="lvwLogistic" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdLogistic grdNormal notAllowSelect" cellspacing="0"
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
                                                <img class="imgLogisticVerified" <%# IsEditable.ToString() == "True" && Eval("IsVerified").ToString() == "True" ? "" : "style='display:none'"%>
                                                    title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                                <img class="imgLogisticApprove imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "True") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Approve This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/unlock.png")%>'
                                                    alt="" />
                                                <img class="imgLogisticVoid imgLink" <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || (Eval("IsVerified").ToString() == "False" && Eval("IsApproved").ToString() == "False") ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Void This Item")%>' src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgLogisticEdit imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%>
                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                            </td>
                                            <td style="width: 1px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 24px">
                                                <img class="imgLogisticDelete imgLink" <%# IsEditable.ToString() == "False" || Eval("IsApproved").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "style='display:none'" : ""%>
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
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
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
                    <span class="lblLink" id="lblLogisticQuickPick">
                        <%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
