<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderCloseNewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderCloseNewCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_pocloseandnewctl">
    $(function () {
        hideLoadingPanel();
    });

    //#region Supplier
    function getSupplierFilterExpression() {
        var filterExpression = "<%:filterExpressionSupplier %>";
        return filterExpression;
    }

    $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
            $('#<%=txtSupplierCode.ClientID %>').val(value);
            onTxtSupplierChanged(value);
        });
    });

    $('#<%=txtSupplierCode.ClientID %>').change(function () {
        onTxtSupplierChanged($(this).val());
    });

    function onTxtSupplierChanged(value) {
        var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnSupplierID.ClientID %>').val('');
                $('#<%=txtSupplierCode.ClientID %>').val('');
                $('#<%=txtSupplierName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=btnCloseNew.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            showToastConfirmation("Are You Sure Want To Close?", function (result) {
                if (result) {
                    cbpCloseNewPO.PerformCallback('void');
                }
            })
        }
    });

    function oncboClosedPOReasonValueChanged(evt) {
        if (cboClosedPOReason.GetValue() == 'X416^999') {
            $('#<%=trReason.ClientID %>').show();
        } else {
            $('#<%=trReason.ClientID %>').hide();
        }
    }

    function onCloseAndNewSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }

    function onCloseAndNewSuccess() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnCloseNew" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Close & New")%></div>
        </li>
    </ul>
</div>
<input type="hidden" id="hdnPurchaseOrderIDCtlClosedNew" runat="server" value="" />
<input type="hidden" id="hdnNewPurchaseOrderNo" runat="server" value="" />
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table>
            <colgroup>
                <col style="width: 180px" />
                <col style="width: 150px" />
                <col style="width: 300px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("No Pemesanan")%></label>
                </td>
                <td>
                    <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                    <asp:TextBox ID="txtPurchaseOrderNo" ReadOnly="true" Width="100%" runat="server" />
                </td>
                <td colspan="2" style="padding-left:30px">
                    <asp:CheckBox ID="chkIsUsingNewPrice" Checked="true" Width="100%" runat="server"
                        Text=" Menggunakan Harga Baru ?" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory lblLink" id="lblSupplier" runat="server">
                        <%=GetLabel("Supplier/Penyedia")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                        runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Alasan Tutup (Closed) PO") %></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboClosedPOReason" ClientInstanceName="cboClosedPOReason" runat="server"
                        Width="100%">
                        <ClientSideEvents ValueChanged="function(s,e) { oncboClosedPOReasonValueChanged(e); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Alasan Lain")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtReason" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpCloseNewPO" runat="server" Width="100%" ClientInstanceName="cbpCloseNewPO"
            ShowLoadingPanel="false" OnCallback="cbpCloseNewPO_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onCloseAndNewSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
