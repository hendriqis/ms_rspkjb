<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MealPlanDtItemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Nutrition.Program.MealPlanDtItemEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnMealID.ClientID %>').val('');
        $('#<%=txtMealCode.ClientID %>').val('');
        $('#<%=txtMealName.ClientID %>').val('');

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

    $('#btnImport').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('import');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnMealID.ClientID %>').val(entity.MealID);
        $('#<%=txtMealCode.ClientID %>').val(entity.MealCode);
        $('#<%=txtMealName.ClientID %>').val(entity.MealName);

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

    //#region MealPlanDtItem
    function onGetMealFilterExpression() {
        var filterExpression = "MealID NOT IN (SELECT MealID FROM MealPlanDtItem WHERE MealPlanDtID = " + $('#<%=hdnMealPlanDtID.ClientID %>').val() + " AND GCMealDay ='" + cboDay.GetValue() + "' AND ClassID = '" + cboClass.GetValue() + "' AND IsDeleted = 0) AND IsDeleted = 0";
        return filterExpression;
    }

    function onGetMealPlanCodeFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblMeal.lblLink').click(function () {
        openSearchDialog('meal', onGetMealFilterExpression(), function (value) {
            $('#<%=txtMealCode.ClientID %>').val(value);
            onTxtMealCodeChanged(value);
        });
    });
    $('#lblPanelMenuMakananTemplate.lblLink').click(function () {
        openSearchDialog('mealplan', onGetMealPlanCodeFilterExpression(), function (value) {
            $('#<%=txtMealPlanCodeTemplate.ClientID %>').val(value);
            onTxtMealPlanCodeTemplateChanged(value);
        });
    });

    $('#<%=txtMealCode.ClientID %>').change(function () {
        onTxtMealCodeChanged($(this).val());
    });

    function onTxtMealCodeChanged(value) {
        var filterExpression = onGetMealFilterExpression() + " AND MealCode = '" + value + "'";
        Methods.getObject('GetMealList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMealID.ClientID %>').val(result.MealID);
                $('#<%=txtMealName.ClientID %>').val(result.MealName);
            }
            else {
                $('#<%=hdnMealID.ClientID %>').val('');
                $('#<%=txtMealCode.ClientID %>').val('');
                $('#<%=txtMealName.ClientID %>').val('');
            }
        });
    }

    function onTxtMealPlanCodeTemplateChanged(value) {
        var filterExpression = onGetMealPlanCodeFilterExpression() + " AND MealPlanCode = '" + value + "'";
        Methods.getObject('GetMealPlanList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMealPlanID.ClientID %>').val(result.MealPlanID);
                $('#<%=txtMealPlanNameTemplate.ClientID %>').val(result.MealPlanName);
            }
            else {
                $('#<%=hdnMealPlanID.ClientID %>').val('');
                $('#<%=txtMealPlanCodeTemplate.ClientID %>').val('');
                $('#<%=txtMealPlanNameTemplate.ClientID %>').val('');
            }
        });
    }
    function GetMealPlanDtIDTemplate() {
        var filterExpression = "MealPlanCode = '" + $('#<%=txtMealPlanCodeTemplate.ClientID %>').val() + "' AND GCMealTime = '" + cboMealTimeTemplate.GetValue() + "' AND IsDeleted = 0";
        Methods.getObject('GetvMealPlanDtList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMealPlanDtIDTemplate.ClientID %>').val(result.MealPlanDtID);
                $('#<%=hdnMealPlanIDTemplate.ClientID %>').val(result.MealPlanID);
            }
        });

    }
    //#endregion
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnMealPlanDtID" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanDtIDTemplate" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanIDTemplate" value="" runat="server" />
    <input type="hidden" id="hdnMealPlanID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <tr>
                        <td style="vertical-align: top">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Panel Menu")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanCode" ReadOnly="true" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanName" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jadwal Makan")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMealTime" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="120px" />
                                                <col />
                                                <col />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kelas Rawat")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboClass" ClientInstanceName="cboClass" Width="100px">
                                                        <ClientSideEvents ValueChanged="function(s){ cbpEntryPopupView.PerformCallback('refresh'); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Hari ke-")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDay" ClientInstanceName="cboDay" Width="50px">
                                                        <ClientSideEvents ValueChanged="function(s){ cbpEntryPopupView.PerformCallback('refresh'); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="border: 1px solid black">
                            <table class="tblEntryContent" style="width: 100%">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col />
                                </colgroup>
                                <th>
                                    <label class="lblNormal" style="font-weight: bold">
                                        <%=GetLabel("SALIN DARI :")%></label>
                                </th>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPanelMenuMakananTemplate">
                                            <%=GetLabel("Panel Menu")%></label>
                                    </td>
                                    <td>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanCodeTemplate" ReadOnly="true" Width="100%" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMealPlanNameTemplate" ReadOnly="true" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblJadwalMakanTemplate">
                                            <%=GetLabel("Jadwal Makan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboMealTimeTemplate" ClientInstanceName="cboMealTimeTemplate"
                                            Width="100%">
                                            <ClientSideEvents ValueChanged="function(s){ GetMealPlanDtIDTemplate(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="120px" />
                                                <col />
                                                <col />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Kelas Rawat")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboClassTemplate" ClientInstanceName="cboClassTemplate"
                                                        Width="100px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Hari ke-")%></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ID="cboDayTemplate" ClientInstanceName="cboDayTemplate"
                                                        Width="50px">
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <center>
                                                        <br />
                                                        <input type="button" id="btnImport" value='<%= GetLabel("Proses Template")%>' style="min-width:110px; min-height:25px" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
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
                                <col style="width: 120px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblMeal">
                                        <%=GetLabel("Menu Makanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnMealID" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMealCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMealName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
                                    </center">
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
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
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
                                                <input type="hidden" value="<%#:Eval("MealPlanDtID") %>" bindingfield="MealPlanDtID" />
                                                <input type="hidden" value="<%#:Eval("ClassID") %>" bindingfield="ClassID" />
                                                <input type="hidden" value="<%#:Eval("MealID") %>" bindingfield="MealID" />
                                                <input type="hidden" value="<%#:Eval("MealCode") %>" bindingfield="MealCode" />
                                                <input type="hidden" value="<%#:Eval("MealName") %>" bindingfield="MealName" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MealCode" HeaderText="Kode Menu" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                        <asp:BoundField DataField="MealName" HeaderText="Nama Menu" ItemStyle-HorizontalAlign="Left" />
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
    <div style="width: 100%; text-align: center">
        <br />
        <input type="button" value='<%= GetLabel("Tutup Konfigurasi")%>' style="min-width:110px; min-height:25px" onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
