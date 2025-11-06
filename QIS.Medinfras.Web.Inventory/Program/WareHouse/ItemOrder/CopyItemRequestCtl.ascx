<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyItemRequestCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.CopyItemRequestCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_copyitemrequest">
    $(function () {
        setDatePicker('<%=txtItemRequestDateCopy.ClientID %>');
        $('#<%=txtItemRequestDateCopy.ClientID %>').datepicker('option', 'maxDate', '0');

        hideLoadingPanel();
    });

    //#region Location To Copy
    function getLocationFilterExpressionToCopy() {
        var filterExpression = "<%:filterExpressionLocationToCopy %>";
        return filterExpression;
    }

    $('#<%=lblLocationToCopy.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('locationroleuser', getLocationFilterExpressionToCopy(), function (value) {
            $('#<%=txtLocationCodeToCopy.ClientID %>').val(value);
            onTxtLocationToCodeCopyChanged(value);
        });
    });

    $('#<%=txtLocationCodeToCopy.ClientID %>').live('change', function () {
        onTxtLocationToCodeCopyChanged($(this).val());
    });

    function onTxtLocationToCodeCopyChanged(value) {
        var filterExpression = getLocationFilterExpressionToCopy() + "LocationCode = '" + value + "'";
        Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
            if (result != null) {
                var locFrom = $('#<%=hdnLocationIDFrom.ClientID %>').val();
                var locTo = $('#<%=hdnLocationIDTo.ClientID %>').val();
                if (result.LocationID != locFrom && result.LocationID != locTo) {
                    $('#<%=hdnLocationIDToCopy.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationNameToCopy.ClientID %>').val(result.LocationName);
                } else {
                    showToast('Information', 'Lokasi ini tidak dapat dipilih karena merupakan lokasi permintaan asal.');
                    $('#<%=hdnLocationIDToCopy.ClientID %>').val('');
                    $('#<%=txtLocationCodeToCopy.ClientID %>').val('');
                    $('#<%=txtLocationNameToCopy.ClientID %>').val('');
                }
            }
            else {
                $('#<%=hdnLocationIDToCopy.ClientID %>').val('');
                $('#<%=txtLocationCodeToCopy.ClientID %>').val('');
                $('#<%=txtLocationNameToCopy.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=btnSaveCopyItemRequest.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpCopyItemRequest.PerformCallback('save');
        }
    });

    function onAfterSaveCopyItemRequestCtl() {
        pcRightPanelContent.Hide();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnSaveCopyItemRequest" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnItemRequestID" runat="server" value="" />
        <input type="hidden" id="hdnProductLineID" runat="server" value="" />
        <input type="hidden" id="hdnIsUsedProductLine" runat="server" value="0" />
        <table>
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 130px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemRequestNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 130px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtItemRequestDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemRequestTime" Width="100px" CssClass="time" runat="server"
                                                ReadOnly="true" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <h4>
            <label class="lblNormal" runat="server" id="Label1">
                <%=GetLabel("Copy Permintaan Pembelian")%></label>
        </h4>
        <table>
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 130px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtItemRequestDateCopy" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemRequestTimeCopy" Width="100px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocationFromCopy">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFromCopy" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeFromCopy" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameFromCopy" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblLocationToCopy">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDToCopy" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeToCopy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameToCopy" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpCopyItemRequest" runat="server" Width="100%" ClientInstanceName="cbpCopyItemRequest"
            ShowLoadingPanel="false" OnCallback="cbpCopyItemRequest_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail') {
                        showToast('Save Failed', result[1]);
                    }
                    else {
                        showToast('INFORMATION', 'Berhasil salin permintaan barang di nomor permintaan <b>' + result[1]) + '</b>.';
                        onAfterSaveCopyItemRequestCtl();
                    }
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
