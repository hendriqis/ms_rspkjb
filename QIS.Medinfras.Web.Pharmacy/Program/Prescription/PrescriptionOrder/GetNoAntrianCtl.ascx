<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetNoAntrianCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.GetNoAntrianCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PrintPrescriptionByTypeCtl">
    $('#btnPrintCtl').click(function (evt) {
        cbpGetAntrianProcess.PerformCallback('GetAntrian');
    });
    function onCbpGetAntrianProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'GetAntrian') {
            if (param[1] == 'fail') {
                displayMessageBox('Antrian Pelayanan Resep', param[2]);
            } else {
                ///PH-00090
                var filterExpression = $('#<%=hdnTransactionIDCtl.ClientID %>').val(); 
                openReportViewer("PH-00090", filterExpression);
            }

        }

        pcRightPanelContent.Hide();
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
<input type="hidden" id="hdnIsBridgingMedinlinkCtl" runat="server" />
<input type="hidden" id="hdnIsGetQueueNoMedinlinkCtl" runat="server" />
<input type="hidden" id="hdnDepartmentIDCtl" runat="server" />

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
                            <asp:TextBox ID="txtAntrianNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                      
                    <tr id="btnAmbilAntrian" runat="server">
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnPrintCtl" value='<%= GetLabel("Ambil Antrian")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpGetAntrianProcess" runat="server" Width="100%" ClientInstanceName="cbpGetAntrianProcess"
            ShowLoadingPanel="false" OnCallback="cbpGetAntrianProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpGetAntrianProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
