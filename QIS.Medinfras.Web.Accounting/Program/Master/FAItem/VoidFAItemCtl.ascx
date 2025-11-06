<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VoidFAItemCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.VoidFAItemCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_voidfaitemctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnVoidFAItem.ClientID %>').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpVoidFAItem.PerformCallback('void');
        }
    });

    //#region Product Line
    $('#lblProductLine.lblLink').click(function () {
        var filterPL = "IsDeleted = 0 AND GCItemType IN ('X001^002','X001^003','X001^008','X001^009')";
        openSearchDialog('productline', filterPL, function (value) {
            $('#<%=txtProductLineCode.ClientID %>').val(value);
            onTxtProductLineCodeChanged(value);
        });
    });

    $('#<%=txtProductLineCode.ClientID %>').change(function () {
        onTxtProductLineCodeChanged($(this).val());
    });

    function onTxtProductLineCodeChanged(value) {
        var filterExpression = "ProductLineCode = '" + value + "'";
        Methods.getObject('GetProductLineList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
            }
            else {
                $('#<%=hdnProductLineID.ClientID %>').val('');
                $('#<%=txtProductLineCode.ClientID %>').val('');
                $('#<%=txtProductLineName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onVoidFAItem() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnVoidFAItem" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Void")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnTransactionCodeVoidFAItemCtl" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" id="lblProductLine">
                        <%=GetLabel("Product Line")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpVoidFAItem" runat="server" Width="100%" ClientInstanceName="cbpVoidFAItem"
            ShowLoadingPanel="false" OnCallback="cbpVoidFAItem_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onVoidFAItem();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
