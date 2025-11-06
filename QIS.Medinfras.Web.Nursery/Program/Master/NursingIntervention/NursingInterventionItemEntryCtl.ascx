<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingInterventionItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Nursing.Program.NursingInterventionItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>



<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnNursingInterventionID" value="" runat="server" />
    <input type="hidden" value="" id="hdnIsAdd" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Intervensi")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtNursingDiagnoseName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="display:none">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <input type="hidden" runat="server" id="hdnID" />
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:110px"/>
                                <col />
                            </colgroup>
                            <tr valign="top">
                                <td class="tdLabel" style="padding-top:5px"><label class="lblMandatory lblLink" id="lblNursingItem"><%=GetLabel("Aktifitas")%></label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNursingItemEntry" runat="server" TextMode="MultiLine" Wrap="true" Columns="55" Rows="3" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal" id="lblNursingItemType"><%=GetLabel("Jenis Aktifitas")%></label></td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox runat="server" Width="150px" ID="cboGCNursingItemType" ClientInstanceName="cboGCNursingItemType" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Urutan Tampilan")%></label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" Width="50px" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><asp:CheckBox runat="server" ID="chkIsEditableByUser" Text="Dapat diubah oleh pengguna" /> </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' class="btnEntryPopupSave w3-btn w3-hover-blue"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                             
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView1" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView1"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView1_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback1(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="pnlEntry" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdSaved" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;" /></td>
                                                        <td style="width:3px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("NursingInterventionItemID") %>" bindingfield="NursingInterventionItemID" />
                                                <input type="hidden" value="<%#:Eval("NursingItemText") %>" bindingfield="NursingItemText" />
                                                <input type="hidden" value="<%#:Eval("GCNursingItemType") %>" bindingfield="GCNursingItemType" />
                                                <input type="hidden" value="<%#:Eval("DisplayOrder") %>" bindingfield="DisplayOrder" />
                                                <input type="hidden" value="<%#:Eval("IsEditableByUser") %>" bindingfield="IsEditableByUser" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NursingInterventionItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="DisplayOrder" HeaderText="Urutan Tampilan" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="NursingItemText" HeaderText="Aktifitas Intervensi" ItemStyle-HorizontalAlign="Left"/>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada data aktifitas untuk intervensi keperawatan")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onCbpEntryPopupViewEndCallback1(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#lblEntryPopupAddData').click();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtNursingItemEntry.ClientID %>').val('');
        cboGCNursingItemType.SetValue('');
        $('#<%=txtDisplayOrder.ClientID %>').val('0');
        $('#<%=chkIsEditableByUser.ClientID %>').prop('checked', false);
        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#containerPopupEntryData').show();
    });
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });
    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView1.PerformCallback('save');
        return false;
    });

    //#region Nursing Item
    function onGetNursingItemFilterExpression() {
        var filterExpression = "<%:OnGetNursingItemFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNursingItem.lblLink').click(function () {
        openSearchDialog('nursingItem', onGetNursingItemFilterExpression(), function (value) {
            $('#<%=txtNursingItemEntry.ClientID %>').val(value);
        });
    });
    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.NursingInterventionItemID);
            cbpEntryPopupView1.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.NursingInterventionItemID);
        $('#<%=txtNursingItemEntry.ClientID %>').val(entity.NursingItemText);
        cboGCNursingItemType.SetValue(entity.GCNursingItemType);
        $('#<%=txtDisplayOrder.ClientID %>').val(entity.DisplayOrder);
        $('#<%=chkIsEditableByUser.ClientID %>').prop('checked', entity.IsEditableByUser == 'True');
        $('#containerPopupEntryData').show();
    });

</script>

