<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RLReportRowCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RLReportRowCtl" %>


<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>


<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtRowTitle.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');

        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var mainID = $row.find('.hdnMainID').val();
            $('#<%=hdnMainID.ClientID %>').val(mainID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var mainID = $row.find('.hdnMainID').val();
        var rowTitle = $row.find('.hdnRowTitle').val();
        var displayOrder = $row.find('.hdnDisplayOrder').val();

        $('#<%=hdnMainID.ClientID %>').val(mainID);
        $('#<%=txtRowTitle.ClientID %>').val(rowTitle);
        $('#<%=txtDisplayOrder.ClientID %>').val(displayOrder);

        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $('#containerPopupEntryData').show();
    });
     

    
    //#endregion

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
    }
</script>

<div style="height:440px; overflow-y:auto; overflow-x:hidden">
     <div class="pageTitle"><%=GetLabel("Baris Laporan RL")%></div>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Laporan RL")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="400px" runat="server" /></td>
                    </tr> 
                </table>

                <div id="containerPopupEntryData" style="margin-top:8px;display:none;">
                    <input type="hidden" id="hdnIsAdd" runat="server" value="" />
                    <input type="hidden" id="hdnMainID" runat="server" value="" />
                    <input type="hidden" id="hdnReportID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>                            
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Judul Baris")%></label></td>
                                <td><asp:TextBox ID="txtRowTitle" runat="server" Width="120px" CssClass="required" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Urutan Tampilan")%></label></td>
                                <td><asp:TextBox ID="txtDisplayOrder" runat="server" Width="300px" CssClass="required" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Tutup")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">                           
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                   <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" class="hdnMainID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnRowTitle" value="<%#: Eval("RowTitle")%>" />
                                                <input type="hidden" class="hdnDisplayOrder" value="<%#: Eval("DisplayOrder")%>" />                                         
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField DataField="RowTitle" HeaderText="Judul Baris" ItemStyle-CssClass="tdCustomerItemCode" />
                                        <asp:BoundField DataField="DisplayOrder" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="Urutan Tampilan" ItemStyle-CssClass="tdCustomerItemName" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
      </table>
     <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

