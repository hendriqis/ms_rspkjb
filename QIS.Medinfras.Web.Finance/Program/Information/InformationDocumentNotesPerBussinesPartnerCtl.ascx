<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationDocumentNotesPerBussinesPartnerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InformationDocumentNotesPerBussinesPartnerCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_custdocctl1">
   

    

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
        }
        hideLoadingPanel();
    };

   
     

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

     
</script>
<style>
.displaynone{display:none;}
</style>
<div style="height: 525px; overflow-y: auto">
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <input type="hidden" id="hdnVirtualDirectory" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Rekanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBussinesPartnerName" Width="160px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table> 
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px" CssClass="displaynone"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="displaynone"></ItemStyle>
                                        </asp:TemplateField>
                                         <asp:BoundField DataField="LogDateInString" HeaderText="Tanggal" ItemStyle-CssClass="tdFileName"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" CssClass="tdFileName"></ItemStyle>
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="LogRemarks" HeaderText="CATATAN" ItemStyle-CssClass="tdFileName"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" CssClass="tdFileName"></ItemStyle>
                                        </asp:BoundField>
                                       
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
               
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
