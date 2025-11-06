<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationDetailListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.LocationDetailListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    //#region Item Group
    function onGetItemGroupFilterExpression() {
        var filterExpression = "<%:OnGetItemGroupFilterExpression() %>";
        return filterExpression;
    }

    $('#lblItemGroup.lblLink').click(function () {
        openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
            $('#<%=txtItemGroupCode.ClientID %>').val(value);
            onTxtItemGroupCodeChanged(value);
        });
    });

    $('#<%=txtItemGroupCode.ClientID %>').change(function () {
        onTxtItemGroupCodeChanged($(this).val());
    });

    function onTxtItemGroupCodeChanged(value) {
        var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
        Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
            }
            else {
                $('#<%=hdnItemGroupID.ClientID %>').val('');
                $('#<%=txtItemGroupCode.ClientID %>').val('');
                $('#<%=txtItemGroupName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Request Location
    function onGetRequestLocationExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblRequestLocation.lblLink').click(function () {
        openSearchDialog('location', onGetRequestLocationExpression(), function (value) {
            $('#<%=txtRequestLocationCode.ClientID %>').val(value);
            onTxtRequestLocationChanged(value);
        });
    });

    $('#<%=txtRequestLocationCode.ClientID %>').change(function () {
        onTxtRequestLocationChanged($(this).val());
    });

    function onTxtRequestLocationChanged(value) {
        var filterExpression = onGetRequestLocationExpression() + " AND LocationCode = '" + value + "'";
        Methods.getObject('GetLocationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRequestLocationID.ClientID %>').val(result.LocationID);
                $('#<%=txtRequestLocationCode.ClientID %>').val(result.LocationCode);
                $('#<%=txtRequestLocationName.ClientID %>').val(result.LocationName);
            }
            else {
                $('#<%=hdnRequestLocationID.ClientID %>').val('');
                $('#<%=txtRequestLocationCode.ClientID %>').val('');
                $('#<%=txtRequestLocationName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Distribution Location
    function onGetDistributionLocationExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblDistributionLocation.lblLink').click(function () {
        openSearchDialog('location', onGetDistributionLocationExpression(), function (value) {
            $('#<%=txtDistributionLocationCode.ClientID %>').val(value);
            onTxtDistributionLocationChanged(value);
        });
    });

    $('#<%=txtDistributionLocationCode.ClientID %>').change(function () {
        onTxtDistributionLocationChanged($(this).val());
    });

    function onTxtDistributionLocationChanged(value) {
        var filterExpression = onGetDistributionLocationExpression() + " AND LocationCode = '" + value + "'";
        Methods.getObject('GetLocationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDistributionLocationID.ClientID %>').val(result.LocationID);
                $('#<%=txtDistributionLocationCode.ClientID %>').val(result.LocationCode);
                $('#<%=txtDistributionLocationName.ClientID %>').val(result.LocationName);
            }
            else {
                $('#<%=hdnDistributionLocationID.ClientID %>').val('');
                $('#<%=txtDistributionLocationCode.ClientID %>').val('');
                $('#<%=txtDistributionLocationName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Restriction
    $('#lblRestriction.lblLink').click(function () {
        openSearchDialog('restriction', 'isDeleted = 0', function (value) {
            $('#<%=txtRestrictionCode.ClientID %>').val(value);
            onTxtRestrictionCodeChanged(value);
        });
    });

    $('#<%=txtRestrictionCode.ClientID %>').change(function () {
        onTxtRestrictionCodeChanged($(this).val());
    });

    function onTxtRestrictionCodeChanged(value) {
        var filterExpression = "RestrictionCode = '" + value + "'";
        Methods.getObject('GetRestrictionHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRestrictionID.ClientID %>').val(result.RestrictionID);
                $('#<%=txtRestrictionName.ClientID %>').val(result.RestrictionName);
            }
            else {
                $('#<%=hdnRestrictionID.ClientID %>').val('');
                $('#<%=txtRestrictionCode.ClientID %>').val('');
                $('#<%=txtRestrictionName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        //        $('#<%=txtLocationCode.ClientID %>').removeAttr('readonly');
        $('#<%=txtLocationCode.ClientID %>').val('');
        $('#<%=txtLocationName.ClientID %>').val('');
        $('#<%=txtShortName.ClientID %>').val('');
        $('#<%=hdnLocationID.ClientID %>').val('');
        $('#<%=chkIsAvailable.ClientID %>').prop('checked', false);
        $('#<%=chkIsAllowOverIssued.ClientID %>').prop('checked', false);
        $('#<%=chkIsNettable.ClientID %>').prop('checked', false);
        $('#<%=chkIsHoldForTransaction.ClientID %>').prop('checked', false);
        $('#<%=hdnRestrictionID.ClientID %>').val('');
        $('#<%=txtRestrictionCode.ClientID %>').val('');
        $('#<%=txtRestrictionName.ClientID %>').val('');
        $('#<%=hdnItemGroupID.ClientID %>').val('');
        $('#<%=txtItemGroupCode.ClientID %>').val('');
        $('#<%=txtItemGroupName.ClientID %>').val('');
        $('#<%=chkIsMinMaxReadOnly.ClientID %>').prop('checked', false);
        $('#<%=chkisUsingLocationAverageQty.ClientID %>').prop('checked', false);
        $('#<%=chkisUsingLocationPatient.ClientID %>').prop('checked', false);
        cboTransactionType.SetValue();
        cboGCLocationGroup.SetValue();
        cboHealthcareUnit.SetValue();
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        $('#containerPopupEntryData').hide();
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var locationID = $row.find('.hdnLocationID').val();
            $('#<%=hdnLocationID.ClientID %>').val(locationID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var locationID = $row.find('.hdnLocationID').val();
        var locationCode = $row.find('.hdnLocationCode').val();
        var locationName = $row.find('.hdnLocationName').val();
        var restrictionID = $row.find('.hdnRestrictionID').val();
        var restrictionCode = $row.find('.hdnRestrictionCode').val();
        var restrictionName = $row.find('.hdnRestrictionName').val();
        var itemGroupID = $row.find('.hdnItemGroupID').val();
        var itemGroupCode = $row.find('.hdnItemGroupCode').val();
        var itemGroupName = $row.find('.hdnItemGroupName').val();
        var requestLocationID = $row.find('.hdnRequestLocationID').val();
        var requestLocationCode = $row.find('.hdnRequestLocationCode').val();
        var requestLocationName = $row.find('.hdnRequestLocationName').val();
        var distributionLocationID = $row.find('.hdnDistributionLocationID').val();
        var distributionLocationCode = $row.find('.hdnDistributionLocationCode').val();
        var distributionLocationName = $row.find('.hdnDistributionLocationName').val();
        var shortName = $row.find('.hdnShortName').val();
        var gcLocationGroup = $row.find('.hdnGCLocationGroup').val();
        var gcHealthcareUnit = $row.find('.hdnGCHealthcareUnit').val();
        var gcItemRequestType = $row.find('.hdnGCItemRequestType').val();
        var isAvailable = ($row.find('.hdnIsAvailable').val() == 'True');
        var isAllowOverIssued = ($row.find('.hdnIsAllowOverIssued').val() == 'True');
        var isNettable = ($row.find('.hdnIsNettable').val() == 'True');
        var isHoldForTransaction = ($row.find('.hdnIsHoldForTransaction').val() == 'True');
        var isMinMaxReadOnly = ($row.find('.hdnIsMinMaxReadOnly').val() == 'True');
        var isUsingLocationAverageQty = ($row.find('.hdnIsUsingLocationAverageQty').val() == 'True');
        var IsPatientUseLocation = ($row.find('.hdnIsPatientUseLocation').val() == 'True');
        $('#<%=txtLocationCode.ClientID %>').val(locationCode);
        $('#<%=txtLocationName.ClientID %>').val(locationName);
        $('#<%=hdnLocationID.ClientID %>').val(locationID);
        $('#<%=txtShortName.ClientID %>').val(shortName);
        $('#<%=hdnRestrictionID.ClientID %>').val(restrictionID);
        $('#<%=txtRestrictionCode.ClientID %>').val(restrictionCode);
        $('#<%=txtRestrictionName.ClientID %>').val(restrictionName);
        $('#<%=hdnItemGroupID.ClientID %>').val(itemGroupID);
        $('#<%=txtItemGroupCode.ClientID %>').val(itemGroupCode);
        $('#<%=txtItemGroupName.ClientID %>').val(itemGroupName);
        $('#<%=hdnRequestLocationID.ClientID %>').val(requestLocationID);
        $('#<%=txtRequestLocationCode.ClientID %>').val(requestLocationCode);
        $('#<%=txtRequestLocationName.ClientID %>').val(requestLocationName);
        $('#<%=hdnDistributionLocationID.ClientID %>').val(distributionLocationID);
        $('#<%=txtDistributionLocationCode.ClientID %>').val(distributionLocationCode);
        $('#<%=txtDistributionLocationName.ClientID %>').val(distributionLocationName);
        $('#<%=chkIsAvailable.ClientID %>').prop('checked', isAvailable);
        $('#<%=chkIsAllowOverIssued.ClientID %>').prop('checked', isAllowOverIssued);
        $('#<%=chkIsNettable.ClientID %>').prop('checked', isNettable);
        $('#<%=chkIsHoldForTransaction.ClientID %>').prop('checked', isHoldForTransaction);
        $('#<%=chkIsMinMaxReadOnly.ClientID %>').prop('checked', isMinMaxReadOnly);
        $('#<%=chkisUsingLocationAverageQty.ClientID %>').prop('checked', isUsingLocationAverageQty);
        $('#<%=chkisUsingLocationPatient.ClientID %>').prop('checked', IsPatientUseLocation);
        cboGCLocationGroup.SetValue(gcLocationGroup);
        cboTransactionType.SetValue(gcItemRequestType);
        cboHealthcareUnit.SetValue(gcHealthcareUnit);
        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        $('#containerImgLoadingViewPopup').hide();
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion
</script>
<div style="height: 550px">
    <input type="hidden" id="hdnParentLocationID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Gudang")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParentLocationCode" ReadOnly="true" Width="150%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Gudang")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParentLocationName" ReadOnly="true" Width="150%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnLocationID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 60%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td style="padding: 5px; vertical-align: top">
                                    <table style="width: 100%">
                                        <colgroup>
                                            <col style="width: 100%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Kode Lokasi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocationCode" Width="120px" CssClass="required" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Nama Lokasi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocationName" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Nama Singkat")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShortName" CssClass="required" Width="200px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblRestriction">
                                                    <%=GetLabel("Status Lokasi")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnRestrictionID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtRestrictionCode" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRestrictionName" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Jenis Item")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboGCLocationGroup" ClientInstanceName="cboGCLocationGroup"
                                                    Width="350px">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Tipe Transaksi")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboTransactionType" ClientInstanceName="cboTransactionType"
                                                    Width="305px">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Unit")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox runat="server" ID="cboHealthcareUnit" ClientInstanceName="cboHealthcareUnit"
                                                    Width="350px">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblItemGroup">
                                                    <%=GetLabel("Kelompok Item")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblRequestLocation">
                                                    <%=GetLabel("Lokasi Permintaan")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnRequestLocationID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtRequestLocationCode" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRequestLocationName" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblDistributionLocation">
                                                    <%=GetLabel("Lokasi Distribusi")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnDistributionLocationID" value="" runat="server" />
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 120px" />
                                                        <col style="width: 3px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtDistributionLocationCode" Width="100%" runat="server" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDistributionLocationName" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding: 5px; vertical-align: top">
                                    <table style="width: 100%">
                                        <tr style="display: none">
                                            <td>
                                                <asp:CheckBox ID="chkIsAllowOverIssued" runat="server" />
                                                <%=GetLabel("Allow Over Issued")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsMinMaxReadOnly" runat="server" />
                                                <%=GetLabel("Proses Min Max dihitung Sistem")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkisUsingLocationAverageQty" runat="server" />
                                                <%=GetLabel("Is Using Location Average Qty")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkisUsingLocationPatient" runat="server" />
                                                <%=GetLabel("Is Patient Location")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="visibility: hidden">
                                                <asp:CheckBox ID="chkIsAvailable" runat="server" />
                                                <%=GetLabel("Include Balance in Available Stock Information")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="visibility: hidden">
                                                <asp:CheckBox ID="chkIsNettable" runat="server" />
                                                <%=GetLabel("Nettable")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="visibility: hidden">
                                                <asp:CheckBox ID="chkIsHoldForTransaction" runat="server" />
                                                <%=GetLabel("Lock Down")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnLocationID" value="<%#: Eval("LocationID")%>" />
                                                <input type="hidden" class="hdnLocationCode" value="<%#: Eval("LocationCode")%>" />
                                                <input type="hidden" class="hdnLocationName" value="<%#: Eval("LocationName")%>" />
                                                <input type="hidden" class="hdnRestrictionID" value="<%#: Eval("RestrictionID")%>" />
                                                <input type="hidden" class="hdnRestrictionCode" value="<%#: Eval("RestrictionCode")%>" />
                                                <input type="hidden" class="hdnRestrictionName" value="<%#: Eval("RestrictionName")%>" />
                                                <input type="hidden" class="hdnItemGroupID" value="<%#: Eval("ItemGroupID")%>" />
                                                <input type="hidden" class="hdnItemGroupCode" value="<%#: Eval("ItemGroupCode")%>" />
                                                <input type="hidden" class="hdnItemGroupName" value="<%#: Eval("ItemGroupName1")%>" />
                                                <input type="hidden" class="hdnRequestLocationID" value="<%#: Eval("RequestLocationID")%>" />
                                                <input type="hidden" class="hdnRequestLocationCode" value="<%#: Eval("RequestLocationCode")%>" />
                                                <input type="hidden" class="hdnRequestLocationName" value="<%#: Eval("RequestLocationName")%>" />
                                                <input type="hidden" class="hdnDistributionLocationID" value="<%#: Eval("DistributionLocationID")%>" />
                                                <input type="hidden" class="hdnDistributionLocationCode" value="<%#: Eval("DistributionLocationCode")%>" />
                                                <input type="hidden" class="hdnDistributionLocationName" value="<%#: Eval("DistributionLocationName")%>" />
                                                <input type="hidden" class="hdnShortName" value="<%#: Eval("ShortName")%>" />
                                                <input type="hidden" class="hdnGCLocationGroup" value="<%#: Eval("GCLocationGroup")%>" />
                                                <input type="hidden" class="hdnGCHealthcareUnit" value="<%#: Eval("GCHealthcareUnit")%>" />
                                                <input type="hidden" class="hdnGCItemRequestType" value="<%#: Eval("GCItemRequestType")%>" />
                                                <input type="hidden" class="hdnIsAvailable" value="<%#: Eval("IsAvailable")%>" />
                                                <input type="hidden" class="hdnIsAllowOverIssued" value="<%#: Eval("IsAllowOverIssued")%>" />
                                                <input type="hidden" class="hdnIsNettable" value="<%#: Eval("IsNettable")%>" />
                                                <input type="hidden" class="hdnIsHoldForTransaction" value="<%#: Eval("IsHoldForTransaction")%>" />
                                                <input type="hidden" class="hdnIsMinMaxReadOnly" value="<%#: Eval("IsMinMaxReadOnly")%>" />
                                                <input type="hidden" class="hdnIsUsingLocationAverageQty" value="<%#: Eval("IsUsingLocationAverageQty")%>" />
                                                <input type="hidden" class="hdnIsPatientUseLocation" value="<%#: Eval("IsPatientUseLocation")%>" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="LocationCode" HeaderText="Kode Lokasi">
                                            <HeaderStyle Width="150px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LocationName" HeaderText="Nama Lokasi" />
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="onRefreshControl();pcRightPanelContent.Hide();" />
    </div>
</div>
