<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillNoAntrianCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.FillNoAntrianCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PrintPrescriptionByTypeCtl">
    $('#btnSaveCtl').click(function (evt) {
        cbpGetAntrianProcessNonBridging.PerformCallback('save');
    });
    function onCbpGetAntrianProcessNonBridgingEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[1] == 'fail') {
            displayMessageBox('Antrian Pelayanan Resep', param[2]);
        }
        else {
            pcRightPanelContent.Hide();
        }
    }
</script>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<input type="hidden" id="hdnTransactionIDCtl" runat="server" />
<input type="hidden" id="hdnPrescriptionOrderIDCtl" runat="server" />
<div style="height: 100px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Resep")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>  
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Antrian")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAntrianNo" runat="server" Width="99%" />
                        </td>
                    </tr>                      
                    <tr id="btnSave" runat="server">
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpGetAntrianProcessNonBridging" runat="server" Width="100%" ClientInstanceName="cbpGetAntrianProcessNonBridging"
            ShowLoadingPanel="false" OnCallback="cbpGetAntrianProcessNonBridging_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpGetAntrianProcessNonBridgingEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
