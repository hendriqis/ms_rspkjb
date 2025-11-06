<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeTransactionDetailRemarksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeTransactionDetailRemarksCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_DetailInfoctl">
    $('#btnDetailInfoCancel').click(function () {
        $('#containerDetailInfoEntryData').hide();
    });

    $('#btnDetailInfoSave').click(function (evt) {
        if (IsValid(evt, 'fsDetailInfo', 'mpDetailInfo'))
            cbpDetailInfo.PerformCallback('save');
        return false;
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        $('#<%=txtNoteText.ClientID %>').val(entity.Remarks);
        
        $('#containerDetailInfoEntryData').show();
    });

    function onDetailInfoEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerDetailInfoEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnTransactionID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerDetailInfoEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <fieldset id="fsDetailInfo" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Item Pemeriksaan") %>
                                  </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 1px; width: 145px">
                                                <asp:TextBox ID="txtItemCode" Width="100px" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName" Width="300px" runat="server" ReadOnly="true"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align:top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Catatan Klinis")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoteText" Width="100%" CssClass="required" runat="server" 
                                    TextMode="Multiline" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnDetailInfoSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnDetailInfoCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpDetailInfo" runat="server" Width="100%" ClientInstanceName="cbpDetailInfo"
                    ShowLoadingPanel="false" OnCallback="cbpDetailInfo_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onDetailInfoEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="ItemCode" HeaderText="Kode" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="ItemName1" HeaderText="Nama Pemeriksaan" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Pemeriksaan tidak tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
