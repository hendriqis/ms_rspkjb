<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadTarifBookEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.UploadTarifBookEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_UploadTarifBookEntryCtlEntryCtl">
    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                displayMessageBox('Save Failed', 'Error Message : ' + param[2]);
                pcRightPanelContent.Hide();
            }
            else {
                displayMessageBox('SUCCESS', 'Insert Success');
                pcRightPanelContent.Hide();
                onRefreshGrid();
            }
        }
        else if (param[0] == 'upload') {
            if (param[1] == 'success') {
                cbpEntryPopupView.PerformCallback('refresh');
            }
            else {
                displayMessageBox('Upload Failed', 'Error Message : ' + param[2]);
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
        cbpEntryPopupView.PerformCallback('download');
    });

    $('#<%=TmpTariffDocumentUpload.ClientID %>').die('change');
    $('#<%=TmpTariffDocumentUpload.ClientID %>').live('change', function () {
        readURL(this);
    });

    function readURL(input) {
        var reader = new FileReader()
        reader.onload = function (e) {
            $('#<%=hdnTmpTariffUploadedFile.ClientID %>').val(e.target.result);
            if ($('#<%=hdnTmpTariffUploadedFile.ClientID %>').val() != "" && $('#<%=hdnTmpTariffUploadedFile.ClientID %>').val() != null) {
                cbpEntryPopupView.PerformCallback('upload');
            } else {
                displayErrorMessageBox('Upload Failed', 'Silahkan coba lagi.');
            }
        };
        reader.readAsDataURL(input.files[0]);
    }

    $('#chkAll').live('click', function () {
        $('input:checkbox').prop('checked', this.checked);
    });

    $('#btnSave').live('click', function () {
        var param = '';
        $('.chkIsSelectedID input:checked').each(function () {
            var keyID = $(this).closest('tr').find('.keyField').html();
            if (param != '')
                param += ',';
            param += keyID;
        });
        if (param == "") {
            showToast('Warning', 'Please Select Item Request First');
        }
        else {
            $('#<%=hdnSelectedListKey.ClientID %>').val(param);
            cbpEntryPopupView.PerformCallback('save');
        }
    });
</script>
<input type="hidden" id="hdnBookID" runat="server" />
<input type="hidden" id="hdnFileName" runat="server" />
<input type="hidden" id="hdnTmpTariffUploadedFile" runat="server" />
<input type="hidden" id="hdnSelectedListKey" runat="server" />
<table class="tblContentArea">
    <tr>
        <td>
            Upload File
        </td>
        <td>
            <asp:FileUpload ID="TmpTariffDocumentUpload" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <input type="button" value="Simpan" id="btnSave" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <label style="color: Red;">
                Note: Untuk File Csv , List Separator menggunakan symbol (;)
            </label>
        </td>
    </tr>
</table>
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <div style="height: 500px; overflow: scroll;">
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="KeyID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkAll" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelectedID" runat="server" CssClass="chkIsSelectedID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GCItemType" HeaderText="GCItemType" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemType" HeaderText="ItemType" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemID" HeaderText="ItemID" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="ItemName1" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="ClassID" HeaderText="ClassID" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ClassCode" HeaderText="ClassCode" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ClassName" HeaderText="ClassName" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ProposedTariffComp1" HeaderText="ProposedTariffComp1"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ProposedTariffComp2" HeaderText="ProposedTariffComp2"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ProposedTariffComp3" HeaderText="ProposedTariffComp3"
                                            HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </div>
        </td>
    </tr>
</table>