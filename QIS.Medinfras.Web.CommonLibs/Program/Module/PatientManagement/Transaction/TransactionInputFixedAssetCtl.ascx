<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionInputFixedAssetCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionInputFixedAssetCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_TransactionInputFixedAssetCtl">
    $('#btnCancel').click(function () {
        $('#containerChargesDtInfoEntryData').hide();
    });

    $('#btnSave').click(function (evt) {
        cbpChargesDtInfoFixedAsset.PerformCallback('save');
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnChargesDtIDCtl2.ClientID %>').val(entity.ID);
        $('#<%=txtItemCodeCtl2.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemNameCtl2.ClientID %>').val(entity.ItemName1);
        $('#<%=hdnFixedAssetIDCtl2.ClientID %>').val(entity.FixedAssetID);
        $('#<%=txtFixedAssetCodeCtl2.ClientID %>').val(entity.FixedAssetCode);
        $('#<%=txtFixedAssetNameCtl2.ClientID %>').val(entity.FixedAssetName);

        $('#containerChargesDtInfoEntryData').show();
    });

    //#region Fixed Asset
    $('#lblFixedAsset.lblLink').live('click', function () {
        openSearchDialog('faitem', '', function (value) {
            $('#<%=txtFixedAssetCodeCtl2.ClientID %>').val(value);
            ontxtFixedAssetCodeCtl2Changed(value);
        });
    });

    $('#<%=txtFixedAssetCodeCtl2.ClientID %>').live('change', function () {
        ontxtFixedAssetCodeCtl2Changed($(this).val());
    });

    function ontxtFixedAssetCodeCtl2Changed(value) {
        var filterExpression = "IsDeleted = 0 AND FixedAssetCode = '" + value + "'";
        Methods.getObject('GetFAItemList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnFixedAssetIDCtl2.ClientID %>').val(result.FixedAssetID);
                $('#<%=txtFixedAssetNameCtl2.ClientID %>').val(result.FixedAssetName);
            }
            else {
                $('#<%=hdnFixedAssetIDCtl2.ClientID %>').val('');
                $('#<%=txtFixedAssetCodeCtl2.ClientID %>').val('');
                $('#<%=txtFixedAssetNameCtl2.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onEndCallbackChargesDtInfoFixedAssetEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerChargesDtInfoEntryData').hide();
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnTransactionIDCtl2" />
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
                        <td>
                            <asp:TextBox ID="txtTransactionNoCtl2" Width="170px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal - Jam Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionDateTimeCtl2" Width="170px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitNameCtl2" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerChargesDtInfoEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnChargesDtIDCtl2" runat="server" value="" />
                    <table class="tblEntryDetail" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Item Code")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCodeCtl2" Width="150px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Item Name")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemNameCtl2" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblFixedAsset">
                                    <%=GetLabel("Aset dan Inventaris")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnFixedAssetIDCtl2" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFixedAssetCodeCtl2" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFixedAssetNameCtl2" ReadOnly="true" Width="100%" runat="server" />
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
                                            <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' />
                                        </td>
                                        <td>
                                            <input type="button" id="btnCancel" value='<%= GetLabel("Batal")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpChargesDtInfoFixedAsset" runat="server" Width="100%"
                    ClientInstanceName="cbpChargesDtInfoFixedAsset" ShowLoadingPanel="false" OnCallback="cbpChargesDtInfoFixedAsset_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onEndCallbackChargesDtInfoFixedAssetEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlFixedAssetGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdChargesFixedAsset" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("FixedAssetID") %>" bindingfield="FixedAssetID" />
                                                <input type="hidden" value="<%#:Eval("FixedAssetCode") %>" bindingfield="FixedAssetCode" />
                                                <input type="hidden" value="<%#:Eval("FixedAssetName") %>" bindingfield="FixedAssetName" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="ItemCode" HeaderText="Item Code"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="FixedAssetCode" HeaderText="Fixed Asset Code"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="FixedAssetName" HeaderText="Fixed Asset Name" HeaderStyle-HorizontalAlign="Left" />
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
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
