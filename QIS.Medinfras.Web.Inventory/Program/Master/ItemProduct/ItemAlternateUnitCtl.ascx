<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemAlternateUnitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.ItemAlternateUnitCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ItemAlternateUnitCtl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val("");
        cboGCAlternateUnit.SetValue("");
        $('#<%=txtConversionFactor.ClientID %>').val("0");
        $('#<%=chkIsActive.ClientID %>').prop('checked', true);

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
        else {
            return false;
        }
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.ID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var GCAlternateUnit = $row.find('.GCAlternateUnit').val();
        var ConvertionFactor = $row.find('.ConversionFactor').val();
        var IsActive = $row.find('.IsActive').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        cboGCAlternateUnit.SetValue(GCAlternateUnit);
        $('#<%=txtConversionFactor.ClientID %>').val(ConvertionFactor);
        if (IsActive == 'True') {
            $('#<%=chkIsActive.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsActive.ClientID %>').prop('checked', false);
        }

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
        }
        hideLoadingPanel();
    }

</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnItemIDCtlAlternate" value="" runat="server" />
    <input type="hidden" id="hdnDrugDetailTypeCtlAlternate" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Item")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCodeCtlAlternate" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Item")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemNameCtlAlternate" ReadOnly="true" Width="100%" runat="server" />
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
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Satuan Alternatif")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboGCAlternateUnit" ClientInstanceName="cboGCAlternateUnit"
                                        runat="server" Width="120px">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Konversi")%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="numeric" Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=GetLabel("Aktif")%>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsActive" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="GCAlternateUnit" value="<%#: Eval("GCAlternateUnit")%>" />
                                                <input type="hidden" class="AlternateUnit" value="<%#: Eval("AlternateUnit")%>" />
                                                <input type="hidden" class="ConversionFactor" value="<%#: Eval("ConversionFactor")%>" />
                                                <input type="hidden" class="IsActive" value="<%#: Eval("IsActive")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AlternateUnit" HeaderStyle-Width="150px" HeaderText="Satuan Alternatif"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ConversionFactorInString2" HeaderStyle-Width="100px" HeaderText="Nilai Konversi"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="cfIsActiveInString" HeaderStyle-Width="50px" HeaderText="Status Aktif"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfCreatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diubah Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("LastUpdatedByName") %></div>
                                                <div>
                                                    <%#:Eval("cfLastUpdatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
