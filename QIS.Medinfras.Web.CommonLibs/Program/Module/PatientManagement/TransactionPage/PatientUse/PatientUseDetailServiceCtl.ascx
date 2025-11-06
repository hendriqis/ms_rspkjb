<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientUseDetailServiceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientUseDetailServiceCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<script type="text/javascript" id="dxss_ptservicectl">
    //#region Service
    function onLoadService() {
        if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
            $('#lblServiceQuickPick').show();
        }
        else {
            $('#lblServiceQuickPick').hide();
        }

        $('#btnServiceSave').click(function (evt) {
            if (IsValid(evt, 'fsTrxService', 'mpTrxService')) {
                if ($('#<%=hdnServiceItemID.ClientID %>').val() == $('#<%=hdnPrescriptionReturnItem.ClientID %>').val() && parseFloat($('#<%=txtServiceQty.ClientID %>').val()) >= 0)
                    showToast('Warning', 'Jumlah Pelayanan Retur Resep Harus Minus');
                else
                    cbpService.PerformCallback('save');
            }
            return false;
        });

        $('#btnServiceCancel').click(function () {
            $('#containerEntryService').hide();
        });

        $('#lblServiceQuickPick').live('click', function () {
            $('#<%=hdnServiceTransactionDtID.ClientID %>').val('');
            $('#<%=hdnServiceItemID.ClientID %>').val('');
            $('#containerEntryService').hide();
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                showLoadingPanel();
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/ServiceQuickPicksCtl.ascx');
                var transactionID = getTransactionHdID();
                var serviceUnitID = getHealthcareServiceUnitID();
                var registrationID = getRegistrationID();
                var visitID = getVisitID();
                var departmentID = getDepartmentID();
                var isAccompany = "0";
                if (typeof isAccompanyChargesPage == 'function') {
                    if (isAccompanyChargesPage()) {
                        isAccompany = "1";
                    }
                }

                var today = new Date();
                var tempDate = "00";
                if (today.getDate() < 10) {
                    tempDate = "0" + today.getDate();
                } else {
                    tempDate = today.getDate();
                }
                var pad = "00";
                var tmpMonth = (today.getMonth() + 1).toString();
                var month = pad.substring(0, pad.length - tmpMonth.length) + tmpMonth;
                var date = tempDate + "-" + month + "-" + today.getFullYear();
                var transactionDate = $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDateServiceCtl.ClientID %>').val();

                var id = transactionID + '|' + serviceUnitID + '|' + visitID + '|' + registrationID + '|' + departmentID + '|' + isAccompany + '|' + transactionDate;
                openUserControlPopup(url, id, 'Quick Picks', 1000, 600);
            }
        });
    }

    function onCbpServiceEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var transactionID = s.cpTransactionID;
                onAfterSaveRecordDtSuccess(transactionID);
                $('#containerEntryService').hide();
                setCustomToolbarVisibility();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'switch') {
            if (param[1] == 'fail')
                showToast('Switch Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    $('.imgServiceSwitch.imgLink').die('click');
    $('.imgServiceSwitch.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var obj = rowToObject($row);
        cbpService.PerformCallback('switch|' + obj.ID);
    });

    //#region Entry Service
    $('.imgServiceDelete.imgLink').die('click');
    $('.imgServiceDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showDeleteConfirmation(function (data) {
            var obj = rowToObject($row);
            var param = 'delete|' + obj.ID + ';' + data.GCDeleteReason + ';' + data.Reason;
            cbpService.PerformCallback(param);
        });
    });

    //#region Edit
    $('.imgServiceEdit.imgLink').die('click');
    $('.imgServiceEdit.imgLink').live('click', function () {
        $('#containerEntryService').show();
        $row = $(this).closest('tr').parent().closest('tr');
        var obj = rowToObject($row);
        if (obj.IsSubContractItem == 'True') {
            $('#lblTestPartner').attr('class', 'lblLink');
            $('#<%=txtTestPartnerCode.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#lblTestPartner').attr('class', 'lblDisabled');
            $('#<%=txtTestPartnerCode.ClientID %>').attr('readonly', 'readonly');
        }
        $('#<%=hdnServicePhysicianID.ClientID %>').val(obj.ParamedicID);
        $('#<%=txtServicePhysicianCode.ClientID %>').val(obj.ParamedicCode);
        $('#<%=txtServicePhysicianName.ClientID %>').val(obj.ParamedicName);
        $('#<%=hdnTestPartnerID.ClientID %>').val(obj.TestPartnerID);
        $('#<%=txtTestPartnerCode.ClientID %>').val(obj.TestPartnerCode);
        $('#<%=txtTestPartnerName.ClientID %>').val(obj.TestPartnerName);
        $('#<%=txtServiceItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#<%=hdnServiceItemID.ClientID %>').val(obj.ItemID);
        $('#<%=txtServiceItemCode.ClientID %>').val(obj.ItemCode);
        $('#<%=txtServiceItemName.ClientID %>').val(obj.ItemName1);
        $('#<%=hdnServiceTransactionDtID.ClientID %>').val(obj.ID);
        $('#<%=txtServiceQty.ClientID %>').val(obj.ChargedQuantity);

        setCheckBoxEnabled($('#<%=chkServiceIsCITO.ClientID %>'), obj.IsAllowCITO == 'True');
        $('#<%=chkServiceIsCITO.ClientID %>').prop('checked', obj.IsCITO == 'True');

        if (obj.IsAllowCITO == 'True') {
            var registrationID = getRegistrationID();
            var visitID = getVisitID();
            var classID = obj.ChargeClassID;
            var itemID = obj.ItemID;
            var trxDate = getTrxDate();

            Methods.getItemTariff(registrationID, visitID, classID, obj.ItemID, trxDate, function (result) {
                $('#<%=hdnServiceBaseCITOAmount.ClientID %>').val(result.CITOAmount);
                $('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val(result.IsCITOInPercentage ? '1' : '0');
                $('#<%=hdnServicePrice.ClientID %>').val(result.Price);
            });
        }
    });
    //#endregion

    //#endregion
    //#endregion

    $('#<%:chkServiceIsCITO.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            var isCITOInPercentage = ($('#<%=hdnServiceIsCITOInPercentage.ClientID %>').val() == '1');
            var baseCITOAmount = parseFloat($('#<%=hdnServiceBaseCITOAmount.ClientID %>').val());
            var CITO = 0;
            var unitPrice = parseFloat($('#<%=hdnServicePrice.ClientID %>').val());
            var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
            $('#<%=hdnServiceTariff.ClientID %>').val(unitPrice * qty).trigger('changeValue');

            if (isCITOInPercentage) {
                var tariff = parseFloat($('#<%=hdnServiceTariff.ClientID %>').val());
                CITO = (tariff * baseCITOAmount) / 100;
                $('#<%=hdnCITOAmount.ClientID %>').val(CITO).trigger('changeValue');
            } else {
                var qty = parseFloat($('#<%=txtServiceQty.ClientID %>').val());
                CITO = baseCITOAmount * qty;
            }
        } else {
            $('#<%=hdnCITOAmount.ClientID %>').val(0).trigger('changeValue');
        }
    });

    $('.imgServiceParamedic.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ID;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/ParamedicTeamCtl.ascx");
        openUserControlPopup(url, id, 'Tim Medis', 600, 500);
    });

    $('.imgIsPackageItem.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);
        var id = entity.ItemID;
        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/ViewItemPackageDetail.ascx");
        openUserControlPopup(url, id, 'Item Paket', 600, 500);
    });

    //#region Physician
    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtServicePhysicianCode.ClientID %>').val(value);
            onTxtServicePhysicianCodeChanged(value);
        });
    });

    $('#<%=txtServicePhysicianCode.ClientID %>').live('change', function () {
        onTxtServicePhysicianCodeChanged($(this).val());
    });

    function onTxtServicePhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnServicePhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtServicePhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnServicePhysicianID.ClientID %>').val('');
                $('#<%=txtServicePhysicianCode.ClientID %>').val('');
                $('#<%=txtServicePhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Test Partner
    $('#lblTestPartner.lblLink').live('click', function () {
        openSearchDialog('testpartner', '', function (value) {
            $('#<%=txtTestPartnerCode.ClientID %>').val(value);
            onTxtTestPartnerCodeChanged(value);
        });
    });

    $('#<%=txtTestPartnerCode.ClientID %>').live('change', function () {
        onTxtTestPartnerCodeChanged($(this).val());
    });

    function onTxtTestPartnerCodeChanged(value) {
        var filterExpression = "BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvTestPartnerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTestPartnerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtTestPartnerName.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnTestPartnerID.ClientID %>').val('');
                $('#<%=txtTestPartnerName.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<input type="hidden" id="hdnLabHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnServiceTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnPrescriptionReturnItem" runat="server" value="" />
<input type="hidden" id="hdnIsEditable" runat="server" value="" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnServiceItemID" runat="server" value="" />
<input type="hidden" value="" id="hdnServiceTransactionID" runat="server" />
<input type="hidden" value="" id="hdnServiceBaseCITOAmount" runat="server" />
<input type="hidden" value="" id="hdnServiceIsCITOInPercentage" runat="server" />
<input type="hidden" value="" id="hdnServicePrice" runat="server" />
<input type="hidden" value="" id="hdnServiceTariff" runat="server" />
<input type="hidden" value="" id="hdnCITOAmount" runat="server" />
<div id="containerEntryService" style="margin-top: 4px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Tambah Atau Ubah Data")%></div>
    <fieldset id="fsTrxService" style="margin: 0">
        <table class="tblEntryDetail" style="width: 100%">
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblDisabled" id="lblServiceItem">
                                    <%=GetLabel("Pelayanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnDrugMSItemID" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionDateServiceCtl" runat="server" />
                                <input type="hidden" value="" id="hdnTransactionTime" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 400px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceItemName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblPhysician">
                                    <%=GetLabel("Dokter/Paramedis")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 400px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServicePhysicianCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServicePhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblDisabled" id="lblTestPartner">
                                    <%=GetLabel("Test Partner")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnTestPartnerID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col style="width: 400px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtTestPartnerCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTestPartnerName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceQty" Width="120px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("CITO")%></label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkServiceIsCITO" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" id="btnServiceSave" value='<%= GetLabel("Save")%>' />
                                        </td>
                                        <td>
                                            <input type="button" id="btnServiceCancel" value='<%= GetLabel("Cancel")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <colgroup>
                <col style="width: 100px" />
                <col />
            </colgroup>
        </table>
    </fieldset>
</div>
<dxcp:ASPxCallbackPanel ID="cbpService" runat="server" Width="100%" ClientInstanceName="cbpService"
    ShowLoadingPanel="false" OnCallback="cbpService_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpServiceEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em;">
                <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" runat="server"
                    id="hdnChargesDt" />
                <asp:ListView ID="lvwService" runat="server">
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 80px">
                                </th>
                                <th>
                                    <div style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Item")%>
                                    </div>
                                </th>
                                <th style="width: 80px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Jumlah")%>
                                    </div>
                                </th>
                                <th style="width: 230px">
                                    <div style="text-align: right; padding-right: 3px">
                                        <%=GetLabel("Petugas")%>
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
                                                <img class="imgServiceParamedic imgLink" <%# IsEditable.ToString() == "True" && IsShowParamedicTeam.ToString() == "True" ?  "" : "style='display:none'" %>
                                                    title='<%=GetLabel("Tim Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/paramedic_team.png")%>'
                                                    alt="" style="margin-right: 2px" />
                                            </td>
                                            <td>
                                                <img class="imgServiceHasil" <%# Eval("IsHasTestResult").ToString() == "True" ?  "" : "style='display:none'" %>
                                                    title='<%=GetLabel("Sudah Ada Hasil")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" style="margin-right: 2px" />
                                            </td>
                                            <td>
                                                <img class="imgServiceEdit <%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsVerified").ToString() == "True" || Eval("IsReviewed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="margin-right: 2px" />
                                            </td>
                                            <td>
                                                <img class="imgServiceDelete <%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? "imgDisabled" : "imgLink"%>"
                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" || Eval("IsHasTestResult").ToString() == "True" || Eval("IsReviewed").ToString() == "True" || Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" style="margin-right: 2px" />
                                            </td>
                                            <td>
                                                <img class="imgServiceVerified" <%# Eval("IsVerified").ToString() == "True" ?  "" : "style='display:none'" %>
                                                    title='<%=GetLabel("Verified")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                            </td>
                                            <td>
                                                <img class="imgIsPackageItem imgLink" <%# Eval("IsPackageItem").ToString() == "True" ?  "" : "style='display:none'" %>
                                                    title='<%=GetLabel("Lihat Paket")%>' src='<%# ResolveUrl("~/Libs/Images/Button/verify.png")%>'
                                                    alt="" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                    <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                    <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                    <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                    <input type="hidden" value='<%#: Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                    <input type="hidden" value='<%#: Eval("ParamedicCode") %>' bindingfield="ParamedicCode" />
                                    <input type="hidden" value='<%#: Eval("ParamedicName") %>' bindingfield="ParamedicName" />
                                    <input type="hidden" value='<%#: Eval("BusinessPartnerID") %>' bindingfield="TestPartnerID" />
                                    <input type="hidden" value='<%#: Eval("BusinessPartnerCode") %>' bindingfield="TestPartnerCode" />
                                    <input type="hidden" value='<%#: Eval("BusinessPartnerName") %>' bindingfield="TestPartnerName" />
                                    <input type="hidden" value='<%#: Eval("IsSubContractItem") %>' bindingfield="IsSubContractItem" />
                                    <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                    <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                    <input type="hidden" value='<%#: Eval("IsAllowCITO") %>' bindingfield="IsAllowCITO" />
                                    <input type="hidden" value='<%#: Eval("IsCITO") %>' bindingfield="IsCITO" />
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <div>
                                        <%#: Eval("ItemName1")%></div>
                                    <div>
                                        <span style="font-style: italic">
                                            <%#: Eval("ItemCode") %></span>, <span style="color: Blue">
                                                <%#: Eval("ParamedicName")%></span>
                                    </div>
                                    <div <%# Eval("BusinessPartnerName").ToString() != "" ?  "" : "style='display:none'" %>>
                                        <%#: Eval("BusinessPartnerName")%></div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 3px; text-align: right;">
                                    <div>
                                        <%#: Eval("ChargedQuantity")%></div>
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
                <div class="imgLoadingGrdView" id="containerImgLoadingService">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <span class="lblLink" id="lblServiceQuickPick">
                        <%= GetLabel("Quick Picks")%></span>
                </div>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
