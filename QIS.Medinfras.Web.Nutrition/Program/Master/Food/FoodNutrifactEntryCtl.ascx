<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FoodNutrifactEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Nutrition.Program.FoodNutrifactEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnNutrientID.ClientID %>').val('');
        $('#<%=txtNutrientCode.ClientID %>').val('');
        $('#<%=txtNutrientName.ClientID %>').val('');
        $('#<%=txtNutrientAmount.ClientID %>').val('1');
        $('#<%=txtNotes.ClientID %>').val('');
        cboGCNutrientUnit.SetValue();

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
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnNutrientID.ClientID %>').val(entity.NutrientID);
        $('#<%=txtNutrientCode.ClientID %>').val(entity.NutrientCode);
        $('#<%=txtNutrientName.ClientID %>').val(entity.NutrientName);
        $('#<%=txtNutrientAmount.ClientID %>').val(entity.NutrientAmount);
        $('#<%=txtNotes.ClientID %>').val(entity.Remarks);
        cboGCNutrientUnit.SetValue(entity.GCNutrientUnit);

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
        hideLoadingPanel();
    }

    //#region Nutrient
    function onGetLocationNutrientFilterExpression() {
        var filterExpression = "<%:OnGetNutrientFilterExpression() %>";
        return filterExpression;
    }

    $('#lblNutrient.lblLink').click(function () {
        openSearchDialog('nutrient', onGetLocationNutrientFilterExpression(), function (value) {
            $('#<%=txtNutrientCode.ClientID %>').val(value);
            onTxtNutrientCodeChanged(value);
        });
    });

    $('#<%=txtNutrientCode.ClientID %>').change(function () {
        onTxtNutrientCodeChanged($(this).val());
    });

    function onTxtNutrientCodeChanged(value) {
        var filterExpression = onGetLocationNutrientFilterExpression() + " AND NutrientCode = '" + value + "'";
        Methods.getObject('GetNutrientList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnNutrientID.ClientID %>').val(result.NutrientID);
                $('#<%=txtNutrientName.ClientID %>').val(result.NutrientName);
                cboGCNutrientUnit.SetValue(result.GCNutrientUnit);
            }
            else {
                $('#<%=hdnNutrientID.ClientID %>').val('');
                $('#<%=txtNutrientCode.ClientID %>').val('');
                $('#<%=txtNutrientName.ClientID %>').val('');
                cboGCNutrientUnit.SetValue();
            }
        });
    }
    //#endregion
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Bahan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Bahan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <input type="hidden" runat="server" id="hdnIsAdd" />
                        <input type="hidden" runat="server" id="hdnID" />
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 40px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblNutrient">
                                        <%=GetLabel("Nutrisi")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" runat="server" id="hdnNutrientID" />
                                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNutrientCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNutrientName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jumlah Nutrisi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNutrientAmount" CssClass="number required" runat="server" Width="100px" />
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboGCNutrientUnit" ClientInstanceName="cboGCNutrientUnit" Width="130px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblLabel">
                                        <%=GetLabel("Keterangan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNotes" Width="60%" runat="server" TextMode="MultiLine" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" class="detailEntrySaveCancelButton" id="btnEntryPopupCancel"
                                                        value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" style="float: left; margin-left: 7px" />
                                                        </td>
                                                        <td style="width: 3px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("NutrientID") %>" bindingfield="NutrientID" />
                                                <input type="hidden" value="<%#:Eval("NutrientCode") %>" bindingfield="NutrientCode" />
                                                <input type="hidden" value="<%#:Eval("NutrientName") %>" bindingfield="NutrientName" />
                                                <input type="hidden" value="<%#:Eval("GCNutrientUnit") %>" bindingfield="GCNutrientUnit" />
                                                <input type="hidden" value="<%#:Eval("NutrientUnit") %>" bindingfield="NutrientUnit" />
                                                <input type="hidden" value="<%#:Eval("NutrientAmount") %>" bindingfield="NutrientAmount" />
                                                <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NutrientCode" HeaderText="Kode Nutrisi" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="NutrientName" HeaderText="Nama Nutrisi" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="400px" />
                                        <asp:BoundField DataField="CFNutrientAmount" HeaderText="Nilai Nutrisi" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
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
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
