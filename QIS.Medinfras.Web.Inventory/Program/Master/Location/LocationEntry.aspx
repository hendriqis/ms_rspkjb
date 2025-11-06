<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="LocationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.LocationEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
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
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("healthcareID", $('#<%=hdnHealthcareID.ClientID %>').val()));
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Gudang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationCode" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Gudang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Singkat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtShortName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblRestriction">
                                <%=GetLabel("Status Transaksi")%></label>
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
                                        <asp:TextBox ID="txtRestrictionName" Width="100%" runat="server" />
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
                                Width="305px">
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
                                Width="305px">
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
                                        <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtRequestLocationName" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtDistributionLocationName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                            </label>
                        </td>
                        <td>
                            <table style="width: 120%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 250px" />
                                    <col style="width: 380px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsMinMaxReadOnly" runat="server" />
                                        <%=GetLabel("Proses Min Max dihitung Sistem")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkisUsingLocationAverageQty" runat="server" />
                                        <%=GetLabel("Is Using Location Average Qty")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParent"><%=GetLabel("Parent")%></label></td>
                        <td>
                            <input type="hidden" id="hdnParentID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParentName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <tr style="display: none">
                        <td>
                            <asp:CheckBox ID="chkIsHeader" runat="server" Checked="true" onclick="javascript: return false;" />
                            <%=GetLabel("Is Header For Other")%>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <asp:CheckBox ID="chkIsAvailable" runat="server" />
                            <%=GetLabel("Include Balance in Available Stock Information")%>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <asp:CheckBox ID="chkIsAllowOverIssued" runat="server" />
                            <%=GetLabel("Allow Over Issued")%>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <asp:CheckBox ID="chkIsNettable" runat="server" />
                            <%=GetLabel("Nettable")%>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <asp:CheckBox ID="chkIsHoldForTransaction" runat="server" />
                            <%=GetLabel("Lock Down")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
