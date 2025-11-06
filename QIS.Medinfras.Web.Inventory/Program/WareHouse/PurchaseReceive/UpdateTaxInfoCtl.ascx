<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateTaxInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.UpdateTaxInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_updatetaxctl">
    $(function () {
        hideLoadingPanel();

        setDatePicker('<%=txtTaxDate.ClientID %>');
    });

    $('#<%=btnSaveTaxInfo.ClientID %>').click(function (evt) {
        if ($('#<%=hdnFlag.ClientID %>').val() == "1") {
            var messageConfirmation = "Data faktur pajak penerimaan ini sudah ada di proses tidak faktur, mau mengganti data faktur?";
            showToastConfirmation(messageConfirmation, function (result) {
                if (result) {
                    if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                        cbpUpdateTaxInfo.PerformCallback('update');
                    }
                }
                else {
                    if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                        cbpUpdateTaxInfo.PerformCallback('refresh');
                    }
                }
            });
        }
        else {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                cbpUpdateTaxInfo.PerformCallback('refresh');
            }
        }
    });

    function onSaveTaxInfoCtl() {
        pcRightPanelContent.Hide();
        onRefreshControl();
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnSaveTaxInfo" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnPurchaseReceiveID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnFlag" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 250px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Faktur Pajak")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtTaxNo" Width="200px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Faktur Pajak")%></label>
                </td>
                <td style="padding-right: 1px; width: 200px">
                    <asp:TextBox ID="txtTaxDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpUpdateTaxInfo" runat="server" Width="100%" ClientInstanceName="cbpUpdateTaxInfo"
            ShowLoadingPanel="false" OnCallback="cbpUpdateTaxInfo_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSaveTaxInfoCtl();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
