<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARProportionalCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARProportionalCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ARProportionalCtl">

    $('#btnProcess').live('click', function (evt) {
        cbpProcess.PerformCallback('process');
        return true;
    });

    function oncbpProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'process') {
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

</script>
<div style="height: 250px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnARInvoiceIDCtl" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 500px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Invoice")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtARInvoiceNoCtl" Width="150px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnProcess" value='<%= GetLabel("Proses Proporsional")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
