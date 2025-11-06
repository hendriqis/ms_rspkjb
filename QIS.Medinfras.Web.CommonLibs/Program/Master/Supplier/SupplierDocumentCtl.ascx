<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierDocumentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.SupplierDocumentCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_custdocctl">
    $(function () {
        setDatePicker('<%=txtDocumentDate.ClientID %>');
        $('#<%=txtDocumentDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val();

        $('#<%=txtUploadFile.ClientID %>').val(fileName);

        $('#<%=txtFileName1.ClientID %>').val(fileName);
        $('#<%=txtRename1.ClientID %>').val(fileName);
        if ($('#previewImage').width() > $('#previewImage').height())
            $('#previewImage').width('150px');
        else
            $('#previewImage').height('150px');
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                $('#previewImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
        else
            $('#previewImage').attr('src', input.value);
    }

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

    $('#btnUploadFile').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtDocumentDate.ClientID %>').removeAttr('readonly');
        $('#<%=txtNotes.ClientID %>').removeAttr('readonly');

        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#<%=hdnUploadedFile1.ClientID %>').val('');

        cboGCDocumentType.SetValue('');
        $('#<%=txtDocumentNo.ClientID %>').val('');
        $('#<%=txtRename1.ClientID %>').val('');
        $('#<%=txtUploadFile.ClientID %>').val('');
        $('#<%=txtNotes.ClientID %>').val('');
        $('#previewImage').removeAttr('src');
        $('#<%=txtFileName1.ClientID %>').val('');

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

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=txtNotes.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtDocumentDate.ClientID %>').attr('readonly', 'readonly');
        $('#<%=cboGCDocumentType.ClientID %>').attr('disabled', 'true');

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $('#<%=hdnUploadedFile1.ClientID %>').val();

        $('#<%=txtDocumentDate.ClientID %>').val($.datepicker.formatDate('dd-mm-yy', new Date(entity.CreatedDate)));
        cboGCDocumentType.SetValue(entity.GCDocumentType);
        $('#<%=txtDocumentNo.ClientID %>').val(entity.DocumentNo);
        $('#<%=txtUploadFile.ClientID %>').val(entity.FileName);
        $('#<%=txtRename1.ClientID %>').val(entity.FileName);
        $('#<%=txtNotes.ClientID %>').val(entity.Remarks);
        $('#<%=txtFileName1.ClientID %>').val(entity.FileName);

        var path = $('#<%=hdnVirtualDirectory.ClientID %>').val() + entity.FileName;
        $("#previewImage").attr("src", path);

        $('#containerPopupEntryData').show();

    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });
   
</script>
<div style="height: 525px; overflow-y: auto">
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" id="hdnContractID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <input type="hidden" id="hdnVirtualDirectory" value="" runat="server" />
    <input type="hidden" id="hdnSupplierID" value="" runat="server" />
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
                                <%=GetLabel("Nama Pemasok")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" Width="250px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Document Date") %></label>
                                </td>
                                <td style="padding-left: 1px">
                                    <asp:TextBox ID="txtDocumentDate" runat="server" Width="120px" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Document Type") %></label>
                                </td>
                                <td>
                                    <dx:ASPxComboBox runat="server" ID="cboGCDocumentType" ClientInstanceName="cboGCDocumentType"
                                        Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Document No.") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDocumentNo" CssClass="required" Width="295px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("File to upload") %></label>
                                </td>
                                <td style="padding-left: 1px">
                                    <asp:TextBox ID="txtUploadFile" Width="61%" runat="server" ReadOnly="true" />
                                    <input type="button" id="btnUploadFile" value="Upload" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Rename File To") %></label>
                                </td>
                                <td style="padding-left: 1px">
                                    <asp:TextBox ID="txtRename1" Width="295px" runat="server" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="tdLabel" style="padding-top: 5px" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Notes") %></label>
                                </td>
                                <td style="padding-left: 1px" colspan="2">
                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                            </td>
                                            <td rowspan="4" style="height: 150px; width: 150px; border: 1px solid ActiveBorder;">
                                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                                <img src="" id="previewImage" width="150px" height="150px" alt="" />
                                                <div style="display: none">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                </div>
                                            </td>
                                            <td style="vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFileName1" Width="100%" runat="server" ReadOnly="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%=GetLabel("Allowed extension") %>
                                                            : jpg,jpeg,png,txt,doc,docx,pdf,mp4
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%=GetLabel("Maximum upload size") %>
                                                            : 10MB
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
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
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEdit imgLink" src='<%#ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td style="width: 3px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDelete imgLink" src='<%#ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("SupplierID") %>" bindingfield="SupplierID" />
                                                <input type="hidden" value="<%#:Eval("CreatedDate") %>" bindingfield="CreatedDate" />
                                                <input type="hidden" value="<%#:Eval("DocumentNo") %>" bindingfield="DocumentNo" />
                                                <input type="hidden" value="<%#:Eval("FileName") %>" bindingfield="FileName" />
                                                <input type="hidden" value="<%#:Eval("GCDocumentType") %>" bindingfield="GCDocumentType" />
                                                <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FileName" HeaderText="Nama File" ItemStyle-CssClass="tdFileName"
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
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
