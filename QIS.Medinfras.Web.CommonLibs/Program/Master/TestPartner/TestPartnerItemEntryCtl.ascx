<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestPartnerItemEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestPartnerItemEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_TestPartnerItemEntryCtl">
    $(function () {
        setDatePicker('<%=txtStartDate.ClientID %>');
        $('#<%:txtStartDate.ClientID %>').datepicker('option', 'minDate', '-' + 0);

        $('#ulTabGrdDetail li').click(function () {
            $('#ulTabGrdDetail li.selected').removeAttr('class');
            $('.containerGrdDtTariff').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });

    });

    //#region Test Partner Item

    $('#lblAddDataItem').die('click');
    $('#lblAddDataItem').live('click', function () {
        $('#<%=hdnTestPartnerItemID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtPartnerItemCode.ClientID %>').val('');
        $('#<%=txtPartnerItemName.ClientID %>').val('');

        $('#containerPopupEntryDataItem').show();
    });

    $('#btnCancelTPItem').die('click');
    $('#btnCancelTPItem').live('click', function () {
        $('#containerPopupEntryDataItem').hide();
    });

    $('#btnSaveTPItem').die('click');
    $('#btnSaveTPItem').live('click', function () {
        var itemID = $('#<%=hdnItemID.ClientID %>').val();
        if (itemID != null && itemID != "" && itemID != "0") {
            cbpEntryPopupView.PerformCallback('save');
        } else {
            alert("Harap pilih Item Master terlebih dahulu.");
        }
    });

    $('.imgDeleteTPItem.imgLink').die('click');
    $('.imgDeleteTPItem.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnTestPartnerItemID').val();
            $('#<%=hdnTestPartnerItemID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEditTPItem.imgLink').die('click');
    $('.imgEditTPItem.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnTestPartnerItemID').val();
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();
        var itemName = $row.find('.hdnItemName1').val();
        var partnerItemCode = $row.find('.hdnPartnerItemCode').val();
        var partnerItemName = $row.find('.hdnPartnerItemName').val();

        $('#<%=hdnTestPartnerItemID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=txtPartnerItemCode.ClientID %>').val(partnerItemCode);
        $('#<%=txtPartnerItemName.ClientID %>').val(partnerItemName);

        $('#containerPopupEntryDataItem').show();
    });

    $('.imgExpand.imgLink').die('click');
    $('.imgExpand.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var ID = $row.find('.hdnTestPartnerItemID').val();
        $('#<%=hdnTestPartnerItemID.ClientID %>').val(ID);

        $trDetailTariff = $(this).closest('tr').next();
        if ($trDetailTariff.attr('class') != 'trDetailTariff') {
            $('#ulTabGrdDetail li:eq(0)').click();

            $trCollapse = $('.trDetailTariff');

            $row.find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
            $newTr = $("<tr><td></td><td></td><td colspan='5'></td></tr>").attr('class', 'trDetailTariff');
            $newTr.insertAfter($row);
            $newTr.find('td').last().append($('#containerGrdDetailTariff'));

            if ($trCollapse != null) {
                $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $trCollapse.remove();
            }

            $('.grdDetailTariff tr:gt(0)').remove();

            cbpViewDetailTariff.PerformCallback('refresh');
        }
        else {
            $row.find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
            $('#tempContainerGrdDetailTariff').append($('#containerGrdDetailTariff'));

            $('.grdDetailTariff tr:gt(0)').remove();

            $trDetailTariff.remove();
        }
    });

    function onCbpEntryPopupViewBeginCallback(s) {
        showLoadingPanel();
        $('#tempContainerGrdDetailTariff').append($('#containerGrdDetailTariff'));

        $('.grdDetailTariff tr:gt(0)').remove();
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblAddDataItem').click();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'refresh') {
        }
        else if (param[0] == 'saveTariff') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);

            cbpEntryPopupView.PerformCallback('refresh');
        }
        else if (param[0] == 'deleteTariff') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);

            cbpEntryPopupView.PerformCallback('refresh');
        }

        $('#containerPopupEntryDataItem').hide();
        $('#containerPopupEntryDataItemTariff').hide();

        hideLoadingPanel();
    }

    //#region Item
    function getItemMasterFilterExpression() {
        var businessPartnerID = $('#<%=hdnBusinessPartnerIDItemCtl.ClientID %>').val();
        var gcTestPartnerType = $('#<%=hdnGCTestPartnerTypeCtl.ClientID %>').val();
        var requestID = $('#<%=hdnRequestIDItemCtl.ClientID %>').val();

        var filterExpression = "ItemID NOT IN (SELECT ItemID FROM TestPartnerItem WITH(NOLOCK) WHERE BusinessPartnerID = " + businessPartnerID + " AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemStatus = 'X181^001'";

        if (requestID == "LB") {
            filterExpression += " AND GCItemType IN ('X001^004')";
        } else if (requestID == "IS") {
            filterExpression += " AND GCItemType IN ('X001^005')";
        } else {
            if (gcTestPartnerType == "X230^001") {
                filterExpression += " AND GCItemType IN ('X001^004')";
            } else if (gcTestPartnerType == "X230^002") {
                filterExpression += " AND GCItemType IN ('X001^005')";
            } else {
                filterExpression += " AND GCItemType IN ('')";
            }
        }

        return filterExpression;
    }

    $('#lblItem.lblLink').live('click', function () {
        var filterExpression = getItemMasterFilterExpression();
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtPartnerItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtPartnerItemCodeChanged($(this).val());
    });

    function onTxtPartnerItemCodeChanged(value) {
        var filterExpression = getItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=txtPartnerItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtPartnerItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=txtPartnerItemCode.ClientID %>').val('');
                $('#<%=txtPartnerItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion    

    //#endregion

    //#region Test Partner Item Tariff

    $('#lblAddDataItemTariff').die('click');
    $('#lblAddDataItemTariff').live('click', function () {
        $('#<%=hdnTestPartnerItemTariffID.ClientID %>').val("");
        $('#<%=txtStartDate.ClientID %>').val($('#<%=hdnDateTodayInPickerFormat.ClientID %>').val());
        $('#<%=txtAmount.ClientID %>').val("0");

        $('#containerPopupEntryDataItemTariff').show();
    });

    $('#btnCancelTPItemTariff').die('click');
    $('#btnCancelTPItemTariff').live('click', function () {
        $('#containerPopupEntryDataItemTariff').hide();
    });

    $('#btnSaveTPItemTariff').die('click');
    $('#btnSaveTPItemTariff').live('click', function () {
        cbpEntryPopupView.PerformCallback('saveTariff');
    });

    $('.imgDeleteTPItemTariff.imgLink').die('click');
    $('.imgDeleteTPItemTariff.imgLink').live('click', function (evt) {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var oID = $row.find('.oID').val();
            $('#<%=hdnTestPartnerItemTariffID.ClientID %>').val(oID);

            cbpEntryPopupView.PerformCallback('deleteTariff');
        }
    });

    $('.imgEditTPItemTariff.imgLink').die('click');
    $('.imgEditTPItemTariff.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var oID = $row.find('.oID').val();
        var oStartDate = $row.find('.oStartDateInDatePicker').val();
        var oAmount = $row.find('.oAmount').val();

        $('#<%=hdnTestPartnerItemTariffID.ClientID %>').val(oID);
        $('#<%=txtStartDate.ClientID %>').val(oStartDate);
        $('#<%=txtAmount.ClientID %>').val(oAmount);

        $('#containerPopupEntryDataItemTariff').show();
    });

    //#endregion

</script>
<div>
    <input type="hidden" value="" id="hdnRequestIDItemCtl" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerIDItemCtl" runat="server" />
    <input type="hidden" value="" id="hdnGCTestPartnerTypeCtl" runat="server" />
    <input type="hidden" value="" id="hdnDateTodayInPickerFormat" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="tempContainerGrdItem" style="position: relative;">
                    <table class="tblEntryContent" style="width: 80%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Test Partner")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPartnerCodeName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryDataItem" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnTestPartnerItemID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry Test Partner Item")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <input type="hidden" id="hdnItemID" runat="server" value="" />
                                        <label class="lblMandatory lblLink" id="lblItem">
                                            <%=GetLabel("Item Master")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Kode - Nama Item Partner")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPartnerItemCode" runat="server" Width="100%" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPartnerItemName" runat="server" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSaveTPItem" value='<%= GetLabel("Simpan")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancelTPItem" value='<%= GetLabel("Batal")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divTestPartnerItemList" style="height: 400px; overflow-y: auto; overflow-x: hidden">
                        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ onCbpEntryPopupViewBeginCallback(s); }"
                                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgEditTPItem imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        <img class="imgDeleteTPItem imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                        <input type="hidden" class="hdnTestPartnerItemID" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                        <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                        <input type="hidden" class="hdnItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" class="hdnPartnerItemCode" value="<%#: Eval("PartnerItemCode")%>" />
                                                        <input type="hidden" class="hdnPartnerItemName" value="<%#: Eval("PartnerItemName")%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                            alt='' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Item Master" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <label style="font-size: small; font-weight: bold">
                                                            <%#: Eval("ItemName1")%></label>
                                                        <br />
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%#: Eval("ItemCode") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Item Partner" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <label style="font-size: small;">
                                                            <%#: Eval("PartnerItemName")%></label>
                                                        <br />
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%#: Eval("PartnerItemCode") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Created Info" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <label style="font-size: x-small">
                                                            <%#: Eval("CreatedByName")%></label>
                                                        <br />
                                                        <label style="font-size: xx-small; font-style: italic">
                                                            <%#: Eval("cfCreatedDateInFullString") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="LastUpdated Info"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <label style="font-size: x-small">
                                                            <%#: Eval("LastUpdatedByName")%></label>
                                                        <br />
                                                        <label style="font-size: xx-small; font-style: italic">
                                                            <%#: Eval("cfLastUpdatedDateInFullString") %></label>
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
                    </div>
                    <div style="width: 100%; text-align: center" id="divAddDataItem" runat="server">
                        <span class="lblLink" id="lblAddDataItem">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </div>
                <div id="tempContainerGrdDetailTariff" style="display: none;">
                    <div id="containerGrdDetailTariff" class="borderBox" style="width: 100%; padding: 10px 5px; height: 200px; overflow-y: auto; overflow-x: hidden">
                        <div class="containerUlTabPage">
                            <ul class="ulTabPage" id="ulTabGrdDetail">
                                <li class="selected" contentid="containerDetailTariff">
                                    <%=GetLabel("Tariff") %></li>
                            </ul>
                        </div>
                        <div>
                            <div id="containerPopupEntryDataItemTariff" style="margin-top: 10px; display: none;">
                                <input type="hidden" id="hdnTestPartnerItemTariffID" runat="server" value="" />
                                <div class="pageTitle">
                                    <%=GetLabel("Entry Test Partner Item Tariff")%></div>
                                <fieldset id="fsEntryPopupTariff" style="margin: 0">
                                    <table class="tblEntryDetail" style="width: 50%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 150px" />
                                            <col style="width: 100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Start Date") %></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="120px" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory">
                                                    <%=GetLabel("Amount") %></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtAmount" CssClass="txtCurrency" Width="300px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnSaveTPItemTariff" value='<%= GetLabel("Simpan")%>' />
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnCancelTPItemTariff" value='<%= GetLabel("Batal")%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div id="containerDetailTariff" class="containerGrdDtTariff" style="width: 100%">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDetailTariff" runat="server" Width="100%" ClientInstanceName="cbpViewDetailTariff"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDetailTariff_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server" Width="100%">
                                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                position: relative; font-size: 0.95em;">
                                                <asp:GridView ID="grdDetailTariff" runat="server" CssClass="grdDetailTariff notAllowSelect"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <img class="imgEditTPItemTariff imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" style="float: left; margin-left: 7px" />
                                                                <img class="imgDeleteTPItemTariff imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                                <input type="hidden" class="oID" value="<%#: Eval("ID")%>">
                                                                <input type="hidden" class="oStartDate" value="<%#: Eval("StartDate")%>">
                                                                <input type="hidden" class="oStartDateInDatePicker" value="<%#: Eval("cfStartDateInDatePickerString")%>">
                                                                <input type="hidden" class="oAmount" value="<%#: Eval("Amount")%>">
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <label style="font-size: small; font-weight: bold">
                                                                    <%#: Eval("cfStartDateInString")%></label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Tariff" HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <label style="font-size: small;">
                                                                    <%#: Eval("Amount", "{0:N2}")%></label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Created Info" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="170px">
                                                            <ItemTemplate>
                                                                <label style="font-size: x-small">
                                                                    <%#: Eval("CreatedByName")%></label>
                                                                <br />
                                                                <label style="font-size: xx-small; font-style: italic">
                                                                    <%#: Eval("cfCreatedDateInFullString") %></label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="LastUpdated Info"
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="170px">
                                                            <ItemTemplate>
                                                                <label style="font-size: x-small">
                                                                    <%#: Eval("LastUpdatedByName")%></label>
                                                                <br />
                                                                <label style="font-size: xx-small; font-style: italic">
                                                                    <%#: Eval("cfLastUpdatedDateInFullString") %></label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Data Tidak Tersedia")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <div class="imgLoadingGrdView" id="Div1">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div style="width: 100%; text-align: center" id="divAddDataItemTariff" runat="server">
                                                    <span class="lblLink" id="lblAddDataItemTariff">
                                                        <%= GetLabel("Tambah Data Tariff")%></span>
                                                </div>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
