<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadTarifBookEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.DownloadTarifBookEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_DownloadTarifBookEntryCtl">


    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        } else if (param[0] == 'download') {
            if (param[1] == 'success') {
                var hdnParam = $('#<%=hdnStringCSV.ClientID %>').val();
                downloadCSVDocument(hdnParam);
            }
        }
        hideLoadingPanel();
    }

    function downloadCSVDocument(stringparam) {
        var fileName = $('#<%=hdnFileName.ClientID %>').val();

        var link = document.createElement("a");
        link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
        link.download = fileName;
        link.click();
    }
    $('#btnDownloadFile').live('click', function () {
        //document.getElementById("btnSample").click();
        //onCustomButtonClick('download');
        //onCustomButtonClick('download');
        cbpEntryPopupView.PerformCallback('download');
    });
     
     
</script>
<input type="hidden" id="hdnBookID" runat="server"/>
<input type="hidden" id="hdnFileName" runat="server"/> 
    <table class="tblContentArea">
     <tr>
            <td>Jenis Item</td>
            <td> <asp:RadioButtonList ID="rblCheckAll" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Non Inventory" Value="0" Selected="True"  />
                    <asp:ListItem Text="Inventory" Value="1" />   
                   </asp:RadioButtonList>
             </td>
        </tr>
    <tr>
        <td></td>
        <td>  <input type="button" id="btnDownloadFile" value='<%= GetLabel("download")%>' />  
            <asp:Button runat="server" ID="btnSample" ClientIDMode="Static" Text="" style="display:none;" OnClick="btnSample_Click" />
         </td>
    </tr>
    </table>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
       
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                            </asp:Panel>
                            <input type="hidden" id="hdnStringCSV" runat="server"/>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                
            </td>
        </tr>
    </table>
    
 