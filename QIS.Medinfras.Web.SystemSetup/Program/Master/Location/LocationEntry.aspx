<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="LocationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.LocationEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad () {
            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                openSearchDialog('itemgroupdrugms', '', function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "'";
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
            //#region Parent
            $('#lblParent.lblLink').click(function () {
                var filterExpression = "IsHeader = 1 AND IsDeleted = 0";
                openSearchDialog('location', filterExpression, function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.LocationID);
                        $('#<%=txtParentName.ClientID %>').val(result.LocationName);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
            //#region Restriction
            $('#lblRestriction.lblLink').click(function () {
                openSearchDialog('restriction', '', function (value) {
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
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("healthcareID", $('#<%=hdnHealthcareID.ClientID %>').val()));
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Location")%></div>
    <table class="tblContentArea" style="width:100%">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Location Code")%></label></td>
                        <td><asp:TextBox ID="txtLocationCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Location Name")%></label></td>
                        <td><asp:TextBox ID="txtLocationName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Short Name")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblRestriction"><%=GetLabel("Restriction")%></label></td>
                        <td>
                            <input type="hidden" id="hdnRestrictionID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtRestrictionCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtRestrictionName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblItemGroup"><%=GetLabel("Item Group")%></label></td>
                        <td>
                            <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
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
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <tr><td><asp:CheckBox ID="chkIsHeader" runat="server" /> <%=GetLabel("Is Header For Other")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsAvailable" runat="server" /> <%=GetLabel("Include Balance in Available Stock Information")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsAllowOverIssued" runat="server" /> <%=GetLabel("Allow Over Issued")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsNettable" runat="server" /> <%=GetLabel("Nettable")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsHoldForTransaction" runat="server" /> <%=GetLabel("Lock Down")%></td></tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
