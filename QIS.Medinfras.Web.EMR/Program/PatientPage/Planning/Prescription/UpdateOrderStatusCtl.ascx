<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrderStatusCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.UpdateOrderStatusCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_UpdateOrderStatusCtl">
    $(function () {
    });

    function onBeforeProcess(param) {
        return true;
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterUpdateOrderStatus == 'function')
            onAfterUpdateOrderStatus(param);
    }
</script>
<input type="hidden" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />

<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Resep") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                runat="server" Width="235px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal/Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDateTime" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan Order")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Height="150px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
