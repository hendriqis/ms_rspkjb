<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicRevenueSharingCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.Finance.Program.ParamedicRevenueSharingCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>


<script type="text/javascript" id="dxss_paramedicrevenusharingctl">

//#region Button
    $('#<%=btnRevenueSharingClassEdit.ClientID %>').click(function () {

        $('#<%=btnRevenueSharingClassEdit.ClientID %>').hide();
        $('#<%=btnRevenueSharingClassSave.ClientID %>').show();
        $('#<%=btnRevenueSharingClassCancel.ClientID %>').show();

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
            var keyField = $(this).find('.keyField').html();
            var clientInstanceName = 'cboRevenueClass' + keyField;
            var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
            cbo.SetEnabled(true);
        });
    });

    $('#<%=btnRevenueSharingClassSave.ClientID %>').click(function () {
        var result = '';
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
            if (result != "")
                result += "|";
            var keyField = $(this).find('.keyField').html();
            var clientInstanceName = 'cboRevenueClass' + keyField;
            var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
            result += keyField + ';' + cbo.GetValue();
        });
        $('#<%=hdnResult.ClientID %>').val(result);
        $('#<%=btnRevenueSharingClassEdit.ClientID %>').show();
        $('#<%=btnRevenueSharingClassSave.ClientID %>').hide();
        $('#<%=btnRevenueSharingClassCancel.ClientID %>').hide();

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
            var keyField = $(this).find('.keyField').html();
            var clientInstanceName = 'cboRevenueClass' + keyField;
            var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
            cbo.SetEnabled(false);
        });
        cbpRevenueSharingClassProcess.PerformCallback('save');
    });

    $('#<%=btnRevenueSharingClassCancel.ClientID %>').click(function () {
        $('#<%=btnRevenueSharingClassEdit.ClientID %>').show();
        $('#<%=btnRevenueSharingClassSave.ClientID %>').hide();
        $('#<%=btnRevenueSharingClassCancel.ClientID %>').hide();

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0)').each(function () {
            var keyField = $(this).find('.keyField').html();
            var clientInstanceName = 'cboRevenueClass' + keyField;
            var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
            cbo.SetEnabled(false);
        });
        cbpEntryPopupView.PerformCallback('refresh');
    });

    //#endregion
    function onSaveRevenueSharingClassSuccess() {

    }
</script>


<input type="hidden" value="" id="hdnResult" runat="server" />
<div style="height:600px; overflow-y:hidden; overflow-x:hidden">
    <div class="toolbarArea" style="margin-bottom: 5px">
        <ul>
            <li runat="server" id="btnRevenueSharingClassEdit"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbEdit.png")%>' alt="" /><div><%=GetLabel("Edit")%></div></li>
            <li runat="server" id="btnRevenueSharingClassSave" style="display:none"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div><%=GetLabel("Save")%></div></li>
            <li runat="server" id="btnRevenueSharingClassCancel" style="display:none"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div><%=GetLabel("Cancel")%></div></li>
        </ul>
    </div>
    <div class="pageTitle"><%=GetLabel("Kelas Perawatan")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:120%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rumah Sakit")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHealthcare" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtItem" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Dokter / Paramedis")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParamedic" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Peranan")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtParamedicRole" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr> 
                </table>

                <div id="containerPopupEntryData" style="margin-top:8px;display:none;">
                    <input type="hidden" id="hdnRowClassID" runat="server" value="" />
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                    </fieldset>
                </div>
                      <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">                           
                                <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ClassID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:BoundField DataField="ClassName" HeaderText="Nama Kelas" HeaderStyle-Width="180px" />
                                        <asp:TemplateField HeaderText="Jasa Medis">
                                            <ItemTemplate>
                                                <input type="hidden" runat="server" id="hdnRowClassID" class="hdnRowClassID" />                                        
                                                <dxe:ASPxComboBox ID="cboRevenueClass" ClientEnabled="false" ClientInstanceName='<%#: "cboRevenueClass" + DataBinder.Eval(Container.DataItem, "ClassID")%>' runat="server" Width="100%" />                                        
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
      </table>

      <dxcp:ASPxCallbackPanel ID="cbpRevenueSharingClassProcess" runat="server" Width="100%" ClientInstanceName="cbpRevenueSharingClassProcess"
            ShowLoadingPanel="false" OnCallback="cbpRevenueSharingClassProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onSaveRevenueSharingClassSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
     <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

