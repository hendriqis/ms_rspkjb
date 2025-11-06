<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="FAItemEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onLoad() {
            setRightPanelButtonEnabled();

            setDatePicker('<%=txtDepreciationStartDate.ClientID %>');
            setDatePicker('<%=txtGuaranteeStartDate.ClientID %>');
            setDatePicker('<%=txtGuaranteeEndDate.ClientID %>');

            var value = cboBudgetCategory.GetValue();
            if (value == "X393^002") { // CAPEX
                $('#<%=chkIsUsedDepreciation.ClientID %>').prop('checked', true);
                $('#<%=chkIsUsedDepreciation.ClientID %>').removeAttr('style');
            } else {
                $('#<%=chkIsUsedDepreciation.ClientID %>').prop('checked', false);
                $('#<%=chkIsUsedDepreciation.ClientID %>').attr('style', 'display:none');
            }

            $('#<%=chkIsBusinessPartnerFromMaster.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#<%=trBusinessPartner.ClientID %>').removeAttr('style');
                    $('#<%=trBusinessPartnerNonMaster.ClientID %>').attr('style', 'display:none');
                }
                else {
                    $('#<%=trBusinessPartner.ClientID %>').attr('style', 'display:none');
                    $('#<%=trBusinessPartnerNonMaster.ClientID %>').removeAttr('style');
                }
            });

            if ($('#<%=hdnPurchaseReceiveID.ClientID %>').val() != '' && $('#<%=hdnPurchaseReceiveID.ClientID %>').val() != '0') {
                $('#<%=txtProcurementNumber.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtReceivedDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtProcurementDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtProcurementAmount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtProcurementQuantity.ClientID %>').attr('readonly', 'readonly');
                cboProcurementUnit.SetEnabled(false);

                $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtBusinessPartnerCode.ClientID %>').attr('readonly', 'readonly');
                $('#lblItem').attr('class', 'lblDisabled');
                $('#lblBusinessPartner').attr('class', 'lblDisabled');
            }
            else {
                setDatePicker('<%=txtReceivedDate.ClientID %>');
                setDatePicker('<%=txtProcurementDate.ClientID %>');
            }

            //#region ParentFixedAsset
            function onGetParentFixedAssetFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionParentFixedAsset()%>";
                return filterExpression;
            }

            $('#lblParentFixedAsset.lblLink').click(function () {
                openSearchDialog('faitem', onGetParentFixedAssetFilterExpression(), function (value) {
                    $('#<%=txtParentFixedAssetCode.ClientID %>').val(value);
                    onTxtParentFixedAssetCodeChanged(value);
                });
            });

            $('#<%=txtParentFixedAssetCode.ClientID %>').change(function () {
                onTxtParentFixedAssetCodeChanged($(this).val());
            });

            function onTxtParentFixedAssetCodeChanged(value) {
                var filterExpression = onGetParentFixedAssetFilterExpression() + " AND FixedAssetCode = '" + value + "'";
                Methods.getObject('GetFAItemList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.FixedAssetID);
                        $('#<%=txtParentFixedAssetName.ClientID %>').val(result.FixedAssetName);

                        $('#trSequence').removeAttr('style');
                        $('#trDisplayOrder').removeAttr('style');
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentFixedAssetCode.ClientID %>').val('');
                        $('#<%=txtParentFixedAssetName.ClientID %>').val('');

                        $('#trSequence').attr('style', "display:none");
                        $('#trDisplayOrder').attr('style', "display:none");
                    }
                });
            }
            //#endregion

            //#region Item
            function onGetItemFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionItem()%>";
                return filterExpression;
            }

            $('#lblItem.lblLink').click(function () {
                openSearchDialog('itemproduct', onGetItemFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetvItemProductList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=txtFixedAssetName.ClientID %>').val(result.ItemName1);

                        cboProcurementUnit.SetValue(result.GCItemUnit);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=txtFixedAssetName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region FA Group
            function onGetFAGroupFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblFAGroup.lblLink').click(function () {
                openSearchDialog('fagroup', onGetFAGroupFilterExpression(), function (value) {
                    $('#<%=txtFAGroupCode.ClientID %>').val(value);
                    onTxtFAGroupCodeChanged(value);
                });
            });

            $('#<%=txtFAGroupCode.ClientID %>').change(function () {
                onTxtFAGroupCodeChanged($(this).val());
            });

            function onTxtFAGroupCodeChanged(value) {
                var filterExpression = onGetFAGroupFilterExpression() + " AND FAGroupCode = '" + value + "'";
                Methods.getObject('GetFAGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFAGroupID.ClientID %>').val(result.FAGroupID);
                        $('#<%=txtFAGroupName.ClientID %>').val(result.FAGroupName);

                        var filterGroupCOA = "FAGroupID = " + result.FAGroupID;
                        Methods.getObject('GetvFAGroupCOAList', filterGroupCOA, function (resultGroupCOA) {
                            if (resultGroupCOA != null) {
                                $('#<%=hdnGLAccount1ID.ClientID %>').val(resultGroupCOA.GLAccount1);
                                $('#<%=txtGLAccount1Code.ClientID %>').val(resultGroupCOA.GLAccount1No);
                                $('#<%=txtGLAccount1Name.ClientID %>').val(resultGroupCOA.GLAccount1Name);

                                $('#<%=hdnGLAccount2ID.ClientID %>').val(resultGroupCOA.GLAccount2);
                                $('#<%=txtGLAccount2Code.ClientID %>').val(resultGroupCOA.GLAccount2No);
                                $('#<%=txtGLAccount2Name.ClientID %>').val(resultGroupCOA.GLAccount2Name);

                                $('#<%=hdnGLAccount3ID.ClientID %>').val(resultGroupCOA.GLAccount3);
                                $('#<%=txtGLAccount3Code.ClientID %>').val(resultGroupCOA.GLAccount3No);
                                $('#<%=txtGLAccount3Name.ClientID %>').val(resultGroupCOA.GLAccount3Name);

                                $('#<%=hdnGLAccount4ID.ClientID %>').val(resultGroupCOA.GLAccount4);
                                $('#<%=txtGLAccount4Code.ClientID %>').val(resultGroupCOA.GLAccount4No);
                                $('#<%=txtGLAccount4Name.ClientID %>').val(resultGroupCOA.GLAccount4Name);

                                $('#<%=hdnGLAccount5ID.ClientID %>').val(resultGroupCOA.GLAccount5);
                                $('#<%=txtGLAccount5Code.ClientID %>').val(resultGroupCOA.GLAccount5No);
                                $('#<%=txtGLAccount5Name.ClientID %>').val(resultGroupCOA.GLAccount5Name);

                                $('#<%=hdnGLAccount6ID.ClientID %>').val(resultGroupCOA.GLAccount6);
                                $('#<%=txtGLAccount6Code.ClientID %>').val(resultGroupCOA.GLAccount6No);
                                $('#<%=txtGLAccount6Name.ClientID %>').val(resultGroupCOA.GLAccount6Name);
                            }
                            else {
                                $('#<%=hdnGLAccount1ID.ClientID %>').val("");
                                $('#<%=txtGLAccount1Code.ClientID %>').val("");
                                $('#<%=txtGLAccount1Name.ClientID %>').val("");

                                $('#<%=hdnGLAccount2ID.ClientID %>').val("");
                                $('#<%=txtGLAccount2Code.ClientID %>').val("");
                                $('#<%=txtGLAccount2Name.ClientID %>').val("");

                                $('#<%=hdnGLAccount3ID.ClientID %>').val("");
                                $('#<%=txtGLAccount3Code.ClientID %>').val("");
                                $('#<%=txtGLAccount3Name.ClientID %>').val("");

                                $('#<%=hdnGLAccount4ID.ClientID %>').val("");
                                $('#<%=txtGLAccount4Code.ClientID %>').val("");
                                $('#<%=txtGLAccount4Name.ClientID %>').val("");

                                $('#<%=hdnGLAccount5ID.ClientID %>').val("");
                                $('#<%=txtGLAccount5Code.ClientID %>').val("");
                                $('#<%=txtGLAccount5Name.ClientID %>').val("");

                                $('#<%=hdnGLAccount6ID.ClientID %>').val("");
                                $('#<%=txtGLAccount6Code.ClientID %>').val("");
                                $('#<%=txtGLAccount6Name.ClientID %>').val("");
                            }
                        });
                    }
                    else {
                        $('#<%=hdnFAGroupID.ClientID %>').val('');
                        $('#<%=txtFAGroupCode.ClientID %>').val('');
                        $('#<%=txtFAGroupName.ClientID %>').val('');


                        $('#<%=hdnGLAccount1ID.ClientID %>').val("");
                        $('#<%=txtGLAccount1Code.ClientID %>').val("");
                        $('#<%=txtGLAccount1Name.ClientID %>').val("");

                        $('#<%=hdnGLAccount2ID.ClientID %>').val("");
                        $('#<%=txtGLAccount2Code.ClientID %>').val("");
                        $('#<%=txtGLAccount2Name.ClientID %>').val("");

                        $('#<%=hdnGLAccount3ID.ClientID %>').val("");
                        $('#<%=txtGLAccount3Code.ClientID %>').val("");
                        $('#<%=txtGLAccount3Name.ClientID %>').val("");

                        $('#<%=hdnGLAccount4ID.ClientID %>').val("");
                        $('#<%=txtGLAccount4Code.ClientID %>').val("");
                        $('#<%=txtGLAccount4Name.ClientID %>').val("");

                        $('#<%=hdnGLAccount5ID.ClientID %>').val("");
                        $('#<%=txtGLAccount5Code.ClientID %>').val("");
                        $('#<%=txtGLAccount5Name.ClientID %>').val("");

                        $('#<%=hdnGLAccount6ID.ClientID %>').val("");
                        $('#<%=txtGLAccount6Code.ClientID %>').val("");
                        $('#<%=txtGLAccount6Name.ClientID %>').val("");
                    }
                });
            }
            //#endregion

            //#region FA Location
            function onGetFALocationFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblFALocation.lblLink').click(function () {
                openSearchDialog('falocation', onGetFALocationFilterExpression(), function (value) {
                    $('#<%=txtFALocationCode.ClientID %>').val(value);
                    onTxtFALocationCodeChanged(value);
                });
            });

            $('#<%=txtFALocationCode.ClientID %>').change(function () {
                onTxtFALocationCodeChanged($(this).val());
            });

            function onTxtFALocationCodeChanged(value) {
                var filterExpression = onGetFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
                Methods.getObject('GetFALocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFALocationID.ClientID %>').val(result.FALocationID);
                        $('#<%=txtFALocationName.ClientID %>').val(result.FALocationName);
                    }
                    else {
                        $('#<%=hdnFALocationID.ClientID %>').val('');
                        $('#<%=txtFALocationCode.ClientID %>').val('');
                        $('#<%=txtFALocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Supplier
            function onGetSupplierFilterExpression() {
                var filterExpression = "<%:OnGetFilterExpressionSupplier()%>";
                return filterExpression;
            }

            $('#lblBusinessPartner.lblLink').click(function () {
                openSearchDialog('businesspartners', onGetSupplierFilterExpression(), function (value) {
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                    onTxtBusinessPartnerCodeChanged(value);
                });
            });

            $('#<%=txtBusinessPartnerCode.ClientID %>').change(function () {
                onTxtBusinessPartnerCodeChanged($(this).val());
            });

            function onTxtBusinessPartnerCodeChanged(value) {
                var filterExpression = onGetSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Depreciation Method
            function onGetFADepreciationMethodFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblMethod.lblLink').click(function () {
                openSearchDialog('fadepreciationmethod', onGetFADepreciationMethodFilterExpression(), function (value) {
                    $('#<%=txtFADepreciationMethodCode.ClientID %>').val(value);
                    onTxtDepreciationMethodCodeChanged(value);
                });
            });

            $('#<%=txtFADepreciationMethodCode.ClientID %>').change(function () {
                onTxtDepreciationMethodCodeChanged($(this).val());
            });

            function onTxtDepreciationMethodCodeChanged(value) {
                var filterExpression = onGetFADepreciationMethodFilterExpression() + " AND MethodCode = '" + value + "'";
                Methods.getObject('GetFADepreciationMethodList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFADepreciationMethodID.ClientID %>').val(result.MethodID);
                        $('#<%=txtFADepreciationMethodName.ClientID %>').val(result.MethodName);
                    }
                    else {
                        $('#<%=hdnFADepreciationMethodID.ClientID %>').val('');
                        $('#<%=txtFADepreciationMethodCode.ClientID %>').val('');
                        $('#<%=txtFADepreciationMethodName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            if ($('#<%=hdnID.ClientID %>').val() == '' || $('#<%=hdnFAAcceptanceID.ClientID %>').val() != 0) {
                $('#<%:btnCreateFAAcceptance.ClientID %>').attr("disabled", true);
            }
            else {
                $('#<%:btnCreateFAAcceptance.ClientID %>').removeAttr("disabled");
            }

            $('#<%=btnCreateFAAcceptance.ClientID %>').click(function () {
                if ($('#<%=hdnID.ClientID %>').val() != '') {
                    if ($('#<%=hdnFALocationID.ClientID %>').val() != '' && $('#<%=hdnFALocationID.ClientID %>').val() != '0') {
                        if ($('#<%=hdnFAGroupID.ClientID %>').val() != '' && $('#<%=hdnFAGroupID.ClientID %>').val() != '0') {
                            var url = "";
                            var reqID = $('#<%=hdnFALocationID.ClientID %>').val() + "|" + $('#<%=hdnFAGroupID.ClientID %>').val() + "|" + $('#<%=hdnID.ClientID %>').val();
                            url = ResolveUrl("~/Program/FixedAssetProcess/FAAcceptanceProcessEntry.aspx?id=");
                            showLoadingPanel();
                            window.location.href = url + reqID;
                        } else {
                            alert("Harap pilih Kelompok Asset terlebih dahulu.");
                        }
                    } else {
                        alert("Harap pilih Lokasi Asset terlebih dahulu.");
                    }
                }
            });
        }

        function oncboBudgetCategoryValueChanged(s) {
            var value = s.GetValue();
            if (value == "X393^002") { // CAPEX
                $('#<%=chkIsUsedDepreciation.ClientID %>').prop('checked', true);
                $('#<%=chkIsUsedDepreciation.ClientID %>').removeAttr('style');
            } else {
                $('#<%=chkIsUsedDepreciation.ClientID %>').prop('checked', false);
                $('#<%=chkIsUsedDepreciation.ClientID %>').attr('style', 'display:none');
            }
        }

        function onAfterSaveNewSuccess(type, retval) {
            if (retval != "" && retval != "0") {
                var oFixedAssetID = retval;
                var filterExpression = "FixedAssetID = " + oFixedAssetID;

                Methods.getObject('GetFAItemList', filterExpression, function (result) {
                    if (result != null) {
                        var messageOutput = "Kode Aset : <b>" + result.FixedAssetCode + "</b> || Nama Aset : <b>" + result.FixedAssetName + "</b>";
                        showToast('SAVE SUCCESS', messageOutput);
                    }
                });
            }
        }

        function onAfterSaveCloseSuccess(type, retval) {
            if (retval != "" && retval != "0") {
                var oFixedAssetID = retval;
                var filterExpression = "FixedAssetID = " + oFixedAssetID;

                Methods.getObject('GetFAItemList', filterExpression, function (result) {
                    if (result != null) {
                        var messageOutput = "Kode Aset : <b>" + result.FixedAssetCode + "</b> || Nama Aset : <b>" + result.FixedAssetName + "</b>";
                        showToast('SAVE SUCCESS', messageOutput);
                    }
                });

                setHdnListIDValue(oFixedAssetID);
            }
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            if (param != "") {
                setHdnListIDValue(param);
                setDocumentLocationBackToList();
            }
        }

        function setRightPanelButtonEnabled() {
            var oFixedAssetID = $('#<%=hdnID.ClientID %>').val();
            if (oFixedAssetID != "" && oFixedAssetID != null) {
                var filterExpression = "FixedAssetID = " + oFixedAssetID + "  AND IsDeleted = 0 AND GCTransactionStatus != 'X121^999'";
                Methods.getObject('GetvFAAcceptanceDtList', filterExpression, function (result) {
                    if (result != null) {
                        $('#btnFAItemChangeAfterAcceptance').removeAttr('enabled');
                    } else {
                        $('#btnFAItemChangeAfterAcceptance').attr('enabled', 'false');
                    }
                });
            }
            else {
                $('#btnFAItemChangeAfterAcceptance').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code, error) {
            var oFixedAssetID = $('#<%=hdnID.ClientID %>').val();
            if (code == 'FAItemChangeAfterAcceptance') {
                return oFixedAssetID;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var itemID = $('#<%=hdnID.ClientID %>').val();
            var menuCode = $('#<%=hdnMenuCode.ClientID %>').val();
            if (code == "AC-00004") {
                filterExpression.text = itemID + '|' + menuCode;
                return true;
            } else if (code == "AC-00016") {
                filterExpression.text = $('#<%=hdnID.ClientID %>').val();
                return true;
            }
        }

    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFAAcceptanceID" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseReceiveDtID" runat="server" value="" />
    <input type="hidden" id="hdnDirectPurchaseID" runat="server" value="" />
    <input type="hidden" id="hdnOldFAGroupID" runat="server" value="" />
    <input type="hidden" id="hdnOldLocationID" runat="server" value="" />
    <input type="hidden" id="hdnIsBonusItemFromPOR" runat="server" value="0" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnMenuCode" runat="server" value="" />
    <input type="hidden" value="0" id="hdnIsDiscountAppliedToFAItem" runat="server" />
    <input type="hidden" value="0" id="hdnIsPPNAppliedToFAItem" runat="server" />
    <input type="hidden" value="0" id="hdnAC0021" runat="server" />    
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Data Aset & Inventaris") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Aset & Inventaris")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFixedAssetCode" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Aset & Inventaris")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFixedAssetName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblItem">
                                <%=GetLabel("Item (Master Barang)")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnItemID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtItemCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtItemName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Seri")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerialNumber" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblParentFixedAsset">
                                <%=GetLabel("Aset & Inventaris Induk")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnParentID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtParentFixedAssetCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtParentFixedAssetName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Hitung Penyusutan")%></label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsUsedDepreciation" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblFAGroup">
                                <%=GetLabel("Kelompok")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFAGroupID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFAGroupCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFAGroupName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblFALocation">
                                <%=GetLabel("Lokasi Aset & Inventaris")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFALocationID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFALocationCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFALocationName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kategori Anggaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBudgetCategory" ClientInstanceName="cboBudgetCategory" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ oncboBudgetCategoryValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Anggaran")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBudgetPlanNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr id="trSequence" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Sequence")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSequenceNo" Width="300px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                    <tr id="trDisplayOrder" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Urutan Tampilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayOrder" Width="300px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBusinessPartnerFromMaster" runat="server" /><%=GetLabel("Supplier/Vendor dari Master")%>
                        </td>
                    </tr>
                    <tr id="trBusinessPartnerNonMaster" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Supplier/Vendor")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBusinessPartnerNameNonMaster" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trBusinessPartner" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblBusinessPartner">
                                <%=GetLabel("Supplier/Vendor")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBusinessPartnerName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Kontrak")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContractNumber" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%=GetLabel("Lain - lain") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Surat Kerusakan Aset")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoSuratKerusakanAset" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Aset Lama")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOldFixedAssetCode" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%=GetLabel("Pengaturan COA untuk Aset & Inventaris")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount1">
                                <%=GetLabel("COA Aset Tetap")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount1ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID1" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName1" runat="server" />
                            <input type="hidden" id="hdnIDFieldName1" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName1" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName1" runat="server" />
                            <input type="hidden" id="hdnMethodName1" runat="server" />
                            <input type="hidden" id="hdnFilterExpression1" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount1Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount1Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount2">
                                <%=GetLabel("COA Akumulasi Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount2ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID2" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName2" runat="server" />
                            <input type="hidden" id="hdnIDFieldName2" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName2" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName2" runat="server" />
                            <input type="hidden" id="hdnMethodName2" runat="server" />
                            <input type="hidden" id="hdnFilterExpression2" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount2Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount2Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount3">
                                <%=GetLabel("COA Beban Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount3ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID3" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName3" runat="server" />
                            <input type="hidden" id="hdnIDFieldName3" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName3" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName3" runat="server" />
                            <input type="hidden" id="hdnMethodName3" runat="server" />
                            <input type="hidden" id="hdnFilterExpression3" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount3Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount3Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount4">
                                <%=GetLabel("COA Keuntungan Penjualan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount4ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID4" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName4" runat="server" />
                            <input type="hidden" id="hdnIDFieldName4" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName4" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName4" runat="server" />
                            <input type="hidden" id="hdnMethodName4" runat="server" />
                            <input type="hidden" id="hdnFilterExpression4" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount4Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount4Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount5">
                                <%=GetLabel("COA Kerugian Penjualan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount5ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID5" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName5" runat="server" />
                            <input type="hidden" id="hdnIDFieldName5" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName5" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName5" runat="server" />
                            <input type="hidden" id="hdnMethodName5" runat="server" />
                            <input type="hidden" id="hdnFilterExpression5" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount5Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount5Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblGLAccount6">
                                <%=GetLabel("COA Pemusnahan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccount6ID" runat="server" />
                            <input type="hidden" id="hdnSubLedgerID6" runat="server" />
                            <input type="hidden" id="hdnSearchDialogTypeName6" runat="server" />
                            <input type="hidden" id="hdnIDFieldName6" runat="server" />
                            <input type="hidden" id="hdnCodeFieldName6" runat="server" />
                            <input type="hidden" id="hdnDisplayFieldName6" runat="server" />
                            <input type="hidden" id="hdnMethodName6" runat="server" />
                            <input type="hidden" id="hdnFilterExpression6" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount6Code" Width="100%" ReadOnly="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGLAccount6Name" Width="100%" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top; display: none">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt1">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt1ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt1Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt1Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt2">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt2ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt2Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt2Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt3">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt3ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt3Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt3Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt4">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt4ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt4Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt4Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt5">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt5ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt5Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt5Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblDisabled" runat="server" id="lblSubLedgerDt6">
                                <%=GetLabel("Sub")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDt6ID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt6Code" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSubLedgerDt6Name" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Informasi Berita Acara") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 40%" />
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Berita Acara")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAAcceptanceNo" Width="220px" runat="server" ReadOnly="true" />
                        </td>
                        <td rowspan="4" id="tdCreateFAAcceptance" runat="server">
                            <input type="button" id="btnCreateFAAcceptance" value="Buat Berita Acara" class="btnProcess w3-button w3-blue w3-border w3-border-blue w3-round-large"
                                style="font-weight: 600" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Berita Acara")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAAcceptanceDate" Width="220px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Berita Acara")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAAcceptanceStatus" Width="220px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%=GetLabel("Data Perolehan Aset & Inventaris") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Permintaan Pembelian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseRequestNumber" Width="220px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Penerimaan Barang")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPurchaseReceiveID" runat="server" />
                            <input type="hidden" id="hdnTransactionCode" runat="server" value="" />
                            <asp:TextBox ID="txtProcurementNumber" Width="220px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Faktur Pembelian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcurementReferenceNumber" Width="220px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Penerimaan Barang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReceivedDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Perolehan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcurementDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai Perolehan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProcurementAmount" Width="220px" CssClass="txtCurrency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jumlah Perolehan")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtProcurementQuantity" CssClass="number" Width="120px" runat="server" />
                                    </td>
                                    <td style="padding-left: 2px;">
                                        <dxe:ASPxComboBox ID="cboProcurementUnit" ClientInstanceName="cboProcurementUnit"
                                            Width="98px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Product Line Penerimaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineFromPOR" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Masa Garansi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGuaranteePeriod" CssClass="number" Width="120px" runat="server" />&nbsp;
                            <%=GetLabel("Tahun") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Periode Garansi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGuaranteeStartDate" CssClass="datepicker" Width="120px" runat="server" />&nbsp;-&nbsp;
                            <asp:TextBox ID="txtGuaranteeEndDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%=GetLabel("Data Perhitungan Penyusutan Aset & Inventaris") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblMethod">
                                <%=GetLabel("Metode Penyusutan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFADepreciationMethodID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFADepreciationMethodCode" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFADepreciationMethodName" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Umur Penyusutan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepreciationStartLength" CssClass="number" Width="120px" runat="server" />&nbsp;
                            <%=GetLabel("Tahun") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Mulai Penyusutan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepreciationStartDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai Buku Residu")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssetFinalValue" CssClass="txtCurrency" Width="220px" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <%=GetLabel("Informasi Pemusnahan") %></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Pemusnahan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnFAWriteOffID" runat="server" />
                            <asp:TextBox ID="txtFAWriteOffNo" Width="220px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pemusnahan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAWriteOffDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Pemusnahan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFAWriteOffTransactionStatus" Width="120px" runat="server" Style="text-align: center"
                                ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
