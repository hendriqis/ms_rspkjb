<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARNoteCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARNoteCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ARNoteCtl">
    setDatePicker('<%=txtARNoteDate.ClientID %>');
    $('#<%=txtARNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#lblARNoteAddData').die('click');
    $('#lblARNoteAddData').live('click', function () {
        $('#<%=hdnARNoteID.ClientID %>').val("");
        $('#<%=txtNoteText.ClientID %>').val("");
        $('#containerARNotesEntryData').show();
    });

    $('#btnARNotesCancel').click(function () {
        $('#containerARNotesEntryData').hide();
    });

    $('#btnARNotesSave').click(function (evt) {
        if (IsValid(evt, 'fsARNotes', 'mpARNotes'))
            cbpARNotes.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnARNoteID.ClientID %>').val(entity.ID);
                cbpARNotes.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnARNoteID.ClientID %>').val(entity.ID);
        $('#<%=txtNoteText.ClientID %>').val(entity.NoteText);
        $('#<%=txtARNoteDate.ClientID %>').val(entity.NoteDate);
        $('#<%=txtARNoteTime.ClientID %>').val(entity.NoteTime);
        
        $('#containerARNotesEntryData').show();
    });

    function onARNotesEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerARNotesEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnTransactionCode" />
    <input type="hidden" value="" runat="server" id="hdnTransactionID" />
    <input type="hidden" value="" runat="server" id="hdnBusinessPartnerID" />
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
                                <%=GetLabel("TransactionNo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("BusinessPartner")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBusinessPartnerName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerARNotesEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnARNoteID" runat="server" value="" />
                    <fieldset id="fsARNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Tanggal") %>
                                    -
                                    <%=GetLabel("Waktu") %>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 1px; width: 145px">
                                                <asp:TextBox ID="txtARNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="width: 5px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtARNoteTime" Width="100px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Note Text")%></label>
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
                                                <input type="button" id="btnARNotesSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnARNotesCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpARNotes" runat="server" Width="100%" ClientInstanceName="cbpARNotes"
                    ShowLoadingPanel="false" OnCallback="cbpARNotes_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onARNotesEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdARNotes" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("cfNoteDateInDatePickerFormat") %>" bindingfield="NoteDate" />
                                                <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                                <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfNoteDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="NoteTime" HeaderText="Waktu" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="NoteText" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblARNoteAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
