<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrinterLocationIPSettingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.PrinterLocationIPSettingCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_settingipprintergroupctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtIPAddress.ClientID %>').val('');

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
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        $('#<%=hdnID.ClientID %>').val(ID);

        var ipAddress = $row.find('.hdnIPAddress').val();
        $('#<%=txtIPAddress.ClientID %>').val(ipAddress);

        $('#containerPopupEntryData').show();
    });

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
        $('#containerImgLoadingView').hide();
    }
</script>
<div style="height: 420px; overflow-y: auto">
    <input type="hidden" id="hdnGCPrinterGroup" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Printer Group")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Printer Group")%></label>
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtPrinterGroupCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrinterGroupName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 10px" />
                                <col style="width: 150px" />
                                <col style="width: 50%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("IP Address")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIPAddress" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" style="width: 80px" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                    <input type="button" style="width: 80px" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlSettingIPAddressGroup" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="hdnIPAddress" value="<%#: Eval("IPAddress")%>" />
                                                    <input type="hidden" class="hdnCreatedByUserName" value="<%#: Eval("CreatedByUserName")%>" />
                                                    <input type="hidden" class="hdnLastUpdatedByUserName" value="<%#: Eval("LastUpdatedByUserName")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IPAddress" HeaderText="IP Address" ItemStyle-CssClass="tdIPAddress" />
                                            <asp:BoundField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                DataField="CreatedByUserName" HeaderText="Created By" ItemStyle-CssClass="tdCreatedByUserName" />
                                            <asp:BoundField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                DataField="LastUpdatedByUserName" HeaderText="Updated By" ItemStyle-CssClass="tdLastUpdatedByUserName" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
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
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="onRefreshControl();pcRightPanelContent.Hide();" />
    </div>
</div>
