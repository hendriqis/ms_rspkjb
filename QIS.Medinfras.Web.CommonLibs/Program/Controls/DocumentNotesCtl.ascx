<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.DocumentNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_documentnotesctl">
    setDatePicker('<%=txtDocumentNoteDate.ClientID %>');
    $('#<%=txtDocumentNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#lblDocumentNoteAddData').die('click');
    $('#lblDocumentNoteAddData').live('click', function () {
        $('#<%=hdnDocumentNoteID.ClientID %>').val("");
        $('#<%=txtDocumentNoteText.ClientID %>').val("");
        $('#containerDocumentNotesEntryData').show();
    });

    $('#btnDocumentNotesCancel').click(function () {
        $('#containerDocumentNotesEntryData').hide();
    });

    $('#btnDocumentNotesSave').click(function (evt) {
        if (IsValid(evt, 'fsDocumentNotes', 'mpDocumentNotes'))
            cbpDocumentNotes.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnDocumentNoteID.ClientID %>').val(entity.IDNote);
                cbpDocumentNotes.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnDocumentNoteID.ClientID %>').val(entity.IDNote);
        $('#<%=txtDocumentNoteText.ClientID %>').val(entity.NoteText);
        $('#<%=txtDocumentNoteDate.ClientID %>').val(entity.NoteDate);

        $('#containerDocumentNotesEntryData').show();
    });

    function onDocumentNotesEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerDocumentNotesEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
    $("#<%=txtDocumentNoteText.ClientID %>").on("keypress", function (e) {
        var key = e.keyCode;

        // If the user has pressed enter
        if (e.keyCode == 13) {
            e.preventDefault();
            this.value = this.value.substring(0, this.selectionStart) + "" + "\n" + this.value.substring(this.selectionEnd, this.value.length);
            return false;
        }
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" id="hdnParamID" runat="server" />
    <input type="hidden" value="" id="hdnParamDocumentID" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Dokumen")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDocumentNo" Width="160px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerDocumentNotesEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnDocumentNoteID" runat="server" value="" />
                    <fieldset id="fsDocumentNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Tanggal ") %>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 1px; width: 145px">
                                                <asp:TextBox ID="txtDocumentNoteDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                    <asp:TextBox ID="txtDocumentNoteText" Width="100%" CssClass="required" runat="server"
                                        TextMode="Multiline" Rows="5" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnDocumentNotesSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnDocumentNotesCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpDocumentNotes" runat="server" Width="100%" ClientInstanceName="cbpDocumentNotes"
                    ShowLoadingPanel="false" OnCallback="cbpDocumentNotes_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onDocumentNotesEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlDocumentNotesGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdDocumentNotes" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="IDNote" />
                                                <input type="hidden" value="<%#:Eval("LogRemarks") %>" bindingfield="NoteText" />
                                                <input type="hidden" value="<%#:Eval("LogDateInDatePickerFormat") %>" bindingfield="NoteDate" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LogDateInString" HeaderText="Tanggal" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="LogRemarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingDocumentNote">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblDocumentNoteAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
