<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitAutoBillItemDailyEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitAutoBillItemDailyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ServiceUnitAutoBillItemDailyEntryCtl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtItemCodeAutoDaily.ClientID %>').removeAttr('readonly');

        $('#<%=hdnIDAutoDaily.ClientID %>').val('');
        $('#<%=hdnItemIDAutoDaily.ClientID %>').val('');
        $('#<%=txtItemCodeAutoDaily.ClientID %>').val('');
        $('#<%=txtItemNameAutoDaily.ClientID %>').val('');
        $('#<%=txtQtyAutoDaily.ClientID %>').val('1');
        $('#lblItem').attr('class', 'lblLink lblMandatory');

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
            var autoBillItemID = $row.find('.ID').val();
            $('#<%=hdnIDAutoDaily.ClientID %>').val(autoBillItemID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var autoBillItemID = $row.find('.ID').val();
        var itemID = $row.find('.ItemID').val();
        var itemCode = $row.find('.ItemCode').val();
        var itemName = $row.find('.ItemName1').val();
        var quantity = $row.find('.Quantity').val();

        $('#<%=txtItemCodeAutoDaily.ClientID %>').attr('readonly', 'readonly');

        $('#<%=hdnIDAutoDaily.ClientID %>').val(autoBillItemID);
        $('#<%=hdnItemIDAutoDaily.ClientID %>').val(itemID);
        $('#<%=txtItemCodeAutoDaily.ClientID %>').val(itemCode);
        $('#<%=txtItemNameAutoDaily.ClientID %>').val(itemName);
        $('#<%=txtQtyAutoDaily.ClientID %>').val(quantity);
        
        $('#lblItem.lblLink').attr('class', 'lblDisabled');

        $('#containerPopupEntryData').show();
    });

    //#region Item
    $('#lblItem.lblLink').live('click', function () {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDCtlAutoDaily.ClientID %>').val();
        var filterExpression = "ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND ItemID NOT IN (SELECT ItemID FROM ServiceUnitAutoBillItemAdditionalDaily WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0)";
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCodeAutoDaily.ClientID %>').val(value);
            ontxtItemCodeAutoDailyChanged(value);
        });
    });

    $('#<%=txtItemCodeAutoDaily.ClientID %>').live('change', function () {
        ontxtItemCodeAutoDailyChanged($(this).val());
    });

    function ontxtItemCodeAutoDailyChanged(value) {
        var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitIDCtlAutoDaily.ClientID %>').val();
        var filterExpression = "ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND ItemID NOT IN (SELECT ItemID FROM ServiceUnitAutoBillItemAdditionalDaily WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + " AND IsDeleted = 0)";
        filterExpression += " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemIDAutoDaily.ClientID %>').val(result.ItemID);
                $('#<%=txtItemNameAutoDaily.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemIDAutoDaily.ClientID %>').val('');
                $('#<%=txtItemCodeAutoDaily.ClientID %>').val('');
                $('#<%=txtItemNameAutoDaily.ClientID %>').val('');
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
    <input type="hidden" id="hdnHealthcareServiceUnitIDCtlAutoDaily" value="" runat="server" />
    <input type="hidden" id="hdnIDAutoDaily" value="" runat="server" />
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
                                    <label class="lblLink lblMandatory" id="lblItem">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnItemIDAutoDaily" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCodeAutoDaily" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemNameAutoDaily" ReadOnly="true" Width="100%" runat="server" />
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
                                    <asp:TextBox ID="txtQtyAutoDaily" CssClass="required number" Width="100px" runat="server" />
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
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="ItemCode" value="<%#: Eval("ItemCode")%>" />
                                                <input type="hidden" class="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                <input type="hidden" class="Quantity" value="<%#: Eval("Quantity")%>" />
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
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
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
                                        <asp:TemplateField HeaderStyle-Width="230px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <%:GetLabel("Created Information")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-size: small; font-weight: bold">
                                                    <%#: Eval("CreatedByName")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-size: smaller;">
                                                    <%#: Eval("cfCreatedDateInStringFullFormat")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
