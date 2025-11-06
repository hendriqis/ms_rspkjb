<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitAutoBillItemParamedicEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitAutoBillItemParamedicEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ServiceUnitAutoBillItemParamedicEntryCtl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').removeAttr('readonly');
        $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').removeAttr('readonly');
        
        $('#<%=hdnAutoBillItemID.ClientID %>').val('');
        $('#<%=hdnItemIDAutoBillParamedic.ClientID %>').val('');
        $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').val('');
        $('#<%=txtItemNameAutoBillParamedic.ClientID %>').val('');
        $('#<%=hdnParamedicIDAutoBillParamedic.ClientID %>').val('');
        $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').val('');
        $('#<%=txtParamedicNameAutoBillParamedic.ClientID %>').val('');
        $('#<%=txtQty.ClientID %>').val('1');
        $('#<%=chkIsAutoPayment.ClientID %>').prop('checked', false);
        $('#<%=chkIsIsAdministrationItem.ClientID %>').prop('checked', false);

        $('#lblItem').attr('class', 'lblLink lblMandatory');
        $('#lblParamedic').attr('class', 'lblLink lblMandatory');

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
            var autoBillItemID = $row.find('.hdnAutoBillItemID').val();
            $('#<%=hdnAutoBillItemID.ClientID %>').val(autoBillItemID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var autoBillItemID = $row.find('.hdnAutoBillItemID').val();
        var itemID = $row.find('.hdnItemIDAutoBillParamedic').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName = $row.find('.hdnItemName1').val();
        var paramedicID = $row.find('.hdnParamedicID').val();
        var paramedicCode = $row.find('.hdnParamedicCode').val();
        var paramedicName = $row.find('.hdnParamedicName').val();

        var autoPaymentChecked = $row.find('.tdIsAutoPayment').find('input').attr('checked');
        var isAutoPayment = false;
        if (autoPaymentChecked != null && autoPaymentChecked != '')
            isAutoPayment = true;

        var administrationItemChecked = $row.find('.tdIsAdministrationItem').find('input').attr('checked');
        var isAdministrationItem = false;
        if (administrationItemChecked != null && administrationItemChecked != '')
            isAdministrationItem = true;

        var quantity = $row.find('.hdnQuantity').val();

        $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').attr('readonly', 'readonly');

        $('#<%=hdnAutoBillItemID.ClientID %>').val(autoBillItemID);
        $('#<%=hdnItemIDAutoBillParamedic.ClientID %>').val(itemID);
        $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').val(itemCode);
        $('#<%=txtItemNameAutoBillParamedic.ClientID %>').val(itemName);
        $('#<%=hdnParamedicIDAutoBillParamedic.ClientID %>').val(paramedicID);
        $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').val(paramedicCode);
        $('#<%=txtParamedicNameAutoBillParamedic.ClientID %>').val(paramedicName);
        $('#<%=txtQty.ClientID %>').val(quantity);
        $('#<%=chkIsAutoPayment.ClientID %>').prop('checked', isAutoPayment);
        $('#<%=chkIsIsAdministrationItem.ClientID %>').prop('checked', isAdministrationItem);

        $('#lblItem.lblLink').attr('class', 'lblDisabled');
        $('#lblParamedic.lblLink').attr('class', 'lblDisabled');

        $('#containerPopupEntryData').show();
    });

    //#region Paramedic
    $('#lblParamedic.lblLink').live('click', function () {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDAutoBillParamedic.ClientID %>').val();
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ")";
        openSearchDialog('physician', filterExpression, function (value) {
            $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').val(value);
            ontxtParamedicCodeAutoBillParamedicChanged(value);
        });
    });

    $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').live('change', function () {
        ontxtParamedicCodeAutoBillParamedicChanged($(this).val());
    });

    function ontxtParamedicCodeAutoBillParamedicChanged(value) {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDAutoBillParamedic.ClientID %>').val();
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ")";

        filterExpression += " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicIDAutoBillParamedic.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicNameAutoBillParamedic.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnParamedicIDAutoBillParamedic.ClientID %>').val('');
                $('#<%=txtParamedicCodeAutoBillParamedic.ClientID %>').val('');
                $('#<%=txtParamedicNameAutoBillParamedic.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Item
    $('#lblItem.lblLink').live('click', function () {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDAutoBillParamedic.ClientID %>').val();
        var filterExpression = "ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND ItemID NOT IN (SELECT ItemID FROM ServiceUnitAutoBillItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0)";

        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').val(value);
            ontxtItemCodeAutoBillParamedicChanged(value);
        });
    });

    $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').live('change', function () {
        ontxtItemCodeAutoBillParamedicChanged($(this).val());
    });

    function ontxtItemCodeAutoBillParamedicChanged(value) {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDAutoBillParamedic.ClientID %>').val();
        var filterExpression = "ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND ItemID NOT IN (SELECT ItemID FROM ServiceUnitAutoBillItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0)";

        filterExpression += " AND ItemCode = '" + value + "'";

        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemIDAutoBillParamedic.ClientID %>').val(result.ItemID);
                $('#<%=txtItemNameAutoBillParamedic.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemIDAutoBillParamedic.ClientID %>').val('');
                $('#<%=txtItemCodeAutoBillParamedic.ClientID %>').val('');
                $('#<%=txtItemNameAutoBillParamedic.ClientID %>').val('');
            }
        });
    }
    //#endregion

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
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareServiceUnitIDAutoBillParamedic" value="" runat="server" />
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
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnAutoBillItemID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 160px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblParamedic">
                                        <%=GetLabel("Dokter")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnParamedicIDAutoBillParamedic" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtParamedicCodeAutoBillParamedic" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtParamedicNameAutoBillParamedic" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnItemIDAutoBillParamedic" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCodeAutoBillParamedic" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemNameAutoBillParamedic" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jumlah")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQty" CssClass="required number" Width="100px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsAutoPayment" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <%=GetLabel("Pembayaran Otomatis")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsIsAdministrationItem" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <%=GetLabel("Biaya Administrasi")%>
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
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnAutoBillItemID" value="<%#: Eval("AutoBillItemParamedicID")%>" />
                                                <input type="hidden" class="hdnItemIDAutoBillParamedic" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="hdnItemName1" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="hdnParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="hdnParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" class="hdnParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                <input type="hidden" class="hdnQuantity" value="<%#: Eval("Quantity")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%:GetLabel("Item")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                    <%#: Eval("ItemName1")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller;">
                                                    <%#: Eval("ItemCode")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller; font-weight: bold"">
                                                    <%#: Eval("ParamedicName")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%:GetLabel("Quantity")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                    <%#: Eval("Quantity")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller;">
                                                    <%#: Eval("ItemUnit")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CheckBoxField HeaderStyle-Width="120px" DataField="IsAutoPayment" HeaderText="Is Auto Payment"
                                            ItemStyle-CssClass="tdIsAutoPayment" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField HeaderStyle-Width="150px" DataField="IsAdministrationItem" HeaderText="Is Administration Item"
                                            ItemStyle-CssClass="tdIsAdministrationItem" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%:GetLabel("Last Updated Information")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                    <%#: Eval("LastUpdatedByName")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller;">
                                                    <%#: Eval("cfLastUpdatedDateInStringFullFormat")%></label>
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
