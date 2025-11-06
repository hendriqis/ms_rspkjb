<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetSEPCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.GetSEPCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<script type="text/javascript" id="dxss_GenerateSEPManualCtl">
  
    function checkTimeFormat(value) {
        if (value.substr(2, 1) == ':') {
            if (!value.match(/^\d\d:\d\d/)) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
            else if (parseInt(value.substr(0, 2)) >= 24 || parseInt(value.substr(3, 2)) >= 60) {
                displayErrorMessageBox('ERROR', "Format jam salah !");
            }
        }
        else {
            displayErrorMessageBox('ERROR', "Format jam salah !");
        }
    }
    $('#<%=btnGetdataPeserta.ClientID %>').live('click', function () {
        cbpGetDataPesertaView.PerformCallback('getDataPeserta');
    });
 
    function onCbpGetDataPesertaViewEndCallback(s) {
        hideLoadingPanel();
       
    }
    function onAfterSaveAddRecordEntryPopup() {
        pcRightPanelContent.Hide();
    }
</script>
  <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
                <input type="hidden" runat="server" id="hdnNoPeserta" value="" />
                <input type="hidden" runat="server" id="hdnIsBridgingVclaim" value="" />
                <input type="hidden" runat="server" id="hdnIsBridgingEklaim" value="" />

                 <input type="hidden" runat="server" id="hdnMedicalNo" value="" />
                 <input type="hidden" runat="server" id="hdnPatientName" value="" />
                 <input type="hidden" runat="server" id="hdnDOB" value="" />
                 <input type="hidden" runat="server" id="hdnGender" value="" />
                 <input type="hidden" runat="server" id="hdnoldNoSep" value = "" />
  
<div style="height: 100%; overflow-y: auto;">
     <input type="button" id="btnGetdataPeserta" class="btn" runat="server" value="Get Data SEP"/>
       <dxcp:ASPxCallbackPanel ID="cbpGetDataPesertaView" runat="server" Width="100%" ClientInstanceName="cbpGetDataPesertaView"
                ShowLoadingPanel="false" OnCallback="cbpGetDataPesertaView_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpGetDataPesertaViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
               <input type="hidden" runat="server" id="hdnIsGetPeserta" value="0" />
                    <table class="tblContentArea">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("No.SEP")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtNoSEP" runat="server"   >
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label >
                                    <%=GetLabel("Tanggal SEP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTglSEP" Width="120px" runat="server"  />
                            </td>
                        </tr>
                         
                         <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Diagnosa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnosaName"  runat="server" ReadOnly />
                            </td>
                        </tr>
                         <tr>
                            <td class="tdLabel">
                                <label >
                                    <%=GetLabel("catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCatatan"  runat="server" TextMode="MultiLine" Rows="5" Columns="30" ReadOnly />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
                </PanelCollection>
        </dxcp:ASPxCallbackPanel>
</div>
